﻿CREATE PROCEDURE SP_DELPARAMCLAUSE ( 
	IN P_ETAPE CHAR(30) , 
	IN P_CODEPARAM CHAR(15) ) 
	LANGUAGE SQL 
	SPECIFIC SP_DELPARAMCLAUSE 
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
  
IF ( TRIM ( P_ETAPE ) = 'EGDI' ) THEN 
DELETE FROM KALCELG WHERE KEJID = TRIM ( P_CODEPARAM ) ; 
END IF ; 
  
IF ( TRIM ( P_ETAPE ) = 'ActeGestion' ) THEN 
DELETE FROM KALCELG AS L WHERE EXISTS ( 
SELECT KEJID FROM KALCELG 
INNER JOIN KALCONT ON KEJKEIID = KEIID 
INNER JOIN KALETAP ON KEIKEHID = KEHID 
WHERE KEHKEGID = TRIM ( P_CODEPARAM ) AND KEJID = L . KEJID ) ; 
  
DELETE FROM KALCONT AS L WHERE EXISTS ( 
SELECT KEIID FROM KALCONT 
INNER JOIN KALETAP ON KEIKEHID = KEHID 
WHERE KEHKEGID = TRIM ( P_CODEPARAM ) AND KEIID = L . KEIID ) ; 
  
DELETE FROM KALETAP WHERE KEHKEGID = TRIM ( P_CODEPARAM ) ; 
  
DELETE FROM KALACTG WHERE KEGID = TRIM ( P_CODEPARAM ) ; 
END IF ; 
  
IF ( TRIM ( P_ETAPE ) = 'Etape' ) THEN 
DELETE FROM KALCELG AS L WHERE EXISTS ( 
SELECT KEJID FROM KALCELG 
INNER JOIN KALCONT ON KEJKEIID = KEIID 
WHERE KEIKEHID = TRIM ( P_CODEPARAM ) AND KEJID = L . KEJID ) ; 
  
DELETE FROM KALCONT WHERE KEIKEHID = TRIM ( P_CODEPARAM ) ; 
DELETE FROM KALETAP WHERE KEHID = TRIM ( P_CODEPARAM ) ; 
END IF ; 
  
IF ( TRIM ( P_ETAPE ) = 'Contexte' ) THEN 
DELETE FROM KALCELG WHERE KEJKEIID = TRIM ( P_CODEPARAM ) ; 
DELETE FROM KALCONT WHERE KEIID = TRIM ( P_CODEPARAM ) ; 
END IF ; 
  
END P1  ; 
  

  
