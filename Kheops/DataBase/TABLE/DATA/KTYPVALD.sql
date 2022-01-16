﻿CREATE TABLE ZALBINKHEO.KTYPVALD ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KTYPVALD de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KTYPVALD de ZALBINKHEO. 
	KGMID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KGMTYVAL CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KGMBASE CHAR(5) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FTYPVALD   ; 
  
LABEL ON TABLE ZALBINKHEO.KTYPVALD 
	IS 'Référentiel TypeValeur lgn                     KGM' ; 
  
LABEL ON COLUMN ZALBINKHEO.KTYPVALD 
( KGMID IS 'ID unique' , 
	KGMTYVAL IS 'TYpe de valeur grp' , 
	KGMBASE IS 'Type  Valeur (Base)' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KTYPVALD 
( KGMID TEXT IS 'ID unique' , 
	KGMTYVAL TEXT IS 'Type de valeur groupé  lien KTYPVAL' , 
	KGMBASE TEXT IS 'Type valeur (Base)' ) ; 
  