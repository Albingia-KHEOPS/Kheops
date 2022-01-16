﻿CREATE PROCEDURE SP_GETFRAISACC ( 
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_CODEAVN INTEGER , 
	IN P_ANNEE INTEGER , 
	IN P_MONTANT DECIMAL(7, 0) , 
	OUT P_FRAIS NUMERIC(7, 0) ) 
	LANGUAGE SQL 
	SPECIFIC SP_GETFRAISACC 
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
  
	DECLARE V_BRANCHE CHAR ( 2 ) DEFAULT '' ; 
	DECLARE V_SBRANCHE CHAR ( 3 ) DEFAULT '' ; 
	DECLARE V_CATEGORIE CHAR ( 50 ) DEFAULT '' ; 
	 
	DECLARE V_COUNT INTEGER DEFAULT 0 ; 
	 
	DECLARE V_MONTANT DECIMAL ( 7 , 0 ) DEFAULT 0 ; 
	DECLARE V_FRAISACCMIN NUMERIC ( 7 , 0 ) DEFAULT 0 ; 
	DECLARE V_FRAISACCMAX NUMERIC ( 7 , 0 ) DEFAULT 0 ; 
	 
	DECLARE V_ANNEEREF INTEGER DEFAULT 0 ; 
	 
	SET P_FRAIS = 0 ; 
	 
	/* RÉCUPÉRATION DE LA BRANCHE, SOUS-BRANCHE ET CATÉGORIE     
	DE L'OFFRE OU CONTRAT PASSÉ EN PARAMÈTRE */ 
	SELECT PBBRA , PBSBR , PBCAT INTO V_BRANCHE , V_SBRANCHE , V_CATEGORIE 
		FROM YPOBASE 
		WHERE TRIM ( PBIPB ) = TRIM ( P_CODEOFFRE ) AND PBALX = P_VERSION AND PBTYP = P_TYPE AND PBAVN = P_CODEAVN ; 
  
	/* VÉRIFICATION QUE L'ASSOCIATION BRANCHE + SOUS-BRANCHE + CATEGORIE EXISTE BIEN */ 
	SELECT COUNT ( * ) INTO V_COUNT 
		FROM YCATEGO 
		WHERE TRIM ( CABRA ) = TRIM ( V_BRANCHE ) AND TRIM ( CASBR ) = TRIM ( V_SBRANCHE ) AND TRIM ( CACAT ) = TRIM ( V_CATEGORIE ) ; 
		 
	IF ( V_COUNT > 0 ) THEN 
		SET V_MONTANT = 0 ; 
		SET V_FRAISACCMIN = 0 ; 
		SET V_FRAISACCMAX = 0 ; 
		 
		/* RÉCUPÉRATION DE L'ANNÉE RÉFÉRENCE EN FONCTION DE L'ANNÉE PASSÉE EN PARAMÈTRE */ 
		SELECT IFNULL ( F_GETANNEEREF ( P_ANNEE ) , 0 ) INTO V_ANNEEREF FROM SYSIBM . SYSDUMMY1 ; 
		 
		/* RÉCUPÉRATION DU MONTANT, FRAIS ACC. MIN. ET FRAIS ACC. MAX.     
		POUR L'ASSOCIATION BRANCHE + SOUS-BRANCHE + CATEGORIE */ 
		SELECT IFNULL ( MONTANT , 0 ) , IFNULL ( FRAISACCMIN , 0 ) , IFNULL ( FRAISACCMAX , 0 ) 
			INTO V_MONTANT , V_FRAISACCMIN , V_FRAISACCMAX 
		FROM KPFRAISACC 
			WHERE TRIM ( BRANCHE ) = TRIM ( V_BRANCHE ) AND TRIM ( SBRANCHE ) = TRIM ( V_SBRANCHE ) AND ANNEE = V_ANNEEREF 
				AND ( LOCATE ( ';' CONCAT TRIM ( V_CATEGORIE ) CONCAT ';' , ';' CONCAT TRIM ( CATEGORIE ) CONCAT ';' ) > 0 ) 
			ORDER BY ANNEE DESC 
			FETCH FIRST 1 ROWS ONLY ;	 
		 
		IF ( P_MONTANT >= V_MONTANT AND V_ANNEEREF > 0 ) THEN 
			/* SI LE MONTANT EN PARAMÈTRE >= AU MONTANT MAX ET L'ANNEE EXISTE DANS LA TABLE DES ANNÉES DE RÉFÉRENCE,     
			ON RENVOIT LES FRAIS ACCESSOIRES MAX */ 
			SET P_FRAIS = V_FRAISACCMAX ; 
		ELSE 
			/* SI LE MONTANT EN PARAMÈTRE < AU MONTANT MAX OU L'ANNEE N'EXISTE PAS DANS LA TABLE DES ANNÉES DE RÉFÉRENCE,    
			ON RENVOIT LES FRAIS ACCESSOIRES MIN*/ 
			SET P_FRAIS = V_FRAISACCMIN ; 
		END IF ; 
	ELSE 
		/* SI L'ASSOCIATION N'EXISTE PAS */ 
		SET P_FRAIS = - 1 ; 
	END IF ; 
		 
END P1  ; 
  

  
