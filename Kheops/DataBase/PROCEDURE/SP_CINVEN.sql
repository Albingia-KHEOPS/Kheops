﻿CREATE OR REPLACE PROCEDURE SP_CINVEN(
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_CODEINVEN INTEGER , 
	IN P_NEWVERSION INTEGER , 
	IN P_CODECONTRAT CHAR(9) , 
	IN P_VERSIONCONTRAT INTEGER , 
	IN P_TRAITEMENT VARCHAR(1) , 
	IN P_COPYCODEOFFRE CHAR(9) , 
	IN P_COPYVERSION INTEGER , 
	IN P_MODECOPY CHAR(7) , 
	OUT P_NEWCODEINVEN INTEGER ) 
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
  
	DECLARE V_CODEDESI INTEGER DEFAULT 0 ; 
	DECLARE V_NEWCODEDESI INTEGER DEFAULT 0 ; 
	DECLARE V_NEWKBEID INTEGER DEFAULT 0 ; 
	DECLARE V_NEWKBFID INTEGER DEFAULT 0 ; 
	 
	DECLARE V_CODEINVENDTL INTEGER DEFAULT 0 ; 
	DECLARE V_NEWCODEINVENDTL INTEGER DEFAULT 0 ; 
	 
	DECLARE V_CODEINVENAPP INTEGER DEFAULT 0 ; 
	DECLARE V_NEWCODEINVENAPP INTEGER DEFAULT 0 ; 
	 
	DECLARE V_CODEOFFRE VARCHAR ( 9 ) DEFAULT '' ; 
	DECLARE V_TYPEOFFRE VARCHAR ( 1 ) DEFAULT '' ; 
	DECLARE V_VERSOFFRE INTEGER DEFAULT 0 ; 
	DECLARE V_NEWVERS INTEGER DEFAULT 0 ; 
	 
	DECLARE V_APPL CHAR ( 2 ) DEFAULT '' ; 
	
	SET P_CODEOFFRE = LPAD ( TRIM ( P_CODEOFFRE ) , 9 , ' ') ;
	SET P_CODECONTRAT = LPAD ( TRIM ( P_CODECONTRAT ) , 9 , ' ') ;
	SET P_COPYCODEOFFRE = LPAD ( TRIM ( P_COPYCODEOFFRE ) , 9 , ' ') ;


	SET V_CODEOFFRE = P_CODEOFFRE ; 
	SET V_TYPEOFFRE = P_TYPE ; 
	SET V_VERSOFFRE = P_VERSION ; 
	SET V_NEWVERS = P_NEWVERSION ; 
	 
	IF ( P_TRAITEMENT = 'P' ) THEN 
		SET V_CODEOFFRE = P_CODECONTRAT ; 
		SET V_TYPEOFFRE = 'P' ; 
		SET V_VERSOFFRE = P_VERSIONCONTRAT ; 
		SET V_NEWVERS = P_VERSIONCONTRAT ; 
	END IF ; 
	 
	IF ( P_TRAITEMENT = 'C' ) THEN 
	SET V_CODEOFFRE = P_COPYCODEOFFRE ; 
	SET V_VERSOFFRE = P_COPYVERSION ; 
	SET V_NEWVERS = P_COPYVERSION ; 
	END IF ; 
	 
	FOR LOOP_INVEN AS FREE_LIST CURSOR FOR 
		SELECT DISTINCT KBEID V_CODEINVEN , KBEKADID V_INVENDESI 
		FROM KPINVEN 
		LEFT JOIN KPGARAN ON KBEID = KDEINVEN 
		WHERE KBEIPB = P_CODEOFFRE AND KBEALX = P_VERSION AND KBETYP = P_TYPE 
		AND ( P_MODECOPY <> 'AFFNOUV' OR KDEINVEN IS NULL )
		UNION ALL 
		SELECT DISTINCT KBEID V_CODEINVEN , KBEKADID V_INVENDESI 
		FROM KPINVEN 
		INNER JOIN KPGARAN ON KBEID = KDEINVEN AND P_MODECOPY = 'AFFNOUV' 
		INNER JOIN KPOPTD ON KDCID = KDEKDCID
		INNER JOIN KPOPT ON KDCKDBID = KDBID
		INNER JOIN KPOFOPT ON (KDBIPB, KDBALX, KDBTYP, KDBOPT, 'O', 'O' , P_CODECONTRAT , P_VERSIONCONTRAT ) = (KFJIPB, KFJALX, 'O', KFJOPT, KFJTENG, KFJSEL, KFJPOG , KFJALG)
		WHERE KBEIPB = P_CODEOFFRE AND KBEALX = P_VERSION AND KBETYP = P_TYPE 
	DO 
		/*----  COPIE DE KPINVEN  ----*/ 
		IF ( V_INVENDESI != 0 ) THEN 
			CALL SP_SECOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KADCHR' , V_INVENDESI , V_NEWCODEDESI ) ; 
			IF ( V_NEWCODEDESI = 0 ) THEN 
				CALL SP_NCHRONO ( 'KADCHR' , V_NEWCODEDESI ) ; 
				CALL SP_INCOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KADCHR' , V_INVENDESI , V_NEWCODEDESI ) ; 
			END IF ; 
		END IF ; 
		 
		IF ( V_CODEINVEN != 0 ) THEN 
			CALL SP_SECOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KBEID' , V_CODEINVEN , V_NEWKBEID ) ; 
			IF ( V_NEWKBEID = 0 ) THEN 
				CALL SP_NCHRONO ( 'KBEID' , V_NEWKBEID ) ; 
				CALL SP_INCOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KBEID' , V_CODEINVEN , V_NEWKBEID ) ; 
			END IF ; 
		END IF ; 
		 
		 --CALL SP_NCHRONO ( 'KBEID' , V_NEWKBEID ) ; 
		IF ( TRIM ( P_MODECOPY ) = 'CNVA' OR TRIM ( P_MODECOPY ) = 'OFFRE' OR TRIM ( P_MODECOPY ) = 'AFNCOPY' ) THEN 
			IF ( P_TRAITEMENT = 'V' ) THEN 
				INSERT INTO KPINVEN 
				( KBEID , KBETYP , KBEIPB , KBEALX , KBECHR , KBEDESC , KBEKAGID , KBEKADID , 
				KBEREPVAL , KBEVAL , KBEVAA , KBEVAW , KBEVAT , KBEVAU , KBEVAH , KBEIVO , KBEIVA , KBEIVW ) 
				( SELECT V_NEWKBEID , KBETYP , KBEIPB , P_NEWVERSION , KBECHR , KBEDESC , KBEKAGID , V_NEWCODEDESI , 
				KBEREPVAL , 0 , KBEVAA , KBEVAW , '' , '' , KBEVAH , KBEIVO , KBEIVA , KBEIVW 
				FROM KPINVEN WHERE KBEID = V_CODEINVEN ) ; 
			ELSE 
				IF ( P_TRAITEMENT = 'P' ) THEN 
					INSERT INTO KPINVEN 
					( KBEID , KBETYP , KBEIPB , KBEALX , KBECHR , KBEDESC , KBEKAGID , KBEKADID , 
					KBEREPVAL , KBEVAL , KBEVAA , KBEVAW , KBEVAT , KBEVAU , KBEVAH , KBEIVO , KBEIVA , KBEIVW ) 
					( SELECT V_NEWKBEID , 'P' , P_CODECONTRAT , P_VERSIONCONTRAT , KBECHR , KBEDESC , KBEKAGID , V_NEWCODEDESI , 
					KBEREPVAL , 0 , KBEVAA , KBEVAW , '' , '' , KBEVAH , KBEIVO , KBEIVA , KBEIVW 
					FROM KPINVEN WHERE KBEID = V_CODEINVEN ) ; 
				ELSE 
					IF ( P_TRAITEMENT = 'C' ) THEN 
						INSERT INTO KPINVEN 
						( KBEID , KBETYP , KBEIPB , KBEALX , KBECHR , KBEDESC , KBEKAGID , KBEKADID , 
						KBEREPVAL , KBEVAL , KBEVAA , KBEVAW , KBEVAT , KBEVAU , KBEVAH , KBEIVO , KBEIVA , KBEIVW ) 
						( SELECT V_NEWKBEID , P_TYPE , V_CODEOFFRE , V_NEWVERS , KBECHR , KBEDESC , KBEKAGID , V_NEWCODEDESI , 
						'' , 0 , KBEVAA , KBEVAW , '' , '' , KBEVAH , KBEIVO , KBEIVA , KBEIVW 
						FROM KPINVEN WHERE KBEID = V_CODEINVEN ) ; 
					END IF ; 
				END IF ; 
			END IF ;	 
		ELSE 
			IF ( P_TRAITEMENT = 'V' ) THEN 
				INSERT INTO KPINVEN 
				( KBEID , KBETYP , KBEIPB , KBEALX , KBECHR , KBEDESC , KBEKAGID , KBEKADID , 
				KBEREPVAL , KBEVAL , KBEVAA , KBEVAW , KBEVAT , KBEVAU , KBEVAH , KBEIVO , KBEIVA , KBEIVW ) 
				( SELECT V_NEWKBEID , KBETYP , KBEIPB , P_NEWVERSION , KBECHR , KBEDESC , KBEKAGID , V_NEWCODEDESI , 
				KBEREPVAL , KBEVAL , KBEVAA , KBEVAW , KBEVAT , KBEVAU , KBEVAH , KBEIVO , KBEIVA , KBEIVW 
				FROM KPINVEN WHERE KBEID = V_CODEINVEN ) ; 
			ELSE 
				IF ( P_TRAITEMENT = 'P' ) THEN 
					INSERT INTO KPINVEN 
					( KBEID , KBETYP , KBEIPB , KBEALX , KBECHR , KBEDESC , KBEKAGID , KBEKADID , 
					KBEREPVAL , KBEVAL , KBEVAA , KBEVAW , KBEVAT , KBEVAU , KBEVAH , KBEIVO , KBEIVA , KBEIVW ) 
					( SELECT V_NEWKBEID , 'P' , P_CODECONTRAT , P_VERSIONCONTRAT , KBECHR , KBEDESC , KBEKAGID , V_NEWCODEDESI , 
					KBEREPVAL , KBEVAL , KBEVAA , KBEVAW , KBEVAT , KBEVAU , KBEVAH , KBEIVO , KBEIVA , KBEIVW 
					FROM KPINVEN WHERE KBEID = V_CODEINVEN ) ; 
				ELSE 
					IF ( P_TRAITEMENT = 'C' ) THEN 
						INSERT INTO KPINVEN 
						( KBEID , KBETYP , KBEIPB , KBEALX , KBECHR , KBEDESC , KBEKAGID , KBEKADID , 
						KBEREPVAL , KBEVAL , KBEVAA , KBEVAW , KBEVAT , KBEVAU , KBEVAH , KBEIVO , KBEIVA , KBEIVW ) 
						( SELECT V_NEWKBEID , P_TYPE , V_CODEOFFRE , V_NEWVERS , KBECHR , KBEDESC , KBEKAGID , V_NEWCODEDESI , 
						KBEREPVAL , KBEVAL , KBEVAA , KBEVAW , KBEVAT , KBEVAU , KBEVAH , KBEIVO , KBEIVA , KBEIVW 
						FROM KPINVEN WHERE KBEID = V_CODEINVEN ) ; 
					END IF ; 
				END IF ; 
			END IF ; 
		END IF ; 
		 
		SET V_APPL = '' ; 
		SELECT KBGPERI INTO V_APPL FROM KPINVAPP WHERE KBGKBEID = V_CODEINVEN ; 
		/* MISE À JOUR DE LA TABLE DE GARANTIE SI L'INVENTAIRE S'APPLIQUE À UNE GARANTIE */ 
		IF ( TRIM ( V_APPL ) = 'GA' ) THEN 
			IF ( P_TRAITEMENT = 'V' ) THEN 
				UPDATE KPGARAN SET KDEINVEN = V_NEWKBEID WHERE KDEINVEN = V_CODEINVEN AND KDEIPB = P_CODEOFFRE AND KDEALX = P_NEWVERSION AND KDETYP = P_TYPE ; 
				UPDATE KPGARAH SET KDEINVEN = V_NEWKBEID WHERE KDEINVEN = V_CODEINVEN AND KDEIPB = P_CODEOFFRE AND KDEALX = P_NEWVERSION AND KDETYP = P_TYPE ; 
			ELSE 
				IF ( P_TRAITEMENT = 'P' ) THEN 
					UPDATE KPGARAN SET KDEINVEN = V_NEWKBEID WHERE KDEINVEN = V_CODEINVEN AND KDEIPB = P_CODECONTRAT  AND KDEALX = P_VERSIONCONTRAT AND KDETYP = 'P' ; 
					UPDATE KPGARAH SET KDEINVEN = V_NEWKBEID WHERE KDEINVEN = V_CODEINVEN AND KDEIPB = P_CODECONTRAT  AND KDEALX = P_VERSIONCONTRAT AND KDETYP = 'P' ; 
				ELSE 
					IF ( P_TRAITEMENT = 'C' ) THEN 
						UPDATE KPGARAN SET KDEINVEN = V_NEWKBEID WHERE KDEINVEN = V_CODEINVEN AND KDEIPB = V_CODEOFFRE AND KDEALX = V_NEWVERS AND KDETYP = P_TYPE ; 
						UPDATE KPGARAH SET KDEINVEN = V_NEWKBEID WHERE KDEINVEN = V_CODEINVEN AND KDEIPB = V_CODEOFFRE AND KDEALX = V_NEWVERS AND KDETYP = P_TYPE ; 
					END IF ; 
				END IF ; 
			END IF ; 
		END IF ; 
		 
		/*----  COPIE DE KPINVED  ----*/ 
		IF ( TRIM ( P_MODECOPY ) = 'VERSION' OR TRIM ( P_MODECOPY ) = 'AFFNOUV' ) THEN 
			FOR LOOP_INVED AS FREE_LIST CURSOR FOR 
				SELECT KBFID V_CODEINVED , KBFKADID V_INVEDDESI , KBFADH V_ADR 
				FROM KPINVED 
				WHERE KBFKBEID = V_CODEINVEN 
				DO 
					IF ( V_INVEDDESI != 0 ) THEN 
						CALL SP_SECOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KADCHR' , V_INVEDDESI , V_NEWCODEDESI ) ; 
						IF ( V_NEWCODEDESI = 0 ) THEN 
							CALL SP_NCHRONO ( 'KADCHR' , V_NEWCODEDESI ) ; 
							CALL SP_INCOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KADCHR' , V_INVEDDESI , V_NEWCODEDESI ) ; 
						END IF ; 
					END IF ; 
				 
					CALL SP_NCHRONO ( 'KBFID' , V_NEWKBFID ) ; 
				 
				IF ( P_TRAITEMENT = 'V' ) THEN 
				 
					CALL SP_SECOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KBFID' , V_CODEINVED , V_NEWKBFID ) ; 
			 
					IF ( V_NEWKBFID = 0 ) THEN 
						CALL SP_NCHRONO ( 'KBFID' , V_NEWKBFID ) ; 
						CALL SP_INCOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KBFID' , V_CODEINVED , V_NEWKBFID ) ; 
					END IF ; 
				 
					INSERT INTO KPINVED 
					( KBFID , KBFTYP , KBFIPB , KBFALX , KBFKBEID , KBFNUMLGN , KBFDESC , KBFKADID , KBFSITE , 
					KBFNTLI , KBFCP , KBFVILLE , KBFADH , KBFDATDEB , KBFDEBHEU , KBFDATFIN , KBFFINHEU , KBFMNT1 , KBFMNT2 , KBFNBEVN , 
					KBFNBPER , KBFNOM , KBFPNOM , KBFDATNAI , KBFFONC , KBFCDEC , KBFCIP , KBFACCS , KBFAVPR , KBFMSR , KBFCMAT , KBFSEX , KBFMDQ , KBFMDA , KBFACTP , 
					KBFKADFH , KBFEXT , KBFMNT3 , KBFMNT4 , KBFQUA , KBFREN , KBFRLO ) 
					( SELECT V_NEWKBFID , KBFTYP , KBFIPB , P_NEWVERSION , V_NEWKBEID , KBFNUMLGN , KBFDESC , V_NEWCODEDESI , KBFSITE , 
					KBFNTLI , KBFCP , KBFVILLE , KBFADH , KBFDATDEB , KBFDEBHEU , KBFDATFIN , KBFFINHEU , KBFMNT1 , KBFMNT2 , KBFNBEVN , 
					KBFNBPER , KBFNOM , KBFPNOM , KBFDATNAI , KBFFONC , KBFCDEC , KBFCIP , KBFACCS , KBFAVPR , KBFMSR , KBFCMAT , KBFSEX , KBFMDQ , KBFMDA , KBFACTP , 
					KBFKADFH , KBFEXT , KBFMNT3 , KBFMNT4 , KBFQUA , KBFREN , KBFRLO 
					FROM KPINVED WHERE KBFID = V_CODEINVED ) ;					 
				ELSE 
					IF ( P_TRAITEMENT = 'P' ) THEN 
					 
						CALL SP_SECOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KBFID' , V_CODEINVED , V_NEWKBFID ) ; 
			 
						IF ( V_NEWKBFID = 0 ) THEN 
							CALL SP_NCHRONO ( 'KBFID' , V_NEWKBFID ) ; 
							CALL SP_INCOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KBFID' , V_CODEINVED , V_NEWKBFID ) ; 
						END IF ; 
					 
						INSERT INTO KPINVED 
						( KBFID , KBFTYP , KBFIPB , KBFALX , KBFKBEID , KBFNUMLGN , KBFDESC , KBFKADID , KBFSITE , 
						KBFNTLI , KBFCP , KBFVILLE , KBFADH , KBFDATDEB , KBFDEBHEU , KBFDATFIN , KBFFINHEU , KBFMNT1 , KBFMNT2 , KBFNBEVN , 
						KBFNBPER , KBFNOM , KBFPNOM , KBFDATNAI , KBFFONC , KBFCDEC , KBFCIP , KBFACCS , KBFAVPR , KBFMSR , KBFCMAT , KBFSEX , KBFMDQ , KBFMDA , KBFACTP , 
						KBFKADFH , KBFEXT , KBFMNT3 , KBFMNT4 , KBFQUA , KBFREN , KBFRLO ) 
						( SELECT V_NEWKBFID , 'P' , P_CODECONTRAT , P_VERSIONCONTRAT , V_NEWKBEID , KBFNUMLGN , KBFDESC , V_NEWCODEDESI , KBFSITE , 
						KBFNTLI , KBFCP , KBFVILLE , KBFADH , KBFDATDEB , KBFDEBHEU , KBFDATFIN , KBFFINHEU , KBFMNT1 , KBFMNT2 , KBFNBEVN , 
						KBFNBPER , KBFNOM , KBFPNOM , KBFDATNAI , KBFFONC , KBFCDEC , KBFCIP , KBFACCS , KBFAVPR , KBFMSR , KBFCMAT , KBFSEX , KBFMDQ , KBFMDA , KBFACTP , 
						KBFKADFH , KBFEXT , KBFMNT3 , KBFMNT4 , KBFQUA , KBFREN , KBFRLO 
						FROM KPINVED WHERE KBFID = V_CODEINVED ) ; 
  
					ELSE 
						IF ( P_TRAITEMENT = 'C' ) THEN 
						 
							CALL SP_SECOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KBFID' , V_CODEINVED , V_NEWKBFID ) ; 
			 
							IF ( V_NEWKBFID = 0 ) THEN 
								CALL SP_NCHRONO ( 'KBFID' , V_NEWKBFID ) ; 
								CALL SP_INCOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'KBFID' , V_CODEINVED , V_NEWKBFID ) ; 
							END IF ; 
						 
							INSERT INTO KPINVED 
							( KBFID , KBFTYP , KBFIPB , KBFALX , KBFKBEID , KBFNUMLGN , KBFDESC , KBFKADID , KBFSITE , 
							KBFNTLI , KBFCP , KBFVILLE , KBFADH , KBFDATDEB , KBFDEBHEU , KBFDATFIN , KBFFINHEU , KBFMNT1 , KBFMNT2 , KBFNBEVN , 
							KBFNBPER , KBFNOM , KBFPNOM , KBFDATNAI , KBFFONC , KBFCDEC , KBFCIP , KBFACCS , KBFAVPR , KBFMSR , KBFCMAT , KBFSEX , KBFMDQ , KBFMDA , KBFACTP , 
							KBFKADFH , KBFEXT , KBFMNT3 , KBFMNT4 , KBFQUA , KBFREN , KBFRLO ) 
							( SELECT V_NEWKBFID , P_TYPE , V_CODEOFFRE , V_NEWVERS , V_NEWKBEID , KBFNUMLGN , KBFDESC , V_NEWCODEDESI , KBFSITE , 
							KBFNTLI , KBFCP , KBFVILLE , KBFADH , KBFDATDEB , KBFDEBHEU , KBFDATFIN , KBFFINHEU , KBFMNT1 , KBFMNT2 , KBFNBEVN , 
							KBFNBPER , KBFNOM , KBFPNOM , KBFDATNAI , KBFFONC , KBFCDEC , KBFCIP , KBFACCS , KBFAVPR , KBFMSR , KBFCMAT , KBFSEX , KBFMDQ , KBFMDA , KBFACTP , 
							KBFKADFH , KBFEXT , KBFMNT3 , KBFMNT4 , KBFQUA , KBFREN , KBFRLO 
							FROM KPINVED WHERE KBFID = V_CODEINVED ) ; 
						END IF ; 
					END IF ; 
				END IF ; 
			END FOR ; 
		END IF ; 
		/*----  COPIE DE KPINVAPP  ----*/ 
		IF ( P_TRAITEMENT = 'V' ) THEN 
			INSERT INTO KPINVAPP 
			( KBGTYP , KBGIPB , KBGALX , KBGNUM , KBGKBEID , KBGPERI , KBGRSQ , KBGOBJ , KBGFOR , KBGGAR ) 
			( SELECT KBGTYP , KBGIPB , P_NEWVERSION , KBGNUM , V_NEWKBEID , KBGPERI , KBGRSQ , KBGOBJ , KBGFOR , KBGGAR 
			FROM KPINVAPP WHERE KBGKBEID = V_CODEINVEN ) ; 
		ELSE 
			IF ( P_TRAITEMENT = 'P' ) THEN 
				INSERT INTO KPINVAPP 
				( KBGTYP , KBGIPB , KBGALX , KBGNUM , KBGKBEID , KBGPERI , KBGRSQ , KBGOBJ , KBGFOR , KBGGAR ) 
				( SELECT 'P' , P_CODECONTRAT , P_VERSIONCONTRAT , KBGNUM , V_NEWKBEID , KBGPERI , KBGRSQ , KBGOBJ , KBGFOR , KBGGAR 
				FROM KPINVAPP WHERE KBGKBEID = V_CODEINVEN ) ; 
			ELSE 
				IF ( P_TRAITEMENT = 'C' ) THEN 
				INSERT INTO KPINVAPP 
				( KBGTYP , KBGIPB , KBGALX , KBGNUM , KBGKBEID , KBGPERI , KBGRSQ , KBGOBJ , KBGFOR , KBGGAR ) 
				( SELECT P_TYPE , V_CODEOFFRE , V_NEWVERS , KBGNUM , V_NEWKBEID , KBGPERI , KBGRSQ , KBGOBJ , KBGFOR , KBGGAR 
				FROM KPINVAPP WHERE KBGKBEID = V_CODEINVEN ) ; 
				END IF ; 
			END IF ; 
		END IF ; 
	 
	END FOR ; 
	 
	SET P_NEWCODEINVEN = 0 ; 
END P1  ;

