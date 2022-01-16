CREATE TABLE ZALBINKHEO.YLETTYP ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YLETTYP de ZALBINKHEO ignoré. 
	LTLET CHAR(7) CCSID 297 NOT NULL DEFAULT '' , 
	LTFML CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	LTLBL CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	LTLBC CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	LTREP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	LTREL CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	LTLR1 CHAR(7) CCSID 297 NOT NULL DEFAULT '' , 
	LTDL1 NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne LTDL1. 
	LTLR2 CHAR(7) CCSID 297 NOT NULL DEFAULT '' , 
	LTDL2 NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne LTDL2. 
	LTLR3 CHAR(7) CCSID 297 NOT NULL DEFAULT '' , 
	LTDL3 NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne LTDL3. 
	LTLR4 CHAR(7) CCSID 297 NOT NULL DEFAULT '' , 
	LTDL4 NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne LTDL4. 
	LTLR5 CHAR(7) CCSID 297 NOT NULL DEFAULT '' , 
	LTDL5 NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne LTDL5. 
	LTTLT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	LTENV CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	LTVER NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	LTVEA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	LTVEM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	LTVEJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	LTVEH DECIMAL(6, 0) NOT NULL DEFAULT 0 , 
	LTFPC CHAR(30) CCSID 297 NOT NULL DEFAULT '' , 
	LTFPV NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	LTRFG CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FLETTYP    ; 
  
LABEL ON TABLE ZALBINKHEO.YLETTYP 
	IS 'LETTRE TYPE : Nouvelle version                  LT' ; 
  
LABEL ON COLUMN ZALBINKHEO.YLETTYP 
( LTLET IS 'Code Lettre type' , 
	LTFML IS 'Famille lettre Type' , 
	LTLBL IS 'Libellé lettre type' , 
	LTLBC IS 'Libellé court' , 
	LTREP IS 'Réponse (O/N)' , 
	LTREL IS 'A relancer O/N' , 
	LTLR1 IS 'Lettre relance 1' , 
	LTDL1 IS 'Délai de relance  1' , 
	LTLR2 IS 'Lettre relance 2' , 
	LTDL2 IS 'Délai de relance 2' , 
	LTLR3 IS 'Lettre Relance 3' , 
	LTDL3 IS 'Délai de relance  3' , 
	LTLR4 IS 'Lettre Relance 4' , 
	LTDL4 IS 'Délai de relance 4' , 
	LTLR5 IS 'Lettre Relance 5' , 
	LTDL5 IS 'Délai de relance 5' , 
	LTTLT IS 'Type de courrier' , 
	LTENV IS 'Environnement' , 
	LTVER IS 'N° version' , 
	LTVEA IS 'Année N° version' , 
	LTVEM IS 'Mois N° version' , 
	LTVEJ IS 'Jour N° version' , 
	LTVEH IS 'Heure  N° version' , 
	LTFPC IS 'LTFPC               Clé                 fdp' , 
	LTFPV IS 'LTFPV               Version             fond page' , 
	LTRFG IS 'ref de Garantie O/N' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YLETTYP 
( LTLET TEXT IS 'Code Lettre type' , 
	LTFML TEXT IS 'Famille lettre Type' , 
	LTLBL TEXT IS 'Libellé lettre type' , 
	LTLBC TEXT IS 'Libellé court lettre type' , 
	LTREP TEXT IS 'Attente d''une réponse (O/N)' , 
	LTREL TEXT IS 'A relancer O/N' , 
	LTLR1 TEXT IS 'lettre de relance 1' , 
	LTDL1 TEXT IS 'Délai Lettre de relance 1' , 
	LTLR2 TEXT IS 'lettre relance 2' , 
	LTDL2 TEXT IS 'Délai Lettre de relance 2' , 
	LTLR3 TEXT IS 'lettre relance 3' , 
	LTDL3 TEXT IS 'Délai Lettre de relance   3' , 
	LTLR4 TEXT IS 'lettre relance 4' , 
	LTDL4 TEXT IS 'Délai Lettre de relance 4' , 
	LTLR5 TEXT IS 'lettre de relance 5' , 
	LTDL5 TEXT IS 'Délai Lettre de relance  5' , 
	LTTLT TEXT IS 'Type de courrier (T.texte ou Libre)' , 
	LTENV TEXT IS 'Environnement lettre type' , 
	LTVER TEXT IS 'N° version' , 
	LTVEA TEXT IS 'N° version : Année' , 
	LTVEM TEXT IS 'N° version : Mois' , 
	LTVEJ TEXT IS 'N° version : Jour' , 
	LTVEH TEXT IS 'N° version : Heure' , 
	LTFPC TEXT IS 'Clé du fond de page (facultatif)' , 
	LTFPV TEXT IS 'Version de fond de page' , 
	LTRFG TEXT IS 'Refus de garantie O/N' ) ; 
  
