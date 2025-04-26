-- Script pour vérifier les tables existantes
USE BibliothequeIHEC;

-- Lister toutes les tables
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
