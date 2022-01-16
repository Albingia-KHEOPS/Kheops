﻿CREATE TABLE ZALBINKHEO.KPOPPAP ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPOPPAP de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPOPPAP de ZALBINKHEO. 
	KFQID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KFQTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFQIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KFQALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KFQKFPID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KFQPERI CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KFQRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KFQOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KFQCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KFQCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KFQMAJU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KFQMAJD NUMERIC(8, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPOPPAP    ; 
  
LABEL ON TABLE ZALBINKHEO.KPOPPAP 
	IS 'KHEOPS Opposit° applique à                     KFQ' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPOPPAP 
( KFQID IS 'ID unique' , 
	KFQTYP IS 'Type O/P' , 
	KFQIPB IS 'IPB' , 
	KFQALX IS 'ALX' , 
	KFQKFPID IS 'Lien KPOPP' , 
	KFQPERI IS 'Périmètre' , 
	KFQRSQ IS 'Risque' , 
	KFQOBJ IS 'Objet' , 
	KFQCRU IS 'Création User' , 
	KFQCRD IS 'Création Date' , 
	KFQMAJU IS 'MAJ User' , 
	KFQMAJD IS 'MAJ Date' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPOPPAP 
( KFQID TEXT IS 'ID unique' , 
	KFQTYP TEXT IS 'Type O/P' , 
	KFQIPB TEXT IS 'IPB' , 
	KFQALX TEXT IS 'ALX' , 
	KFQKFPID TEXT IS 'Lien KPOPP' , 
	KFQPERI TEXT IS 'Périmètre' , 
	KFQRSQ TEXT IS 'Risque' , 
	KFQOBJ TEXT IS 'Objet' , 
	KFQCRU TEXT IS 'Création User' , 
	KFQCRD TEXT IS 'Création Date' , 
	KFQMAJU TEXT IS 'MAJ User' , 
	KFQMAJD TEXT IS 'MAJ Date' ) ; 
  