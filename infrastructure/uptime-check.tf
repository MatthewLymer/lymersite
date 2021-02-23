resource "google_monitoring_uptime_check_config" "https" {

  depends_on = [
    google_project_service.stackdriver
  ]

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