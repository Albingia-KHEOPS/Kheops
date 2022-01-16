CREATE TABLE ZALBINKHEO.KPVALH ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPVALH de ZALBINKHEO ignoré. 
	KIFTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KIFIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KIFALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KIFALX. 
	KIFAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	KIFPERI CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KIFRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KIFOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KIFFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KIFOPT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KIFCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KIFCRD NUMERIC(10, 0) NOT NULL DEFAULT 0 , 
	KIFCRH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KIFFEA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KIFFEM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KIFFEJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KIFCTD NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KIFCTD. 
	KIFCTU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KIFRUL CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KIFRUT CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPVALH     ; 
  
LABEL ON TABLE ZALBINKHEO.KPVALH 
	IS 'Validation Save champs                         KIF' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPVALH 
( KIFTYP IS 'Type  O/P' , 
	KIFIPB IS 'N° de police / Offre' , 
	KIFALX IS 'N° Aliment / connexe' , 
	KIFAVN IS 'N° avenant' , 
	KIFPERI IS 'Périmètre' , 
	KIFRSQ IS 'Risque' , 
	KIFOBJ IS 'Objet' , 
	KIFFOR IS 'Formule' , 
	KIFOPT IS 'Option' , 
	KIFCRU IS 'Création User' , 
	KIFCRD IS 'Création Date' , 
	KIFCRH IS 'Création Heure' , 
	KIFFEA IS 'Fin d''effet Année' , 
	KIFFEM IS 'Fin d''effet Mois' , 
	KIFFEJ IS 'Fin d''effet Jour' , 
	KIFCTD IS 'Durée du contrat' , 
	KIFCTU IS 'Unité de la durée' , 
	KIFRUL IS 'A régulariser O/N' , 
	KIFRUT IS 'Type régule' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPVALH 
( KIFTYP TEXT IS 'Type  O Offre  P Police' , 
	KIFIPB TEXT IS 'N° de Police / Offre' , 
	KIFALX TEXT IS 'N° Aliment ou Connexe' , 
	KIFAVN TEXT IS 'N° avenant' , 
	KIFPERI TEXT IS 'Périmètre' , 
	KIFRSQ TEXT IS 'Risque' , 
	KIFOBJ TEXT IS 'Objet' , 
	KIFFOR TEXT IS 'Formule' , 
	KIFOPT TEXT IS 'Option' , 
	KIFCRU TEXT IS 'Création User' , 
	KIFCRD TEXT IS 'Création Date' , 
	KIFCRH TEXT IS 'Création Heure' , 
	KIFFEA TEXT IS 'Fin d''effet Année' , 
	KIFFEM TEXT IS 'Fin d''effet Mois' , 
	KIFFEJ TEXT IS 'Fin d''effet Jour' , 
	KIFCTD TEXT IS 'Durée du contrat' , 
	KIFCTU TEXT IS 'Unité de la durée contrat' , 
	KIFRUL TEXT IS 'A régulariser O/N' , 
	KIFRUT TEXT IS 'Type de régularisation (A,C,E...)' ) ; 
  
