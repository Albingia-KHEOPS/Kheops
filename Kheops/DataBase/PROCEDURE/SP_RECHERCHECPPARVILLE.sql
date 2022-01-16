﻿CREATE PROCEDURE SP_RECHERCHECPPARVILLE ( 
	IN P_VILLE CHAR(38) ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_RECHERCHECPPARVILLE 
	NOT DETERMINISTIC 
	MODIFIES SQL DATA 
	CALLED ON NULL INPUT 
	SET OPTION  ALWBLK = *ALLREAD , 
	ALWCPYDTA = *OPTIMIZE , 
	COMMIT = *CHG , 
	CLOSQLCSR = *ENDMOD , 
	DECRESULT = (31, 31, 00) , 
	DFTRDBCOL = ZALBINKHEO , 
	DYNDFTCOL = *YES , 
	SQLPATH = 'ZALBINKHEO, ZALBINKMOD' , 
	DYNUSRPRF = *USER , 
	SRTSEQ = *HEX   
	P1 : BEGIN ATOMIC 
DECLARE CURSORVILLE CURSOR FOR 
SELECT ABLCPO CODE , ABLLIB LIBELLE FROM GENERALE . YHEXLOC WHERE LOWER ( TRIM ( ABLLIB ) ) LIKE '' CONCAT LOWER ( TRIM ( P_VILLE ) ) CONCAT '' ; 
OPEN CURSORVILLE ; 
END P1  ; 
  

  

