﻿CREATE TABLE ZALBINKHEO.KPEXCEL ( 
	KGUTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KGUIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KGUALX INTEGER NOT NULL DEFAULT 0 , 
	KGUDT1 VARCHAR(5000) CCSID 297 NOT NULL DEFAULT '' , 
	KGUDT2 VARCHAR(5000) CCSID 297 NOT NULL DEFAULT '' , 
	KGUDT3 VARCHAR(5000) CCSID 297 NOT NULL DEFAULT '' , 
	KGUDT4 VARCHAR(5000) CCSID 297 NOT NULL DEFAULT '' , 
	KGUDT5 VARCHAR(5000) CCSID 297 NOT NULL DEFAULT '' , 
	CONSTRAINT ZALBINKHEO.KPEXCELIDX PRIMARY KEY( KGUTYP , KGUIPB , KGUALX ) )   
	RCDFMT KPEXCEL    ; 
  
