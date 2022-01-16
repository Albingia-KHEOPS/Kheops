﻿CREATE PROCEDURE SP_FORMGARHISTO ( 
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_CODEFORMULE INTEGER , 
	IN P_CODEOPTION INTEGER , 
	IN P_CODECIBLE INTEGER , 
	IN P_DATENOW INTEGER , 
	IN P_AVENANT INTEGER ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_FORMGARHISTO 
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
	 
	DECLARE V_MODELEDATEAPPLI INTEGER DEFAULT 0 ; 
	DECLARE V_DATEEFFET INTEGER DEFAULT 0 ; 
	DECLARE V_DATECREATION INTEGER DEFAULT 0 ; 
  
	 
	 
	DECLARE CURSORFORMULE CURSOR FOR 
	 
		SELECT CODEOPTION , GUIDV , CODEVOLET , DESCRVOLET , CARACVOLET , GUIDVOLET , VOLETCOLLAPSE , CHECKV , GUIDB , CODEBLOC , DESCRBLOC , CARACBLOC , GUIDBLOC , CHECKB , CODEBLOCINCOMP , BLOCINCOMP , CODEBLOCASSOC , BLOCASSOC , 
			SOCLE . KARID GUIDM , SOCLE . KARMODELE CODEMODELE , D1LIB DESCRMODELE , 
			 
			GUIDNIV1 , ACTIONNIV1 , CODENIV1 , CODEGARNIV1 , DESCRNIV1 , CARACNIV1 , NATURENIV1 , NATUREPARAMNIV1 , CODEPARENTNIV1 , CODENIV1NIV1 , FLAGMODIFNIV1 , PARAMNATMODNIV1 , CODEGARINCOMPNIV1 , 
			CODEGARASSOCNIV1 , CODEGARAALTNIV1 , INVENPOSSIBLE1 , TYPEINVEN1 , CODEINVEN1 , ALIMNIV1 , 
  
			GUIDNIV2 , CODENIV2 , CODEGARNIV2 , DESCRNIV2 , CARACNIV2 , NATURENIV2 , NATUREPARAMNIV2 , CODEPARENTNIV2 , CODENIV1NIV2 , FLAGMODIFNIV2 , PARAMNATMODNIV2 , CODEGARINCOMPNIV2 , 
			CODEGARASSOCNIV2 , CODEGARAALTNIV2 , INVENPOSSIBLE2 , TYPEINVEN2 , CODEINVEN2 , 
			 
			GUIDNIV3 , CODENIV3 , CODEGARNIV3 , DESCRNIV3 , CARACNIV3 , NATURENIV3 , NATUREPARAMNIV3 , CODEPARENTNIV3 , CODENIV1NIV3 , FLAGMODIFNIV3 , PARAMNATMODNIV3 , CODEGARINCOMPNIV3 , 
			CODEGARASSOCNIV3 , CODEGARAALTNIV3 , INVENPOSSIBLE3 , TYPEINVEN3 , CODEINVEN3 , 
			 
			GUIDNIV4 , CODENIV4 , CODEGARNIV4 , DESCRNIV4 , CARACNIV4 , NATURENIV4 , NATUREPARAMNIV4 , CODEPARENTNIV4 , CODENIV1NIV4 , FLAGMODIFNIV4 , PARAMNATMODNIV4 , CODEGARINCOMPNIV4 , 
			CODEGARASSOCNIV4 , CODEGARAALTNIV4 , INVENPOSSIBLE4 , TYPEINVEN4 , CODEINVEN4 , 
			 
			APPLIQUEA 
		FROM V_FORMGARHISTO		 
		LEFT JOIN KCATMODELE SOCLE ON SOCLE . KARID = MODELEID AND SOCLE . KARDATEAPP <= V_MODELEDATEAPPLI 
			AND ( SOCLE . KARMODELE = MODELE1 OR SOCLE . KARMODELE = MODELE2 OR SOCLE . KARMODELE = MODELE3 OR SOCLE . KARMODELE = MODELE4 ) 
		LEFT JOIN ZALBINKMOD . YPLMGA ON SOCLE . KARMODELE = D1MGA 
		 
		WHERE CODEOFFRE = P_CODEOFFRE AND VERSION = P_VERSION AND TYPEOFFRE = P_TYPE AND CODEFOR = P_CODEFORMULE AND CODEOPT = P_CODEOPTION AND TYPEENRG = 'V' 
			AND CODECIBLE = P_CODECIBLE AND CODEAVN = P_AVENANT 
		ORDER BY VOLETORDRE , BLOCORDRE , SOCLE . KARDATEAPP DESC , TRI1 , TRI2 , TRI3 , TRI4 ; 
	 
	SET P_CODEOFFRE = F_PADLEFT ( 9 , P_CODEOFFRE ) ; 
	/* RÉCUPÉRATION DE LA DATE DE DÉBUT D'EFFET DE L'AFFAIRE OU DE LA DATE DE CRÉATION */ 
	SET V_MODELEDATEAPPLI = 0 ; 
	SET V_DATEEFFET = 0 ; 
	SET V_DATECREATION = 0 ; 
	 
	IF ( TRIM ( P_TYPE ) = 'P' ) THEN 
		SELECT IFNULL ( Y2 . PBSAA * 10000 + Y2 . PBSAM * 100 + Y2 . PBSAJ , 0 ) , Y1 . PBEFA * 10000 + Y1 . PBEFM * 100 + Y1 . PBEFJ 
			INTO V_DATECREATION , V_DATEEFFET 
			FROM YHPBASE Y1 
				LEFT JOIN YPOBASE Y2 ON Y1 . PBOFF = Y2 . PBIPB AND Y1 . PBOFV = Y2 . PBALX AND Y2 . PBTYP = 'O' 
			WHERE Y1 . PBIPB = P_CODEOFFRE AND Y1 . PBALX = P_VERSION AND Y1 . PBTYP = P_TYPE AND Y1 . PBAVN = P_AVENANT ; 
			 
		IF ( V_DATECREATION > 0 ) THEN 
			SET V_MODELEDATEAPPLI = V_DATECREATION ; 
		ELSE 
			SET V_MODELEDATEAPPLI = V_DATEEFFET ; 
		END IF ; 
	END IF ; 
	 
	IF ( TRIM ( P_TYPE ) = 'O' ) THEN 
		SELECT PBSAA * 10000 + PBSAM * 100 + PBSAJ INTO V_MODELEDATEAPPLI 
			FROM YHPBASE 
			WHERE PBIPB = P_CODEOFFRE AND PBALX = P_VERSION AND PBTYP = P_TYPE AND PBAVN = P_AVENANT ; 
	END IF ; 
  
  
/* 	SELECT PBEFA * 10000 + PBEFM * 100 + PBEFJ , PBDEA * 10000 + PBDEM * 100 + PBDEJ INTO V_DATEEFFET , V_DATECREATION      
		FROM YHPBASE WHERE TRIM ( PBIPB ) = TRIM ( P_CODEOFFRE ) AND PBALX = P_VERSION AND PBTYP = P_TYPE AND PBAVN = P_AVENANT ;      
	IF ( V_DATEEFFET > 0 ) THEN      
		SET V_MODELEDATEAPPLI = V_DATEEFFET ;      
	ELSE      
		IF ( V_DATECREATION > 0 ) THEN      
			SET V_MODELEDATEAPPLI = V_DATECREATION ;      
		END IF ;      
	END IF ;	 */ 
	 
	OPEN CURSORFORMULE ; 
END P1  ; 
  

  
