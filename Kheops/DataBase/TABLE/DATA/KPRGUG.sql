﻿CREATE TABLE ZALBINKHEO.KPRGUG ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPRGUG de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPRGUG de ZALBINKHEO. 
	KHXID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHXKHWID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHXTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHXIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KHXALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KHXALX. 
	KHXRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KHXRSQ. 
	KHXFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KHXKDEID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHXGARAN CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KHXDEBP NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KHXFINP NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KHXSIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHXTRG CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHXNPE CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KHXVEN NUMERIC(5, 2) NOT NULL DEFAULT 0 , 
	KHXCAF NUMERIC(11, 0) NOT NULL DEFAULT 0 , 
	KHXCAU NUMERIC(11, 0) NOT NULL DEFAULT 0 , 
	KHXCAE NUMERIC(11, 0) NOT NULL DEFAULT 0 , 
	KHXMHC DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXFRC CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHXFR0 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHXMHT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXMTX DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXMTT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXCNH DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXCNT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXGRM DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXPRO DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXECH DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXECT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXEMH DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXEMT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXDM1 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXDT1 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXDM2 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXDT2 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXCOE NUMERIC(5, 3) NOT NULL DEFAULT 0 , 
	KHXCA1 DECIMAL(13, 2) NOT NULL DEFAULT 0 , 
	KHXCT1 DECIMAL(7, 4) NOT NULL DEFAULT 0 , 
	KHXCU1 CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHXCP1 DECIMAL(13, 2) NOT NULL DEFAULT 0 , 
	KHXCX1 CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHXCA2 DECIMAL(13, 2) NOT NULL DEFAULT 0 , 
	KHXCT2 DECIMAL(7, 4) NOT NULL DEFAULT 0 , 
	KHXCU2 CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHXCP2 DECIMAL(13, 2) NOT NULL DEFAULT 0 , 
	KHXCX2 CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHXAJU DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	KHXLMR DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	KHXMBA DECIMAL(13, 2) NOT NULL DEFAULT 0 , 
	KHXTEN DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	KHXHON DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXHOX CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHXBRG CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KHXBRL CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KHXBAS DECIMAL(13, 2) NOT NULL DEFAULT 0 , 
	KHXBAT DECIMAL(6, 3) NOT NULL DEFAULT 0 , 
	KHXBAU CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHXBAM DECIMAL(13, 2) NOT NULL DEFAULT 0 , 
	KHXXF1 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHXXB1 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXXM1 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXXF2 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHXXB2 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXXM2 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXXF3 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHXXB3 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXXM3 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXREG CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHXPEI NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KHXKEA DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXPBP NUMERIC(5, 2) NOT NULL DEFAULT 0 , 
	KHXKTD DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXASV DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXPBT NUMERIC(5, 3) NOT NULL DEFAULT 0 , 
	KHXSIP NUMERIC(11, 2) NOT NULL DEFAULT 0 , 
	KHXPBS NUMERIC(5, 2) NOT NULL DEFAULT 0 , 
	KHXRIS DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHXPBR NUMERIC(5, 2) NOT NULL DEFAULT 0 , 
	KHXRIA DECIMAL(11, 2) NOT NULL DEFAULT 0 )   
	RCDFMT FPRGUG     ; 
  
LABEL ON TABLE ZALBINKHEO.KPRGUG 
	IS 'Régularisation Garantie                        KHX' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPRGUG 
