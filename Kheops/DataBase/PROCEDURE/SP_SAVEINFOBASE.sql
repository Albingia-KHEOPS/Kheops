CREATE PROCEDURE SP_SAVEINFOBASE ( 
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_YEARSAISIE INTEGER , 
	IN P_MONTHSAISIE INTEGER , 
	IN P_DAYSAISIE INTEGER , 
	IN P_HOURSAISIE INTEGER , 
	IN P_SOUSCRIPTEURCODE CHAR(10) , 
	IN P_GESTIONNAIRECODE CHAR(10) , 
	IN P_INTERLOCUTEUR INTEGER , 
	IN P_REFCOURTIER CHAR(25) , 
	IN P_CODEPRENEURASSU INTEGER , 
	IN P_DESCRIPTIF CHAR(40) , 
	IN P_MOTCLE1 CHAR(10) , 
	IN P_MOTCLE2 CHAR(10) , 
	IN P_MOTCLE3 CHAR(10) , 
	IN P_OBSERVATION CHAR(5000) , 
	IN P_HASADRESSE CHAR(1) , 
	IN P_ADRCHRONO INTEGER , 
	IN P_BATIMENT CHAR(32) , 
	IN P_NUMVOIE CHAR(32) , 
	IN P_NUMVOIE2 CHAR(15) , 
	IN P_EXTVOIE CHAR(1) , 
	IN P_NOMVOIE CHAR(32) , 
	IN P_BP CHAR(32) , 
	IN P_LOC CHAR(5) , 
	IN P_DEPARTEMENT CHAR(2) , 
	IN P_CP CHAR(3) , 
	IN P_VILLE CHAR(32) , 
	IN P_VOIECOMPLETE CHAR(32) , 
	IN P_VILLECOMPLETE CHAR(32) , 
	IN P_CPCDX CHAR(3) , 
	IN P_VILLECDX CHAR(32) , 
	IN P_MATRICULEHEX INTEGER , 
	IN P_PRENEURESTASSURE CHAR(1) , 
	IN P_ISADDRESSEMPTY INTEGER ) 
	LANGUAGE SQL 
	SPECIFIC SP_SAVEINFOBASE 
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
	DECLARE V_NBLIGNE INTEGER DEFAULT 0 ; 
	DECLARE V_TYPEINTERLOCUTEUR CHAR ( 1 ) DEFAULT '' ; 
	DECLARE V_FIRSTRSQ INTEGER DEFAULT 0 ; 
	DECLARE V_OBSCHRONO INTEGER DEFAULT 0 ; 
	DECLARE V_ADRRSQCHRONO INTEGER DEFAULT - 1 ; 
	DECLARE V_ADROBJCHRONO INTEGER DEFAULT - 1 ; 
	DECLARE V_AD1 CHAR ( 32 ) DEFAULT '' ; 
	DECLARE V_AD2 CHAR ( 32 ) DEFAULT '' ; 
	DECLARE V_FIRSTOBJ INTEGER DEFAULT 0 ; 
  
	DECLARE V_ADRVILLE26 CHAR ( 26 ) DEFAULT '' ; 
  
	DECLARE V_NUMRSQ CHAR ( 40 ) DEFAULT '' ; 
	DECLARE V_NUMOBJ CHAR ( 40 ) DEFAULT '' ; 
	DECLARE V_ADRCHR CHAR ( 40 ) DEFAULT '' ; 
