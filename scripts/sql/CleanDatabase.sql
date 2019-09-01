USE [qubaininfo-prod-db]
GO

USE i2fam
GO

-- Disable all constraints
EXEC sp_MSForEachTable 'SELECT "ALTER TABLE [" +  OBJECT_SCHEMA_NAME(parent_object_id) + "].[" + OBJECT_NAME(parent_object_id) + "] DROP CONSTRAINT [" + name + "]" FROM sys.foreign_keys WHERE referenced_object_id = object_id("?")'

-- Delete data in all tables
EXEC sp_MSForEachTable 'SET QUOTED_IDENTIFIER ON; DROP TABLE ?'

-- Dnable all constraints
EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all'

-- Reseed identity columns
EXEC sp_MSForEachTable 'DBCC CHECKIDENT (''?'', RESEED, 0)'
