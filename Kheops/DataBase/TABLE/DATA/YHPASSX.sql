CREATE TABLE ZALBINKHEO.YHPASSX ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YHPASSX de ZALBINKHEO ignoré. 
	PDIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	PDALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne PDALX. 
	PDAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	PDHIN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	PDQL1 CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	PDQL2 CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	PDQL3 CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	PDQLD CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	PDTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FHPASSX    ; 
  
LABEL ON TABLE ZALBINKHEO.YHPASSX 
	IS 'Histo Polices : Ass. Non D.PD' ; 
  
LABEL ON COLUMN ZALBINKHEO.YHPASSX 
( PDIPB IS 'N° de police' , 
	PDALX IS 'N° Aliment' , 
	PDAVN IS 'N° avenant' , 
	PDHIN IS 'N° historique avenan' , 
	PDQL1 IS 'Qualité 1' , 
	PDQL2 IS 'Qualité 2' , 
	PDQL3 IS 'Qualité 3' , 
	PDQLD IS 'Qualité Autre' , 
	PDTYP IS 'Type' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YHPASSX 
( PDIPB TEXT IS 'N° de Police' , 
	PDALX TEXT IS 'N° Aliment' , 
	PDAVN TEXT IS 'N° avenant' , 
	PDHIN TEXT IS 'N° historique par avenant' , 
	PDQL1 TEXT IS 'Qualité 1 de l''assuré' , 
	PDQL2 TEXT IS 'Qualité 2 de l''assuré' , 
	PDQL3 TEXT IS 'Qualité 3 de l''assuré' , 
	PDQLD TEXT IS 'Qualité Autre' , 
	PDTYP TEXT IS 'Type  O Offre  P Police  E à établir' ) ; 
  
