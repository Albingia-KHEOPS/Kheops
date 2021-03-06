CREATE TABLE ZALBINKHEO.KALCELC ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KALCELC de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KALCELC de ZALBINKHEO. 
	KEPID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KEPKEJID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KEPORD NUMERIC(11, 0) NOT NULL DEFAULT 0 , 
	KEPCNM1 CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KEPCNM2 CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KEPCNM3 NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KEPVER NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KEPNTA CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEPCHII CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEPREG CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEPCHIC CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEPCHIS CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEPCHIO NUMERIC(7, 3) NOT NULL DEFAULT 0 , 
	KEPIAN CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEPIAC CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEPTYPO CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEPAIM CHAR(100) CCSID 297 NOT NULL DEFAULT '' , 
	KEPSCR CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KEPCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEPCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KEPCRH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KEPMAJU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEPMAJD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KEPMAJH NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KEPSIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEPSITU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEPSITD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KEPSITH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KEPTXL CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEPNIV CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KEPTCL CHAR(3) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FALCELC    ; 
  
LABEL ON TABLE ZALBINKHEO.KALCELC 
	IS 'Param  Clauses/El gen                          KEP' ; 
  
LABEL ON COLUMN ZALBINKHEO.KALCELC 
( KEPID IS 'ID unique' , 
	KEPKEJID IS 'Lien KALCELG' , 
	KEPORD IS 'N°ordre' , 
	KEPCNM1 IS 'Code Clause Nom 1' , 
	KEPCNM2 IS 'Code Clause Nom 2' , 
	KEPCNM3 IS 'Code de la clause 3' , 
	KEPVER IS 'Clause N° version' , 
	KEPNTA IS 'Nature Générat°O/P/S' , 
	KEPCHII IS 'Impression Imprimé' , 
	KEPREG IS 'Code                Regroup.' , 
	KEPCHIC IS 'Code empl.          Edition' , 
	KEPCHIS IS 'Sous Chap           Edition' , 
	KEPCHIO IS 'Imp   Ordonnancement' , 
	KEPIAN IS 'Impress annexe O/N' , 
	KEPIAC IS 'Code annexe' , 
	KEPTYPO IS 'Typologie clause' , 
	KEPAIM IS 'Attrib.             Impression' , 
	KEPSCR IS 'Script              Cond.' , 
	KEPCRU IS 'Création User' , 
	KEPCRD IS 'Création date' , 
	KEPCRH IS 'Création Heure' , 
	KEPMAJU IS 'Maj User' , 
	KEPMAJD IS 'MAj Date' , 
	KEPMAJH IS 'Maj Heure' , 
	KEPSIT IS 'Code situation' , 
	KEPSITU IS 'Situation User' , 
	KEPSITD IS 'Situation Date' , 
	KEPSITH IS 'Situation Heure' , 
	KEPTXL IS 'Texte               libre' , 
	KEPNIV IS 'Niveau à            générer' , 
	KEPTCL IS 'Type CLause' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KALCELC 
( KEPID TEXT IS 'ID unique' , 
	KEPKEJID TEXT IS 'Lien KALCELG' , 
	KEPORD TEXT IS 'N° ordre' , 
	KEPCNM1 TEXT IS 'Code clause Nom 1' , 
	KEPCNM2 TEXT IS 'Code Clause Nom 2' , 
	KEPCNM3 TEXT IS 'Code Clause Nom 3' , 
	KEPVER TEXT IS 'Clause N° Version' , 
	KEPNTA TEXT IS 'Nature génération O/P/S' , 
	KEPCHII TEXT IS 'Impression Imprimé CP CG CS...' , 
	KEPREG TEXT IS 'Code regroupement' , 
	KEPCHIC TEXT IS 'Impression Chapitre' , 
	KEPCHIS TEXT IS 'Sous chapitre' , 
	KEPCHIO TEXT IS 'Impression N° ordonnancement' , 
	KEPIAN TEXT IS 'Impression en annexe O/N' , 
	KEPIAC TEXT IS 'Code annexe' , 
	KEPTYPO TEXT IS 'Typologie clause Anodine sensible' , 
	KEPAIM TEXT IS 'Attributs impression' , 
	KEPSCR TEXT IS 'Script conditionnement' , 
	KEPCRU TEXT IS 'Création User' , 
	KEPCRD TEXT IS 'Création date' , 
	KEPCRH TEXT IS 'Création Heure' , 
	KEPMAJU TEXT IS 'Maj User' , 
	KEPMAJD TEXT IS 'Maj Date' , 
	KEPMAJH TEXT IS 'Maj Heure' , 
	KEPSIT TEXT IS 'Situation Code' , 
	KEPSITU TEXT IS 'Situation user' , 
	KEPSITD TEXT IS 'Situation Date' , 
	KEPSITH TEXT IS 'Situation Heure' , 
	KEPTXL TEXT IS 'Texte libre' , 
	KEPNIV TEXT IS 'Niveau à générer' , 
	KEPTCL TEXT IS 'Type clause' ) ; 
  
