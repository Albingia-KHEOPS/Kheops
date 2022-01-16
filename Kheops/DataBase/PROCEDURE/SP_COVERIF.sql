﻿CREATE PROCEDURE SP_COVERIF ( 
	IN P_CODECONTRAT CHAR(9) , 
	IN P_VERSIONCONTRAT INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_CODERSQ INTEGER , 
	IN P_CODEOBJ INTEGER , 
	OUT P_CHECK CHAR(1) ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_COVERIF 
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
  
SET P_CHECK = '' ; 
  
SELECT IFNULL ( KFISEL , '' ) INTO P_CHECK 
FROM KPOFRSQ 
WHERE KFIPOG = P_CODECONTRAT AND KFIALG = P_VERSIONCONTRAT AND KFITYE = P_TYPE AND KFIRSQ = P_CODERSQ AND KFIOBJ = P_CODEOBJ ; 
  
END P1  ; 
  

  
