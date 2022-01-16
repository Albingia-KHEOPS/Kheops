CREATE TABLE ZALBINKHEO.YPOCONX ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPOCONX de ZALBINKHEO ignoré. 
	PJTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PJCCX CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	PJCNX DECIMAL(7, 0) NOT NULL DEFAULT 0 , 
	PJIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	PJALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne PJALX. 
	PJBRA CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	PJSBR CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	PJCAT CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	PJOBS NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	PJIDE NUMERIC(15, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPOCONX    ; 
  
LABEL ON TABLE ZALBINKHEO.YPOCONX 
	IS 'Polices/Offres : Connexité                      PJ' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOCONX 
( PJTYP IS 'Type' , 
	PJCCX IS 'Cause de connexité' , 
	PJCNX IS 'N° de connexité' , 
	PJIPB IS 'N° de police connexe' , 
	PJALX IS 'N° Aliment connexe' , 
	PJBRA IS 'Branche' , 
	PJSBR IS 'Sous-branche' , 
	PJCAT IS 'Catégorie' , 
	PJOBS IS 'Lien KPOBSV' , 
	PJIDE IS 'Lien KPENG Engag cnx' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOCONX 
( PJTYP TEXT IS 'Type  O Offre  P Police' , 
	PJCCX TEXT IS 'Cause de connexité' , 
	PJCNX TEXT IS 'N° de connexité' , 
	PJIPB TEXT IS 'N° de Police  connexe' , 
	PJALX TEXT IS 'N° Aliment connexe' , 
	PJBRA TEXT IS 'Branche' , 
	PJSBR TEXT IS 'Sous-branche' , 
	PJCAT TEXT IS 'Catégorie' , 
	PJOBS TEXT IS 'Lien KPOBSV' , 
	PJIDE TEXT IS 'Lien KPENG  Engagement connexité' ) ; 
  
