resource "google_monitoring_uptime_check_config" "https" {
  depends_on = [
    google_project_service.stackdriver
  ]

  project = local.project.id

  display_name = "https-uptime-check"
  timeout      = "60s"

  http_check {
    path         = local.uptime_check.path
    port         = "443"
    use_ssl      = true
    validate_ssl = true
  }

  period = "900s"

  monitored_resource {
    type = "uptime_url"
    labels = {
      project_id = local.project.id
      host       = local.public_domain_name
    }
  }

  content_matchers {
    content = local.uptime_check.content
  }

  selected_regions = [
    "USA"
  ]
}

resource "google_monitoring_alert_policy" "uptime_policy" {
  project = local.project.id

  display_name = "WebApp Uptime Check"
  combiner     = "OR"

  enabled = true

  notification_channels = [
    data.google_monitoring_notification_channel.email.id
  ]

  conditions {
    display_name = "Uptime Check URL - Check passed"

    condition_threshold {
      filter = <<-EOF
        resource.type = "uptime_url"
        metric.type = "monitoring.googleapis.com/uptime_check/check_passed"
        metric.labels.check_id = "${google_monitoring_uptime_check_config.https.uptime_check_id}"
      EOF

      aggregations {
        alignment_period     = "1800s"
        cross_series_reducer = "REDUCE_MEAN"
        per_series_aligner   = "ALIGN_FRACTION_TRUE"
      }

      comparison = "COMPARISON_LT"

      duration = "0s"

      threshold_value = 0.5

      trigger {
        count = 1
      }
    }
  }
}
