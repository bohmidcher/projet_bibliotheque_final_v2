-- Script pour mettre à jour la structure de la table Members
-- Ajout des colonnes manquantes qui causent l'erreur de connexion

-- Vérifier si la colonne Birthdate existe, sinon l'ajouter
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'Birthdate')
BEGIN
    ALTER TABLE [dbo].[Members] ADD [Birthdate] DATETIME NULL;
    PRINT 'Colonne Birthdate ajoutée.';
END

-- Vérifier si la colonne DateInscription existe, sinon l'ajouter
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'DateInscription')
BEGIN
    ALTER TABLE [dbo].[Members] ADD [DateInscription] DATETIME NOT NULL DEFAULT GETDATE();
    PRINT 'Colonne DateInscription ajoutée.';
END

-- Vérifier si la colonne IsAdmin existe, sinon l'ajouter
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'IsAdmin')
BEGIN
    ALTER TABLE [dbo].[Members] ADD [IsAdmin] BIT NOT NULL DEFAULT 0;
    PRINT 'Colonne IsAdmin ajoutée.';
END

-- Vérifier si la colonne NiveauEducatif existe, sinon l'ajouter
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'NiveauEducatif')
BEGIN
    ALTER TABLE [dbo].[Members] ADD [NiveauEducatif] NVARCHAR(50) NULL;
    PRINT 'Colonne NiveauEducatif ajoutée.';
END

-- Vérifier si la colonne Specialite existe, sinon l'ajouter
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'Specialite')
BEGIN
    ALTER TABLE [dbo].[Members] ADD [Specialite] NVARCHAR(50) NULL;
    PRINT 'Colonne Specialite ajoutée.';
END

-- Définir au moins un utilisateur comme administrateur (seulement si la colonne existe)
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'IsAdmin')
BEGIN
    UPDATE [dbo].[Members]
    SET [IsAdmin] = 1
    WHERE [Email] = 'ahmed.chermiti@ihec.tn';
    PRINT 'Utilisateur ahmed.chermiti@ihec.tn défini comme administrateur.';
END

PRINT 'Mise à jour de la table Members terminée.';
