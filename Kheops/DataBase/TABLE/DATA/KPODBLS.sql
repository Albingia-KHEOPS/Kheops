CREATE TABLE ZALBINKHEO.KPODBLS ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPODBLS de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPODBLS de ZALBINKHEO. 
	KAFID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KAFTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KAFIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KAFALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KAFALX. 
	KAFICT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KAFSOU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KAFSAID NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KAFSAIH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KAFSIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KAFSITD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KAFSITH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KAFSITU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KAFCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KAFCRH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KAFCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KAFACT CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KAFMOT CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KAFIN5 NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KAFOCT CHAR(25) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPODBLS    ; 
  
LABEL ON TABLE ZALBINKHEO.KPODBLS 
	IS 'Offre Double saisie                            KAF' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPODBLS 
( KAFID IS 'ID unique' , 
	KAFTYP IS 'Type O/P' , 
	KAFIPB IS 'N° Offre' , 
	KAFALX IS 'N° version' , 
	KAFICT IS 'Identifiant courtier' , 
	KAFSOU IS 'Souscripteur' , 
	KAFSAID IS 'Saisie Date' , 
	KAFSAIH IS 'Saisie Heure' , 
	KAFSIT IS 'Situation A/X' , 
	KAFSITD IS 'Situation Date' , 
	KAFSITH IS 'Situation Heure' , 
	KAFSITU IS 'Situation User' , 
	KAFCRD IS 'Création Date' , 
	KAFCRH IS 'Création Heure' , 
	KAFCRU IS 'Création User' , 
	KAFACT IS 'Acte' , 
	KAFMOT IS 'Motif' , 
	KAFIN5 IS 'Code interlocuteur' , 
	KAFOCT IS 'Ref contrat chez Co' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPODBLS 
( KAFID TEXT IS 'ID unique' , 
	KAFTYP TEXT IS 'Type  O Offre' , 
	KAFIPB TEXT IS 'N° Offre' , 
	KAFALX TEXT IS 'N° version' , 
	KAFICT TEXT IS 'Identifiant courtier' , 
	KAFSOU TEXT IS 'Souscripteur' , 
	KAFSAID TEXT IS 'Saisie Date' , 
	KAFSAIH TEXT IS 'Saisie Heure' , 
	KAFSIT TEXT IS 'Situation A/X' , 
	KAFSITD TEXT IS 'Situation Date' , 
	KAFSITH TEXT IS 'Situation Heure' , 
	KAFSITU TEXT IS 'Situation user' , 
	KAFCRD TEXT IS 'Création Date' , 
	KAFCRH TEXT IS 'Création Heure' , 
	KAFCRU TEXT IS 'Création User' , 
	KAFACT TEXT IS 'Acte  App Ini, Ref refus Rem rempl' , 
	KAFMOT TEXT IS 'Motif' , 
	KAFIN5 TEXT IS 'Code interlocuteur sur 5' , 
	KAFOCT TEXT IS 'Référence Contrat chez courtier' ) ; 
  
