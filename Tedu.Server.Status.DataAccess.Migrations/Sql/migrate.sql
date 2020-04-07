CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191004173245_InitialCreate') THEN
    CREATE TABLE server (
        id serial NOT NULL,
        host text NOT NULL,
        CONSTRAINT pk_server PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191004173245_InitialCreate') THEN
    CREATE TABLE probe (
        id serial NOT NULL,
        serverid integer NOT NULL,
        checkeddatetimeutc timestamp without time zone NOT NULL,
        type integer NOT NULL,
        result integer NOT NULL,
        CONSTRAINT pk_probe PRIMARY KEY (id),
        CONSTRAINT fk_probe_server_serverid FOREIGN KEY (serverid) REFERENCES server (id) ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191004173245_InitialCreate') THEN
    CREATE INDEX ix_probe_serverid ON probe (serverid);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191004173245_InitialCreate') THEN
    CREATE UNIQUE INDEX ix_server_host ON server (host);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191004173245_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20191004173245_InitialCreate', '2.2.6-servicing-10079');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191004173818_Add-Backup-table') THEN
    CREATE TABLE backup (
        id serial NOT NULL,
        serverid integer NOT NULL,
        isstatusok boolean NOT NULL,
        backupsamount integer NOT NULL,
        createddatetimeutc timestamp without time zone NOT NULL,
        lastbackupstartdatetimeutc timestamp without time zone NOT NULL,
        lastbackupenddatetimeutc timestamp without time zone NOT NULL,
        lastbackupsizebytes integer NOT NULL,
        backupdurationtotalseconds integer NOT NULL,
        backupdurationcopyseconds integer NOT NULL,
        oldestbackupenddatetimeutc timestamp without time zone NOT NULL,
        diskusedbytes integer NOT NULL,
        diskfreebytes integer NOT NULL,
        CONSTRAINT pk_backup PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191004173818_Add-Backup-table') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20191004173818_Add-Backup-table', '2.2.6-servicing-10079');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191007102655_Add-Server-Foreign-Key-To-Backup') THEN
    CREATE INDEX ix_backup_serverid ON backup (serverid);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191007102655_Add-Server-Foreign-Key-To-Backup') THEN
    ALTER TABLE backup ADD CONSTRAINT fk_backup_server_serverid FOREIGN KEY (serverid) REFERENCES server (id) ON DELETE CASCADE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191007102655_Add-Server-Foreign-Key-To-Backup') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20191007102655_Add-Server-Foreign-Key-To-Backup', '2.2.6-servicing-10079');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191007151038_Change-field-types-representing-bytes-to-ulong') THEN
    ALTER TABLE backup ALTER COLUMN lastbackupsizebytes TYPE numeric(20,0);
    ALTER TABLE backup ALTER COLUMN lastbackupsizebytes SET NOT NULL;
    ALTER TABLE backup ALTER COLUMN lastbackupsizebytes DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191007151038_Change-field-types-representing-bytes-to-ulong') THEN
    ALTER TABLE backup ALTER COLUMN diskusedbytes TYPE numeric(20,0);
    ALTER TABLE backup ALTER COLUMN diskusedbytes SET NOT NULL;
    ALTER TABLE backup ALTER COLUMN diskusedbytes DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191007151038_Change-field-types-representing-bytes-to-ulong') THEN
    ALTER TABLE backup ALTER COLUMN diskfreebytes TYPE numeric(20,0);
    ALTER TABLE backup ALTER COLUMN diskfreebytes SET NOT NULL;
    ALTER TABLE backup ALTER COLUMN diskfreebytes DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191007151038_Change-field-types-representing-bytes-to-ulong') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20191007151038_Change-field-types-representing-bytes-to-ulong', '2.2.6-servicing-10079');
    END IF;
END $$;
