CREATE TABLE ZALBINKHEO.YPRTDES ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPRTDES de ZALBINKHEO ignoré. 
	JIIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	JIALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JIALX. 
	JIOJR CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JIRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JIRSQ. 
	JIOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JIOBJ. 
	JIORD NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JIORD. 
	JILDS CHAR(60) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPRTDES    ; 
  
LABEL ON TABLE ZALBINKHEO.YPRTDES 
	IS 'Poli.RT : Désignation                           JI' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTDES 
( JIIPB IS 'N° de police' , 
	JIALX IS 'N° Aliment' , 
	JIOJR IS 'Entête/Risque/Objet' , 
	JIRSQ IS 'Identifiant risque' , 
	JIOBJ IS 'Identifiant objet' , 
	JIORD IS 'N° de texte' , 
	JILDS IS 'Ligne de désignation' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTDES 
( JIIPB TEXT IS 'N° de Police' , 
	JIALX TEXT IS 'N° Aliment' , 
	JIOJR TEXT IS 'E:Entête R:Risque O:Objet' , 
	JIRSQ TEXT IS 'Identifiant risque' , 
	JIOBJ TEXT IS 'Identifiant objet' , 
	JIORD TEXT IS 'N° de texte' , 
	JILDS TEXT IS 'Ligne de désignation' ) ; 
  
