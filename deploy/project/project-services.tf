resource "google_project_service" "artifactregistry" {
  project = google_project.default.id
  service = "artifactregistry.googleapis.com"
}
