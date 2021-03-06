CREATE TABLE ZALBINKHEO.KDOCEMP ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KDOCEMP de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KDOCEMP de ZALBINKHEO. 
	KDVID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDVORI CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDVEMP CHAR(200) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FDOCEMP    ; 
  
LABEL ON TABLE ZALBINKHEO.KDOCEMP 
	IS 'Désignation :  Documents Emplacement' ; 
  
LABEL ON COLUMN ZALBINKHEO.KDOCEMP 
( KDVID IS 'ID unique' , 
	KDVORI IS 'Origine' , 
	KDVEMP IS 'Emplacement' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KDOCEMP 
( KDVID TEXT IS 'ID unique' , 
	KDVORI TEXT IS 'Origine (Clause ....)' , 
	KDVEMP TEXT IS 'Emplacement' ) ; 
  
