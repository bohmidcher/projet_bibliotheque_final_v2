USE BibliothequeIHEC;
GO

-- Supprimer la colonne Matiere si elle existe
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Members') AND name = 'Matiere')
BEGIN
    ALTER TABLE Members DROP COLUMN Matiere;
END
GO

-- Ajouter les nouvelles colonnes si elles n'existent pas
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Members') AND name = 'NiveauEducatif')
BEGIN
    ALTER TABLE Members ADD NiveauEducatif nvarchar(50) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Members') AND name = 'Specialite')
BEGIN
    ALTER TABLE Members ADD Specialite nvarchar(50) NULL;
END
GO 