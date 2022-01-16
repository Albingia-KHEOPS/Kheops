﻿
CREATE PROCEDURE SP_HUPDATEAVNDATES (
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_HUPDATEAVNDATES 
	NOT DETERMINISTIC 
	MODIFIES SQL DATA 
	CALLED ON NULL INPUT 
	SET OPTION  ALWBLK = *ALLREAD , 
	ALWCPYDTA = *OPTIMIZE , 
	COMMIT = *CHG , 
	DBGVIEW = *SOURCE , 
	CLOSQLCSR = *ENDMOD , 
	DECRESULT = (31, 31, 00) , 
	DFTRDBCOL = ZALBINKHEO , 
	DYNDFTCOL = *YES , 
	SQLPATH = 'ZALBINKHEO, ZALBINKMOD' , 
	DYNUSRPRF = *USER , 
	SRTSEQ = *HEX   
P1 : BEGIN ATOMIC  
	
	-- Update dates risques 
	FOR LOOP_CHECKRSQ AS FREE_LIST CURSOR FOR 
		SELECT JEIPB IPB , JEALX ALX , JERSQ RSQ 
		FROM YPRTRSQ 
		WHERE JEIPB = P_CODEOFFRE AND JEALX = P_VERSION 
	DO 
		CALL SP_SETDATES ( IPB , ALX , P_TYPE , 0 , RSQ , 0 , 0 , 0 , 0 , 'RSQ' ) ; 
	END FOR ; 
	-- Update dates objets					 
	FOR LOOP_CHECKOBJ AS FREE_LIST CURSOR FOR 
		SELECT JGIPB IPB , JGALX ALX , JGRSQ RSQ , JGOBJ OBJ 
		FROM YPRTOBJ 
		WHERE JGIPB = P_CODEOFFRE AND JGALX = P_VERSION 
	DO 
		CALL SP_SETDATES ( IPB , ALX , P_TYPE , 0 , RSQ , OBJ , 0 , 0 , 0 , 'OBJ' ) ; 
	END FOR ; 
	-- Update dates garanties 
	FOR LOOP_CHECKGAR AS FREE_LIST CURSOR FOR 
		SELECT KDEIPB IPB , KDEALX ALX , KDEFOR FORMU , KDEOPT OPT , KDEID GARAN 
		FROM KPGARAN 
		WHERE KDEIPB = P_CODEOFFRE AND KDEALX = P_VERSION AND KDETYP = P_TYPE 
	DO 
		CALL SP_SETDATES ( IPB , ALX , P_TYPE , 0 , 0 , 0 , FORMU , OPT , GARAN , 'GAR' ) ; 
	END FOR ; 

END P1 ;

