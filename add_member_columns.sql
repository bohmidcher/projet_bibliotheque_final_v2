-- Script pour ajouter les colonnes manquantes à la table Members
USE BibliothequeIHEC;

-- Partie 1: Ajouter les colonnes
PRINT 'Ajout des colonnes à la table Members...';

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Members' AND COLUMN_NAME = 'Address')
BEGIN
    ALTER TABLE Members ADD Address NVARCHAR(255) NULL;
    PRINT 'Colonne Address ajoutée';
END
ELSE
BEGIN
    PRINT 'Colonne Address existe déjà';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Members' AND COLUMN_NAME = 'Phone')
BEGIN
    ALTER TABLE Members ADD Phone NVARCHAR(20) NULL;
    PRINT 'Colonne Phone ajoutée';
END
ELSE
BEGIN
    PRINT 'Colonne Phone existe déjà';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Members' AND COLUMN_NAME = 'Status')
BEGIN
    ALTER TABLE Members ADD Status NVARCHAR(50) NULL;
    PRINT 'Colonne Status ajoutée';
END
ELSE
BEGIN
    PRINT 'Colonne Status existe déjà';
END

-- Partie 2: Mettre à jour les données (séparée pour éviter les erreurs)
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Members' AND COLUMN_NAME = 'Address')
AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Members' AND COLUMN_NAME = 'Phone')
AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Members' AND COLUMN_NAME = 'Status')
BEGIN
    PRINT 'Mise à jour des données...';
    UPDATE Members
    SET Address = 'IHEC Carthage', 
        Phone = '71234567', 
        Status = 'Actif'
    WHERE Id = 1;
    PRINT 'Données mises à jour avec succès';
END
ELSE
BEGIN
    PRINT 'Impossible de mettre à jour les données, certaines colonnes n''existent pas';
END

PRINT 'Opération terminée';
