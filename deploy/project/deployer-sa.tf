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

resource "google_storage_bucket_iam_member" "github_actions_deployer_gcr_admin" {
  bucket = "artifacts.${local.project.id}.appspot.com"
  role   = "roles/storage.objectAdmin"
  member = "serviceAccount:${google_service_account.github_actions_deployer.email}"
}

resource "google_service_account_key" "github_actions_deployer" {
  service_account_id = google_service_account.github_actions_deployer.name
}
