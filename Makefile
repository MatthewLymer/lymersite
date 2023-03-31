DOCKER_TAG := latest
DOCKER_REGISTRY_DOMAIN := gcr.io
DOCKER_IMAGE := $(DOCKER_REGISTRY_DOMAIN)/matthewlymer-lymersite/httpd:$(DOCKER_TAG)

JEKYLL_VERSION=3.8

.PHONY: list
list:
	@LC_ALL=C $(MAKE) -pRrq -f $(lastword $(MAKEFILE_LIST)) : 2>/dev/null | awk -v RS= -F: '/(^|\n)# Files(\n|$$)/,/(^|\n)# Finished Make data base/ {if ($$1 !~ "^[#.]") {print $$1}}' | sort | egrep -v -e '^[^[:alnum:]]' -e '^$@$$'

.PHONY: clean
clean:
	rm -r ${PWD}/matthewlymer.github.io/_site

.PHONY: build
build: clean
	docker run -it --rm \
	--env "JEKYLL_ENV=production" \
	--env "JEKYLL_DATA_DIR=/srv/matthewlymer.github.io" \
	--volume="${PWD}/.git:/srv/.git:Z" \
	--volume="${PWD}/matthewlymer.github.io:/srv/matthewlymer.github.io:Z" \
	--volume="${PWD}/.jekyll/bundle:/usr/local/bundle:Z" \
	--workdir "/srv/matthewlymer.github.io" \
	jekyll/jekyll:$(JEKYLL_VERSION) \
	jekyll build

.PHONY: docker-build
docker-build: 
	docker build --tag "$(DOCKER_IMAGE)" --file "./containers/httpd/Dockerfile" .

.PHONY: docker-push
docker-push: docker-build
	gcloud auth configure-docker "$(DOCKER_REGISTRY_DOMAIN)" --quiet
	docker push "$(DOCKER_IMAGE)"
