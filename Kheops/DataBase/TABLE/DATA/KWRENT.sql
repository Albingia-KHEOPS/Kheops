CREATE TABLE ZALBINKHEO.KWRENT ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KWRENT de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KWRENT de ZALBINKHEO. 
	W1TYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	W1IPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	W1ALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne W1ALX. 
	W1AVN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	W1HIN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	W1RSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	W1OBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	W1OPT CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	W1CSIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	W1USIT CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	W1DSIT NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	W1TSIT CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	W1NJOB NUMERIC(6, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FWRENT     ; 
  
LABEL ON TABLE ZALBINKHEO.KWRENT 
	IS 'Kheops:controle recup ent TYP/IPB/ALX/OPT/RSQ/OBJ' ; 
  
LABEL ON COLUMN ZALBINKHEO.KWRENT 
( W1TYP IS 'Type  O/P' , 
	W1IPB IS 'N° de police / Offre' , 
	W1ALX IS 'N° Aliment / connexe' , 
	W1AVN IS 'N° avenant' , 
	W1HIN IS 'N° histo' , 
	W1RSQ IS 'Id risque' , 
	W1OBJ IS 'Id objet' , 
	W1OPT IS 'Option récupération' , 
	W1CSIT IS 'Code situation' , 
	W1USIT IS 'User situation' , 
	W1DSIT IS 'Date situation' , 
	W1TSIT IS 'Commentaire         situation' , 
	W1NJOB IS 'N° travail' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KWRENT 
( W1TYP TEXT IS 'Type  O Offre  P Police' , 
	W1IPB TEXT IS 'N° police/offre' , 
	W1ALX TEXT IS 'N° aliment' , 
	W1AVN TEXT IS 'N° avenant' , 
	W1HIN TEXT IS 'N° histo' , 
	W1RSQ TEXT IS 'Id risque' , 
	W1OBJ TEXT IS 'Id objet' , 
	W1OPT TEXT IS 'Option récupération' , 
	W1CSIT TEXT IS 'Code situation' , 
	W1USIT TEXT IS 'User situation' , 
	W1DSIT TEXT IS 'Date situation' , 
	W1TSIT TEXT IS 'Commentaire situation' , 
	W1NJOB TEXT IS 'N° travail' ) ; 
  
