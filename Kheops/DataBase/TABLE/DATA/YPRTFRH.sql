CREATE TABLE ZALBINKHEO.YPRTFRH ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPRTFRH de ZALBINKHEO ignoré. 
	JJIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	JJALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JJALX. 
	JJFHE CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	JJORD NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JJFHV DECIMAL(9, 0) NOT NULL DEFAULT 0 , 
	JJFHU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JJDEV CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	JJIND CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	JJFHB CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	JJMIN DECIMAL(9, 0) NOT NULL DEFAULT 0 , 
	JJMAX DECIMAL(9, 0) NOT NULL DEFAULT 0 , 
	JJLID DECIMAL(11, 0) NOT NULL DEFAULT 0 , 
	JJLIF DECIMAL(11, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPRTFRH    ; 
  
LABEL ON TABLE ZALBINKHEO.YPRTFRH 
	IS 'Poli.RT : Exp franchise                         JJ' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTFRH 
( JJIPB IS 'N° de police' , 
	JJALX IS 'N° Aliment' , 
	JJFHE IS 'Expression complexe Franchise' , 
	JJORD IS 'N° ordre' , 
	JJFHV IS 'Valeur franchise' , 
	JJFHU IS 'Unité franchise' , 
	JJDEV IS 'Code devise' , 
	JJIND IS 'Code indice' , 
	JJFHB IS 'Code base franchise' , 
	JJMIN IS 'Minimum franchise' , 
	JJMAX IS 'Maximum franchise' , 
	JJLID IS 'Limite début' , 
	JJLIF IS 'Limite fin' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTFRH 
( JJIPB TEXT IS 'N° de Police' , 
	JJALX TEXT IS 'N° Aliment' , 
	JJFHE TEXT IS 'Expression complexe Franchise' , 
	JJORD TEXT IS 'N° ordre' , 
	JJFHV TEXT IS 'Valeur de franchise' , 
	JJFHU TEXT IS 'Unité     franchise' , 
	JJDEV TEXT IS 'Code devise' , 
	JJIND TEXT IS 'Code indice' , 
	JJFHB TEXT IS 'Code base franchise' , 
	JJMIN TEXT IS 'Minimum franchise' , 
	JJMAX TEXT IS 'Maximum franchise' , 
	JJLID TEXT IS 'Limite début' , 
	JJLIF TEXT IS 'Limite fin' ) ; 
  
