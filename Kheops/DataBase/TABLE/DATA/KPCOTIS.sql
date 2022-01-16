﻿CREATE TABLE ZALBINKHEO.KPCOTIS ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPCOTIS de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPCOTIS de ZALBINKHEO. 
	KDMID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDMTAP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDMTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDMIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KDMALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KDMATGMHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMATGKHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMATGMTX NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMATGKTX NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMATGMTT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMATGKTT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMATGCOT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMATGKCO NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMCNABAS NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMCNAKBS NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMCNAMHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMCNAKHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMCNAMTX NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMCNAKTX NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMCNAMTT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMCNAKTT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMCNACOB CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDMCNACNC NUMERIC(5, 3) NOT NULL DEFAULT 0 , 
	KDMCNATXF NUMERIC(5, 3) NOT NULL DEFAULT 0 , 
	KDMCNACNM NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMCNACMF NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMCNAKCM NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMGARMHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMGARMTX NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMGARMTT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMHFMHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMHFFLAG CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDMHFMHF NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMHFMTX NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMHFMTT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMAFRB CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDMAFR NUMERIC(7, 2) NOT NULL DEFAULT 0 , 
	KDMKFA NUMERIC(7, 2) NOT NULL DEFAULT 0 , 
	KDMAFT NUMERIC(7, 2) NOT NULL DEFAULT 0 , 
	KDMKFT NUMERIC(7, 2) NOT NULL DEFAULT 0 , 
	KDMFGA NUMERIC(7, 2) NOT NULL DEFAULT 0 , 
	KDMKFG NUMERIC(7, 2) NOT NULL DEFAULT 0 , 
	KDMMHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMMHFLAG CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDMMHF NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMKHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMMTX NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMKTX NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMMTT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMMTFLAG CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDMTTF NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMKTT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMCOB CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDMCOM NUMERIC(5, 3) NOT NULL DEFAULT 0 , 
	KDMCMF NUMERIC(5, 3) NOT NULL DEFAULT 0 , 
	KDMCOT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMCOF NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMKCO NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDMCOEFC NUMERIC(7, 4) NOT NULL DEFAULT 0 )   
	RCDFMT FPCOTIS    ; 
  
LABEL ON TABLE ZALBINKHEO.KPCOTIS 
	IS 'KHEOPS Cotisation' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPCOTIS 
