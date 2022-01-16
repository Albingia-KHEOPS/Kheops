CREATE TABLE ZALBINKHEO.YPRTFOO ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPRTFOO de ZALBINKHEO ignoré. 
	JPIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	JPALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JPALX. 
	JPRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JPRSQ. 
	JPFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	JPOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPRTFOO    ; 
--  SQL150D   10   EDTCDE ignoré pour la colonne JPOBJ. 
LABEL ON TABLE ZALBINKHEO.YPRTFOO 
	IS 'Poli.RT : Objets/Formule                        JP' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTFOO 
( JPIPB IS 'N° de police' , 
	JPALX IS 'N° Aliment' , 
	JPRSQ IS 'Identifiant risque' , 
	JPFOR IS 'Identifiant formule' , 
	JPOBJ IS 'Identifiant objet' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTFOO 
( JPIPB TEXT IS 'N° de Police' , 
	JPALX TEXT IS 'N° Aliment' , 
	JPRSQ TEXT IS 'Identifiant Risque' , 
	JPFOR TEXT IS 'Identifiant formule' , 
	JPOBJ TEXT IS 'Id objet' ) ; 
  
