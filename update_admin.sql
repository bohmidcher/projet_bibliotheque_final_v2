-- Script pour mettre à jour les données dans la table Members

-- Copier les données de JoinDate vers DateInscription si nécessaire
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'JoinDate')
AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND name = 'DateInscription')
BEGIN
    UPDATE [dbo].[Members] 
    SET [DateInscription] = [JoinDate];
    PRINT 'Données de JoinDate copiées vers DateInscription.';
END

-- Définir l'utilisateur ahmed.chermiti@ihec.tn comme administrateur
UPDATE [dbo].[Members]
SET [IsAdmin] = 1
WHERE [Email] = 'ahmed.chermiti@ihec.tn';
PRINT 'Utilisateur ahmed.chermiti@ihec.tn défini comme administrateur.';

PRINT 'Mise à jour des données terminée.';
