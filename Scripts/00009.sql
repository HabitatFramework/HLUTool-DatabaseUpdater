/* Switch off error handling. */
SET IGNORE_ERRORS ON

/* Drop indexes on tables. */
DROP INDEX ix_history_incid ON [history]
DROP INDEX ix_incid_ihs_complex_incid_complex_unique ON [incid_ihs_complex]
DROP INDEX ix_incid_ihs_formation_incid_formation_unique ON [incid_ihs_formation]
DROP INDEX ix_incid_ihs_management_incid_management_unique ON [incid_ihs_management]
DROP INDEX ix_incid_ihs_matrix_incid_matrix_unique ON [incid_ihs_matrix]
DROP INDEX ix_incid_bap_incid ON [incid_bap]
DROP INDEX ix_incid_sources_incid ON [incid_sources]
DROP INDEX ix__incid_mm_polygons_incid ON [incid_mm_polygons]
DROP INDEX ix__incid_mm_polygons_toid ON [incid_mm_polygons]
DROP INDEX ix__incid_mm_polygons_toid_toid_fragment_id ON [incid_mm_polygons]

/* Switch on error handling again. */
SET IGNORE_ERRORS OFF

/* Set a longer time-out to allow the indexes to be created. */
SET TIMEOUT 120

/* Create indexes on tables. */
[SqlServer,PostgreSql,Oracle]
CREATE NONCLUSTERED INDEX ix_history_incid ON [history] (incid ASC)
CREATE UNIQUE NONCLUSTERED INDEX ix_incid_ihs_complex_incid_complex_unique ON [incid_ihs_complex] (incid ASC, complex ASC)
CREATE UNIQUE NONCLUSTERED INDEX ix_incid_ihs_formation_incid_formation_unique ON [incid_ihs_formation] (incid ASC, formation ASC)
CREATE UNIQUE NONCLUSTERED INDEX ix_incid_ihs_management_incid_management_unique ON [incid_ihs_management] (incid ASC, management ASC)
CREATE UNIQUE NONCLUSTERED INDEX ix_incid_ihs_matrix_incid_matrix_unique ON [incid_ihs_matrix] (incid ASC, matrix ASC)
CREATE NONCLUSTERED INDEX ix_incid_bap_incid ON [incid_bap] (incid ASC)
CREATE NONCLUSTERED INDEX ix_incid_sources_incid ON [incid_sources] (incid ASC)
CREATE NONCLUSTERED INDEX ix_incid_mm_polygons_incid ON [incid_mm_polygons] (incid ASC)
CREATE NONCLUSTERED INDEX ix_incid_mm_polygons_toid ON [incid_mm_polygons] (toid ASC)
CREATE NONCLUSTERED INDEX ix_incid_mm_polygons_toid_toid_fragment_id ON [incid_mm_polygons] (toid ASC, toid_fragment_id ASC)
[Access]
CREATE INDEX ix_history_incid ON [history] (incid ASC)
CREATE UNIQUE INDEX ix_incid_ihs_complex_incid_complex_unique ON [incid_ihs_complex] (incid ASC, complex ASC)
CREATE UNIQUE INDEX ix_incid_ihs_formation_incid_formation_unique ON [incid_ihs_formation] (incid ASC, formation ASC)
CREATE UNIQUE INDEX ix_incid_ihs_management_incid_management_unique ON [incid_ihs_management] (incid ASC, management ASC)
CREATE UNIQUE INDEX ix_incid_ihs_matrix_incid_matrix_unique ON [incid_ihs_matrix] (incid ASC, matrix ASC)
CREATE INDEX ix_incid_bap_incid ON [incid_bap] (incid ASC)
CREATE INDEX ix_incid_sources_incid ON [incid_sources] (incid ASC)
CREATE INDEX ix_incid_mm_polygons_incid ON [incid_mm_polygons] (incid ASC)
CREATE INDEX ix_incid_mm_polygons_toid ON [incid_mm_polygons] (toid ASC)
CREATE INDEX ix_incid_mm_polygons_toid_toid_fragment_id ON [incid_mm_polygons] (toid ASC, toid_fragment_id ASC)
[All]

/* Switch off error handling. */
SET IGNORE_ERRORS ON

/* Reset the default time-out. */
SET TIMEOUT DEFAULT

