CREATE TABLE ZALBINKHEO.YBUREAU ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YBUREAU de ZALBINKHEO ignoré. 
	BUIBU CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	BUDBU CHAR(20) CCSID 297 NOT NULL DEFAULT '' , 
	BUNOM CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	BUAD1 CHAR(32) CCSID 297 NOT NULL DEFAULT '' , 
	BUAD2 CHAR(32) CCSID 297 NOT NULL DEFAULT '' , 
	BUDEP CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	BUCPO CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	BUVIL CHAR(26) CCSID 297 NOT NULL DEFAULT '' , 
	BUPAY CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	BUCOM CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	BUTEL CHAR(13) CCSID 297 NOT NULL DEFAULT '' , 
	BUTLC CHAR(13) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FBUREAU    ; 
  
LABEL ON TABLE ZALBINKHEO.YBUREAU 
	IS 'Bureaux                                         BU' ; 
  
LABEL ON COLUMN ZALBINKHEO.YBUREAU 
( BUIBU IS 'Code Bureau' , 
	BUDBU IS 'Désignation bureau' , 
	BUNOM IS 'Nom' , 
	BUAD1 IS 'Adresse 1' , 
	BUAD2 IS 'Adresse 2' , 
	BUDEP IS 'Département' , 
	BUCPO IS 'Code postal 3 car' , 
	BUVIL IS 'Ville' , 
	BUPAY IS 'Code pays' , 
	BUCOM IS 'Code commune' , 
	BUTEL IS 'Téléphone' , 
	BUTLC IS 'Télécopie' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YBUREAU 
( BUIBU TEXT IS 'Code Bureau' , 
	BUDBU TEXT IS 'Désignation bureau' , 
	BUNOM TEXT IS 'Nom' , 
	BUAD1 TEXT IS 'Adresse 1' , 
	BUAD2 TEXT IS 'Adresse 2' , 
	BUDEP TEXT IS 'Département' , 
	BUCPO TEXT IS '3 dernier caractères code postal' , 
	BUVIL TEXT IS 'Ville' , 
	BUPAY TEXT IS 'Code pays' , 
	BUCOM TEXT IS 'Code commune Arrondissement' , 
	BUTEL TEXT IS 'Téléphone' , 
	BUTLC TEXT IS 'Télécopie' ) ; 
  
