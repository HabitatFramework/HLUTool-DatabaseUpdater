/* Create the new habitat_type to ihs_habitat xref table. */
CREATE TABLE [lut_habitat_type_ihs_habitat]	(code_habitat_type nvarchar(11) NOT NULL, code_habitat nvarchar(8) NOT NULL, comments nvarchar(100) NULL, added_by nvarchar(40) NOT NULL, added_date smalldatetime NOT NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NOT NULL, custodian nvarchar(4) NOT NULL)
ALTER TABLE [lut_habitat_type_ihs_habitat] ADD CONSTRAINT pk__lut_habitat_type_ihs_habitat PRIMARY KEY CLUSTERED (code_habitat_type, code_habitat)

/* Create the constraints on the new lut_habitat_type_ihs_habitat table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [lut_habitat_type_ihs_habitat] WITH CHECK ADD CONSTRAINT fk_xref_habitat_type_lut_habitat_type FOREIGN KEY (code_habitat_type) REFERENCES [lut_habitat_type] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_habitat_type_ihs_habitat] WITH CHECK ADD CONSTRAINT fk_xref_habitat_type_lut_ihs_habitat FOREIGN KEY (code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_habitat_type_ihs_habitat] CHECK CONSTRAINT fk_xref_habitat_type_lut_habitat_type
ALTER TABLE [lut_habitat_type_ihs_habitat] CHECK CONSTRAINT fk_xref_habitat_type_lut_ihs_habitat
[Access]
ALTER TABLE [lut_habitat_type_ihs_habitat] ADD CONSTRAINT fk_xref_habitat_type_lut_habitat_type FOREIGN KEY (code_habitat_type) REFERENCES [lut_habitat_type] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_habitat_type_ihs_habitat] ADD CONSTRAINT fk_xref_habitat_type_lut_ihs_habitat FOREIGN KEY (code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
[All]

/* Update the minimum application version. */
UPDATE [lut_version] SET app_version = '2.1.0'
