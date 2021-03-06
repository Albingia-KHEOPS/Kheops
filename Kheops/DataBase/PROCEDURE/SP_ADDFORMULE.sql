CREATE PROCEDURE SP_ADDFORMULE ( 
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	OUT P_ADDFORMULE INTEGER ) 
	LANGUAGE SQL 
	SPECIFIC SP_ADDFORMULE 
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
  
	DECLARE V_COUNT INTEGER DEFAULT 0 ; 
	 
	SET P_CODEOFFRE = LPAD ( RTRIM ( P_CODEOFFRE ) , 9 , ' ' ) ; 

	SET V_COUNT = 0 ; 
	SET P_ADDFORMULE = 0 ; 
	 
	FOR LOOP_RSQ AS FREE_LIST CURSOR FOR 
		SELECT KABRSQ CODERSQ FROM KPRSQ WHERE KABIPB = P_CODEOFFRE AND KABALX = P_VERSION AND KABTYP = P_TYPE 
	DO 
		SELECT COUNT ( * ) INTO V_COUNT FROM KPOPTAP WHERE KDDIPB = P_CODEOFFRE AND KDDALX = P_VERSION AND KDDTYP = P_TYPE AND KDDRSQ = CODERSQ AND KDDPERI = 'RQ' ; 
  
		IF ( V_COUNT = 0 ) THEN 
			FOR LOOP_OBJ AS FREE_LIST CURSOR FOR 
				SELECT KACOBJ CODEOBJ FROM KPOBJ WHERE KACIPB = P_CODEOFFRE AND KACALX = P_VERSION AND KACTYP = P_TYPE AND KACRSQ = CODERSQ 
			DO 
				SELECT COUNT ( * ) INTO V_COUNT FROM KPOPTAP WHERE KDDIPB = P_CODEOFFRE AND KDDALX = P_VERSION AND KDDTYP = P_TYPE AND KDDRSQ = CODERSQ AND KDDOBJ = CODEOBJ AND KDDPERI = 'OB' ; 
			 
				IF ( V_COUNT = 0 ) THEN 
					SET P_ADDFORMULE = 1 ; 
				END IF ; 
			END FOR ; 
		END IF ; 
	END FOR ; 
  
END P1  ; 
  

  

