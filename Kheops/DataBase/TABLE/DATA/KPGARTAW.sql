﻿CREATE TABLE ZALBINKHEO.KPGARTAW ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPGARTAW de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPGARTAW de ZALBINKHEO. 
	KDGID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDGTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDGIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KDGALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KDGFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDGOPT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDGGARAN CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDGKDEID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDGNUMTAR NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KDGLCIMOD CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDGLCIOBL CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDGLCIVALO NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGLCIVALA NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGLCIVALW NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGLCIUNIT CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDGLCIBASE CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDGKDIID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDGFRHMOD CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDGFRHOBL CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDGFRHVALO NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGFRHVALA NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGFRHVALW NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGFRHUNIT CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDGFRHBASE CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDGKDKID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDGFMIVALO NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGFMIVALA NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGFMIVALW NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGFMIUNIT CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDGFMIBASE CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDGFMAVALO NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGFMAVALA NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGFMAVALW NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGFMAUNIT CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDGFMABASE CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDGPRIMOD CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDGPRIOBL CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDGPRIVALO NUMERIC(16, 4) NOT NULL DEFAULT 0 , 
	KDGPRIVALA NUMERIC(16, 4) NOT NULL DEFAULT 0 , 
	KDGPRIVALW NUMERIC(16, 4) NOT NULL DEFAULT 0 , 
	KDGPRIUNIT CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDGPRIBASE CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDGMNTBASE NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGPRIMPRO NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGTMC NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGTFF NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGCMC NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGCHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDGCTT NUMERIC(13, 2) NOT NULL DEFAULT 0 )   
	RCDFMT FPGARTAW   ; 
  
LABEL ON TABLE ZALBINKHEO.KPGARTAW 
	IS 'KHEOPS Tarif W' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPGARTAW 
