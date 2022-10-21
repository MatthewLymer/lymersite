PACKAGE ?= webapp
VERSION ?= latest

DOCKER_REGISTRY_DOMAIN ?= gcr.io
DOCKER_REGISTRY_PATH ?= matthewlymer-lymersite
DOCKER_IMAGE ?= $(DOCKER_REGISTRY_PATH)/$(PACKAGE):$(VERSION)
DOCKER_IMAGE_DOMAIN ?= $(DOCKER_REGISTRY_DOMAIN)/$(DOCKER_IMAGE)

JEKYLL_VERSION=3.8

.PHONY: list
list:
	@LC_ALL=C $(MAKE) -pRrq -f $(lastword $(MAKEFILE_LIST)) : 2>/dev/null | awk -v RS= -F: '/(^|\n)# Files(\n|$$)/,/(^|\n)# Finished Make data base/ {if ($$1 !~ "^[#.]") {print $$1}}' | sort | egrep -v -e '^[^[:alnum:]]' -e '^$@$$'

.PHONY: build
build:
	docker run -it --rm \
	--volume="${PWD}/matthewlymer.github.io:/srv/jekyll:Z" \
	jekyll/jekyll:$(JEKYLL_VERSION) \
	jekyll build

.PHONY: serve
serve:
	docker run -it --rm \
	--volume="${PWD}/matthewlymer.github.io:/srv/jekyll:Z" \
	--publish "4000:4000/tcp" \
	jekyll/jekyll:$(JEKYLL_VERSION) \
	jekyll serve

.PHONY: build-docker
build-docker:
	docker build -t "$(DOCKER_IMAGE_DOMAIN)" -f "./containers/webapp/Dockerfile" .

.PHONY: auth-docker
auth-docker:
	gcloud auth configure-docker "$(DOCKER_REGISTRY_DOMAIN)" --quiet

.PHONY: push-docker
push-docker: build-docker auth-docker
	docker push "$(DOCKER_IMAGE_DOMAIN)"

