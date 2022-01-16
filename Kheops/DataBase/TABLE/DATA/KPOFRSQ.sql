CREATE TABLE ZALBINKHEO.KPOFRSQ ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPOFRSQ de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPOFRSQ de ZALBINKHEO. 
	KFIPOG CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KFIALG NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFIALG. 
	KFIIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KFIALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFIALX. 
	KFICHR NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KFITYE CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFIRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFIRSQ. 
	KFIOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFIOBJ. 
	KFIINV CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFISEL CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPOFRSQ    ; 
  
LABEL ON TABLE ZALBINKHEO.KPOFRSQ 
	IS 'Contrat à établir RSQ                          KFI' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPOFRSQ 
( KFIPOG IS 'N° de contrat généré' , 
	KFIALG IS 'N° Aliment généré' , 
	KFIIPB IS 'Offre (Code)' , 
	KFIALX IS 'Offre (Version)' , 
	KFICHR IS 'Chrono Affichage ID' , 
	KFITYE IS 'Type enregistrement' , 
	KFIRSQ IS 'Identifiant risque' , 
	KFIOBJ IS 'Identifiant objet' , 
	KFIINV IS 'inventaire O/N' , 
	KFISEL IS 'Sélection O/N' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPOFRSQ 
( KFIPOG TEXT IS 'N° de Contrat généré' , 
	KFIALG TEXT IS 'N° Aliment généré' , 
	KFIIPB TEXT IS 'Offre (Code)' , 
	KFIALX TEXT IS 'Offre (Version)' , 
	KFICHR TEXT IS 'N° Chrono Affichage ID unique' , 
	KFITYE TEXT IS 'Type enregistrement  Risque Objet' , 
	KFIRSQ TEXT IS 'Identifiant risque' , 
	KFIOBJ TEXT IS 'Identifiant objet' , 
	KFIINV TEXT IS 'inventaire  O/N' , 
	KFISEL TEXT IS 'Sélection O/N' ) ; 
  
