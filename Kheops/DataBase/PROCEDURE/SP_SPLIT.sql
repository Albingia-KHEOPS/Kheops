﻿CREATE PROCEDURE SP_SPLIT ( 
	IN P_SPT VARCHAR(5000) , 
	IN P_SPLITCHAR VARCHAR(1) , 
	IN P_PART VARCHAR(1) , 
	OUT P_RETVAL CHAR(5000) ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_SPLIT 
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
  
DECLARE V_POSITION INTEGER DEFAULT 0 ; 
SET V_POSITION = LOCATE ( P_SPLITCHAR , P_SPT ) ; 
  
IF ( V_POSITION = 0 ) THEN 
SET P_RETVAL = P_SPT ; 
RETURN ; 
END IF ; 
  
IF ( P_PART = 'L' ) THEN 
SET P_RETVAL = SUBSTR ( P_SPT , 1 , LOCATE ( P_SPLITCHAR , P_SPT ) - 1 ) ; 
ELSE 
SET P_RETVAL = SUBSTR ( P_SPT , V_POSITION + 1 , LENGTH ( P_SPT ) - LOCATE ( P_SPLITCHAR , P_SPT ) ) ; 
END IF ; 
  
SET P_RETVAL = TRIM ( P_RETVAL ) ; 
  
END P1  ; 
  

  

