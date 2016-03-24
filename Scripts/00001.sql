/* Drop the existing lut_version table and recreate it with new columns. */
DROP TABLE [lut_version]
CREATE TABLE [lut_version] (app_version nvarchar(10) NOT NULL, db_version nvarchar(10) NOT NULL, ihs_version nvarchar(11) NOT NULL)
ALTER TABLE [lut_version] ADD CONSTRAINT PK__lut_version PRIMARY KEY CLUSTERED (app_version)

/* Insert a default row into the new table. */
INSERT INTO [lut_version] (app_version, db_Version, ihs_version) VALUES ('2.0.0', '0', '002.000.001')

/* Drop the existing lut_ihs_Version table (which has been replaced by the new lut_version table). */
DROP TABLE [lut_ihs_version]
