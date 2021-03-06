CREATE OR REPLACE PROCEDURE SP_CKPOBJ(
	IN P_CODEOFFRE VARCHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE VARCHAR(1) , 
	IN P_CODERSQ INTEGER , 
	IN P_CODEOBJ INTEGER , 
	IN P_NEWVERSION INTEGER , 
	IN P_CODECONTRAT VARCHAR(9) , 
	IN P_VERSIONCONTRAT INTEGER , 
	IN P_TRAITEMENT VARCHAR(1) , 
	IN P_COPYCODEOFFRE CHAR(9) , 
	IN P_COPYVERSION INTEGER , 
	IN P_MODECOPY CHAR(7) ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
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
  
	DECLARE V_CIBLE VARCHAR ( 10 ) DEFAULT '' ; 
	DECLARE V_INVEN INTEGER DEFAULT 0 ; 
	DECLARE V_DESCR VARCHAR ( 40 ) DEFAULT '' ; 
	DECLARE V_DESI INTEGER DEFAULT 0 ; 
	DECLARE V_OBSV INTEGER DEFAULT 0 ; 
	DECLARE V_TRE CHAR ( 5 ) DEFAULT '' ; 
	DECLARE V_NOMEN1 CHAR ( 15 ) DEFAULT '' ; 
	DECLARE V_NOMEN2 CHAR ( 15 ) DEFAULT '' ; 
	DECLARE V_NOMEN3 CHAR ( 15 ) DEFAULT '' ; 
	DECLARE V_NOMEN4 CHAR ( 15 ) DEFAULT '' ; 
	DECLARE V_NOMEN5 CHAR ( 15 ) DEFAULT '' ; 
	DECLARE V_CLASS CHAR ( 2 ) DEFAULT '' ; 
	 
	DECLARE V_NEWINVEN INTEGER DEFAULT 0 ; 
	DECLARE V_NEWDESI INTEGER DEFAULT 0 ; 
	DECLARE V_NEWOBSV INTEGER DEFAULT 0 ; 
	 
	DECLARE V_CODEOFFRE VARCHAR ( 9 ) DEFAULT '' ; 
	DECLARE V_TYPEOFFRE VARCHAR ( 1 ) DEFAULT '' ; 
	DECLARE V_VERSOFFRE INTEGER DEFAULT 0 ; 
	DECLARE V_NEWVERS INTEGER DEFAULT 0 ; 
	 
	DECLARE V_MANIFHDEB INTEGER DEFAULT 0 ; 
	DECLARE V_MANIFHFIN INTEGER DEFAULT 0 ; 
	 
	DECLARE V_KACMAND INTEGER DEFAULT 0 ; 
	DECLARE V_KACMANF INTEGER DEFAULT 0 ; 
	 
	DECLARE V_NSIR CHAR ( 14 ) DEFAULT '' ; 
	DECLARE V_SURF DECIMAL ( 11 , 2 ) DEFAULT 0 ; 
	DECLARE V_VMC DECIMAL ( 11 , 2 ) DEFAULT 0 ; 
	DECLARE V_PROL CHAR ( 1 ) DEFAULT '' ; 
	DECLARE V_PBI CHAR ( 1 ) DEFAULT '' ; 
	DECLARE V_COUTM2 INTEGER DEFAULT 0 ; 
	 
	SET P_CODEOFFRE = LPAD ( TRIM ( P_CODEOFFRE ) , 9 , ' ') ;
	SET P_CODECONTRAT = LPAD ( TRIM ( P_CODECONTRAT ) , 9 , ' ') ;
	SET P_COPYCODEOFFRE = LPAD ( TRIM ( P_COPYCODEOFFRE ) , 9 , ' ') ;

	SET V_CODEOFFRE = P_CODEOFFRE ; 
	SET V_TYPEOFFRE = P_TYPE ; 
	SET V_NEWVERS = P_NEWVERSION ; 
	SET V_VERSOFFRE = P_VERSION ; 
	 
	IF ( P_TRAITEMENT = 'P' ) THEN 
		SET V_CODEOFFRE = P_CODECONTRAT ; 
		SET V_TYPEOFFRE = 'P' ; 
		SET V_NEWVERS = P_VERSIONCONTRAT ; 
		SET V_VERSOFFRE = P_VERSIONCONTRAT ; 
	END IF ; 
	 
	IF ( P_TRAITEMENT = 'C' ) THEN 
		SET V_CODEOFFRE = P_COPYCODEOFFRE ; 
		SET V_NEWVERS = P_COPYVERSION ; 
		SET V_VERSOFFRE = P_COPYVERSION ; 
	END IF ; 
	 
	SELECT KACCIBLE , IFNULL ( KBGKBEID , 0 ) , KACDESC , KACDESI , KACOBSV , KACTRE , KACNMC01 , KACNMC02 , KACNMC03 , KACNMC04 , KACNMC05 , KACCLASS , KACMANDH , KACMANFH , KACMAND , KACMANF , 
			KACNSIR , KACSURF , KACVMC , KACPROL , KACPBI , KACCM2
			INTO V_CIBLE , V_INVEN , V_DESCR , V_DESI , V_OBSV , V_TRE , V_NOMEN1 , V_NOMEN2 , V_NOMEN3 , V_NOMEN4 , V_NOMEN5 , V_CLASS , V_MANIFHDEB , V_MANIFHFIN , V_KACMAND , V_KACMANF , 
			V_NSIR , V_SURF , V_VMC , V_PROL , V_PBI , V_COUTM2
		FROM KPOBJ 
			LEFT JOIN KPINVAPP ON KACIPB = KBGIPB AND KACALX = KBGALX AND KACTYP = KBGTYP AND KACRSQ = KBGRSQ AND KACOBJ = KBGOBJ 
		WHERE KACTYP = P_TYPE AND KACIPB = P_CODEOFFRE AND KACALX = P_VERSION AND KACRSQ = P_CODERSQ AND KACOBJ = P_CODEOBJ ; 
	 
	IF ( V_DESI != 0 ) THEN 
		CALL SP_SECOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KADCHR' , V_DESI , V_NEWDESI ) ; 
		IF ( V_NEWDESI = 0 ) THEN 
			CALL SP_NCHRONO ( 'KADCHR' , V_NEWDESI ) ; 
			CALL SP_INCOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KADCHR' , V_DESI , V_NEWDESI ) ; 
		END IF ; 
	END IF ; 
	 
	IF ( V_OBSV != 0 ) THEN 
		CALL SP_SECOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KAJCHR' , V_OBSV , V_NEWOBSV ) ; 
		IF ( V_NEWOBSV = 0 ) THEN 
			CALL SP_NCHRONO ( 'KAJCHR' , V_NEWOBSV ) ; 
			CALL SP_INCOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KAJCHR' , V_OBSV , V_NEWOBSV ) ; 
		END IF ; 
	END IF ; 
	 
	IF ( TRIM ( P_MODECOPY ) = 'VERSION' OR TRIM ( P_MODECOPY ) = 'AFFNOUV' ) THEN 
		INSERT INTO KPOBJ 
		( KACTYP , KACIPB , KACALX , KACRSQ , KACOBJ , KACCIBLE , KACINVEN , KACDESC , KACDESI , KACOBSV , KACTRE , KACNMC01 , KACNMC02 , KACNMC03 , KACNMC04 , KACNMC05 , KACCLASS , KACMANDH , KACMANFH , KACMAND , KACMANF , KACNSIR , KACSURF , KACVMC , KACPROL , KACPBI , KACCM2 ) 
		VALUES 
		( V_TYPEOFFRE , V_CODEOFFRE , V_NEWVERS , P_CODERSQ , P_CODEOBJ , V_CIBLE , V_INVEN , V_DESCR , V_NEWDESI , V_NEWOBSV , V_TRE , V_NOMEN1 , V_NOMEN2 , V_NOMEN3 , V_NOMEN4 , V_NOMEN5 , V_CLASS , V_MANIFHDEB , V_MANIFHFIN , V_KACMAND , V_KACMANF , V_NSIR , V_SURF , V_VMC , V_PROL , V_PBI , V_COUTM2 ) ; 
		 
	ELSE 
		INSERT INTO KPOBJ 
		( KACTYP , KACIPB , KACALX , KACRSQ , KACOBJ , KACCIBLE , KACINVEN , KACDESC , KACDESI , KACOBSV , KACTRE , KACNMC01 , KACNMC02 , KACNMC03 , KACNMC04 , KACNMC05 , KACCLASS , KACMANDH , KACMANFH , KACCM2) 
		VALUES 
		( V_TYPEOFFRE , V_CODEOFFRE , V_NEWVERS , P_CODERSQ , P_CODEOBJ , V_CIBLE , V_INVEN , V_DESCR , V_NEWDESI , V_NEWOBSV , V_TRE , V_NOMEN1 , V_NOMEN2 , V_NOMEN3 , V_NOMEN4 , V_NOMEN5 , V_CLASS , 0 , 0 , V_COUTM2) ; 
		 
	END IF ; 
END P1  ; 
  

  

