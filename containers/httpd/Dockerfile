FROM jekyll/jekyll:3.8 AS build

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

# github actions on docker gives
# permission issue otherwise
RUN chown 777 .

RUN mkdir /output && chown jekyll /output

RUN JEKYLL_ENV=production jekyll build --trace --destination /output

FROM httpd:2.4

COPY --from=build /output /usr/local/apache2/htdocs
