CREATE TABLE ZALBINKHEO.KPMATFF ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPMATFF de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPMATFF de ZALBINKHEO. 
	KEATYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEAIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KEAALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KEACHR NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KEAFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KEAOPT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KEAKDBID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KEAKDAID NUMERIC(15, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPMATFF    ; 
  
LABEL ON TABLE ZALBINKHEO.KPMATFF 
	IS 'KHEOPS Matrice/formule Formule' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPMATFF 
( KEATYP IS 'Type O/P' , 
	KEAIPB IS 'IPB' , 
	KEAALX IS 'ALX' , 
	KEACHR IS 'Chrono Affichage ID' , 
	KEAFOR IS 'Formule' , 
	KEAOPT IS 'Option' , 
	KEAKDBID IS 'Lien KPOPT' , 
	KEAKDAID IS 'Lien KPFOR' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPMATFF 
( KEATYP TEXT IS 'Type O/P' , 
	KEAIPB TEXT IS 'IPB' , 
	KEAALX TEXT IS 'ALX' , 
	KEACHR TEXT IS 'N° Chrono Affichage ID unique' , 
	KEAFOR TEXT IS 'Formule' , 
	KEAOPT TEXT IS 'Option' , 
	KEAKDBID TEXT IS 'Lien KPOPT' , 
	KEAKDAID TEXT IS 'Lien KPFOR' ) ; 
  
