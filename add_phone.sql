-- Script pour ajouter la colonne Phone
USE BibliothequeIHEC;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Members' AND COLUMN_NAME = 'Phone')
BEGIN
    ALTER TABLE Members ADD Phone NVARCHAR(20) NULL;
    PRINT 'Colonne Phone ajoutée';
END
ELSE
BEGIN
    PRINT 'Colonne Phone existe déjà';
END
