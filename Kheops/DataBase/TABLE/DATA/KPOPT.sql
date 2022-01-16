﻿CREATE TABLE ZALBINKHEO.KPOPT ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPOPT de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPOPT de ZALBINKHEO. 
	KDBID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDBTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDBIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KDBALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KDBFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDBKDAID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDBOPT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDBDESC CHAR(60) CCSID 297 NOT NULL DEFAULT '' , 
	KDBFORR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDBKDAIDR NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDBSPEID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDBCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDBCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KDBCRH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KDBMAJU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDBMAJD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KDBMAJH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KDBPAQ CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDBACQ NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDBTMC NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDBTFF NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDBTFP DECIMAL(13, 9) NOT NULL DEFAULT 0 , 
	KDBPRO CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDBTMI CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDBTFM CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KDBCMC NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDBCFO CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDBCHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDBCTT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDBCCP DECIMAL(13, 9) NOT NULL DEFAULT 0 , 
	KDBVAL DECIMAL(13, 2) NOT NULL DEFAULT 0 , 
	KDBVAA NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDBVAW NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDBVAT CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KDBVAU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDBVAH CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDBIVO NUMERIC(7, 2) NOT NULL DEFAULT 0 , 
	KDBIVA NUMERIC(7, 2) NOT NULL DEFAULT 0 , 
	KDBIVW NUMERIC(7, 2) NOT NULL DEFAULT 0 , 
	KDBAVE NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KDBAVG NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KDBECO CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDBAVA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KDBAVM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KDBAVJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KDBEHH NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDBEHC NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDBEHI NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDBASVALO NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDBASVALA NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDBASVALW NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDBASUNIT CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDBASBASE CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KDBGER NUMERIC(13, 2) NOT NULL DEFAULT 0 )   
	RCDFMT FOPT       ; 
  
LABEL ON TABLE ZALBINKHEO.KPOPT 
	IS 'KHEOPS O/P Option' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPOPT 
