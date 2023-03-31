resource "google_monitoring_notification_channel" "email" {
  project = local.project.id

  display_name = "Email alerting channel"
  type         = "email"
  labels = {
    email_address = var.alerting_email
  }
}
