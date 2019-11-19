/* Create the lut_habitat_type_ihs_complex table. */
CREATE TABLE [lut_habitat_type_ihs_complex] (code_habitat_type nvarchar(11) NOT NULL, code_habitat nvarchar(8) NOT NULL, code_complex nvarchar(8) NOT NULL, mandatory int NOT NULL, comments nvarchar(100) NULL, added_by nvarchar(40) NOT NULL, added_date smalldatetime NOT NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NOT NULL, custodian nvarchar(4) NOT NULL)
ALTER TABLE [lut_habitat_type_ihs_complex] ADD CONSTRAINT pk__lut_habitat_type_ihs_complex PRIMARY KEY CLUSTERED (code_habitat_type ASC, code_habitat ASC, code_complex ASC)

/* Add a constraint on the lut_habitat_type_ihs_complex table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_habitat_type_ihs_complex] WITH CHECK ADD CONSTRAINT fk_xref_habitat_type_complex_lut_habitat_type FOREIGN KEY(code_habitat_type) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_habitat_type_ihs_complex] CHECK CONSTRAINT fk_xref_habitat_type_complex_lut_habitat_type
[Access]
ALTER TABLE [lut_habitat_type_ihs_complex] ADD CONSTRAINT fk_xref_habitat_type_complex_lut_habitat_type FOREIGN KEY(code_habitat_type) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Add a constraint on the lut_habitat_type_ihs_complex table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_habitat_type_ihs_complex] WITH CHECK ADD CONSTRAINT fk_xref_habitat_type_complex_lut_ihs_habitat FOREIGN KEY(code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_habitat_type_ihs_complex] CHECK CONSTRAINT fk_xref_habitat_type_complex_lut_ihs_habitat
[Access]
ALTER TABLE [lut_habitat_type_ihs_complex] ADD CONSTRAINT fk_xref_habitat_type_complex_lut_ihs_habitat FOREIGN KEY(code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Create the lut_habitat_type_ihs_formation table. */
CREATE TABLE [lut_habitat_type_ihs_formation] (code_habitat_type nvarchar(11) NOT NULL, code_habitat nvarchar(8) NOT NULL, code_formation nvarchar(8) NOT NULL, mandatory int NOT NULL, comments nvarchar(100) NULL, added_by nvarchar(40) NOT NULL, added_date smalldatetime NOT NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NOT NULL, custodian nvarchar(4) NOT NULL)
ALTER TABLE [lut_habitat_type_ihs_formation] ADD CONSTRAINT pk__lut_habitat_type_ihs_formation PRIMARY KEY CLUSTERED (code_habitat_type ASC, code_habitat ASC, code_formation ASC)

/* Add a constraint on the lut_habitat_type_ihs_formation table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_habitat_type_ihs_formation] WITH CHECK ADD CONSTRAINT fk_xref_habitat_type_formation_lut_habitat_type FOREIGN KEY(code_habitat_type) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_habitat_type_ihs_formation] CHECK CONSTRAINT fk_xref_habitat_type_formation_lut_habitat_type
[Access]
ALTER TABLE [lut_habitat_type_ihs_formation] ADD CONSTRAINT fk_xref_habitat_type_formation_lut_habitat_type FOREIGN KEY(code_habitat_type) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Add a constraint on the lut_habitat_type_ihs_formation table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_habitat_type_ihs_formation] WITH CHECK ADD CONSTRAINT fk_xref_habitat_type_formation_lut_ihs_habitat FOREIGN KEY(code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_habitat_type_ihs_formation] CHECK CONSTRAINT fk_xref_habitat_type_formation_lut_ihs_habitat
[Access]
ALTER TABLE [lut_habitat_type_ihs_formation] ADD CONSTRAINT fk_xref_habitat_type_formation_lut_ihs_habitat FOREIGN KEY(code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Create the lut_habitat_type_ihs_management table. */
CREATE TABLE [lut_habitat_type_ihs_management] (code_habitat_type nvarchar(11) NOT NULL, code_habitat nvarchar(8) NOT NULL, code_management nvarchar(8) NOT NULL, mandatory int NOT NULL, comments nvarchar(100) NULL, added_by nvarchar(40) NOT NULL, added_date smalldatetime NOT NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NOT NULL, custodian nvarchar(4) NOT NULL)
ALTER TABLE [lut_habitat_type_ihs_management] ADD CONSTRAINT pk__lut_habitat_type_ihs_management PRIMARY KEY CLUSTERED (code_habitat_type ASC, code_habitat ASC, code_management ASC)

/* Add a constraint on the lut_habitat_type_ihs_management table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_habitat_type_ihs_management] WITH CHECK ADD CONSTRAINT fk_xref_habitat_type_management_lut_habitat_type FOREIGN KEY(code_habitat_type) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_habitat_type_ihs_management] CHECK CONSTRAINT fk_xref_habitat_type_management_lut_habitat_type
[Access]
ALTER TABLE [lut_habitat_type_ihs_management] ADD CONSTRAINT fk_xref_habitat_type_management_lut_habitat_type FOREIGN KEY(code_habitat_type) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Add a constraint on the lut_habitat_type_ihs_management table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_habitat_type_ihs_management] WITH CHECK ADD CONSTRAINT fk_xref_habitat_type_management_lut_ihs_habitat FOREIGN KEY(code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_habitat_type_ihs_management] CHECK CONSTRAINT fk_xref_habitat_type_management_lut_ihs_habitat
[Access]
ALTER TABLE [lut_habitat_type_ihs_management] ADD CONSTRAINT fk_xref_habitat_type_management_lut_ihs_habitat FOREIGN KEY(code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Create the lut_habitat_type_ihs_matrix table. */
CREATE TABLE [lut_habitat_type_ihs_matrix] (code_habitat_type nvarchar(11) NOT NULL, code_habitat nvarchar(8) NOT NULL, code_matrix nvarchar(8) NOT NULL, mandatory int NOT NULL, comments nvarchar(100) NULL, added_by nvarchar(40) NOT NULL, added_date smalldatetime NOT NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NOT NULL, custodian nvarchar(4) NOT NULL)
ALTER TABLE [lut_habitat_type_ihs_matrix] ADD CONSTRAINT pk__lut_habitat_type_ihs_matrix PRIMARY KEY CLUSTERED (code_habitat_type ASC, code_habitat ASC, code_matrix ASC)

/* Add a constraint on the lut_habitat_type_ihs_matrix table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_habitat_type_ihs_matrix] WITH CHECK ADD CONSTRAINT fk_xref_habitat_type_matrix_lut_habitat_type FOREIGN KEY(code_habitat_type) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_habitat_type_ihs_matrix] CHECK CONSTRAINT fk_xref_habitat_type_matrix_lut_habitat_type
[Access]
ALTER TABLE [lut_habitat_type_ihs_matrix] ADD CONSTRAINT fk_xref_habitat_type_matrix_lut_habitat_type FOREIGN KEY(code_habitat_type) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Add a constraint on the lut_habitat_type_ihs_matrix table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_habitat_type_ihs_matrix] WITH CHECK ADD CONSTRAINT fk_xref_habitat_type_matrix_lut_ihs_habitat FOREIGN KEY(code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_habitat_type_ihs_matrix] CHECK CONSTRAINT fk_xref_habitat_type_matrix_lut_ihs_habitat
[Access]
ALTER TABLE [lut_habitat_type_ihs_matrix] ADD CONSTRAINT fk_xref_habitat_type_matrix_lut_ihs_habitat FOREIGN KEY(code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Update the minimum application version. */
UPDATE [lut_version] SET app_version = '3.1.0'
