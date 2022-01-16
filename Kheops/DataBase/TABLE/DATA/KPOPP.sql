﻿CREATE TABLE ZALBINKHEO.KPOPP ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPOPP de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPOPP de ZALBINKHEO. 
	KFPID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KFPTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFPIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KFPALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KFPIDCB NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KFPTFI CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KFPDESI NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KFPREF CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KFPDECH NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KFPMNT NUMERIC(15, 2) NOT NULL DEFAULT 0 , 
	KFPCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KFPCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KFPCRH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KFPMAJU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KFPMAJD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KFPMAJH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KFPTDS CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KFPTYI CHAR(2) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPOPP      ; 
  
LABEL ON TABLE ZALBINKHEO.KPOPP 
	IS 'KHEOPS Opposition                              KFP' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPOPP 
( KFPID IS 'ID unique' , 
	KFPTYP IS 'Type O/P' , 
	KFPIPB IS 'IPB/ALX' , 
	KFPALX IS 'Aliment/Version' , 
	KFPIDCB IS 'ID Crédit bailleur' , 
	KFPTFI IS 'Type financement' , 
	KFPDESI IS 'Lien KPDESI' , 
	KFPREF IS 'Référence' , 
	KFPDECH IS 'Date échéance' , 
	KFPMNT IS 'Montant financé' , 
	KFPCRU IS 'Création User' , 
	KFPCRD IS 'Création Date' , 
	KFPCRH IS 'Création Heure' , 
	KFPMAJU IS 'Maj User' , 
	KFPMAJD IS 'Maj Date' , 
	KFPMAJH IS 'Maj Heure' , 
	KFPTDS IS 'Type de destinataire' , 
	KFPTYI IS 'Type intervenant' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPOPP 
( KFPID TEXT IS 'ID unique' , 
	KFPTYP TEXT IS 'Type O/P' , 
	KFPIPB TEXT IS 'IPB/ALX' , 
	KFPALX TEXT IS 'Aliment/version' , 
	KFPIDCB TEXT IS 'ID crédit bailleur' , 
	KFPTFI TEXT IS 'Type de financement' , 
	KFPDESI TEXT IS 'lien KPDESI' , 
	KFPREF TEXT IS 'Référence' , 
	KFPDECH TEXT IS 'Date échéance' , 
	KFPMNT TEXT IS 'Montant financé' , 
	KFPCRU TEXT IS 'Création user' , 
	KFPCRD TEXT IS 'Création Date' , 
	KFPCRH TEXT IS 'Création Heure' , 
	KFPMAJU TEXT IS 'Maj User' , 
	KFPMAJD TEXT IS 'Maj Date' , 
	KFPMAJH TEXT IS 'Maj Heure' , 
	KFPTDS TEXT IS 'Type de destinataire(Assuré...)' , 
	KFPTYI TEXT IS 'Type intervenant (Cie Expert Avoc..)' ) ; 
  
