CREATE TABLE ZALBINKHEO.YORTCLV ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YORTCLV de ZALBINKHEO ignoré. 
	QFTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	QFIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	QFALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne QFALX. 
	QFILR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	QFVAV CHAR(40) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FORTCLV    ; 
  
LABEL ON TABLE ZALBINKHEO.YORTCLV 
	IS 'Offre RT : Valeur variables                     QF' ; 
  
LABEL ON COLUMN ZALBINKHEO.YORTCLV 
( QFTYP IS 'Type O / P' , 
	QFIPB IS 'N° Offre / Police' , 
	QFALX IS 'N° Version / Aliment' , 
	QFILR IS 'Code variable' , 
	QFVAV IS 'Valeur' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YORTCLV 
( QFTYP TEXT IS 'Type  O Offre  P Police' , 
	QFIPB TEXT IS 'N° Offre / Police' , 
	QFALX TEXT IS 'N° Version ou Aliment' , 
	QFILR TEXT IS 'Code variable' , 
	QFVAV TEXT IS 'Valeur' ) ; 
  
