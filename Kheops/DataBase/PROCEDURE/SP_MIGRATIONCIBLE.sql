﻿CREATE PROCEDURE SP_MIGRATIONCIBLE ( 
	IN P_CODEOFFRE CHAR(9) , 
	IN P_TYPE CHAR(1) , 
	IN P_VERSION INTEGER , 
	IN P_CODECIBLE CHAR(10) , 
	IN P_CODENEWCIBLE CHAR(10) ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_MIGRATIONCIBLE 
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
  
	DECLARE V_RSQPRINCIPAL INTEGER DEFAULT 0 ; 
	DECLARE V_CATEGORIE CHAR ( 5 ) DEFAULT '' ; 
  
	 --get risque principal 
	SELECT MIN ( KABRSQ ) INTO V_RSQPRINCIPAL FROM KPRSQ 
	WHERE TRIM ( KABIPB ) = TRIM ( P_CODEOFFRE ) 
	AND KABALX = P_VERSION 
	AND KABTYP = P_TYPE ; 
	 
	 --mise à jour de la catégorie 
	SELECT KAICAT INTO V_CATEGORIE FROM KCIBLEF WHERE TRIM ( KAICIBLE ) = TRIM ( P_CODENEWCIBLE ) ; 
	UPDATE YPOBASE SET PBCAT = TRIM ( V_CATEGORIE ) WHERE TRIM ( PBIPB ) = TRIM ( P_CODEOFFRE ) AND PBALX = P_VERSION AND PBTYP = P_TYPE ;	 
	 
	 --l'offre 
	UPDATE KPENT 
	SET KAACIBLE = P_CODENEWCIBLE 
	WHERE	TRIM ( KAAIPB ) = TRIM ( P_CODEOFFRE ) 
	AND KAAALX = P_VERSION 
	AND KAATYP = P_TYPE 
	AND KAACIBLE = P_CODECIBLE ; 
  
	 --le risque principal 
	UPDATE KPRSQ 
	SET KABCIBLE = P_CODENEWCIBLE 
	WHERE	TRIM ( KABIPB ) = TRIM ( P_CODEOFFRE ) 
	AND KABALX = P_VERSION 
	AND KABTYP = P_TYPE 
	AND KABRSQ = V_RSQPRINCIPAL 
	AND KABCIBLE = P_CODECIBLE ; 
  
	 --les objets du risque principal 
	UPDATE KPOBJ 
	SET KACCIBLE = P_CODENEWCIBLE 
	WHERE	TRIM ( KACIPB ) = TRIM ( P_CODEOFFRE ) 
	AND KACALX = P_VERSION 
	AND KACTYP = P_TYPE 
	AND KACRSQ = V_RSQPRINCIPAL 
	AND KACCIBLE = P_CODECIBLE ; 
  
	 --supprimer les formules du risque principale 
	FOR CURSOR_ADR AS FREE_LIST CURSOR FOR 
  
		SELECT KDDFOR FROM KPOPTAP 
		WHERE TRIM ( KDDIPB ) = TRIM ( P_CODEOFFRE ) 
		AND KDDALX = P_VERSION 
		AND KDDTYP = P_TYPE 
  
	DO	 
		CALL SP_DELFORM ( P_CODEOFFRE , P_VERSION , P_TYPE , KDDFOR , 'C' ) ; 
	END FOR ; 
  
UPDATE YPOBASE SET PBORK = 'KHE' WHERE TRIM ( PBIPB ) = TRIM ( P_CODEOFFRE ) AND PBALX = P_VERSION AND PBTYP = P_TYPE ; 
  
END P1  ; 
  

  
