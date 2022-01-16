﻿CREATE PROCEDURE SP_COPYENGAGEMENT ( 
	IN P_CODEAFFAIRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_CODEAFFAIREDEST CHAR(9) , 
	IN P_VERSIONDEST INTEGER , 
	IN P_TYPEDEST CHAR(1) ) 
	LANGUAGE SQL 
	SPECIFIC SP_COPYENGAGEMENT 
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
  
	DECLARE V_CODEOBSV INTEGER DEFAULT 0 ; 
	DECLARE V_CODEOBSVDEST INTEGER DEFAULT 0 ; 
	DECLARE V_TYPEOBSV CHAR ( 10 ) DEFAULT '' ; 
	DECLARE V_OBSV VARCHAR ( 5000 ) DEFAULT '' ; 
  
	/* COPIE DES ENGAGEMENTS FORCES */ 
	FOR LOOP_KPENGRSQ AS FREE_LIST CURSOR FOR 
		SELECT KDRRSQ CODERSQ , KDRFAM ENGFAM , KDRVEN ENGVEN , KDRENGF ENGFORCE , KDRSMF SMPF FROM KPENGRSQ WHERE TRIM ( KDRIPB ) = TRIM ( P_CODEAFFAIRE ) AND KDRALX = P_VERSION AND KDRTYP = P_TYPE 
	DO 
		UPDATE KPENGRSQ SET KDRENGF = ENGFORCE , KDRSMF = SMPF
			WHERE TRIM ( KDRIPB ) = TRIM ( P_CODEAFFAIREDEST ) AND KDRALX = P_VERSIONDEST AND KDRTYP = P_TYPEDEST AND KDRRSQ = CODERSQ 
				AND KDRFAM = ENGFAM AND KDRVEN = ENGVEN ; 
		UPDATE KPENGVEN SET KDQENGF = ENGFORCE 
			WHERE TRIM ( KDQIPB ) = TRIM ( P_CODEAFFAIREDEST ) AND KDQALX = P_VERSIONDEST AND KDQTYP = P_TYPEDEST 
				AND KDQFAM = ENGFAM AND KDQVEN = ENGVEN ; 
	END FOR ; 
	 
	/* COPIE DES OBSERVATIONS ENGAGEMENTS*/ 
	SET V_CODEOBSV = 0 ; 
	SET V_CODEOBSVDEST = 0 ; 
	SELECT KDOOBSV , KAJTYPOBS , KAJOBSV INTO V_CODEOBSV , V_TYPEOBSV , V_OBSV 
		FROM KPENG 
			LEFT JOIN KPOBSV ON KAJCHR = KDOOBSV AND KAJIPB = KDOIPB AND KAJALX = KDOALX AND KAJTYP = KDOTYP 
		WHERE TRIM ( KDOIPB ) = TRIM ( P_CODEAFFAIRE ) AND KDOALX = P_VERSION AND KDOTYP = P_TYPE ; 
	IF ( V_CODEOBSV > 0 ) THEN 
		SELECT KDOOBSV INTO V_CODEOBSVDEST FROM KPENG WHERE TRIM ( KDOIPB ) = TRIM ( P_CODEAFFAIREDEST ) AND KDOALX = P_VERSIONDEST AND KDOTYP = P_TYPEDEST ; 
		IF ( V_CODEOBSVDEST = 0 ) THEN 
			CALL SP_NCHRONO ( 'KAJCHR' , V_CODEOBSVDEST ) ; 
			INSERT INTO KPOBSV 
				( KAJCHR , KAJTYP , KAJIPB , KAJALX , KAJTYPOBS , KAJOBSV ) 
			VALUES 
				( V_CODEOBSVDEST , P_TYPEDEST , P_CODEAFFAIREDEST , P_VERSIONDEST , V_TYPEOBSV , V_OBSV ) ; 
			UPDATE KPENG SET KDOOBSV = V_CODEOBSVDEST WHERE TRIM ( KDOIPB ) = TRIM ( P_CODEAFFAIREDEST ) AND KDOALX = P_VERSIONDEST AND KDOTYP = P_TYPEDEST ; 
		ELSE 
			UPDATE KPOBSV SET KAJOBSV = V_OBSV WHERE KAJCHR = V_CODEOBSVDEST ; 
		END IF ; 
		 
	END IF ; 
	 
END P1  ; 
  

  
