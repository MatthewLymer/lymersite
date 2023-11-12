resource "google_artifact_registry_repository" "default" {
  location      = "us-central1"
  repository_id = "default"
  format        = "DOCKER"
}
