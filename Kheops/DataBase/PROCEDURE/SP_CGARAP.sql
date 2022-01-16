﻿CREATE OR REPLACE PROCEDURE SP_CGARAP(
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_CODEGARAN INTEGER , 
	IN P_NEWCODEGARAN INTEGER , 
	IN P_NEWVERSION INTEGER , 
	IN P_CODECONTRAT CHAR(9) , 
	IN P_VERSIONCONTRAT INTEGER , 
	IN P_DATESYSTEME VARCHAR(8) , 
	IN P_USER VARCHAR(15) , 
	IN P_TRAITEMENT VARCHAR(1) , 
	IN P_COPYCODEOFFRE CHAR(9) , 
	IN P_COPYVERSION INTEGER ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
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
  
	DECLARE V_NEWCODEGARAP INTEGER DEFAULT 0 ; 
	DECLARE V_CODEINVEN INTEGER DEFAULT 0 ; 
	DECLARE V_NEWCODEINVEN INTEGER DEFAULT 0 ; 
	 
	DECLARE V_CODEOFFRE VARCHAR ( 9 ) DEFAULT '' ; 
	DECLARE V_TYPEOFFRE VARCHAR ( 1 ) DEFAULT '' ; 
	DECLARE V_VERSOFFRE INTEGER DEFAULT 0 ; 
	DECLARE V_NEWVERS INTEGER DEFAULT 0 ; 

   	SET P_CODEOFFRE = LPAD ( TRIM ( P_CODEOFFRE ) , 9 , ' ') ;
   	SET P_CODECONTRAT = LPAD ( TRIM ( P_CODECONTRAT ) , 9 , ' ') ;
	SET P_COPYCODEOFFRE = LPAD ( TRIM ( P_COPYCODEOFFRE ) , 9 , ' ') ;
	 
	SET V_CODEOFFRE = P_CODEOFFRE ; 
	SET V_TYPEOFFRE = P_TYPE ; 
	SET V_VERSOFFRE = P_VERSION ; 
	SET V_NEWVERS = P_NEWVERSION ; 
	 
	IF ( P_TRAITEMENT = 'P' ) THEN 
		SET V_CODEOFFRE = P_CODECONTRAT ; 
		SET V_TYPEOFFRE = 'P' ; 
		SET V_VERSOFFRE = P_VERSIONCONTRAT ; 
		SET V_NEWVERS = P_VERSIONCONTRAT ; 
	END IF ; 
	 
	IF ( P_TRAITEMENT = 'C' ) THEN 
		SET V_CODEOFFRE = P_COPYCODEOFFRE ; 
		SET V_VERSOFFRE = P_COPYVERSION ; 
		SET V_NEWVERS = P_COPYVERSION ; 
	END IF ; 
	 
	FOR FORM_LOOP AS FREE_LIST CURSOR FOR 
		SELECT KDFID CODEGARAP , KDFINVEN CODEINVEN FROM KPGARAP 
		WHERE KDFKDEID = P_CODEGARAN 
	DO 
		 
		CALL SP_SECOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KDFID' , CODEGARAP , V_NEWCODEGARAP ) ; 
		IF ( V_NEWCODEGARAP = 0 ) THEN 
			CALL SP_NCHRONO ( 'KDFID' , V_NEWCODEGARAP ) ; 
			CALL SP_INCOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KDFID' , CODEGARAP , V_NEWCODEGARAP ) ; 
		END IF ; 
		 
		INSERT INTO KPGARAP 
		( SELECT V_NEWCODEGARAP , V_TYPEOFFRE , V_CODEOFFRE , V_NEWVERS , KDFFOR , KDFOPT , KDFGARAN , P_NEWCODEGARAN , KDFGAN , KDFPERI , 
		KDFRSQ , KDFOBJ , V_NEWCODEINVEN , KDFINVEP , P_USER , P_DATESYSTEME , P_USER , P_DATESYSTEME , 
		KDFPRV , KDFPRA , KDFPRW , KDFPRU , KDFTYC , KDFMNT 
		FROM KPGARAP WHERE KDFID = CODEGARAP ) ; 
	 
	END FOR ; 
  
END P1  ; 
  

  
