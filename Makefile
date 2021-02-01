PACKAGE ?= webapp
VERSION ?= latest

DOCKER_REGISTRY_DOMAIN ?= gcr.io
DOCKER_REGISTRY_PATH ?= matthewlymer-lymersite
DOCKER_IMAGE ?= $(DOCKER_REGISTRY_PATH)/$(PACKAGE):$(VERSION)
DOCKER_IMAGE_DOMAIN ?= $(DOCKER_REGISTRY_DOMAIN)/$(DOCKER_IMAGE)

build-docker:
	docker build -t "$(DOCKER_IMAGE_DOMAIN)" -f "./containers/webapp/Dockerfile" .

auth-docker:
	gcloud auth configure-docker "$(DOCKER_REGISTRY_DOMAIN)" --quiet

push-docker: build-docker auth-docker
	docker push "$(DOCKER_IMAGE_DOMAIN)"
