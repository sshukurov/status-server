# Upgrade Postgres major version #

_Inspired by: https://jlelse.blog/dev/migrate-postgres-docker/_

1. Navigate to repository directory

        cd /var/www/tedu_server/Tedu.Server.Status

1. Stop all services

        docker-compose down

1. Run database service

        docker-compose up -d tedu-status-database

1. Back up data

        docker exec -t tedu-status-db pg_dumpall -c -U postgres > /var/www/tedu_server/Tedu.Server.Status/db_dump.sql

1. Stop database service

        docker-compose down

1. Remove database volume

        docker volume rm teduserverstatus_postgres-data

1. Pull changeset with postgres version change

        git pull

1. Run database service

        docker-compose up -d tedu-status-database

1. Restore data from backup

        docker exec -i tedu-status-db psql -U postgres < /var/www/tedu_server/Tedu.Server.Status/db_dump.sql

1. Run all services

        docker-compose up -d

1. Remove backup file

        rm /var/www/tedu_server/Tedu.Server.Status/db_dump.sql