( KDBID IS 'ID unique' , 
	KDBTYP IS 'Type O/P' , 
	KDBIPB IS 'IPB/ALX' , 
	KDBALX IS 'Aliment/Version' , 
	KDBFOR IS 'Formule' , 
	KDBKDAID IS 'ID KPFOR' , 
	KDBOPT IS 'Option' , 
	KDBDESC IS 'Description' , 
	KDBFORR IS 'Formule si renvoi' , 
	KDBKDAIDR IS 'ID KPFOR' , 
	KDBSPEID IS 'IDLien KPIOPT' , 
	KDBCRU IS 'Création User' , 
	KDBCRD IS 'Création Date' , 
	KDBCRH IS 'Création Heure' , 
	KDBMAJU IS 'Maj User' , 
	KDBMAJD IS 'Maj Date' , 
	KDBMAJH IS 'Maj Heure' , 
	KDBPAQ IS 'Montant Acquis O/N' , 
	KDBACQ IS 'Montant acquis' , 
	KDBTMC IS 'Total Calculé Ref' , 
	KDBTFF IS 'Total Mnt Forcé Ref' , 
	KDBTFP IS 'Total Coef calcul' , 
	KDBPRO IS 'Mnt provisionnel O/N' , 
	KDBTMI IS 'Mnt Forcé pour Mini' , 
	KDBTFM IS 'Motif Total   forcé' , 
	KDBCMC IS 'Comptant Mnt Calculé' , 
	KDBCFO IS 'Comptant mnt Forcé' , 
	KDBCHT IS 'Comptant MntForcé HT' , 
	KDBCTT IS 'Comptant MntForc TTC' , 
	KDBCCP IS 'Coeff calcul forcé  Comptant' , 
	KDBVAL IS 'Valeur Origine' , 
	KDBVAA IS 'Valeur Actualisée' , 
	KDBVAW IS 'Valeur de travail' , 
	KDBVAT IS 'Type de valeur' , 
	KDBVAU IS 'Unité de la valeur' , 
	KDBVAH IS 'HT/TTC' , 
	KDBIVO IS 'Valeur Indice origin' , 
	KDBIVA IS 'Valeur Indice Actual' , 
	KDBIVW IS 'Valeur Indice Travai' , 
	KDBAVE IS 'N° avenant Création' , 
	KDBAVG IS 'N° Avenant Modif' , 
	KDBECO IS 'Formule En cours' , 
	KDBAVA IS 'AA Effet AVN formule' , 
	KDBAVM IS 'Mois Effet Avn Formu' , 
	KDBAVJ IS 'Jour Effet Avn Form' , 
	KDBEHH IS 'Prochaine ECh   HT' , 
	KDBEHC IS 'Prochaine Ech Catnat' , 
	KDBEHI IS 'Prochaine Ech Incend' , 
	KDBASVALO IS 'Assiette Val Origine' , 
	KDBASVALA IS 'Assiette Val Actual' , 
	KDBASVALW IS 'Assiette Valeur W' , 
	KDBASUNIT IS 'Assiette Unité' , 
	KDBASBASE IS 'Assiette Base (TypV)' , 
	KDBGER IS 'Mnt Ref Forcé saisi' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPOPT 
( KDBID TEXT IS 'ID unique' , 
	KDBTYP TEXT IS 'Type O/P' , 
	KDBIPB TEXT IS 'IPB/ALX' , 
	KDBALX TEXT IS 'Aliment/version' , 
	KDBFOR TEXT IS 'Formule' , 
	KDBKDAID TEXT IS 'ID KPFOR' , 
	KDBOPT TEXT IS 'Option' , 
	KDBDESC TEXT IS 'Description' , 
	KDBFORR TEXT IS 'Formule si renvoi' , 
	KDBKDAIDR TEXT IS 'ID KPFOR' , 
	KDBSPEID TEXT IS 'ID Lien KPIOPT' , 
	KDBCRU TEXT IS 'Création user' , 
	KDBCRD TEXT IS 'Création Date' , 
	KDBCRH TEXT IS 'Création Heure' , 
	KDBMAJU TEXT IS 'Maj User' , 
	KDBMAJD TEXT IS 'Maj Date' , 
	KDBMAJH TEXT IS 'Maj Heure' , 
	KDBPAQ TEXT IS 'Montant Acquis O/N' , 
	KDBACQ TEXT IS 'Montant acquis' , 
	KDBTMC TEXT IS 'Mnt Ref Montant Calculé Référence' , 
	KDBTFF TEXT IS 'Mnt Ref Montant Forcé Référence' , 
	KDBTFP TEXT IS 'Mnt Ref Coefficient de calcul' , 
	KDBPRO TEXT IS 'Mnt Ref Montant Provisionnel O/N' , 
	KDBTMI TEXT IS 'Mnt Ref Mnt forcé pour Minimum O/N' , 
	KDBTFM TEXT IS 'Mnt Ref Motif forcé' , 
	KDBCMC TEXT IS 'Comptant : Montant Calculé' , 
	KDBCFO TEXT IS 'Comptant : Montant Forcé O/N' , 
	KDBCHT TEXT IS 'Comptant : Montant Forcé HT' , 
	KDBCTT TEXT IS 'Comptant : montant forcé TTC' , 
	KDBCCP TEXT IS 'Comptant : Coefficient calcul forcé' , 
	KDBVAL TEXT IS 'Valeur origine' , 
	KDBVAA TEXT IS 'Valeur Actualisée' , 
	KDBVAW TEXT IS 'Valeur de travail' , 
	KDBVAT TEXT IS 'Type de valeur' , 
	KDBVAU TEXT IS 'Unité de la valeur' , 
	KDBVAH TEXT IS 'HT/TTC' , 
	KDBIVO TEXT IS 'Valeur de l''indice Origine' , 
	KDBIVA TEXT IS 'Valeur Indice Actualisé' , 
	KDBIVW TEXT IS 'Valeur Indice de travail' , 
	KDBAVE TEXT IS 'N° d''avenant de Création' , 
	KDBAVG TEXT IS 'N° avenant de modification' , 
	KDBECO TEXT IS 'Formule en-cours O/N' , 
	KDBAVA TEXT IS 'Année Effet Avenant Formule' , 
	KDBAVM TEXT IS 'Mois  Effet Avenant Formule' , 
	KDBAVJ TEXT IS 'Jour  Effet Avenant Formule' , 
	KDBEHH TEXT IS 'Prochaine Echéance HT' , 
	KDBEHC TEXT IS 'Prochaine Echéance CATNAT' , 
	KDBEHI TEXT IS 'Prochaine Echéance Incendie' , 
	KDBASVALO TEXT IS 'Assiette Valeur Origine' , 
	KDBASVALA TEXT IS 'Assiette Valeur Actualisée' , 
	KDBASVALW TEXT IS 'Assiette Valeur de travail' , 
	KDBASUNIT TEXT IS 'Assiette Unité' , 
	KDBASBASE TEXT IS 'Assiette Base (Type Valeur)' , 
	KDBGER TEXT IS 'Mnt Ref Montant forcé Saisi' ) ; 
  
