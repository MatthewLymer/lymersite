locals {
  region = "northamerica-northeast1"

  project = {
    id = "matthewlymer-lymersite"
  }

  public_domain_name = "lymer.ca"

  uptime_check = {
    path    = "/"
    content = "Matthew Lymer"
  }
}

variable "webapp_image" {
  type = string
}

provider "google" {
  region  = local.region
  project = local.project.id
}

terraform {
  backend "gcs" {
    bucket = "490635812867-tfstate"
    prefix = "matthewlymer-lymersite"
  }
}
