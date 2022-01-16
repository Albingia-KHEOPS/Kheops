CREATE TABLE ZALBINKHEO.YPOASSX ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPOASSX de ZALBINKHEO ignoré. 
	PDTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PDIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	PDALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne PDALX. 
	PDQL1 CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	PDQL2 CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	PDQL3 CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	PDQLD CHAR(40) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPOASSX    ; 
  
LABEL ON TABLE ZALBINKHEO.YPOASSX 
	IS 'Assurés non désignés                            PD' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOASSX 
( PDTYP IS 'Type' , 
	PDIPB IS 'N° de police / Offre' , 
	PDALX IS 'N° Aliment / connexe' , 
	PDQL1 IS 'Qualité 1' , 
	PDQL2 IS 'Qualité 2' , 
	PDQL3 IS 'Qualité 3' , 
	PDQLD IS 'Qualité Autre' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOASSX 
( PDTYP TEXT IS 'Type  O Offre  P Police  E à établir' , 
	PDIPB TEXT IS 'N° de Police / Offre' , 
	PDALX TEXT IS 'N° Aliment ou Connexe' , 
	PDQL1 TEXT IS 'Qualité 1 de l''assuré' , 
	PDQL2 TEXT IS 'Qualité 2 de l''assuré' , 
	PDQL3 TEXT IS 'Qualité 3 de l''assuré' , 
	PDQLD TEXT IS 'Qualité Autre' ) ; 
  
