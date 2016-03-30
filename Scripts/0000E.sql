/* Create the new lut_legacy_habitat table. */
CREATE TABLE [lut_legacy_habitat] (code nvarchar(50) NOT NULL, description nvarchar(254) NOT NULL, sort_order int NOT NULL, added_by nvarchar(40) NOT NULL, added_date smalldatetime NOT NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NOT NULL, custodian nvarchar(8) NOT NULL)
ALTER TABLE [lut_legacy_habitat] ADD CONSTRAINT pk__legacy_habitat PRIMARY KEY CLUSTERED (code)

/* Set a longer timeout to allow the updates to run. */
SET TIMEOUT 120

/*Insert the existing legacy_habitats into the new table. */
INSERT INTO [lut_legacy_habitat] SELECT DISTINCT legacy_habitat AS code, legacy_habitat AS description, 0 AS sort_order, 'Andy Foy' AS added_by, #2016-03-29# AS added_date, NULL AS modified_by, NULL AS modified_date, 0 AS system_supplied, '0000' AS custodian FROM [incid] WHERE legacy_habitat IS NOT NULL ORDER BY legacy_habitat

/* Reset the default timeout. */
SET TIMEOUT DEFAULT

/* Add a constraint on the incid table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [incid] WITH CHECK ADD CONSTRAINT fk_incid_lut_legacy_habitat FOREIGN KEY(legacy_habitat) REFERENCES [lut_legacy_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid] CHECK CONSTRAINT fk_incid_lut_legacy_habitat
[Access]
ALTER TABLE [incid] ADD CONSTRAINT fk_incid_lut_legacy_habitat FOREIGN KEY(legacy_habitat) REFERENCES [lut_legacy_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
[All]

/* Update the minimum application version. */
UPDATE [lut_version] SET app_version = '2.4.0'