DECLARE V_ADRCHRONO INTEGER DEFAULT 0 ; 
	DECLARE V_NUMVOIE INTEGER DEFAULT 0 ; 
	DECLARE V_CPX INTEGER DEFAULT 0 ; 
	DECLARE V_CP INTEGER DEFAULT 0 ; 
	DECLARE V_NUMCHR INTEGER DEFAULT 0 ; 
 
	IF ( P_INTERLOCUTEUR != 0 ) THEN 
		SET V_TYPEINTERLOCUTEUR = 'T' ; 
	END IF ; 
	UPDATE YPOBASE 
	SET PBREF = P_DESCRIPTIF , PBMO1 = P_MOTCLE1 , PBMO2 = P_MOTCLE2 , PBMO3 = P_MOTCLE3 , PBIAS = P_CODEPRENEURASSU , PBOCT = P_REFCOURTIER , 
	PBTIL = V_TYPEINTERLOCUTEUR , PBIN5 = P_INTERLOCUTEUR , PBSOU = P_SOUSCRIPTEURCODE , PBGES = P_GESTIONNAIRECODE , 
	PBSAA = P_YEARSAISIE , PBSAM = P_MONTHSAISIE , PBSAJ = P_DAYSAISIE , PBSAH = P_HOURSAISIE 
	WHERE TRIM ( PBIPB ) = TRIM ( P_CODEOFFRE ) AND PBALX = P_VERSION AND PBTYP = P_TYPE ; 
		 -- Si l'adresse est vide : on clear dans YPOBASE 
			 --IF(P_ISADDRESSEMPTY = 1) THEN 
	SELECT KAAOBSV INTO V_OBSCHRONO FROM KPENT 
	WHERE TRIM ( KAAIPB ) = TRIM ( P_CODEOFFRE ) AND KAATYP = P_TYPE AND KAAALX = P_VERSION ; 
  
	IF ( TRIM ( P_OBSERVATION ) != '' AND V_OBSCHRONO = 0 ) THEN 
		CALL SP_NCHRONO ( 'KAJCHR' , V_OBSCHRONO ) ; 
		INSERT INTO KPOBSV 
		( KAJIPB , KAJALX , KAJCHR , KAJTYP , KAJTYPOBS , KAJOBSV ) 
		VALUES 
		( P_CODEOFFRE , P_VERSION , V_OBSCHRONO , P_TYPE , 'GENERALE' , TRIM ( P_OBSERVATION ) ) ; 
		UPDATE KPENT 
		SET KAAOBSV = V_OBSCHRONO 
		WHERE TRIM ( KAAIPB ) = TRIM ( P_CODEOFFRE ) AND KAATYP = P_TYPE AND KAAALX = P_VERSION ; 
	ELSE 
		UPDATE KPOBSV 
		SET KAJOBSV = TRIM ( P_OBSERVATION ) 
		WHERE KAJCHR = V_OBSCHRONO ; 
	END IF ; 
  
  
	SET V_NBLIGNE = 0 ; 
	SELECT COUNT ( * ) INTO V_NBLIGNE FROM KPENT 
	WHERE TRIM ( KAAIPB ) = TRIM ( P_CODEOFFRE ) AND KAAALX = P_VERSION AND KAATYP = P_TYPE ; 
	IF ( V_NBLIGNE > 0 ) THEN 
		UPDATE KPENT 
		SET KAAASS = P_PRENEURESTASSURE 
		WHERE TRIM ( KAAIPB ) = TRIM ( P_CODEOFFRE ) AND KAAALX = P_VERSION AND KAATYP = P_TYPE ; 
	END IF ; 
	 
	 -- Instanciation des variables pour insertion sur YADRESS 
	IF ( TRIM ( P_CP ) != '' ) THEN 
		SET V_CP = CAST ( P_CP AS INTEGER ) ;	 
	END IF ; 
	IF ( TRIM ( P_NUMVOIE ) != '' ) THEN 
		SET V_NUMVOIE = CAST ( P_NUMVOIE AS INTEGER ) ;	 
	END IF ; 
	IF ( TRIM ( P_CPCDX ) != '' ) THEN	 
		SET V_CPX = CAST ( P_CPCDX AS INTEGER ) ; 
	END IF ; 
	 -- Traitement de l'adresse 
	IF ( P_HASADRESSE = 'O' ) THEN 
		SET V_NBLIGNE = 0 ; 
		SELECT COUNT ( * ) INTO V_NBLIGNE FROM YPOBASE 
		WHERE TRIM ( PBIPB ) = TRIM ( P_CODEOFFRE ) AND PBALX = P_VERSION AND PBTYP = P_TYPE ; 
		IF ( V_NBLIGNE > 0 ) THEN 
			 -- Enregistrement dans YADRESS 
			SET V_NBLIGNE = 0 ; 
			 -- Instanciation de P_ADRCHRONO si besoin 
			IF ( P_ADRCHRONO = 0 ) THEN 
				CALL SP_YCHRONO ( 'ADRESSE_CHRONO' , V_ADRCHR ) ; 
				SET V_ADRCHRONO = CAST ( V_ADRCHR AS INTEGER ) ; 
			ELSE 
				SET V_ADRCHRONO = P_ADRCHRONO ; 
			END IF ; 
			SELECT COUNT ( * ) INTO V_NBLIGNE FROM YADRESS 
			WHERE ABPCHR = V_ADRCHRONO ; 
			IF ( V_NBLIGNE > 0 ) THEN 
				IF ( P_ISADDRESSEMPTY = 0 ) THEN 
					UPDATE YADRESS 
					SET ABPLG3 = P_BATIMENT , ABPLBN = P_NUMVOIE2 , ABPEXT = P_EXTVOIE , ABPLG4 = P_NOMVOIE , ABPLG5 = P_BP , 
					ABPLOC = P_LOC , ABPDP6 = P_DEPARTEMENT , ABPVI6 = P_VILLE , ABPL4F = P_VOIECOMPLETE , 
					ABPL6F = P_VILLECOMPLETE , ABPVIX = P_VILLECDX , ABPMAT = P_MATRICULEHEX 
					WHERE ABPCHR = V_ADRCHRONO ; 
					 
					IF ( TRIM ( P_CP ) = '' ) THEN 
						UPDATE YADRESS 
						SET ABPCP6 = V_CP 
						WHERE ABPCHR = V_ADRCHRONO ;	 
					ELSE
						UPDATE YADRESS 
						SET ABPCP6 = 0 
						WHERE ABPCHR = V_ADRCHRONO ;	 
					END IF ; 
					IF ( TRIM ( P_NUMVOIE ) != '' ) THEN 
						UPDATE YADRESS 
						SET ABPNUM = V_NUMVOIE 
						WHERE ABPCHR = V_ADRCHRONO ; 
					ELSE 
						UPDATE YADRESS 
						SET ABPNUM = 0 
						WHERE ABPCHR = V_ADRCHRONO ; 
					END IF ; 
					IF ( TRIM ( P_CPCDX ) != '' ) THEN	 
						UPDATE YADRESS 
						SET ABPCEX = V_CPX 
						WHERE ABPCHR = V_ADRCHRONO ; 
					ELSE	 
						UPDATE YADRESS 
						SET ABPCEX = 0 
						WHERE ABPCHR = V_ADRCHRONO ; 
					END IF ;	 
					 
				ELSE 
					DELETE FROM YADRESS WHERE ABPCHR = V_ADRCHRONO ; 
				END IF ; 
				ELSE 
					IF ( P_ISADDRESSEMPTY = 0 ) THEN 
						INSERT INTO YADRESS 
						( ABPLG3, ABPLBN , ABPEXT , ABPLG4 , ABPLG5 , ABPLOC , ABPDP6 , ABPVI6 , ABPL4F , ABPL6F , ABPVIX , ABPMAT , ABPCHR ) 
						VALUES 
						( P_BATIMENT, P_NUMVOIE2 , P_EXTVOIE , P_NOMVOIE , P_BP , P_LOC , P_DEPARTEMENT , P_VILLE , P_VOIECOMPLETE , P_VILLECOMPLETE , P_VILLECDX , P_MATRICULEHEX , V_ADRCHRONO ) ; 
						 
						IF ( TRIM ( P_CP ) = '' ) THEN 
							UPDATE YADRESS 
							SET ABPCP6 = V_CP 
							WHERE ABPCHR = V_ADRCHRONO ;	 
						ELSE
							UPDATE YADRESS 
							SET ABPCP6 = 0 
							WHERE ABPCHR = V_ADRCHRONO ;	 
						END IF ; 
						IF ( TRIM ( P_NUMVOIE ) != '' ) THEN 
							UPDATE YADRESS 
							SET ABPNUM = V_NUMVOIE 
							WHERE ABPCHR = V_ADRCHRONO ; 
						ELSE
							UPDATE YADRESS 
							SET ABPNUM = 0 
							WHERE ABPCHR = V_ADRCHRONO ; 
						END IF ; 
						IF ( TRIM ( P_CPCDX ) != '' ) THEN	 
							UPDATE YADRESS 
							SET ABPCEX = V_CPX 
							WHERE ABPCHR = V_ADRCHRONO ; 
						ELSE	 
							UPDATE YADRESS 
							SET ABPCEX = 0 
							WHERE ABPCHR = V_ADRCHRONO ; 
						END IF ;							 
					 
					END IF ; 
				END IF ; 
  
				 -- Bug100: Refactoring into function call to set address					 
				IF ( P_BATIMENT != '' ) THEN 
					SET V_AD1 = TRIM ( P_BATIMENT ) ; 
					SET V_AD2 = F_BUILDADDRESS ( V_NUMVOIE , P_NUMVOIE2 , P_EXTVOIE , P_NOMVOIE ) ; 
				ELSE 
					SET V_AD1 = F_BUILDADDRESS ( V_NUMVOIE , P_NUMVOIE2 , P_EXTVOIE , P_NOMVOIE ) ; 
					SET V_AD2 = TRIM ( P_BP ) ; 
				END IF ; 
				 
				 -- Enregistrement dans YPOBASE 
				IF ( P_ISADDRESSEMPTY = 0 ) THEN 
					UPDATE YPOBASE 
					SET PBADH = V_ADRCHRONO , PBAD1 = V_AD1 , PBAD2 = V_AD2 , PBDEP = P_DEPARTEMENT , PBCPO = V_CP , PBVIL = P_VILLE , PBPAY = '' 
					WHERE TRIM ( PBIPB ) = TRIM ( P_CODEOFFRE ) AND PBALX = P_VERSION AND PBTYP = P_TYPE ; 
				ELSE 
					UPDATE YPOBASE 
					SET PBADH = 0 , PBAD1 = V_AD1 , PBAD2 = V_AD2 , PBDEP = P_DEPARTEMENT , PBCPO = V_CP , PBVIL = P_VILLE , PBPAY = '' 
					WHERE TRIM ( PBIPB ) = TRIM ( P_CODEOFFRE ) AND PBALX = P_VERSION AND PBTYP = P_TYPE ; 
				END IF ; 
				 -- Détermination du nombre de risque pour vérifier si on enregisre l'adresse au niveau du risque 
				SET V_FIRSTRSQ = 0 ; 
				SELECT JERSQ INTO V_FIRSTRSQ FROM YPRTRSQ 
				WHERE TRIM ( JEIPB ) = TRIM ( P_CODEOFFRE ) AND JEALX = P_VERSION 
				ORDER BY JERSQ FETCH FIRST 1 ROW ONLY ; 
				IF ( V_FIRSTRSQ != 0 ) THEN 
				 -- Enregistrement de l'adresse de l'offre dans le risque principal 
					 --SET V_ADRRSQCHRONO = 0 ; 
					SELECT JFADH INTO V_ADRRSQCHRONO FROM YPRTADR 
					WHERE TRIM ( JFIPB ) = TRIM ( P_CODEOFFRE ) AND JFALX = P_VERSION AND JFRSQ = V_FIRSTRSQ AND JFOBJ = 0 ; 
					SET V_ADRVILLE26 = P_VILLE ; 
					IF ( V_ADRRSQCHRONO = - 1 ) THEN 
						IF ( P_ISADDRESSEMPTY = 0 ) THEN 
							CALL SP_YCHRONO ( 'ADRESSE_CHRONO' , V_NUMRSQ ) ; 
							SET V_NUMCHR = CAST ( V_NUMRSQ AS INTEGER ) ; 
							SET V_ADRRSQCHRONO = V_NUMCHR ; 
						ELSE 
							SET V_NUMCHR = 0 ; 
						END IF ; 
						INSERT INTO YPRTADR 
						( JFIPB , JFALX , JFRSQ , JFOBJ , JFAD1 , JFAD2 , JFDEP , JFCPO , JFVIL , JFPAY , JFADH ) 
						VALUES 
						( P_CODEOFFRE , P_VERSION , V_FIRSTRSQ , 0 , V_AD1 , V_AD2 , P_DEPARTEMENT , P_CP , V_ADRVILLE26 , '' , V_NUMCHR ) ; 
					ELSE 
						IF ( V_ADRRSQCHRONO = 0 ) THEN 
							IF ( P_ISADDRESSEMPTY = 0 ) THEN 
								CALL SP_YCHRONO ( 'ADRESSE_CHRONO' , V_NUMRSQ ) ; 
								SET V_NUMCHR = CAST ( V_NUMRSQ AS INTEGER ) ; 
								SET V_ADRRSQCHRONO = V_NUMCHR ; 
							ELSE 
								SET V_NUMCHR = 0 ; 
							END IF ; 
							UPDATE YPRTADR 
							SET JFADH = V_NUMCHR , JFAD1 = V_AD1 , JFAD2 = V_AD2 , JFDEP = P_DEPARTEMENT , JFCPO = P_CP , JFVIL = V_ADRVILLE26 , JFPAY = '' 
							WHERE TRIM ( JFIPB ) = TRIM ( P_CODEOFFRE ) AND JFALX = P_VERSION AND JFRSQ = V_FIRSTRSQ AND JFOBJ = 0 ; 
						ELSE 
						IF ( P_ISADDRESSEMPTY = 0 ) THEN 
							SET V_NUMCHR = V_ADRRSQCHRONO ; 
							ELSE 
								SET V_NUMCHR = 0 ; 
						END IF ; 
							UPDATE YPRTADR 
							SET JFADH = V_NUMCHR , JFAD1 = V_AD1 , JFAD2 = V_AD2 , JFDEP = P_DEPARTEMENT , JFCPO = P_CP , JFVIL = V_ADRVILLE26 , JFPAY = '' 
							WHERE TRIM ( JFIPB ) = TRIM ( P_CODEOFFRE ) AND JFALX = P_VERSION AND JFRSQ = V_FIRSTRSQ AND JFOBJ = 0 ; 
						END IF ; 
				END IF ; 
					 -- Traitement de l'adresse du risque 
					SET V_NBLIGNE = 0 ; 
					SELECT COUNT ( * ) INTO V_NBLIGNE FROM YADRESS 
					WHERE ABPCHR = V_ADRRSQCHRONO ; 
					IF ( V_NBLIGNE > 0 ) THEN 
						IF ( P_ISADDRESSEMPTY = 0 ) THEN 
							UPDATE YADRESS 
							SET ABPLG3 = P_BATIMENT , ABPLBN = P_NUMVOIE2 , ABPEXT = P_EXTVOIE , ABPLG4 = P_NOMVOIE , ABPLG5 = P_BP , 
							ABPLOC = P_LOC , ABPDP6 = P_DEPARTEMENT , ABPVI6 = P_VILLE , ABPL4F = P_VOIECOMPLETE , 
							ABPL6F = P_VILLECOMPLETE , ABPVIX = P_VILLECDX , ABPMAT = P_MATRICULEHEX 
							WHERE ABPCHR = V_ADRRSQCHRONO ; 
							 
							IF ( TRIM ( P_CP ) = '' ) THEN 
								UPDATE YADRESS 
								SET ABPCP6 = V_CP 
								WHERE ABPCHR = V_ADRRSQCHRONO ;	 
							ELSE
								UPDATE YADRESS 
								SET ABPCP6 = 0 
								WHERE ABPCHR = V_ADRRSQCHRONO ;	 
							END IF ; 
							IF ( TRIM ( P_NUMVOIE ) != '' ) THEN 
								UPDATE YADRESS 
								SET ABPNUM = V_NUMVOIE 
								WHERE ABPCHR = V_ADRRSQCHRONO ;
							ELSE
								UPDATE YADRESS 
								SET ABPNUM = 0
								WHERE ABPCHR = V_ADRRSQCHRONO ; 
							END IF ; 
							IF ( TRIM ( P_CPCDX ) != '' ) THEN	 
								UPDATE YADRESS 
								SET ABPCEX = V_CPX 
								WHERE ABPCHR = V_ADRRSQCHRONO ; 
							ELSE 
								UPDATE YADRESS 
								SET ABPCEX = 0 
								WHERE ABPCHR = V_ADRRSQCHRONO ; 
							END IF ;	 
							 
						ELSE 
							DELETE FROM YADRESS WHERE ABPCHR = V_ADRRSQCHRONO ; 
						END IF ; 
					ELSE 
						IF ( P_ISADDRESSEMPTY = 0 ) THEN 
							INSERT INTO YADRESS 
								( ABPLG3 , ABPLBN , ABPEXT , ABPLG4 , ABPLG5 , ABPLOC , ABPDP6 , ABPVI6 , ABPL4F , ABPL6F , ABPVIX , ABPMAT , ABPCHR ) 
							VALUES 
								( P_BATIMENT , P_NUMVOIE2 , P_EXTVOIE , P_NOMVOIE , P_BP , P_LOC , P_DEPARTEMENT , P_VILLE , P_VOIECOMPLETE , P_VILLECOMPLETE , P_VILLECDX , P_MATRICULEHEX , V_ADRRSQCHRONO ) ; 
								 
							IF ( TRIM ( P_CP ) = '' ) THEN 
								UPDATE YADRESS 
								SET ABPCP6 = V_CP 
								WHERE ABPCHR = V_ADRRSQCHRONO ;	 
							ELSE
								UPDATE YADRESS 
								SET ABPCP6 = 0 
								WHERE ABPCHR = V_ADRRSQCHRONO ;	 
							END IF ; 
							IF ( TRIM ( P_NUMVOIE ) != '' ) THEN 
								UPDATE YADRESS 
								SET ABPNUM = V_NUMVOIE 
								WHERE ABPCHR = V_ADRRSQCHRONO ; 
							ELSE 
								UPDATE YADRESS 
								SET ABPNUM = 0 
								WHERE ABPCHR = V_ADRRSQCHRONO ; 
							END IF ; 
							IF ( TRIM ( P_CPCDX ) != '' ) THEN	 
								UPDATE YADRESS 
								SET ABPCEX = V_CPX 
								WHERE ABPCHR = V_ADRRSQCHRONO ; 
							ELSE
								UPDATE YADRESS 
								SET ABPCEX = 0 
								WHERE ABPCHR = V_ADRRSQCHRONO ; 
							END IF ;	 
							 
						END IF ; 
					END IF ; 
						 -- Traitement de l'adresse de l'objet si le risque est mono-objet 
					SET V_NBLIGNE = 0 ; 
					SELECT COUNT ( * ) INTO V_NBLIGNE FROM YPRTOBJ 
					WHERE TRIM ( JGIPB ) = TRIM ( P_CODEOFFRE ) AND JGALX = P_VERSION AND JGRSQ = V_FIRSTRSQ ; 
					IF ( V_NBLIGNE = 1 ) THEN 
						SELECT JGOBJ INTO V_FIRSTOBJ FROM YPRTOBJ 
						WHERE TRIM ( JGIPB ) = TRIM ( P_CODEOFFRE ) AND JGALX = P_VERSION AND JGRSQ = V_FIRSTRSQ ; 
  
						 --SET V_ADROBJCHRONO = 0 ; 
						SELECT JFADH INTO V_ADROBJCHRONO FROM YPRTADR 
						WHERE TRIM ( JFIPB ) = TRIM ( P_CODEOFFRE ) AND JFALX = P_VERSION AND JFRSQ = V_FIRSTRSQ AND JFOBJ = V_FIRSTOBJ ; 
  
						IF ( V_ADROBJCHRONO = - 1 ) THEN 
							IF ( P_ISADDRESSEMPTY = 0 ) THEN 
							CALL SP_YCHRONO ( 'ADRESSE_CHRONO' , V_NUMOBJ ) ; 
								SET V_NUMCHR = CAST ( V_NUMOBJ AS INTEGER ) ; 
								SET V_ADROBJCHRONO = V_NUMCHR ; 
							ELSE 
								SET V_NUMCHR = 0 ; 
							END IF ; 
							INSERT INTO YPRTADR 
							( JFIPB , JFALX , JFRSQ , JFOBJ , JFAD1 , JFAD2 , JFDEP , JFCPO , JFVIL , JFPAY , JFADH ) 
							VALUES 
							( P_CODEOFFRE , P_VERSION , V_FIRSTRSQ , V_FIRSTOBJ , V_AD1 , V_AD2 , P_DEPARTEMENT , P_CP , V_ADRVILLE26 , '' , V_NUMCHR ) ; 
						ELSE 
							IF ( V_ADROBJCHRONO = 0 ) THEN 
								IF ( P_ISADDRESSEMPTY = 0 ) THEN 
									CALL SP_YCHRONO ( 'ADRESSE_CHRONO' , V_NUMOBJ ) ; 
									SET V_NUMCHR = CAST ( V_NUMOBJ AS INTEGER ) ; 
									SET V_ADROBJCHRONO = V_NUMCHR ; 
								ELSE 
									SET V_NUMCHR = 0 ; 
								END IF ; 
  
								UPDATE YPRTADR 
								SET JFADH = V_NUMCHR , JFAD1 = V_AD1 , JFAD2 = V_AD2 , JFDEP = P_DEPARTEMENT , JFCPO = P_CP , JFVIL = V_ADRVILLE26 , JFPAY = '' 
								WHERE TRIM ( JFIPB ) = TRIM ( P_CODEOFFRE ) AND JFALX = P_VERSION AND JFRSQ = V_FIRSTRSQ AND JFOBJ = V_FIRSTOBJ ; 
						ELSE 
								IF ( P_ISADDRESSEMPTY = 0 ) THEN 
									SET V_NUMCHR = V_ADROBJCHRONO ; 
								ELSE 
									SET V_NUMCHR = 0 ; 
								END IF ; 
  
								UPDATE YPRTADR 
								SET JFADH = V_NUMCHR , JFAD1 = V_AD1 , JFAD2 = V_AD2 , JFDEP = P_DEPARTEMENT , JFCPO = P_CP , JFVIL = V_ADRVILLE26 , JFPAY = '' 
								WHERE TRIM ( JFIPB ) = TRIM ( P_CODEOFFRE ) AND JFALX = P_VERSION AND JFRSQ = V_FIRSTRSQ AND JFOBJ = V_FIRSTOBJ ; 
							END IF ; 
						END IF ; 
					 -- Traitement de l'adresse de l'objet 
						SET V_NBLIGNE = 0 ; 
							SELECT COUNT ( * ) INTO V_NBLIGNE FROM YADRESS 
						WHERE ABPCHR = V_ADROBJCHRONO ; 
						IF ( V_NBLIGNE > 0 ) THEN 
							IF ( P_ISADDRESSEMPTY = 0 ) THEN 
								UPDATE YADRESS 
								SET ABPLG3 = P_BATIMENT , ABPLBN = P_NUMVOIE2 , ABPEXT = P_EXTVOIE , ABPLG4 = P_NOMVOIE , ABPLG5 = P_BP , 
								ABPLOC = P_LOC , ABPDP6 = P_DEPARTEMENT , ABPVI6 = P_VILLE , ABPL4F = P_VOIECOMPLETE , 
									ABPL6F = P_VILLECOMPLETE , ABPVIX = P_VILLECDX , ABPMAT = P_MATRICULEHEX 
								WHERE ABPCHR = V_ADROBJCHRONO ; 
								 
								IF ( TRIM ( P_CP ) = '' ) THEN 
									UPDATE YADRESS 
									SET ABPCP6 = V_CP 
									WHERE ABPCHR = V_ADROBJCHRONO ;	 
								ELSE
									UPDATE YADRESS 
									SET ABPCP6 = 0 
									WHERE ABPCHR = V_ADROBJCHRONO ;	 
								END IF ; 
								IF ( TRIM ( P_NUMVOIE ) != '' ) THEN 
									UPDATE YADRESS 
									SET ABPNUM = V_NUMVOIE 
									WHERE ABPCHR = V_ADROBJCHRONO ; 
								ELSE
									UPDATE YADRESS 
									SET ABPNUM = 0 
									WHERE ABPCHR = V_ADROBJCHRONO ; 
								END IF ; 
								IF ( TRIM ( P_CPCDX ) != '' ) THEN	 
									UPDATE YADRESS 
									SET ABPCEX = V_CPX 
									WHERE ABPCHR = V_ADROBJCHRONO ; 
								ELSE	 
									UPDATE YADRESS 
									SET ABPCEX = 0 
									WHERE ABPCHR = V_ADROBJCHRONO ; 
								END IF ;	 
							 
							ELSE 
								DELETE FROM YADRESS WHERE ABPCHR = V_ADROBJCHRONO ; 
							END IF ; 
						ELSE 
							IF ( P_ISADDRESSEMPTY = 0 ) THEN 
								INSERT INTO YADRESS 
									( ABPLG3 , ABPLBN , ABPEXT , ABPLG4 , ABPLG5 , ABPLOC , ABPDP6 , ABPVI6 , ABPL4F , ABPL6F , ABPVIX , ABPMAT , ABPCHR ) 
								VALUES 
								( P_BATIMENT , P_NUMVOIE2 , P_EXTVOIE , P_NOMVOIE , P_BP , P_LOC , P_DEPARTEMENT , P_VILLE , P_VOIECOMPLETE , P_VILLECOMPLETE , P_VILLECDX , P_MATRICULEHEX , V_ADROBJCHRONO ) ; 
								 
								IF ( TRIM ( P_CP ) = '' ) THEN 
									UPDATE YADRESS 
									SET ABPCP6 = V_CP 
									WHERE ABPCHR = V_ADROBJCHRONO ;	 
								ELSE
									UPDATE YADRESS 
									SET ABPCP6 = 0 
									WHERE ABPCHR = V_ADROBJCHRONO ;	 
								END IF ; 
								IF ( TRIM ( P_NUMVOIE ) != '' ) THEN 
									UPDATE YADRESS 
									SET ABPNUM = V_NUMVOIE 
									WHERE ABPCHR = V_ADROBJCHRONO ; 
								ELSE 
									UPDATE YADRESS 
									SET ABPNUM = 0 
									WHERE ABPCHR = V_ADROBJCHRONO ; 
								END IF ; 
								IF ( TRIM ( P_CPCDX ) != '' ) THEN	 
									UPDATE YADRESS 
									SET ABPCEX = V_CPX 
									WHERE ABPCHR = V_ADROBJCHRONO ; 
								ELSE	 
									UPDATE YADRESS 
									SET ABPCEX = 0 
									WHERE ABPCHR = V_ADROBJCHRONO ; 
								END IF ;	 
						END IF ; 
				END IF ; 
				END IF ; 
		END IF ; 
	END IF ; 
END IF ; 
END P1  ;



