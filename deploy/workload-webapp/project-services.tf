resource "google_project_service" "run" {
  service            = "run.googleapis.com"
  disable_on_destroy = false
}

resource "google_project_service" "stackdriver" {
  service            = "stackdriver.googleapis.com"
  disable_on_destroy = false
}