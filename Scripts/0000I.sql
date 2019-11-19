/* Add the new determination quality. */
INSERT INTO [lut_bap_quality_determination] (code ,description ,sort_order ,added_by ,added_date ,modified_by ,modified_date ,system_supplied ,custodian) VALUES ('PP' ,'Previously present, but may no longer exist' ,5 ,'Andy Foy' ,'2019-10-22' ,NULL ,NULL ,1 ,'0000')

/* Set the sort order for the 'possibly is' determination quality. */
UPDATE [lut_bap_quality_determination] SET sort_order= 6 WHERE code = 'PI'

/* Update the minimum application version. */
UPDATE [lut_version] SET app_version = '3.0.0'
