/* Create the new incid_osmm_updates table. */
CREATE TABLE [incid_osmm_updates] (incid_osmm_update_id int NOT NULL, incid nvarchar(12) NOT NULL, osmm_xref_id int NULL, spatial_flag nvarchar(1) NULL, process_flag int NULL, change_flag nvarchar(1) NULL, status int NULL, created_date datetime NOT NULL, created_user_id nvarchar(40) NOT NULL, last_modified_date datetime NOT NULL, last_modified_user_id nvarchar(40) NOT NULL)
ALTER TABLE [incid_osmm_updates] ADD CONSTRAINT pk__incid_osmm_updates PRIMARY KEY CLUSTERED (incid_osmm_update_id)

[SqlServer,PostgreSql,Oracle]
ALTER TABLE [incid_osmm_updates] WITH CHECK ADD CONSTRAINT fk_incid_osmm_updates_incid FOREIGN KEY(incid) REFERENCES [incid] (incid)
ALTER TABLE [incid_osmm_updates] CHECK CONSTRAINT fk_incid_osmm_updates_incid
[Access]
ALTER TABLE [incid_osmm_updates] ADD CONSTRAINT fk_incid_osmm_updates_incid FOREIGN KEY(incid) REFERENCES [incid] (incid)
[All]

[SqlServer,PostgreSql,Oracle]
ALTER TABLE [incid_osmm_updates] WITH CHECK ADD CONSTRAINT fk_incid_osmm_updates_lut_osmm_ihs_xref FOREIGN KEY(osmm_xref_id) REFERENCES [lut_osmm_ihs_xref] (osmm_xref_id)
ALTER TABLE [incid_osmm_updates] CHECK CONSTRAINT fk_incid_osmm_updates_lut_osmm_ihs_xref
[Access]
ALTER TABLE [incid_osmm_updates] ADD CONSTRAINT fk_incid_osmm_updates_lut_osmm_ihs_xref FOREIGN KEY(osmm_xref_id) REFERENCES [lut_osmm_ihs_xref] (osmm_xref_id)
[All]

[SqlServer,PostgreSql,Oracle]
ALTER TABLE [incid_osmm_updates] WITH CHECK ADD CONSTRAINT fk_incid_osmm_updates_user_created FOREIGN KEY(created_user_id) REFERENCES [lut_user] (user_id)
ALTER TABLE [incid_osmm_updates] CHECK CONSTRAINT fk_incid_osmm_updates_user_created
[Access]
ALTER TABLE [incid_osmm_updates] ADD CONSTRAINT fk_incid_osmm_updates_user_created FOREIGN KEY(created_user_id) REFERENCES [lut_user] (user_id)
[All]

[SqlServer,PostgreSql,Oracle]
ALTER TABLE [incid_osmm_updates] WITH CHECK ADD CONSTRAINT fk_incid_osmm_updates_user_modified FOREIGN KEY(last_modified_user_id) REFERENCES [lut_user] (user_id)
ALTER TABLE [incid_osmm_updates] CHECK CONSTRAINT fk_incid_osmm_updates_user_modified
[Access]
ALTER TABLE [incid_osmm_updates] ADD CONSTRAINT fk_incid_osmm_updates_user_modified FOREIGN KEY(last_modified_user_id) REFERENCES [lut_user] (user_id)
[All]
