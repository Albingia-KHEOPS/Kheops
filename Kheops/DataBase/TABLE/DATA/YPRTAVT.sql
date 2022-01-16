CREATE TABLE ZALBINKHEO.YPRTAVT ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPRTAVT de ZALBINKHEO ignoré. 
	KHIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KHALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KHALX. 
	KHCHA CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KHRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KHRSQ. 
	KHOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KHOBJ. 
	KHFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KHCHI CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KHCTX CHAR(6) CCSID 297 NOT NULL DEFAULT '' , 
	KHACT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHANV CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KHNVV CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KHCGN CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHAVO CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHOPE CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHCHR CHAR(6) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPRTAVT    ; 
  
LABEL ON TABLE ZALBINKHEO.YPRTAVT 
	IS 'Poli.RT : Trace modif Avnt                      KH' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTAVT 
( KHIPB IS 'N° Police' , 
	KHALX IS 'N° Aliment' , 
	KHCHA IS 'Chapitre gestion' , 
	KHRSQ IS 'Identifiant risque' , 
	KHOBJ IS 'Identifiant objet' , 
	KHFOR IS 'Identifiant formule' , 
	KHCHI IS 'Chapitre impression' , 
	KHCTX IS 'Contexte' , 
	KHACT IS 'Type action' , 
	KHANV IS 'Ancienne valeur' , 
	KHNVV IS 'Nouvelle valeur' , 
	KHCGN IS 'Code générateur' , 
	KHAVO IS 'Avenant Obligatoire' , 
	KHOPE IS 'Opération à effect.' , 
	KHCHR IS 'Sous-Chapitre regrp' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTAVT 
( KHIPB TEXT IS 'N° Police' , 
	KHALX TEXT IS 'N° Aliment' , 
	KHCHA TEXT IS 'Chapitre de gestion' , 
	KHRSQ TEXT IS 'Identifiant risque' , 
	KHOBJ TEXT IS 'Identifiant objet' , 
	KHFOR TEXT IS 'Identifiant formule' , 
	KHCHI TEXT IS 'Chapitre d''impression' , 
	KHCTX TEXT IS 'Contexte d''utilisation' , 
	KHACT TEXT IS 'Type action   (Création Modif Supp)' , 
	KHANV TEXT IS 'Ancienne valeur' , 
	KHNVV TEXT IS 'Nouvelle valeur' , 
	KHCGN TEXT IS 'Code générateur de clause' , 
	KHAVO TEXT IS 'Avenant obligatoire O/N' , 
	KHOPE TEXT IS 'Opération à effectuer' , 
	KHCHR TEXT IS 'Sous-chapitre de regroupement' ) ; 
  
