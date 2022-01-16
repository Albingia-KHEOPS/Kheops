﻿CREATE PROCEDURE SP_BACKGARANTIE ( 
	IN P_CODEAFFAIRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_CODEFOR INTEGER , 
	IN P_CODEOPT INTEGER ) 
	LANGUAGE SQL 
	SPECIFIC SP_BACKGARANTIE 
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
	SET V_COUNT = 0 ; 
	SET P_CODEAFFAIRE = F_PADLEFT ( 9 , P_CODEAFFAIRE ) ; 
	 
	SELECT COUNT ( * ) INTO V_COUNT FROM KPGARAW WHERE KDEIPB = P_CODEAFFAIRE AND KDEALX = P_VERSION AND KDETYP = P_TYPE AND KDEFOR = P_CODEFOR AND KDEOPT = P_CODEOPT ; 
	IF ( V_COUNT = 0 ) THEN 
		 -- SI MODE CRÉATION 
		CALL SP_DELFORM ( P_CODEAFFAIRE , P_VERSION , P_TYPE , P_CODEFOR , 'C' ) ; 
	ELSE 
		 -- SI MODE MODIFICATION 
		DELETE FROM KPOPTD WHERE KDCIPB = P_CODEAFFAIRE AND KDCALX = P_VERSION AND KDCTYP = P_TYPE AND KDCFOR = P_CODEFOR AND KDCOPT = P_CODEOPT ; 
		DELETE FROM KPGARAN WHERE KDEIPB = P_CODEAFFAIRE AND KDEALX = P_VERSION AND KDETYP = P_TYPE AND KDEFOR = P_CODEFOR AND KDEOPT = P_CODEOPT ; 
		DELETE FROM KPGARTAR WHERE KDGIPB = P_CODEAFFAIRE AND KDGALX = P_VERSION AND KDGTYP = P_TYPE AND KDGFOR = P_CODEFOR AND KDGOPT = P_CODEOPT ; 
  
		INSERT INTO KPOPTD ( SELECT * FROM KPOPTDW WHERE KDCIPB = P_CODEAFFAIRE AND KDCALX = P_VERSION AND KDCTYP = P_TYPE AND KDCFOR = P_CODEFOR AND KDCOPT = P_CODEOPT ) ; 
		INSERT INTO KPGARAN ( SELECT * FROM KPGARAW WHERE KDEIPB = P_CODEAFFAIRE AND KDEALX = P_VERSION AND KDETYP = P_TYPE AND KDEFOR = P_CODEFOR AND KDEOPT = P_CODEOPT ) ; 
		INSERT INTO KPGARTAR ( SELECT * FROM KPGARTAW WHERE KDGIPB = P_CODEAFFAIRE AND KDGALX = P_VERSION AND KDGTYP = P_TYPE AND KDGFOR = P_CODEFOR AND KDGOPT = P_CODEOPT ) ; 
  
		DELETE FROM KPOPTDW WHERE KDCIPB = P_CODEAFFAIRE AND KDCALX = P_VERSION AND KDCTYP = P_TYPE AND KDCFOR = P_CODEFOR AND KDCOPT = P_CODEOPT ; 
		DELETE FROM KPGARAW WHERE KDEIPB = P_CODEAFFAIRE AND KDEALX = P_VERSION AND KDETYP = P_TYPE AND KDEFOR = P_CODEFOR AND KDEOPT = P_CODEOPT ; 
		DELETE FROM KPGARTAW WHERE KDGIPB = P_CODEAFFAIRE AND KDGALX = P_VERSION AND KDGTYP = P_TYPE AND KDGFOR = P_CODEFOR AND KDGOPT = P_CODEOPT ; 
	END IF ; 
  
END P1  ; 
  

  

