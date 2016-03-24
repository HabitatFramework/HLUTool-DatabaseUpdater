/* Set a longer timeout to allow the updates to run. */
SET TIMEOUT 120

/* Change all lower case source importances to upper case. */
[SqlServer,PostgreSql,Oracle]
UPDATE [incid_sources] SET source_boundary_importance = UPPER(source_boundary_importance)
UPDATE [incid_sources] SET source_habitat_importance = UPPER(source_habitat_importance)
[Access]
UPDATE [incid_sources] SET source_boundary_importance = UCASE(source_boundary_importance)
UPDATE [incid_sources] SET source_habitat_importance = UCASE(source_habitat_importance)
[All]

/* Reset the default timeout. */
SET TIMEOUT DEFAULT
