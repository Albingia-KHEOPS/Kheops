﻿CREATE TABLE ZALBINKHEO.KPDOCH ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPDOCH de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPDOCH de ZALBINKHEO. 
	KEQID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KEQTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEQIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KEQALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KEQSUA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KEQNUM NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KEQSBR CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KEQSERV CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEQACTG CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEQACTN NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KEQECO CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEQAVN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KEQETAP CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEQKEMID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KEQORD NUMERIC(7, 2) NOT NULL DEFAULT 0 , 
	KEQTGL CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEQTDOC CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEQKESID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KEQAJT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEQTRS CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEQMAIT CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEQLIEN NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KEQCDOC CHAR(20) CCSID 297 NOT NULL DEFAULT '' , 
	KEQVER NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KEQLIB CHAR(60) CCSID 297 NOT NULL DEFAULT '' , 
	KEQNTA CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEQDACC CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEQTAE CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KEQNOM CHAR(255) CCSID 297 NOT NULL DEFAULT '' , 
	KEQCHM CHAR(255) CCSID 297 NOT NULL DEFAULT '' , 
	KEQSTU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEQSIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEQSTD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KEQSTH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KEQENVU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEQENVD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KEQENVH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KEQTEDI CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEQORID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KEQTYDS CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KEQTYI CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KEQIDS NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KEQINL NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KEQNBEX NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KEQCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEQCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KEQCRH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KEQMAJU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEQMAJD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KEQMAJH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KEQSTG CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEQDIMP CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPDOCH     ; 
  
LABEL ON TABLE ZALBINKHEO.KPDOCH 
	IS 'Documents W pour sauveg                        KEQ' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPDOCH 
