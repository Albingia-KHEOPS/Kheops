CREATE TABLE ZALBINKHEO.YHRTFOO ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YHRTFOO de ZALBINKHEO ignoré. 
	JPIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	JPALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JPALX. 
	JPAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	JPHIN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	JPRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JPRSQ. 
	JPFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	JPOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FHRTFOO    ; 
--  SQL150D   10   EDTCDE ignoré pour la colonne JPOBJ. 
LABEL ON TABLE ZALBINKHEO.YHRTFOO 
	IS 'H-Poli.RT:Objets/formule                        JP' ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTFOO 
( JPIPB IS 'N° de police' , 
	JPALX IS 'N° Aliment' , 
	JPAVN IS 'N° avenant' , 
	JPHIN IS 'N° historique avenan' , 
	JPRSQ IS 'Identifiant risque' , 
	JPFOR IS 'Identifiant formule' , 
	JPOBJ IS 'Identifiant objet' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTFOO 
( JPIPB TEXT IS 'N° de Police' , 
	JPALX TEXT IS 'N° Aliment' , 
	JPAVN TEXT IS 'N° avenant' , 
	JPHIN TEXT IS 'N° historique par avenant' , 
	JPRSQ TEXT IS 'Identifiant Risque' , 
	JPFOR TEXT IS 'Identifiant formule' , 
	JPOBJ TEXT IS 'Id objet' ) ; 
  
