﻿CREATE TABLE ZALBINKHEO.KNMVALF ( 
	KHKID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHKNMG CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KHKTYPO CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KHKORDR NUMERIC(7, 2) NOT NULL DEFAULT 0 , 
	KHKNIV NUMERIC(1, 0) NOT NULL DEFAULT 0 , 
	KHKMER NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHKKHIID NUMERIC(15, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FNMVALF    ; 
  
LABEL ON TABLE ZALBINKHEO.KNMVALF 
	IS 'Nomenclature Valeur Filtre                     KHK' ; 
  
LABEL ON COLUMN ZALBINKHEO.KNMVALF 
( KHKID IS 'ID Unique' , 
	KHKNMG IS 'Grille' , 
	KHKTYPO IS 'Typologie' , 
	KHKORDR IS 'N° Ordre' , 
	KHKNIV IS 'Niveau de 1 à 5' , 
	KHKMER IS 'Niveau mère' , 
	KHKKHIID IS 'ID KNMREF 1' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KNMVALF 
( KHKID TEXT IS 'ID Unique' , 
	KHKNMG TEXT IS 'Grille' , 
	KHKTYPO TEXT IS 'Typologie' , 
	KHKORDR TEXT IS 'N° Ordre' , 
	KHKNIV TEXT IS 'Niveau  de 1 à 5' , 
	KHKMER TEXT IS 'Niveau mère' , 
	KHKKHIID TEXT IS 'ID KNMREF 1' ) ; 
  
