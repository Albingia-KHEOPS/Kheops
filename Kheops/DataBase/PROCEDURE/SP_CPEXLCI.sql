﻿
CREATE OR REPLACE PROCEDURE SP_CPEXLCI ( 
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_NEWVERSION INTEGER , 
	IN P_OLDCODE INTEGER , 
	IN P_NEWCODE INTEGER ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_CPEXLCI
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
  
	DECLARE V_NEWCODE INTEGER DEFAULT 0 ; 
	DECLARE V_ACTEGESTION CHAR ( 9 ) DEFAULT '' ; 
	DECLARE V_VERSION INTEGER DEFAULT 0 ; 
	DECLARE V_TYPE CHAR ( 1 ) DEFAULT '' ; 
	DECLARE V_DESI INTEGER DEFAULT 0 ; 
	DECLARE V_NEWDESI INTEGER DEFAULT 0 ;

	SET P_CODEOFFRE = LPAD ( RTRIM ( P_CODEOFFRE ) , 9 , ' ') ;
	 
	FOR LOOP_EXLCI AS FREE_LIST CURSOR FOR 
		SELECT KDJID V_OLDCODE FROM KPEXPLCID WHERE KDJKDIID = P_OLDCODE 
	DO 
	 
		CALL SP_NCHRONO ( 'KDJID' , V_NEWCODE ) ; 
		INSERT INTO KPEXPLCID 
			( SELECT V_NEWCODE , P_NEWCODE , KDJORDRE , KDJLCVAL , KDJLCVAU , KDJLCBASE , KDJLOVAL , KDJLOVAU , KDJLOBASE 
				FROM KPEXPLCID WHERE KDJID = V_OLDCODE ) ; 
		 
	END FOR ; 
	 
	SELECT KDIIPB , KDIALX , KDITYP , KDIDESI INTO V_ACTEGESTION , V_VERSION , V_TYPE , V_DESI FROM KPEXPLCI WHERE KDIID = P_NEWCODE ; 
	IF ( V_DESI != 0 ) THEN 
		CALL SP_NCHRONO ( 'KADCHR' , V_NEWDESI ) ; 
		INSERT INTO KPDESI 
			( SELECT V_NEWDESI , V_TYPE , V_ACTEGESTION , V_VERSION , KADPERI , KADRSQ , KADOBJ , KADDESI 
				FROM KPDESI 
			WHERE KADCHR = V_DESI ) ; 
		UPDATE KPEXPLCI SET KDIDESI = V_NEWDESI WHERE KDIID = P_NEWCODE ; 
	END IF ; 
  
END P1  ; 
  

  

