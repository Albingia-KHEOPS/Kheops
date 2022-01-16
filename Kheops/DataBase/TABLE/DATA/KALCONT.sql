CREATE TABLE ZALBINKHEO.KALCONT ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KALCONT de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KALCONT de ZALBINKHEO. 
	KEIID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KEISERV CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEIACTG CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEIETAPE CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEIKEHID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KEICTX CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEIEMOD CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEIAJC CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEIAJT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEIMDT1 CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KEIMDT2 CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KEIMDT3 NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KEISCID2 CHAR(50) CCSID 297 NOT NULL DEFAULT '' , 
	KEICHI CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEICHIS CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEIIMP NUMERIC(10, 0) NOT NULL DEFAULT 0 , 
	KEICXI NUMERIC(7, 3) NOT NULL DEFAULT 0 , 
	KEIEXT CHAR(1) CCSID 297 NOT NULL DEFAULT ' ' )   
	RCDFMT FALCONT    ; 
  
LABEL ON TABLE ZALBINKHEO.KALCONT 
	IS 'Param  Contextes                               KEI' ; 
  
LABEL ON COLUMN ZALBINKHEO.KALCONT 
( KEIID IS 'ID unique' , 
	KEISERV IS 'Service' , 
	KEIACTG IS 'Acte de gestion' , 
	KEIETAPE IS 'Etape' , 
	KEIKEHID IS 'Lien KALETAP' , 
	KEICTX IS 'Contexte' , 
	KEIEMOD IS 'Emplace édit modif' , 
	KEIAJC IS 'Ajout Clause O/N' , 
	KEIAJT IS 'Ajout Texte O/N' , 
	KEIMDT1 IS 'Clause modèle NM1' , 
	KEIMDT2 IS 'Clause Modèle NM2' , 
	KEIMDT3 IS 'Clause modèle NM3' , 
	KEISCID2 IS 'Lien Script Ctrl' , 
	KEICHI IS 'Chapitre impression' , 
	KEICHIS IS 'Sous chapitre Impres' , 
	KEIIMP IS 'N° Impression' , 
	KEICXI IS 'N° ordonnancement' , 
	KEIEXT IS 'Spécificité J/G/S/A  ' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KALCONT 
( KEIID TEXT IS 'ID unique' , 
	KEISERV TEXT IS 'Service  (Produ,Sinistre...)' , 
	KEIACTG TEXT IS 'Acte de gestion' , 
	KEIETAPE TEXT IS 'Etape' , 
	KEIKEHID TEXT IS 'Lien KALETAP' , 
	KEICTX TEXT IS 'Contexte' , 
	KEIEMOD TEXT IS 'Emplacement édition modifiable O/N' , 
	KEIAJC TEXT IS 'Ajout Clause autorisée O/N' , 
	KEIAJT TEXT IS 'Ajout texte autorisé  O/N' , 
	KEIMDT1 TEXT IS 'Clause modele NM1' , 
	KEIMDT2 TEXT IS 'Clause Modèle NM2' , 
	KEIMDT3 TEXT IS 'Clause Modèle NM3' , 
	KEISCID2 TEXT IS 'Lien Script de Contrôle' , 
	KEICHI TEXT IS 'Chapitre impression' , 
	KEICHIS TEXT IS 'Sous chapitre Impression' , 
	KEIIMP TEXT IS 'N° Impression' , 
	KEICXI TEXT IS 'N° ordonnancement' , 
	KEIEXT TEXT IS 'Spécif J DocJoint G cg S cs A csAnx' ) ; 
  
