CREATE TABLE ZALBINKHEO.YCATEGP ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YCATEGP de ZALBINKHEO ignoré. 
	CAPBRA CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	CAPSBR CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	CAPCAT CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	CAPAA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	CAPE1 DECIMAL(7, 0) NOT NULL DEFAULT 0 , 
	CAPE2 DECIMAL(7, 0) NOT NULL DEFAULT 0 , 
	CAPT1 DECIMAL(7, 0) NOT NULL DEFAULT 0 , 
	CAPT2 DECIMAL(7, 0) NOT NULL DEFAULT 0 , 
	CAPP1 DECIMAL(7, 0) NOT NULL DEFAULT 0 , 
	CAPP2 DECIMAL(7, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FCATEGP    ; 
  
LABEL ON TABLE ZALBINKHEO.YCATEGP 
	IS 'Catégorie : Coût de Police                     CAP' ; 
  
LABEL ON COLUMN ZALBINKHEO.YCATEGP 
( CAPBRA IS 'Branche' , 
	CAPSBR IS 'Sous-branche' , 
	CAPCAT IS 'Catégorie' , 
	CAPAA IS 'Année en-cours' , 
	CAPE1 IS 'Mnt frais  acc      En cours Cas 1' , 
	CAPE2 IS 'Mnt frais acc       En cous cas 2' , 
	CAPT1 IS 'Mnt frais           accessoires         Terme futur CAS 1' , 
	CAPT2 IS 'Mnt frais           accessoires         Futur cas 2' , 
	CAPP1 IS 'Mnt frais           accessoires' , 
	CAPP2 IS 'Mnt frais           accessoires         Avant Cas 2' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YCATEGP 
( CAPBRA TEXT IS 'Branche' , 
	CAPSBR TEXT IS 'Sous-branche' , 
	CAPCAT TEXT IS 'Catégorie' , 
	CAPAA TEXT IS 'Année en-cours' , 
	CAPE1 TEXT IS 'Mnt Frais Acc En-cours cas 1' , 
	CAPE2 TEXT IS 'Mnt Frais Acc En-cours cas 2' , 
	CAPT1 TEXT IS 'Mnt Frais Acc Futur cas 1' , 
	CAPT2 TEXT IS 'Mnt Frais Acc Futur cas 2' , 
	CAPP1 TEXT IS 'Mnt Frais Acc Avant cas 1' , 
	CAPP2 TEXT IS 'Mnt Frais Acc Avant Cas 2' ) ; 
  
