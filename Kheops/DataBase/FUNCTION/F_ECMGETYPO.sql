CREATE FUNCTION ZALBINKHEO.F_ECMGETYPO ( 
	P_NATURECONTRAT VARCHAR(3) , 
	P_INTERCALAIRE VARCHAR(1) , 
	P_CODEBLOC INTEGER , 
	P_DATENOW INTEGER ) 
	RETURNS INTEGER   
	LANGUAGE SQL 
	SPECIFIC ZALBINKHEO.F_ECMGETYPO 
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
  
	 
DECLARE V_MODELEID INTEGER DEFAULT 0 ; 
DECLARE V_TYPO VARCHAR ( 3 ) DEFAULT '' ; 
DECLARE V_TYPOBLOC VARCHAR ( 20 ) DEFAULT '' ; 
DECLARE V_TYPOSTD VARCHAR ( 5 ) DEFAULT ';STD;' ; 
DECLARE V_TYPOITC VARCHAR ( 5 ) DEFAULT ';ITC;' ; 
DECLARE V_TYPOCOA VARCHAR ( 5 ) DEFAULT ';COA;' ; 
  
SET V_TYPO = 'STD' ; 
  
FOR LOOV_TYPO AS FREE_LIST CURSOR FOR 
	SELECT DISTINCT KARTYPO 
	FROM ZALBINKHEO . KCATMODELE 
	WHERE KARKAQID = P_CODEBLOC AND KARDATEAPP <= P_DATENOW 
DO 
	SET V_TYPOBLOC = V_TYPOBLOC || ';' || TRIM ( KARTYPO ) ; 
END FOR ; 

SET V_TYPOBLOC = V_TYPOBLOC || ';' ; 
  
IF P_NATURECONTRAT = 'C' AND LOCATE ( V_TYPOCOA , V_TYPOBLOC ) > 0 THEN 
	SET V_TYPO = 'COA' ; 
ELSEIF P_INTERCALAIRE = 'O' AND LOCATE ( V_TYPOITC , V_TYPOBLOC ) > 0 THEN 
	SET V_TYPO = 'ITC' ; 
ELSE 
	SET V_TYPO = 'STD' ; 
END IF ; 

SELECT KARID INTO V_MODELEID FROM ZALBINKHEO . KCATMODELE WHERE KARTYPO = V_TYPO AND KARKAQID = P_CODEBLOC AND KARDATEAPP <= P_DATENOW 
ORDER BY KARDATEAPP DESC FETCH FIRST 1 ROW ONLY ; 
  
RETURN V_MODELEID ; 
	 
END  ; 
  
SET PATH "QSYS","QSYS2","SYSPROC","SYSIBMADM","YALBIN" ; 
  
