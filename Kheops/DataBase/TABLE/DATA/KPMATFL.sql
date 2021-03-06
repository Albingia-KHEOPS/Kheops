CREATE TABLE ZALBINKHEO.KPMATFL ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPMATFL de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPMATFL de ZALBINKHEO. 
	KECTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KECIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KECALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KECKEACHR NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KECKEBCHR NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KECICO CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPMATFL    ; 
  
LABEL ON TABLE ZALBINKHEO.KPMATFL 
	IS 'KHEOPS Matrice Lien /Formule' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPMATFL 
( KECTYP IS 'Type O/P' , 
	KECIPB IS 'IPB' , 
	KECALX IS 'ALX' , 
	KECKEACHR IS 'Lien KPMATFF' , 
	KECKEBCHR IS 'Lien KPMATFR' , 
	KECICO IS 'Icône' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPMATFL 
( KECTYP TEXT IS 'Type O/P' , 
	KECIPB TEXT IS 'IPB' , 
	KECALX TEXT IS 'ALX' , 
	KECKEACHR TEXT IS 'Lien KPMATFF' , 
	KECKEBCHR TEXT IS 'Lien KPMATFR' , 
	KECICO TEXT IS 'Icône C Complet S spécifique' ) ; 
  
