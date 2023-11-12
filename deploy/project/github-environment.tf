# TODO: protect branch

resource "github_repository_environment" "production" {
  repository  = local.github.repository
  environment = "production"
}

resource "github_actions_environment_secret" "github_actions_deployer_key" {
  repository      = local.github.repository
  environment     = github_repository_environment.production.environment
  secret_name     = "GCP_DEPLOYER_SA"
  plaintext_value = base64decode(google_service_account_key.github_actions_deployer.private_key)
}
