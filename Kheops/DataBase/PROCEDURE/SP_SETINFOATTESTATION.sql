﻿CREATE PROCEDURE SP_SETINFOATTESTATION ( 
	IN P_CODECONTRAT CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_FONCTION CHAR(6) , 
	IN P_PERIODEDEB INTEGER , 
	IN P_PERIODEFIN INTEGER , 
	IN P_EXERCICE INTEGER , 
	IN P_TYPEATTES CHAR(10) , 
	IN P_COUVATTES CHAR(1) , 
	IN P_USER CHAR(15) , 
	IN P_DATENOW INTEGER , 
	OUT P_ERREUR CHAR(50) , 
	OUT P_LOTID INTEGER ) 
	LANGUAGE SQL 
	SPECIFIC SP_SETINFOATTESTATION 
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
  
	DECLARE V_PERIODEDEB INTEGER DEFAULT 0 ; 
	DECLARE V_PERIODEFIN INTEGER DEFAULT 0 ; 
	DECLARE V_NEWID INTEGER DEFAULT 0 ; 
	 
	DECLARE V_TOP_OPT_EDTB CHAR ( 1 ) DEFAULT '' ; 
	DECLARE V_CODEFOR INTEGER DEFAULT 0 ; 
	DECLARE V_CODEOPT INTEGER DEFAULT 0 ; 
	DECLARE V_CODERSQ INTEGER DEFAULT 0 ; 
	DECLARE V_DEBFOR INTEGER DEFAULT 0 ; 
	DECLARE V_FINFOR INTEGER DEFAULT 0 ; 
	 
	SET V_NEWID = 0 ; 
	 
	/* VÉRIFICATION DES DATES DU CONTRAT AVEC LA PÉRIODE */ 
	SELECT ( ( PBEFA * 10000 ) + ( PBEFM * 100 ) + PBEFJ ) , ( ( PBFEA * 10000 ) + ( PBFEM * 100 ) + PBFEJ ) 
		INTO V_PERIODEDEB , V_PERIODEFIN 
	FROM YPOBASE 
		WHERE TRIM ( PBIPB ) = TRIM ( P_CODECONTRAT ) AND PBALX = P_VERSION AND PBTYP = P_TYPE ; 
		 
	/* SI LES DATES DU CONTRAT NE RENTRENT PAS DANS LA PÉRIODE => RENVOI MESSAGE */ 
	IF ( V_PERIODEDEB > P_PERIODEFIN OR ( V_PERIODEFIN != 0 AND V_PERIODEFIN < P_PERIODEDEB ) ) THEN 
		SET P_ERREUR = 'Aucun élément trouvé dans la période' ; 
	ELSE 
		SET P_ERREUR = '' ; 
		/* SÉLECTION DES RISQUES */ 
		FOR LOOP_RSQ AS FREE_LIST CURSOR FOR 
			SELECT JERSQ CODERSQ , ( JEVDA * 10000 + JEVDM * 100 + JEVDJ ) DEBRSQ , ( JEVFA * 10000 + JEVFM * 100 + JEVFJ ) FINRSQ , 
				JERUL RULRSQ 
			FROM YPRTRSQ WHERE TRIM ( JEIPB ) = TRIM ( P_CODECONTRAT ) AND JEALX = P_VERSION 
		DO 
			IF ( ( P_FONCTION = 'REGULE' AND RULRSQ <> 'N' ) OR ( P_FONCTION != 'REGULE' ) ) THEN 
				IF ( DEBRSQ = 0 ) THEN 
					SET DEBRSQ = V_PERIODEDEB ; 
				END IF ; 
				IF ( FINRSQ = 0 ) THEN 
					SET FINRSQ = V_PERIODEFIN ; 
				END IF ; 
				 
				/* SI LES DATES DU RISQUE RENTRENT DANS LA PÉRIODE */ 
				IF ( DEBRSQ <= P_PERIODEFIN AND ( FINRSQ >= P_PERIODEDEB OR FINRSQ = 0 ) ) THEN 
					IF ( V_NEWID = 0 ) THEN 
						CALL SP_NCHRONO ( 'KHVID' , V_NEWID ) ; 
					END IF ; 
					/* INSERTION DU RISQUE */ 
					INSERT INTO KPSELW 
						( KHVID , KHVTYP , KHVIPB , KHVALX , KHVPERI , KHVRSQ , KHVOBJ , KHVFOR , KHVKDEID , KHVEDTB , KHVDEB , KHVFIN ) 
					VALUES 
						( V_NEWID , P_TYPE , P_CODECONTRAT , P_VERSION , 'RQ' , CODERSQ , 0 , 0 , 0 , '' , DEBRSQ , FINRSQ ) ; 
						 
					/* SÉLECTION DES OBJETS */ 
					FOR LOOP_OBJ AS FREE_LIST CURSOR FOR 
						SELECT JGOBJ CODEOBJ , ( JGVDA * 10000 + JGVDM * 100 + JGVDJ ) DEBOBJ , ( JGVFA * 10000 + JGVFM * 100 + JGVFJ ) FINOBJ 
						FROM YPRTOBJ WHERE TRIM ( JGIPB ) = TRIM ( P_CODECONTRAT ) AND JGALX = P_VERSION AND JGRSQ = CODERSQ 
					DO 
						IF ( DEBOBJ = 0 ) THEN 
							SET DEBOBJ = DEBRSQ ; 
						END IF ; 
						IF ( FINOBJ = 0 ) THEN 
							SET FINOBJ = FINRSQ ; 
						END IF ; 
						 
						/* SI LES DATES DE L'OBJET RENTRENT DANS LA PÉRIODE */ 
						IF ( DEBOBJ <= P_PERIODEFIN AND ( FINOBJ >= P_PERIODEDEB OR FINOBJ = 0 ) ) THEN 
							/* INSERTION DE L'OBJET */ 
							INSERT INTO KPSELW 
								( KHVID , KHVTYP , KHVIPB , KHVALX , KHVPERI , KHVRSQ , KHVOBJ , KHVFOR , KHVKDEID , KHVEDTB , KHVDEB , KHVFIN ) 
							VALUES 
								( V_NEWID , P_TYPE , P_CODECONTRAT , P_VERSION , 'OB' , CODERSQ , CODEOBJ , 0 , 0 , '' , DEBOBJ , FINOBJ ) ; 
						END IF ; 
					END FOR ; 
					 
					/* SÉLECTION DES FORMULES */ 
					SET V_CODEFOR = 0 ; 
					SET V_TOP_OPT_EDTB = 'O' ; 
					FOR LOOP_OPT AS FREE_LIST CURSOR FOR 
						SELECT KDBFOR CODEFOR , KDBOPT CODEOPT , IFNULL ( SELWRSQ . KHVRSQ , 0 ) CODERSQFOR , SELWRSQ . KHVDEB DEBFOR , SELWRSQ . KHVFIN FINFOR 
							FROM KPOPT 
								INNER JOIN KPOPTAP OPTAPRSQ ON OPTAPRSQ . KDDKDBID = KDBID AND OPTAPRSQ . KDDPERI = 'RQ' AND OPTAPRSQ . KDDRSQ = CODERSQ 
								LEFT JOIN KPSELW SELWRSQ ON SELWRSQ . KHVIPB = OPTAPRSQ . KDDIPB AND SELWRSQ . KHVALX = OPTAPRSQ . KDDALX AND SELWRSQ . KHVTYP = OPTAPRSQ . KDDTYP 
									AND SELWRSQ . KHVPERI = 'RQ' AND SELWRSQ . KHVRSQ = OPTAPRSQ . KDDRSQ AND SELWRSQ . KHVID = V_NEWID
							WHERE TRIM ( KDBIPB ) = TRIM ( P_CODECONTRAT ) AND KDBALX = P_VERSION AND KDBTYP = P_TYPE 
						UNION 
						SELECT KDBFOR CODEFOR , KDBOPT CODEOPT , IFNULL ( SELWOBJ . KHVRSQ , 0 ) CODERSQFOR , SELWOBJ . KHVDEB DEBFOR , SELWOBJ . KHVFIN FINFOR 
							FROM KPOPT 
								INNER JOIN KPOPTAP OPTAPOBJ ON OPTAPOBJ . KDDKDBID = KDBID AND OPTAPOBJ . KDDPERI = 'OB' AND OPTAPOBJ . KDDRSQ = CODERSQ 
								LEFT JOIN KPSELW SELWOBJ ON SELWOBJ . KHVIPB = OPTAPOBJ . KDDIPB AND SELWOBJ . KHVALX = OPTAPOBJ . KDDALX AND SELWOBJ . KHVTYP = OPTAPOBJ . KDDTYP 
									AND SELWOBJ . KHVPERI = 'OB' AND SELWOBJ . KHVRSQ = OPTAPOBJ . KDDRSQ AND SELWOBJ . KHVOBJ = OPTAPOBJ . KDDOBJ AND SELWOBJ . KHVID = V_NEWID
							WHERE TRIM ( KDBIPB ) = TRIM ( P_CODECONTRAT ) AND KDBALX = P_VERSION AND KDBTYP = P_TYPE 
						ORDER BY CODEFOR					 
		
					DO 
						IF ( V_CODEFOR != CODEFOR ) THEN 
							/* SI LES DATES DE LA FORMULE RENTRENT DANS LA PÉRIODE */ 
							IF ( V_CODEFOR != 0 AND V_DEBFOR <= P_PERIODEFIN AND ( V_FINFOR >= P_PERIODEDEB OR V_FINFOR = 0 ) ) THEN 
								/* APPEL DE LA PROCÉDURE POUR INSÉRER LA FORMULE ET TRAITER LES GARANTIES DE CELLE-CI */ 
								CALL SP_INSGARANTIEATTES ( P_CODECONTRAT , P_VERSION , P_TYPE , 0, P_FONCTION , P_PERIODEDEB , P_PERIODEFIN , V_NEWID , V_CODERSQ , V_CODEFOR , V_CODEOPT , V_TOP_OPT_EDTB , V_DEBFOR , V_FINFOR ) ; 
							END IF ; 
							 
							SET V_CODEFOR = CODEFOR ; 
							SET V_CODEOPT = CODEOPT ; 
							SET V_CODERSQ = CODERSQFOR ; 
							SET V_DEBFOR = DEBFOR ; 
							SET V_FINFOR = FINFOR ; 
						ELSE 
							IF ( DEBFOR < V_DEBFOR ) THEN 
								SET V_DEBFOR = DEBFOR ; 
							END IF ; 
							IF ( FINFOR > V_FINFOR ) THEN 
								SET V_FINFOR = FINFOR ; 
							END IF ; 
						END IF ; 
					END FOR ; 
					/* SI LES DATES DE LA FORMULE RENTRENT DANS LA PÉRIODE */ 
					IF ( V_CODEFOR != 0 AND V_DEBFOR <= P_PERIODEFIN AND ( V_FINFOR >= P_PERIODEDEB OR V_FINFOR = 0 ) ) THEN 
						/* APPEL DE LA PROCÉDURE POUR INSÉRER LA FORMULE ET TRAITER LES GARANTIES DE CELLE-CI */ 
						CALL SP_INSGARANTIEATTES ( P_CODECONTRAT , P_VERSION , P_TYPE , 0, P_FONCTION , P_PERIODEDEB , P_PERIODEFIN , V_NEWID , V_CODERSQ , V_CODEFOR , V_CODEOPT , V_TOP_OPT_EDTB , V_DEBFOR , V_FINFOR) ; 
						SET V_CODEFOR = 0 ; 
					END IF ;					 
				END IF ;			 
			END IF ;			 
		END FOR ; 
	END IF ; 
	SET P_LOTID = V_NEWID ; 
	 
	IF ( P_LOTID != 0 AND TRIM ( P_ERREUR ) = '' ) THEN 
		IF ( P_FONCTION = 'ATTEST' ) THEN 
			/* CRÉATION DANS LA TABLE KPATT ET ALIMENTATION DE KPATTF */ 
			P2 : BEGIN 
				DECLARE V_NEWATTID INTEGER DEFAULT 0 ; 
				DECLARE V_NEWATTFID INTEGER DEFAULT 0 ; 
				 
				CALL SP_NCHRONO ( 'KHTID' , V_NEWATTID ) ; 
				/* INSERTION DANS KPATT */ 
				INSERT INTO KPATT 
					( KHTID , KHTTYP , KHTIPB , KHTALX , KHTEXE , KHTDEBP , KHTFINP , KHTTYAT , KHTCATT , KHTROF , KHTGAF , KHTCRU , KHTCRD , KHTMAJU , KHTMAJD ) 
				VALUES 
					( V_NEWATTID , P_TYPE , P_CODECONTRAT , P_VERSION , P_EXERCICE , P_PERIODEDEB , P_PERIODEFIN , P_TYPEATTES , P_COUVATTES , 'N' , 'N' , P_USER , P_DATENOW , P_USER , P_DATENOW ) ; 
				 
				/* LECTURE DE TOUS LES ENREGISTREMENTS DE KPSELW POUR LE LOT DONNÉ P_LOTID */ 
				FOR LOOP_ATTES AS FREE_LIST CURSOR FOR 
					SELECT KHVPERI PERI , KHVRSQ CODERSQ , KHVOBJ CODEOBJ , KHVFOR CODEFOR , KHVKDEID CODEGARAN , KHVEDTB EDTB , KHVDEB DEBATTES , KHVFIN FINATTES 
					FROM KPSELW 
					WHERE TRIM ( KHVIPB ) = TRIM ( P_CODECONTRAT ) AND KHVALX = P_VERSION AND KHVTYP = P_TYPE AND KHVID = P_LOTID 
				DO 
					CALL SP_NCHRONO ( 'KHUID' , V_NEWATTFID ) ; 
					/* INSERTION DANS KPATTF */ 
					INSERT INTO KPATTF 
						( KHUID , KHUKHTID , KHUPERI , KHURSQ , KHUOBJ , KHUFOR , KHUKDEID , KHUEDTB , KHUDEB , KHUFIN , KHUSIT ) 
					VALUES 
						( V_NEWATTFID , V_NEWATTID , PERI , CODERSQ , CODEOBJ , CODEFOR , CODEGARAN , EDTB , DEBATTES , FINATTES , 'V' ) ; 
				END FOR ; 
				 
				SET P_LOTID = V_NEWATTID ; 
			END P2 ; 
		END IF ; 
	END IF ; 
END P1  ;
  

  