/* Drop constraints on tables. */
ALTER TABLE [lut_ihs_matrix] DROP CONSTRAINT fk_lut_ihs_matrix_lut_habitat_type
ALTER TABLE [lut_ihs_management] DROP CONSTRAINT fk_lut_ihs_management_lut_habitat_type
ALTER TABLE [lut_ihs_habitat_ihs_nvc] DROP CONSTRAINT fk_xref_nvc_lut_ihs_habitat
ALTER TABLE [lut_ihs_habitat_ihs_matrix] DROP CONSTRAINT fk_xref_matrix_lut_ihs_matrix
ALTER TABLE [lut_ihs_habitat_ihs_matrix] DROP CONSTRAINT fk_xref_matrix_lut_ihs_habitat
ALTER TABLE [lut_ihs_habitat_ihs_management] DROP CONSTRAINT fk_xref_management_lut_ihs_management
ALTER TABLE [lut_ihs_habitat_ihs_management] DROP CONSTRAINT fk_xref_management_lut_ihs_habitat
ALTER TABLE [lut_ihs_habitat_ihs_formation] DROP CONSTRAINT fk_xref_formation_lut_ihs_habitat
ALTER TABLE [lut_ihs_habitat_ihs_formation] DROP CONSTRAINT fk_xref_formation_lut_ihs_formation
ALTER TABLE [lut_ihs_habitat_ihs_complex] DROP CONSTRAINT fk_xref_complex_lut_ihs_habitat
ALTER TABLE [lut_ihs_habitat_ihs_complex] DROP CONSTRAINT fk_xref_complex_lut_ihs_complex
ALTER TABLE [lut_ihs_formation] DROP CONSTRAINT fk_lut_ihs_formation_lut_habitat_type
ALTER TABLE [lut_ihs_complex] DROP CONSTRAINT fk_lut_ihs_complex_lut_habitat_type
ALTER TABLE [lut_habitat_type_ihs_habitat] DROP CONSTRAINT fk_xref_habitat_type_lut_ihs_habitat
ALTER TABLE [lut_habitat_type_ihs_habitat] DROP CONSTRAINT fk_xref_habitat_type_lut_habitat_type
ALTER TABLE [lut_habitat_type] DROP CONSTRAINT fk_lut_habitat_type_lut_habitat_class
ALTER TABLE [incid_sources] DROP CONSTRAINT fk_incid_sources_lut_sources
ALTER TABLE [incid_sources] DROP CONSTRAINT fk_incid_sources_lut_importance_habitat
ALTER TABLE [incid_sources] DROP CONSTRAINT fk_incid_sources_lut_importance_boundary
ALTER TABLE [incid_sources] DROP CONSTRAINT fk_incid_sources_lut_habitat_type
ALTER TABLE [incid_sources] DROP CONSTRAINT fk_incid_sources_lut_habitat_class
ALTER TABLE [incid_sources] DROP CONSTRAINT fk_incid_sources_incid
ALTER TABLE [incid_ihs_matrix] DROP CONSTRAINT fk_ihs_matrix_lut_ihs_matrix
ALTER TABLE [incid_ihs_matrix] DROP CONSTRAINT fk_ihs_matrix_incid
ALTER TABLE [incid_ihs_management] DROP CONSTRAINT fk_ihs_management_lut_ihs_management
ALTER TABLE [incid_ihs_management] DROP CONSTRAINT fk_ihs_management_incid
ALTER TABLE [incid_ihs_formation] DROP CONSTRAINT fk_ihs_formation_lut_ihs_formation
ALTER TABLE [incid_ihs_formation] DROP CONSTRAINT fk_ihs_formation_incid
ALTER TABLE [incid_ihs_complex] DROP CONSTRAINT fk_ihs_complex_lut_ihs_complex
ALTER TABLE [incid_ihs_complex] DROP CONSTRAINT fk_ihs_complex_incid
ALTER TABLE [incid_bap] DROP CONSTRAINT fk_incid_bap_lut_bap_quality_interpretation
ALTER TABLE [incid_bap] DROP CONSTRAINT fk_incid_bap_lut_bap_quality_determination
ALTER TABLE [incid_bap] DROP CONSTRAINT fk_incid_bap_incid
ALTER TABLE [incid] DROP CONSTRAINT fk_incid_user_modified
ALTER TABLE [incid] DROP CONSTRAINT fk_incid_user_created
ALTER TABLE [incid] DROP CONSTRAINT fk_incid_lut_ihs_habitat
ALTER TABLE [incid] DROP CONSTRAINT fk_incid_lut_digitisation_map_base
ALTER TABLE [incid] DROP CONSTRAINT fk_incid_lut_boundary_map_base
ALTER TABLE [history] DROP CONSTRAINT fk_history_lut_operation
ALTER TABLE [history] DROP CONSTRAINT fk_habitat_history_user
ALTER TABLE [history] DROP CONSTRAINT fk_habitat_history_lut_reason
ALTER TABLE [history] DROP CONSTRAINT fk_habitat_history_lut_process
ALTER TABLE [history] DROP CONSTRAINT fk_habitat_history_incid_mm_polygons
ALTER TABLE [history] DROP CONSTRAINT fk_habitat_history_incid
ALTER TABLE [exports_fields] DROP CONSTRAINT fk_exports_fields_exports

