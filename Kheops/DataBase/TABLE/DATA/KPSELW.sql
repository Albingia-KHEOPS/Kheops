﻿CREATE TABLE ZALBINKHEO.KPSELW ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPSELW de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPSELW de ZALBINKHEO. 
	KHVID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHVTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHVIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KHVALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KHVALX. 
	KHVPERI CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KHVRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KHVOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KHVFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KHVKDEID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHVEDTB CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHVDEB NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KHVFIN NUMERIC(8, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPSELW     ; 
  
LABEL ON TABLE ZALBINKHEO.KPSELW 
	IS 'KHEOPS Sélec Attest Régule                     KHV' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPSELW 
( KHVID IS 'ID unique' , 
	KHVTYP IS 'Type' , 
	KHVIPB IS 'IPB' , 
	KHVALX IS 'Aliment' , 
	KHVPERI IS 'Pèrimètre' , 
	KHVRSQ IS 'N° de risque' , 
	KHVOBJ IS 'N° Objet' , 
	KHVFOR IS 'Formule' , 
	KHVKDEID IS 'Lien KPGARAN' , 
	KHVEDTB IS 'Type Edition Tableau' , 
	KHVDEB IS 'Plage Début' , 
	KHVFIN IS 'Plage Fin' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPSELW 
( KHVID TEXT IS 'ID lot de sélection' , 
	KHVTYP TEXT IS 'Type' , 
	KHVIPB TEXT IS 'IPB' , 
	KHVALX TEXT IS 'Aliment' , 
	KHVPERI TEXT IS 'Périmètre   RQ OB FO GA' , 
	KHVRSQ TEXT IS 'N° de risque' , 
	KHVOBJ TEXT IS 'N° objet' , 
	KHVFOR TEXT IS 'Formule' , 
	KHVKDEID TEXT IS 'Lien KPGARAN' , 
	KHVEDTB TEXT IS 'Type Edition Tableau Libellé ou Comp' , 
	KHVDEB TEXT IS 'Plage Début' , 
	KHVFIN TEXT IS 'Plage Fin' ) ; 
  
