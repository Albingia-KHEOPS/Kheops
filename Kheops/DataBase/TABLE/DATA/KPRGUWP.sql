﻿CREATE TABLE ZALBINKHEO.KPRGUWP ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPRGUWP de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPRGUWP de ZALBINKHEO. 
	KHYTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHYIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KHYALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KHYALX. 
	KHYRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KHYRSQ. 
	KHYFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KHYKDEID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHYGARAN CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KHYDEBP NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KHYFINP NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KHYTRG CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHYNPE CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KHYVEN NUMERIC(5, 2) NOT NULL DEFAULT 0 , 
	KHYCAF NUMERIC(11, 0) NOT NULL DEFAULT 0 , 
	KHYCAU NUMERIC(11, 0) NOT NULL DEFAULT 0 , 
	KHYCAE NUMERIC(11, 0) NOT NULL DEFAULT 0 , 
	KHYDM1 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYDT1 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYDM2 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYDT2 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYCOE NUMERIC(5, 3) NOT NULL DEFAULT 0 , 
	KHYCA1 DECIMAL(13, 2) NOT NULL DEFAULT 0 , 
	KHYCT1 DECIMAL(7, 4) NOT NULL DEFAULT 0 , 
	KHYCU1 CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHYCP1 DECIMAL(13, 2) NOT NULL DEFAULT 0 , 
	KHYCX1 CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHYCA2 DECIMAL(13, 2) NOT NULL DEFAULT 0 , 
	KHYCT2 DECIMAL(7, 4) NOT NULL DEFAULT 0 , 
	KHYCU2 CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHYCP2 DECIMAL(13, 2) NOT NULL DEFAULT 0 , 
	KHYCX2 CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHYAJU DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	KHYLMR DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	KHYMBA DECIMAL(13, 2) NOT NULL DEFAULT 0 , 
	KHYTEN DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	KHYBRG CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KHYBRL CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KHYBAS DECIMAL(13, 2) NOT NULL DEFAULT 0 , 
	KHYBAT DECIMAL(6, 3) NOT NULL DEFAULT 0 , 
	KHYBAU CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHYBAM DECIMAL(13, 2) NOT NULL DEFAULT 0 , 
	KHYXF1 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHYXB1 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYXM1 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYXF2 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHYXB2 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYXM2 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYXF3 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHYXB3 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYXM3 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYREG CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHYPEI NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KHYCNH DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYCNT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYGRM DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYKEA DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYPBP NUMERIC(5, 2) NOT NULL DEFAULT 0 , 
	KHYKTD DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYASV DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYPBT NUMERIC(5, 3) NOT NULL DEFAULT 0 , 
	KHYSIP NUMERIC(11, 2) NOT NULL DEFAULT 0 , 
	KHYPBS NUMERIC(5, 2) NOT NULL DEFAULT 0 , 
	KHYRIS DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHYPBR NUMERIC(5, 2) NOT NULL DEFAULT 0 , 
	KHYRIA DECIMAL(11, 2) NOT NULL DEFAULT 0 )   
	RCDFMT FPRGUWP    ; 
  
LABEL ON TABLE ZALBINKHEO.KPRGUWP 
	IS 'Régularisation Pér/Gar_                        KHY' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPRGUWP 
