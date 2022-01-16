CREATE TABLE ZALBINKHEO.YPCONUM ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPCONUM de ZALBINKHEO ignoré. 
	FCBRA CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	FCSBR CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	FCSAA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	FCBUR CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	FCIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPCONUM    ; 
  
LABEL ON TABLE ZALBINKHEO.YPCONUM 
	IS 'Poli.CO : N° Police en attente                  FC' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPCONUM 
( FCBRA IS 'Branche' , 
	FCSBR IS 'Sous-branche' , 
	FCSAA IS 'Année Date accord' , 
	FCBUR IS 'Code Bureau Souscrip' , 
	FCIPB IS 'N° de police attente' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPCONUM 
( FCBRA TEXT IS 'Branche' , 
	FCSBR TEXT IS 'Sous-branche' , 
	FCSAA TEXT IS 'Année Date accord' , 
	FCBUR TEXT IS 'Code Bureau Souscripteur' , 
	FCIPB TEXT IS 'N° Police en attente attribution' ) ; 
  
