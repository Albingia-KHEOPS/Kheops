CREATE TABLE ZALBINKHEO.YPRTADR ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPRTADR de ZALBINKHEO ignoré. 
	JFIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	JFALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JFALX. 
	JFRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JFRSQ. 
	JFOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JFOBJ. 
	JFAD1 CHAR(32) CCSID 297 NOT NULL DEFAULT '' , 
	JFAD2 CHAR(32) CCSID 297 NOT NULL DEFAULT '' , 
	JFDEP CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	JFCPO CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	JFVIL CHAR(26) CCSID 297 NOT NULL DEFAULT '' , 
	JFPAY CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	JFADH DECIMAL(7, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPRTADR    ; 
  
LABEL ON TABLE ZALBINKHEO.YPRTADR 
	IS 'Poli.RT : Adresse risque                        JF' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTADR 
( JFIPB IS 'N° de police' , 
	JFALX IS 'N° Aliment' , 
	JFRSQ IS 'Identifiant risque' , 
	JFOBJ IS 'Identifiant objet' , 
	JFAD1 IS 'Adresse' , 
	JFAD2 IS 'Adresse' , 
	JFDEP IS 'Département' , 
	JFCPO IS 'Code postal 3 car' , 
	JFVIL IS 'Ville' , 
	JFPAY IS 'Code pays' , 
	JFADH IS 'N° Chrono Adresse' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTADR 
( JFIPB TEXT IS 'N° de Police' , 
	JFALX TEXT IS 'N° Aliment' , 
	JFRSQ TEXT IS 'Identifiant risque' , 
	JFOBJ TEXT IS 'Identifiant objet' , 
	JFAD1 TEXT IS 'Adresse' , 
	JFAD2 TEXT IS 'Adresse' , 
	JFDEP TEXT IS 'Département' , 
	JFCPO TEXT IS '3 derniers caractères code postal' , 
	JFVIL TEXT IS 'Ville' , 
	JFPAY TEXT IS 'Code pays' , 
	JFADH TEXT IS 'Numéro chrono Adresse' ) ; 
  