( KHXID IS 'ID unique' , 
	KHXKHWID IS 'Lien KPRGU Entête' , 
	KHXTYP IS 'Type' , 
	KHXIPB IS 'N° de police' , 
	KHXALX IS 'N° Aliment' , 
	KHXRSQ IS 'Identifiant risque' , 
	KHXFOR IS 'Id formule' , 
	KHXKDEID IS 'Lien KPGARAN' , 
	KHXGARAN IS 'Code garantie' , 
	KHXDEBP IS 'Début période' , 
	KHXFINP IS 'Fin de période' , 
	KHXSIT IS 'Situation N/A/V' , 
	KHXTRG IS 'Type de régule' , 
	KHXNPE IS 'Nature Régule' , 
	KHXVEN IS 'Ventilation CA en %' , 
	KHXCAF IS 'C.A France' , 
	KHXCAU IS 'C.A USA/CANADA' , 
	KHXCAE IS 'C.A Export' , 
	KHXMHC IS 'Montant prime HT' , 
	KHXFRC IS 'Mnt régule forcé O/N' , 
	KHXFR0 IS 'Forcé à 0 O/N' , 
	KHXMHT IS 'Mnt régule HT HorsCN' , 
	KHXMTX IS 'Mnt régule Taxes HCN' , 
	KHXMTT IS 'Mnt régule TTC HCN' , 
	KHXCNH IS 'CATNAT Montant HT' , 
	KHXCNT IS 'CATNAT Mnt taxe' , 
	KHXGRM IS 'GAREAT Mnt HT' , 
	KHXPRO IS 'Montant prime Prov' , 
	KHXECH IS 'Montant prime HT' , 
	KHXECT IS 'Montant prime HT' , 
	KHXEMH IS 'Déja émis HT' , 
	KHXEMT IS 'Mnt taxe Déja émis' , 
	KHXDM1 IS 'Déja émis HT 1' , 
	KHXDT1 IS 'Déja Emis Taxe 1' , 
	KHXDM2 IS 'Déja émis HT 2' , 
	KHXDT2 IS 'Déja émis  taxes 2' , 
	KHXCOE IS 'Coefficient' , 
	KHXCA1 IS 'MB&CA Prévisionnel 1' , 
	KHXCT1 IS 'MB&CA Prévis Taux' , 
	KHXCU1 IS 'Unité Taux 1' , 
	KHXCP1 IS 'MB&CA Prévis Prime' , 
	KHXCX1 IS 'Code taxe 1' , 
	KHXCA2 IS 'MB&CA Définitif' , 
	KHXCT2 IS 'MB&CA Définitif Taux' , 
	KHXCU2 IS 'Unité Taux 2' , 
	KHXCP2 IS 'MB&CA définitif Prim' , 
	KHXCX2 IS 'Code taxe 2' , 
	KHXAJU IS '% Ajustibilité' , 
	KHXLMR IS 'Limite Ristourne' , 
	KHXMBA IS 'MB Assuré' , 
	KHXTEN IS 'Tendance' , 
	KHXHON IS 'Mnt Honoraire HT' , 
	KHXHOX IS 'Honoraire Code taxe' , 
	KHXBRG IS 'Code Base de régule' , 
	KHXBRL IS 'Libellé définition' , 
	KHXBAS IS 'Base de régule' , 
	KHXBAT IS 'Taux sur Base' , 
	KHXBAU IS 'Unité Taux sur base' , 
	KHXBAM IS 'Base Montant' , 
	KHXXF1 IS 'Famille de taxe 1' , 
	KHXXB1 IS 'Base Famille taxe 1' , 
	KHXXM1 IS 'Montant de taxe Fam1' , 
	KHXXF2 IS 'Famille de taxe 2' , 
	KHXXB2 IS 'Base Famille taxe 2' , 
	KHXXM2 IS 'Montant de taxe Fam2' , 
	KHXXF3 IS 'Famille de taxe 3' , 
	KHXXB3 IS 'Base Famille taxe 3' , 
	KHXXM3 IS 'Mnt taxe famille 3' , 
	KHXREG IS 'Regulee' , 
	KHXPEI IS 'Période indemnisat°' , 
	KHXKEA IS 'PB Prime émise acq' , 
	KHXPBP IS 'PB % prime retenue' , 
	KHXKTD IS 'PB Prime technique' , 
	KHXASV IS 'PB valeur Assiette' , 
	KHXPBT IS 'PB taux appel' , 
	KHXSIP IS 'PB Sinistres Payés' , 
	KHXPBS IS 'PB seuil rapport S/P' , 
	KHXRIS IS 'PB mnt Ristourne' , 
	KHXPBR IS 'PB Ristourne en %' , 
	KHXRIA IS 'PB Ristourne Anticip' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPRGUG 
( KHXID TEXT IS 'ID unique' , 
	KHXKHWID TEXT IS 'Lien KPRGU   Entête' , 
	KHXTYP TEXT IS 'Type  O Offre  P Police  E à établir' , 
	KHXIPB TEXT IS 'N° de Police' , 
	KHXALX TEXT IS 'N° Aliment' , 
	KHXRSQ TEXT IS 'Identifiant risque' , 
	KHXFOR TEXT IS 'Id formule' , 
	KHXKDEID TEXT IS 'Lien KPGARAN' , 
	KHXGARAN TEXT IS 'Code garantie' , 
	KHXDEBP TEXT IS 'Début période' , 
	KHXFINP TEXT IS 'Fin de période' , 
	KHXSIT TEXT IS 'Code situation  N/A/V' , 
	KHXTRG TEXT IS 'Type de régule' , 
	KHXNPE TEXT IS 'Nature Régule' , 
	KHXVEN TEXT IS 'Ventilation CA  en %' , 
	KHXCAF TEXT IS 'Chiffre Affaire France' , 
	KHXCAU TEXT IS 'Chiffre d''Affaire USA/CANADA' , 
	KHXCAE TEXT IS 'Chiffre d''Affaire Export' , 
	KHXMHC TEXT IS 'Mnt régule HT Hors CN calculé' , 
	KHXFRC TEXT IS 'Mnt régule forcé Forcé O/N' , 
	KHXFR0 TEXT IS 'Si forcé forcé à zéro O/N' , 
	KHXMHT TEXT IS 'Mnt régule HT Hors Catnat' , 
	KHXMTX TEXT IS 'Mnt régule Taxes Hors Catnat' , 
	KHXMTT TEXT IS 'Mnt régule TTC Hors CN' , 
	KHXCNH TEXT IS 'Mnt régule CATNAT montant HT' , 
	KHXCNT TEXT IS 'Mnt régule CATNAT Montant de taxe' , 
	KHXGRM TEXT IS 'Mnt régule GAREAT Montant HT Compris' , 
	KHXPRO TEXT IS 'Montant prime provisionnelle' , 
	KHXECH TEXT IS 'Déja émis calculé HT' , 
	KHXECT TEXT IS 'Déja émis calculé Taxes' , 
	KHXEMH TEXT IS 'Déja émis retenu HT' , 
	KHXEMT TEXT IS 'Déja émis retenu Mnt Taxe' , 
	KHXDM1 TEXT IS 'Déja émis HT 1' , 
	KHXDT1 TEXT IS 'Déja émis Taxes 1' , 
	KHXDM2 TEXT IS 'Déja émis HT 2' , 
	KHXDT2 TEXT IS 'Déja émis Taxes 2' , 
	KHXCOE TEXT IS 'Coefficient' , 
	KHXCA1 TEXT IS 'MB&CA Prévisionnel OU CA 1' , 
	KHXCT1 TEXT IS 'MB&CA Prévisionnel ou 1 Taux' , 
	KHXCU1 TEXT IS 'Unité taux 1' , 
	KHXCP1 TEXT IS 'MB&CA prévisionnel ou 1 Prime' , 
	KHXCX1 TEXT IS 'Code taxe  1' , 
	KHXCA2 TEXT IS 'MB&CA Définitif ou CA 2' , 
	KHXCT2 TEXT IS 'MB&CA Définitif ou 2  Taux' , 
	KHXCU2 TEXT IS 'Unité taux 2' , 
	KHXCP2 TEXT IS 'MB&CA Définitif ou 2 Prime' , 
	KHXCX2 TEXT IS 'Code taxe 2' , 
	KHXAJU TEXT IS '% Ajustabilité' , 
	KHXLMR TEXT IS 'Limite ristourne' , 
	KHXMBA TEXT IS 'MB Assuré ou MB & tendance' , 
	KHXTEN TEXT IS 'Tendance' , 
	KHXHON TEXT IS 'Montant Honoraire HT' , 
	KHXHOX TEXT IS 'Honoraire Code taxe' , 
	KHXBRG TEXT IS 'Code Base de régule' , 
	KHXBRL TEXT IS 'Libellé définition' , 
	KHXBAS TEXT IS 'Base Base de régule' , 
	KHXBAT TEXT IS 'Base Taux sur Base' , 
	KHXBAU TEXT IS 'Unité Taux sur Base' , 
	KHXBAM TEXT IS 'Base Montant' , 
	KHXXF1 TEXT IS 'Famille de taxe 1' , 
	KHXXB1 TEXT IS 'Base Famille de taxe 1' , 
	KHXXM1 TEXT IS 'Montant de taxe Famille 1' , 
	KHXXF2 TEXT IS 'Famille de taxe 2' , 
	KHXXB2 TEXT IS 'Base Famille de taxe 2' , 
	KHXXM2 TEXT IS 'Montant de taxe Famille 2' , 
	KHXXF3 TEXT IS 'Famille de taxe 3' , 
	KHXXB3 TEXT IS 'Base Famille de taxe 3' , 
	KHXXM3 TEXT IS 'Montant de taxe Famille 3' , 
	KHXREG TEXT IS 'Garantie régulée' , 
	KHXPEI TEXT IS 'Période Indemnisation en NB Mois' , 
	KHXKEA TEXT IS 'PB Prime émise acquitée' , 
	KHXPBP TEXT IS 'PB % de prime retenue' , 
	KHXKTD TEXT IS 'PB Prime technique due' , 
	KHXASV TEXT IS 'PB Valeur de l''Assiette' , 
	KHXPBT TEXT IS 'PB taux Appel' , 
	KHXSIP TEXT IS 'PB Sinistres Payés' , 
	KHXPBS TEXT IS 'PB seuil de rapport S/P' , 
	KHXRIS TEXT IS 'PB montant de Ristourne' , 
	KHXPBR TEXT IS 'PB Ristourne en %' , 
	KHXRIA TEXT IS 'PB Ristourne Anticipée' ) ; 
  
