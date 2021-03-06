
CREATE OR REPLACE PROCEDURE SP_SAVEOBJ ( 
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_CODERSQ INTEGER , 
	IN P_CODEOBJ INTEGER , 
	IN P_CHRONODESI INTEGER , 
	IN P_DESIGNATION CHAR(5000) , 
	IN P_ENTREEJOUR INTEGER , 
	IN P_ENTREEMOIS INTEGER , 
	IN P_ENTREEANNEE INTEGER , 
	IN P_ENTREEHEURE INTEGER , 
	IN P_SORTIEJOUR INTEGER , 
	IN P_SORTIEMOIS INTEGER , 
	IN P_SORTIEANNEE INTEGER , 
	IN P_SORTIEHEURE INTEGER , 
	IN P_VALEUR DECIMAL(11, 0) , 
	IN P_CODEUNITE CHAR(1) , 
	IN P_CODETYPE CHAR(5) , 
	IN P_VALEURHT CHAR(1) , 
	IN P_COUTM2 INTEGER , 
	IN P_CODEBRANCHE CHAR(2) , 
	IN P_CODEOBJET INTEGER , 
	IN P_DERNIEROBJET INTEGER , 
	IN P_NBOBJET INTEGER , 
	IN P_CIBLE CHAR(10) , 
	IN P_DESCRIPTIF CHAR(40) , 
	IN P_REPORTVALEUR CHAR(1) , 
	IN P_REPORTOBLIG CHAR(1) , 
	IN P_INSADR CHAR(1) , 
	IN P_ADRBATIMENT CHAR(32) , 
	IN P_ADRNUMVOIE CHAR(10) , 
	IN P_ADRNUMVOIE2 CHAR(15) , 
	IN P_ADREXTVOIE CHAR(1) , 
	IN P_ADRNOMVOIE CHAR(32) , 
	IN P_ADRBP CHAR(32) , 
	IN P_ADRCP CHAR(5) , 
	IN P_ADRDEP CHAR(2) , 
	IN P_ADRVILLE CHAR(26) , 
	IN P_ADRCPX CHAR(5) , 
	IN P_ADRVILLEX CHAR(32) , 
	IN P_ADRMATHEX INTEGER , 
	IN P_ADRNUMCHRONO INTEGER , 
	IN P_APE CHAR(5) , 
	IN P_NOMENCLATURE1 CHAR(15) , 
	IN P_NOMENCLATURE2 CHAR(15) , 
	IN P_NOMENCLATURE3 CHAR(15) , 
	IN P_NOMENCLATURE4 CHAR(15) , 
	IN P_NOMENCLATURE5 CHAR(15) , 
	IN P_TERRITORIALITE CHAR(3) , 
	IN P_TRE CHAR(5) , 
	IN P_CLASSE CHAR(2) , 
	IN P_TYPERISQUE CHAR(15) , 
	IN P_TYPEMATERIEL CHAR(15) , 
	IN P_NATURELIEUX CHAR(15) , 
	IN P_DATEENTREEDESC INTEGER , 
	IN P_HEUREENTREEDESC INTEGER , 
	IN P_DATESORTIEDESC INTEGER , 
	IN P_HEURESORTIEDESC INTEGER , 
	IN P_MODAVENANTLOCALE CHAR(1) , 
	IN P_DATEEFFETAVNLOCALANNEE INTEGER , 
	IN P_DATEEFFETAVNLOCALMOIS INTEGER , 
	IN P_DATEEFFETAVNLOCALJOUR INTEGER , 
	IN P_ISRISQUETEMPORAIRE CHAR(1) , 
	IN P_DATESYSTEM NUMERIC(10, 0) , 
	IN P_HEURESYSTEM NUMERIC(6, 0) , 
	IN P_USER CHAR(10) , 
	IN P_ISADDRESSEMPTY INTEGER , 
	IN P_LATITUDE NUMERIC(10, 7) , 
	IN P_LONGITUDE NUMERIC(10, 7) , 
	IN P_DATEMISEAJOUR INTEGER , 
	IN P_DATEMODIFAVNLOCALANNEE INTEGER , 
	IN P_DATEMODIFAVNLOCALMOIS INTEGER , 
	IN P_DATEMODIFAVNLOCALJOUR INTEGER , 
	OUT P_OUTCODERSQOBJ CHAR(50) ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_SAVEOBJ
	NOT DETERMINISTIC 
	MODIFIES SQL DATA 
	CALLED ON NULL INPUT 
	SET OPTION  ALWBLK = *ALLREAD , 
	ALWCPYDTA = *OPTIMIZE , 
	COMMIT = *CHG , 
	DECRESULT = (31, 31, 00) , 
	DFTRDBCOL = ZALBINKHEO , 
	DYNDFTCOL = *YES , 
	SQLPATH = 'ZALBINKHEO, ZALBINKMOD' , 
	DYNUSRPRF = *USER , 
	SRTSEQ = *HEX   
	P1 : BEGIN ATOMIC 
  
	DECLARE V_CHRONODESI INTEGER DEFAULT 0 ; 
	DECLARE V_COUNTOBJ INTEGER DEFAULT 0 ; 
	DECLARE V_COUNTCREATEOBJ INTEGER DEFAULT 0 ; 
	DECLARE V_MONOOBJ INTEGER DEFAULT 0 ; 
	DECLARE V_LASTOBJ INTEGER DEFAULT 0 ; 
	DECLARE V_COUNTDESI INTEGER DEFAULT 0 ; 
  
	DECLARE V_ADRVOIE CHAR ( 32 ) DEFAULT '' ; 
	DECLARE V_ADRVILLE CHAR ( 32 ) DEFAULT '' ; 
	 
  
	 
  
	DECLARE V_MAJ INTEGER DEFAULT 0 ; 
  
	DECLARE V_OLDVALUE INTEGER DEFAULT 0 ; 
	DECLARE V_OLDUNITE CHAR ( 1 ) DEFAULT '' ; 
	DECLARE V_OLDTYPE CHAR ( 5 ) DEFAULT '' ; 
	DECLARE V_CONTROLASS INTEGER DEFAULT 0 ; 
  
	 
	DECLARE V_CALCULPAR INTEGER DEFAULT 0 ; 
  
	DECLARE V_COUNTRSQ INTEGER DEFAULT 0 ; 
  
	 
  
	DECLARE V_CODENOMENCLATURE1 CHAR ( 15 ) DEFAULT '' ; 
	DECLARE V_CODENOMENCLATURE2 CHAR ( 15 ) DEFAULT '' ; 
	DECLARE V_CODENOMENCLATURE3 CHAR ( 15 ) DEFAULT '' ; 
	DECLARE V_CODENOMENCLATURE4 CHAR ( 15 ) DEFAULT '' ; 
	DECLARE V_CODENOMENCLATURE5 CHAR ( 15 ) DEFAULT '' ; 
  
	DECLARE V_NUMAVN INTEGER DEFAULT 0 ; 
	DECLARE V_DATEAVN INTEGER DEFAULT 0 ; 
  
	DECLARE V_BRANCHE CHAR ( 2 ) DEFAULT '' ; 
	DECLARE V_SOUSBRANCHE CHAR ( 3 ) DEFAULT '' ; 
	DECLARE V_CATEGORIE CHAR ( 5 ) DEFAULT '' ; 
  
	DECLARE V_COUNT INTEGER DEFAULT 0 ; 
	DECLARE V_NEWAPP INTEGER DEFAULT 0 ; 
  
	DECLARE V_MODIFFORMULE INTEGER DEFAULT 0 ; 
	DECLARE V_INDIVO DECIMAL ( 7 , 2 ) DEFAULT 0 ; 
  
	 
	SET P_CODEOFFRE = LPAD ( TRIM ( P_CODEOFFRE ) , 9 , ' ' ) ; 
	SET V_MODIFFORMULE = 0 ; 
  
	SET V_CALCULPAR = 1 ; 
  
	SET V_MONOOBJ = 0 ; 
	SET V_LASTOBJ = 0 ; 
	/* 20160218 : CHANGEMENT DE L'HEURE DE FIN SI = 0 */ 
	IF ( P_SORTIEHEURE = 0 AND P_SORTIEANNEE != 0 AND ( P_ENTREEANNEE * 10000 + P_ENTREEMOIS * 100 + P_ENTREEJOUR ) = ( P_SORTIEANNEE * 10000 + P_SORTIEMOIS * 100 + P_SORTIEJOUR ) ) THEN 
		SET P_SORTIEHEURE = 2359 ; 
	END IF ; 
	 --RÉCUPÉRATION DE LA BRANCHE, SOUS BRANCHE, CATÉGORIE DE LA CIBLE 
	SELECT KAIBRA , KAISBR , KAICAT INTO V_BRANCHE , V_SOUSBRANCHE , V_CATEGORIE FROM KCIBLEF WHERE KAICIBLE = TRIM ( P_CIBLE ) ; 
  
	 --ENREGISTREMENT SI CRÉATION D'OBJET 
	SELECT PBAVN , PBAVA * 10000 + PBAVM * 100 + PBAVJ INTO V_NUMAVN , V_DATEAVN FROM YPOBASE WHERE PBIPB = P_CODEOFFRE AND PBALX = P_VERSION AND PBTYP = P_TYPE ; 
  
	SET V_ADRVOIE = TRIM ( TRIM ( TRIM ( P_ADRNUMVOIE ) CONCAT ' ' CONCAT TRIM ( P_ADREXTVOIE ) ) CONCAT ' ' CONCAT TRIM ( P_ADRNOMVOIE ) ) ; 
	SET V_ADRVILLE = TRIM ( P_ADRDEP ) CONCAT P_ADRCP CONCAT ' ' CONCAT TRIM ( P_ADRVILLE ) ; 
  
	 
  
	SELECT COUNT ( * ) INTO V_COUNTOBJ 
	FROM KPOBJ 
	WHERE KACIPB = P_CODEOFFRE AND KACALX = P_VERSION AND KACTYP = P_TYPE AND KACRSQ = P_CODERSQ AND KACOBJ <> P_CODEOBJ ; 
	 -->PERMET DE SAVOIR SI IL N'Y A QU'UN SEUL OBJET OU PAS : 
	 --		SI V_COUNTOBJ = 0 => UN SEUL OBJET, SINON PLUSIEURS OBJETS (INCLUANT CELUI EN MODIFICATION OU CRÉATION) 
	IF ( P_CODERSQ = 0 OR V_COUNTOBJ = 0 ) THEN  --ENREGISTREMENT DU RISQUE SI MONO OBJET (VOIR COMMENTAIRE CI-DESSUS POUR LA RAISON DU 0) 
		IF ( P_DATEEFFETAVNLOCALANNEE = 0 AND P_DATEEFFETAVNLOCALMOIS = 0 AND P_DATEEFFETAVNLOCALJOUR = 0 ) THEN 
			SET P_DATEEFFETAVNLOCALJOUR = P_ENTREEJOUR ; 
			SET P_DATEEFFETAVNLOCALMOIS = P_ENTREEMOIS ; 
			SET P_DATEEFFETAVNLOCALANNEE = P_ENTREEANNEE ; 
		END IF ; 
		CALL SP_SAVERSQ ( P_CODEOFFRE , P_VERSION , P_TYPE , P_CODERSQ , P_CHRONODESI , P_DESIGNATION , 
		P_ENTREEJOUR , P_ENTREEMOIS , P_ENTREEANNEE , P_ENTREEHEURE , P_SORTIEJOUR , P_SORTIEMOIS , P_SORTIEANNEE , P_SORTIEHEURE , 
		P_VALEUR , P_CODEUNITE , P_CODETYPE , P_VALEURHT , P_CODEBRANCHE , P_CODEOBJET , P_DERNIEROBJET , P_NBOBJET , P_CIBLE , 
		P_DESCRIPTIF , P_REPORTVALEUR , P_REPORTOBLIG , P_APE , P_NOMENCLATURE1 , P_NOMENCLATURE2 , P_NOMENCLATURE3 , P_NOMENCLATURE4 , P_NOMENCLATURE5 , P_TERRITORIALITE , P_TRE , P_CLASSE , 
		P_TYPERISQUE , P_TYPEMATERIEL , P_NATURELIEUX , P_DATEENTREEDESC , P_HEUREENTREEDESC , P_DATESORTIEDESC , P_HEURESORTIEDESC , 
		P_ADRBATIMENT , P_ADRNUMVOIE , P_ADRNUMVOIE2 , P_ADREXTVOIE , P_ADRNOMVOIE , P_ADRBP , CAST ( ( '0' CONCAT P_ADRCP ) AS INTEGER ) , P_ADRDEP , P_ADRCP , P_ADRVILLE , V_ADRVOIE , 
		V_ADRVILLE , P_ADRCPX , P_ADRVILLEX , P_ADRMATHEX , 
		P_MODAVENANTLOCALE , P_DATEEFFETAVNLOCALANNEE , P_DATEEFFETAVNLOCALMOIS , P_DATEEFFETAVNLOCALJOUR , P_ISRISQUETEMPORAIRE , P_DATESYSTEM , P_HEURESYSTEM , P_USER , P_ISADDRESSEMPTY , 
		P_LATITUDE , P_LONGITUDE , 
		P_CODERSQ ) ; 
	END IF ; 
	 
	IF ( P_CODEOBJ = 0 ) THEN 
		SELECT ( IFNULL ( MAX ( JGOBJ ) , 0 ) + 1 ) INTO P_CODEOBJ 
		FROM YPRTOBJ 
		WHERE JGIPB = P_CODEOFFRE AND JGALX = P_VERSION AND JGRSQ = P_CODERSQ ; 
  
		/* AJOUT LE 2016-02-01 : CHANGEMENT DU S'APPLIQUE À SI AJOUT D'UN NOUVEL OBJET ET QUE LE RISQUE EST MONO-FORMULE */ 
		IF ( V_NUMAVN > 0 ) THEN 
			SELECT COUNT ( * ) INTO V_COUNT FROM KPOPTAP WHERE KDDIPB = P_CODEOFFRE AND KDDALX = P_VERSION AND KDDTYP = P_TYPE AND KDDRSQ = P_CODERSQ AND KDDPERI = 'RQ' ; 
			IF ( V_COUNT = 1 ) THEN 
				FOR LOOP_APPOBJ AS FREE_LIST CURSOR FOR 
					SELECT KACOBJ CODEOBJ FROM KPOBJ WHERE KACIPB = P_CODEOFFRE AND KACALX = P_VERSION AND KACTYP = P_TYPE AND KACRSQ = P_CODERSQ 
				DO 
					SET V_NEWAPP = 0 ; 
  
					CALL SP_NCHRONO ( 'KDDID' , V_NEWAPP ) ; 
					INSERT INTO KPOPTAP ( KDDID , KDDTYP , KDDIPB , KDDALX , KDDFOR , KDDOPT , KDDKDBID , KDDPERI , KDDRSQ , KDDOBJ , KDDINVEN , KDDINVEP , KDDCRU , KDDCRD , KDDMAJU , KDDMAJD ) 
					( SELECT V_NEWAPP , KDDTYP , KDDIPB , KDDALX , KDDFOR , KDDOPT , KDDKDBID , 'OB' , KDDRSQ , CODEOBJ , KDDINVEN , KDDINVEP , KDDCRU , KDDCRD , KDDMAJU , KDDMAJD 
					FROM KPOPTAP WHERE KDDIPB = P_CODEOFFRE AND KDDALX = P_VERSION AND KDDTYP = P_TYPE AND KDDRSQ = P_CODERSQ AND KDDPERI = 'RQ' ) ; 
  
				END FOR ; 
				DELETE FROM KPOPTAP WHERE KDDIPB = P_CODEOFFRE AND KDDALX = P_VERSION AND KDDTYP = P_TYPE AND KDDRSQ = P_CODERSQ AND KDDPERI = 'RQ' ; 
			END IF ; 
		END IF ; 
	END IF ; 
  
	/*        
		BLOC ADRESSE REFACTORISER        
	*/ 
  
	 --END IF ; 
	SELECT COUNT ( * ) INTO V_COUNTDESI FROM KPDESI WHERE KADCHR = P_CHRONODESI ; 
  
	IF ( P_CHRONODESI = 0 OR V_COUNTDESI = 0 ) THEN 
		CALL SP_NCHRONO ( 'KADCHR' , P_CHRONODESI ) ; 
  
		INSERT INTO KPDESI 
			( KADIPB , KADALX , KADCHR , KADTYP , KADRSQ , KADOBJ , KADDESI ) 
		VALUES 
			( P_CODEOFFRE , P_VERSION , P_CHRONODESI , P_TYPE , P_CODERSQ , P_CODEOBJ , P_DESIGNATION ) ; 
	ELSE 
		UPDATE KPDESI 
			SET KADDESI = P_DESIGNATION 
		WHERE KADCHR = P_CHRONODESI ; 
	END IF ; 
  
	SET V_CHRONODESI = P_CHRONODESI ; 
  
	SELECT COUNT ( * ) INTO V_COUNTCREATEOBJ 
		FROM YPRTOBJ 
		WHERE JGIPB = P_CODEOFFRE AND JGALX = P_VERSION AND JGRSQ = P_CODERSQ AND JGOBJ = P_CODEOBJ ; 
  
	SELECT JDIVO INTO V_INDIVO FROM YPRTENT WHERE JDIPB = P_CODEOFFRE AND JDALX = P_VERSION ; 
	IF ( V_COUNTCREATEOBJ > 0 ) THEN 
		SET V_MAJ = 1 ; 
  
		 --IF ( LOCATE ( 'CNVA' , P_CODEOFFRE ) = 0 ) THEN 
		 -- RECHERCHE SI ENREGISTREMENT EXISTE DANS KPCTRLA 
		SELECT COUNT ( * ) INTO V_CONTROLASS FROM KPCTRLA WHERE KGTTYP = P_TYPE AND KGTIPB = P_CODEOFFRE AND KGTALX = P_VERSION ; 
  
		 -- RÉCUPÉRATION DES ANCIENNES DONNÉES DE L'OBJET 
		SELECT JGVAA , JGVAU , JGVAT INTO V_OLDVALUE , V_OLDUNITE , V_OLDTYPE 
			FROM YPRTOBJ 
			WHERE JGIPB = P_CODEOFFRE AND JGALX = P_VERSION AND JGRSQ = P_CODERSQ AND JGOBJ = P_CODEOBJ ; 
  
		 -- SI UNE DES DONNÉES A CHANGÉ => SUPPRESSION ENREGISTREMENT KPCTRLE ; INSERTION ENREGISTREMENT KPCTRLA 
		IF ( ( V_OLDVALUE <> P_VALEUR OR V_OLDUNITE <> P_CODEUNITE OR V_OLDTYPE <> P_CODETYPE OR V_NUMAVN > 0 ) AND V_CONTROLASS = 0 ) THEN 
			SELECT COUNT ( * ) INTO V_COUNT FROM KPCTRLE WHERE KEVTYP = P_TYPE AND KEVIPB = P_CODEOFFRE AND KEVALX = P_VERSION AND KEVETAPE = 'COT' ; 
			IF ( V_COUNT > 0 ) THEN 
				DELETE FROM KPCTRLE WHERE KEVTYP = P_TYPE AND KEVIPB = P_CODEOFFRE AND KEVALX = P_VERSION AND KEVETAPE = 'COT' ; 
				INSERT INTO KPCTRLA ( KGTTYP , KGTIPB , KGTALX , KGTETAPE , KGTLIB ) VALUES ( P_TYPE , P_CODEOFFRE , P_VERSION , 'RSQ' , 'VALEUR' ) ; 
				SET V_CALCULPAR = 1 ; 
			END IF ; 
		END IF ; 
		IF ( V_CONTROLASS > 0 ) THEN 
			SET V_CALCULPAR = 1 ; 
		END IF ; 
  
		 --END IF ; 
		UPDATE YPRTOBJ 
			SET JGVDJ = P_ENTREEJOUR , JGVDM = P_ENTREEMOIS , JGVDA = P_ENTREEANNEE , JGVDH = P_ENTREEHEURE , 
				JGVFJ = P_SORTIEJOUR , JGVFM = P_SORTIEMOIS , JGVFA = P_SORTIEANNEE , JGVFH = P_SORTIEHEURE , 
				JGVAL = P_VALEUR , JGVAA = P_VALEUR , JGVAU = P_CODEUNITE , JGVAT = P_CODETYPE , JGVAH = P_VALEURHT , JGTRR = P_TERRITORIALITE , 
				JGBRA = V_BRANCHE , JGSBR = V_SOUSBRANCHE , JGCAT = V_CATEGORIE , JGTEM = P_ISRISQUETEMPORAIRE , JGIVO = V_INDIVO , JGIVA = V_INDIVO , 
				JGAVA = P_DATEMODIFAVNLOCALANNEE , JGAVJ = P_DATEMODIFAVNLOCALJOUR , JGAVM = P_DATEMODIFAVNLOCALMOIS 
				 
		WHERE JGIPB = P_CODEOFFRE AND JGALX = P_VERSION AND JGRSQ = P_CODERSQ AND JGOBJ = P_CODEOBJ ; 
	ELSE 
		SET V_MAJ = 0 ; 
		INSERT INTO YPRTOBJ 
			( JGIPB , JGALX , JGRSQ , JGOBJ , JGCCH , JGVDJ , JGVDM , JGVDA , JGVDH , 
				JGVFJ , JGVFM , JGVFA , JGVFH , JGVAL , JGVAA , JGVAU , JGVAT , JGVAH , JGTRR , JGBRA , JGSBR , JGCAT , JGTEM , JGIVO , JGIVA ) 
		VALUES 
			( P_CODEOFFRE , P_VERSION , P_CODERSQ , P_CODEOBJ , P_CODEOBJ , P_ENTREEJOUR , P_ENTREEMOIS , P_ENTREEANNEE , P_ENTREEHEURE , 
				P_SORTIEJOUR , P_SORTIEMOIS , P_SORTIEANNEE , P_SORTIEHEURE , P_VALEUR , P_VALEUR , P_CODEUNITE , P_CODETYPE , P_VALEURHT , P_TERRITORIALITE , V_BRANCHE , V_SOUSBRANCHE , V_CATEGORIE , P_ISRISQUETEMPORAIRE , V_INDIVO , V_INDIVO ) ; 
	END IF ; 
  
	SELECT COUNT ( * ) INTO V_COUNTOBJ 
		FROM KPOBJ 
		WHERE KACIPB = P_CODEOFFRE AND KACALX = P_VERSION AND KACTYP = P_TYPE AND KACRSQ = P_CODERSQ AND KACOBJ = P_CODEOBJ ; 
  
  
	IF ( V_COUNTOBJ > 0 ) THEN 
		UPDATE KPOBJ 
			SET KACCIBLE = P_CIBLE , KACDESC = P_DESCRIPTIF , KACDESI = V_CHRONODESI , KACAPE = P_APE , 
				KACTRE = P_TRE , KACCLASS = P_CLASSE , KACMAND = P_DATEENTREEDESC , KACMANF = P_DATESORTIEDESC , 
				KACMANDH = P_HEUREENTREEDESC , KACMANFH = P_HEURESORTIEDESC , 
				KACNMC01 = P_NOMENCLATURE1 , KACNMC02 = P_NOMENCLATURE2 , KACNMC03 = P_NOMENCLATURE3 , KACNMC04 = P_NOMENCLATURE4 , KACNMC05 = P_NOMENCLATURE5 , KACCM2 = P_COUTM2 
			WHERE KACIPB = P_CODEOFFRE AND KACALX = P_VERSION AND KACTYP = P_TYPE AND KACRSQ = P_CODERSQ AND KACOBJ = P_CODEOBJ ; 
	ELSE 
		INSERT INTO KPOBJ 
			( KACIPB , KACALX , KACTYP , KACRSQ , KACOBJ , KACCIBLE , KACDESC , 
				KACDESI , KACAPE , KACNMC01 , KACNMC02 , KACNMC03 , KACNMC04 , KACNMC05 , KACCM2 , KACTRE , KACCLASS , KACMAND , KACMANF , 
				KACMANDH , KACMANFH ) 
		VALUES 
			( P_CODEOFFRE , P_VERSION , P_TYPE , P_CODERSQ , P_CODEOBJ , P_CIBLE , P_DESCRIPTIF , 
				V_CHRONODESI , P_APE , P_NOMENCLATURE1 , P_NOMENCLATURE2 , P_NOMENCLATURE3 , P_NOMENCLATURE4 , P_NOMENCLATURE5 , P_COUTM2 , P_TRE , P_CLASSE , P_DATEENTREEDESC , P_DATESORTIEDESC , 
				P_HEUREENTREEDESC , P_HEURESORTIEDESC) ; 
	END IF ; 
	 --MODIFICATION MODE AVENANT 
	IF ( P_MODAVENANTLOCALE = 'O' ) THEN 
  
		IF ( V_COUNTCREATEOBJ = 0 ) THEN 
			UPDATE YPRTOBJ 
			SET JGAVE = V_NUMAVN 
			WHERE JGIPB = P_CODEOFFRE AND JGALX = P_VERSION AND JGRSQ = P_CODERSQ AND JGOBJ = P_CODEOBJ ; 
		ELSE 
			UPDATE YPRTOBJ 
			SET JGAVF = V_NUMAVN 
			WHERE JGIPB = P_CODEOFFRE AND JGALX = P_VERSION AND JGRSQ = P_CODERSQ AND JGOBJ = P_CODEOBJ ; 
		END IF ; 
  
	END IF ; 
  
  
	SELECT COUNT ( * ) INTO V_COUNTOBJ 
	FROM YPRTOBJ 
	WHERE JGIPB = P_CODEOFFRE AND JGALX = P_VERSION AND JGRSQ = P_CODERSQ ; 
  
	IF ( V_COUNTOBJ = 1 ) THEN 
		SET V_MONOOBJ = P_CODEOBJ ; 
		 UPDATE YPRTRSQ 
		   SET JEAVA = P_DATEMODIFAVNLOCALANNEE , JEAVM = P_DATEMODIFAVNLOCALMOIS , JEAVJ = P_DATEMODIFAVNLOCALJOUR 
		  WHERE JEIPB = P_CODEOFFRE AND JEALX = P_VERSION AND JERSQ = P_CODERSQ ; 

	END IF ; 
	SELECT MAX ( JGOBJ ) INTO V_LASTOBJ FROM YPRTOBJ WHERE JGIPB = P_CODEOFFRE AND JGALX = P_VERSION AND JGRSQ = P_CODERSQ ; 
  
	UPDATE YPRTRSQ 
	SET JEOBJ = V_MONOOBJ , JEDRO = V_LASTOBJ , JENBO = V_COUNTOBJ 
	WHERE JEIPB = P_CODEOFFRE AND JEALX = P_VERSION AND JERSQ = P_CODERSQ ; 
  
	IF ( V_COUNTOBJ = 2 AND V_MAJ = 0 AND P_MODAVENANTLOCALE != 'O' ) THEN 
		UPDATE KPRSQ 
			SET KABDESI = 0 , KABMAND = 0 , KABMANF = 0 , KABNMC02 = '' , KABNMC03 = '' , KABNMC04 = '' , KABNMC05 = '' 
			WHERE KABIPB = P_CODEOFFRE AND KABALX = P_VERSION AND KABTYP = P_TYPE AND KABRSQ = P_CODERSQ ; 
			 -- , KABNMC01 = '' , KABNMC02 = '' , KABDESC = '' 
		/* 20160428 : VU AVEC FDU, ON NE RÉINITIALISE PLUS LES DATES DU RISQUES */ 
		/* 20171031 : MAINTENANT ON RÉINITIALISE LES PERIODES DU RISQUES */ 
		UPDATE YPRTRSQ 
			SET JEVDJ = 0 , JEVDM = 0 , JEVDA = 0 , JEVDH = 0 , JEVFJ = 0 , JEVFM = 0 , JEVFA = 0 , JEVFH = 0 
			WHERE JEIPB = P_CODEOFFRE AND JEALX = P_VERSION AND JERSQ = P_CODERSQ ; 
	END IF ; 
  
	SET P_OUTCODERSQOBJ = P_CODERSQ CONCAT '_' CONCAT P_CODEOBJ ; 
	/*           -- SI MONO-OBJET => RÉPERCUSSION DE L'ADRESSE DANS LE RISQUE --  */ 
	SELECT COUNT ( * ) INTO V_COUNTOBJ FROM YPRTOBJ WHERE JGIPB = P_CODEOFFRE AND JGALX = P_VERSION AND JGRSQ = P_CODERSQ ; 
	SELECT COUNT ( * ) INTO V_COUNTRSQ FROM YPRTRSQ WHERE JEIPB = P_CODEOFFRE AND JEALX = P_VERSION ; 
	 
	/*        
		BLOC ADRESSE REFACTORISER        
	*/ 
	CALL SP_SAVEOBJ_ADR ( P_CODEOFFRE , P_VERSION , P_TYPE , P_CODERSQ , V_COUNTRSQ , P_CODEOBJ , V_COUNTOBJ , P_INSADR , P_ADRBATIMENT , P_ADRNUMVOIE , P_ADRNUMVOIE2 , P_ADREXTVOIE , P_ADRNOMVOIE , P_ADRBP , P_ADRCP , P_ADRDEP , P_ADRVILLE , P_ADRCPX , P_ADRVILLEX , P_ADRMATHEX , P_ADRNUMCHRONO , P_ISADDRESSEMPTY , P_LATITUDE , P_LONGITUDE ) ; 
	 
	IF ( V_COUNTOBJ = 1 AND V_COUNTRSQ = 1 ) THEN 
		UPDATE KPENT SET KAACIBLE = P_CIBLE WHERE KAATYP = P_TYPE AND KAAIPB = P_CODEOFFRE AND KAAALX = P_VERSION ; 
	END IF ; 
  
	P2 : BEGIN ATOMIC  -- TRAITEMENT DU CALCUL D'ASSIETTE STANDARD D'UNE FORMULE (KPOPT) 
		DECLARE V_VALEUR DECIMAL ( 19 , 0 ) DEFAULT 0 ; 
		DECLARE V_UNIT CHAR ( 1 ) DEFAULT '' ; 
		DECLARE V_TYPE CHAR ( 5 ) DEFAULT '' ; 
		DECLARE V_HT CHAR ( 1 ) DEFAULT '' ; 
		DECLARE V_ENSTYPE CHAR ( 5 ) DEFAULT '' ; 
  
		DECLARE V_COUNTAPP INTEGER DEFAULT 0 ; 
  
		DECLARE V_CALCULOK INTEGER DEFAULT 0 ; 
		DECLARE V_DATEAVNRSQ INTEGER DEFAULT 0 ; 
		DECLARE V_DATEAVNOPT INTEGER DEFAULT 0 ; 
		DECLARE V_NUMAVNOPT INTEGER DEFAULT 0 ; 
  
		DECLARE V_YEARAVN INTEGER DEFAULT 0 ; 
		DECLARE V_MONTHAVN INTEGER DEFAULT 0 ; 
		DECLARE V_DAYAVN INTEGER DEFAULT 0 ; 
		 
		SET V_CALCULOK = 1 ; 
		FOR LOOP_RSQ AS FREE_LIST CURSOR FOR 
			SELECT KDDFOR V_CODEFORMULE , KDDOPT V_CODEOPTION 
				FROM KPOPTAP 
				WHERE KDDIPB = P_CODEOFFRE AND KDDALX = P_VERSION AND KDDTYP = P_TYPE AND KDDRSQ = P_CODERSQ 
			GROUP BY KDDFOR , KDDOPT 
		DO 
			SET V_MODIFFORMULE = 0 ; 
			SET V_DATEAVNRSQ = 0 ; 
			SELECT ( JEAVA * 10000 + JEAVM * 100 + JEAVJ ) INTO V_DATEAVNRSQ FROM YPRTRSQ WHERE JEIPB = P_CODEOFFRE AND JEALX = P_VERSION AND JERSQ = P_CODERSQ ; 
  
  
			 --COMPTE LE NOMBRE D'ENREGISTREMENT RQ 
			SELECT COUNT ( * ) INTO V_COUNTAPP FROM KPOPTAP WHERE KDDIPB = P_CODEOFFRE AND KDDALX = P_VERSION AND KDDTYP = P_TYPE AND KDDFOR = V_CODEFORMULE AND KDDOPT = V_CODEOPTION AND KDDPERI = 'RQ' ; 
  
			IF ( V_COUNTAPP > 0 ) THEN 
				SELECT JEVAL , JEVAU , JEVAT INTO V_VALEUR , V_UNIT , V_TYPE FROM YPRTRSQ WHERE JEIPB = P_CODEOFFRE AND JEALX = P_VERSION AND JERSQ = P_CODERSQ ; 
			END IF ; 
  
			 --COMPTE LE NOMBRE D'ENREGISTREMENT OB 
			SELECT COUNT ( * ) INTO V_COUNTAPP FROM KPOPTAP WHERE KDDIPB = P_CODEOFFRE AND KDDALX = P_VERSION AND KDDTYP = P_TYPE AND KDDFOR = V_CODEFORMULE AND KDDOPT = V_CODEOPTION AND KDDPERI = 'OB' ; 
  
			IF ( V_COUNTAPP > 0 ) THEN 
  
				FOR LOOP_OBJ AS FREE_LIST CURSOR FOR 
					SELECT JGVAL VALEUR , JGVAU UNIT , JGVAT TYPE , JGVAH HT , KGMTYVAL ENSTYP 
						FROM YPRTOBJ 
							INNER JOIN KPOPTAP ON JGIPB = KDDIPB AND JGALX = KDDALX AND JGRSQ = KDDRSQ AND JGOBJ = KDDOBJ AND KDDFOR = V_CODEFORMULE AND KDDOPT = V_CODEOPTION AND KDDPERI = 'OB' 
							LEFT JOIN KTYPVALD ON KGMBASE = JGVAT 
						WHERE JGIPB = P_CODEOFFRE AND JGALX = P_VERSION AND JGRSQ = P_CODERSQ 
							AND ( ( ( ( JGVDA * 10000 + JGVDM * 100 + JGVDJ ) >= V_DATEAVN OR ( JGVDA * 10000 + JGVDM * 100 + JGVDJ ) = 0 ) 
								AND ( ( JGVFA * 10000 + JGVFM * 100 + JGVFJ ) >= V_DATEAVN OR ( JGVFA * 10000 + JGVFM * 100 + JGVFJ ) = 0 ) ) 
							OR ( V_DATEAVN = 0 ) ) 
				DO 
					IF ( ( V_ENSTYPE = '' OR V_ENSTYPE = ENSTYP ) AND ( V_UNIT = '' OR V_UNIT = UNIT ) AND ( V_HT = '' OR V_HT = HT ) AND ( V_CALCULOK > 0 ) ) THEN 
						SET V_VALEUR = V_VALEUR + VALEUR ; 
						SET V_UNIT = UNIT ; 
						SET V_TYPE = TYPE ; 
						SET V_HT = HT ; 
						SET V_ENSTYPE = ENSTYP ; 
					ELSE 
						SET V_CALCULOK = 0 ; 
						SET V_VALEUR = 0 ; 
						SET V_UNIT = '' ; 
						SET V_TYPE = '' ; 
						SET V_HT = '' ; 
						SET V_ENSTYPE = '' ; 
					END IF ; 
				END FOR ; 
			END IF ; 
  
			IF ( V_CALCULPAR > 0 ) THEN 
				UPDATE KPOPT SET KDBASVALO = V_VALEUR , KDBASVALA = V_VALEUR , KDBASVALW = 0 , KDBASUNIT = V_UNIT , KDBASBASE = V_TYPE 
					WHERE KDBIPB = P_CODEOFFRE AND KDBALX = P_VERSION AND KDBTYP = P_TYPE AND KDBFOR = V_CODEFORMULE AND KDBOPT = V_CODEOPTION ; 
				P3 : BEGIN ATOMIC  -- TRAITEMENT DU CALCUL D'ASSIETTE DE GARANTIE SANS INVENTAIRE (KPGARAN) 
					DECLARE V_NATAPP CHAR ( 1 ) DEFAULT '' ; 
					DECLARE V_COUNTGAR INTEGER DEFAULT 0 ; 
					DECLARE V_COUNTENS INTEGER DEFAULT 0 ; 
  
					FOR LOOP_GARAN AS FREE_LIST CURSOR FOR 
						SELECT KDEID IDGAR , KGKTYVAL ENSGAR , KDEGARAN GARANTIE , KDEASBASE ASSBASE 
							FROM KPGARAN 
								INNER JOIN KGARTVL ON KGKGAR = KDEGARAN 
							WHERE KDEIPB = P_CODEOFFRE AND KDEALX = P_VERSION AND KDETYP = P_TYPE AND KDEFOR = V_CODEFORMULE AND KDEOPT = V_CODEOPTION 
								AND ( KDEALA = 'A' OR KDEALA = 'B' )  --AND KDEINVSP <> 'O' 
					DO 
						SET V_NATAPP = '' ; 
						SET V_COUNTGAR = 0 ; 
						SET V_COUNTENS = 0 ; 
						SET V_VALEUR = 0 ; 
						SET V_UNIT = '' ; 
						SET V_TYPE = '' ; 
						SET V_HT = '' ; 
						SET V_ENSTYPE = '' ; 
  
						 -- RÉCUPÉRATION DE LA NATURE DU S'APPLIQUE À POUR LA GARANTIE 
						SELECT DISTINCT KDFGAN INTO V_NATAPP FROM KPGARAP WHERE KDFKDEID = IDGAR ; 
						SET V_CALCULOK = 1 ; 
						 -- V_NATAPP => A = ACCORDÉE ; E = EXCLUE ; '' = AUCUNE APPLICATION 
						CASE V_NATAPP 
							WHEN 'A' THEN  -- LORSQUE LA GARANTIE EST ACCORDÉE DANS LA TABLE KPGARAP 
								 -- RÉCUPÉRATION DE TOUS LES OBJETS ET ADDITION DE LEUR VALEUR QUI SONT PRÉSENTS DANS KPGARAP 
								FOR LOOP_OBET AS FREE_LIST CURSOR FOR 
									SELECT JGVAL VALEUR , JGVAU UNIT , JGVAT TYPE , JGVAH HT , IFNULL ( KGMTYVAL , '' ) ENSTYP 
										FROM YPRTOBJ 
											INNER JOIN KPGARAP ON KDFIPB = JGIPB AND KDFALX = JGALX AND KDFTYP = P_TYPE AND KDFFOR = V_CODEFORMULE AND KDFOPT = V_CODEOPTION AND KDFGARAN = GARANTIE AND KDFRSQ = JGRSQ AND KDFOBJ = JGOBJ 
											LEFT JOIN KTYPVALD ON KGMBASE = JGVAT 
										WHERE JGIPB = P_CODEOFFRE AND JGALX = P_VERSION AND JGRSQ = P_CODERSQ 
											AND ( ( ( ( JGVDA * 10000 + JGVDM * 100 + JGVDJ ) >= V_DATEAVN OR ( JGVDA * 10000 + JGVDM * 100 + JGVDJ ) = 0 ) 
												AND ( ( JGVFA * 10000 + JGVFM * 100 + JGVFJ ) >= V_DATEAVN OR ( JGVFA * 10000 + JGVFM * 100 + JGVFJ ) = 0 ) ) 
											OR ( V_DATEAVN = 0 ) ) 
								DO 
  
									IF ( ( V_ENSTYPE = '' OR V_ENSTYPE = ENSTYP ) AND ( V_UNIT = '' OR V_UNIT = UNIT ) AND ( V_HT = '' OR V_HT = HT ) AND ( V_CALCULOK > 0 ) ) THEN 
										SET V_VALEUR = V_VALEUR + VALEUR ; 
										SET V_UNIT = UNIT ; 
										SET V_TYPE = TYPE ; 
										SET V_HT = HT ; 
										SET V_ENSTYPE = ENSTYP ; 
									ELSE 
										SET V_CALCULOK = 0 ; 
										SET V_VALEUR = 0 ; 
										SET V_UNIT = '' ; 
										SET V_TYPE = '' ; 
										SET V_HT = '' ; 
										SET V_ENSTYPE = '' ; 
									END IF ; 
								END FOR ; 
  
								SELECT COUNT ( * ) INTO V_COUNTENS FROM KGARTVL WHERE KGKGAR = GARANTIE AND KGKTYVAL = V_ENSTYPE ; 
  
								IF ( V_CALCULPAR > 0 AND V_COUNTENS > 0 ) THEN 
									IF ( V_TYPE <> '' ) THEN 
										SET ASSBASE = V_TYPE ; 
									END IF ; 
									UPDATE KPGARAN 
										SET KDEASVALO = V_VALEUR , KDEASVALA = V_VALEUR , KDEASVALW = 0 , KDEASUNIT = V_UNIT , KDEASBASE = ASSBASE 
										WHERE KDEID = IDGAR ; 
									--UPDATE KPGARAH 
									--	SET KDEASVALO = V_VALEUR , KDEASVALA = V_VALEUR , KDEASVALW = 0 , KDEASUNIT = V_UNIT , KDEASBASE = ASSBASE 
									--	WHERE KDEID = IDGAR ; 
									--UPDATE KPGARAW 
									--	SET KDEASVALO = V_VALEUR , KDEASVALA = V_VALEUR , KDEASVALW = 0 , KDEASUNIT = V_UNIT , KDEASBASE = ASSBASE 
									--	WHERE KDEID = IDGAR ; 
									SET V_MODIFFORMULE = 1 ; 
								END IF ; 
  
							WHEN 'E' THEN  -- LORSQUE LA GARANTIE EST EXCLUE DANS LA TABLE KPGARAP 
								 -- RÉCUPÉRATION DE TOUS LES OBJETS ET ADDITION DE LEUR VALEUR QUI NE SONT PAS PRÉSENTS DANS KPGARAP 
								FOR LOOP_OBJET AS FREE_LIST CURSOR FOR 
									SELECT JGVAL VALEUR , JGVAU UNIT , JGVAT TYPE , JGVAH HT , IFNULL ( KGMTYVAL , '' ) ENSTYP 
										FROM YPRTOBJ 
											INNER JOIN KPGARAP ON KDFIPB = JGIPB AND KDFALX = JGALX AND KDFTYP = P_TYPE AND KDFFOR = V_CODEFORMULE AND KDFOPT = V_CODEOPTION AND KDFGARAN = GARANTIE AND KDFRSQ = JGRSQ AND KDFOBJ <> JGOBJ 
											LEFT JOIN KTYPVALD ON KGMBASE = JGVAT 
										WHERE JGIPB = P_CODEOFFRE AND JGALX = P_VERSION AND JGRSQ = P_CODERSQ 
											AND ( ( ( ( JGVDA * 10000 + JGVDM * 100 + JGVDJ ) >= V_DATEAVN OR ( JGVDA * 10000 + JGVDM * 100 + JGVDJ ) = 0 ) 
												AND ( ( JGVFA * 10000 + JGVFM * 100 + JGVFJ ) >= V_DATEAVN OR ( JGVFA * 10000 + JGVFM * 100 + JGVFJ ) = 0 ) ) 
											OR ( V_DATEAVN = 0 ) ) 
								DO 
									IF ( ( V_ENSTYPE = '' OR V_ENSTYPE = ENSTYP ) AND ( V_UNIT = '' OR V_UNIT = UNIT ) AND ( V_HT = '' OR V_HT = HT ) AND ( V_CALCULOK > 0 ) ) THEN 
										SET V_VALEUR = V_VALEUR + VALEUR ; 
										SET V_UNIT = UNIT ; 
										SET V_TYPE = TYPE ; 
										SET V_HT = HT ; 
										SET V_ENSTYPE = ENSTYP ; 
									ELSE 
										SET V_CALCULOK = 0 ; 
										SET V_VALEUR = 0 ; 
										SET V_UNIT = '' ; 
										SET V_TYPE = '' ; 
										SET V_HT = '' ; 
										SET V_ENSTYPE = '' ; 
									END IF ; 
								END FOR ; 
  
								SELECT COUNT ( * ) INTO V_COUNTENS FROM KGARTVL WHERE KGKGAR = GARANTIE AND KGKTYVAL = V_ENSTYPE ; 
  
								IF ( V_CALCULPAR > 0 AND V_COUNTENS > 0 ) THEN 
									IF ( V_TYPE <> '' ) THEN 
										SET ASSBASE = V_TYPE ; 
									END IF ; 
									UPDATE KPGARAN 
										SET KDEASVALO = V_VALEUR , KDEASVALA = V_VALEUR , KDEASVALW = 0 , KDEASUNIT = V_UNIT , KDEASBASE = ASSBASE 
									WHERE KDEID = IDGAR ; 
									--UPDATE KPGARAH 
									--	SET KDEASVALO = V_VALEUR , KDEASVALA = V_VALEUR , KDEASVALW = 0 , KDEASUNIT = V_UNIT , KDEASBASE = ASSBASE 
									--	WHERE KDEID = IDGAR ; 
									--UPDATE KPGARAW 
									--	SET KDEASVALO = V_VALEUR , KDEASVALA = V_VALEUR , KDEASVALW = 0 , KDEASUNIT = V_UNIT , KDEASBASE = ASSBASE 
									--WHERE KDEID = IDGAR ; 
									SET V_MODIFFORMULE = 1 ; 
								END IF ; 
  
							ELSE  -- LORSQUE QU'IL N'Y A AUCUNE APPLICATION DANS LA TABLE KPGARAP 
								SELECT KDBASVALO , KDBASUNIT , KDBASBASE INTO V_VALEUR , V_UNIT , V_TYPE 
									FROM KPOPT 
								WHERE KDBIPB = P_CODEOFFRE AND KDBALX = P_VERSION AND KDBTYP = P_TYPE AND KDBFOR = V_CODEFORMULE AND KDBOPT = V_CODEOPTION ; 
  
								 -- VÉRIFIE QUE LE TYPE D'ENSEMBLE EST PRÉSENT DANS LE RÉFÉRENTIEL 
								SELECT COUNT ( * ) INTO V_COUNTGAR FROM KTYPVALD WHERE KGMTYVAL = ENSGAR AND KGMBASE = V_TYPE ; 
								IF ( V_COUNTGAR > 0 ) THEN 
									/* IF ( ASSBASE = '' ) THEN         
										SET ASSBASE = V_TYPE ;         
									END IF ; */ 
									IF ( V_TYPE <> '' ) THEN 
										SET ASSBASE = V_TYPE ; 
									END IF ; 
									 -- MISE À JOUR DE L'ASSIETTE AVEC LES VALEURS DE L'OPTION 
									UPDATE KPGARAN 
										SET KDEASVALO = V_VALEUR , KDEASVALA = V_VALEUR , KDEASVALW = 0 , KDEASUNIT = V_UNIT , KDEASBASE = ASSBASE 
										WHERE KDEID = IDGAR ; 
									--UPDATE KPGARAH 
									--	SET KDEASVALO = V_VALEUR , KDEASVALA = V_VALEUR , KDEASVALW = 0 , KDEASUNIT = V_UNIT , KDEASBASE = ASSBASE 
									--	WHERE KDEID = IDGAR ; 
									--UPDATE KPGARAW 
									--	SET KDEASVALO = V_VALEUR , KDEASVALA = V_VALEUR , KDEASVALW = 0 , KDEASUNIT = V_UNIT , KDEASBASE = ASSBASE 
									--	WHERE KDEID = IDGAR ; 
									SET V_MODIFFORMULE = 1 ; 
								END IF ; 
								 -- SI LE TYPE EST VIDE 
								IF ( V_TYPE = '' ) THEN 
									UPDATE KPGARAN 
										SET KDEASVALO = V_VALEUR , KDEASVALA = V_VALEUR , KDEASVALW = 0 , KDEASUNIT = V_UNIT , KDEASBASE = ASSBASE 
										WHERE KDEID = IDGAR ; 
									--UPDATE KPGARAH 
									--	SET KDEASVALO = V_VALEUR , KDEASVALA = V_VALEUR , KDEASVALW = 0 , KDEASUNIT = V_UNIT , KDEASBASE = ASSBASE 
									--	WHERE KDEID = IDGAR ; 
									--UPDATE KPGARAW 
									--SET KDEASVALO = V_VALEUR , KDEASVALA = V_VALEUR , KDEASVALW = 0 , KDEASUNIT = V_UNIT , KDEASBASE = ASSBASE 
									--WHERE KDEID = IDGAR ; 
									SET V_MODIFFORMULE = 1 ; 
								END IF ; 
						END CASE ; 
					END FOR ; 
				END P3 ; 
			END IF ; 
			 
			IF ( V_MODIFFORMULE > 0 AND V_NUMAVN > 0 ) THEN 
				SET V_DATEAVNOPT = 0 ; 
				SET V_NUMAVNOPT = 0 ; 
				SELECT KDBAVG , ( KDBAVA * 10000 + KDBAVM * 100 + KDBAVJ ) 
					INTO V_NUMAVNOPT , V_DATEAVNOPT 
					FROM KPOPT 
				WHERE KDBIPB = P_CODEOFFRE AND KDBALX = P_VERSION AND KDBTYP = P_TYPE AND KDBFOR = V_CODEFORMULE AND KDBOPT = V_CODEOPTION ; 
				IF ( V_NUMAVNOPT <> V_NUMAVN ) THEN 
					IF ( V_DATEAVNOPT <> 0 AND V_DATEAVNOPT < V_DATEAVNRSQ ) THEN 
						SET V_DATEAVNOPT = V_DATEAVNRSQ ; 
					END IF ; 
					IF ( V_DATEAVNOPT < V_DATEAVN ) THEN 
						SET V_DATEAVNOPT = V_DATEAVN ; 
					END IF ; 
  
					SET V_YEARAVN = ROUND ( V_DATEAVNOPT / 10000 , 0 ) ; 
					SET V_MONTHAVN = ROUND ( ( V_DATEAVNOPT - ( V_YEARAVN * 10000 ) ) / 100 , 0 ) ; 
					SET V_DAYAVN = ROUND ( ( V_DATEAVNOPT - ( V_YEARAVN * 10000 ) - ( V_MONTHAVN * 100 ) ) , 0 ) ; 
  
					UPDATE KPOPT SET KDBAVG = V_NUMAVN , KDBAVA = V_YEARAVN , KDBAVM = V_MONTHAVN , KDBAVJ = V_DAYAVN 
						WHERE KDBIPB = P_CODEOFFRE AND KDBALX = P_VERSION AND KDBTYP = P_TYPE AND KDBFOR = V_CODEFORMULE AND KDBOPT = V_CODEOPTION ; 
				END IF ; 
			END IF ; 
  
		END FOR ; 
	END P2 ; 
  
	/* APPEL DE LA PROCÉDURE DE RECALCUL DES PRIMES DE GARANTIES */ 
	CALL SP_CALCULPRIMEGARANTIE ( P_CODEOFFRE , P_VERSION , P_TYPE ) ; 
  
	CALL SP_ARBRESV ( P_TYPE , P_CODEOFFRE , P_VERSION , 'OBJ' , 40 , 1 , 'OBJ' , P_CODERSQ , P_CODEOBJ , 0 , 0 , 0 , '' , P_USER , P_DATESYSTEM , P_HEURESYSTEM , 'O' , '' ) ; 
	CALL SP_ARBRESV ( P_TYPE , P_CODEOFFRE , P_VERSION , 'RSQ' , 30 , 1 , 'RSQ' , P_CODERSQ , 0 , 0 , 0 , 0 , '' , P_USER , P_DATESYSTEM , P_HEURESYSTEM , 'O' , 'N' ) ; 
  
	CALL SP_SETDATES ( P_CODEOFFRE , P_VERSION , P_TYPE , V_NUMAVN , P_CODERSQ , P_CODEOBJ , 0 , 0 , 0 , 'OBJ' ) ; 
  
END P1  ; 

