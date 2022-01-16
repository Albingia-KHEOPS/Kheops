﻿CREATE TABLE ZALBINKHEO.KEXPFRH ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KEXPFRH de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KEXPFRH de ZALBINKHEO. 
	KHEID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHEFHE CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KHEDESC CHAR(60) CCSID 297 NOT NULL DEFAULT '' , 
	KHEDESI NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHEMODI CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FEXPFRH    ; 
  
LABEL ON TABLE ZALBINKHEO.KEXPFRH 
	IS 'Refér Exp Cpx Franchise                        KHE' ; 
  
LABEL ON COLUMN ZALBINKHEO.KEXPFRH 
( KHEID IS 'ID Unique' , 
	KHEFHE IS 'Expression Complexe' , 
	KHEDESC IS 'Description' , 
	KHEDESI IS 'Lien KDESI' , 
	KHEMODI IS 'Modifiable O/N' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KEXPFRH 
( KHEID TEXT IS 'ID unique' , 
	KHEFHE TEXT IS 'Expression Complexe' , 
	KHEDESC TEXT IS 'Description' , 
	KHEDESI TEXT IS 'Lien KDESI' , 
	KHEMODI TEXT IS 'Modifiable O/N' ) ; 
  