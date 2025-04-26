-- Mettre à jour la table Authors pour permettre les valeurs nulles
ALTER TABLE Authors ALTER COLUMN Name nvarchar(max) NULL;
ALTER TABLE Authors ALTER COLUMN Birthdate datetime2 NULL;
ALTER TABLE Authors ALTER COLUMN Nationality nvarchar(max) NULL;
