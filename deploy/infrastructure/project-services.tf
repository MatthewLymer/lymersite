resource "google_project_service" "container" {
  service = "container.googleapis.com"

  depends_on = [
    google_project.default
  ]
}

resource "google_project_service" "run" {
  service = "run.googleapis.com"

  depends_on = [
    google_project.default
  ]
}

resource "google_project_service" "stackdriver" {
  service = "stackdriver.googleapis.com"

  depends_on = [
    google_project.default
  ]
}