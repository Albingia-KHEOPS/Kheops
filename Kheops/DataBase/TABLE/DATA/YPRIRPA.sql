﻿CREATE TABLE ZALBINKHEO.YPRIRPA ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPRIRPA de ZALBINKHEO ignoré. 
	POIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	POALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne POALX. 
	POTYE CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	POCIE CHAR(6) CCSID 297 NOT NULL DEFAULT '' , 
	POAPP DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	POPOL CHAR(25) CCSID 297 NOT NULL DEFAULT '' , 
	POMHT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POMTX DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POAFR DECIMAL(7, 0) NOT NULL DEFAULT 0 , 
	POAFT DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	POATM DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	POTTT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POMTT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POCOT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POCOM DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	POTXF DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	POMFA DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POCNH DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POCNT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POCNL DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POCNM DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POCNC DECIMAL(5, 3) NOT NULL DEFAULT 0 , 
	POKHT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POKHX DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POKFA DECIMAL(7, 2) NOT NULL DEFAULT 0 , 
	POKFT DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	POKAT DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	POKTX DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POKTT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POKCO DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POKAP DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POKNH DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POKNT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POKNL DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	POKNM DECIMAL(11, 2) NOT NULL DEFAULT 0 )   
	RCDFMT FPRIRPA    ; 
  
LABEL ON TABLE ZALBINKHEO.YPRIRPA 
	IS 'PrimW-Régul: Part Coass Aper                    PO' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRIRPA 
( POIPB IS 'N° de police / Offre' , 
	POALX IS 'N° Aliment' , 
	POTYE IS 'Type enregistement' , 
	POCIE IS 'Identifiant Cie' , 
	POAPP IS '% Apérition/Coass' , 
	POPOL IS 'Réf police chez coas' , 
	POMHT IS 'Montant prime HT' , 
	POMTX IS 'Montant de taxe' , 
	POAFR IS 'Mnt frais           accessoires' , 
	POAFT IS 'Taxe sur accessoire' , 
	POATM IS 'Taxe Attentat' , 
	POTTT IS 'Total Taxes' , 
	POMTT IS 'Montant prime TTC' , 
	POCOT IS 'Total commission' , 
	POCOM IS 'Part comm apérition' , 
	POTXF IS 'Taux de frais apérit' , 
	POMFA IS 'Montant frais apér.' , 
	POCNH IS 'CATNAT Montant HT' , 
	POCNT IS 'CATNAT : Mnt de taxe' , 
	POCNL IS 'CATNAT Montant TTC' , 
	POCNM IS 'CATNAT Total Commiss' , 
	POCNC IS 'CATNAT  Taux commissCat Nat' , 
	POKHT IS 'Mnt HT avec CN  DevC' , 
	POKHX IS 'Mnt taxe horsCN DevC' , 
	POKFA IS 'Mnt frais       DevCaccessoires' , 
	POKFT IS 'Taxe accessoire DevC' , 
	POKAT IS 'Taxe Attentat   DevC' , 
	POKTX IS 'Total Taxes     DevC' , 
	POKTT IS 'Mnt prime TTC   DevC' , 
	POKCO IS 'Mnt commissions DevC' , 
	POKAP IS 'Mnt Frais Apér  DevC' , 
	POKNH IS 'CATNAT HT       DevC' , 
	POKNT IS 'CATNAT mnt taxe DevC' , 
	POKNL IS 'CATNAT TTC      DevC' , 
	POKNM IS 'CATNAT Mnt Comm DevC' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRIRPA 
( POIPB TEXT IS 'N° de Police' , 
	POALX TEXT IS 'N° Aliment' , 
	POTYE TEXT IS 'Type enreg: 1 Part Albingia  2 Coass' , 
	POCIE TEXT IS 'Identifiant Compagnie' , 
	POAPP TEXT IS '% Apérition/Coass' , 
	POPOL TEXT IS 'Référence police chez Coassureur' , 
	POMHT TEXT IS 'Montant prime HT (Avec CATNAT)' , 
	POMTX TEXT IS 'Montant de taxe (Hors CATNAT)' , 
	POAFR TEXT IS 'Montant de frais accessoires' , 
	POAFT TEXT IS 'Taxe sur accessoire' , 
	POATM TEXT IS 'Taxe attentat' , 
	POTTT TEXT IS 'Total taxes (Avec CATNAT)' , 
	POMTT TEXT IS 'Montant prime TTC (Avec CATNAT)' , 
	POCOT TEXT IS 'Total commissions' , 
	POCOM TEXT IS 'Part de commissionnement' , 
	POTXF TEXT IS 'Taux de frais apérition' , 
	POMFA TEXT IS 'Montant Frais d''apérition' , 
	POCNH TEXT IS 'CATNAT : Montant HT' , 
	POCNT TEXT IS 'CATNAT : Montant de taxe' , 
	POCNL TEXT IS 'CATNAT : Montant TTC' , 
	POCNM TEXT IS 'CATNAT : Total commission' , 
	POCNC TEXT IS 'CATNAT : Taux de commission' , 
	POKHT TEXT IS 'Mnt prime HT (Avec CATNAT)   Dev Cpt' , 
	POKHX TEXT IS 'Mnt de taxe (Hors CATNAT)    Dev Cpt' , 
	POKFA TEXT IS 'Mnt frais accessoires        Dev Cpt' , 
	POKFT TEXT IS 'Taxe sur accessoire          Dev Cpt' , 
	POKAT TEXT IS 'Taxe attentat                Dev Cpt' , 
	POKTX TEXT IS 'Total taxes (Avec CATNAT)    Dev Cpt' , 
	POKTT TEXT IS 'Montant prime TTC            Dev Cpt' , 
	POKCO TEXT IS 'Mnt commissions (BM si RT)   Dev Cpt' , 
	POKAP TEXT IS 'Montant Frais d''apérition    Dev Cpt' , 
	POKNH TEXT IS 'CATNAT : Montant HT          Dev Cpt' , 
	POKNT TEXT IS 'CATNAT : Montant de taxe     Dev Cpt' , 
	POKNL TEXT IS 'CATNAT : Montant TTC         Dev Cpt' , 
	POKNM TEXT IS 'CATNAT : Mnt commissions     Dev Cpt' ) ; 
  