( KDGID IS 'ID unique' , 
	KDGTYP IS 'Type O/P' , 
	KDGIPB IS 'IPB' , 
	KDGALX IS 'ALX' , 
	KDGFOR IS 'Formule' , 
	KDGOPT IS 'Option' , 
	KDGGARAN IS 'Garantie' , 
	KDGKDEID IS 'Lien KPGARAN' , 
	KDGNUMTAR IS 'Numéro Tarif' , 
	KDGLCIMOD IS 'LCI Modifiable' , 
	KDGLCIOBL IS 'LCI Obligatoire' , 
	KDGLCIVALO IS 'LCI Valeur Origine' , 
	KDGLCIVALA IS 'LCI valeur Actualis' , 
	KDGLCIVALW IS 'LCI Valeur Travail' , 
	KDGLCIUNIT IS 'LCI Unité' , 
	KDGLCIBASE IS 'LCI Base' , 
	KDGKDIID IS 'Lien KPEXPLCI' , 
	KDGFRHMOD IS 'Franchise Modifiable' , 
	KDGFRHOBL IS 'Franchise Obligatoir' , 
	KDGFRHVALO IS 'Franchise Valeur Ori' , 
	KDGFRHVALA IS 'Franchise Valeur Act' , 
	KDGFRHVALW IS 'FRanchise Valeur W' , 
	KDGFRHBASE IS 'Franchise Base' , 
	KDGKDKID IS 'Lien KEXPFRH' , 
	KDGFMIVALO IS 'Franchise Mini Ori' , 
	KDGFMIVALA IS 'Franchise Mini Val A' , 
	KDGFMIVALW IS 'Franchise Mini Val W' , 
	KDGFMIUNIT IS 'FRanchise Mini Unité' , 
	KDGFMIBASE IS 'Franchise Mini Base' , 
	KDGFMAVALO IS 'Franchise Max Val Or' , 
	KDGFMAVALA IS 'Franchise maxi Val A' , 
	KDGFMAVALW IS 'Franchise max Val W' , 
	KDGFMAUNIT IS 'Franchise Max Unité' , 
	KDGFMABASE IS 'Franchise Maxi Base' , 
	KDGPRIMOD IS 'Prime Modifiable O/N' , 
	KDGPRIOBL IS 'Prime Obligatoire' , 
	KDGPRIVALO IS 'Prime Valeur Origine' , 
	KDGPRIVALA IS 'Prime Valeur Actual' , 
	KDGPRIVALW IS 'Prime valeur W' , 
	KDGPRIUNIT IS 'Prime Unité' , 
	KDGPRIBASE IS 'Prime Base' , 
	KDGMNTBASE IS 'Prime Montant Base' , 
	KDGPRIMPRO IS 'Prime Provisionnelle' , 
	KDGTMC IS 'Total: Mnt calculé' , 
	KDGTFF IS 'Total Mnt Forcé' , 
	KDGCMC IS 'Comptant mnt calculé' , 
	KDGCHT IS 'Comptant MntForcéHT' , 
	KDGCTT IS 'Comptant MntForcéTTC' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPGARTAW 
( KDGID TEXT IS 'ID unique' , 
	KDGTYP TEXT IS 'Type O/P' , 
	KDGIPB TEXT IS 'IPB' , 
	KDGALX TEXT IS 'ALX' , 
	KDGFOR TEXT IS 'Formule' , 
	KDGOPT TEXT IS 'Option' , 
	KDGGARAN TEXT IS 'Garantie' , 
	KDGKDEID TEXT IS 'Lien KPGARAN' , 
	KDGNUMTAR TEXT IS 'Numéro TARIF' , 
	KDGLCIMOD TEXT IS 'LCI Modifiable' , 
	KDGLCIOBL TEXT IS 'LCI obligatoire' , 
	KDGLCIVALO TEXT IS 'LCI valeur Origine' , 
	KDGLCIVALA TEXT IS 'LCI Valeur Actualisée' , 
	KDGLCIVALW TEXT IS 'LCI Valeur Travail' , 
	KDGLCIUNIT TEXT IS 'LCI Unité' , 
	KDGLCIBASE TEXT IS 'LCI Base' , 
	KDGKDIID TEXT IS 'Lien KPEXPLCI' , 
	KDGFRHMOD TEXT IS 'Franchise Modifiable' , 
	KDGFRHOBL TEXT IS 'Franchise Obligatoire' , 
	KDGFRHVALO TEXT IS 'Franchise Valeur Origine' , 
	KDGFRHVALA TEXT IS 'Franchise Valeur actualisée' , 
	KDGFRHVALW TEXT IS 'Franchise Valeur W' , 
	KDGFRHUNIT TEXT IS 'Franchise Unité' , 
	KDGFRHBASE TEXT IS 'Franchise Base' , 
	KDGKDKID TEXT IS 'Lien KPEXPFRH' , 
	KDGFMIVALO TEXT IS 'Franchise Minimum origine' , 
	KDGFMIVALA TEXT IS 'Franchise Minimum valeur Actualisé' , 
	KDGFMIVALW TEXT IS 'Franchise Minimum Valeur travail' , 
	KDGFMIUNIT TEXT IS 'Franchise Minimum Unité' , 
	KDGFMIBASE TEXT IS 'Franchise minimum Base' , 
	KDGFMAVALO TEXT IS 'Franchise maximum Valeur Origine' , 
	KDGFMAVALA TEXT IS 'Franchise maximum Valeur actualisée' , 
	KDGFMAVALW TEXT IS 'Franchise Maximum Valeur de travail' , 
	KDGFMAUNIT TEXT IS 'Franchise Maximum Unité' , 
	KDGFMABASE TEXT IS 'Franchise maximum Base' , 
	KDGPRIMOD TEXT IS 'Prime Modifiable O/N' , 
	KDGPRIOBL TEXT IS 'Prime Obligatoire' , 
	KDGPRIVALO TEXT IS 'Prime Valeur origine' , 
	KDGPRIVALA TEXT IS 'Prime Valeur Actualisée' , 
	KDGPRIVALW TEXT IS 'Prime Valeur de travail' , 
	KDGPRIUNIT TEXT IS 'Prime Unité' , 
	KDGPRIBASE TEXT IS 'Prime Base' , 
	KDGMNTBASE TEXT IS 'Prime Montant de Base' , 
	KDGPRIMPRO TEXT IS 'Prime Provisionnelle' , 
	KDGTMC TEXT IS 'Total : Montant Calculé' , 
	KDGTFF TEXT IS 'Total : Montant Forcé' , 
	KDGCMC TEXT IS 'Comptant montant Calculé' , 
	KDGCHT TEXT IS 'Comptant Mnt Forcé HT' , 
	KDGCTT TEXT IS 'Comptant Mnt Forcé TTC' ) ; 
  