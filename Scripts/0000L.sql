/* Remove redundant columns from the lut_ihs_complex table. */
ALTER TABLE [lut_ihs_habitat] DROP COLUMN bap_priority
ALTER TABLE [lut_ihs_habitat] DROP COLUMN code_bap_priority_habitat

/* Remove redundant constraints from the lut_ihs_complex table. */
ALTER TABLE [lut_ihs_complex] DROP CONSTRAINT fk_lut_ihs_complex_lut_habitat_type
ALTER TABLE [lut_ihs_complex] DROP CONSTRAINT ck_lut_ihs_complex_bap_habitat

/* Remove redundant columns from the lut_ihs_complex table. */
ALTER TABLE [lut_ihs_complex] DROP COLUMN bap_priority, bap_habitat

/* Remove redundant constraints from the lut_ihs_formation table. */
ALTER TABLE [lut_ihs_formation] DROP CONSTRAINT fk_lut_ihs_formation_lut_habitat_type
ALTER TABLE [lut_ihs_formation] DROP CONSTRAINT ck_lut_ihs_formation_bap_habitat

/* Remove redundant columns from the lut_ihs_formation table. */
ALTER TABLE [lut_ihs_formation] DROP COLUMN bap_priority, bap_habitat

/* Remove redundant constraints from the lut_ihs_management table. */
ALTER TABLE [lut_ihs_management] DROP CONSTRAINT fk_lut_ihs_management_lut_habitat_type
ALTER TABLE [lut_ihs_management] DROP CONSTRAINT ck_lut_ihs_management_bap_habitat

/* Remove redundant columns from the lut_ihs_management table. */
ALTER TABLE [lut_ihs_management] DROP COLUMN bap_priority, bap_habitat

/* Remove redundant constraints from the lut_ihs_matrix table. */
ALTER TABLE [lut_ihs_matrix] DROP CONSTRAINT fk_lut_ihs_matrix_lut_habitat_type
ALTER TABLE [lut_ihs_matrix] DROP CONSTRAINT ck_lut_ihs_matrix_bap_habitat

/* Remove redundant columns from the lut_ihs_matrix table. */
ALTER TABLE [lut_ihs_matrix] DROP COLUMN bap_priority, bap_habitat
