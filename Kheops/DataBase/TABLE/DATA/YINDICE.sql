CREATE TABLE ZALBINKHEO.YINDICE ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YINDICE de ZALBINKHEO ignoré. 
	GIIND CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	GIIVA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	GIIVM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	GIIVJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	GIIPA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	GIIPM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	GIIPJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	GIIDV DECIMAL(7, 2) NOT NULL DEFAULT 0 , 
	GIIDU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	GICTL CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	GIECA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	GIECM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	GICRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	GICRA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	GICRM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	GICRJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	GIMJU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	GIMJA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	GIMJM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	GIMJJ NUMERIC(2, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FINDICE    ; 
  
LABEL ON TABLE ZALBINKHEO.YINDICE 
	IS 'Indice : Valeur                                 GI' ; 
  
LABEL ON COLUMN ZALBINKHEO.YINDICE 
( GIIND IS 'Code indice' , 
	GIIVA IS 'Année de validité' , 
	GIIVM IS 'Mois de validité' , 
	GIIVJ IS 'Jour de validité' , 
	GIIPA IS 'Année de parution' , 
	GIIPM IS 'Mois de parution' , 
	GIIPJ IS 'Jour de parution' , 
	GIIDV IS 'Valeur de l''indice' , 
	GIIDU IS 'Unité Valeur' , 
	GICTL IS 'Top O/N/T' , 
	GIECA IS 'Année               terme' , 
	GIECM IS 'Mois                terme' , 
	GICRU IS 'User                cré' , 
	GICRA IS 'Année               cré' , 
	GICRM IS 'Mois                cré' , 
	GICRJ IS 'Jour                cré' , 
	GIMJU IS 'User                màj' , 
	GIMJA IS 'Année               màj' , 
	GIMJM IS 'Mois màj' , 
	GIMJJ IS 'Jour                màj' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YINDICE 
( GIIND TEXT IS 'Code indice' , 
	GIIVA TEXT IS 'Année de validité' , 
	GIIVM TEXT IS 'Mois de validité' , 
	GIIVJ TEXT IS 'Jour de validité' , 
	GIIPA TEXT IS 'Année de parution' , 
	GIIPM TEXT IS 'Mois de parution' , 
	GIIPJ TEXT IS 'Jour de parution' , 
	GIIDV TEXT IS 'Valeur de l''indice' , 
	GIIDU TEXT IS 'Unité Valeur ''D=Montant % )' , 
	GICTL TEXT IS 'A contrôler' , 
	GIECA TEXT IS 'Validité terme : Année' , 
	GIECM TEXT IS 'Validité terme : Mois' , 
	GICRU TEXT IS 'User création' , 
	GICRA TEXT IS 'Année création' , 
	GICRM TEXT IS 'Mois création' , 
	GICRJ TEXT IS 'Jour création' , 
	GIMJU TEXT IS 'User màj' , 
	GIMJA TEXT IS 'Année màj' , 
	GIMJM TEXT IS 'Mois màj' , 
	GIMJJ TEXT IS 'Jour màj' ) ; 
  
