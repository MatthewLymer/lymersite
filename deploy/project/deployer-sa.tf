resource "google_service_account" "github_actions_deployer" {
  project      = local.project.id
  account_id   = "github-actions-deployer"
  display_name = "GitHub Actions deployer"
}

resource "google_storage_bucket_iam_member" "github_actions_deployer_tfstate_admin" {
  bucket = "490635812867-tfstate"
  role   = "roles/storage.objectAdmin"
  member = "serviceAccount:${google_service_account.github_actions_deployer.email}"
}

resource "google_service_account_key" "github_actions_deployer" {
  service_account_id = google_service_account.github_actions_deployer.name
}

resource "google_artifact_registry_repository_iam_member" "github_actions_deployer_default_writer" {
  project    = local.project.id
  location   = google_artifact_registry_repository.default.location
  repository = google_artifact_registry_repository.default.name
  role       = "roles/artifactregistry.writer"
  member     = "serviceAccount:${google_service_account.github_actions_deployer.email}"
}
