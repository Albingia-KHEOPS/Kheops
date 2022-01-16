﻿CREATE TABLE ZALBINKHEO.KCIBLE ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KCIBLE de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KCIBLE de ZALBINKHEO. 
	KAHID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KAHCIBLE CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KAHDESC CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KAHCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KAHCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KAHCRH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KAHMAJU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KAHMAJD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KAHMAJH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KAHNMG CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KAHCON CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KAHFAM CHAR(5) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FCIBLE     ; 
  
LABEL ON TABLE ZALBINKHEO.KCIBLE 
	IS 'KHEOPS Réf Cible' ; 
  
LABEL ON COLUMN ZALBINKHEO.KCIBLE 
( KAHID IS 'ID unique' , 
	KAHCIBLE IS 'Cible' , 
	KAHDESC IS 'Description' , 
	KAHCRU IS 'Création User' , 
	KAHCRD IS 'Création Date' , 
	KAHCRH IS 'Création Heure' , 
	KAHMAJU IS 'Maj User' , 
	KAHMAJD IS 'Maj Date' , 
	KAHMAJH IS 'Maj Heure' , 
	KAHNMG IS 'Nomenclature Grille' , 
	KAHCON IS 'Activité: Concept' , 
	KAHFAM IS 'Activité Famille' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KCIBLE 
( KAHID TEXT IS 'ID unique' , 
	KAHCIBLE TEXT IS 'Cible' , 
	KAHDESC TEXT IS 'Description' , 
	KAHCRU TEXT IS 'Création User' , 
	KAHCRD TEXT IS 'Création Date' , 
	KAHCRH TEXT IS 'Création heure' , 
	KAHMAJU TEXT IS 'Maj User' , 
	KAHMAJD TEXT IS 'Maj Date' , 
	KAHMAJH TEXT IS 'Maj Heure' , 
	KAHNMG TEXT IS 'Nomenclature : grille' , 
	KAHCON TEXT IS 'Activité : Concept' , 
	KAHFAM TEXT IS 'Activité : Famille' ) ; 
  
