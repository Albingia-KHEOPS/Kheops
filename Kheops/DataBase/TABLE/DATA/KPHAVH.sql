CREATE TABLE ZALBINKHEO.KPHAVH ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPHAVH de ZALBINKHEO ignoré. 
	KIGTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KIGIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KIGALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KIGALX. 
	KIGAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	KIGPERI CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KIGRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KIGOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KIGFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KIGOPT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KIGCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KIGCRD NUMERIC(10, 0) NOT NULL DEFAULT 0 , 
	KIGCRH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KIGFEA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KIGFEM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KIGFEJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KIGCTD NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KIGCTD. 
	KIGCTU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KIGRUL CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KIGRUT CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPHAVH     ; 
  
LABEL ON TABLE ZALBINKHEO.KPHAVH 
	IS 'Hors avenant trace modif                       KIG' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPHAVH 
( KIGTYP IS 'Type  O/P' , 
	KIGIPB IS 'N° de police / Offre' , 
	KIGALX IS 'N° Aliment / connexe' , 
	KIGAVN IS 'N° avenant' , 
	KIGPERI IS 'Périmètre' , 
	KIGRSQ IS 'Risque' , 
	KIGOBJ IS 'Objet' , 
	KIGFOR IS 'Formule' , 
	KIGOPT IS 'Option' , 
	KIGCRU IS 'Création User' , 
	KIGCRD IS 'Création Date' , 
	KIGCRH IS 'Création Heure' , 
	KIGFEA IS 'Fin d''effet Année' , 
	KIGFEM IS 'Fin d''effet Mois' , 
	KIGFEJ IS 'Fin d''effet Jour' , 
	KIGCTD IS 'Durée du contrat' , 
	KIGCTU IS 'Unité de la durée' , 
	KIGRUL IS 'A régulariser O/N' , 
	KIGRUT IS 'Type régule' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPHAVH 
( KIGTYP TEXT IS 'Type  O Offre  P Police' , 
	KIGIPB TEXT IS 'N° de Police / Offre' , 
	KIGALX TEXT IS 'N° Aliment ou Connexe' , 
	KIGAVN TEXT IS 'N° avenant' , 
	KIGPERI TEXT IS 'Périmètre' , 
	KIGRSQ TEXT IS 'Risque' , 
	KIGOBJ TEXT IS 'Objet' , 
	KIGFOR TEXT IS 'Formule' , 
	KIGOPT TEXT IS 'Option' , 
	KIGCRU TEXT IS 'Création User' , 
	KIGCRD TEXT IS 'Création Date' , 
	KIGCRH TEXT IS 'Création Heure' , 
	KIGFEA TEXT IS 'Fin d''effet Année' , 
	KIGFEM TEXT IS 'Fin d''effet Mois' , 
	KIGFEJ TEXT IS 'Fin d''effet Jour' , 
	KIGCTD TEXT IS 'Durée du contrat' , 
	KIGCTU TEXT IS 'Unité de la durée contrat' , 
	KIGRUL TEXT IS 'A régulariser O/N' , 
	KIGRUT TEXT IS 'Type de régularisation (A,C,E...)' ) ; 
  
