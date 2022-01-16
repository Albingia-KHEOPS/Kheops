﻿CREATE TABLE ZALBINKHEO.KGARTVL ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KGARTVL de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KGARTVL de ZALBINKHEO. 
	KGKID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KGKGAR CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KGKORD NUMERIC(5, 2) NOT NULL DEFAULT 0 , 
	KGKTYVAL CHAR(5) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FGARTVL    ; 
  
LABEL ON TABLE ZALBINKHEO.KGARTVL 
	IS 'Garantie : Type de valeur                      KGK' ; 
  
LABEL ON COLUMN ZALBINKHEO.KGARTVL 
( KGKID IS 'Id Unique' , 
	KGKGAR IS 'Code garantie' , 
	KGKORD IS 'N° Ordre' , 
	KGKTYVAL IS 'Lien KTYPVAL' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KGARTVL 
( KGKID TEXT IS 'ID unique' , 
	KGKGAR TEXT IS 'Code garantie' , 
	KGKORD TEXT IS 'N° Ordre' , 
	KGKTYVAL TEXT IS 'Type de valeur Lien KTYPVAL' ) ; 
  
