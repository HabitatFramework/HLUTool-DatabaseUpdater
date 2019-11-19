/* Create the new lut_osmm_updates_change table. */
CREATE TABLE [lut_osmm_updates_change] (code nvarchar(1) NOT NULL, description nvarchar(50) NOT NULL, sort_order int NOT NULL)
ALTER TABLE [lut_osmm_updates_change] ADD CONSTRAINT pk__lut_osmm_updates_change PRIMARY KEY CLUSTERED (code)

/* Create the new lut_osmm_updates_process table. */
CREATE TABLE [lut_osmm_updates_process] (code nvarchar(4) NOT NULL, description nvarchar(50) NOT NULL, sort_order int NOT NULL)
ALTER TABLE [lut_osmm_updates_process] ADD CONSTRAINT pk__lut_osmm_updates_process PRIMARY KEY CLUSTERED (code)

/* Create the new lut_osmm_updates_spatial table. */
CREATE TABLE [lut_osmm_updates_spatial] (code nvarchar(1) NOT NULL, description nvarchar(50) NOT NULL, sort_order int NOT NULL)
ALTER TABLE [lut_osmm_updates_spatial] ADD CONSTRAINT pk__lut_osmm_updates_spatial PRIMARY KEY CLUSTERED (code)
