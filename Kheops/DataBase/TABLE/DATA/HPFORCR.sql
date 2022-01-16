﻿CREATE TABLE ZALBINKHEO.HPFORCR ( 
--  SQL150B   10   REUSEDLT(*NO) de la table HPFORCR de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour HPFORCR de ZALBINKHEO. 
	KGQID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KGQTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KGQIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KGQALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KGQAVN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KGQHIN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KGQFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KGQRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KGQF400 NUMERIC(5, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FHPFORCR   ; 
  
LABEL ON TABLE ZALBINKHEO.HPFORCR 
	IS 'Histo For corres n°AS4/KHEOPS' ; 
  
LABEL ON COLUMN ZALBINKHEO.HPFORCR 
( KGQID IS 'ID unique' , 
	KGQTYP IS 'Type O/P' , 
	KGQIPB IS 'IPB' , 
	KGQALX IS 'ALX' , 
	KGQAVN IS 'N° avenant' , 
	KGQHIN IS 'N° histo par avenant' , 
	KGQFOR IS 'Formule' , 
	KGQRSQ IS 'N° Risque' , 
	KGQF400 IS 'Formule AS400' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.HPFORCR 
( KGQID TEXT IS 'ID Unique' , 
	KGQTYP TEXT IS 'Type O/P' , 
	KGQIPB TEXT IS 'IPB' , 
	KGQALX TEXT IS 'ALX' , 
	KGQAVN TEXT IS 'N° avenant' , 
	KGQHIN TEXT IS 'N° histo par avenant' , 
	KGQFOR TEXT IS 'Formule' , 
	KGQRSQ TEXT IS 'N° Risque' , 
	KGQF400 TEXT IS 'Formule AS400' ) ; 
  