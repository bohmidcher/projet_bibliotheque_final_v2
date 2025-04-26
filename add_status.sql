-- Script pour ajouter la colonne Status
USE BibliothequeIHEC;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Members' AND COLUMN_NAME = 'Status')
BEGIN
    ALTER TABLE Members ADD Status NVARCHAR(50) NULL;
    PRINT 'Colonne Status ajoutée';
END
ELSE
BEGIN
    PRINT 'Colonne Status existe déjà';
END
