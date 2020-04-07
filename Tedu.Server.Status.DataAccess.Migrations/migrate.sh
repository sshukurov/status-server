#!/bin/sh
# migrate.sh

set -e

until PGPASSWORD=$PASSWORD psql -h "$HOST" -d "$DATABASE" -U "$USER" -c '\q'; do
  >&2 echo "Database is unavailable - sleeping"
  sleep 1
done

>&2 echo "Database is up - running migration script"
PGPASSWORD=$PASSWORD psql -h "$HOST" -d "$DATABASE" -U "$USER" -e -f "/etc/migration/migrate.sql"