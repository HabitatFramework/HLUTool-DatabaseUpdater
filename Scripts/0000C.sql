/* Create the exports_field_types table. */
CREATE TABLE [exports_field_types] (field_type int NOT NULL, field_description nvarchar(20) NOT NULL)
ALTER TABLE [exports_field_types] ADD CONSTRAINT PK__exports_field_types PRIMARY KEY CLUSTERED (field_type)

/* Insert the field types into the new table. */
INSERT INTO [exports_field_types] (field_type, field_description) VALUES (3, 'Integer')
INSERT INTO [exports_field_types] (field_type, field_description) VALUES (6, 'Single')
INSERT INTO [exports_field_types] (field_type, field_description) VALUES (7, 'Double')
INSERT INTO [exports_field_types] (field_type, field_description) VALUES (8, 'Date/Time')
INSERT INTO [exports_field_types] (field_type, field_description) VALUES (10, 'Text')
INSERT INTO [exports_field_types] (field_type, field_description) VALUES (99, 'AutoNumber')

/* Alter the export_fields table to include field_type, field_length and field_format columns. */
ALTER TABLE [exports_fields] ADD field_type int NULL, field_length int NULL, field_format nvarchar(40) NULL

/* Update the export_fields table to set the field_type column default value. */
UPDATE [exports_fields] SET field_type = 10 WHERE (field_type = 0 OR field_type IS NULL)

/* Alter the export_fields table to set the field_type column to NOT NULL. */
ALTER TABLE [exports_fields] ALTER COLUMN field_type int NOT NULL

/* Add a constraint on the exports_fields table. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [exports_fields] WITH CHECK ADD CONSTRAINT fk_exports_fields_exports_field_types FOREIGN KEY(field_type) REFERENCES [exports_field_types] (field_type)
[Access]
ALTER TABLE [exports_fields] ADD CONSTRAINT fk_exports_fields_exports_field_types FOREIGN KEY(field_type) REFERENCES [exports_field_types] (field_type)
[All]

/* Delete the contents of the exports_fields table. */
[SqlServer,PostgreSql,Oracle]
DELETE FROM [exports_fields]
[Access]
DELETE * FROM [exports_fields]
[All]

/* Delete the contents of the exports table. */
[SqlServer,PostgreSql,Oracle]
DELETE FROM [exports]
[Access]
DELETE * FROM [exports]
[All]

/* Add a new export format to export all attribute fields. */
INSERT INTO [exports] (export_id, export_name) VALUES (1, 'All attribute fields')

/* Add all the attribute fields to the new export format. */
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (1, 1, 'incid', 'incid', 1, 'incid', 1, NULL, 10, 12, NULL)
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (2, 1, 'incid', 'legacy_habitat', 2, 'legacy_habitat', 2, NULL, 10, 50, NULL)
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (3, 1, 'incid', 'site_ref', 3, 'site_ref', 3, NULL, 10, 16, NULL)
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (4, 1, 'incid', 'site_name', 4, 'site_name', 4, NULL, 10, 100, NULL)
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (5, 1, 'incid', 'boundary_base_map', 5, 'boundary_base_map', 5, NULL, 10, 20, 'Lookup')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (6, 1, 'incid', 'digitisation_base_map', 6, 'digitisation_base_map', 6, NULL, 10, 20, 'Lookup')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (7, 1, 'incid', 'ihs_version', 7, 'ihs_version', 7, NULL, 10, 11, NULL)
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (8, 1, 'incid', 'ihs_habitat', 8, 'ihs_habitat', 8, NULL, 10, 100, 'Both')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (9, 1, 'incid_ihs_matrix', 'matrix', 3, 'ihs_matrix<no>', 9, 3, 10, 100, 'Both')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (10, 1, 'incid_ihs_formation', 'formation', 3, 'ihs_formation<no>', 10, 2, 10, 100, 'Both')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (11, 1, 'incid_ihs_management', 'management', 3, 'ihs_management<no>', 11, 2, 10, 100, 'Both')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (12, 1, 'incid_ihs_complex', 'complex', 3, 'ihs_complex<no>', 12, 2, 10, 100, 'Both')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (13, 1, 'incid_bap', 'bap_habitat', 3, 'bap_habitat', 13, 3, 10, 100, 'Lookup')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (14, 1, 'incid_bap', 'quality_determination', 4, 'bap_quality_determination', 14, 3, 10, 100, 'Lookup')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (15, 1, 'incid_bap', 'quality_interpretation', 5, 'bap_quality_interpretation', 15, 3, 10, 10, 'Lookup')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (16, 1, 'incid_bap', 'interpretation_comments', 6, 'bap_interpretation_comments', 16, 3, 10, 254, NULL)
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (17, 1, 'incid_sources', 'source_id', 4, 'source<no>_name', 17, 3, 10, 100, 'Lookup')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (18, 1, 'incid_sources', 'source_date_start', 4, 'source<no>_date', 18, 3, 10, 30, 'v')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (19, 1, 'incid_sources', 'source_habitat_class', 7, 'source<no>_habitat_class', 19, 3, 10, 100, 'Lookup')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (20, 1, 'incid_sources', 'source_habitat_type', 8, 'source<no>_habitat_type', 20, 3, 10, 100, 'Lookup')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (21, 1, 'incid_sources', 'source_boundary_importance', 9, 'source<no>_boundary_importance', 21, 3, 10, 12, 'Lookup')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (22, 1, 'incid_sources', 'source_habitat_importance', 10, 'source<no>_habitat_importance', 22, 3, 10, 12, 'Lookup')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (23, 1, 'incid', 'general_comments', 9, 'general_comments', 23, NULL, 10, 254, NULL)
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (24, 1, 'incid', 'created_date', 10, 'created_date', 24, NULL, 10, 20, 'dd/MM/yyyy HH:mm:ss')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (25, 1, 'incid', 'created_user_id', 11, 'created_user', 25, NULL, 10, 100, 'Lookup')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (26, 1, 'incid', 'last_modified_date', 12, 'last_modified_date', 26, NULL, 10, 20, 'dd/MM/yyyy HH:mm:ss')
INSERT INTO [exports_fields] (export_field_id, export_id, table_name, column_name, column_ordinal, field_name, field_ordinal, fields_count, field_type, field_length, field_format) VALUES (27, 1, 'incid', 'last_modified_user_id', 13, 'last_modified_user', 27, NULL, 10, 100, 'Lookup')

/* Update the minimum application version. */
UPDATE [lut_version] SET app_version = '2.3.0'
