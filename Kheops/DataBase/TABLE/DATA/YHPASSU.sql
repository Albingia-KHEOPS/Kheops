CREATE TABLE ZALBINKHEO.YHPASSU ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YHPASSU de ZALBINKHEO ignoré. 
	PCIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	PCALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne PCALX. 
	PCAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	PCHIN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	PCIAS NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	PCPRI CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PCQL1 CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	PCQL2 CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	PCQL3 CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	PCQLD CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	PCCNR CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PCASS CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PCSCP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PCTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PCDESI NUMERIC(15, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FHPASSU    ; 
  
LABEL ON TABLE ZALBINKHEO.YHPASSU 
	IS 'Histo Polices : Assurés' ; 
  
LABEL ON COLUMN ZALBINKHEO.YHPASSU 
( PCIPB IS 'N° de police' , 
	PCALX IS 'N° Aliment' , 
	PCAVN IS 'N° avenant' , 
	PCHIN IS 'N° historique avenan' , 
	PCIAS IS 'Identifiant Assuré' , 
	PCPRI IS 'Sousc.principal O/N' , 
	PCQL1 IS 'Qualité 1 assuré' , 
	PCQL2 IS 'Qualité 2 assuré' , 
	PCQL3 IS 'Qualité 3 assuré' , 
	PCQLD IS 'Qualité Autre' , 
	PCCNR IS 'CNR O/N' , 
	PCASS IS 'Assuré O/N' , 
	PCSCP IS 'Souscripteur O/N' , 
	PCTYP IS 'Type' , 
	PCDESI IS 'Lien vers KPDESI' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YHPASSU 
( PCIPB TEXT IS 'N° de Police' , 
	PCALX TEXT IS 'N° Aliment' , 
	PCAVN TEXT IS 'N° avenant' , 
	PCHIN TEXT IS 'N° historique par avenant' , 
	PCIAS TEXT IS 'Identifiant Assuré souscript. 10/00' , 
	PCPRI TEXT IS 'Souscripteur principal (O/N)' , 
	PCQL1 TEXT IS 'Qualité 1 de l''assuré' , 
	PCQL2 TEXT IS 'Qualité 2 de l''assuré' , 
	PCQL3 TEXT IS 'Qualité 3 de l''assuré' , 
	PCQLD TEXT IS 'Qualité Autre' , 
	PCCNR TEXT IS 'CNR O/N' , 
	PCASS TEXT IS 'Assuré O/N' , 
	PCSCP TEXT IS 'Souscripteur O/N' , 
	PCTYP TEXT IS 'Type  O Offre  P Police  E à établir' , 
	PCDESI TEXT IS 'Lien vers KPDESI' ) ; 
  
