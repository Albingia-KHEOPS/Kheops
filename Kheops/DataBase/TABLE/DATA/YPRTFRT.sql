CREATE TABLE ZALBINKHEO.YPRTFRT ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPRTFRT de ZALBINKHEO ignoré. 
	JLIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	JLALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JLALX. 
	JLFHE CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	JLORD NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JLTXT CHAR(70) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPRTFRT    ; 
  
LABEL ON TABLE ZALBINKHEO.YPRTFRT 
	IS 'Poli.RT : Exp Franch texte                      JL' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTFRT 
( JLIPB IS 'N° de police' , 
	JLALX IS 'N° Aliment' , 
	JLFHE IS 'Expression complexe Franchise' , 
	JLORD IS 'N° ordre' , 
	JLTXT IS 'Texte' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTFRT 
( JLIPB TEXT IS 'N° de Police' , 
	JLALX TEXT IS 'N° Aliment' , 
	JLFHE TEXT IS 'Expression complexe Franchise' , 
	JLORD TEXT IS 'N° ordre texte' , 
	JLTXT TEXT IS 'Texte' ) ; 
  
