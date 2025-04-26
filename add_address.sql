-- Script pour ajouter la colonne Address
USE BibliothequeIHEC;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Members' AND COLUMN_NAME = 'Address')
BEGIN
    ALTER TABLE Members ADD Address NVARCHAR(255) NULL;
    PRINT 'Colonne Address ajoutée';
END
ELSE
BEGIN
    PRINT 'Colonne Address existe déjà';
END
