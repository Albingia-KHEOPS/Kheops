CREATE TABLE ZALBINKHEO.YPRIPGA ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPRIPGA de ZALBINKHEO ignoré. 
	PLIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	PLALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne PLALX. 
	PLTYE CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PLGAR CHAR(6) CCSID 297 NOT NULL DEFAULT '' , 
	PLMHT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PLMTX DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PLTAX CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	PLTXV DECIMAL(7, 3) NOT NULL DEFAULT 0 , 
	PLTXU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PLMTT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PLXF1 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PLMX1 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PLXF2 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PLMX2 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PLXF3 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PLMX3 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PLSUP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PLGAP NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PLKHT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PLKHX DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PLKTT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PLKX1 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PLKX2 DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PLKX3 DECIMAL(11, 2) NOT NULL DEFAULT 0 )   
	RCDFMT FPRIPGA    ; 
  
LABEL ON TABLE ZALBINKHEO.YPRIPGA 
	IS 'Primes-W: /Garanties                            PL' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRIPGA 
( PLIPB IS 'N° de police' , 
	PLALX IS 'N° Aliment' , 
	PLTYE IS 'Type enregistrement' , 
	PLGAR IS 'Code garantie' , 
	PLMHT IS 'Montant HT' , 
	PLMTX IS 'Montant de taxe' , 
	PLTAX IS 'Code taxe' , 
	PLTXV IS 'Valeur code taxe' , 
	PLTXU IS 'Unité code Taxe' , 
	PLMTT IS 'Montant TTC' , 
	PLXF1 IS 'Famille de taxe 1' , 
	PLMX1 IS 'Montant de taxe 1' , 
	PLXF2 IS 'Famille de taxe 2' , 
	PLMX2 IS 'Montant de taxe 2' , 
	PLXF3 IS 'Famille de taxe 3' , 
	PLMX3 IS 'Montant de taxe 3' , 
	PLSUP IS 'Garantie supprim O/N' , 
	PLGAP IS 'N° ordre            Présentation' , 
	PLKHT IS 'Mnt HT          DevC' , 
	PLKHX IS 'Mnt taxes       DevC' , 
	PLKTT IS 'Mnt TTC         DevC' , 
	PLKX1 IS 'Mnt de taxe 1   DevC' , 
	PLKX2 IS 'Mnt taxe 2      DevC' , 
	PLKX3 IS 'Mnt de taxe 3   DevC' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRIPGA 
( PLIPB TEXT IS 'N° de Police' , 
	PLALX TEXT IS 'N° Aliment' , 
	PLTYE TEXT IS 'Typ enreg: 1 Part Albing 2 Prime tot' , 
	PLGAR TEXT IS 'Code garantie' , 
	PLMHT TEXT IS 'Montant HT (Hors CATNAT)' , 
	PLMTX TEXT IS 'Montant de taxe (Hors CATNAT)' , 
	PLTAX TEXT IS 'Code taxe' , 
	PLTXV TEXT IS 'Valeur code taxe' , 
	PLTXU TEXT IS 'Unité code taxe' , 
	PLMTT TEXT IS 'Montant TTC  (Hors CATNAT)' , 
	PLXF1 TEXT IS 'Famille de taxe 1' , 
	PLMX1 TEXT IS 'Montant de taxe 1' , 
	PLXF2 TEXT IS 'Famille de taxe 2' , 
	PLMX2 TEXT IS 'Montant de taxe 2' , 
	PLXF3 TEXT IS 'Famille de taxe 3' , 
	PLMX3 TEXT IS 'Montant de taxe 3' , 
	PLSUP TEXT IS 'Garantie supprimée O/N' , 
	PLGAP TEXT IS 'N° ordre présentation' , 
	PLKHT TEXT IS 'Montant HT (Hors CATNAT)     Dev Cpt' , 
	PLKHX TEXT IS 'Montant de taxe(Hors CATNAT) Dev Cpt' , 
	PLKTT TEXT IS 'Montant TTC (hors CATNAT)    Dev Cpt' , 
	PLKX1 TEXT IS 'Montant de taxe 1            Dev Cpt' , 
	PLKX2 TEXT IS 'Montant de taxe 2            Dev Cpt' , 
	PLKX3 TEXT IS 'Montant de taxe 3            Dev Cpt' ) ; 
  
