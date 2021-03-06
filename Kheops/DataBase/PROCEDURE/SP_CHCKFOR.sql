CREATE PROCEDURE SP_CHCKFOR ( 
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_CODEFORMULE INTEGER , 
	IN P_TYPEDEL CHAR(1) ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_CHCKFOR 
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
  
  
DECLARE V_COUNTGAR INTEGER DEFAULT 0 ; 
  
  
SELECT COUNT ( * ) INTO V_COUNTGAR 
FROM KPGARAN 
WHERE KDEIPB = P_CODEOFFRE AND KDEALX = P_VERSION AND KDETYP = P_TYPE AND KDEFOR = P_CODEFORMULE ; 
  
IF ( V_COUNTGAR = 0 AND P_CODEFORMULE <> 0 ) THEN 
  
	DELETE FROM KPFOR 
	WHERE KDAIPB = P_CODEOFFRE AND KDAALX = P_VERSION AND KDATYP = P_TYPE AND KDAFOR = P_CODEFORMULE ; 
	 
	DELETE FROM KPOPT 
	WHERE KDBIPB = P_CODEOFFRE AND KDBALX = P_VERSION AND KDBTYP = P_TYPE AND KDBFOR = P_CODEFORMULE ; 
	 
	CALL SP_DELFORM ( P_CODEOFFRE , P_VERSION , P_TYPE , P_CODEFORMULE , P_TYPEDEL ) ; 
  
END IF ; 
  
END P1  ;



