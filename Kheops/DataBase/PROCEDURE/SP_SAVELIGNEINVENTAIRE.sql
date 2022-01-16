﻿CREATE PROCEDURE SP_SAVELIGNEINVENTAIRE ( 
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_CODERSQ INTEGER , 
	IN P_CODEOBJ INTEGER , 
	IN P_TYPEINVENTAIRE INTEGER , 
	IN P_CODEINVENTAIRE INTEGER , 
	IN P_CODELIGNE INTEGER , 
	IN P_NUMLGN INTEGER , 
	IN P_DESC CHAR(40) , 
	IN P_DESINATUREMARCHANDISE CHAR(40) , 
	IN P_SITE CHAR(40) , 
	IN P_NATLIEU CHAR(5) , 
	IN P_CP INTEGER , 
	IN P_VILLE CHAR(30) , 
	IN P_LIENADH INTEGER , 
	IN P_DATEDEB INTEGER , 
	IN P_HEUREDEB INTEGER , 
	IN P_DATEFIN INTEGER , 
	IN P_HEUREFIN INTEGER , 
	IN P_MNT1 DECIMAL(13, 2) , 
	IN P_MNT2 DECIMAL(13, 2) , 
	IN P_NBEVT INTEGER , 
	IN P_NBPERS INTEGER , 
	IN P_NOM CHAR(40) , 
	IN P_PNOM CHAR(40) , 
	IN P_DATENAIS INTEGER , 
	IN P_FONC CHAR(30) , 
	IN P_CAPITALDC INTEGER , 
	IN P_CAPITALIP INTEGER , 
	IN P_ACCIDENT CHAR(1) , 
	IN P_AVTPROD CHAR(1) , 
	IN P_NUMSERIE CHAR(40) , 
	IN P_CODEMAT CHAR(15) , 
	IN P_SEXE CHAR(1) , 
	IN P_MEDIQUEST CHAR(1) , 
	IN P_MEDIANTE INTEGER , 
	IN P_PROACTIVITE CHAR(1) , 
	IN P_CODEEXT CHAR(15) , 
	IN P_DESIFRANCHISE CHAR(5000) , 
	IN P_QUALITE CHAR(5) , 
	IN P_RENONCE CHAR(5) , 
	IN P_RSQLOC CHAR(1) , 
	IN P_MNT3 DECIMAL(13, 2) , 
	IN P_MNT4 DECIMAL(13, 2) , 
	IN P_MODELE CHAR(40) , 
	IN P_MARQUE CHAR(40) , 
	IN P_IMMATRICULATION CHAR(20) , 
	IN P_DESIDEPART CHAR(40) , 
	IN P_DESIDESTINATION CHAR(40) , 
	IN P_DESIMODALITE CHAR(40) , 
	IN P_PAYS CHAR(3) , 
	IN P_ADRCHR INTEGER , 
	IN P_ADRBATIMENT CHAR(32) , 
	IN P_ADRNUMVOIE CHAR(10) , 
	IN P_ADRNUMVOIE2 CHAR(15) , 
	IN P_ADREXTVOIE CHAR(1) , 
	IN P_ADRVOIE CHAR(32) , 
	IN P_ADRBP CHAR(32) , 
	IN P_ADRCP CHAR(5) , 
	IN P_ADRDEP CHAR(2) , 
	IN P_ADRVILLE CHAR(26) , 
	IN P_ADRCPX CHAR(5) , 
	IN P_ADRVILLEX CHAR(32) , 
	IN P_ADRMATHEX INTEGER ,
	IN P_ADRLATITUDE NUMERIC(10, 7) , 
	IN P_ADRLONGITUDE NUMERIC(10, 7) ,
	IN P_ISADDRESSEMPTY INTEGER ,
	OUT P_IDROWCODE INTEGER ) 
	LANGUAGE SQL 
	SPECIFIC SP_SAVELIGNEINVENTAIRE 
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
  
	DECLARE V_IDDESIFRANCHISE INTEGER DEFAULT 0 ; 
	DECLARE V_IDDESIDEPART INTEGER DEFAULT 0 ; 
	DECLARE V_IDDESIDESTINATION INTEGER DEFAULT 0 ; 
	DECLARE V_IDDESIMODALITE INTEGER DEFAULT 0 ; 
	DECLARE V_IDDESINATUREMARCHANDISE INTEGER DEFAULT 0 ; 
	DECLARE V_COUNTROW INTEGER DEFAULT 0 ; 

	DECLARE V_ACTION_YADRESS CHAR (6) DEFAULT 'UPDATE';
	DECLARE V_NUMVOIE INTEGER DEFAULT 0 ; 
	DECLARE V_CP CHAR ( 4 ) DEFAULT '' ; 
	DECLARE V_DEP CHAR ( 2 ) DEFAULT '' ; 
	DECLARE V_VOIE CHAR ( 32 ) DEFAULT '' ; 
	DECLARE V_VILLE CHAR ( 32 ) DEFAULT '' ; 
	DECLARE V_CPINT INTEGER DEFAULT 0 ; 
	DECLARE V_CPXINT INTEGER DEFAULT 0 ; 
	DECLARE V_NUMCHR CHAR ( 40 ) DEFAULT '' ;
	 
	SET V_IDDESIFRANCHISE = 0 ; 
	SET V_IDDESIDEPART = 0 ; 
	SET V_IDDESIDESTINATION = 0 ; 
	SET V_IDDESIMODALITE = 0 ; 
	SET V_IDDESINATUREMARCHANDISE = 0 ; 
	 
	SET V_COUNTROW = 0 ; 
	 
	/* SAVOIR SI LA LIGNE EXISTE OU NON */ 
	SELECT COUNT ( * ) INTO V_COUNTROW FROM KPINVED WHERE KBFID = P_CODELIGNE ; 
	 
	/* TRAITEMENT DE LA DESIGNATION DE LA FRANCHISE */ 
	IF ( ( P_TYPEINVENTAIRE = 3 OR P_TYPEINVENTAIRE = 5  OR P_TYPEINVENTAIRE = 16 ) AND TRIM ( P_DESIFRANCHISE ) != '' ) THEN 
		/* RECHERCHE DE L'ID DE LA DESIGNATION DE LA FRANCHISE */ 
		SELECT KBFKADFH INTO V_IDDESIFRANCHISE FROM KPINVED WHERE KBFID = P_CODELIGNE ; 
		 
		IF ( V_IDDESIFRANCHISE = 0 ) THEN 
			/* SI AUCUNE DESIGNATION DE FRANCHISE */ 
			CALL SP_NCHRONO ( 'KADCHR' , V_IDDESIFRANCHISE ) ; 
			INSERT INTO KPDESI 
				( KADCHR , KADTYP , KADIPB , KADALX , KADPERI , KADRSQ , KADOBJ , KADDESI ) 
			VALUES 
				( V_IDDESIFRANCHISE , P_TYPE , P_CODEOFFRE , P_VERSION , '' , P_CODERSQ , P_CODEOBJ , P_DESIFRANCHISE ) ; 
		ELSE 
			/* SI UNE DESIGNATION EXISTE */ 
			UPDATE KPDESI SET KADDESI = P_DESIFRANCHISE WHERE KADCHR = V_IDDESIFRANCHISE ; 
		END IF ; 
	END IF ;	 
  
	 
	IF ( P_TYPEINVENTAIRE = 13 ) THEN 
		IF ( TRIM ( P_DESIDEPART ) != '' ) THEN 
			SELECT KBFSIT2 INTO V_IDDESIDEPART FROM KPINVED WHERE KBFID = P_CODELIGNE ; 
			 
			IF ( V_IDDESIDEPART = 0 ) THEN 
	 
				CALL SP_NCHRONO ( 'KADCHR' , V_IDDESIDEPART ) ; 
				INSERT INTO KPDESI 
					( KADCHR , KADTYP , KADIPB , KADALX , KADPERI , KADRSQ , KADOBJ , KADDESI ) 
				VALUES 
					( V_IDDESIDEPART , P_TYPE , P_CODEOFFRE , P_VERSION , '' , P_CODERSQ , P_CODEOBJ , P_DESIDEPART ) ; 
			ELSE 
				UPDATE KPDESI SET KADDESI = P_DESIDEPART WHERE KADCHR = V_IDDESIDEPART ; 
			END IF ; 
		END IF ; 
		 
		IF ( TRIM ( P_DESIDESTINATION ) != '' ) THEN 
			SELECT KBFSIT3 INTO V_IDDESIDESTINATION FROM KPINVED WHERE KBFID = P_CODELIGNE ; 
				 
			IF ( V_IDDESIDESTINATION = 0 ) THEN 
	 
				CALL SP_NCHRONO ( 'KADCHR' , V_IDDESIDESTINATION ) ; 
				INSERT INTO KPDESI 
					( KADCHR , KADTYP , KADIPB , KADALX , KADPERI , KADRSQ , KADOBJ , KADDESI ) 
				VALUES 
					( V_IDDESIDESTINATION , P_TYPE , P_CODEOFFRE , P_VERSION , '' , P_CODERSQ , P_CODEOBJ , P_DESIDESTINATION ) ; 
			ELSE 
				UPDATE KPDESI SET KADDESI = P_DESIDESTINATION WHERE KADCHR = V_IDDESIDESTINATION ; 
			END IF ; 
		END IF ; 
		 
		IF ( TRIM ( P_DESIMODALITE ) != '' ) THEN 
			SELECT KBFDES2 INTO V_IDDESIMODALITE FROM KPINVED WHERE KBFID = P_CODELIGNE ; 
			 
			IF ( V_IDDESIMODALITE = 0 ) THEN 
	 
				CALL SP_NCHRONO ( 'KADCHR' , V_IDDESIMODALITE ) ; 
				INSERT INTO KPDESI 
					( KADCHR , KADTYP , KADIPB , KADALX , KADPERI , KADRSQ , KADOBJ , KADDESI ) 
				VALUES 
					( V_IDDESIMODALITE , P_TYPE , P_CODEOFFRE , P_VERSION , '' , P_CODERSQ , P_CODEOBJ , P_DESIMODALITE ) ; 
			ELSE 
				UPDATE KPDESI SET KADDESI = P_DESIMODALITE WHERE KADCHR = V_IDDESIMODALITE ; 
			END IF ; 
		END IF ; 
			 
			 
	END IF ;		 
  
	 
	IF ( P_TYPEINVENTAIRE = 13 OR P_TYPEINVENTAIRE = 14 ) THEN 
		IF ( TRIM ( P_DESINATUREMARCHANDISE ) != '' ) THEN 
			SELECT KBFKADID INTO V_IDDESINATUREMARCHANDISE FROM KPINVED WHERE KBFID = P_CODELIGNE ; 
			 
			IF ( V_IDDESINATUREMARCHANDISE = 0 ) THEN 
	 
				CALL SP_NCHRONO ( 'KADCHR' , V_IDDESINATUREMARCHANDISE ) ; 
				INSERT INTO KPDESI 
					( KADCHR , KADTYP , KADIPB , KADALX , KADPERI , KADRSQ , KADOBJ , KADDESI ) 
				VALUES 
					( V_IDDESINATUREMARCHANDISE , P_TYPE , P_CODEOFFRE , P_VERSION , '' , P_CODERSQ , P_CODEOBJ , P_DESINATUREMARCHANDISE ) ; 
			ELSE 
				UPDATE KPDESI SET KADDESI = P_DESINATUREMARCHANDISE WHERE KADCHR = V_IDDESINATUREMARCHANDISE ; 
			END IF ; 
		END IF ; 
	END IF ; 

	/* TRAITEMENT DE L'ADRESSE A ENREGISTRER DANS YADRESSE */ 
	IF (P_TYPEINVENTAIRE = 18) THEN
		SET V_NUMVOIE = CAST ( ('0' CONCAT TRIM(P_ADRNUMVOIE)) AS INTEGER ) ; 
		SET V_VOIE = TRIM ( TRIM ( TRIM ( P_ADRNUMVOIE ) CONCAT ' ' CONCAT TRIM ( P_ADREXTVOIE ) ) CONCAT ' ' CONCAT TRIM ( P_ADRVOIE ) ) ; 
		SET V_DEP = P_ADRDEP ;
		SET V_VILLE = P_ADRDEP CONCAT P_ADRCP CONCAT ' ' CONCAT P_ADRVILLE ; 
	
		IF ( P_ADRVOIE <> '' OR P_ADRCP <> '' OR P_ADRVILLE <> '' OR P_ADRNUMVOIE <> '' OR P_ADREXTVOIE <> '' OR P_ADRBATIMENT <> '' OR P_ADRBP <> '' OR P_ADRCPX <> '' ) THEN 
		 
			IF ( LENGTH ( TRIM ( P_ADRCP ) ) = 5 ) THEN 
				SET V_CP = SUBSTR ( P_ADRCP , 3 , 3 ) ; 
				SET V_DEP = SUBSTR ( P_ADRCP , 1 , 2 ) ; 
				SET V_CPINT = CAST ( V_CP AS INTEGER ) ; 
			ELSE 
				SET V_CPINT = CAST ( ('0' CONCAT TRIM(P_ADRCP)) AS INTEGER ) ; 
				SET V_CP = TRIM ( CAST ( V_CPINT AS CHAR (4) ) ) ; 
			END IF ;  
			 			 
			IF ( LENGTH ( TRIM ( P_ADRCPX ) ) = 5 ) THEN 
				SET V_CPXINT = CAST ( SUBSTR ( P_ADRCPX , 3 , 3 ) AS INTEGER ) ; 
				IF ( V_DEP = '' ) THEN 
					SET V_DEP = SUBSTR ( P_ADRCPX , 1 , 2 ) ; 
				END IF ; 
			ELSE 
				SET V_CPXINT = CAST ( ('0' CONCAT TRIM(P_ADRCPX)) AS INTEGER ) ; 
			END IF ; 			
			
			IF ( P_ADRCHR = 0 AND P_ISADDRESSEMPTY = 0) THEN
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
	 
		CALL SP_UPDATE_ADR ( V_ACTION_YADRESS, P_ADRNUMVOIE, P_ADRNUMVOIE2, P_ADRCP, P_ADRCPX, P_ADRCHR, V_CPINT, V_CPXINT, 
				P_ADRBATIMENT, P_ADREXTVOIE, P_ADRVOIE, P_ADRBP, P_ADRCP, V_DEP, P_ADRVILLE, V_VOIE, V_VILLE, 
				P_ADRVILLEX, P_ADRMATHEX, 0, '', '', 0, '' , 0, 0, 0, '', 0, '', '', P_ADRLATITUDE, P_ADRLONGITUDE);

		SET P_LIENADH = P_ADRCHR;
		IF (V_ACTION_YADRESS = 'DELETE') THEN
			SET P_LIENADH = 0;
		END IF;
	END IF;
	 
	IF ( V_COUNTROW = 0 ) THEN 
		/* SI LA LIGNE N'EXISTE PAS => INSERTION */ 
		CALL SP_NCHRONO ( 'KBFID' , P_CODELIGNE ) ; 
		INSERT INTO KPINVED 
			( KBFID , KBFTYP , KBFIPB , KBFALX , KBFKBEID , KBFNUMLGN , KBFDESC , KBFKADID , KBFSITE , KBFNTLI , KBFCP , KBFVILLE , KBFADH , 
				KBFDATDEB , KBFDEBHEU , KBFDATFIN , KBFFINHEU , KBFMNT1 , KBFMNT2 , KBFNBEVN , KBFNBPER , KBFNOM , KBFPNOM , KBFDATNAI , KBFFONC , KBFCDEC , 
				KBFCIP , KBFACCS , KBFAVPR , KBFMSR , KBFCMAT , KBFSEX , KBFMDQ , KBFMDA , KBFACTP , KBFEXT , KBFKADFH , 
				KBFQUA , KBFREN , KBFRLO , KBFMNT3 , KBFMNT4 , KBFMOD , KBFMRQ , KBFMIM , KBFSIT2 , KBFSIT3 , KBFDES2 , KBFPAY ) 
		VALUES 
			( P_CODELIGNE , P_TYPE , P_CODEOFFRE , P_VERSION , P_CODEINVENTAIRE , P_NUMLGN , P_DESC , V_IDDESINATUREMARCHANDISE , P_SITE , P_NATLIEU , P_CP , P_VILLE , P_LIENADH , 
				P_DATEDEB , P_HEUREDEB , P_DATEFIN , P_HEUREFIN , P_MNT1 , P_MNT2 , P_NBEVT , P_NBPERS , P_NOM , P_PNOM , P_DATENAIS , P_FONC , P_CAPITALDC , 
				P_CAPITALIP , P_ACCIDENT , P_AVTPROD , P_NUMSERIE , P_CODEMAT , P_SEXE , P_MEDIQUEST , P_MEDIANTE , P_PROACTIVITE , P_CODEEXT , V_IDDESIFRANCHISE , 
				P_QUALITE , P_RENONCE , P_RSQLOC , P_MNT3 , P_MNT4 , P_MODELE , P_MARQUE , P_IMMATRICULATION , V_IDDESIDEPART , V_IDDESIDESTINATION , V_IDDESIMODALITE , P_PAYS ) ; 
	ELSE 
		/* SI LA LIGNE EXISTE => UPDATE */ 
		UPDATE KPINVED 
			SET KBFDESC = P_DESC , KBFSITE = P_SITE , KBFCP = P_CP , KBFVILLE = P_VILLE, KBFADH = P_LIENADH , KBFDATDEB = P_DATEDEB , KBFDEBHEU = P_HEUREDEB , KBFDATFIN = P_DATEFIN , KBFFINHEU = P_HEUREFIN , 
				KBFMNT1 = P_MNT1 , KBFMNT2 = P_MNT2 , KBFNBEVN = P_NBEVT , KBFNBPER = P_NBPERS , KBFNOM = P_NOM , KBFPNOM = P_PNOM , KBFDATNAI = P_DATENAIS , KBFFONC = P_FONC , KBFCDEC = P_CAPITALDC , 
				KBFCIP = P_CAPITALIP , KBFACCS = P_ACCIDENT , KBFAVPR = P_AVTPROD , KBFMSR = P_NUMSERIE , KBFCMAT = P_CODEMAT , KBFNTLI = P_NATLIEU , KBFSEX = P_SEXE , KBFMDQ = P_MEDIQUEST , KBFMDA = P_MEDIANTE , KBFACTP = P_PROACTIVITE , KBFEXT = P_CODEEXT , KBFKADFH = V_IDDESIFRANCHISE , 
				KBFQUA = P_QUALITE , KBFREN = P_RENONCE , KBFRLO = P_RSQLOC , KBFMNT3 = P_MNT3 , KBFMNT4 = P_MNT4 , 
				 
				KBFKADID = V_IDDESINATUREMARCHANDISE , KBFMOD = P_MODELE , KBFMRQ = P_MARQUE , KBFMIM = P_IMMATRICULATION , KBFSIT2 = V_IDDESIDEPART , 
				KBFSIT3 = V_IDDESIDESTINATION , KBFDES2 = V_IDDESIMODALITE , KBFPAY = P_PAYS 
				 
				 
		WHERE KBFID = P_CODELIGNE ; 
	END IF ; 
	 
	SET P_IDROWCODE = P_CODELIGNE ; 
END P1  ; 
  

  
