﻿CREATE TABLE ZALBINKHEO.KPOBSV ( 
	KAJCHR INTEGER NOT NULL DEFAULT 0 , 
	KAJTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KAJIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KAJALX INTEGER NOT NULL DEFAULT 0 , 
	KAJTYPOBS CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KAJOBSV VARCHAR(5000) CCSID 297 NOT NULL DEFAULT '' , 
	CONSTRAINT ZALBINKHEO.KPOBSVIDX PRIMARY KEY( KAJCHR ) )   
	RCDFMT KPOBSV     ; 
  
