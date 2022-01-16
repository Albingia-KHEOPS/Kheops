CREATE TABLE ZALBINKHEO.HWADR ( 
--  SQL150B   10   REUSEDLT(*NO) de la table HWADR de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour HWADR de ZALBINKHEO. 
	W3ORI CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	W3IDORI NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	W3TYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	W3IPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	W3ALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	W3AVN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	W3HIN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	W3NOM CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	W3AD1 CHAR(32) CCSID 297 NOT NULL DEFAULT '' , 
	W3AD2 CHAR(32) CCSID 297 NOT NULL DEFAULT '' , 
	W3COM CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	W3VIL CHAR(26) CCSID 297 NOT NULL DEFAULT '' , 
	W3PAY CHAR(3) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT HWADR      ; 
  
LABEL ON TABLE ZALBINKHEO.HWADR 
	IS 'Histo:recup. adresses  KWADR' ; 
  
LABEL ON COLUMN ZALBINKHEO.HWADR 
( W3ORI IS 'Origine' , 
	W3IDORI IS 'Lien fichier origine' , 
	W3TYP IS 'Type O/P' , 
	W3IPB IS 'N° contrat ou offre' , 
	W3ALX IS 'N° aliment' , 
	W3AVN IS 'N° Avenant' , 
	W3HIN IS 'N° Historique' , 
	W3NOM IS 'Nom' , 
	W3AD1 IS 'Adresse' , 
	W3AD2 IS 'Adresse 2' , 
	W3COM IS 'Code commune        arrondissement' , 
	W3VIL IS 'Ville' , 
	W3PAY IS 'Pays' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.HWADR 
( W3ORI TEXT IS 'Origine' , 
	W3IDORI TEXT IS 'Lien fichier origine' , 
	W3TYP TEXT IS 'Type O/P' , 
	W3IPB TEXT IS 'N° contrat ou offre' , 
	W3ALX TEXT IS 'N° aliment' , 
	W3AVN TEXT IS 'N° Avenant' , 
	W3HIN TEXT IS 'N° Historique' , 
	W3NOM TEXT IS 'Nom' , 
	W3AD1 TEXT IS 'Adresse' , 
	W3AD2 TEXT IS 'Adresse 2' , 
	W3COM TEXT IS 'Code commune arrondissement' , 
	W3VIL TEXT IS 'Ville' , 
	W3PAY TEXT IS 'Code Pays' ) ; 
  
