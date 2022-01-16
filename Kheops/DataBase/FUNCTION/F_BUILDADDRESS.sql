CREATE FUNCTION ZALBINKHEO.F_BUILDADDRESS ( 
	P_NUMVOIE INTEGER , 
	P_NUMVOIE2 CHAR(15) , 
	P_EXTVOIE CHAR(32) , 
	P_NOMVOIE CHAR(32)) 
	RETURNS VARCHAR(1000)   
	LANGUAGE SQL 
	SPECIFIC ZALBINKHEO.F_BUILDADDRESS 
	NOT DETERMINISTIC 
	READS SQL DATA 
	CALLED ON NULL INPUT 
	SET OPTION  ALWBLK = *ALLREAD , 
	ALWCPYDTA = *OPTIMIZE , 
	COMMIT = *NONE , 
	CLOSQLCSR = *ENDMOD , 
	DECRESULT = (31, 31, 00) , 
	DFTRDBCOL = *NONE , 
	DYNDFTCOL = *NO , 
	DYNUSRPRF = *USER , 
	SRTSEQ = *HEX   
	BEGIN 
	
	DECLARE V_RETURN VARCHAR ( 1000 ) ; 
	SET V_RETURN = '' ; 
	
	
	IF ( P_NUMVOIE != 0 ) THEN 
		SET V_RETURN = TRIM ( P_NUMVOIE ) CONCAT ' ' ; 
	END IF ; 

	IF ( TRIM ( P_NUMVOIE2 ) != '' ) THEN 
		SET V_RETURN = TRIM ( V_RETURN )  CONCAT '/' CONCAT TRIM ( P_NUMVOIE2 ) CONCAT ' ' ; 
	END IF ; 
		
	IF ( TRIM ( P_EXTVOIE ) != '' ) THEN 
		SET V_RETURN = TRIM ( V_RETURN ) CONCAT TRIM ( P_EXTVOIE ) CONCAT ' ' ; 
	END IF ; 
	
	SET V_RETURN = V_RETURN CONCAT TRIM ( P_NOMVOIE ) ; 

	
	IF (TRIM(V_RETURN) <> '') THEN 
		IF ( RIGHT ( RTRIM ( V_RETURN ) , 1 ) = ' ' ) THEN 
		SET V_RETURN = SUBSTRING ( RTRIM ( V_RETURN ) , 1 , LENGTH ( RTRIM ( V_RETURN ) ) - 1 ) ; 
		END IF ; 
	END IF;
	
	RETURN TRIM ( V_RETURN ) ; 
END  ;