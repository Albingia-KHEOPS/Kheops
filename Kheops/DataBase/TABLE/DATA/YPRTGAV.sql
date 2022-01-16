CREATE TABLE ZALBINKHEO.YPRTGAV ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPRTGAV de ZALBINKHEO ignoré. 
	JQIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	JQALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JQALX. 
	JQRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JQRSQ. 
	JQFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	JQGAR CHAR(6) CCSID 297 NOT NULL DEFAULT '' , 
	JQRQH CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	JQOBH CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	JQFOH CHAR(8) CCSID 297 NOT NULL DEFAULT '' , 
	JQNAT CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPRTGAV    ; 
  
LABEL ON TABLE ZALBINKHEO.YPRTGAV 
	IS 'Poli.RT : Garantie commune                      JQ' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTGAV 
( JQIPB IS 'N° de police / Offre' , 
	JQALX IS 'N° Aliment / connexe' , 
	JQRSQ IS 'Identifiant risque' , 
	JQFOR IS 'Identifiant formule' , 
	JQGAR IS 'Code garantie' , 
	JQRQH IS 'N° Chrono. Risque   formaté' , 
	JQOBH IS 'N° Chrono. Objet    formaté' , 
	JQFOH IS 'N° Chrono. Formule  formaté' , 
	JQNAT IS 'Nature garantie' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTGAV 
( JQIPB TEXT IS 'N° de Police' , 
	JQALX TEXT IS 'N° Aliment ou Connexe' , 
	JQRSQ TEXT IS 'Identifiant Risque' , 
	JQFOR TEXT IS 'Identifiant formule' , 
	JQGAR TEXT IS 'Code garantie' , 
	JQRQH TEXT IS 'N° Chronologique Risque formaté' , 
	JQOBH TEXT IS 'N° Chronologique Objet  formaté' , 
	JQFOH TEXT IS 'N° Chronologique Formule formaté' , 
	JQNAT TEXT IS 'Nature de la garantie acquise' ) ; 
  
