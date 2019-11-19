/* Add new bap_priority column to the lut_habitat_type table. */
ALTER TABLE [lut_habitat_type] ADD bap_priority bit

/* Set the default bap_priority value to 0. */
UPDATE [lut_habitat_type] SET bap_priority = 0

/* Set the bap_priority value to 1 for all 'PHAP' habitat types. */
UPDATE [lut_habitat_type] SET bap_priority = 1 WHERE habitat_class_code = 'PHAP'

/* Remove redundant constraints from the incid_bap table. */
ALTER TABLE [incid_bap] DROP CONSTRAINT ck_incid_bap_bap_habitat
