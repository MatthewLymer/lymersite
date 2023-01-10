locals {
  region          = "northamerica-northeast1"
  billing_account = "017629-BB9381-1C2281"

  project = {
    id   = "matthewlymer-lymersite"
    name = "LymerSite"
  }
}

provider "google" {
  region  = local.region
  project = local.project.id
}

resource "google_project" "default" {
  project_id      = local.project.id
  name            = local.project.name
  billing_account = local.billing_account
}

terraform {
  backend "gcs" {
    bucket = "490635812867-tfstate"
    prefix = "matthewlymer-lymersite-project"
  }
}