/* Drop the unused ihs_category lookup table. */
ALTER TABLE [incid_mm_polygons] DROP CONSTRAINT fk_incid_mm_polygons_lut_ihs_category
DROP TABLE [lut_ihs_category]
