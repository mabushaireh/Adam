-- CREATE DATABASE i2fam
-- GO

USE [qubaininfo-prod-db]
GO

use i2fam
GO

SELECT * from AspNetUsers

UPDATE AspNetUsers SET LockoutEnabled = 0


-- DROP DATABASE i2fam

-- GO

SELECT * FROM AppFamilyMembers where FamilyId = 146
GO

SELECT * FROM AppFamilyMemberUpdates
GO

SELECT * FROM AppLookupItems WHERE CategoryId = 0 AND Lang = 0 AND LocaleString like 'Spain'
GO


-- TRUNCATE TABLE AppFamilyMembers
-- GO