( KHYTYP IS 'Type' , 
	KHYIPB IS 'N° de police' , 
	KHYALX IS 'N° Aliment' , 
	KHYRSQ IS 'Identifiant risque' , 
	KHYFOR IS 'Id formule' , 
	KHYKDEID IS 'Lien KPGARAN' , 
	KHYGARAN IS 'Code garantie' , 
	KHYDEBP IS 'Début période' , 
	KHYFINP IS 'Fin de période' , 
	KHYTRG IS 'Type de régule' , 
	KHYNPE IS 'Nature Régule' , 
	KHYVEN IS 'Ventilation CA en %' , 
	KHYCAF IS 'C.A France' , 
	KHYCAU IS 'C.A USA/CANADA' , 
	KHYCAE IS 'C.A Export' , 
	KHYDM1 IS 'Déja émis HT 1' , 
	KHYDT1 IS 'Déja Emis Taxe 1' , 
	KHYDM2 IS 'Déja émis HT 2' , 
	KHYDT2 IS 'Déja émis  taxes 2' , 
	KHYCOE IS 'Coefficient' , 
	KHYCA1 IS 'MB&CA Prévisionnel 1' , 
	KHYCT1 IS 'MB&CA Prévis Taux' , 
	KHYCU1 IS 'Unité Taux 1' , 
	KHYCP1 IS 'MB&CA Prévis Prime' , 
	KHYCX1 IS 'Code taxe 1' , 
	KHYCA2 IS 'MB&CA Définitif' , 
	KHYCT2 IS 'MB&CA Définitif Taux' , 
	KHYCU2 IS 'Unité Taux 2' , 
	KHYCP2 IS 'MB&CA définitif Prim' , 
	KHYCX2 IS 'Code taxe 2' , 
	KHYAJU IS '% Ajustibilité' , 
	KHYLMR IS 'Limite Ristourne' , 
	KHYMBA IS 'MB Assuré' , 
	KHYTEN IS 'Tendance' , 
	KHYBRG IS 'Code Base de régule' , 
	KHYBRL IS 'Libellé définition' , 
	KHYBAS IS 'Base de régule' , 
	KHYBAT IS 'Taux sur Base' , 
	KHYBAU IS 'Unité Taux sur base' , 
	KHYBAM IS 'Base Montant' , 
	KHYXF1 IS 'Famille de taxe 1' , 
	KHYXB1 IS 'Base Famille taxe 1' , 
	KHYXM1 IS 'Montant de taxe Fam1' , 
	KHYXF2 IS 'Famille de taxe 2' , 
	KHYXB2 IS 'Base Famille taxe 2' , 
	KHYXM2 IS 'Montant de taxe Fam2' , 
	KHYXF3 IS 'Famille de taxe 3' , 
	KHYXB3 IS 'Base Famille taxe 3' , 
	KHYXM3 IS 'Mnt taxe famille 3' , 
	KHYREG IS 'Regulee' , 
	KHYPEI IS 'Période indemnisat°' , 
	KHYCNH IS 'CATNAT Montant HT' , 
	KHYCNT IS 'CATNAT Mnt taxe' , 
	KHYGRM IS 'GAREAT Mnt HT' , 
	KHYKEA IS 'PB Prime émise acq' , 
	KHYPBP IS 'PB % prime retenue' , 
	KHYKTD IS 'PB Prime technique' , 
	KHYASV IS 'PB valeur Assiette' , 
	KHYPBT IS 'PB taux appel' , 
	KHYSIP IS 'PB Sinistres Payés' , 
	KHYPBS IS 'PB seuil rapport S/P' , 
	KHYRIS IS 'PB mnt Ristourne' , 
	KHYPBR IS 'PB Ristourne en %' , 
	KHYRIA IS 'PB Ristourne Anticip' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPRGUWP 
( KHYTYP TEXT IS 'Type  O Offre  P Police  E à établir' , 
	KHYIPB TEXT IS 'N° de Police' , 
	KHYALX TEXT IS 'N° Aliment' , 
	KHYRSQ TEXT IS 'Identifiant risque' , 
	KHYFOR TEXT IS 'Id formule' , 
	KHYKDEID TEXT IS 'Lien KPGARAN' , 
	KHYGARAN TEXT IS 'Code garantie' , 
	KHYDEBP TEXT IS 'Début période' , 
	KHYFINP TEXT IS 'Fin de période' , 
	KHYTRG TEXT IS 'Type de régule' , 
	KHYNPE TEXT IS 'Nature Régule' , 
	KHYVEN TEXT IS 'Ventilation CA  en %' , 
	KHYCAF TEXT IS 'Chiffre Affaire France' , 
	KHYCAU TEXT IS 'Chiffre d''Affaire USA/CANADA' , 
	KHYCAE TEXT IS 'Chiffre d''Affaire Export' , 
	KHYDM1 TEXT IS 'Déja émis HT 1' , 
	KHYDT1 TEXT IS 'Déja émis Taxes 1' , 
	KHYDM2 TEXT IS 'Déja émis HT 2' , 
	KHYDT2 TEXT IS 'Déja émis Taxes 2' , 
	KHYCOE TEXT IS 'Coefficient' , 
	KHYCA1 TEXT IS 'MB&CA Prévisionnel OU CA 1' , 
	KHYCT1 TEXT IS 'MB&CA Prévisionnel ou 1 Taux' , 
	KHYCU1 TEXT IS 'Unité taux 1' , 
	KHYCP1 TEXT IS 'MB&CA prévisionnel ou 1 Prime' , 
	KHYCX1 TEXT IS 'Code taxe  1' , 
	KHYCA2 TEXT IS 'MB&CA Définitif ou CA 2' , 
	KHYCT2 TEXT IS 'MB&CA Définitif ou 2  Taux' , 
	KHYCU2 TEXT IS 'Unité taux 2' , 
	KHYCP2 TEXT IS 'MB&CA Définitif ou 2 Prime' , 
	KHYCX2 TEXT IS 'Code taxe 2' , 
	KHYAJU TEXT IS '% Ajustabilité' , 
	KHYLMR TEXT IS 'Limite ristourne' , 
	KHYMBA TEXT IS 'MB Assuré ou MB & tendance' , 
	KHYTEN TEXT IS 'Tendance' , 
	KHYBRG TEXT IS 'Code Base de régule' , 
	KHYBRL TEXT IS 'Libellé définition' , 
	KHYBAS TEXT IS 'Base Base de régule' , 
	KHYBAT TEXT IS 'Base Taux sur Base' , 
	KHYBAU TEXT IS 'Unité Taux sur Base' , 
	KHYBAM TEXT IS 'Base Montant' , 
	KHYXF1 TEXT IS 'Famille de taxe 1' , 
	KHYXB1 TEXT IS 'Base Famille de taxe 1' , 
	KHYXM1 TEXT IS 'Montant de taxe Famille 1' , 
	KHYXF2 TEXT IS 'Famille de taxe 2' , 
	KHYXB2 TEXT IS 'Base Famille de taxe 2' , 
	KHYXM2 TEXT IS 'Montant de taxe Famille 2' , 
	KHYXF3 TEXT IS 'Famille de taxe 3' , 
	KHYXB3 TEXT IS 'Base Famille de taxe 3' , 
	KHYXM3 TEXT IS 'Montant de taxe Famille 3' , 
	KHYREG TEXT IS 'Garantie régulée' , 
	KHYPEI TEXT IS 'Période Indemnisation en NB Mois' , 
	KHYCNH TEXT IS 'CATNAT montant HT' , 
	KHYCNT TEXT IS 'CATNAT Montant de taxe' , 
	KHYGRM TEXT IS 'GAREAT Montant HT Compris' , 
	KHYKEA TEXT IS 'PB Prime émise acquitée' , 
	KHYPBP TEXT IS 'PB % de prime retenue' , 
	KHYKTD TEXT IS 'PB Prime technique due' , 
	KHYASV TEXT IS 'PB Valeur de l''Assiette' , 
	KHYPBT TEXT IS 'PB taux Appel' , 
	KHYSIP TEXT IS 'PB Sinistres Payés' , 
	KHYPBS TEXT IS 'PB seuil de rapport S/P' , 
	KHYRIS TEXT IS 'PB montant de Ristourne' , 
	KHYPBR TEXT IS 'PB Ristourne en %' , 
	KHYRIA TEXT IS 'PB Ristourne Anticipée' ) ; 
  