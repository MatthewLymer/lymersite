DOCKER_TAG:=$(shell git rev-parse HEAD)
DOCKER_REGISTRY_DOMAIN:=us-central1-docker.pkg.dev
DOCKER_IMAGE:=$(DOCKER_REGISTRY_DOMAIN)/matthewlymer-lymersite/default/httpd:$(DOCKER_TAG)

.PHONY: build
build:
	docker build --platform=linux/amd64 --tag $(DOCKER_IMAGE) --file "../containers/httpd/Dockerfile" ../

.PHONY: push
push: build
	gcloud auth configure-docker $(DOCKER_REGISTRY_DOMAIN) --quiet
	docker push $(DOCKER_IMAGE)

.PHONY: plan
plan:
	cd workload-webapp && \
		terraform init && \
		terraform plan -var 'webapp_image=$(DOCKER_IMAGE)'

.PHONY: apply
apply: push
	cd workload-webapp && \
		terraform init && \
		terraform apply -auto-approve -var 'webapp_image=$(DOCKER_IMAGE)'