( KDMID IS 'ID' , 
	KDMTAP IS 'Part T/A  Total/Alb' , 
	KDMTYP IS 'Type O/P' , 
	KDMIPB IS 'IPB' , 
	KDMALX IS 'ALX' , 
	KDMATGMHT IS 'ATG Montant HT dev' , 
	KDMATGKHT IS 'ATG montant HT cpt' , 
	KDMATGMTX IS 'ATG Mnt taxes dev' , 
	KDMATGKTX IS 'ATG Mnt taxes cpt' , 
	KDMATGMTT IS 'ATG Mnt TTC dev' , 
	KDMATGKTT IS 'ATG Mnt TTC cpt' , 
	KDMATGCOT IS 'ATG Commissions dev' , 
	KDMATGKCO IS 'ATG Commission cpt' , 
	KDMCNABAS IS 'CATNAT Base dev' , 
	KDMCNAKBS IS 'CATNAT Base cpt' , 
	KDMCNAMHT IS 'CNA HT dev' , 
	KDMCNAKHT IS 'CNA HT cpt' , 
	KDMCNAKTX IS 'CATNAT Taxes cpt' , 
	KDMCNAMTT IS 'CATNAT TTC dev' , 
	KDMCNAKTT IS 'CATNAT TTC cpt' , 
	KDMCNACOB IS 'CATNAT Comm barème' , 
	KDMCNACNC IS 'CATNAT Taux commiss' , 
	KDMCNATXF IS 'CATNAT TX comm forcé' , 
	KDMCNACNM IS 'CNA Commission dev' , 
	KDMCNACMF IS 'CATNAT CommForcéeDev' , 
	KDMGARMHT IS 'HT hors CN,ATG  dev' , 
	KDMGARMTX IS 'Taxes horsCN,ATG dev' , 
	KDMGARMTT IS 'TTC Hors CN,ATG dev' , 
	KDMHFMHT IS 'HT HorsFrais cal dev' , 
	KDMHFFLAG IS 'Flag HTHorsFrais for' , 
	KDMHFMHF IS 'HT HorsFrais Forcé d' , 
	KDMHFMTX IS 'Taxes HorsFrais dev' , 
	KDMHFMTT IS 'TTC Hors Frais dev' , 
	KDMAFRB IS 'Frais barême O/N' , 
	KDMAFR IS 'Frais HT dev' , 
	KDMKFA IS 'Frais HT cpt' , 
	KDMAFT IS 'Frais taxes dev' , 
	KDMKFT IS 'Frais taxes cpt' , 
	KDMFGA IS 'FGA Montant dev' , 
	KDMKFG IS 'FGA montant cpt' , 
	KDMMHT IS 'Total HT calculé dev' , 
	KDMMHFLAG IS 'Flah HT forcé O/N' , 
	KDMMHF IS 'Total HT forcé dev' , 
	KDMKHT IS 'Total HT cpt' , 
	KDMMTX IS 'Total taxes dev' , 
	KDMKTX IS 'Total Taxes cpt' , 
	KDMMTT IS 'Total TTC dev' , 
	KDMMTFLAG IS 'Flag TTC forcé O/N' , 
	KDMTTF IS 'Total TTC forcé dev' , 
	KDMKTT IS 'Total TTC Cpt' , 
	KDMCOB IS 'Commission barême' , 
	KDMCOM IS 'Taux de commission' , 
	KDMCMF IS 'Tx commission forcé' , 
	KDMCOT IS 'Commissions dev' , 
	KDMCOF IS 'Commiss Forcée  dev' , 
	KDMKCO IS 'Commissions cpt' , 
	KDMCOEFC IS 'Coeff commercial' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPCOTIS 
( KDMID TEXT IS 'ID' , 
	KDMTAP TEXT IS 'Part T= Total A = Albingia' , 
	KDMTYP TEXT IS 'Type O/P' , 
	KDMIPB TEXT IS 'IPB' , 
	KDMALX TEXT IS 'ALX' , 
	KDMATGMHT TEXT IS 'ATG Montant HT dev' , 
	KDMATGKHT TEXT IS 'ATG Montant HT cpt' , 
	KDMATGMTX TEXT IS 'ATG Montant  taxes dev' , 
	KDMATGKTX TEXT IS 'ATG montant taxes cpt' , 
	KDMATGMTT TEXT IS 'ATG Montant TTC  dev' , 
	KDMATGKTT TEXT IS 'ATG Montant TTC cpt' , 
	KDMATGCOT TEXT IS 'ATG Commissions dev' , 
	KDMATGKCO TEXT IS 'ATG Commission  cpt' , 
	KDMCNABAS TEXT IS 'CATNAT Base dev' , 
	KDMCNAKBS TEXT IS 'CATNAT Base cpt' , 
	KDMCNAMHT TEXT IS 'CATNAT HT dev' , 
	KDMCNAKHT TEXT IS 'CATNAT HT cpt' , 
	KDMCNAMTX TEXT IS 'CATNAT Taxes dev' , 
	KDMCNAKTX TEXT IS 'CATNAT Taxes cpt' , 
	KDMCNAMTT TEXT IS 'CATNAT TTC dev' , 
	KDMCNAKTT TEXT IS 'CATNAT TTC cpt' , 
	KDMCNACOB TEXT IS 'CATNAT Commission dans barème O/N' , 
	KDMCNACNC TEXT IS 'CATNAT Taux Commission' , 
	KDMCNATXF TEXT IS 'CATNAT Taux commission forcé' , 
	KDMCNACNM TEXT IS 'CATNAT Commission dev' , 
	KDMCNACMF TEXT IS 'CATNAT Commission forcée dev' , 
	KDMCNAKCM TEXT IS 'CATNAT Commission cpt' , 
	KDMGARMHT TEXT IS 'HT hors CN Hors ATG dev' , 
	KDMGARMTX TEXT IS 'Taxes Hors CN,ATG dev' , 
	KDMGARMTT TEXT IS 'TTC hors CN,ATG dev' , 
	KDMHFMHT TEXT IS 'HT hors Frais Calculé dev' , 
	KDMHFFLAG TEXT IS 'Flag HT hors Frais forcé  O/N' , 
	KDMHFMHF TEXT IS 'HT hors Frais Forcé dev' , 
	KDMHFMTX TEXT IS 'Taxes Hors Frais dev' , 
	KDMHFMTT TEXT IS 'TTC Hors Frais dev' , 
	KDMAFRB TEXT IS 'Frais dans Barême O/N' , 
	KDMAFR TEXT IS 'Frais HT dev' , 
	KDMKFA TEXT IS 'Frais HT cpt' , 
	KDMAFT TEXT IS 'Frais taxes dev' , 
	KDMKFT TEXT IS 'Frais Taxes cpt' , 
	KDMFGA TEXT IS 'FGA Montant dev' , 
	KDMKFG TEXT IS 'FGA Montant cpt' , 
	KDMMHT TEXT IS 'Total HT calculé dev' , 
	KDMMHFLAG TEXT IS 'Flag Total HT forcé O/N' , 
	KDMMHF TEXT IS 'Total HT forcé dev' , 
	KDMKHT TEXT IS 'Total HT cpt' , 
	KDMMTX TEXT IS 'Total Taxes dev' , 
	KDMKTX TEXT IS 'Total Taxes cpt' , 
	KDMMTT TEXT IS 'Total TTC dev' , 
	KDMMTFLAG TEXT IS 'Flag Total TTC forcé O/N' , 
	KDMTTF TEXT IS 'Total TTC forcé dev' , 
	KDMKTT TEXT IS 'Total TTC cpt' , 
	KDMCOB TEXT IS 'Commission dans barême O/N' , 
	KDMCOM TEXT IS 'Taux de commission' , 
	KDMCMF TEXT IS 'Taux de commission forcé' , 
	KDMCOT TEXT IS 'Commissions dev' , 
	KDMCOF TEXT IS 'Commission Forcée dev' , 
	KDMKCO TEXT IS 'Commissions cpt' , 
	KDMCOEFC TEXT IS 'Coefficient commercial' ) ; 
  
