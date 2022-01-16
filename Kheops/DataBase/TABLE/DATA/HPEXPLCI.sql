﻿CREATE TABLE ZALBINKHEO.HPEXPLCI ( 
--  SQL150B   10   REUSEDLT(*NO) de la table HPEXPLCI de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour HPEXPLCI de ZALBINKHEO. 
	KDIID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDITYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDIIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KDIALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KDIAVN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KDIHIN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KDILCE CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDIDESC CHAR(60) CCSID 297 NOT NULL DEFAULT '' , 
	KDIDESI NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDIORI CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDIMODI CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT HPEXPLCI   ; 
  
LABEL ON TABLE ZALBINKHEO.HPEXPLCI 
	IS 'KHEOPS histo Exp complexe LCI' ; 
  
LABEL ON COLUMN ZALBINKHEO.HPEXPLCI 
( KDIID IS 'ID unique' , 
	KDITYP IS 'Type O/P' , 
	KDIIPB IS 'IPB' , 
	KDIALX IS 'ALX' , 
	KDIAVN IS 'n° avenant' , 
	KDIHIN IS 'N° histo par avenant' , 
	KDILCE IS 'Expression complexe' , 
	KDIDESC IS 'Description' , 
	KDIDESI IS 'Lien KPDESI' , 
	KDIORI IS 'Origine R/S Réf/Sais' , 
	KDIMODI IS 'Modifiable O/N' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.HPEXPLCI 
( KDIID TEXT IS 'ID unique' , 
	KDITYP TEXT IS 'Type O/P' , 
	KDIIPB TEXT IS 'IPB' , 
	KDIALX TEXT IS 'ALX' , 
	KDIAVN TEXT IS 'n° avenant' , 
	KDIHIN TEXT IS 'N° histo par avenant' , 
	KDILCE TEXT IS 'Expression complexe' , 
	KDIDESC TEXT IS 'Description' , 
	KDIDESI TEXT IS 'Lien KPDESI' , 
	KDIORI TEXT IS 'Origine R/S Référentiel/Saisie' , 
	KDIMODI TEXT IS 'Modifiable O/N' ) ; 
  
