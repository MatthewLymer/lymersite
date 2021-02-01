# [Lymer.ca](https://lymer.ca)

The personal website of Matthew Lymer


# Prerequisites

* Docker
* Google Cloud Platform Account


# Running the application

```
docker-compose up
```

Then browse to http://localhost:5000.


# Hosting

The project is hosted on [Google Cloud Platform](https://console.cloud.google.com "Google Cloud Platform") using [Cloud Run](https://cloud.google.com/run).
This was chosen for the ease of deployment and relatively low cost.

[Terraform](https://www.terraform.io) is used to make infrastructure changes.

[Docker](https://www.docker.com) is used to contain the application and for ease of dependency management in the hosting environment.


# How to deploy

## Terraform

Start a terraform docker container

```
docker run -it -e "GOOGLE_APPLICATION_CREDENTIALS=/root/.config/gcloud/legacy_credentials/PUT-ACCOUNT-NAME-HERE/adc.json" -v ${HOME}/.config/gcloud:/root/.config/gcloud -w /data/infrastructure -v ${PWD}:/data --entrypoint /bin/sh hashicorp/terraform
```

Then from within the shell, you can initialize terraform and apply the changes

```
terraform init
terraform apply
```

If this is the first time you've run terraform for this project, you may receive "access denied" type errors, since adding permissions is not instantaneous (or maybe an order of operations issues, not sure).

Once terraform has configured the project properly you can now upload the docker image.

## Build and Deploy docker image

```
docker run -it --rm -v ${HOME}/.config/gcloud:/root/.config/gcloud -w /data -v ${PWD}:/data -v /var/run/docker.sock:/var/run/docker.sock google/cloud-sdk /bin/bash "-c" "make push-docker"
```