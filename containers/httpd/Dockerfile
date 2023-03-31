FROM jekyll/jekyll:3.8 AS build

WORKDIR /app/matthewlymer.github.io

COPY ./matthewlymer.github.io /app/matthewlymer.github.io
COPY ./.git /app/.git

RUN chown -R jekyll .

RUN JEKYLL_ENV=production jekyll build

FROM httpd:2.4

COPY --from=build /app/matthewlymer.github.io/_site /usr/local/apache2/htdocs
