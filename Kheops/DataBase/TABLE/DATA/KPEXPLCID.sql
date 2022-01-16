﻿CREATE TABLE ZALBINKHEO.KPEXPLCID ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPEXPLCID de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPEXPLCID de ZALBINKHEO. 
	KDJID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDJKDIID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDJORDRE NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDJLCVAL NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDJLCVAU CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDJLCBASE CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDJLOVAL NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDJLOVAU CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDJLOBASE CHAR(3) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPEXPLCID  ; 
  
LABEL ON TABLE ZALBINKHEO.KPEXPLCID 
	IS 'KHEOPS Exp Complexe LCI Détail' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPEXPLCID 
( KDJID IS 'ID unique' , 
	KDJKDIID IS 'Lien KPEXPLCI' , 
	KDJORDRE IS 'N° Ordre' , 
	KDJLCVAL IS 'Valeur' , 
	KDJLCVAU IS 'Unité' , 
	KDJLCBASE IS 'Base' , 
	KDJLOVAL IS 'Concurrence Valeur' , 
	KDJLOVAU IS 'Concurrence Unité' , 
	KDJLOBASE IS 'Concurrence Base' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPEXPLCID 
( KDJID TEXT IS 'ID unique' , 
	KDJKDIID TEXT IS 'Lien KPEXPLCI' , 
	KDJORDRE TEXT IS 'N° Ordre' , 
	KDJLCVAL TEXT IS 'Valeur' , 
	KDJLCVAU TEXT IS 'Unité' , 
	KDJLCBASE TEXT IS 'Base' , 
	KDJLOVAL TEXT IS 'Concurrence Valeur' , 
	KDJLOVAU TEXT IS 'Concurrence Unité' , 
	KDJLOBASE TEXT IS 'Concurrence Base' ) ; 
  