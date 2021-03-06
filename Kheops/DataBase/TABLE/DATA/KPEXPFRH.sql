CREATE TABLE ZALBINKHEO.KPEXPFRH ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPEXPFRH de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPEXPFRH de ZALBINKHEO. 
	KDKID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDKTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDKIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KDKALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KDKFHE CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDKDESC CHAR(60) CCSID 297 NOT NULL DEFAULT '' , 
	KDKDESI NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDKORI CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDKMODI CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPEXPRFH   ; 
  
LABEL ON TABLE ZALBINKHEO.KPEXPFRH 
	IS 'KHEOPS Exp Complexe Franchise' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPEXPFRH 
( KDKID IS 'ID Unique' , 
	KDKTYP IS 'Type O/P' , 
	KDKIPB IS 'IPB' , 
	KDKALX IS 'Alx' , 
	KDKFHE IS 'Expression Complexe' , 
	KDKDESC IS 'Description' , 
	KDKDESI IS 'Lien KPDESI' , 
	KDKORI IS 'Origine R/S Ref/Sais' , 
	KDKMODI IS 'Modifiable O/N' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPEXPFRH 
( KDKID TEXT IS 'ID unique' , 
	KDKTYP TEXT IS 'Type O/P' , 
	KDKIPB TEXT IS 'IPB' , 
	KDKALX TEXT IS 'Alx' , 
	KDKFHE TEXT IS 'Expression Complexe' , 
	KDKDESC TEXT IS 'Description' , 
	KDKDESI TEXT IS 'Lien KPDESI' , 
	KDKORI TEXT IS 'Origine R/S  Référentiel ou Saisie' , 
	KDKMODI TEXT IS 'Modifiable O/N' ) ; 
  
