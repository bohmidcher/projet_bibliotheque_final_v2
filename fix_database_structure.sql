-- Script pour corriger la structure de la base de données
-- Résoudre les erreurs de connexion en ajoutant les colonnes manquantes et en mappant les colonnes existantes

-- Vérifier si la colonne Birthdate existe, sinon l'ajouter
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'Birthdate')
BEGIN
    ALTER TABLE [dbo].[Members] ADD [Birthdate] DATETIME NULL;
    PRINT 'Colonne Birthdate ajoutée.';
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

-- Vérifier si la colonne DateInscription existe, sinon la créer et copier les données de JoinDate
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'DateInscription')
BEGIN
    -- Ajouter la colonne DateInscription
    ALTER TABLE [dbo].[Members] ADD [DateInscription] DATETIME NOT NULL DEFAULT GETDATE();
    
    -- Copier les données de JoinDate vers DateInscription
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'JoinDate')
    BEGIN
        EXEC('UPDATE [dbo].[Members] SET [DateInscription] = [JoinDate]');
        PRINT 'Données de JoinDate copiées vers DateInscription.';
    END
    
    PRINT 'Colonne DateInscription ajoutée.';
END

-- Définir au moins un utilisateur comme administrateur
UPDATE [dbo].[Members]
SET [IsAdmin] = 1
WHERE [Email] = 'ahmed.chermiti@ihec.tn';
PRINT 'Utilisateur ahmed.chermiti@ihec.tn défini comme administrateur.';

PRINT 'Mise à jour de la structure de la base de données terminée.';
