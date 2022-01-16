CREATE TABLE ZALBINKHEO.KGANPAR ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KGANPAR de ZALBINKHEO ignoré. 
	KAUCAR CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KAUNAT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KAUAFFI CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KAUMODI CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KAUGANC CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KAUGANNC CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FGANPAR    ; 
  
LABEL ON TABLE ZALBINKHEO.KGANPAR 
	IS 'KHEOPS Param Nature garantie                   KAU' ; 
  
LABEL ON COLUMN ZALBINKHEO.KGANPAR 
( KAUCAR IS 'Caractère' , 
	KAUNAT IS 'Nature' , 
	KAUAFFI IS 'Affichage initial' , 
	KAUMODI IS 'Modifiable O/N' , 
	KAUGANC IS 'Nature si cochée' , 
	KAUGANNC IS 'Nature si non cochée' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KGANPAR 
( KAUCAR TEXT IS 'Caractère' , 
	KAUNAT TEXT IS 'Nature' , 
	KAUAFFI TEXT IS 'Affichage initiale Coché ou non' , 
	KAUMODI TEXT IS 'Modifiable O/N' , 
	KAUGANC TEXT IS 'Nature si garantie cochée' , 
	KAUGANNC TEXT IS 'Nature si garantie non cochée' ) ; 
  
