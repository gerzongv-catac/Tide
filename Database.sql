CREATE SCHEMA "Tide";

---

CREATE TABLE "Tide"."Country"
(
    "Code"   CHAR(2) PRIMARY KEY  NOT NULL,
    "Name"   VARCHAR(32)          NOT NULL,
    "Active" BOOLEAN DEFAULT TRUE NOT NULL
);

CREATE TABLE "Tide"."Organization"
(
    "Id"          SERIAL PRIMARY KEY   NOT NULL,
    "Name"        VARCHAR(128)         NOT NULL,
    "CountryCode" CHAR(2)              NOT NULL,
    "Active"      BOOLEAN DEFAULT TRUE NOT NULL,
    CONSTRAINT FK_Organization FOREIGN KEY ("CountryCode") REFERENCES "Tide"."Country" ("Code")
);

CREATE INDEX idx1 ON "Tide"."Organization" ("CountryCode");

CREATE TABLE "Tide"."Station"
(
    "Code"           CHAR(4) PRIMARY KEY  NOT NULL,
    "Name"           VARCHAR(128)         NOT NULL,
    "Location"       VARCHAR(512)         NOT NULL,
    "IpAddress"      VARCHAR(15)          NOT NULL,
    "Latitude"       FLOAT                NOT NULL,
    "Longitude"      FLOAT                NOT NULL,
    "Depth"          FLOAT                NOT NULL,
    "OrganizationId" INTEGER              NOT NULL,
    "Active"         BOOLEAN DEFAULT TRUE NOT NULL,
    CONSTRAINT FK_Station FOREIGN KEY ("OrganizationId") REFERENCES "Tide"."Organization" ("Id")
);

CREATE INDEX idx2 ON "Tide"."Station" ("OrganizationId");

CREATE TABLE "Tide"."Data"
(
    "Id"          SERIAL PRIMARY KEY NOT NULL,
    "StationCode" CHAR(4)            NOT NULL,
    "Time"        TIMESTAMP          NOT NULL,
    "Value"       FLOAT              NOT NULL,
    CONSTRAINT FK_Data FOREIGN KEY ("StationCode") REFERENCES "Tide"."Station" ("Code")

);

CREATE INDEX idx3 ON "Tide"."Data" ("StationCode");
CREATE INDEX idx4 ON "Tide"."Data" ("Time");
CREATE INDEX idx5 ON "Tide"."Data" ("StationCode", "Time");

---

INSERT INTO "Tide"."Country" ("Code", "Name")
VALUES ('NI', 'Nicaragua');

INSERT INTO "Tide"."Organization" ("Name", "CountryCode")
VALUES ('Central American Tsunami Advisory Center', 'NI');

INSERT INTO "Tide"."Station" ("Code", "Name", "Location", "IpAddress", "Latitude", "Longitude", "Depth",
                              "OrganizationId")
VALUES ('pblu', 'Puerto El Bluff', 'Puerto El Bluff', '', 0, 0, 0, 1),
       ('asro', 'Aserraderos', 'Aserraderos', '', 0, 0, 0, 1),
       ('pbil', 'Puerto Bilwi', 'Puerto Bilwi', '', 0, 0, 0, 1),
       ('pots', 'Puerto Potosi', 'Puerto Potosi', '186.77.179.158', 0, 0, 0, 1),
       ('pcoi', 'Muelle de Corn Island', 'Muelle de Corn Island', '', 0, 0, 0, 1),
       ('psan', 'Puerto Sandino', 'Puerto Sandino', '', 0, 0, 0, 1),
       ('pcrt', 'Puerto Corinto', 'Puerto Corinto', '186.77.179.147', 0, 0, 0, 1),
       ('psjs', 'Puerto San Juan del Sur', 'Puerto San Juan del Sur', '186.77.179.154', 0, 0, 0, 1);
