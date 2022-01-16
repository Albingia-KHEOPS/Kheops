﻿CREATE TABLE ZALBINKHEO.HPMATFL ( 
--  SQL150B   10   REUSEDLT(*NO) de la table HPMATFL de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour HPMATFL de ZALBINKHEO. 
	KECTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KECIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KECALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KECAVN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KECHIN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KECKEACHR NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KECKEBCHR NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KECICO CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT HPMATFL    ; 
  
LABEL ON TABLE ZALBINKHEO.HPMATFL 
	IS 'Histo  Matrice Lien /Formule' ; 
  
LABEL ON COLUMN ZALBINKHEO.HPMATFL 
( KECTYP IS 'Type O/P' , 
	KECIPB IS 'IPB' , 
	KECALX IS 'ALX' , 
	KECAVN IS 'N° avenant' , 
	KECHIN IS 'N° histo par avenant' , 
	KECKEACHR IS 'Lien KPMATFF' , 
	KECKEBCHR IS 'Lien KPMATFR' , 
	KECICO IS 'Icône' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.HPMATFL 
( KECTYP TEXT IS 'Type O/P' , 
	KECIPB TEXT IS 'IPB' , 
	KECALX TEXT IS 'ALX' , 
	KECAVN TEXT IS 'N° avenant' , 
	KECHIN TEXT IS 'N° histo par avenant' , 
	KECKEACHR TEXT IS 'Lien KPMATFF' , 
	KECKEBCHR TEXT IS 'Lien KPMATFR' , 
	KECICO TEXT IS 'Icône C Complet S spécifique' ) ; 
  
