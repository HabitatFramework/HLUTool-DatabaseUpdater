/* Create the new lut_ihs_habitat_bap_habitat table. */
CREATE TABLE [lut_ihs_habitat_bap_habitat] (code_habitat nvarchar(8) NOT NULL, bap_habitat nvarchar(11) NOT NULL, comments nvarchar(100) NULL, added_by nvarchar(40) NOT NULL, added_date smalldatetime NOT NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NOT NULL, custodian nvarchar(8) NOT NULL)
ALTER TABLE [lut_ihs_habitat_bap_habitat]  ADD CONSTRAINT pk__lut_ihs_habitat_bap_habitat PRIMARY KEY CLUSTERED (code_habitat ASC, bap_habitat ASC)

/* Add a constraint on the lut_ihs_habitat_bap_habitat table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_ihs_habitat_bap_habitat] WITH CHECK ADD CONSTRAINT fk_xref_habitat_bap_habitat_lut_habitat_type FOREIGN KEY(bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_ihs_habitat_bap_habitat] CHECK CONSTRAINT fk_xref_habitat_bap_habitat_lut_habitat_type
[Access]
ALTER TABLE [lut_ihs_habitat_bap_habitat] ADD CONSTRAINT fk_xref_habitat_bap_habitat_lut_habitat_type FOREIGN KEY(bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Add a constraint on the lut_ihs_habitat_bap_habitat table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_ihs_habitat_bap_habitat] WITH CHECK ADD CONSTRAINT fk_xref_habitat_bap_habitat_lut_ihs_habitat FOREIGN KEY(code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_ihs_habitat_bap_habitat] CHECK CONSTRAINT fk_xref_habitat_bap_habitat_lut_ihs_habitat
[Access]
ALTER TABLE [lut_ihs_habitat_bap_habitat] ADD CONSTRAINT fk_xref_habitat_bap_habitat_lut_ihs_habitat FOREIGN KEY(code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Create the new lut_ihs_complex_bap_habitat table. */
CREATE TABLE [lut_ihs_complex_bap_habitat] (code_complex nvarchar(8) NOT NULL, bap_habitat nvarchar(11) NOT NULL, comments nvarchar(100) NULL, added_by nvarchar(40) NOT NULL, added_date smalldatetime NOT NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NOT NULL, custodian nvarchar(8) NOT NULL)
ALTER TABLE [lut_ihs_complex_bap_habitat]  ADD CONSTRAINT pk__lut_ihs_complex_bap_habitat PRIMARY KEY CLUSTERED (code_complex ASC, bap_habitat ASC)

/* Add a constraint on the lut_ihs_complex_bap_habitat table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_ihs_complex_bap_habitat] WITH CHECK ADD CONSTRAINT fk_xref_complex_bap_habitat_lut_habitat_type FOREIGN KEY(bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_ihs_complex_bap_habitat] CHECK CONSTRAINT fk_xref_complex_bap_habitat_lut_habitat_type
[Access]
ALTER TABLE [lut_ihs_complex_bap_habitat] ADD CONSTRAINT fk_xref_complex_bap_habitat_lut_habitat_type FOREIGN KEY(bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Add a constraint on the lut_ihs_complex_bap_habitat table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_ihs_complex_bap_habitat] WITH CHECK ADD CONSTRAINT fk_xref_complex_bap_habitat_lut_ihs_habitat FOREIGN KEY(code_complex) REFERENCES [lut_ihs_complex] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_ihs_complex_bap_habitat] CHECK CONSTRAINT fk_xref_complex_bap_habitat_lut_ihs_habitat
[Access]
ALTER TABLE [lut_ihs_complex_bap_habitat] ADD CONSTRAINT fk_xref_complex_bap_habitat_lut_ihs_habitat FOREIGN KEY(code_complex) REFERENCES [lut_ihs_complex] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Create the new lut_ihs_formation_bap_habitat table. */
CREATE TABLE [lut_ihs_formation_bap_habitat] (code_formation nvarchar(8) NOT NULL, bap_habitat nvarchar(11) NOT NULL, comments nvarchar(100) NULL, added_by nvarchar(40) NOT NULL, added_date smalldatetime NOT NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NOT NULL, custodian nvarchar(8) NOT NULL)
ALTER TABLE [lut_ihs_formation_bap_habitat]  ADD CONSTRAINT pk__lut_ihs_formation_bap_habitat PRIMARY KEY CLUSTERED (code_formation ASC, bap_habitat ASC)

