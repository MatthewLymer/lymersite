resource "google_project_service" "container" {
  service = "container.googleapis.com"
}

resource "google_project_service" "run" {
  service = "run.googleapis.com"
}