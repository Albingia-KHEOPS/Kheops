CREATE TABLE ZALBINKHEO.KREAVEN ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KREAVEN de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KREAVEN de ZALBINKHEO. 
	KGAFAM CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KGAVEN NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KGALIBV CHAR(30) CCSID 297 NOT NULL DEFAULT '' , 
	KGASEPA CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FREAVEN    ; 
  
LABEL ON TABLE ZALBINKHEO.KREAVEN 
	IS 'KHEOPS reas parame colonne                     KGA' ; 
  
LABEL ON COLUMN ZALBINKHEO.KREAVEN 
( KGAFAM IS 'Famille réassurance' , 
	KGAVEN IS 'N° colonne ventilat°' , 
	KGALIBV IS 'Libellé ventilation' , 
	KGASEPA IS 'Séparation' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KREAVEN 
( KGAFAM TEXT IS 'Famille de réassurance' , 
	KGAVEN TEXT IS 'N° colonne ventilation' , 
	KGALIBV TEXT IS 'Libellé ventilation' , 
	KGASEPA TEXT IS 'Séparation O/N' ) ; 
  
