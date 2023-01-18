resource "google_service_account" "github_actions_deployer" {
  project      = local.project.id
  account_id   = "github-actions-deployer"
  display_name = "GitHub Actions deployer"
}

resource "google_storage_bucket_iam_member" "github_actions_deployer" {
  bucket = "artifacts.${local.project.id}.appspot.com"
  role   = "roles/storage.objectAdmin"
  member = "serviceAccount:${google_service_account.github_actions_deployer.email}"
}

resource "google_service_account_key" "github_actions_deployer" {
  service_account_id = google_service_account.github_actions_deployer.name
}

resource "github_actions_secret" "github_actions_deployer_key" {
  repository      = local.github.repository
  secret_name     = "GCP_DEPLOYER_SA"
  plaintext_value = base64decode(google_service_account_key.github_actions_deployer.private_key)
}