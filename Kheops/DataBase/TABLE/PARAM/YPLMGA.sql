CREATE TABLE ZALBINKMOD.YPLMGA ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPLMGA de ZALBINKMOD ignoré. 
	D1MGA CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	D1LIB CHAR(30) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPLMGA     ; 
  
LABEL ON COLUMN ZALBINKMOD.YPLMGA 
( D1MGA IS 'Code' , 
	D1LIB IS 'Libellé' ) ; 
  
LABEL ON COLUMN ZALBINKMOD.YPLMGA 
( D1MGA TEXT IS 'Code modèle garantie' , 
	D1LIB TEXT IS 'Libellé' ) ; 
  
