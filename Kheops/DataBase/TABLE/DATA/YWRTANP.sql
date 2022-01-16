CREATE TABLE ZALBINKHEO.YWRTANP ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YWRTANP de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour YWRTANP de ZALBINKHEO. 
	WWIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	WWALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne WWALX. 
	WWIPK NUMERIC(3, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FWRTANP    ; 
  
LABEL ON TABLE ZALBINKHEO.YWRTANP 
	IS 'Primes RT marquées pour annulation (IPB/ALX/IPK)' ; 
  
LABEL ON COLUMN ZALBINKHEO.YWRTANP 
( WWIPB IS 'N° de police / Offre' , 
	WWALX IS 'N° Aliment' , 
	WWIPK IS 'N° de prime/ police' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YWRTANP 
( WWIPB TEXT IS 'N° de Police' , 
	WWALX TEXT IS 'N° Aliment' , 
	WWIPK TEXT IS 'N° de prime / Police' ) ; 
  
