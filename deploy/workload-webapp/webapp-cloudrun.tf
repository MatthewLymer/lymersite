locals {
  cloud_run = {
    name     = "webapp-service"
    location = "us-central1"
  }
}

data "google_cloud_run_service" "default" {
  project  = local.project.id
  name     = local.cloud_run.name
  location = local.cloud_run.location
}

resource "google_cloud_run_service" "default" {
  project  = local.project.id
  name     = local.cloud_run.name
  location = local.cloud_run.location

  autogenerate_revision_name = true

  depends_on = [
    google_project_service.run
  ]

  template {
    spec {
      timeout_seconds = 30

      containers {
        image = coalesce(var.webapp_image, data.google_cloud_run_service.default.template[0].spec[0].containers[0].image)
        ports {
          container_port = 80
        }
        resources {
          limits = {
            cpu    = "1000m"
            memory = "128Mi"
          }
        }
      }
    }

    metadata {
      annotations = {
        "autoscaling.knative.dev/maxScale" = 3
      }
    }
  }

  traffic {
    percent         = 100
    latest_revision = true
  }
}

data "google_iam_policy" "noauth" {
  binding {
    role = "roles/run.invoker"
    members = [
      "allUsers",
    ]
  }
}

resource "google_cloud_run_service_iam_policy" "noauth" {
  location    = google_cloud_run_service.default.location
  project     = google_cloud_run_service.default.project
  service     = google_cloud_run_service.default.name
  policy_data = data.google_iam_policy.noauth.policy_data
}

resource "google_cloud_run_domain_mapping" "default" {
  location = "us-central1"
  name     = local.public_domain_name

  metadata {
    namespace = local.project.id
  }

  spec {
    route_name = google_cloud_run_service.default.name
  }
}

resource "google_cloud_run_domain_mapping" "www" {
  location = "us-central1"
  name     = "www.${local.public_domain_name}"

  metadata {
    namespace = local.project.id
  }

  spec {
    route_name = google_cloud_run_service.default.name
  }
}

output "url" {
  value = google_cloud_run_service.default.status[0].url
}
