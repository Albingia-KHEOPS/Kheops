﻿
CREATE PROCEDURE SP_HCOPYADR (
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_AVN INTEGER ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_HCOPYADR 
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

	DECLARE V_NEWCODEADR INTEGER DEFAULT 0 ; 

	FOR LOOP_ADR AS FREE_LIST CURSOR FOR 
		SELECT JFRSQ RSQ, JFOBJ OBJ, JFADH ADH 
			FROM YPRTADR WHERE JFIPB = P_CODEOFFRE AND JFALX = P_VERSION 
	DO 
		SET V_NEWCODEADR = 0 ; 
		CALL SP_SECOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'YADRESS' , ADH , V_NEWCODEADR ) ; 
		IF V_NEWCODEADR = 0 THEN 
			CALL SP_YCHRONO ( 'ADRESSE_CHRONO' , V_NEWCODEADR ) ; 
			CALL SP_INCOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'YADRESS' , ADH , V_NEWCODEADR ) ; 
					 
			INSERT INTO YADRESS 
				( ABPCHR , ABPLG3 , ABPNUM , ABPEXT , ABPLBN , ABPLG4 , ABPL4F , ABPLG5 , ABPDP6 , ABPCP6 , ABPVI6 , 
				ABPCDX , ABPCEX , ABPL6F , ABPPAY , ABPLOC , ABPMAT , ABPRET , ABPERR , ABPMJU , ABPMJA , ABPMJM , ABPMJJ , ABPVIX )
			( SELECT V_NEWCODEADR , ABPLG3 , ABPNUM , ABPEXT , ABPLBN , ABPLG4 , ABPL4F , ABPLG5 , ABPDP6 , ABPCP6 , ABPVI6 , 
			ABPCDX , ABPCEX , ABPL6F , ABPPAY , ABPLOC , ABPMAT , ABPRET , ABPERR , ABPMJU , ABPMJA , ABPMJM , ABPMJJ , ABPVIX 
			FROM YADRESS WHERE ABPCHR = ADH ) ; 
		END IF ; 
		INSERT INTO YHRTADR 
		( JFIPB , JFALX , JFAVN , JHHIN , JFRSQ , JFOBJ , JFAD1 , JFAD2 , JFDEP , JFCPO , JFVIL , JFPAY , JFADH ) 
		( SELECT JFIPB , JFALX , P_AVN , 1 , JFRSQ , JFOBJ , JFAD1 , JFAD2 , JFDEP , JFCPO , JFVIL , JFPAY , V_NEWCODEADR 
		FROM YPRTADR WHERE JFIPB = P_CODEOFFRE AND JFALX = P_VERSION AND JFADH = ADH AND JFRSQ = RSQ AND JFOBJ = OBJ  ) ; 
	END FOR ;
END P1 ;
