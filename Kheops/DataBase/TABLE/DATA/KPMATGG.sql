﻿CREATE TABLE ZALBINKHEO.KPMATGG ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPMATGG de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPMATGG de ZALBINKHEO. 
	KEETYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEEIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KEEALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KEECHR NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KEETYE CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEEKDCID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KEEVOLET CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEEKAKID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KEEKAKID. 
	KEEBLOC CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEEKAEID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KEEGARAN CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEESEQ NUMERIC(10, 0) NOT NULL DEFAULT 0 , 
	KEENIV NUMERIC(1, 0) NOT NULL DEFAULT 0 , 
	KEEVID CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPMATGG    ; 
  
LABEL ON TABLE ZALBINKHEO.KPMATGG 
	IS 'KHEOPS Matrice/Risque Garantie' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPMATGG 
( KEETYP IS 'Type O/P' , 
	KEEIPB IS 'IPB' , 
	KEEALX IS 'ALX' , 
	KEECHR IS 'Chrono Affichage ID' , 
	KEETYE IS 'Type enregistrement' , 
	KEEKDCID IS 'Lien KPOPTD' , 
	KEEVOLET IS 'Volet' , 
	KEEKAKID IS 'Lien KVOLET' , 
	KEEBLOC IS 'Bloc' , 
	KEEKAEID IS 'Lien KBLOC' , 
	KEEGARAN IS 'Garantie' , 
	KEESEQ IS 'Séquence Garantie' , 
	KEENIV IS 'Niveau Garantie' , 
	KEEVID IS 'Non Affectée O/N' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPMATGG 
( KEETYP TEXT IS 'Type O/P' , 
	KEEIPB TEXT IS 'IPB' , 
	KEEALX TEXT IS 'ALX' , 
	KEECHR TEXT IS 'N° Chrono Affichage ID unique' , 
	KEETYE TEXT IS 'Type enregistrement  Volet Bloc Gar' , 
	KEEKDCID TEXT IS 'Lien KPOPTD' , 
	KEEVOLET TEXT IS 'Volet' , 
	KEEKAKID TEXT IS 'Lien KVOLET' , 
	KEEBLOC TEXT IS 'Bloc' , 
	KEEKAEID TEXT IS 'Lien KBLOC' , 
	KEEGARAN TEXT IS 'Garantie' , 
	KEESEQ TEXT IS 'Séquence garantie' , 
	KEENIV TEXT IS 'Niveau Garantie' , 
	KEEVID TEXT IS 'Non Affectée O/N' ) ; 
  