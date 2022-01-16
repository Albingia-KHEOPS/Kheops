CREATE PROCEDURE SP_SCLAUSE ( 
	IN P_LIBELLE CHAR(60) , 
	IN P_MOTCLE1 INTEGER , 
	IN P_MOTCLE2 INTEGER , 
	IN P_MOTCLE3 INTEGER , 
	IN P_RUBRIQUE CHAR(5) , 
	IN P_SOUSRUBRIQUE CHAR(5) , 
	IN P_SEQUENCE INTEGER , 
	IN P_MODE CHAR(1) , 
	IN P_DATEOFFRE INTEGER ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_SCLAUSE 
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
  
DECLARE V_RETURN INTEGER DEFAULT 0 ; 
  
DECLARE V_QUERY VARCHAR ( 8000 ) DEFAULT '' ; 
DECLARE CURSOR1 CURSOR WITH RETURN FOR SQL_STATEMENT ; 
  
  
SET V_QUERY = 'SELECT KDUID CODE, KDUNM1 RUBRIQUE, KDUNM2 SOUSRUBRIQUE, KDUNM3 SEQUENCE, KDUVER VERSION, KDULIB LIBELLE
                                                                                         FROM KCLAUSE K1
                                                                          WHERE
                                                                                         (LOWER(TRIM(KDULIB)) LIKE ''%' CONCAT LOWER ( TRIM ( P_LIBELLE ) ) CONCAT '%'' OR (''' CONCAT TRIM ( P_LIBELLE ) CONCAT ''' = '''')) AND
                                                                                         (KDUKDXID = ' CONCAT P_MOTCLE1 CONCAT ' OR (' CONCAT P_MOTCLE1 CONCAT ' = 0)) AND
                                                                                         (KDUKDXID = ' CONCAT P_MOTCLE2 CONCAT ' OR (' CONCAT P_MOTCLE2 CONCAT ' = 0)) AND
                                                                                         (KDUKDXID = ' CONCAT P_MOTCLE3 CONCAT ' OR (' CONCAT P_MOTCLE3 CONCAT ' = 0)) AND
                                                                                         (KDUNM1 = ''' CONCAT TRIM ( P_RUBRIQUE ) CONCAT ''' OR (''' CONCAT TRIM ( P_RUBRIQUE ) CONCAT ''' = '''')) AND
                                                                                         (KDUNM2 = ''' CONCAT TRIM ( P_SOUSRUBRIQUE ) CONCAT ''' OR (''' CONCAT TRIM ( P_SOUSRUBRIQUE ) CONCAT ''' = '''')) AND
                                                                                         (KDUNM3 = ' CONCAT P_SEQUENCE CONCAT ' OR (' CONCAT P_SEQUENCE CONCAT ' = 0))' ; 
  
SET V_QUERY = V_QUERY CONCAT ' AND EXISTS(
                                                                          SELECT MAX(K.KDUVER)  
                                                                                         FROM KCLAUSE K 
                                                                          WHERE K.KDUNM1 = K1.KDUNM1 AND K.KDUNM2 = K1.KDUNM2 AND K.KDUNM3 = K1.KDUNM3' ; 
  
IF ( P_MODE = 'A' ) THEN 
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' AND K.KDUDATD <= ' CONCAT P_DATEOFFRE CONCAT ' AND (K.KDUDATF = 0 OR K.KDUDATF >= ' CONCAT P_DATEOFFRE CONCAT ')' ; 
END IF ; 
  
  
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' GROUP BY K.KDUNM1, K.KDUNM2, K.KDUNM3
                                                                                         HAVING MAX(K.KDUVER) = K1.KDUVER)' ; 
  
  
  
  
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' ORDER BY KDUNM1, KDUNM2, KDUNM3, KDUVER' ; 
  
PREPARE SQL_STATEMENT FROM V_QUERY ; 
OPEN CURSOR1 ; 
  
END P1  ; 
  

  

