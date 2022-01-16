﻿CREATE OR REPLACE PROCEDURE SP_SAVEOBJ_ADR ( 
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_CODERSQ INTEGER , 
	IN P_COUNTRSQ INTEGER ,
	IN P_CODEOBJ INTEGER ,
	IN P_COUNTOBJ INTEGER ,
	IN P_INSADR CHAR(1) , 
	IN P_BATIMENT CHAR(32) , 
	IN P_NUMVOIE CHAR(10) , 
	IN P_NUMVOIE2 CHAR(15) , 
	IN P_EXTVOIE CHAR(1) , 
	IN P_VOIE CHAR(32) , 
	IN P_BP CHAR(32) , 
	IN P_CP CHAR(5) , 
	IN P_DEP CHAR(2) , 
	IN P_VILLE CHAR(26) , 
	IN P_CPX CHAR(5) , 
	IN P_VILLEX CHAR(32) , 
	IN P_MATHEX INTEGER , 
	IN P_ADRNUMCHRONO INTEGER ,
	IN P_ISADDRESSEMPTY INTEGER,
	IN P_LATITUDE NUMERIC(10, 7) , 
	IN P_LONGITUDE NUMERIC(10, 7) ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_SAVEOBJ_ADR
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
	
	DECLARE V_ACTION_YADRESS CHAR (6) DEFAULT 'UPDATE';
	DECLARE V_ACTION_YPRTADR CHAR (12) DEFAULT '';
	DECLARE V_ACTION_YPOBASE CHAR (12) DEFAULT '';
	
	DECLARE V_CP INTEGER DEFAULT 0 ;
	DECLARE V_VOIE CHAR ( 32 ) DEFAULT '' ;
	DECLARE V_VILLE CHAR ( 32 ) DEFAULT '' ;
	

	DECLARE V_COUNTADR INTEGER DEFAULT 0 ;
	
	DECLARE V_CHRONORSQ INTEGER DEFAULT - 1 ;
	DECLARE V_CHRONOOFFRE INTEGER DEFAULT 0 ;
	DECLARE V_CHRONO_AFFECTATION INTEGER DEFAULT 0 ;
	DECLARE V_CHRONO_YPOBASE INTEGER DEFAULT 0 ;
	
	DECLARE V_INTADRCP INTEGER DEFAULT 0 ;
	DECLARE V_INTADRCPX INTEGER DEFAULT 0 ;

	DECLARE V_NUMRSQ CHAR ( 40 ) DEFAULT '' ;
	DECLARE V_NUMOBJ CHAR ( 40 ) DEFAULT '' ;
	DECLARE V_NUMCHR CHAR ( 40 ) DEFAULT '' ;
	DECLARE V_CHRRSQ INTEGER DEFAULT 0 ;
	DECLARE V_CHROBJ INTEGER DEFAULT 0 ;
	
	DECLARE V_NUMCHRONO INTEGER DEFAULT 0 ;
	DECLARE V_NUMVOIE INTEGER DEFAULT 0 ;
	
	SET V_NUMVOIE = CAST ( ('0' CONCAT TRIM(P_NUMVOIE)) AS INTEGER ) ;	
	SET V_VOIE = TRIM ( TRIM ( TRIM ( P_NUMVOIE ) CONCAT ' ' CONCAT TRIM ( P_EXTVOIE ) ) CONCAT ' ' CONCAT TRIM ( P_VOIE ) ) ;
	SET V_VILLE = TRIM ( P_DEP ) CONCAT P_CP CONCAT ' ' CONCAT TRIM ( P_VILLE ) ;
	SET V_CP = CAST ( ('0' CONCAT TRIM(P_CP)) AS INTEGER ) ;
	SET V_INTADRCP = V_CP ;
	SET V_INTADRCPX = CAST ( ('0' CONCAT TRIM(P_CPX)) AS INTEGER ) ;
	
	
	IF ( P_ADRNUMCHRONO = 0 ) THEN
		CALL SP_YCHRONO ( 'ADRESSE_CHRONO' , V_NUMCHR ) ;
		SET P_ADRNUMCHRONO = CAST ( V_NUMCHR AS INTEGER ) ;
	END IF ;
	
	IF ( P_INSADR = 'O' AND P_ISADDRESSEMPTY = 0 ) THEN
		SET V_ACTION_YADRESS = 'CREATE';
	END IF ;

	CALL SP_UPDATE_ADR ( V_ACTION_YADRESS, P_NUMVOIE , P_NUMVOIE2 , P_CP, P_CPX, P_ADRNUMCHRONO, V_INTADRCP, V_INTADRCPX, 
				P_BATIMENT, P_EXTVOIE, P_VOIE, P_BP, P_CP, P_DEP, P_VILLE, V_VOIE, V_VILLE, 
				P_VILLEX, P_MATHEX, 1, '', P_CODEOFFRE, P_VERSION, P_TYPE,P_CODERSQ, 0, P_ADRNUMCHRONO, '', 0, '', '', 	P_LATITUDE,P_LONGITUDE );
	
	

	SELECT COUNT ( * ) INTO V_COUNTADR
	FROM YPRTADR
	WHERE -- ( ( JFADH = P_ADRNUMCHRONO AND P_ADRNUMCHRONO > 0 ) OR ( JFADH = 0 ) ) AND
	 JFIPB = P_CODEOFFRE AND JFALX = P_VERSION AND JFRSQ = P_CODERSQ AND JFOBJ = P_CODEOBJ ;	
	
	IF ( P_ISADDRESSEMPTY = 0 ) THEN
		SET V_NUMCHRONO = P_ADRNUMCHRONO ;
	END IF ;
	
	SET V_ACTION_YPRTADR = '';
	SET V_ACTION_YADRESS = '';
	
	IF ( V_COUNTADR > 0 ) THEN
		SET V_ACTION_YPRTADR = 'UPDATE_SET';	
		IF ( P_ISADDRESSEMPTY = 1 ) THEN
			SET V_ACTION_YADRESS = 'DELETE';
		END IF ;
	ELSE
		IF ( P_ISADDRESSEMPTY = 0 ) THEN
			SET V_ACTION_YPRTADR = 'CREATE';
		END IF ;
	END IF ;
	
	CALL SP_UPDATE_ADR ( V_ACTION_YADRESS, P_NUMVOIE , P_NUMVOIE2 , P_CP, P_CPX, P_ADRNUMCHRONO, V_INTADRCP, V_INTADRCPX, 
				P_BATIMENT, P_EXTVOIE, P_VOIE, P_BP, P_CP, P_DEP, P_VILLE, V_VOIE, V_VILLE, 
				P_VILLEX, P_MATHEX, 1, V_ACTION_YPRTADR, P_CODEOFFRE, P_VERSION, P_TYPE, P_CODERSQ, P_CODEOBJ, V_NUMCHRONO, '', 0, '', '', 	P_LATITUDE,P_LONGITUDE);
	
	SET V_ACTION_YPRTADR = '';
	SET V_ACTION_YADRESS = '';
	
	IF ( P_COUNTOBJ = 1 ) THEN
		-- SAUVEGARDE DE L'ADRESSE DANS LE RISQUE 
		SELECT JFADH INTO V_CHRONORSQ FROM YPRTADR WHERE JFIPB = P_CODEOFFRE AND JFALX = P_VERSION AND JFRSQ = P_CODERSQ AND JFOBJ = 0 ;
		IF ( V_CHRONORSQ = - 1 OR V_CHRONORSQ = 0 ) THEN -- pas d'adresse de risque existante
			IF ( P_ISADDRESSEMPTY = 0 ) THEN -- adresse non vide
				CALL SP_YCHRONO ( 'ADRESSE_CHRONO' , V_NUMRSQ ) ;
				SET V_CHRRSQ = CAST ( V_NUMRSQ AS INTEGER ) ;
				IF ( V_CHRRSQ != 0 ) THEN
					SET V_ACTION_YPRTADR = 'CREATE';
					SET V_ACTION_YADRESS = 'CREATE';
					SET V_CHRONO_AFFECTATION = V_CHRRSQ;
				END IF ;
			END IF ;
		ELSE
			SET V_CHRONO_AFFECTATION = V_CHRONORSQ;
			IF ( P_ISADDRESSEMPTY = 0 ) THEN
				SET V_ACTION_YPRTADR = 'UPDATE_WHERE';
				SET V_ACTION_YADRESS = 'UPDATE';
			ELSE
				SET V_ACTION_YPRTADR = 'DELETE';
				SET V_ACTION_YADRESS = 'DELETE';
			END IF ;
		END IF ;
		
		CALL SP_UPDATE_ADR ( V_ACTION_YADRESS, P_NUMVOIE , P_NUMVOIE2 , P_CP, P_CPX, V_CHRONO_AFFECTATION, V_INTADRCP, V_INTADRCPX, 
				P_BATIMENT, P_EXTVOIE, P_VOIE, P_BP, P_CP, P_DEP, P_VILLE, V_VOIE, V_VILLE, 
				P_VILLEX, P_MATHEX, 1, V_ACTION_YPRTADR, P_CODEOFFRE, P_VERSION, P_TYPE, P_CODERSQ, 0, V_CHRONO_AFFECTATION, '', 0, '', '', 	P_LATITUDE,P_LONGITUDE);
				
		SET V_ACTION_YPRTADR = '';
		SET V_ACTION_YADRESS = '';
		SET V_CHRONO_AFFECTATION = 0;
		
		/*           -- SI MONO-RISQUE => RÃ‰PERCUSSION DE L'ADRESSE DANS L'OFFRE --    */
		IF ( P_COUNTRSQ = 1 ) THEN
			-- SAUVEGARDE DE L'ADRESSE DANS L'OFFRE 
			SELECT PBADH INTO V_CHRONOOFFRE FROM YPOBASE WHERE PBIPB = P_CODEOFFRE AND PBALX = P_VERSION AND PBTYP = P_TYPE ;
			IF ( V_CHRONOOFFRE = 0 ) THEN
				IF ( P_ISADDRESSEMPTY = 0 ) THEN
					CALL SP_YCHRONO ( 'ADRESSE_CHRONO' , V_NUMOBJ ) ;
					SET V_CHROBJ = CAST ( V_NUMOBJ AS INTEGER ) ;
					IF ( V_CHROBJ != 0 ) THEN
						SET V_ACTION_YPOBASE = 'UPDATE_SET';
						SET V_ACTION_YADRESS = 'CREATE';
						SET V_CHRONO_AFFECTATION = V_CHROBJ;
						SET V_CHRONO_YPOBASE = V_CHROBJ;
					END IF ;
				END IF ;
			ELSE
				SET V_CHRONO_AFFECTATION = V_CHRONOOFFRE;
				IF ( P_ISADDRESSEMPTY = 0 ) THEN
					SET V_ACTION_YPOBASE = 'UPDATE_WHERE';
					SET V_ACTION_YADRESS = 'UPDATE';
					SET V_CHRONO_YPOBASE = V_CHRONOOFFRE;					
				ELSE	
					SET V_ACTION_YADRESS = 'DELETE';
					SET V_ACTION_YPOBASE = 'UPDATE_SET';
				END IF ;
			END IF ;

			CALL SP_UPDATE_ADR ( V_ACTION_YADRESS, P_NUMVOIE , P_NUMVOIE2 , P_CP, P_CPX, V_CHRONO_AFFECTATION, V_INTADRCP, V_INTADRCPX, 
				P_BATIMENT, P_EXTVOIE, P_VOIE, P_BP, P_CP, P_DEP, P_VILLE, V_VOIE, V_VILLE, 
				P_VILLEX, P_MATHEX, 1, V_ACTION_YPRTADR, P_CODEOFFRE, P_VERSION, P_TYPE, P_CODERSQ, 0, V_CHRONO_AFFECTATION, V_ACTION_YPOBASE, V_CHRONO_YPOBASE, P_DEP, P_CP, 	P_LATITUDE,P_LONGITUDE);
			
		END IF ;
	END IF ;
	
END P1;