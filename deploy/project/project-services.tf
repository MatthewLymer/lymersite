resource "google_project_service" "container" {
  project = google_project.default.id
  service = "container.googleapis.com"
}