/* Add a constraint on the lut_ihs_formation_bap_habitat table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_ihs_formation_bap_habitat] WITH CHECK ADD CONSTRAINT fk_xref_formation_bap_habitat_lut_habitat_type FOREIGN KEY(bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_ihs_formation_bap_habitat] CHECK CONSTRAINT fk_xref_formation_bap_habitat_lut_habitat_type
[Access]
ALTER TABLE [lut_ihs_formation_bap_habitat] ADD CONSTRAINT fk_xref_formation_bap_habitat_lut_habitat_type FOREIGN KEY(bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Add a constraint on the lut_ihs_formation_bap_habitat table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_ihs_formation_bap_habitat] WITH CHECK ADD CONSTRAINT fk_xref_formation_bap_habitat_lut_ihs_habitat FOREIGN KEY(code_formation) REFERENCES [lut_ihs_formation] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_ihs_formation_bap_habitat] CHECK CONSTRAINT fk_xref_formation_bap_habitat_lut_ihs_habitat
[Access]
ALTER TABLE [lut_ihs_formation_bap_habitat] ADD CONSTRAINT fk_xref_formation_bap_habitat_lut_ihs_habitat FOREIGN KEY(code_formation) REFERENCES [lut_ihs_formation] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Create the new lut_ihs_management_bap_habitat table. */
CREATE TABLE [lut_ihs_management_bap_habitat] (code_management nvarchar(8) NOT NULL, bap_habitat nvarchar(11) NOT NULL, comments nvarchar(100) NULL, added_by nvarchar(40) NOT NULL, added_date smalldatetime NOT NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NOT NULL, custodian nvarchar(8) NOT NULL)
ALTER TABLE [lut_ihs_management_bap_habitat]  ADD CONSTRAINT pk__lut_ihs_management_bap_habitat PRIMARY KEY CLUSTERED (code_management ASC, bap_habitat ASC)

/* Add a constraint on the lut_ihs_management_bap_habitat table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_ihs_management_bap_habitat] WITH CHECK ADD CONSTRAINT fk_xref_management_bap_habitat_lut_habitat_type FOREIGN KEY(bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_ihs_management_bap_habitat] CHECK CONSTRAINT fk_xref_management_bap_habitat_lut_habitat_type
[Access]
ALTER TABLE [lut_ihs_management_bap_habitat] ADD CONSTRAINT fk_xref_management_bap_habitat_lut_habitat_type FOREIGN KEY(bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Add a constraint on the lut_ihs_management_bap_habitat table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_ihs_management_bap_habitat] WITH CHECK ADD CONSTRAINT fk_xref_management_bap_habitat_lut_ihs_habitat FOREIGN KEY(code_management) REFERENCES [lut_ihs_management] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_ihs_management_bap_habitat] CHECK CONSTRAINT fk_xref_management_bap_habitat_lut_ihs_habitat
[Access]
ALTER TABLE [lut_ihs_management_bap_habitat] ADD CONSTRAINT fk_xref_management_bap_habitat_lut_ihs_habitat FOREIGN KEY(code_management) REFERENCES [lut_ihs_management] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Create the new lut_ihs_matrix_bap_habitat table. */
CREATE TABLE [lut_ihs_matrix_bap_habitat] (code_matrix nvarchar(8) NOT NULL, bap_habitat nvarchar(11) NOT NULL, comments nvarchar(100) NULL, added_by nvarchar(40) NOT NULL, added_date smalldatetime NOT NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NOT NULL, custodian nvarchar(8) NOT NULL)
ALTER TABLE [lut_ihs_matrix_bap_habitat]  ADD CONSTRAINT pk__lut_ihs_matrix_bap_habitat PRIMARY KEY CLUSTERED (code_matrix ASC, bap_habitat ASC)

/* Add a constraint on the lut_ihs_matrix_bap_habitat table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_ihs_matrix_bap_habitat] WITH CHECK ADD CONSTRAINT fk_xref_matrix_bap_habitat_lut_habitat_type FOREIGN KEY(bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_ihs_matrix_bap_habitat] CHECK CONSTRAINT fk_xref_matrix_bap_habitat_lut_habitat_type
[Access]
ALTER TABLE [lut_ihs_matrix_bap_habitat] ADD CONSTRAINT fk_xref_matrix_bap_habitat_lut_habitat_type FOREIGN KEY(bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]

/* Add a constraint on the lut_ihs_matrix_bap_habitat table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_ihs_matrix_bap_habitat] WITH CHECK ADD CONSTRAINT fk_xref_matrix_bap_habitat_lut_ihs_habitat FOREIGN KEY(code_matrix) REFERENCES [lut_ihs_matrix] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [lut_ihs_matrix_bap_habitat] CHECK CONSTRAINT fk_xref_matrix_bap_habitat_lut_ihs_habitat
[Access]
ALTER TABLE [lut_ihs_matrix_bap_habitat] ADD CONSTRAINT fk_xref_matrix_bap_habitat_lut_ihs_habitat FOREIGN KEY(code_matrix) REFERENCES [lut_ihs_matrix] (code) ON UPDATE NO ACTION ON DELETE NO ACTION
[All]
