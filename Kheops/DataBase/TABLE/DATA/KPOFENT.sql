CREATE TABLE ZALBINKHEO.KPOFENT ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPOFENT de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPOFENT de ZALBINKHEO. 
	KFHPOG CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KFHALG NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFHALG. 
	KFHIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KFHALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFHALX. 
	KFHNPO DECIMAL(2, 0) NOT NULL DEFAULT 0 , 
	KFHEFD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KFHSAD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KFHBRA CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KFHCIBLE CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KFHIPR CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KFHALR NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFHALR. 
	KFHTYPO CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFHIPM CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KHFSIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFHSTU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KFHSTD NUMERIC(8, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPOFENT    ; 
  
LABEL ON TABLE ZALBINKHEO.KPOFENT 
	IS 'Contrat à établir ENTETE                       KFH' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPOFENT 
( KFHPOG IS 'N° Contrat  généré' , 
	KFHALG IS 'N° Aliment généré' , 
	KFHIPB IS 'Offre (Code)' , 
	KFHALX IS 'Offre (Version)' , 
	KFHNPO IS 'Chrono police' , 
	KFHEFD IS 'Date Effet' , 
	KFHSAD IS 'Date Accord' , 
	KFHBRA IS 'Branche' , 
	KFHCIBLE IS 'Cible' , 
	KFHIPR IS 'Pol.rempl (Code)' , 
	KFHALR IS 'Pol Rempl. (Alim.)' , 
	KFHTYPO IS 'Typo contrat' , 
	KFHIPM IS 'Police mère' , 
	KHFSIT IS 'Situation Code' , 
	KFHSTU IS 'Situation User' , 
	KFHSTD IS 'Situation Date' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPOFENT 
( KFHPOG TEXT IS 'N° de Contrat généré' , 
	KFHALG TEXT IS 'N° Aliment généré' , 
	KFHIPB TEXT IS 'Offre (Code)' , 
	KFHALX TEXT IS 'Offre (Version)' , 
	KFHNPO TEXT IS 'N° chrono police' , 
	KFHEFD TEXT IS 'Date effet' , 
	KFHSAD TEXT IS 'Date Accord' , 
	KFHBRA TEXT IS 'Branche' , 
	KFHCIBLE TEXT IS 'Cible' , 
	KFHIPR TEXT IS 'Police remplacée (Code)' , 
	KFHALR TEXT IS 'Police remplacée (Aliment)' , 
	KFHTYPO TEXT IS 'Typo Contrat  Aliment Mère Contrat' , 
	KFHIPM TEXT IS 'Police mère' , 
	KHFSIT TEXT IS 'Situation Code (A ,V)' , 
	KFHSTU TEXT IS 'Situation User' , 
	KFHSTD TEXT IS 'Situation Date' ) ; 
  
