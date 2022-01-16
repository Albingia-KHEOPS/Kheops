﻿CREATE TABLE ZALBINKHEO.HPIOPT ( 
--  SQL150B   10   REUSEDLT(*NO) de la table HPIOPT de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour HPIOPT de ZALBINKHEO. 
	KFCID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KFCTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFCIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KFCALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFCALX. 
	KFCAVN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KFCHIN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KFCFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KFCOPT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KFCKDBID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KFCRRCR CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFCRRC CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFCMNTE NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KFCSEUI NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KFCSEUR NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KFCSEUC NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KFCPERR CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFCAUTM CHAR(60) CCSID 297 NOT NULL DEFAULT '' , 
	KFCCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KFCCRH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KFCMAJU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KFCMAJD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KFCMAJH NUMERIC(6, 0) NOT NULL DEFAULT 0 )   
	RCDFMT HPIOPT     ; 
  
LABEL ON TABLE ZALBINKHEO.HPIOPT 
	IS 'KHEOPS Hist Inf Spé Option KFC' ; 
  
LABEL ON COLUMN ZALBINKHEO.HPIOPT 
( KFCID IS 'ID unique' , 
	KFCTYP IS 'Type O/P' , 
	KFCIPB IS 'IPB/ALX' , 
	KFCALX IS 'Aliment/Version' , 
	KFCAVN IS 'N° avenant' , 
	KFCHIN IS 'N° histo par avenant' , 
	KFCFOR IS 'Formule' , 
	KFCOPT IS 'Option' , 
	KFCKDBID IS 'Lien KPOPT' , 
	KFCRRCR IS 'Ren Recours Réciproq' , 
	KFCRRC IS 'Renonciat° Recours' , 
	KFCMNTE IS 'EDUCA Mnt/Elève' , 
	KFCSEUI IS 'EDUCA Seuil interru' , 
	KFCSEUR IS 'EDUCA Seuil Redoubl' , 
	KFCSEUC IS 'EDUCA Seuil Cours P' , 
	KFCPERR IS 'EDUCA Perte revenus' , 
	KFCAUTM IS 'Autre Motif' , 
	KFCCRD IS 'Création Date' , 
	KFCCRH IS 'Création Heure' , 
	KFCMAJU IS 'Maj User' , 
	KFCMAJD IS 'Maj Date' , 
	KFCMAJH IS 'Maj Heure' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.HPIOPT 
( KFCID TEXT IS 'ID unique' , 
	KFCTYP TEXT IS 'Type O/P' , 
	KFCIPB TEXT IS 'IPB/ALX' , 
	KFCALX TEXT IS 'Aliment/version' , 
	KFCAVN TEXT IS 'N° avenant' , 
	KFCHIN TEXT IS 'N° histo par avenant' , 
	KFCFOR TEXT IS 'Formule' , 
	KFCOPT TEXT IS 'Option' , 
	KFCKDBID TEXT IS 'Lien KPOPT' , 
	KFCRRCR TEXT IS 'Renonciation à Recours Réciproque' , 
	KFCRRC TEXT IS 'Renonciation à recours' , 
	KFCMNTE TEXT IS 'EDUCA : Montant par élève' , 
	KFCSEUI TEXT IS 'EDUCA Seuil interruption' , 
	KFCSEUR TEXT IS 'EDUCA Seuil  Redoublement' , 
	KFCSEUC TEXT IS 'EDUCA Seuil Cours particuliers' , 
	KFCPERR TEXT IS 'EDUCA Perte de revenus' , 
	KFCAUTM TEXT IS 'Autre motif   ex lien KPIDESI' , 
	KFCCRD TEXT IS 'Création Date' , 
	KFCCRH TEXT IS 'Création Heure' , 
	KFCMAJU TEXT IS 'Maj User' , 
	KFCMAJD TEXT IS 'Maj Date' , 
	KFCMAJH TEXT IS 'Maj Heure' ) ; 
  
