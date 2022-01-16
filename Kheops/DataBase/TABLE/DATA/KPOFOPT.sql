﻿CREATE TABLE ZALBINKHEO.KPOFOPT ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPOFOPT de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPOFOPT de ZALBINKHEO. 
	KFJPOG CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KFJALG NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFJALG. 
	KFJIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KFJALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFJALX. 
	KFJCHR NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KFJTENG CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFJFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KFJOPT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KFJKDAID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KFJKDBID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KFJKAKID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KFJSEL CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPOFOPT    ; 
  
LABEL ON TABLE ZALBINKHEO.KPOFOPT 
	IS 'Contrat à établir Option                       KFJ' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPOFOPT 
( KFJPOG IS 'N° de contrat généré' , 
	KFJALG IS 'N°Aliment généré' , 
	KFJIPB IS 'Offre (Code)' , 
	KFJALX IS 'Offre (Version)' , 
	KFJCHR IS 'Chrono Affichage ID' , 
	KFJTENG IS 'Type enregistrement' , 
	KFJFOR IS 'Formule' , 
	KFJOPT IS 'Option' , 
	KFJKDAID IS 'Lien KPFOR' , 
	KFJKDBID IS 'Lien KPOPT' , 
	KFJKAKID IS 'Lien KVOLET' , 
	KFJSEL IS 'Sélection O/N' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPOFOPT 
( KFJPOG TEXT IS 'N° de Contrat généré' , 
	KFJALG TEXT IS 'N° Aliment généré' , 
	KFJIPB TEXT IS 'Offre (Code)' , 
	KFJALX TEXT IS 'Offre (Version)' , 
	KFJCHR TEXT IS 'N° Chrono Affichage ID unique' , 
	KFJTENG TEXT IS 'Type enregistrement F/O/V' , 
	KFJFOR TEXT IS 'Formule' , 
	KFJOPT TEXT IS 'Option' , 
	KFJKDAID TEXT IS 'Lien KPFOR' , 
	KFJKDBID TEXT IS 'Lien KPOPT' , 
	KFJKAKID TEXT IS 'Lien KVOLET' , 
	KFJSEL TEXT IS 'Sélection  O/N' ) ; 
  