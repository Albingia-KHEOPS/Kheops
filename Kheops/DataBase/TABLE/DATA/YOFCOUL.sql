CREATE TABLE ZALBINKHEO.YOFCOUL ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YOFCOUL de ZALBINKHEO ignoré. 
	EAIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	EAALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne EAALX. 
	EATBR CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	EAREL CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	EASOU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	EABUR CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	EALT1 CHAR(7) CCSID 297 NOT NULL DEFAULT '' , 
	EAL1A NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	EAL1M NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	EAL1J NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	EALTD CHAR(7) CCSID 297 NOT NULL DEFAULT '' , 
	EALDA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	EALDM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	EALDJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	EALTP CHAR(7) CCSID 297 NOT NULL DEFAULT '' , 
	EALPA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	EALPM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	EALPJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	EALTR CHAR(7) CCSID 297 NOT NULL DEFAULT '' , 
	EALRD NUMERIC(3, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FOFCOUL    ; 
  
LABEL ON TABLE ZALBINKHEO.YOFCOUL 
	IS 'Offres : Relances                               EA' ; 
  
LABEL ON COLUMN ZALBINKHEO.YOFCOUL 
( EAIPB IS 'N° de police / Offre' , 
	EAALX IS 'N° Aliment / connexe' , 
	EATBR IS 'CO ou RT' , 
	EAREL IS 'Top relance O/N/V' , 
	EASOU IS 'Souscripteur' , 
	EABUR IS 'Bureau du courtier' , 
	EALT1 IS 'Première lettre rel' , 
	EAL1A IS 'Année édition let 1' , 
	EAL1M IS 'Mois édition let 1' , 
	EAL1J IS 'Jour édition let 1' , 
	EALTD IS 'Dernière lettre édit' , 
	EALDA IS 'Année édit. dern let' , 
	EALDM IS 'Mois édit. dern let' , 
	EALDJ IS 'Jour édit. dern let' , 
	EALTP IS 'Lettre à éditer' , 
	EALPA IS 'Année édit.Lettre' , 
	EALPM IS 'Mois édit. Lettre' , 
	EALPJ IS 'Jour édit. Lettre' , 
	EALTR IS 'Lettre relance' , 
	EALRD IS 'Délai de relance' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YOFCOUL 
( EAIPB TEXT IS 'N° de Police / Offre' , 
	EAALX TEXT IS 'N° Aliment ou Connexe' , 
	EATBR TEXT IS 'CO ou RT' , 
	EAREL TEXT IS 'Top de relance O/N/V' , 
	EASOU TEXT IS 'Souscripteur' , 
	EABUR TEXT IS 'Code Bureau du courtier' , 
	EALT1 TEXT IS 'Première lettre de relance' , 
	EAL1A TEXT IS 'Année édition lettre 1' , 
	EAL1M TEXT IS 'Mois  édition lettre 1' , 
	EAL1J TEXT IS 'Jour  édition lettre 1' , 
	EALTD TEXT IS 'Dernière lettre éditée' , 
	EALDA TEXT IS 'Année édition dernière lettre' , 
	EALDM TEXT IS 'Mois  édition dernière lettre' , 
	EALDJ TEXT IS 'Jour  édition dernière lettre' , 
	EALTP TEXT IS 'Lettre à éditer' , 
	EALPA TEXT IS 'Année édition Lettre' , 
	EALPM TEXT IS 'Mois  édition Lettre' , 
	EALPJ TEXT IS 'Jour  édition Lettre' , 
	EALTR TEXT IS 'Lettre de relance de lettre éditée' , 
	EALRD TEXT IS 'Délai  de relance' ) ; 
  
