CREATE TABLE ZALBINKHEO.KALETAP ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KALETAP de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KALETAP de ZALBINKHEO. 
	KEHID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KEHSERV CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEHACTG CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEHKEGID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KEHETAPE CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KEHETORD NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KEHGCLA CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEHSAIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEHNIVE NUMERIC(2, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FALETAP    ; 
  
LABEL ON TABLE ZALBINKHEO.KALETAP 
	IS 'Param  Etapes                                  KEH' ; 
  
LABEL ON COLUMN ZALBINKHEO.KALETAP 
( KEHID IS 'ID unique' , 
	KEHSERV IS 'Service' , 
	KEHACTG IS 'Acte de gestion' , 
	KEHKEGID IS 'Lien KALACTG' , 
	KEHETAPE IS 'Etape' , 
	KEHETORD IS 'N°Ordre de l''étape' , 
	KEHGCLA IS 'Aff Gestion O/N' , 
	KEHSAIT IS 'Aff saisie tableur' , 
	KEHNIVE IS 'Niveau Etape' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KALETAP 
( KEHID TEXT IS 'ID unique' , 
	KEHSERV TEXT IS 'Service  (Produ,Sinistre...)' , 
	KEHACTG TEXT IS 'Acte de gestion' , 
	KEHKEGID TEXT IS 'Lien KALACTG' , 
	KEHETAPE TEXT IS 'Etape' , 
	KEHETORD TEXT IS 'N° ordre de l''étape' , 
	KEHGCLA TEXT IS 'Affichage dans Gestion O/N' , 
	KEHSAIT TEXT IS 'Affichage dans saisie tableur' , 
	KEHNIVE TEXT IS 'Niveau Etape 10 Infgen 30 rsq 50 for' ) ; 
  
