CREATE PROCEDURE SP_UPDATEINVENTAIRE ( 
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_CODERSQ INTEGER , 
	IN P_CODEOBJ INTEGER , 
	IN P_CODEINVEN INTEGER , 
	IN P_DESCRIPTIF CHAR(40) , 
	IN P_DESCRIPTION CHAR(5000) , 
	IN P_VALREPORT NUMERIC(13, 2) , 
	IN P_UNITREPORT CHAR(3) , 
	IN P_TYPEREPORT CHAR(5) , 
	IN P_TAXEREPORT CHAR(1) , 
	IN P_ACTIVEREPORT CHAR(1) , 
	IN P_TYPEALIM CHAR(1) , 
	IN P_GARANTIE INTEGER , 
	IN P_PERIMETRE CHAR(2) ) 
	LANGUAGE SQL 
	SPECIFIC SP_UPDATEINVENTAIRE 
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
	DECLARE V_CODEDESI INTEGER DEFAULT 0 ; 
	DECLARE V_VALORIG NUMERIC ( 7 , 2 ) DEFAULT 0 ; 
	DECLARE V_VALACTU NUMERIC ( 7 , 2 ) DEFAULT 0 ; 
	DECLARE V_FLAGREPORT CHAR ( 1 ) DEFAULT '' ; 
  
	/* TRAITEMENT DE LA DESIGNATION */ 
	SELECT KBEKADID INTO V_CODEDESI FROM KPINVEN WHERE KBEID = P_CODEINVEN ; 
	IF ( V_CODEDESI = 0 ) THEN 
		CALL SP_NCHRONO ( 'KADCHR' , V_CODEDESI ) ; 
		INSERT INTO KPDESI 
			( KADCHR , KADTYP , KADIPB , KADALX , KADPERI , KADRSQ , KADOBJ , KADDESI ) 
		VALUES 
			( V_CODEDESI , P_TYPE , P_CODEOFFRE , P_VERSION , P_PERIMETRE , P_CODERSQ , P_CODEOBJ , P_DESCRIPTION ) ; 
	ELSE 
		UPDATE KPDESI SET KADDESI = P_DESCRIPTION WHERE KADCHR = V_CODEDESI ; 
	END IF ; 
  
	/* RÉCUPÉRATION DES VALEURS D'ORIGINE ET ACTUALISÉE */ 
	SELECT JDIVO , JDIVA INTO V_VALORIG , V_VALACTU FROM YPRTENT WHERE TRIM ( JDIPB ) = TRIM ( P_CODEOFFRE ) AND JDALX = P_VERSION ; 
	 
	/* RÉCUPÉRATION DU FLAG DE REPORT */ 
	SELECT KBEREPVAL INTO V_FLAGREPORT FROM KPINVEN WHERE KBEID = P_CODEINVEN ; 
	 
	/* MISE À JOUR DE L'INVENTAIRE */ 
	UPDATE KPINVEN 
		SET KBEDESC = P_DESCRIPTIF , KBEKADID = V_CODEDESI , KBEREPVAL = P_ACTIVEREPORT , KBEVAL = P_VALREPORT , KBEVAA = P_VALREPORT , KBEVAW = P_VALREPORT , 
			KBEVAT = P_TYPEREPORT , KBEVAU = P_UNITREPORT , KBEVAH = P_TAXEREPORT , KBEIVO = V_VALORIG , KBEIVA = V_VALACTU 
		WHERE KBEID = P_CODEINVEN ; 
		 
	UPDATE KPGARAN SET KDEINVEN = P_CODEINVEN WHERE KDEID = P_GARANTIE ; 
  
	IF ( P_ACTIVEREPORT = 'O' ) THEN 
		IF ( P_GARANTIE != 0 ) THEN 
			IF ( P_TYPEALIM = 'I' ) THEN 
				UPDATE KPGARAN SET KDEASVALO = P_VALREPORT , KDEASVALA = P_VALREPORT , KDEASVALW = 0 , KDEASUNIT = P_UNITREPORT , KDEASBASE = P_TYPEREPORT WHERE KDEID = P_GARANTIE ; 
			ELSE 
				UPDATE KPGARAN SET KDEASVALW = P_VALREPORT WHERE KDEID = P_GARANTIE ; 
			END IF ; 
		ELSE 
			UPDATE YPRTOBJ 
				SET JGVAL = P_VALREPORT , JGVAA = P_VALREPORT , JGVAW = P_VALREPORT , JGVAU = P_UNITREPORT , JGVAT = P_TYPEREPORT , JGVAH = P_TAXEREPORT 
				WHERE TRIM ( JGIPB ) = TRIM ( P_CODEOFFRE ) AND JGALX = P_VERSION AND JGRSQ = P_CODERSQ AND JGOBJ = P_CODEOBJ ; 
		END IF ;	 
	ELSE 
		IF ( P_GARANTIE != 0 AND V_FLAGREPORT = 'O' ) THEN 
			IF ( P_TYPEALIM = 'I' ) THEN 
				UPDATE KPGARAN SET KDEASVALO = 0 , KDEASVALA = 0 , KDEASVALW = 0 , KDEASUNIT = '' , KDEASBASE = '' WHERE KDEID = P_GARANTIE ; 
			ELSE 
				UPDATE KPGARAN SET KDEASVALW = 0 WHERE KDEID = P_GARANTIE ; 
			END IF ; 
		END IF ; 
	END IF ; 
	 
END P1  ; 
  

  