( KEQID IS 'ID unique' , 
	KEQTYP IS 'Type O/P' , 
	KEQIPB IS 'IPB' , 
	KEQALX IS 'ALX' , 
	KEQSUA IS 'Sinistre AA' , 
	KEQNUM IS 'Sinistre N°' , 
	KEQSBR IS 'Sinistre Sbr' , 
	KEQSERV IS 'Service' , 
	KEQACTG IS 'Acte de gestion' , 
	KEQACTN IS 'N° Acte de gestion' , 
	KEQECO IS 'En Cours O/N' , 
	KEQAVN IS 'N° Avenant' , 
	KEQETAP IS 'Etape' , 
	KEQKEMID IS 'Lien KALCELD' , 
	KEQORD IS 'N° Ordre' , 
	KEQTGL IS 'Généré ou Libre' , 
	KEQTDOC IS 'Type Document' , 
	KEQKESID IS 'Lien KPDOCTX texte' , 
	KEQAJT IS 'Ajouté O/N' , 
	KEQTRS IS 'Transformé O/N' , 
	KEQMAIT IS 'Table maître/origine' , 
	KEQLIEN IS 'Lien Table maître' , 
	KEQCDOC IS 'Code document' , 
	KEQVER IS 'N° Version' , 
	KEQLIB IS 'Libellé' , 
	KEQNTA IS 'Nature (O/P/S)' , 
	KEQDACC IS 'Accompagnant O/N' , 
	KEQTAE IS 'Action enchainée' , 
	KEQNOM IS 'Nom du document' , 
	KEQCHM IS 'Chemin complet' , 
	KEQSTU IS 'Situation User' , 
	KEQSIT IS 'Situation Code' , 
	KEQSTD IS 'Situation Date' , 
	KEQSTH IS 'Situation Heure' , 
	KEQENVU IS 'Envoi User' , 
	KEQENVD IS 'Envoi Date' , 
	KEQENVH IS 'Envoi Heure' , 
	KEQTEDI IS 'Originale,Copie,Dupl' , 
	KEQORID IS 'Lien KPDOC original' , 
	KEQTYDS IS 'Type de destinataire' , 
	KEQTYI IS 'Type intervenant' , 
	KEQIDS IS 'Identifiant destinat' , 
	KEQNBEX IS 'nb exemplaires supp' , 
	KEQCRU IS 'Création user' , 
	KEQCRD IS 'Création Date' , 
	KEQCRH IS 'Création Heure' , 
	KEQMAJU IS 'Maj User' , 
	KEQMAJD IS 'Maj Date' , 
	KEQMAJH IS 'Maj Heure' , 
	KEQSTG IS 'Statut génération' , 
	KEQDIMP IS 'Imprimable O/N' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPDOCH 
( KEQID TEXT IS 'ID unique' , 
	KEQTYP TEXT IS 'Type O/P' , 
	KEQIPB TEXT IS 'IPB' , 
	KEQALX TEXT IS 'ALX' , 
	KEQSUA TEXT IS 'Sinistre AA' , 
	KEQNUM TEXT IS 'Sinistre N°' , 
	KEQSBR TEXT IS 'Sinistre Sbr' , 
	KEQSERV TEXT IS 'Service  (Produ,Sinistre...)' , 
	KEQACTG TEXT IS 'Acte de gestion' , 
	KEQACTN TEXT IS 'N° acte de gestion' , 
	KEQECO TEXT IS 'En cours O/N' , 
	KEQAVN TEXT IS 'N° Avenant' , 
	KEQETAP TEXT IS 'Etape' , 
	KEQKEMID TEXT IS 'Lien KALCELD' , 
	KEQORD TEXT IS 'N° Ordre' , 
	KEQTGL TEXT IS 'Type de Génération Généré ou Libre' , 
	KEQTDOC TEXT IS 'Type Document LETTYP LIBRE CP .....' , 
	KEQKESID TEXT IS 'Si Complément   Lien KPDOCTX Texte' , 
	KEQAJT TEXT IS 'Ajouté O/N' , 
	KEQTRS TEXT IS 'Transformé O/N' , 
	KEQMAIT TEXT IS 'Table Maître ou Origine (KCLAUSE...' , 
	KEQLIEN TEXT IS 'Lien table maître/Origine (KCLAUSE..' , 
	KEQCDOC TEXT IS 'Code document (si lettyp (3 x 5))' , 
	KEQVER TEXT IS 'N°Version' , 
	KEQLIB TEXT IS 'Libellé' , 
	KEQNTA TEXT IS 'Nature de la génération (O/P/S)' , 
	KEQDACC TEXT IS 'Document Accompagnant O/N' , 
	KEQTAE TEXT IS 'Action enchainée' , 
	KEQNOM TEXT IS 'Nom du document' , 
	KEQCHM TEXT IS 'Chemin complet' , 
	KEQSTU TEXT IS 'Situation User' , 
	KEQSIT TEXT IS 'Situation Code' , 
	KEQSTD TEXT IS 'Situation Date' , 
	KEQSTH TEXT IS 'Situation Heure' , 
	KEQENVU TEXT IS 'Envoi User' , 
	KEQENVD TEXT IS 'Envoi Date' , 
	KEQENVH TEXT IS 'Envoi Heure' , 
	KEQTEDI TEXT IS 'Typo Edit Originale,Copie,Duplicata' , 
	KEQORID TEXT IS 'Lien KPDOC original si Copie/Dup' , 
	KEQTYDS TEXT IS 'Type de destinataire sur document' , 
	KEQTYI TEXT IS 'Type intervenant sur document' , 
	KEQIDS TEXT IS 'Id destinataire sur document' , 
	KEQINL TEXT IS 'Code interlocuteur sur document' , 
	KEQNBEX TEXT IS 'Nombre d''exemplaires supplémentaires' , 
	KEQCRU TEXT IS 'Création User' , 
	KEQCRD TEXT IS 'Création Date' , 
	KEQCRH TEXT IS 'Création Heure' , 
	KEQMAJU TEXT IS 'Maj User' , 
	KEQMAJD TEXT IS 'Maj Date' , 
	KEQMAJH TEXT IS 'Maj Heure' , 
	KEQSTG TEXT IS 'Statut génération G à jour/ Modif ap' , 
	KEQDIMP TEXT IS 'Imprimable  O/N' ) ; 
  
