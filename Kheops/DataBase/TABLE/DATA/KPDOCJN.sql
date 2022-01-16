﻿CREATE TABLE ZALBINKHEO.KPDOCJN ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPDOCJN de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPDOCJN de ZALBINKHEO. 
	KETID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KETKEQID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KETORD NUMERIC(5, 2) NOT NULL DEFAULT 0 , 
	KETKGWID NUMERIC(15, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPDOCJN    ; 
  
LABEL ON TABLE ZALBINKHEO.KPDOCJN 
	IS 'Document Doc joints                            KET' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPDOCJN 
( KETID IS 'ID unique' , 
	KETKEQID IS 'Lien KDOC Document' , 
	KETORD IS 'N° Ordre' , 
	KETKGWID IS 'Lien KPELJN' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPDOCJN 
( KETID TEXT IS 'ID unique' , 
	KETKEQID TEXT IS 'Lien KDOC  Document' , 
	KETORD TEXT IS 'N° ordre' , 
	KETKGWID TEXT IS 'Lien KPELJN' ) ; 
  