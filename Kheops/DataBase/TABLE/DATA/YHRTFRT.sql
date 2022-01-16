CREATE TABLE ZALBINKHEO.YHRTFRT ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YHRTFRT de ZALBINKHEO ignoré. 
	JLIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	JLALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JLALX. 
	JLAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	JLHIN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	JLFHE CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	JLORD NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JLTXT CHAR(70) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FHRTFRT    ; 
  
LABEL ON TABLE ZALBINKHEO.YHRTFRT 
	IS 'H-Poli.RT:Exp Franch Texte' ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTFRT 
( JLIPB IS 'N° de police' , 
	JLALX IS 'N° Aliment' , 
	JLAVN IS 'N° avenant' , 
	JLHIN IS 'N° historique avenan' , 
	JLFHE IS 'Expression complexe Franchise' , 
	JLORD IS 'N° ordre' , 
	JLTXT IS 'Texte' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTFRT 
( JLIPB TEXT IS 'N° de Police' , 
	JLALX TEXT IS 'N° Aliment' , 
	JLAVN TEXT IS 'N° avenant' , 
	JLHIN TEXT IS 'N° historique par avenant' , 
	JLFHE TEXT IS 'Expression complexe Franchise' , 
	JLORD TEXT IS 'N° ordre texte' , 
	JLTXT TEXT IS 'Texte' ) ; 
  
