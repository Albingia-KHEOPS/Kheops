﻿CREATE TABLE ZALBINKHEO.KNMGRI ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KNMGRI de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KNMGRI de ZALBINKHEO. 
	KHJNMG CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KHJDESI CHAR(60) CCSID 297 NOT NULL DEFAULT '' , 
	KHJTYPO1 CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KHJLIB1 CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KHJLIEN1 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHJVALF1 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHJTYPO2 CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KHJLIB2 CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KHJLIEN2 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHJVALF2 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHJTYPO3 CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KHJLIB3 CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KHJLIEN3 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHJVALF3 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHJTYPO4 CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KHJLIB4 CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KHJLIEN4 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHJVALF4 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHJTYPO5 CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KHJLIB5 CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KHJLIEN5 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHJVALF5 CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FNMGRI     ; 
  
LABEL ON TABLE ZALBINKHEO.KNMGRI 
	IS 'Nomenclature Grille                            KHJ' ; 
  
LABEL ON COLUMN ZALBINKHEO.KNMGRI 
( KHJNMG IS 'Code Grille' , 
	KHJDESI IS 'Désignation' , 
	KHJTYPO1 IS 'Nomenclature 1 Typo' , 
	KHJLIB1 IS 'Libellé 1' , 
	KHJLIEN1 IS 'Lien 1' , 
	KHJVALF1 IS 'Valeur Filtrée 1' , 
	KHJTYPO2 IS 'Typologie 2' , 
	KHJLIB2 IS 'Libellé 2' , 
	KHJLIEN2 IS 'Lien 2' , 
	KHJVALF2 IS 'Valeurs Filtrées 2' , 
	KHJTYPO3 IS 'Typologie 3' , 
	KHJLIB3 IS 'Libellé 3' , 
	KHJLIEN3 IS 'Lien 3' , 
	KHJVALF3 IS 'Valeurs Filtrées 3' , 
	KHJTYPO4 IS 'Typologie 4' , 
	KHJLIB4 IS 'Libellé 4' , 
	KHJLIEN4 IS 'Lien 4' , 
	KHJVALF4 IS 'Valeurs Filtrées 4' , 
	KHJTYPO5 IS 'Typologie 5' , 
	KHJLIB5 IS 'Libellé 5' , 
	KHJLIEN5 IS 'Lien 5' , 
	KHJVALF5 IS 'Valeurs Filtrées 5' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KNMGRI 
( KHJNMG TEXT IS 'Code Grille' , 
	KHJDESI TEXT IS 'Désignation' , 
	KHJTYPO1 TEXT IS 'Typologie 1' , 
	KHJLIB1 TEXT IS 'Libellé 1' , 
	KHJLIEN1 TEXT IS 'Lien 1  I Indép 1 Mère 2,3..Fille' , 
	KHJVALF1 TEXT IS 'Valeurs Filtrées1 O/N si Indépendant' , 
	KHJTYPO2 TEXT IS 'Typologie 2' , 
	KHJLIB2 TEXT IS 'Libellé 2' , 
	KHJLIEN2 TEXT IS 'Lien 2' , 
	KHJVALF2 TEXT IS 'Valeurs Filtrées 2' , 
	KHJTYPO3 TEXT IS 'Typologie 3' , 
	KHJLIB3 TEXT IS 'Libellé 3' , 
	KHJLIEN3 TEXT IS 'Lien 3' , 
	KHJVALF3 TEXT IS 'Valeurs filtrées 3' , 
	KHJTYPO4 TEXT IS 'Typologie 4' , 
	KHJLIB4 TEXT IS 'Libellé 4' , 
	KHJLIEN4 TEXT IS 'Lien 4' , 
	KHJVALF4 TEXT IS 'Valeurs Filtrées 4' , 
	KHJTYPO5 TEXT IS 'Typologie 5' , 
	KHJLIB5 TEXT IS 'Libellé 5' , 
	KHJLIEN5 TEXT IS 'Lien 5' , 
	KHJVALF5 TEXT IS 'Valeurs Filtrées 5' ) ; 
  
