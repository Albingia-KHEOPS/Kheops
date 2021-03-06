CREATE OR REPLACE PROCEDURE SP_SAVCONT_ADR ( 
	IN P_CODECONTRAT CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) ,  
	IN P_ADRCHR INTEGER , 
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
	IN P_ISADDRESSEMPTY INTEGER,
	IN P_ADRLATITUDE NUMERIC(10, 7) , 
	IN P_ADRLONGITUDE NUMERIC(10, 7) 
	
	) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_SAVCONT_ADR 
	NOT DETERMINISTIC 
	MODIFIES SQL DATA 
	CALLED ON NULL INPUT 
	SET OPTION  ALWBLK = *ALLREAD , 
	ALWCPYDTA = *OPTIMIZE , 
	COMMIT = *CHG , 
	DBGVIEW = *SOURCE , 
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
				 
		DECLARE V_NUMVOIE INTEGER DEFAULT 0 ; 
		DECLARE V_CP CHAR ( 4 ) DEFAULT '' ; 
		DECLARE V_DEP CHAR ( 2 ) DEFAULT '' ; 
		DECLARE V_VOIE CHAR ( 32 ) DEFAULT '' ; 
		DECLARE V_VILLE CHAR ( 32 ) DEFAULT '' ; 
		DECLARE V_CPINT INTEGER DEFAULT 0 ; 
		DECLARE V_CPXINT INTEGER DEFAULT 0 ; 
		 
		DECLARE V_COURTDIRECT CHAR ( 1 ) DEFAULT '' ; 
		DECLARE V_TYPEENC CHAR ( 1 ) DEFAULT '' ; 
		DECLARE V_CODEENC CHAR ( 1 ) DEFAULT '' ; 
		DECLARE V_NEWCODEENC CHAR ( 1 ) DEFAULT '' ; 
		DECLARE V_PBORK CHAR ( 3 ) DEFAULT '' ; 
		 
		DECLARE V_DESIASSU INTEGER DEFAULT 0 ; 
		 
		DECLARE V_NBRSQ INTEGER DEFAULT 0 ; 
		DECLARE V_RSQADH INTEGER DEFAULT 0 ; 
		DECLARE V_CODERSQ INTEGER DEFAULT 0 ; 
	  
		DECLARE V_NBOBJ INTEGER DEFAULT 0 ; 
		DECLARE V_OBJADH INTEGER DEFAULT 0 ; 
		DECLARE V_CODEOBJ INTEGER DEFAULT 0 ; 
	  
		DECLARE V_NUMCHR CHAR ( 40 ) DEFAULT '' ; 
		DECLARE V_CHRONO_AFFECTATION INTEGER DEFAULT 0 ;
	
	
		
		SET V_NUMVOIE = CAST ( ('0' CONCAT TRIM(P_NUMVOIE)) AS INTEGER ) ; 
		SET V_VOIE = TRIM ( TRIM ( TRIM ( P_NUMVOIE ) CONCAT ' ' CONCAT TRIM ( P_EXTVOIE ) ) CONCAT ' ' CONCAT TRIM ( P_VOIE ) ) ; 
		SET V_DEP = P_DEP ;
		SET V_VILLE = P_DEP CONCAT P_CP CONCAT ' ' CONCAT P_VILLE ; 
	
		IF ( P_VOIE <> '' OR P_CP <> '' OR P_VILLE <> '' OR P_NUMVOIE <> '' OR P_EXTVOIE <> '' OR P_BATIMENT <> '' OR P_BP <> '' OR P_CPX <> '' ) THEN 
		 
			IF ( LENGTH ( TRIM ( P_CP ) ) = 5 ) THEN 
				SET V_CP = SUBSTR ( P_CP , 3 , 3 ) ; 
				SET V_DEP = SUBSTR ( P_CP , 1 , 2 ) ; 
				SET V_CPINT = CAST ( V_CP AS INTEGER ) ; 
			ELSE 
				SET V_CPINT = CAST ( ('0' CONCAT TRIM(P_CP)) AS INTEGER ) ; 
				SET V_CP = TRIM ( CAST ( V_CPINT AS CHAR (4) ) ) ; 
			END IF ;  
			 			 
			IF ( LENGTH ( TRIM ( P_CPX ) ) = 5 ) THEN 
				SET V_CPXINT = CAST ( SUBSTR ( P_CPX , 3 , 3 ) AS INTEGER ) ; 
				IF ( V_DEP = '' ) THEN 
					SET V_DEP = SUBSTR ( P_CPX , 1 , 2 ) ; 
				END IF ; 
			ELSE 
				SET V_CPXINT = CAST ( ('0' CONCAT TRIM(P_CPX)) AS INTEGER ) ; 
			END IF ; 			
			
			IF ( P_INSADR = 'O' AND P_ISADDRESSEMPTY = 0) THEN
				SET V_ACTION_YADRESS = 'CREATE';
				CALL SP_YCHRONO ( 'ADRESSE_CHRONO' , V_NUMCHR ) ; 
				SET P_ADRCHR = CAST ( V_NUMCHR AS INTEGER ) ; 
			END IF;
			
			IF ( P_ISADDRESSEMPTY != 0 ) THEN 
				SET V_ACTION_YADRESS = 'DELETE';
			END IF ;
		ELSE	 
			SET V_ACTION_YADRESS = 'DELETE';
		END IF ;	 
	 
		IF (TRIM(V_ACTION_YADRESS) != 'DELETE') THEN
			SET V_CHRONO_AFFECTATION = P_ADRCHR; 
		END IF;
	  
		IF ( V_CP = '0' ) THEN 
				SET V_CP = '' ; 
		END IF ; 
	 
		CALL SP_UPDATE_ADR ( V_ACTION_YADRESS, P_NUMVOIE, P_NUMVOIE2, P_CP, P_CPX, P_ADRCHR, V_CPINT, V_CPXINT, 
				P_BATIMENT, P_EXTVOIE, P_VOIE, P_BP, P_CP, V_DEP, P_VILLE, V_VOIE, V_VILLE, 
				P_VILLEX, P_MATHEX, 0, '', P_CODECONTRAT, P_VERSION, P_TYPE , 0, 0, 0, 'UPDATE_SET', V_CHRONO_AFFECTATION, P_DEP, V_CP, P_ADRLATITUDE, P_ADRLONGITUDE);
		
		SET V_ACTION_YPRTADR = '';
		SET V_ACTION_YADRESS = '';
	/* ----------------------------------------------- */ 
	/* MODIFICATION DE LA SAUVEGARDE DE L'ADRESSE V4.1 */ 
	/* ----------------------------------------------- */ 
		SELECT COUNT ( * ) INTO V_NBRSQ FROM YPRTRSQ WHERE JEIPB = P_CODECONTRAT AND JEALX = P_VERSION ; 
		IF ( V_NBRSQ = 1 ) THEN /* SI MONO-RISQUE */ 
			SET V_CODERSQ = 0 ; 
			SET V_RSQADH = 0 ; 
			 
			SELECT JERSQ , IFNULL ( JFADH , 0 ) INTO V_CODERSQ , V_RSQADH 
			FROM YPRTRSQ 
				LEFT JOIN YPRTADR ON JFIPB = JEIPB AND JFALX = JEALX AND JFRSQ = JERSQ AND JFOBJ = 0 
			WHERE JEIPB = P_CODECONTRAT AND JEALX = P_VERSION ; 
			 
			IF ( V_RSQADH > 0 ) THEN 
				IF ( P_ISADDRESSEMPTY = 0 ) THEN 
					SET V_ACTION_YPRTADR = 'UPDATE_WHERE';
					SET V_ACTION_YADRESS = 'UPDATE';
				ELSE 
					SET V_ACTION_YPRTADR = 'DELETE';
					SET V_ACTION_YADRESS = 'DELETE';
				END IF ; 
			ELSE 
				IF ( P_ISADDRESSEMPTY = 0 ) THEN 
					CALL SP_YCHRONO ( 'ADRESSE_CHRONO' , V_NUMCHR ) ; 
					SET V_RSQADH = CAST ( V_NUMCHR AS INTEGER ) ;				 
					 
					SET V_ACTION_YPRTADR = 'CREATE';
					SET V_ACTION_YADRESS = 'CREATE'; 
				END IF ; 
			END IF ; 
			
			
			CALL SP_UPDATE_ADR ( V_ACTION_YADRESS, P_NUMVOIE, P_NUMVOIE2, P_CP, P_CPX, V_RSQADH, V_CPINT, V_CPXINT, 
				P_BATIMENT, P_EXTVOIE, P_VOIE, P_BP, P_CP, V_DEP, P_VILLE, V_VOIE, V_VILLE, 
				P_VILLEX, P_MATHEX, 0, V_ACTION_YPRTADR, P_CODECONTRAT, P_VERSION, P_TYPE, V_CODERSQ, 0, V_RSQADH, '', 0, '', '', P_ADRLATITUDE, P_ADRLONGITUDE);
			
			
			SET V_ACTION_YPRTADR = '';
			SET V_ACTION_YADRESS = '';
			
			SELECT COUNT ( * ) INTO V_NBOBJ FROM YPRTOBJ WHERE JGIPB = P_CODECONTRAT AND JGALX = P_VERSION ; 
			IF ( V_NBOBJ = 1 ) THEN /* SI MONO-OBJET */ 
				SET V_CODEOBJ = 0 ; 
				SET V_OBJADH = 0 ; 
				 
				SELECT JGOBJ , IFNULL ( JFADH , 0 ) INTO V_CODEOBJ , V_OBJADH 
				FROM YPRTOBJ 
					LEFT JOIN YPRTADR ON JFIPB = JGIPB AND JFALX = JGALX AND JFRSQ = JGRSQ AND JFOBJ = JGOBJ 
				WHERE JGIPB = P_CODECONTRAT AND JGALX = P_VERSION AND JGRSQ = V_CODERSQ ; 
				 
				IF ( V_OBJADH > 0 ) THEN 
					IF ( P_ISADDRESSEMPTY = 0 ) THEN 
						SET V_ACTION_YPRTADR = 'UPDATE_WHERE';
						SET V_ACTION_YADRESS = 'UPDATE';
					ELSE 
						SET V_ACTION_YPRTADR = 'DELETE' ; 
						SET V_ACTION_YADRESS = 'DELETE' ; 
					END IF ; 
				ELSE 
					IF ( P_ISADDRESSEMPTY = 0 ) THEN 
						CALL SP_YCHRONO ( 'ADRESSE_CHRONO' , V_NUMCHR ) ; 
						SET V_OBJADH = CAST ( V_NUMCHR AS INTEGER ) ;				 
						 
						SET V_ACTION_YPRTADR = 'CREATE';
						SET V_ACTION_YADRESS = 'CREATE'; 
					END IF ; 
				END IF ; 
			END IF ; 
			
			CALL SP_UPDATE_ADR ( V_ACTION_YADRESS, P_NUMVOIE, P_NUMVOIE2, P_CP, P_CPX, V_OBJADH, V_CPINT, V_CPXINT, 
				P_BATIMENT, P_EXTVOIE, P_VOIE, P_BP, P_CP, V_DEP, P_VILLE, V_VOIE, V_VILLE, 
				P_VILLEX, P_MATHEX, 0, V_ACTION_YPRTADR, P_CODECONTRAT, P_VERSION, P_TYPE, V_CODERSQ, V_CODEOBJ, V_OBJADH, '', 0, '', '', P_ADRLATITUDE, P_ADRLONGITUDE);
		END IF ; 
		 
	
	/* ------------------------------------------------------ */ 
	/* FIN DE MODIFICATION DE LA SAUVEGARDE DE L'ADRESSE V4.1 */ 
	/* ------------------------------------------------------ */ 
	
	END P1;
