CREATE PROCEDURE SP_RENUMBLOC ( 
	IN P_IDVOLET NUMERIC(15, 0) , 
	IN P_IDBLOC NUMERIC(15, 0) , 
	IN P_ORDRE NUMERIC(6, 0) , 
	IN P_IS_TRANSFORMED SMALLINT ) 
	LANGUAGE SQL 
	SPECIFIC SP_RENUMBLOC 
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
  
DECLARE V_COUNT INTEGER DEFAULT 1 ; 
DECLARE V_ORDRE NUMERIC ( 6 ) DEFAULT 1 ; 
DECLARE V_IDVOLET NUMERIC ( 15 ) DEFAULT 1 ; 
  
IF ( P_IDVOLET <> 0 ) THEN 
	SELECT COUNT ( * ) 
	INTO V_COUNT 
	FROM KCATBLOC 
	WHERE KAQKAPID = P_IDVOLET ; 
	 
	SELECT KAQORDRE 
	INTO V_ORDRE 
	FROM KCATBLOC 
	WHERE KAQID = P_IDBLOC ; 
  
	IF ( P_ORDRE <= V_ORDRE ) THEN 
		UPDATE KCATBLOC 
		SET KAQORDRE = P_ORDRE 
		WHERE KAQKAPID = P_IDVOLET AND KAQID = P_IDBLOC ; 
	 
		UPDATE KCATBLOC 
		SET KAQORDRE = KAQORDRE + 1 
		WHERE KAQKAPID = P_IDVOLET AND KAQID <> P_IDBLOC AND KAQORDRE >= P_ORDRE AND KAQORDRE < V_ORDRE ; 
  
	ELSE 
		IF ( P_IS_TRANSFORMED = 0 ) THEN 
			SET P_ORDRE = P_ORDRE + 1 ; 
		END IF ; 
		 
		UPDATE KCATBLOC 
		SET KAQORDRE = P_ORDRE - 1 
		WHERE KAQKAPID = P_IDVOLET AND KAQID = P_IDBLOC ; 
	 
		UPDATE KCATBLOC 
		SET KAQORDRE = KAQORDRE - 1 
		WHERE KAQKAPID = P_IDVOLET AND KAQID <> P_IDBLOC AND KAQORDRE < P_ORDRE AND KAQORDRE > V_ORDRE ; 
	 
	END IF ; 
	 
	UPDATE KCATBLOC 
	SET KAQORDRE = V_COUNT 
	WHERE KAQKAPID = P_IDVOLET AND KAQID = P_IDBLOC AND KAQORDRE > V_COUNT ; 
ELSE	 
	SELECT KAQORDRE , KAQKAPID 
	INTO V_ORDRE , V_IDVOLET 
	FROM KCATBLOC 
	WHERE KAQID = P_IDBLOC ; 
  
  
	UPDATE KCATBLOC 
	SET KAQORDRE = KAQORDRE - 1 
	WHERE KAQKAPID = V_IDVOLET AND KAQID <> P_IDBLOC AND KAQORDRE >= V_ORDRE ; 
END IF ; 
  
END P1  ;

