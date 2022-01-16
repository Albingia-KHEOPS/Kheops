﻿CREATE PROCEDURE SP_UPDECH ( 
	IN P_ID_OFFRE CHAR(9) , 
	IN P_TYPE_OFFRE CHAR(1) , 
	IN P_VERSION_OFFRE NUMERIC(5, 0) , 
	IN P_ANNEE_ECH NUMERIC(4, 0) , 
	IN P_MOIS_ECH NUMERIC(2, 0) , 
	IN P_JOUR_ECH NUMERIC(2, 0) , 
	IN P_PRIME_POURCENT DECIMAL(3, 0) , 
	IN P_MONTANT_ECH DECIMAL(11, 2) , 
	IN P_MONTANT_CALC DECIMAL(11, 2) , 
	IN P_FRAIS_ACCESSOIRS DECIMAL(7, 0) , 
	IN P_ATTENTAT CHAR(1) , 
	IN P_MODE CHAR(1) , 
	IN P_TYPE_ECH NUMERIC(1, 0) , 
	OUT P_ERREUR CHAR(128) ) 
	LANGUAGE SQL 
	SPECIFIC SP_UPDECH 
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
DECLARE V_ECH_COUNT INTEGER DEFAULT 0 ; 
DECLARE V_COMPTANT_COUNT INTEGER DEFAULT 0 ; 
  
  
IF ( P_MODE = 'U' ) THEN 
	UPDATE YPOECHE 
	SET 
	PIEHE = P_TYPE_ECH , 
	PIPCR = P_PRIME_POURCENT , 
	PIPCC = 0 , 
	PIPMR = P_MONTANT_ECH , 
	PIPMC = P_MONTANT_CALC , 
	PIAFR = P_FRAIS_ACCESSOIRS , 
	PIIPK = 0 , 
	PIATT = P_ATTENTAT 
	WHERE PITYP = P_TYPE_OFFRE AND PIIPB = P_ID_OFFRE AND PIALX = P_VERSION_OFFRE 
	AND PIEHA = P_ANNEE_ECH AND PIEHM = P_MOIS_ECH AND PIEHJ = P_JOUR_ECH ; 
END IF ; 
IF ( P_MODE = 'I' ) THEN 
  
	SELECT COUNT ( * ) INTO V_ECH_COUNT FROM YPOECHE 
	WHERE PITYP = P_TYPE_OFFRE AND PIIPB = P_ID_OFFRE AND PIALX = P_VERSION_OFFRE 
	AND PIEHA = P_ANNEE_ECH AND PIEHM = P_MOIS_ECH AND PIEHJ = P_JOUR_ECH ; 
	 
	IF ( V_ECH_COUNT != 0 ) THEN 
		SET P_ERREUR = 'Une autre écheance existe pour cette date' ; 
	ELSE 
		INSERT INTO YPOECHE ( PITYP , PIIPB , PIALX , PIEHA , PIEHM , PIEHJ , PIEHE , PIPCR , PIPCC , PIPMR , 
		PIPMC , PIAFR , PIIPK , PIATT ) 
		VALUES ( P_TYPE_OFFRE , P_ID_OFFRE , P_VERSION_OFFRE , P_ANNEE_ECH , P_MOIS_ECH , P_JOUR_ECH , 
		P_TYPE_ECH , P_PRIME_POURCENT , 0 , P_MONTANT_ECH , P_MONTANT_CALC , P_FRAIS_ACCESSOIRS , 0 , P_ATTENTAT ) ; 
	END IF ; 
END IF ; 
IF ( P_MODE = 'C' ) THEN 
SELECT COUNT ( * ) INTO V_COMPTANT_COUNT FROM YPOECHE 
WHERE PITYP = P_TYPE_OFFRE AND PIIPB = P_ID_OFFRE AND PIALX = P_VERSION_OFFRE 
AND PIEHE = 1 ; 
	IF ( V_COMPTANT_COUNT != 0 ) THEN 
		UPDATE YPOECHE 
		SET 
		PIEHE = P_TYPE_ECH , 
		PIPCR = P_PRIME_POURCENT , 
		PIPCC = 0 , 
		PIPMR = P_MONTANT_ECH , 
		PIPMC = P_MONTANT_CALC , 
		PIAFR = P_FRAIS_ACCESSOIRS , 
		PIIPK = 0 , 
		PIATT = P_ATTENTAT 
		WHERE PITYP = P_TYPE_OFFRE AND PIIPB = P_ID_OFFRE AND PIALX = P_VERSION_OFFRE 
		AND PIEHE = 1 ; 
	ELSE 
		INSERT INTO YPOECHE ( PITYP , PIIPB , PIALX , PIEHA , PIEHM , PIEHJ , PIEHE , PIPCR , PIPCC , PIPMR , 
		PIPMC , PIAFR , PIIPK , PIATT ) 
		VALUES ( P_TYPE_OFFRE , P_ID_OFFRE , P_VERSION_OFFRE , P_ANNEE_ECH , P_MOIS_ECH , P_JOUR_ECH , 
		1 , P_PRIME_POURCENT , 0 , P_MONTANT_ECH , P_MONTANT_CALC , P_FRAIS_ACCESSOIRS , 0 , P_ATTENTAT ) ; 
	END IF ; 
END IF ; 
  
UPDATE YPOBASE 
	SET PBPER = 'E' 
WHERE TRIM ( PBIPB ) = TRIM ( P_ID_OFFRE ) 
AND PBALX = P_VERSION_OFFRE 
AND PBTYP = P_TYPE_OFFRE ; 
  
END P1  ; 
  

  

