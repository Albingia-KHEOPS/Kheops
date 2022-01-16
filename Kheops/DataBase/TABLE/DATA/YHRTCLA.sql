﻿CREATE TABLE ZALBINKHEO.YHRTCLA ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YHRTCLA de ZALBINKHEO ignoré. 
	QEIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	QEALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne QEALX. 
	QEAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	QEHIN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	QECHA CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	QERSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne QERSQ. 
	QEOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne QEOBJ. 
	QEFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	QEOPT NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	QEGAR CHAR(6) CCSID 297 NOT NULL DEFAULT '' , 
	QEOSP NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne QEOSP. 
	QECTX CHAR(6) CCSID 297 NOT NULL DEFAULT '' , 
	QECAR CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	QECLA CHAR(8) CCSID 297 NOT NULL DEFAULT '' , 
	QEIMA CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	QENTA CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	QEIL1 NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	QEIL2 NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	QEIL3 NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	QEIL4 NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	QEIL5 NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	QEIL6 NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	QEIL7 NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	QEIL8 NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	QEIL9 NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	QEVER NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	QEHIA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	QEHIM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	QEHIJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	QEHIH DECIMAL(6, 0) NOT NULL DEFAULT 0 , 
	QEIMP DECIMAL(8, 0) NOT NULL DEFAULT 0 , 
	QETOP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	QEAJT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	QECLM CHAR(8) CCSID 297 NOT NULL DEFAULT '' , 
	QETVL CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	QETXL DECIMAL(11, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne QETXL. 
	QEORI CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	QENAT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	QERCP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	QETAE CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	QEDSP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	QECHI CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	QECXI DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	QECHR CHAR(6) CCSID 297 NOT NULL DEFAULT '' , 
	QEAVG DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	QEAIM CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	QESPA CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FHRTCLA    ; 
  
LABEL ON TABLE ZALBINKHEO.YHRTCLA 
	IS 'H-Poli.RT:Clauses                               QE' ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTCLA 
( QEIPB IS 'N° Police' , 
	QEALX IS 'N° Version / Aliment' , 
	QEAVN IS 'N° avenant' , 
	QEHIN IS 'N° historique avenan' , 
	QECHA IS 'Chapitre de gestion' , 
	QERSQ IS 'Identifiant risque' , 
	QEOBJ IS 'Identifiant objet' , 
	QEFOR IS 'Identifiant formule' , 
	QEOPT IS 'Identifiant Option' , 
	QEGAR IS 'Code garantie' , 
	QEOSP IS 'Id objet spécifique' , 
	QECTX IS 'Contexte' , 
	QECAR IS 'Caractère Garantie' , 
	QECLA IS 'Code de la clause' , 
	QEIMA IS 'Impression en annexe' , 
	QENTA IS 'Nature association' , 
	QEIL1 IS 'Code variable 1' , 
	QEIL2 IS 'Code variable 2' , 
	QEIL3 IS 'Code variable 3' , 
	QEIL4 IS 'Code variable 4' , 
	QEIL5 IS 'Code variable 5' , 
	QEIL6 IS 'Code variable 6' , 
	QEIL7 IS 'Code variable 7' , 
	QEIL8 IS 'Code variable' , 
	QEIL9 IS 'Code variable 9' , 
	QEVER IS 'N° version' , 
	QEHIA IS 'Année Historisation' , 
	QEHIM IS 'Mois Historisation' , 
	QEHIJ IS 'Jour historisation' , 
	QEHIH IS 'Heure Historisation' , 
	QEIMP IS 'N° impression' , 
	QETOP IS 'Top Traitement' , 
	QEAJT IS 'Clause ajoutée O/N' , 
	QECLM IS 'Code clause mère' , 
	QETVL IS 'Top validation' , 
	QETXL IS 'N° texte libre' , 
	QEORI IS 'Origine' , 
	QENAT IS 'Nature clause' , 
	QERCP IS 'Récupéré historique' , 
	QETAE IS 'Type action enchaîn.' , 
	QEDSP IS 'Dispo spécifique O/N' , 
	QECHI IS 'Chapitre            d''impression' , 
	QECXI IS 'N° ordonnancement   d''impression' , 
	QECHR IS 'Sous-Chapitre regrp' , 
	QEAVG IS 'N°avenant génération' , 
	QEAIM IS 'Attribut impression' , 
	QESPA IS 'Clause spécif. Avn' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTCLA 
( QEIPB TEXT IS 'N° Police' , 
	QEALX TEXT IS 'N° Version ou Aliment' , 
	QEAVN TEXT IS 'N° avenant' , 
	QEHIN TEXT IS 'N° historique par avenant' , 
	QECHA TEXT IS 'Chapitre de gestion' , 
	QERSQ TEXT IS 'Identifiant risque' , 
	QEOBJ TEXT IS 'Identifiant objet' , 
	QEFOR TEXT IS 'Identifiant formule' , 
	QEOPT TEXT IS 'Option' , 
	QEGAR TEXT IS 'Code garantie' , 
	QEOSP TEXT IS 'Id objet si spécifique' , 
	QECTX TEXT IS 'Contexte' , 
	QECAR TEXT IS 'Caractère de la garantie' , 
	QECLA TEXT IS 'Code de la clause' , 
	QEIMA TEXT IS 'Impression en annexe O/N' , 
	QENTA TEXT IS 'Nature de l''association' , 
	QEIL1 TEXT IS 'Code variable 1' , 
	QEIL2 TEXT IS 'Code variable 2' , 
	QEIL3 TEXT IS 'Code variable 3' , 
	QEIL4 TEXT IS 'Code variable 4' , 
	QEIL5 TEXT IS 'Code variable 5' , 
	QEIL6 TEXT IS 'Code variable 6' , 
	QEIL7 TEXT IS 'Code variable 7' , 
	QEIL8 TEXT IS 'Code variable 8' , 
	QEIL9 TEXT IS 'Code variable 9' , 
	QEVER TEXT IS 'N° version' , 
	QEHIA TEXT IS 'Année Historisation' , 
	QEHIM TEXT IS 'Mois Historisation' , 
	QEHIJ TEXT IS 'Jour Historisation' , 
	QEHIH TEXT IS 'Heure Historisation' , 
	QEIMP TEXT IS 'N° d''ordre impression' , 
	QETOP TEXT IS 'Top de traitement' , 
	QEAJT TEXT IS 'Clause ajoutée (O/N)' , 
	QECLM TEXT IS 'Code clause Mère si association' , 
	QETVL TEXT IS 'Top Validation : '' '' / ''V''' , 
	QETXL TEXT IS 'N° Texte libre' , 
	QEORI TEXT IS 'Historique ou version en cours' , 
	QENAT TEXT IS 'Nature clause  Anodine/Sensible' , 
	QERCP TEXT IS 'Récupéré de l''historique O/N' , 
	QETAE TEXT IS 'Type action enchaînée' , 
	QEDSP TEXT IS 'Dispo spécifique O/N' , 
	QECHI TEXT IS 'Chapitre d''impression' , 
	QECXI TEXT IS 'N° ordonnancement d''impression' , 
	QECHR TEXT IS 'Sous-chapitre de regroupement' , 
	QEAVG TEXT IS 'N° avenant de génération' , 
	QEAIM TEXT IS 'Attribut d''impression (Gras ..)' , 
	QESPA TEXT IS 'Clause spécifique à l''avenant O/N' ) ; 
  