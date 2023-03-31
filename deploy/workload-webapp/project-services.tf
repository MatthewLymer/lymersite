resource "google_project_service" "run" {
  project            = local.project.id
  service            = "run.googleapis.com"
  disable_on_destroy = false
}

resource "google_project_service" "stackdriver" {
  project            = local.project.id
  service            = "stackdriver.googleapis.com"
  disable_on_destroy = false
}
