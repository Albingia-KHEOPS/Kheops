CREATE TABLE ZALBINKHEO.YGARANT ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YGARANT de ZALBINKHEO ignoré. 
	GAGAR CHAR(6) CCSID 297 NOT NULL DEFAULT '' , 
	GADES CHAR(60) CCSID 297 NOT NULL DEFAULT '' , 
	GADEA CHAR(30) CCSID 297 NOT NULL DEFAULT '' , 
	GATAX CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	GACNX CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	GACOM CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	GACAR CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	GATBR CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	GADFG CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	GAIFC CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	GAFAM CHAR(6) CCSID 297 NOT NULL DEFAULT '' , 
	GARGE CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	GATRG CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	GAINV CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	GATYI CHAR(20) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FGARANT    ; 
  
LABEL ON TABLE ZALBINKHEO.YGARANT 
	IS 'Garanties                                       GA' ; 
  
LABEL ON COLUMN ZALBINKHEO.YGARANT 
( GAGAR IS 'Code garantie' , 
	GADES IS 'Désignation garantie' , 
	GADEA IS 'Abrégé garantie' , 
	GATAX IS 'Code taxe' , 
	GACNX IS 'Code taxe Cat.Nat' , 
	GACOM IS 'Garant commune O/N' , 
	GACAR IS 'Caractère Garantie' , 
	GATBR IS 'CO/RT/'' ''' , 
	GADFG IS 'Définition garantie' , 
	GAIFC IS 'Info complémentaire' , 
	GAFAM IS 'Famille Garantie' , 
	GARGE IS 'Régularisable O/N' , 
	GATRG IS 'Type de grille régul' , 
	GAINV IS 'Invent Possible O/N' , 
	GATYI IS 'Type Inventaire' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YGARANT 
( GAGAR TEXT IS 'Code garantie' , 
	GADES TEXT IS 'Désignation garantie' , 
	GADEA TEXT IS 'Abrégé garantie' , 
	GATAX TEXT IS 'Code taxe' , 
	GACNX TEXT IS 'Code taxe Catastrophe Naturelle' , 
	GACOM TEXT IS 'Garantie pouvant être commune   O/N' , 
	GACAR TEXT IS 'Caractère de la garantie' , 
	GATBR TEXT IS 'CO ou RT ou '' ''' , 
	GADFG TEXT IS 'Définition garantie (Maintenance ..)' , 
	GAIFC TEXT IS 'Info complémentaire (Maintenance ..)' , 
	GAFAM TEXT IS 'Famille Garantie' , 
	GARGE TEXT IS 'Régularisable O/N' , 
	GATRG TEXT IS 'Type de Grille régularisation' , 
	GAINV TEXT IS 'Inventaire possible O/N' , 
	GATYI TEXT IS 'Type Inventaire' ) ; 
  
