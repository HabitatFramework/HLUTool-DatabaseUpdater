/* Copy existing habitat bap habitats codes into new lut_ihs_habitat_bap_habitat table. */
INSERT INTO [lut_ihs_habitat_bap_habitat] (code_habitat, bap_habitat, comments, added_by, added_date, modified_by, modified_date, system_supplied, custodian) SELECT code, code_bap_priority_habitat, NULL, added_by, added_date, modified_by, modified_date, system_supplied, custodian FROM [lut_ihs_habitat] WHERE code_bap_priority_habitat IS NOT NULL

/* Copy existing complex bap habitats codes into new lut_ihs_complex_bap_habitat table. */
INSERT INTO [lut_ihs_complex_bap_habitat] (code_complex, bap_habitat, comments, added_by, added_date, modified_by, modified_date, system_supplied, custodian) SELECT code, bap_habitat, NULL, added_by, added_date, modified_by, modified_date, system_supplied, custodian FROM [lut_ihs_complex] WHERE bap_habitat IS NOT NULL

/* Copy existing formation bap habitats codes into new lut_ihs_formation_bap_habitat table. */
INSERT INTO [lut_ihs_formation_bap_habitat] (code_formation, bap_habitat, comments, added_by, added_date, modified_by, modified_date, system_supplied, custodian) SELECT code, bap_habitat, NULL, added_by, added_date, modified_by, modified_date, system_supplied, custodian FROM [lut_ihs_formation] WHERE bap_habitat IS NOT NULL

/* Copy existing management bap habitats codes into new lut_ihs_management_bap_habitat table. */
INSERT INTO [lut_ihs_management_bap_habitat] (code_management, bap_habitat, comments, added_by, added_date, modified_by, modified_date, system_supplied, custodian) SELECT code, bap_habitat, NULL, added_by, added_date, modified_by, modified_date, system_supplied, custodian FROM [lut_ihs_management] WHERE bap_habitat IS NOT NULL

/* Copy existing matrix bap habitats codes into new lut_ihs_matrix_bap_habitat table. */
INSERT INTO [lut_ihs_matrix_bap_habitat] (code_matrix, bap_habitat, comments, added_by, added_date, modified_by, modified_date, system_supplied, custodian) SELECT code, bap_habitat, NULL, added_by, added_date, modified_by, modified_date, system_supplied, custodian FROM [lut_ihs_matrix] WHERE bap_habitat IS NOT NULL
