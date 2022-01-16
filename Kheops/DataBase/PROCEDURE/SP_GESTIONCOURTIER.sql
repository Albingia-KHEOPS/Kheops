﻿CREATE PROCEDURE SP_GESTIONCOURTIER ( 
	IN P_CODECONTRAT CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_NEWCOURTGEST INTEGER , 
	IN P_OLDCOURTGEST INTEGER , 
	IN P_NEWCOURTAPP INTEGER , 
	IN P_OLDCOURTAPP INTEGER , 
	IN P_NEWCOURTPAY INTEGER , 
	IN P_OLDCOURTPAY INTEGER ) 
	LANGUAGE SQL 
	SPECIFIC SP_GESTIONCOURTIER 
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
	DECLARE V_ERROR CHAR ( 128 ) DEFAULT '' ; 
	 
	/* COURTIER APPORTEUR */ 
	IF ( P_NEWCOURTAPP != P_OLDCOURTAPP ) THEN 
		DELETE FROM YPOCOUR 
			WHERE TRIM ( PFIPB ) = TRIM ( P_CODECONTRAT ) AND PFALX = P_VERSION AND PFTYP = P_TYPE 
				AND PFCTI = 'A' OR ( PFCTI = 'N' AND PFICT = P_OLDCOURTAPP ) ; 
		IF ( P_NEWCOURTAPP = P_OLDCOURTGEST ) THEN 
			DELETE FROM YPOCOUR 
				WHERE TRIM ( PFIPB ) = TRIM ( P_CODECONTRAT ) AND PFALX = P_VERSION AND PFTYP = P_TYPE 
					AND PFCTI = 'O' ; 
		END IF ; 
		CALL SP_ADDCORT ( P_CODECONTRAT , P_TYPE , P_VERSION , 'A' , P_NEWCOURTAPP , 100 , 'I' , V_ERROR ) ; 
	END IF ; 
	/* COURTIER GESTIONNAIRE */ 
	IF ( P_NEWCOURTGEST = P_OLDCOURTGEST ) THEN 
		IF ( P_OLDCOURTGEST = P_OLDCOURTAPP AND P_NEWCOURTGEST != P_NEWCOURTAPP ) THEN 
			CALL SP_ADDCORT ( P_CODECONTRAT , P_TYPE , P_VERSION , 'O' , P_NEWCOURTGEST , 0 , 'I' , V_ERROR ) ; 
		END IF ; 
		IF ( P_OLDCOURTGEST != P_OLDCOURTAPP AND P_NEWCOURTGEST = P_NEWCOURTAPP ) THEN 
			DELETE FROM YPOCOUR 
				WHERE TRIM ( PFIPB ) = TRIM ( P_CODECONTRAT ) AND PFALX = P_VERSION AND PFTYP = P_TYPE AND PFCTI = 'O' ; 
		END IF ; 
	ELSE 
		IF ( P_OLDCOURTGEST = P_OLDCOURTAPP AND P_NEWCOURTGEST != P_NEWCOURTAPP ) THEN 
			DELETE FROM YPOCOUR 
				WHERE TRIM ( PFIPB ) = TRIM ( P_CODECONTRAT ) AND PFALX = P_VERSION AND PFTYP = P_TYPE 
					AND PFCTI = 'O' OR ( PFCTI = 'N' AND PFICT = P_OLDCOURTGEST ) ; 
			CALL SP_ADDCORT ( P_CODECONTRAT , P_TYPE , P_VERSION , 'O' , P_NEWCOURTGEST , 0 , 'I' , V_ERROR ) ; 
		END IF ; 
		IF ( P_OLDCOURTGEST != P_OLDCOURTAPP AND P_NEWCOURTGEST != P_NEWCOURTAPP ) THEN 
			DELETE FROM YPOCOUR 
				WHERE TRIM ( PFIPB ) = TRIM ( P_CODECONTRAT ) AND PFALX = P_VERSION AND PFTYP = P_TYPE 
					AND PFCTI = 'O' OR ( PFCTI = 'N' AND PFICT = P_OLDCOURTGEST ) ; 
			CALL SP_ADDCORT ( P_CODECONTRAT , P_TYPE , P_VERSION , 'O' , P_NEWCOURTGEST , 0 , 'I' , V_ERROR ) ; 
		END IF ; 
		IF ( P_OLDCOURTGEST != P_OLDCOURTAPP AND P_NEWCOURTGEST = P_NEWCOURTAPP ) THEN 
			DELETE FROM YPOCOUR 
				WHERE TRIM ( PFIPB ) = TRIM ( P_CODECONTRAT ) AND PFALX = P_VERSION AND PFTYP = P_TYPE 
					AND PFCTI = 'O' OR ( PFCTI = 'N' AND PFICT = P_OLDCOURTGEST ) ; 
		END IF ; 
	END IF ; 
	/* COURTIER PAYEUR */ 
	IF ( P_NEWCOURTPAY = P_OLDCOURTPAY ) THEN 
		IF ( ( P_OLDCOURTAPP = P_OLDCOURTPAY OR P_OLDCOURTGEST = P_OLDCOURTPAY ) AND P_NEWCOURTAPP != P_NEWCOURTPAY AND P_NEWCOURTGEST != P_NEWCOURTPAY ) THEN 
			CALL SP_ADDCORT ( P_CODECONTRAT , P_TYPE , P_VERSION , 'N' , P_NEWCOURTPAY , 0 , 'I' , V_ERROR ) ; 
		END IF ; 
		IF ( ( P_OLDCOURTAPP != P_OLDCOURTPAY AND P_OLDCOURTGEST != P_OLDCOURTPAY ) AND ( P_NEWCOURTAPP = P_NEWCOURTPAY OR P_NEWCOURTGEST = P_NEWCOURTPAY ) ) THEN 
			DELETE FROM YPOCOUR 
				WHERE TRIM ( PFIPB ) = TRIM ( P_CODECONTRAT ) AND PFALX = P_VERSION AND PFTYP = P_TYPE 
					AND PFCTI = 'N' AND PFICT = P_OLDCOURTPAY ;		 
		END IF ; 
	ELSE 
		IF ( ( P_OLDCOURTAPP = P_OLDCOURTPAY OR P_OLDCOURTGEST = P_OLDCOURTPAY ) AND P_NEWCOURTAPP != P_NEWCOURTPAY AND P_NEWCOURTGEST != P_NEWCOURTPAY ) THEN 
			CALL SP_ADDCORT ( P_CODECONTRAT , P_TYPE , P_VERSION , 'N' , P_NEWCOURTPAY , 0 , 'I' , V_ERROR ) ; 
		END IF ; 
		IF ( ( P_OLDCOURTAPP != P_OLDCOURTPAY AND P_OLDCOURTGEST != P_OLDCOURTPAY ) AND ( P_NEWCOURTAPP = P_NEWCOURTPAY OR P_NEWCOURTGEST = P_NEWCOURTPAY ) ) THEN 
			DELETE FROM YPOCOUR 
				WHERE TRIM ( PFIPB ) = TRIM ( P_CODECONTRAT ) AND PFALX = P_VERSION AND PFTYP = P_TYPE 
					AND PFCTI = 'N' AND PFICT = P_OLDCOURTPAY ;		 
		END IF ; 
		IF ( ( P_OLDCOURTAPP != P_OLDCOURTPAY AND P_OLDCOURTGEST != P_OLDCOURTPAY ) AND P_NEWCOURTAPP != P_NEWCOURTPAY AND P_NEWCOURTGEST != P_NEWCOURTPAY ) THEN 
			DELETE FROM YPOCOUR 
				WHERE TRIM ( PFIPB ) = TRIM ( P_CODECONTRAT ) AND PFALX = P_VERSION AND PFTYP = P_TYPE 
					AND PFCTI = 'N' AND PFICT = P_OLDCOURTPAY ;		 
			CALL SP_ADDCORT ( P_CODECONTRAT , P_TYPE , P_VERSION , 'N' , P_NEWCOURTPAY , 0 , 'I' , V_ERROR ) ; 
		END IF ; 
	END IF ; 
  
  
END P1  ; 
  

  
