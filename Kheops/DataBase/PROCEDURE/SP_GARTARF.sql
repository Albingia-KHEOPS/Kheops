CREATE PROCEDURE SP_GARTARF ( 
	IN P_SEQUENCE INTEGER , 
	IN P_TYPE INTEGER , 
	OUT P_MOD CHAR(1) , 
	OUT P_OBL CHAR(1) , 
	OUT P_VAL DECIMAL(16, 4) , 
	OUT P_UNT CHAR(3) , 
	OUT P_BAS CHAR(3) ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_GARTARF 
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
SELECT C4BAS , C4VAL , CASE C4UNT WHEN 'UM' THEN 'D' ELSE C4UNT END , C4MAJ , C4OBL INTO P_BAS , P_VAL , P_UNT , P_MOD , P_OBL 
FROM ZALBINKMOD . YPLTGAL 
WHERE C4SEQ = P_SEQUENCE AND C4TYP = P_TYPE ; 
END P1  ; 
  

  

