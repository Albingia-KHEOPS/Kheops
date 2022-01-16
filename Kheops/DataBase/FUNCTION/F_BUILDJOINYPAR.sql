﻿CREATE FUNCTION ZALBINKHEO.F_BUILDJOINYPAR ( 
	P_TYPEJOIN VARCHAR(5) , 
	P_CON VARCHAR(5) , 
	P_FAM VARCHAR(5) , 
	P_ALIAS VARCHAR(20) , 
	P_OTHERCRITERIA VARCHAR(500) ) 
	RETURNS VARCHAR(1000)   
	LANGUAGE SQL 
	SPECIFIC ZALBINKHEO.F_BUILDJOINYPAR 
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
	DECLARE P_RETURN VARCHAR ( 1000 ) ; 
	SET P_RETURN = TRIM ( P_TYPEJOIN ) CONCAT ' JOIN ZALBINKHEO.YYYYPAR ' ; 
  
IF ( P_ALIAS != '' ) THEN 
SET P_RETURN = TRIM ( P_RETURN ) CONCAT ' ' CONCAT TRIM ( P_ALIAS ) ; 
END IF ; 
  
SET P_RETURN = TRIM ( P_RETURN ) CONCAT ' ON *.TCON = ''' CONCAT TRIM ( P_CON ) CONCAT ''' AND *.TFAM = ''' CONCAT TRIM ( P_FAM ) CONCAT ''' ' ; 
  
IF ( P_OTHERCRITERIA != '' ) THEN 
SET P_RETURN = TRIM ( P_RETURN ) CONCAT ' ' CONCAT TRIM ( P_OTHERCRITERIA ) ; 
END IF ; 
  
IF ( P_ALIAS != '' ) THEN 
SET P_RETURN = REPLACE ( TRIM ( P_RETURN ) , '*.' , TRIM ( P_ALIAS ) CONCAT '.' ) ; 
ELSE 
SET P_RETURN = REPLACE ( TRIM ( P_RETURN ) , '*.' , '' ) ; 
END IF ; 
  
RETURN TRIM ( P_RETURN ) ; 
END  ; 
  
SET PATH "QSYS","QSYS2","SYSPROC","SYSIBMADM","YALBIN" ; 
  
