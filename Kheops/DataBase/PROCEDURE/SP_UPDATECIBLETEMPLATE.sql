CREATE PROCEDURE SP_UPDATECIBLETEMPLATE ( 
	IN P_IDTEMP INTEGER , 
	IN P_ISCHECKED CHAR(1) ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_UPDATECIBLETEMPLATE 
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
  
DECLARE V_TYPETEMP CHAR ( 1 ) DEFAULT '' ; 
DECLARE V_CIBLETEMP INTEGER DEFAULT 0 ; 
DECLARE V_IDMINTEMP INTEGER DEFAULT 0 ; 
  
SELECT KGOTYP , KGOKAIID INTO V_TYPETEMP , V_CIBLETEMP FROM KCANEV WHERE KGOID = P_IDTEMP ; 
  
UPDATE KCANEV SET KGOCDEF = 'N' WHERE KGOKAIID = V_CIBLETEMP AND KGOTYP = V_TYPETEMP ; 
IF ( P_ISCHECKED = 'O' ) THEN 
UPDATE KCANEV SET KGOCDEF = P_ISCHECKED WHERE KGOID = P_IDTEMP ; 
ELSE 
UPDATE KCANEV SET KGOCDEF = P_ISCHECKED WHERE KGOID = P_IDTEMP ; 
SELECT MIN ( KGOID ) INTO V_IDMINTEMP FROM KCANEV WHERE KGOKAIID = V_CIBLETEMP AND KGOTYP = V_TYPETEMP ; 
UPDATE KCANEV SET KGOCDEF = 'O' WHERE KGOID = V_IDMINTEMP ; 
END IF ; 
  
END P1  ; 
  

  

