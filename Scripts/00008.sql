/* Switch off error handling. */
SET IGNORE_ERRORS ON

/* Drop any constraints on the lut_ihs_category table so that it can be dropped. */
ALTER TABLE [incid_mm_polygons] DROP CONSTRAINT fk_incid_mm_polygons_lut_ihs_category

/* Switch on error handling again. */
SET IGNORE_ERRORS OFF

/* Drop the unused ihs_category lookup table. */
DROP TABLE [lut_ihs_category]
