data "google_monitoring_notification_channel" "email" {
  project = local.project.id

  display_name = "Email alerting channel"
}

