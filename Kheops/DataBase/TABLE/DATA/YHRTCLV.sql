CREATE TABLE ZALBINKHEO.YHRTCLV ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YHRTCLV de ZALBINKHEO ignoré. 
	QFIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	QFALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne QFALX. 
	QFAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	QFHIN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	QFILR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	QFVAV CHAR(40) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FHRTCLV    ; 
  
LABEL ON TABLE ZALBINKHEO.YHRTCLV 
	IS 'H-Poli.RT:Valeur variable                       QF' ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTCLV 
( QFIPB IS 'N° Police' , 
	QFALX IS 'N° Aliment' , 
	QFAVN IS 'N° avenant' , 
	QFHIN IS 'N° historique avenan' , 
	QFILR IS 'Code variable' , 
	QFVAV IS 'Valeur' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTCLV 
( QFIPB TEXT IS 'N° Police' , 
	QFALX TEXT IS 'N° Aliment' , 
	QFAVN TEXT IS 'N° avenant' , 
	QFHIN TEXT IS 'N° historique par avenant' , 
	QFILR TEXT IS 'Code variable' , 
	QFVAV TEXT IS 'Valeur' ) ; 
  
