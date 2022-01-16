﻿CREATE TABLE ZALBINKHEO.KJOBSORTI ( 
	IPB CHAR(9) CCSID 297 DEFAULT NULL , 
	ALX NUMERIC(4, 0) DEFAULT NULL , 
	TYP CHAR(1) CCSID 297 DEFAULT NULL , 
	AVN NUMERIC(3, 0) DEFAULT NULL , 
	RSQ NUMERIC(5, 0) DEFAULT NULL , 
	OBJ NUMERIC(5, 0) DEFAULT NULL , 
	FORM NUMERIC(5, 0) DEFAULT NULL , 
	OPT NUMERIC(5, 0) DEFAULT NULL , 
	GARAN NUMERIC(15, 0) DEFAULT NULL , 
	DATEDEB NUMERIC(8, 0) DEFAULT NULL , 
	HEUREDEB NUMERIC(4, 0) DEFAULT NULL , 
	DATEFIN NUMERIC(8, 0) DEFAULT NULL , 
	HEUREFIN NUMERIC(4, 0) DEFAULT NULL , 
	SORTI CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT KJOBSORTI  ; 
  
LABEL ON TABLE ZALBINKHEO.KJOBSORTI 
	IS '' ; 
  
LABEL ON COLUMN ZALBINKHEO.KJOBSORTI 
( DATEFIN TEXT IS 'DATEFIN' , 
	HEUREFIN TEXT IS 'HEUREFIN' ) ; 
  
