﻿CREATE TABLE ZALBINKHEO.KEXPLCI ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KEXPLCI de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KEXPLCI de ZALBINKHEO. 
	KHGID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHGLCE CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KHGDESC CHAR(60) CCSID 297 NOT NULL DEFAULT '' , 
	KHGDESI NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHGMODI CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FEXPLCI    ; 
  
LABEL ON TABLE ZALBINKHEO.KEXPLCI 
	IS 'Référ Exp complexe LCI                         KHG' ; 
  
LABEL ON COLUMN ZALBINKHEO.KEXPLCI 
( KHGID IS 'ID unique' , 
	KHGLCE IS 'Expression complexe' , 
	KHGDESC IS 'Description' , 
	KHGDESI IS 'Lien KDESI' , 
	KHGMODI IS 'Modifiable O/N' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KEXPLCI 
( KHGID TEXT IS 'ID unique' , 
	KHGLCE TEXT IS 'Expression complexe' , 
	KHGDESC TEXT IS 'Description' , 
	KHGDESI TEXT IS 'Lien KDESI' , 
	KHGMODI TEXT IS 'Modifiable O/N' ) ; 
  