-- Script pour ajouter les colonnes manquantes à la table Members

-- Ajouter la colonne Birthdate
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'Birthdate')
BEGIN
    ALTER TABLE [dbo].[Members] ADD [Birthdate] DATETIME NULL;
    PRINT 'Colonne Birthdate ajoutée.';
END

-- Ajouter la colonne IsAdmin
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'IsAdmin')
BEGIN
    ALTER TABLE [dbo].[Members] ADD [IsAdmin] BIT NOT NULL DEFAULT 0;
    PRINT 'Colonne IsAdmin ajoutée.';
END

-- Ajouter la colonne NiveauEducatif
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'NiveauEducatif')
BEGIN
    ALTER TABLE [dbo].[Members] ADD [NiveauEducatif] NVARCHAR(50) NULL;
    PRINT 'Colonne NiveauEducatif ajoutée.';
END

-- Ajouter la colonne Specialite
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'Specialite')
BEGIN
    ALTER TABLE [dbo].[Members] ADD [Specialite] NVARCHAR(50) NULL;
    PRINT 'Colonne Specialite ajoutée.';
END

-- Ajouter la colonne DateInscription
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'DateInscription')
BEGIN
    ALTER TABLE [dbo].[Members] ADD [DateInscription] DATETIME NOT NULL DEFAULT GETDATE();
    PRINT 'Colonne DateInscription ajoutée.';
END

PRINT 'Ajout des colonnes terminé.';
