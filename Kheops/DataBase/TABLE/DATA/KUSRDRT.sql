CREATE TABLE ZALBINKHEO.KUSRDRT ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KUSRDRT de ZALBINKHEO ignoré. 
	KHRUSR CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KHRBRA CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHRTYD CHAR(3) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FUSRDRT    ; 
  
LABEL ON TABLE ZALBINKHEO.KUSRDRT 
	IS 'USER DROITS ACCES GROUPE                       KHR' ; 
  
LABEL ON COLUMN ZALBINKHEO.KUSRDRT 
( KHRUSR IS 'User' , 
	KHRBRA IS 'Branche' , 
	KHRTYD IS 'Type de droit' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KUSRDRT 
( KHRUSR TEXT IS 'User' , 
	KHRBRA TEXT IS 'Branche' , 
	KHRTYD TEXT IS 'Type de droit' ) ; 
  
