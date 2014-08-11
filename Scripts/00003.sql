/* Alter most of the lookup tables to include added and modified, system_supplied and custodian columns. */
ALTER TABLE [lut_bap_quality_determination] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_bap_quality_interpretation] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_boundary_map] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_habitat_class] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_habitat_type] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_ihs_category] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_ihs_complex] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_ihs_formation] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_ihs_habitat] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_ihs_habitat_ihs_complex] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_ihs_habitat_ihs_formation] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_ihs_habitat_ihs_management] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_ihs_habitat_ihs_matrix] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_ihs_habitat_ihs_nvc] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_ihs_management] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_ihs_matrix] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_process] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_reason] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL
ALTER TABLE [lut_sources] ADD added_by nvarchar(40) NULL, added_date smalldatetime NULL, modified_by nvarchar(40) NULL, modified_date smalldatetime NULL, system_supplied bit NULL, custodian nvarchar(8) NULL

/* Update the above lookup tables to set the added, system_supplied and custodian columns. */
UPDATE [lut_bap_quality_determination] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_bap_quality_interpretation] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_boundary_map] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_habitat_class] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_habitat_type] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_ihs_category] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_ihs_complex] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_ihs_formation] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_ihs_habitat] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_ihs_habitat_ihs_complex] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_ihs_habitat_ihs_formation] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_ihs_habitat_ihs_management] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_ihs_habitat_ihs_matrix] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_ihs_habitat_ihs_nvc] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_ihs_management] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_ihs_matrix] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_process] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_reason] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'
UPDATE [lut_sources] SET added_by = 'Andy Foy', added_date = #2014-08-15#, modified_by = NULL, modified_date = NULL, system_supplied = 1, custodian = '0000'

/* Alter the above lookup tables to set the added, system_supplied and custodian columns to NOT NULL. */
ALTER TABLE [lut_bap_quality_determination] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_bap_quality_determination] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_bap_quality_determination] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_bap_quality_determination] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_bap_quality_interpretation] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_bap_quality_interpretation] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_bap_quality_interpretation] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_bap_quality_interpretation] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_boundary_map] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_boundary_map] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_boundary_map] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_boundary_map] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_habitat_class] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_habitat_class] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_habitat_class] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_habitat_class] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_habitat_type] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_habitat_type] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_habitat_type] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_habitat_type] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_ihs_category] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_ihs_category] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_ihs_category] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_ihs_category] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_ihs_complex] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_ihs_complex] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_ihs_complex] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_ihs_complex] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_ihs_formation] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_ihs_formation] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_ihs_formation] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_ihs_formation] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_ihs_habitat] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_ihs_habitat] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_ihs_habitat] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_ihs_habitat] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_complex] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_complex] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_complex] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_complex] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_formation] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_formation] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_formation] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_formation] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_management] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_management] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_management] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_management] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_matrix] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_matrix] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_matrix] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_matrix] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_nvc] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_nvc] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_nvc] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_ihs_habitat_ihs_nvc] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_ihs_management] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_ihs_management] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_ihs_management] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_ihs_management] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_ihs_matrix] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_ihs_matrix] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_ihs_matrix] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_ihs_matrix] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_process] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_process] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_process] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_process] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_reason] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_reason] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_reason] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_reason] ALTER COLUMN custodian nvarchar(8) NOT NULL
ALTER TABLE [lut_sources] ALTER COLUMN added_by nvarchar(40) NOT NULL
ALTER TABLE [lut_sources] ALTER COLUMN added_date smalldatetime NOT NULL
ALTER TABLE [lut_sources] ALTER COLUMN system_supplied bit NOT NULL
ALTER TABLE [lut_sources] ALTER COLUMN custodian nvarchar(8) NOT NULL
