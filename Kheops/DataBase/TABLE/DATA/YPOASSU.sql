CREATE TABLE ZALBINKHEO.YPOASSU ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPOASSU de ZALBINKHEO ignoré. 
	PCTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PCIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	PCALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne PCALX. 
	PCIAS NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	PCPRI CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PCQL1 CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	PCQL2 CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	PCQL3 CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	PCQLD CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	PCCNR CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PCASS CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PCSCP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PCDESI NUMERIC(15, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPOASSU    ; 
  
LABEL ON TABLE ZALBINKHEO.YPOASSU 
	IS 'Polices/Offres : Assurés                        PC' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOASSU 
( PCTYP IS 'Type                O/P' , 
	PCIPB IS 'N° de police / Offre' , 
	PCALX IS 'N° Aliment / connexe' , 
	PCIAS IS 'Identifiant Assuré' , 
	PCPRI IS 'Souscr.principal O/N' , 
	PCQL1 IS 'Qualité 1 assuré' , 
	PCQL2 IS 'Qualité 2 assuré' , 
	PCQL3 IS 'Qualité 3 assuré' , 
	PCQLD IS 'Qualité Autre' , 
	PCCNR IS 'CNR O/N' , 
	PCASS IS 'Assuré O/N' , 
	PCSCP IS 'Souscripteur O/N' , 
	PCDESI IS 'Lien vers KPDESI' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOASSU 
( PCTYP TEXT IS 'Type  O Offre  P Police' , 
	PCIPB TEXT IS 'N° de Police / Offre' , 
	PCALX TEXT IS 'N° Aliment ou Connexe' , 
	PCIAS TEXT IS 'Identifiant Assuré 10/00' , 
	PCPRI TEXT IS 'Souscripteur principal (O/N)' , 
	PCQL1 TEXT IS 'Qualité 1 de l''assuré' , 
	PCQL2 TEXT IS 'Qualité 2 de l''assuré' , 
	PCQL3 TEXT IS 'Qualité 3 de l''assuré' , 
	PCQLD TEXT IS 'Qualité Autre' , 
	PCCNR TEXT IS 'CNR O/N' , 
	PCASS TEXT IS 'Assuré O/N' , 
	PCSCP TEXT IS 'Souscripteur O/N' , 
	PCDESI TEXT IS 'Lien vers KPDESI' ) ; 
  
