CREATE TABLE ZALBINKHEO.YPRTLCI ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPRTLCI de ZALBINKHEO ignoré. 
	JKIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	JKALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JKALX. 
	JKLCE CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	JKORD NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JKLCV DECIMAL(11, 0) NOT NULL DEFAULT 0 , 
	JKLCU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JKDEV CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	JKLCB CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	JKLOV DECIMAL(11, 0) NOT NULL DEFAULT 0 , 
	JKLOU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JKLOB CHAR(3) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPRTLCI    ; 
  
LABEL ON TABLE ZALBINKHEO.YPRTLCI 
	IS 'Poli.RT : LCI exp cplx                          JK' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTLCI 
( JKIPB IS 'N° de police' , 
	JKALX IS 'N° Aliment' , 
	JKLCE IS 'Expression LCI' , 
	JKORD IS 'N° ordre' , 
	JKLCV IS 'LCI : Valeur' , 
	JKLCU IS 'LCI : Unité' , 
	JKDEV IS 'Code devise' , 
	JKLCB IS 'Code base LCI' , 
	JKLOV IS 'LCI : Valeur à concu' , 
	JKLOU IS 'LCI : Unité concurr' , 
	JKLOB IS 'LCI: Base concurr.' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTLCI 
( JKIPB TEXT IS 'N° de Police' , 
	JKALX TEXT IS 'N° Aliment' , 
	JKLCE TEXT IS 'Expression complexe LCI' , 
	JKORD TEXT IS 'N° ordre' , 
	JKLCV TEXT IS 'LCI : Valeur' , 
	JKLCU TEXT IS 'LCI : Unité' , 
	JKDEV TEXT IS 'Code devise' , 
	JKLCB TEXT IS 'LCI : Code base' , 
	JKLOV TEXT IS 'LCI : Valeur à concurrence' , 
	JKLOU TEXT IS 'LCI : Unité  à concurrence' , 
	JKLOB TEXT IS 'LCI : Base   à concurrence' ) ; 
  
