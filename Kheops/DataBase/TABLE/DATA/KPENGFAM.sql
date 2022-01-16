﻿CREATE TABLE ZALBINKHEO.KPENGFAM ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPENGFAM de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPENGFAM de ZALBINKHEO. 
	KDPID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDPTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDPIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KDPALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KDPKDOID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDPFAM CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDPENG NUMERIC(13, 0) NOT NULL DEFAULT 0 , 
	KDPENA NUMERIC(13, 0) NOT NULL DEFAULT 0 , 
	KDPCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDPCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KDPMAJU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDPMAJD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KDPLCT NUMERIC(13, 0) NOT NULL DEFAULT 0 , 
	KDPLCA NUMERIC(13, 0) NOT NULL DEFAULT 0 , 
	KDPCAT NUMERIC(13, 0) NOT NULL DEFAULT 0 , 
	KDPCAA NUMERIC(13, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPENGFAM   ; 
  
LABEL ON TABLE ZALBINKHEO.KPENGFAM 
	IS 'Engagement Famille   ID                        KDP' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPENGFAM 
( KDPID IS 'ID unique' , 
	KDPTYP IS 'Type O/P' , 
	KDPIPB IS 'IPB' , 
	KDPALX IS 'ALX' , 
	KDPKDOID IS 'Lien KPENG' , 
	KDPFAM IS 'Famille réassurance' , 
	KDPENG IS 'Engagement cpt' , 
	KDPENA IS 'Engagement ALB cpt' , 
	KDPCRU IS 'création User' , 
	KDPCRD IS 'Création Date' , 
	KDPMAJU IS 'MAJ User' , 
	KDPMAJD IS 'MAJ Date' , 
	KDPLCT IS 'LCI 100%' , 
	KDPLCA IS 'LCI Part Alb' , 
	KDPCAT IS 'Capitaux 100%' , 
	KDPCAA IS 'Capitaux Part Alb' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPENGFAM 
( KDPID TEXT IS 'ID unique' , 
	KDPTYP TEXT IS 'Type O/P' , 
	KDPIPB TEXT IS 'IPB' , 
	KDPALX TEXT IS 'ALX' , 
	KDPKDOID TEXT IS 'Lien KPENG' , 
	KDPFAM TEXT IS 'Famille de réassurance' , 
	KDPENG TEXT IS 'Engagement cpt  100%' , 
	KDPENA TEXT IS 'Engagement ALB cpt' , 
	KDPCRU TEXT IS 'Création User' , 
	KDPCRD TEXT IS 'Création Date' , 
	KDPMAJU TEXT IS 'MAJ User' , 
	KDPMAJD TEXT IS 'MAJ Date' , 
	KDPLCT TEXT IS 'LCI 100%' , 
	KDPLCA TEXT IS 'LCI Part Alb' , 
	KDPCAT TEXT IS 'Capitaux 100%' , 
	KDPCAA TEXT IS 'Capitaux Part ALb' ) ; 
  
