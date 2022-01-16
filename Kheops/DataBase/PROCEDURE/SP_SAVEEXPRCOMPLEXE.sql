﻿CREATE PROCEDURE SP_SAVEEXPRCOMPLEXE ( 
	IN P_IDEXPR INTEGER , 
	IN P_TYPEEXPR CHAR(9) , 
	IN P_CODEEXPR CHAR(5) , 
	IN P_LIBEXPR CHAR(60) , 
	IN P_MODIFEXPR CHAR(1) , 
	IN P_DESCREXPR CHAR(500) , 
	OUT P_OUTIDEXPR INTEGER ) 
	LANGUAGE SQL 
	SPECIFIC SP_SAVEEXPRCOMPLEXE 
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
  
DECLARE V_IDDESCR INTEGER DEFAULT 0 ; 
DECLARE V_NBCOUNT INTEGER DEFAULT 0 ; 
  
IF ( P_IDEXPR = 0 ) THEN 
IF ( P_TYPEEXPR = 'LCI' ) THEN 
/* VÉRIFICATION QUE LE CODE EST UNIQUE EN BASE */ 
SELECT COUNT ( * ) INTO V_NBCOUNT FROM KEXPLCI WHERE KHGLCE = P_CODEEXPR ; 
IF ( V_NBCOUNT > 0 ) THEN 
SET P_IDEXPR = - 1 ; 
END IF ; 
END IF ; 
IF ( P_TYPEEXPR = 'FRANCHISE' ) THEN 
/* VÉRIFICATION QUE LE CODE EST UNIQUE EN BASE */ 
SELECT COUNT ( * ) INTO V_NBCOUNT FROM KEXPFRH WHERE KHEFHE = P_CODEEXPR ; 
IF ( V_NBCOUNT > 0 ) THEN 
SET P_IDEXPR = - 1 ; 
END IF ; 
END IF ; 
END IF ; 
  
IF ( P_IDEXPR >= 0 ) THEN 
/*                          SAUVEGARDE DE LA DESCRIPTION           */ 
IF ( P_TYPEEXPR = 'LCI' ) THEN 
SELECT KHGDESI INTO V_IDDESCR FROM KEXPLCI WHERE KHGID = P_IDEXPR ; 
END IF ; 
IF ( P_TYPEEXPR = 'FRANCHISE' ) THEN 
SELECT KHEDESI INTO V_IDDESCR FROM KEXPFRH WHERE KHEID = P_IDEXPR ; 
END IF ; 
IF ( V_IDDESCR > 0 ) THEN 
UPDATE KDESI SET KDWDESI = P_DESCREXPR WHERE KDWID = V_IDDESCR ; 
ELSE 
IF ( TRIM ( P_DESCREXPR ) != '' ) THEN 
CALL SP_NCHRONO ( 'KDWID' , V_IDDESCR ) ; 
INSERT INTO KDESI 
( KDWID , KDWDESI ) 
VALUES 
( V_IDDESCR , TRIM ( P_DESCREXPR ) ) ; 
END IF ; 
END IF ; 
  
/*                          SAUVEGARDE DES INFORMATIONS DE L'EXPRESSION COMPLEXE */ 
IF ( P_IDEXPR > 0 ) THEN 
IF ( P_TYPEEXPR = 'LCI' ) THEN 
UPDATE KEXPLCI SET KHGLCE = P_CODEEXPR , KHGDESC = P_LIBEXPR , KHGDESI = V_IDDESCR , KHGMODI = P_MODIFEXPR 
WHERE KHGID = P_IDEXPR ; 
END IF ; 
IF ( P_TYPEEXPR = 'FRANCHISE' ) THEN 
UPDATE KEXPFRH SET KHEFHE = P_CODEEXPR , KHEDESC = P_LIBEXPR , KHEDESI = V_IDDESCR , KHEMODI = P_MODIFEXPR 
WHERE KHEID = P_IDEXPR ; 
END IF ; 
ELSE 
IF ( P_TYPEEXPR = 'LCI' ) THEN 
CALL SP_NCHRONO ( 'KHGID' , P_IDEXPR ) ; 
INSERT INTO KEXPLCI 
( KHGID , KHGLCE , KHGDESC , KHGDESI , KHGMODI ) 
VALUES 
( P_IDEXPR , P_CODEEXPR , P_LIBEXPR , V_IDDESCR , P_MODIFEXPR ) ; 
END IF ; 
IF ( P_TYPEEXPR = 'FRANCHISE' ) THEN 
CALL SP_NCHRONO ( 'KHEID' , P_IDEXPR ) ; 
INSERT INTO KEXPFRH 
( KHEID , KHEFHE , KHEDESC , KHEDESI , KHEMODI ) 
VALUES 
( P_IDEXPR , P_CODEEXPR , P_LIBEXPR , V_IDDESCR , P_MODIFEXPR ) ; 
END IF ; 
END IF ; 
END IF ; 
SET P_OUTIDEXPR = P_IDEXPR ; 
  
END P1  ; 
  

  
