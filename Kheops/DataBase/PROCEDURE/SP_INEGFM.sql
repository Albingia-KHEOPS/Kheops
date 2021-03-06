CREATE PROCEDURE SP_INEGFM ( 
	IN P_DATE_DEBUT INTEGER , 
	IN P_DATE_FIN INTEGER , 
	IN P_ID_ENGAGEMENT NUMERIC(15, 0) , 
	IN P_FAMILLE CHAR(10) , 
	IN P_ENG_TOTAL DECIMAL(13, 0) , 
	IN P_ENG_ALBIN DECIMAL(13, 0) , 
	IN P_CR_USER CHAR(10) , 
	IN P_CR_DATE NUMERIC(8, 0) , 
	IN P_MAJ_USER CHAR(10) , 
	IN P_MAJ_DATE NUMERIC(8, 0) , 
	OUT P_ERREUR CHAR(128) ) 
	LANGUAGE SQL 
	SPECIFIC SP_INEGFM 
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
DECLARE V_KDPID INTEGER DEFAULT 0 ; 
CALL SP_NCHRONO ( 'KDPID' , V_KDPID ) ; 
INSERT INTO KPENGFAM ( KDPID , KDPTYP , KDPIPB , KDPALX , KDPKDOID , KDPFAM , KDPENG , KDPENA , KDPCRU , KDPCRD , KDPMAJU , KDPMAJD ) 
VALUES ( V_KDPID , '' , '' , 0 , P_ID_ENGAGEMENT , P_FAMILLE , P_ENG_TOTAL , P_ENG_ALBIN , P_CR_USER , P_CR_DATE , P_MAJ_USER , P_MAJ_DATE ) ; 
END P1  ; 
  

  

