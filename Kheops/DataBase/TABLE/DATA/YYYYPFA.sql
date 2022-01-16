CREATE TABLE ZALBINKHEO.YYYYPFA ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YYYYPFA de ZALBINKHEO ignoré. 
	TCON CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	TFAM CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	TFLIB CHAR(35) CCSID 297 NOT NULL DEFAULT '' , 
	TFLGR NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	TFLN1 CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	TFLN2 CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	TFLA1 CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	TFLA2 CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	TFGN1 CHAR(35) CCSID 297 NOT NULL DEFAULT '' , 
	TFGN2 CHAR(35) CCSID 297 NOT NULL DEFAULT '' , 
	TFGA1 CHAR(35) CCSID 297 NOT NULL DEFAULT '' , 
	TFGA2 CHAR(35) CCSID 297 NOT NULL DEFAULT '' , 
	TFTZ1 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	TFTZ2 CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	TFND1 NUMERIC(1, 0) NOT NULL DEFAULT 0 , 
	TFND2 NUMERIC(1, 0) NOT NULL DEFAULT 0 , 
	TFTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	TFTAN CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	TFBLC CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FYYYPFA    ; 
  
LABEL ON TABLE ZALBINKHEO.YYYYPFA 
	IS 'Paramètres : famille (séquentiel)' ; 
  
LABEL ON COLUMN ZALBINKHEO.YYYYPFA 
( TCON IS 'Concept' , 
	TFAM IS 'Famille' , 
	TFLIB IS 'Libellé famille' , 
	TFLGR IS 'Longueur code' , 
	TFLN1 IS 'Lib.court num. 1' , 
	TFLN2 IS 'Lib.court num. 2' , 
	TFLA1 IS 'Lib.court alpha 1' , 
	TFLA2 IS 'Lib.court alpha 2' , 
	TFGN1 IS 'Lib.long num. 1' , 
	TFGN2 IS 'Lib.long num.2' , 
	TFGA1 IS 'Lib.long alpha 1' , 
	TFGA2 IS 'Lib.long alpha 2' , 
	TFTYP IS 'Type (restriction)' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YYYYPFA 
( TCON TEXT IS 'Concept' , 
	TFAM TEXT IS 'Famille' , 
	TFLIB TEXT IS 'Libellé famille' , 
	TFLGR TEXT IS 'Longueur code' , 
	TFLN1 TEXT IS 'Libellé court numérique 1' , 
	TFLN2 TEXT IS 'Libellé court numérique 2' , 
	TFLA1 TEXT IS 'Libellé court alphanumérique 1' , 
	TFLA2 TEXT IS 'Libellé court alphanumérique 2' , 
	TFGN1 TEXT IS 'Libellé Long numérique 1' , 
	TFGN2 TEXT IS 'Libellé Long numérique 2' , 
	TFGA1 TEXT IS 'Libellé Long alphanumérique 1' , 
	TFGA2 TEXT IS 'Libellé Long alphanumérique 2' , 
	TFTZ1 TEXT IS 'Type de zone numérique 1 (D ou M)' , 
	TFTZ2 TEXT IS 'Type de zone numérique 2 (D ou M)' , 
	TFND1 TEXT IS 'Nombre décimales numérique 1' , 
	TFND2 TEXT IS 'Nombre décimales numérique 2' , 
	TFTYP TEXT IS 'Type paramètres (pour restriction)' , 
	TFTAN TEXT IS 'Type famille alpha ou numérique' , 
	TFBLC TEXT IS 'Autorisation code à blanc : O/N' ) ; 
  
