﻿CREATE TABLE ZALBINKHEO.KPRGU ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPRGU de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPRGU de ZALBINKHEO. 
	KHWID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHWTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHWIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KHWALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KHWALX. 
	KHWTTR CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KHWAVN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KHWAVND NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KHWEXE NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KHWDEBP NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KHWFINP NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KHWTRGU CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KHWIPK NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KHWICT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KHWICC NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KHWXCM DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	KHWCNC DECIMAL(5, 3) NOT NULL DEFAULT 0 , 
	KHWENC CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHWAFC CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHWAFR DECIMAL(7, 0) NOT NULL DEFAULT 0 , 
	KHWATT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHWMHT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHWCNH DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHWGRG DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHWTTT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHWMTT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KHWETA CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHWSIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHWSTU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KHWSTD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KHWSTH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KHWCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KHWCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KHWMAJU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KHWMAJD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KHWAVP NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KHWDESI NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHWOBSV NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHWOBSC NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHWMTF CHAR(10) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPRGU      ; 
  
LABEL ON TABLE ZALBINKHEO.KPRGU 
	IS 'Régularisation Entête                          KHW' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPRGU 
( KHWID IS 'ID unique' , 
	KHWTYP IS 'Type O/P' , 
	KHWIPB IS 'IPB' , 
	KHWALX IS 'ALX' , 
	KHWTTR IS 'Type de traitement' , 
	KHWAVN IS 'N° avenant' , 
	KHWAVND IS 'Date avenant' , 
	KHWEXE IS 'Exercice' , 
	KHWDEBP IS 'Début période' , 
	KHWFINP IS 'Fin de période' , 
	KHWTRGU IS 'Typologie régule' , 
	KHWIPK IS 'N° de prime de régul' , 
	KHWICT IS 'Courtier Adressé à' , 
	KHWICC IS 'Id courtier Commiss' , 
	KHWXCM IS 'Taux de commission' , 
	KHWCNC IS 'Taux commission     Cat Nat' , 
	KHWENC IS 'Code encaissement' , 
	KHWAFC IS 'Applic Frais/Acc' , 
	KHWAFR IS 'Mnt frais           accessoires' , 
	KHWATT IS 'App Taxes Attentat' , 
	KHWMHT IS 'Mnt HT (avec CN)' , 
	KHWCNH IS 'CATNAT Montant HT' , 
	KHWGRG IS 'GAREAT Montant HT' , 
	KHWTTT IS 'Total Taxes' , 
	KHWMTT IS 'Montant prime TTC' , 
	KHWETA IS 'Etat N/A/V' , 
	KHWSIT IS 'Situation' , 
	KHWSTU IS 'Situation User' , 
	KHWSTD IS 'Situation Date' , 
	KHWSTH IS 'Situation heure' , 
	KHWCRU IS 'Création User' , 
	KHWCRD IS 'Création date' , 
	KHWMAJU IS 'MAJ User' , 
	KHWMAJD IS 'Maj Date' , 
	KHWAVP IS 'N° avenant de la per' , 
	KHWDESI IS 'Lien KPDESI' , 
	KHWOBSV IS 'Lien KPOBSV' , 
	KHWOBSC IS 'ObsvCotis LienKPOBSV' , 
	KHWMTF IS 'Motif de régularisat' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPRGU 
( KHWID TEXT IS 'ID Unique' , 
	KHWTYP TEXT IS 'Type O/P' , 
	KHWIPB TEXT IS 'IPB' , 
	KHWALX TEXT IS 'ALX' , 
	KHWTTR TEXT IS 'Type de traitement (Affnouv/avenant)' , 
	KHWAVN TEXT IS 'N° avenant' , 
	KHWAVND TEXT IS 'Date avenant' , 
	KHWEXE TEXT IS 'Exercice' , 
	KHWDEBP TEXT IS 'Début période' , 
	KHWFINP TEXT IS 'Fin de période' , 
	KHWTRGU TEXT IS 'Typologie de régularisation' , 
	KHWIPK TEXT IS 'N° de prime de régularisation' , 
	KHWICT TEXT IS 'Id courtier  Adressé à' , 
	KHWICC TEXT IS 'Id courtier Commission' , 
	KHWXCM TEXT IS 'Taux de commission' , 
	KHWCNC TEXT IS 'Taux de commission Cat nat' , 
	KHWENC TEXT IS 'Code encaissement' , 
	KHWAFC TEXT IS 'Application Frais Accessoire' , 
	KHWAFR TEXT IS 'Montant de frais accessoires' , 
	KHWATT TEXT IS 'Application Taxes Attentat' , 
	KHWMHT TEXT IS 'Montant prime HT (y compris CATNAT)' , 
	KHWCNH TEXT IS 'CATNAT Montant  HT' , 
	KHWGRG TEXT IS 'GAREAT Montant HT' , 
	KHWTTT TEXT IS 'Total taxes' , 
	KHWMTT TEXT IS 'Montant prime TTC' , 
	KHWETA TEXT IS 'Etat N/A/V' , 
	KHWSIT TEXT IS 'Situation' , 
	KHWSTU TEXT IS 'Situation User' , 
	KHWSTD TEXT IS 'Situation Date' , 
	KHWSTH TEXT IS 'Situation  heure' , 
	KHWCRU TEXT IS 'Création User' , 
	KHWCRD TEXT IS 'Création date' , 
	KHWMAJU TEXT IS 'MAJ User' , 
	KHWMAJD TEXT IS 'MAJ Date' , 
	KHWAVP TEXT IS 'N° avenant le + récent de le période' , 
	KHWDESI TEXT IS 'Lien KPDESI Désignation' , 
	KHWOBSV TEXT IS 'Lien KPOBSV' , 
	KHWOBSC TEXT IS 'Observation Cotisation Lien KPOBSV' , 
	KHWMTF TEXT IS 'Motif de régularisation' ) ; 
  