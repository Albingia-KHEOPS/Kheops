CREATE TABLE ZALBINKHEO.KGARFAM ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KGARFAM de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KGARFAM de ZALBINKHEO. 
	GVGAR CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	GVBRA CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	GVSBR CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	GVCAT CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	GVFAM CHAR(6) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FGARFAM    ; 
  
LABEL ON TABLE ZALBINKHEO.KGARFAM 
	IS 'Garantie : Famille réassurance                  GV' ; 
  
LABEL ON COLUMN ZALBINKHEO.KGARFAM 
( GVGAR IS 'Code garantie' , 
	GVBRA IS 'Branche' , 
	GVSBR IS 'Sous-branche' , 
	GVCAT IS 'Catégorie' , 
	GVFAM IS 'Famille Garantie' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KGARFAM 
( GVGAR TEXT IS 'Code garantie' , 
	GVBRA TEXT IS 'Branche' , 
	GVSBR TEXT IS 'Sous-branche' , 
	GVCAT TEXT IS 'Catégorie' , 
	GVFAM TEXT IS 'Famille Garantie' ) ; 
  
