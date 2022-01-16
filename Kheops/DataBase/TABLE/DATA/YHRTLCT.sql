CREATE TABLE ZALBINKHEO.YHRTLCT ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YHRTLCT de ZALBINKHEO ignoré. 
	JMIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	JMALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JMALX. 
	JMAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	JMHIN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	JMLCE CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	JMORD NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JMTXT CHAR(70) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FHRTLCT    ; 
  
LABEL ON TABLE ZALBINKHEO.YHRTLCT 
	IS 'H-Poli.RT:LCI texte libre                       JM' ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTLCT 
( JMIPB IS 'N° de police' , 
	JMALX IS 'N° Aliment' , 
	JMAVN IS 'N° avenant' , 
	JMHIN IS 'N° historique avenan' , 
	JMLCE IS 'Expression LCI' , 
	JMORD IS 'N° ordre' , 
	JMTXT IS 'Texte' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTLCT 
( JMIPB TEXT IS 'N° de Police' , 
	JMALX TEXT IS 'N° Aliment' , 
	JMAVN TEXT IS 'N° avenant' , 
	JMHIN TEXT IS 'N° historique par avenant' , 
	JMLCE TEXT IS 'Expression complexe LCI' , 
	JMORD TEXT IS 'N° ordre texte' , 
	JMTXT TEXT IS 'Texte' ) ; 
  
