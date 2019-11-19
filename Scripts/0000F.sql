/* Create the new lut_osmm_ihs_xref table. */
CREATE TABLE [lut_osmm_ihs_xref] (osmm_xref_id int NOT NULL, make nvarchar(20) NOT NULL, desc_group nvarchar(150) NOT NULL, desc_term nvarchar(150) NULL, theme nvarchar(80) NOT NULL, feat_code int NOT NULL, ihs_habitat nvarchar(8) NOT NULL, ihs_matrix1 nvarchar(8) NULL, ihs_matrix2 nvarchar(8) NULL, ihs_matrix3 nvarchar(8) NULL, ihs_formation1 nvarchar(8) NULL, ihs_formation2 nvarchar(8) NULL, ihs_management1 nvarchar(8) NULL, ihs_management2 nvarchar(8) NULL, ihs_complex1 nvarchar(8) NULL, ihs_complex2 nvarchar(8) NULL, ihs_summary nvarchar(80) NULL, manmade bit NULL, comments nvarchar(150) NULL, is_local bit NOT NULL, added_by nvarchar(40) NOT NULL, added_date smalldatetime NOT NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NOT NULL, custodian nvarchar(8) NOT NULL)
ALTER TABLE [lut_osmm_ihs_xref] ADD CONSTRAINT pk__lut_osmm_ihs_xref PRIMARY KEY CLUSTERED (osmm_xref_id)

[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_osmm_ihs_xref] WITH CHECK ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_complex1 FOREIGN KEY(ihs_complex1) REFERENCES [lut_ihs_complex] (code)
ALTER TABLE [lut_osmm_ihs_xref] CHECK CONSTRAINT fk_lut_osmm_ihs_xref_ihs_complex1
[Access]
ALTER TABLE [lut_osmm_ihs_xref] ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_complex1 FOREIGN KEY(ihs_complex1) REFERENCES [lut_ihs_complex] (code)
[All]

[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_osmm_ihs_xref] WITH CHECK ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_complex2 FOREIGN KEY(ihs_complex2) REFERENCES [lut_ihs_complex] (code)
ALTER TABLE [lut_osmm_ihs_xref] CHECK CONSTRAINT fk_lut_osmm_ihs_xref_ihs_complex2
[Access]
ALTER TABLE [lut_osmm_ihs_xref] ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_complex2 FOREIGN KEY(ihs_complex2) REFERENCES [lut_ihs_complex] (code)
[All]

[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_osmm_ihs_xref] WITH CHECK ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_formation1 FOREIGN KEY(ihs_formation1) REFERENCES [lut_ihs_formation] (code)
ALTER TABLE [lut_osmm_ihs_xref] CHECK CONSTRAINT fk_lut_osmm_ihs_xref_ihs_formation1
[Access]
ALTER TABLE [lut_osmm_ihs_xref] ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_formation1 FOREIGN KEY(ihs_formation1) REFERENCES [lut_ihs_formation] (code)
[All]

[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_osmm_ihs_xref] WITH CHECK ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_formation2 FOREIGN KEY(ihs_formation2) REFERENCES [lut_ihs_formation] (code)
ALTER TABLE [lut_osmm_ihs_xref] CHECK CONSTRAINT fk_lut_osmm_ihs_xref_ihs_formation2
[Access]
ALTER TABLE [lut_osmm_ihs_xref] ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_formation2 FOREIGN KEY(ihs_formation2) REFERENCES [lut_ihs_formation] (code)
[All]

[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_osmm_ihs_xref] WITH CHECK ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_habitat FOREIGN KEY(ihs_habitat) REFERENCES [lut_ihs_habitat] (code)
ALTER TABLE [lut_osmm_ihs_xref] CHECK CONSTRAINT fk_lut_osmm_ihs_xref_ihs_habitat
[Access]
ALTER TABLE [lut_osmm_ihs_xref] ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_habitat FOREIGN KEY(ihs_habitat) REFERENCES [lut_ihs_habitat] (code)
[All]

[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_osmm_ihs_xref] WITH CHECK ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_management1 FOREIGN KEY(ihs_management1) REFERENCES [lut_ihs_management] (code)
ALTER TABLE [lut_osmm_ihs_xref] CHECK CONSTRAINT fk_lut_osmm_ihs_xref_ihs_management1
[Access]
ALTER TABLE [lut_osmm_ihs_xref] ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_management1 FOREIGN KEY(ihs_management1) REFERENCES [lut_ihs_management] (code)
[All]

[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_osmm_ihs_xref] WITH CHECK ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_management2 FOREIGN KEY(ihs_management2) REFERENCES [lut_ihs_management] (code)
ALTER TABLE [lut_osmm_ihs_xref] CHECK CONSTRAINT fk_lut_osmm_ihs_xref_ihs_management2
[Access]
ALTER TABLE [lut_osmm_ihs_xref] ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_management2 FOREIGN KEY(ihs_management2) REFERENCES [lut_ihs_management] (code)
[All]

[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_osmm_ihs_xref] WITH CHECK ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_matrix1 FOREIGN KEY(ihs_matrix1) REFERENCES [lut_ihs_matrix] (code)
ALTER TABLE [lut_osmm_ihs_xref] CHECK CONSTRAINT fk_lut_osmm_ihs_xref_ihs_matrix1
[Access]
ALTER TABLE [lut_osmm_ihs_xref] ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_matrix1 FOREIGN KEY(ihs_matrix1) REFERENCES [lut_ihs_matrix] (code)
[All]

[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_osmm_ihs_xref] WITH CHECK ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_matrix2 FOREIGN KEY(ihs_matrix2) REFERENCES [lut_ihs_matrix] (code)
ALTER TABLE [lut_osmm_ihs_xref] CHECK CONSTRAINT fk_lut_osmm_ihs_xref_ihs_matrix2
[Access]
ALTER TABLE [lut_osmm_ihs_xref] ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_matrix2 FOREIGN KEY(ihs_matrix2) REFERENCES [lut_ihs_matrix] (code)
[All]

[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_osmm_ihs_xref] WITH CHECK ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_matrix3 FOREIGN KEY(ihs_matrix3) REFERENCES [lut_ihs_matrix] (code)
ALTER TABLE [lut_osmm_ihs_xref] CHECK CONSTRAINT fk_lut_osmm_ihs_xref_ihs_matrix3
[Access]
ALTER TABLE [lut_osmm_ihs_xref] ADD CONSTRAINT fk_lut_osmm_ihs_xref_ihs_matrix3 FOREIGN KEY(ihs_matrix3) REFERENCES [lut_ihs_matrix] (code)
[All]
