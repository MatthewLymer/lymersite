PACKAGE ?= webapp
VERSION ?= latest

DOCKER_REGISTRY_DOMAIN ?= gcr.io
DOCKER_REGISTRY_PATH ?= matthewlymer-lymersite
DOCKER_IMAGE ?= $(DOCKER_REGISTRY_PATH)/$(PACKAGE):$(VERSION)
DOCKER_IMAGE_DOMAIN ?= $(DOCKER_REGISTRY_DOMAIN)/$(DOCKER_IMAGE)

JEKYLL_VERSION=3.8

build:
	docker run -it --rm \
	--volume="${PWD}/webroot:/srv/jekyll:Z" \
	jekyll/jekyll:$(JEKYLL_VERSION) \
	jekyll build

serve:
	docker run -it --rm \
	--volume="${PWD}/webroot:/srv/jekyll:Z" \
	--publish "4000:4000/tcp" \
	jekyll/jekyll:$(JEKYLL_VERSION) \
	jekyll serve

build-docker:
	docker build -t "$(DOCKER_IMAGE_DOMAIN)" -f "./containers/webapp/Dockerfile" .

auth-docker:
	gcloud auth configure-docker "$(DOCKER_REGISTRY_DOMAIN)" --quiet

push-docker: build-docker auth-docker
	docker push "$(DOCKER_IMAGE_DOMAIN)"
