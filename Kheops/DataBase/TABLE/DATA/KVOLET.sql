﻿CREATE TABLE ZALBINKHEO.KVOLET ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KVOLET de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KVOLET de ZALBINKHEO. 
	KAKID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KAKVOLET CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KAKDESC CHAR(60) CCSID 297 NOT NULL DEFAULT '' , 
	KAKCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KAKCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KAKCRH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KAKMAJU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KAKMAJD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KAKMAJH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KAKBRA CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KAKFGEN CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KAKPRES CHAR(2) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FVOLET     ; 
  
LABEL ON TABLE ZALBINKHEO.KVOLET 
	IS 'KHEOPS Volet  Référentiel' ; 
  
LABEL ON COLUMN ZALBINKHEO.KVOLET 
( KAKID IS 'Id unique Volet' , 
	KAKVOLET IS 'Volet' , 
	KAKDESC IS 'Description' , 
	KAKCRU IS 'Création user' , 
	KAKCRD IS 'Date création' , 
	KAKCRH IS 'Création Heure' , 
	KAKMAJU IS 'Maj User' , 
	KAKMAJD IS 'Maj date' , 
	KAKMAJH IS 'MAJ Heure' , 
	KAKBRA IS 'Branche' , 
	KAKFGEN IS 'Formule générale O/N' , 
	KAKPRES IS 'Présentat° / défaut' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KVOLET 
( KAKID TEXT IS 'ID unique Volet' , 
	KAKVOLET TEXT IS 'Volet' , 
	KAKDESC TEXT IS 'Description' , 
	KAKCRU TEXT IS 'Création User' , 
	KAKCRD TEXT IS 'Date création' , 
	KAKCRH TEXT IS 'Création heure' , 
	KAKMAJU TEXT IS 'Maj user' , 
	KAKMAJD TEXT IS 'Maj Date' , 
	KAKMAJH TEXT IS 'MAJ Heure' , 
	KAKBRA TEXT IS 'Branche' , 
	KAKFGEN TEXT IS 'O/N si O  => dans Formule générale' , 
	KAKPRES TEXT IS 'Présentation  par défaut' ) ; 
  