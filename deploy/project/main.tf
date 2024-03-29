locals {
  region          = "northamerica-northeast1"
  billing_account = "017629-BB9381-1C2281"

  project = {
    id   = "matthewlymer-lymersite"
    name = "LymerSite"
  }

  github = {
    repository = "lymersite"
  }
}

variable "github_token" {
  type        = string
  description = "The fine grained personal access token"
}

variable "alerting_email" {
  type        = string
  description = "The email address to send alerts to"
}

provider "google" {
  region  = local.region
  project = local.project.id
}

provider "github" {
  token = var.github_token
}

resource "google_project" "default" {
  project_id      = local.project.id
  name            = local.project.name
  billing_account = local.billing_account
}

terraform {
  backend "gcs" {
    bucket = "490635812867-tfstate" # TODO: this should probably be in a different bucket for security reasons
    prefix = "matthewlymer-lymersite-project"
  }

  required_providers {
    github = {
      source = "integrations/github"
    }
  }
}
