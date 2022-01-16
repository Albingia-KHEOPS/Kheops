﻿CREATE TABLE ZALBINKHEO.HPMATFF ( 
--  SQL150B   10   REUSEDLT(*NO) de la table HPMATFF de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour HPMATFF de ZALBINKHEO. 
	KEATYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEAIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KEAALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KEAAVN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KEAHIN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KEACHR NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KEAFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KEAOPT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KEAKDBID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KEAKDAID NUMERIC(15, 0) NOT NULL DEFAULT 0 )   
	RCDFMT HPMATFF    ; 
  
LABEL ON TABLE ZALBINKHEO.HPMATFF 
	IS 'Histo  Matrice/formule Formule' ; 
  
LABEL ON COLUMN ZALBINKHEO.HPMATFF 
( KEATYP IS 'Type O/P' , 
	KEAIPB IS 'IPB' , 
	KEAALX IS 'ALX' , 
	KEAAVN IS 'N° avenant' , 
	KEAHIN IS 'N° histo par avenant' , 
	KEACHR IS 'Chrono Affichage ID' , 
	KEAFOR IS 'Formule' , 
	KEAOPT IS 'Option' , 
	KEAKDBID IS 'Lien KPOPT' , 
	KEAKDAID IS 'Lien KPFOR' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.HPMATFF 
( KEATYP TEXT IS 'Type O/P' , 
	KEAIPB TEXT IS 'IPB' , 
	KEAALX TEXT IS 'ALX' , 
	KEAAVN TEXT IS 'N° avenant' , 
	KEAHIN TEXT IS 'N° histo par avenant' , 
	KEACHR TEXT IS 'N° Chrono Affichage ID unique' , 
	KEAFOR TEXT IS 'Formule' , 
	KEAOPT TEXT IS 'Option' , 
	KEAKDBID TEXT IS 'Lien KPOPT' , 
	KEAKDAID TEXT IS 'Lien KPFOR' ) ; 
  