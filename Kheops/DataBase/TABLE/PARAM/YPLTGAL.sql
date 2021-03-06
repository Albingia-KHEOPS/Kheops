CREATE TABLE ZALBINKMOD.YPLTGAL ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPLTGAL de ZALBINKMOD ignoré. 
	C4SEQ NUMERIC(10, 0) NOT NULL DEFAULT 0 , 
	C4TYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	C4BAS CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	C4VAL DECIMAL(15, 4) NOT NULL DEFAULT 0 , 
	C4UNT CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	C4MAJ CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	C4OBL CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	C4ALA CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPLTGAL    ; 
  
LABEL ON TABLE ZALBINKMOD.YPLTGAL 
	IS 'Garantie type' ; 
  
LABEL ON COLUMN ZALBINKMOD.YPLTGAL 
( C4SEQ IS 'N° séquence' , 
	C4TYP IS 'Type' , 
	C4BAS IS 'Base' , 
	C4UNT IS 'Unité' , 
	C4MAJ IS 'Modifiable' , 
	C4OBL IS 'Obli' ) ; 
  
LABEL ON COLUMN ZALBINKMOD.YPLTGAL 
( C4SEQ TEXT IS 'N° séquence garantie' , 
	C4TYP TEXT IS 'Type' , 
	C4BAS TEXT IS 'Base' , 
	C4VAL TEXT IS 'Valeur' , 
	C4UNT TEXT IS 'Unité' , 
	C4MAJ TEXT IS 'Modifiable' , 
	C4OBL TEXT IS 'Obligatoire' ) ; 
  
