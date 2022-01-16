﻿CREATE TABLE ZALBINKHEO.YHRTOBJ ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YHRTOBJ de ZALBINKHEO ignoré. 
	JGIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	JGALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JGALX. 
	JGAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	JGHIN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	JGRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JGRSQ. 
	JGOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JGOBJ. 
	JGCCH NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JGCCH. 
	JGIGD NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	JGBRA CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	JGSBR CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	JGCAT CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	JGRCS CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	JGCCS CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	JGVAL DECIMAL(11, 0) NOT NULL DEFAULT 0 , 
	JGVAA DECIMAL(11, 0) NOT NULL DEFAULT 0 , 
	JGVAW DECIMAL(11, 0) NOT NULL DEFAULT 0 , 
	JGVAT CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	JGVAU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGVAH CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGNOJ DECIMAL(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JGNOJ. 
	JGMMQ CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	JGMTY CHAR(30) CCSID 297 NOT NULL DEFAULT '' , 
	JGMSR CHAR(30) CCSID 297 NOT NULL DEFAULT '' , 
	JGMFA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	JGTEM CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGVGD DECIMAL(2, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JGVGD. 
	JGVGU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGVDA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	JGVDM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JGVDJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JGVDH DECIMAL(4, 0) NOT NULL DEFAULT 0 , 
	JGVFA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	JGVFM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JGVFJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JGVFH DECIMAL(4, 0) NOT NULL DEFAULT 0 , 
	JGRGT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGTRR CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	JGCNA CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGINA CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGIND CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	JGIXC CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGIXF CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGIXL CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGIXP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGIVO DECIMAL(7, 2) NOT NULL DEFAULT 0 , 
	JGIVA DECIMAL(7, 2) NOT NULL DEFAULT 0 , 
	JGIVW DECIMAL(7, 2) NOT NULL DEFAULT 0 , 
	JGGAU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGGVL DECIMAL(9, 0) NOT NULL DEFAULT 0 , 
	JGGUN CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGPBN CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGPBS DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	JGPBR DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	JGPBT DECIMAL(2, 0) NOT NULL DEFAULT 0 , 
	JGPBC DECIMAL(2, 0) NOT NULL DEFAULT 0 , 
	JGPBP DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	JGPBA DECIMAL(1, 0) NOT NULL DEFAULT 0 , 
	JGCLV CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGAGM DECIMAL(2, 0) NOT NULL DEFAULT 0 , 
	JGAVE DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	JGAVA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	JGAVM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JGAVJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JGRUL CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGRUT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JGAVF DECIMAL(3, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FHRTOBJ    ; 
  
LABEL ON TABLE ZALBINKHEO.YHRTOBJ 
	IS 'H-PolI.RT:Objet                                 JG' ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTOBJ 
( JGIPB IS 'N° de police' , 
	JGALX IS 'N° Aliment' , 
	JGAVN IS 'N° avenant' , 
	JGHIN IS 'N° historique avenan' , 
	JGRSQ IS 'Identifiant risque' , 
	JGOBJ IS 'Identifiant objet' , 
	JGCCH IS 'Chrono obj' , 
	JGIGD IS 'Identifiant Matériel' , 
	JGBRA IS 'Branche' , 
	JGSBR IS 'Sous-branche' , 
	JGCAT IS 'Catégorie' , 
	JGRCS IS 'Référence CS' , 
	JGCCS IS 'Code référence CS' , 
	JGVAL IS 'Valeur objet Origine' , 
	JGVAA IS 'Valeur Objet Actual.' , 
	JGVAW IS 'W. Valeur de l''objet' , 
	JGVAT IS 'Type de valeur Objet' , 
	JGVAU IS 'Unité de la valeur' , 
	JGVAH IS 'HT ou TTC  H/T' , 
	JGNOJ IS 'Nombre de matériel' , 
	JGMMQ IS 'Marque' , 
	JGMTY IS 'Type de matériel' , 
	JGMSR IS 'Série de matériel' , 
	JGMFA IS 'Année de fabrication' , 
	JGTEM IS 'Gar.tempo O/N' , 
	JGVGD IS 'Val.Garant : Durée' , 
	JGVGU IS 'Val.Garant : Unité' , 
	JGVDA IS 'Val.gar Année début' , 
	JGVDM IS 'Val.gar : Mois début' , 
	JGVDJ IS 'Val gar : Jour début' , 
	JGVDH IS 'Heure début validité' , 
	JGVFA IS 'Val.gar Année fin' , 
	JGVFM IS 'Val garan : Mois fin' , 
	JGVFJ IS 'Val garan : Jour fin' , 
	JGVFH IS 'Heure fin validité' , 
	JGRGT IS 'Régime de taxe' , 
	JGTRR IS 'Territorialité' , 
	JGCNA IS 'Application CATNAT' , 
	JGINA IS 'Indexation O/N' , 
	JGIND IS 'Non utilisé' , 
	JGIXC IS 'Index capitaux (O/N)' , 
	JGIXF IS 'Indexation franchise' , 
	JGIXL IS 'Indexation LCI (O/N)' , 
	JGIXP IS 'Indexation Primes' , 
	JGIVO IS 'Valeur indice Origin' , 
	JGIVA IS 'Valeur indice actual' , 
	JGIVW IS 'W.Valeur de l''indice' , 
	JGGAU IS 'Garantie automat O/N' , 
	JGGVL IS 'GA : Valeur limite' , 
	JGGUN IS 'GA : Unité  limite' , 
	JGPBN IS 'Particip. bénéficiai' , 
	JGPBS IS 'PB Seuil P/B' , 
	JGPBR IS 'Mnt ristourne en %' , 
	JGPBT IS 'PB Taux appel' , 
	JGPBC IS 'PB complement Taux' , 
	JGPBP IS 'PB % prime retenue' , 
	JGPBA IS 'PB Nbr années' , 
	JGCLV IS 'Collis renvers  O/N' , 
	JGAGM IS 'Age du matériel' , 
	JGAVE IS 'N° avenant création' , 
	JGAVA IS 'Année Effet avenant' , 
	JGAVM IS 'Mois Effet avenant' , 
	JGAVJ IS 'Jour Effet avenant' , 
	JGRUL IS 'A régulariser O/N' , 
	JGRUT IS 'Type régule' , 
	JGAVF IS 'N° avenant de modif' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTOBJ 
( JGIPB TEXT IS 'N° de Police' , 
	JGALX TEXT IS 'N° Aliment' , 
	JGAVN TEXT IS 'N° avenant' , 
	JGHIN TEXT IS 'N° historique par avenant' , 
	JGRSQ TEXT IS 'Identifiant risque' , 
	JGOBJ TEXT IS 'Identifiant objet' , 
	JGCCH TEXT IS 'Code chronologique Objet' , 
	JGIGD TEXT IS 'Identifiant Matériel' , 
	JGBRA TEXT IS 'Branche' , 
	JGSBR TEXT IS 'Sous-branche' , 
	JGCAT TEXT IS 'Catégorie' , 
	JGRCS TEXT IS 'Référence Conventions spéciales' , 
	JGCCS TEXT IS 'Code référence CS' , 
	JGVAL TEXT IS 'Valeur de l''objet Origine' , 
	JGVAA TEXT IS 'Valeur Actualisé de l''objet' , 
	JGVAW TEXT IS 'W. Valeur de l''objet (travail)' , 
	JGVAT TEXT IS 'Type de valeur de l''objet' , 
	JGVAU TEXT IS 'Unité de la valeur' , 
	JGVAH TEXT IS 'HT ou TTC  H/T' , 
	JGNOJ TEXT IS 'Nombre de matériel' , 
	JGMMQ TEXT IS 'Marque' , 
	JGMTY TEXT IS 'Type de matériel' , 
	JGMSR TEXT IS 'Série de matériel' , 
	JGMFA TEXT IS 'Année de fabrication' , 
	JGTEM TEXT IS 'Garantie temporaire O/N' , 
	JGVGD TEXT IS 'Validité garantie : Durée' , 
	JGVGU TEXT IS 'Validité garantie : Unité' , 
	JGVDA TEXT IS 'Validité garantie : Année début' , 
	JGVDM TEXT IS 'Validité garantie : Mois début' , 
	JGVDJ TEXT IS 'Validité garantie : Jour début' , 
	JGVDH TEXT IS 'Validité garantie : Heure début' , 
	JGVFA TEXT IS 'Validité garantie : Année fin' , 
	JGVFM TEXT IS 'Validité garantie : Mois fin' , 
	JGVFJ TEXT IS 'Validité garantie : Jour fin' , 
	JGVFH TEXT IS 'Validité garantie : Heure fin' , 
	JGRGT TEXT IS 'Régime de taxe' , 
	JGTRR TEXT IS 'Code territorialité' , 
	JGCNA TEXT IS 'Application CATNAT O/N' , 
	JGINA TEXT IS 'Indexation (O/N)' , 
	JGIND TEXT IS 'Non utilisé' , 
	JGIXC TEXT IS 'Indexation des capitaux (O/N)' , 
	JGIXF TEXT IS 'Indexation Franchises' , 
	JGIXL TEXT IS 'Indexation LCI (O/N)' , 
	JGIXP TEXT IS 'Indexation Primes (O/N)' , 
	JGIVO TEXT IS 'Valeur de l''indice Origine' , 
	JGIVA TEXT IS 'Valeur de l''indice actualisé' , 
	JGIVW TEXT IS 'W. Valeur de l''indice (Travail)' , 
	JGGAU TEXT IS 'Garantie automatique O/N' , 
	JGGVL TEXT IS 'Garantie automatique : Valeur limite' , 
	JGGUN TEXT IS 'Garantie automatique : Unité limite' , 
	JGPBN TEXT IS 'Participation bénéficiaire O/N' , 
	JGPBS TEXT IS 'PB : Seuil de rapport S/P' , 
	JGPBR TEXT IS 'PB : Montant de ristourne en %' , 
	JGPBT TEXT IS 'PB : Taux appel de prime' , 
	JGPBC TEXT IS 'PB : Complément taux d''appel' , 
	JGPBP TEXT IS 'PB : % de prime retenue' , 
	JGPBA TEXT IS 'PB : Nombre d''années' , 
	JGCLV TEXT IS 'Collision renversement O/N' , 
	JGAGM TEXT IS 'Age du matériel pour collision renv' , 
	JGAVE TEXT IS 'N° avenant de création' , 
	JGAVA TEXT IS 'Année Effet avenant OBJ' , 
	JGAVM TEXT IS 'Mois  Effet avenant OBJ' , 
	JGAVJ TEXT IS 'Jour  Effet avenant OBJ' , 
	JGRUL TEXT IS 'A régulariser O/N' , 
	JGRUT TEXT IS 'Type de régularisation (A,C,E...)' , 
	JGAVF TEXT IS 'N° avenant de modification' ) ; 
  
