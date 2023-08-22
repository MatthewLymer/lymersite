FROM jekyll/jekyll:3.8 AS build

ENV JEKYLL_UID=0
ENV JEKYLL_GID=0

ARG GITHUB_PAGE_NAME=matthewlymer.github.io

WORKDIR /src/${GITHUB_PAGE_NAME}

# "jekyll build" will run do a "bundle install", which 
# takes a long time, so instead run it before with just
# the Gemfile and Gemfile.lock so it'll be cached by docker
COPY ${GITHUB_PAGE_NAME}/Gemfile Gemfile
COPY ${GITHUB_PAGE_NAME}/Gemfile.lock Gemfile.lock
RUN bundle install

COPY ${GITHUB_PAGE_NAME} .
COPY .git ../.git

RUN mkdir /output && chown jekyll /output

RUN JEKYLL_ENV=production jekyll build --trace --destination /output

FROM httpd:2.4

COPY --from=build /output /usr/local/apache2/htdocs
