﻿CREATE TRIGGER ZALBINKHEO.TEST 
	AFTER INSERT ON ZALBINKHEO.YPOBAS0005 
	REFERENCING NEW AS NOUVLINE 
	FOR EACH ROW 
	MODE DB2ROW 
	SET OPTION  ALWBLK = *ALLREAD , 
	ALWCPYDTA = *OPTIMIZE , 
	COMMIT = *NONE , 
	CLOSQLCSR = *ENDMOD , 
	DECRESULT = (31, 31, 00) , 
	DFTRDBCOL = *NONE , 
	DYNDFTCOL = *NO , 
	DYNUSRPRF = *USER , 
	SRTSEQ = *HEX   
	BEGIN ATOMIC 
INSERT INTO ZALBINKHEO . KGRPTRG ( KDZTYP , KDZIPB , KDZALX , KDZTRT , KDZMAJA , KDZMAJM , KDZMAJJ , KDZACT , KDZOBS ) 
VALUES ( NOUVLINE . PBTYP , QSYS2 . RIGHT ( QSYS2 . REPEAT ( ' ' , 9 ) || NOUVLINE . PBIPB , 9 ) , NOUVLINE . PBALX , 'N' , NOUVLINE . PBMJA , NOUVLINE . PBMJM , NOUVLINE . PBMJJ , 'M' , '' ) ; 
END  ; 
  