/* Switch on error handling again. */
SET IGNORE_ERRORS OFF

/* Create constraints on tables. */
[SqlServer,PostgreSql,Oracle]
ALTER TABLE [exports_fields] WITH CHECK ADD CONSTRAINT fk_exports_fields_exports FOREIGN KEY (export_id) REFERENCES [exports] (export_id) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [exports_fields] CHECK CONSTRAINT fk_exports_fields_exports
ALTER TABLE [history] WITH CHECK ADD CONSTRAINT fk_history_incid FOREIGN KEY (incid) REFERENCES [incid] (incid) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [history] CHECK CONSTRAINT fk_history_incid
ALTER TABLE [history] WITH CHECK ADD CONSTRAINT fk_history_incid_mm_polygons FOREIGN KEY (incid, toid, toid_fragment_id) REFERENCES [incid_mm_polygons] (incid, toid, toid_fragment_id) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [history] CHECK CONSTRAINT fk_history_incid_mm_polygons
ALTER TABLE [history] WITH CHECK ADD CONSTRAINT fk_history_lut_operation FOREIGN KEY (modified_operation) REFERENCES [lut_operation] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [history] CHECK CONSTRAINT fk_history_lut_operation
ALTER TABLE [history] WITH CHECK ADD CONSTRAINT fk_history_lut_process FOREIGN KEY (modified_process) REFERENCES [lut_process] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [history] CHECK CONSTRAINT fk_history_lut_process
ALTER TABLE [history] WITH CHECK ADD CONSTRAINT fk_history_lut_reason FOREIGN KEY (modified_reason) REFERENCES [lut_reason] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [history] CHECK CONSTRAINT fk_history_lut_reason
ALTER TABLE [history] WITH CHECK ADD CONSTRAINT fk_history_user FOREIGN KEY (modified_user_id) REFERENCES [lut_user] (user_id) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [history] CHECK CONSTRAINT fk_history_user
ALTER TABLE [incid] WITH CHECK ADD CONSTRAINT fk_incid_lut_boundary_map_base FOREIGN KEY (boundary_base_map) REFERENCES [lut_boundary_map] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid] CHECK CONSTRAINT fk_incid_lut_boundary_map_base
ALTER TABLE [incid] WITH CHECK ADD CONSTRAINT fk_incid_lut_digitisation_map_base FOREIGN KEY (digitisation_base_map) REFERENCES [lut_boundary_map] (code)
ALTER TABLE [incid] CHECK CONSTRAINT fk_incid_lut_digitisation_map_base
ALTER TABLE [incid] WITH CHECK ADD CONSTRAINT fk_incid_lut_ihs_habitat FOREIGN KEY (ihs_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid] CHECK CONSTRAINT fk_incid_lut_ihs_habitat
ALTER TABLE [incid] WITH CHECK ADD CONSTRAINT fk_incid_user_created FOREIGN KEY (created_user_id) REFERENCES [lut_user] (user_id)
ALTER TABLE [incid] CHECK CONSTRAINT fk_incid_user_created
ALTER TABLE [incid] WITH CHECK ADD CONSTRAINT fk_incid_user_modified FOREIGN KEY (last_modified_user_id) REFERENCES [lut_user] (user_id)
ALTER TABLE [incid] CHECK CONSTRAINT fk_incid_user_modified
ALTER TABLE [incid_bap] WITH CHECK ADD CONSTRAINT fk_incid_bap_incid FOREIGN KEY (incid) REFERENCES [incid] (incid) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_bap] CHECK CONSTRAINT fk_incid_bap_incid
ALTER TABLE [incid_bap] WITH CHECK ADD CONSTRAINT fk_incid_bap_lut_bap_quality_determination FOREIGN KEY (quality_determination) REFERENCES [lut_bap_quality_determination] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_bap] CHECK CONSTRAINT fk_incid_bap_lut_bap_quality_determination
ALTER TABLE [incid_bap] WITH CHECK ADD CONSTRAINT fk_incid_bap_lut_bap_quality_interpretation FOREIGN KEY (quality_interpretation) REFERENCES [lut_bap_quality_interpretation] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_bap] CHECK CONSTRAINT fk_incid_bap_lut_bap_quality_interpretation
ALTER TABLE [incid_ihs_complex] WITH CHECK ADD CONSTRAINT fk_ihs_complex_incid FOREIGN KEY (incid) REFERENCES [incid] (incid) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_complex] CHECK CONSTRAINT fk_ihs_complex_incid
ALTER TABLE [incid_ihs_complex] WITH CHECK ADD CONSTRAINT fk_ihs_complex_lut_ihs_complex FOREIGN KEY (complex) REFERENCES [lut_ihs_complex] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_complex] CHECK CONSTRAINT fk_ihs_complex_lut_ihs_complex
ALTER TABLE [incid_ihs_formation] WITH CHECK ADD CONSTRAINT fk_ihs_formation_incid FOREIGN KEY (incid) REFERENCES [incid] (incid) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_formation] CHECK CONSTRAINT fk_ihs_formation_incid
ALTER TABLE [incid_ihs_formation] WITH CHECK ADD CONSTRAINT fk_ihs_formation_lut_ihs_formation FOREIGN KEY (formation) REFERENCES [lut_ihs_formation] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_formation] CHECK CONSTRAINT fk_ihs_formation_lut_ihs_formation
ALTER TABLE [incid_ihs_management] WITH CHECK ADD CONSTRAINT fk_ihs_management_incid FOREIGN KEY (incid) REFERENCES [incid] (incid) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_management] CHECK CONSTRAINT fk_ihs_management_incid
ALTER TABLE [incid_ihs_management] WITH CHECK ADD CONSTRAINT fk_ihs_management_lut_ihs_management FOREIGN KEY (management) REFERENCES [lut_ihs_management] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_management] CHECK CONSTRAINT fk_ihs_management_lut_ihs_management
ALTER TABLE [incid_ihs_matrix] WITH CHECK ADD CONSTRAINT fk_ihs_matrix_incid FOREIGN KEY (incid) REFERENCES [incid] (incid) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_matrix] CHECK CONSTRAINT fk_ihs_matrix_incid
ALTER TABLE [incid_ihs_matrix] WITH CHECK ADD CONSTRAINT fk_ihs_matrix_lut_ihs_matrix FOREIGN KEY (matrix) REFERENCES [lut_ihs_matrix] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_matrix] CHECK CONSTRAINT fk_ihs_matrix_lut_ihs_matrix
ALTER TABLE [incid_sources] WITH CHECK ADD CONSTRAINT fk_incid_sources_incid FOREIGN KEY (incid) REFERENCES [incid] (incid) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_sources] CHECK CONSTRAINT fk_incid_sources_incid
ALTER TABLE [incid_sources] WITH CHECK ADD CONSTRAINT fk_incid_sources_lut_habitat_class FOREIGN KEY (source_habitat_class) REFERENCES [lut_habitat_class] (code)
ALTER TABLE [incid_sources] CHECK CONSTRAINT fk_incid_sources_lut_habitat_class
ALTER TABLE [incid_sources] WITH CHECK ADD CONSTRAINT fk_incid_sources_lut_habitat_type FOREIGN KEY (source_habitat_type) REFERENCES [lut_habitat_type] (code)
ALTER TABLE [incid_sources] CHECK CONSTRAINT fk_incid_sources_lut_habitat_type
ALTER TABLE [incid_sources] WITH CHECK ADD CONSTRAINT fk_incid_sources_lut_importance_boundary FOREIGN KEY (source_boundary_importance) REFERENCES [lut_importance] (code)
ALTER TABLE [incid_sources] CHECK CONSTRAINT fk_incid_sources_lut_importance_boundary
ALTER TABLE [incid_sources] WITH CHECK ADD CONSTRAINT fk_incid_sources_lut_importance_habitat FOREIGN KEY (source_habitat_importance) REFERENCES [lut_importance] (code)
ALTER TABLE [incid_sources] CHECK CONSTRAINT fk_incid_sources_lut_importance_habitat
ALTER TABLE [incid_sources] WITH CHECK ADD CONSTRAINT fk_incid_sources_lut_sources FOREIGN KEY (source_id) REFERENCES [lut_sources] (source_id) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_sources] CHECK CONSTRAINT fk_incid_sources_lut_sources
ALTER TABLE [lut_habitat_type] WITH CHECK ADD CONSTRAINT fk_lut_habitat_type_lut_habitat_class FOREIGN KEY (habitat_class_code) REFERENCES [lut_habitat_class] (code)
ALTER TABLE [lut_habitat_type] CHECK CONSTRAINT fk_lut_habitat_type_lut_habitat_class
ALTER TABLE [lut_habitat_type_ihs_habitat] WITH CHECK ADD CONSTRAINT fk_xref_habitat_type_lut_habitat_type FOREIGN KEY (code_habitat_type) REFERENCES [lut_habitat_type] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_habitat_type_ihs_habitat] CHECK CONSTRAINT fk_xref_habitat_type_lut_habitat_type
ALTER TABLE [lut_habitat_type_ihs_habitat] WITH CHECK ADD CONSTRAINT fk_xref_habitat_type_lut_ihs_habitat FOREIGN KEY (code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_habitat_type_ihs_habitat] CHECK CONSTRAINT fk_xref_habitat_type_lut_ihs_habitat
ALTER TABLE [lut_ihs_complex] WITH CHECK ADD CONSTRAINT fk_lut_ihs_complex_lut_habitat_type FOREIGN KEY (bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_complex] CHECK CONSTRAINT fk_lut_ihs_complex_lut_habitat_type
ALTER TABLE [lut_ihs_formation] WITH CHECK ADD CONSTRAINT fk_lut_ihs_formation_lut_habitat_type FOREIGN KEY (bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_formation] CHECK CONSTRAINT fk_lut_ihs_formation_lut_habitat_type
ALTER TABLE [lut_ihs_habitat_ihs_complex] WITH CHECK ADD CONSTRAINT fk_xref_complex_lut_ihs_complex FOREIGN KEY (code_complex) REFERENCES [lut_ihs_complex] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_complex] CHECK CONSTRAINT fk_xref_complex_lut_ihs_complex
ALTER TABLE [lut_ihs_habitat_ihs_complex] WITH CHECK ADD CONSTRAINT fk_xref_complex_lut_ihs_habitat FOREIGN KEY (code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_complex] CHECK CONSTRAINT fk_xref_complex_lut_ihs_habitat
ALTER TABLE [lut_ihs_habitat_ihs_formation] WITH CHECK ADD CONSTRAINT fk_xref_formation_lut_ihs_formation FOREIGN KEY (code_formation) REFERENCES [lut_ihs_formation] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_formation] CHECK CONSTRAINT fk_xref_formation_lut_ihs_formation
ALTER TABLE [lut_ihs_habitat_ihs_formation] WITH CHECK ADD CONSTRAINT fk_xref_formation_lut_ihs_habitat FOREIGN KEY (code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_formation] CHECK CONSTRAINT fk_xref_formation_lut_ihs_habitat
ALTER TABLE [lut_ihs_habitat_ihs_management] WITH CHECK ADD CONSTRAINT fk_xref_management_lut_ihs_habitat FOREIGN KEY (code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_management] CHECK CONSTRAINT fk_xref_management_lut_ihs_habitat
ALTER TABLE [lut_ihs_habitat_ihs_management] WITH CHECK ADD CONSTRAINT fk_xref_management_lut_ihs_management FOREIGN KEY (code_management) REFERENCES [lut_ihs_management] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_management] CHECK CONSTRAINT fk_xref_management_lut_ihs_management
ALTER TABLE [lut_ihs_habitat_ihs_matrix] WITH CHECK ADD CONSTRAINT fk_xref_matrix_lut_ihs_habitat FOREIGN KEY (code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_matrix] CHECK CONSTRAINT fk_xref_matrix_lut_ihs_habitat
ALTER TABLE [lut_ihs_habitat_ihs_matrix] WITH CHECK ADD CONSTRAINT fk_xref_matrix_lut_ihs_matrix FOREIGN KEY (code_matrix) REFERENCES [lut_ihs_matrix] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_matrix] CHECK CONSTRAINT fk_xref_matrix_lut_ihs_matrix
ALTER TABLE [lut_ihs_habitat_ihs_nvc] WITH CHECK ADD CONSTRAINT fk_xref_nvc_lut_ihs_habitat FOREIGN KEY (code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_nvc] CHECK CONSTRAINT fk_xref_nvc_lut_ihs_habitat
ALTER TABLE [lut_ihs_management] WITH CHECK ADD CONSTRAINT fk_lut_ihs_management_lut_habitat_type FOREIGN KEY (bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_management] CHECK CONSTRAINT fk_lut_ihs_management_lut_habitat_type
ALTER TABLE [lut_ihs_matrix] WITH CHECK ADD CONSTRAINT fk_lut_ihs_matrix_lut_habitat_type FOREIGN KEY (bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_matrix] CHECK CONSTRAINT fk_lut_ihs_matrix_lut_habitat_type
[Access]
ALTER TABLE [exports_fields] ADD CONSTRAINT fk_exports_fields_exports FOREIGN KEY (export_id) REFERENCES [exports] (export_id) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [history] ADD CONSTRAINT fk_history_incid FOREIGN KEY (incid) REFERENCES [incid] (incid) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [history] ADD CONSTRAINT fk_history_incid_mm_polygons FOREIGN KEY (incid, toid, toid_fragment_id) REFERENCES [incid_mm_polygons] (incid, toid, toid_fragment_id) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [history] ADD CONSTRAINT fk_history_lut_operation FOREIGN KEY (modified_operation) REFERENCES [lut_operation] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [history] ADD CONSTRAINT fk_history_lut_process FOREIGN KEY (modified_process) REFERENCES [lut_process] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [history] ADD CONSTRAINT fk_history_lut_reason FOREIGN KEY (modified_reason) REFERENCES [lut_reason] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [history] ADD CONSTRAINT fk_history_user FOREIGN KEY (modified_user_id) REFERENCES [lut_user] (user_id) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid] ADD CONSTRAINT fk_incid_lut_boundary_map_base FOREIGN KEY (boundary_base_map) REFERENCES [lut_boundary_map] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid] ADD CONSTRAINT fk_incid_lut_digitisation_map_base FOREIGN KEY (digitisation_base_map) REFERENCES [lut_boundary_map] (code)
ALTER TABLE [incid] ADD CONSTRAINT fk_incid_lut_ihs_habitat FOREIGN KEY (ihs_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid] ADD CONSTRAINT fk_incid_user_created FOREIGN KEY (created_user_id) REFERENCES [lut_user] (user_id)
ALTER TABLE [incid] ADD CONSTRAINT fk_incid_user_modified FOREIGN KEY (last_modified_user_id) REFERENCES [lut_user] (user_id)
ALTER TABLE [incid_bap] ADD CONSTRAINT fk_incid_bap_incid FOREIGN KEY (incid) REFERENCES [incid] (incid) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_bap] ADD CONSTRAINT fk_incid_bap_lut_bap_quality_determination FOREIGN KEY (quality_determination) REFERENCES [lut_bap_quality_determination] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_bap] ADD CONSTRAINT fk_incid_bap_lut_bap_quality_interpretation FOREIGN KEY (quality_interpretation) REFERENCES [lut_bap_quality_interpretation] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_complex] ADD CONSTRAINT fk_ihs_complex_incid FOREIGN KEY (incid) REFERENCES [incid] (incid) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_complex] ADD CONSTRAINT fk_ihs_complex_lut_ihs_complex FOREIGN KEY (complex) REFERENCES [lut_ihs_complex] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_formation] ADD CONSTRAINT fk_ihs_formation_incid FOREIGN KEY (incid) REFERENCES [incid] (incid) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_formation] ADD CONSTRAINT fk_ihs_formation_lut_ihs_formation FOREIGN KEY (formation) REFERENCES [lut_ihs_formation] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_management] ADD CONSTRAINT fk_ihs_management_incid FOREIGN KEY (incid) REFERENCES [incid] (incid) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_management] ADD CONSTRAINT fk_ihs_management_lut_ihs_management FOREIGN KEY (management) REFERENCES [lut_ihs_management] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_matrix] ADD CONSTRAINT fk_ihs_matrix_incid FOREIGN KEY (incid) REFERENCES [incid] (incid) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_ihs_matrix] ADD CONSTRAINT fk_ihs_matrix_lut_ihs_matrix FOREIGN KEY (matrix) REFERENCES [lut_ihs_matrix] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_sources] ADD CONSTRAINT fk_incid_sources_incid FOREIGN KEY (incid) REFERENCES [incid] (incid) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [incid_sources] ADD CONSTRAINT fk_incid_sources_lut_habitat_class FOREIGN KEY (source_habitat_class) REFERENCES [lut_habitat_class] (code)
ALTER TABLE [incid_sources] ADD CONSTRAINT fk_incid_sources_lut_habitat_type FOREIGN KEY (source_habitat_type) REFERENCES [lut_habitat_type] (code)
ALTER TABLE [incid_sources] ADD CONSTRAINT fk_incid_sources_lut_importance_boundary FOREIGN KEY (source_boundary_importance) REFERENCES [lut_importance] (code)
ALTER TABLE [incid_sources] ADD CONSTRAINT fk_incid_sources_lut_importance_habitat FOREIGN KEY (source_habitat_importance) REFERENCES [lut_importance] (code)
ALTER TABLE [incid_sources] ADD CONSTRAINT fk_incid_sources_lut_sources FOREIGN KEY (source_id) REFERENCES [lut_sources] (source_id) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_habitat_type] ADD CONSTRAINT fk_lut_habitat_type_lut_habitat_class FOREIGN KEY (habitat_class_code) REFERENCES [lut_habitat_class] (code)
ALTER TABLE [lut_habitat_type_ihs_habitat] ADD CONSTRAINT fk_xref_habitat_type_lut_habitat_type FOREIGN KEY (code_habitat_type) REFERENCES [lut_habitat_type] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_habitat_type_ihs_habitat] ADD CONSTRAINT fk_xref_habitat_type_lut_ihs_habitat FOREIGN KEY (code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_complex] ADD CONSTRAINT fk_lut_ihs_complex_lut_habitat_type FOREIGN KEY (bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_formation] ADD CONSTRAINT fk_lut_ihs_formation_lut_habitat_type FOREIGN KEY (bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_complex] ADD CONSTRAINT fk_xref_complex_lut_ihs_complex FOREIGN KEY (code_complex) REFERENCES [lut_ihs_complex] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_complex] ADD CONSTRAINT fk_xref_complex_lut_ihs_habitat FOREIGN KEY (code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_formation] ADD CONSTRAINT fk_xref_formation_lut_ihs_formation FOREIGN KEY (code_formation) REFERENCES [lut_ihs_formation] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_formation] ADD CONSTRAINT fk_xref_formation_lut_ihs_habitat FOREIGN KEY (code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_management] ADD CONSTRAINT fk_xref_management_lut_ihs_habitat FOREIGN KEY (code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_management] ADD CONSTRAINT fk_xref_management_lut_ihs_management FOREIGN KEY (code_management) REFERENCES [lut_ihs_management] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_matrix] ADD CONSTRAINT fk_xref_matrix_lut_ihs_habitat FOREIGN KEY (code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_matrix] ADD CONSTRAINT fk_xref_matrix_lut_ihs_matrix FOREIGN KEY (code_matrix) REFERENCES [lut_ihs_matrix] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_habitat_ihs_nvc] ADD CONSTRAINT fk_xref_nvc_lut_ihs_habitat FOREIGN KEY (code_habitat) REFERENCES [lut_ihs_habitat] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_management] ADD CONSTRAINT fk_lut_ihs_management_lut_habitat_type FOREIGN KEY (bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE CASCADE ON DELETE CASCADE
ALTER TABLE [lut_ihs_matrix] ADD CONSTRAINT fk_lut_ihs_matrix_lut_habitat_type FOREIGN KEY (bap_habitat) REFERENCES [lut_habitat_type] (code) ON UPDATE CASCADE ON DELETE CASCADE
