CREATE PROCEDURE SP_GETLISTRSQAPPLIQUE_HIST ( 
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_CODEAVT INTEGER , 
	IN P_CODEFORMULE INTEGER , 
	IN P_CODEOPTION INTEGER ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_GETLISTRSQAPPLIQUE_HIST 
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
  
	DECLARE V_QUERY VARCHAR ( 8000 ) DEFAULT '' ; 
	DECLARE V_AVTCREATE INTEGER DEFAULT 0 ; 
	DECLARE V_AVTMODIF INTEGER DEFAULT 0 ; 
	DECLARE CURSORRSQ CURSOR WITH RETURN FOR SQL_STATEMENT ; 
	 
	SET V_AVTMODIF = 0 ; 
	SELECT KDBAVE , KDBAVG INTO V_AVTCREATE , V_AVTMODIF FROM HPOPT WHERE TRIM ( KDBIPB ) = TRIM ( P_CODEOFFRE ) AND KDBALX = P_VERSION AND KDBTYP = P_TYPE 
		AND KDBFOR = P_CODEFORMULE AND KDBOPT = P_CODEOPTION AND KDBAVN = P_CODEAVT ; 
	 
	SET V_QUERY = 'SELECT KABRSQ CODERSQ, KABDESC DESCRSQ, KABCIBLE CIBLERSQ, KAIID CODECIBLE, KABCIBLE DESCCIBLE, KACOBJ CODEOBJ, KACDESC DESCOBJ,
		KBEID CODEINVEN, KBEDESC DESCINVEN, IFNULL(APRSQ.KDDID, 0) RSQUSED, IFNULL(APOBJ.KDDID, 0) OBJUSED
			FROM YHPBASE 
				INNER JOIN YHRTRSQ ON JEIPB = PBIPB AND JEALX = PBALX AND JEAVN = PBAVN ' ; 
				 
	 --IF ( P_CODEAVT > 0 ) THEN 
		SET V_QUERY = V_QUERY CONCAT ' AND (JEAVE = ' CONCAT P_CODEAVT CONCAT ' OR JEAVF = ' CONCAT P_CODEAVT CONCAT ' OR (JEVDA * 10000 + JEVDM * 100 + JEVDJ) >= (PBAVA * 10000 + PBAVM * 100 + PBAVJ) OR (JEVDA * 10000 + JEVDM * 100 + JEVDJ) = 0) ' ; 
	 --END IF ;	 
	SET V_QUERY = V_QUERY CONCAT ' INNER JOIN HPRSQ ON KABIPB = JEIPB AND KABALX = JEALX AND KABTYP = ''' CONCAT P_TYPE CONCAT ''' AND KABRSQ = JERSQ AND KABAVN = JEAVN
			INNER JOIN KCIBLEF ON KAICIBLE = KABCIBLE AND KAIBRA = PBBRA 
			INNER JOIN YHRTOBJ ON JGIPB = KABIPB AND JGALX = KABALX AND JGRSQ = KABRSQ AND JGAVN = KABAVN ' ; 
	 --IF ( P_CODEAVT > 0 ) THEN 
		SET V_QUERY = V_QUERY CONCAT ' AND (JGAVE = ' CONCAT P_CODEAVT CONCAT ' OR JGAVF = ' CONCAT P_CODEAVT CONCAT ' OR (JGVDA * 10000 + JGVDM * 100 + JGVDJ) >= (PBAVA * 10000 + PBAVM * 100 + PBAVJ) OR (JGVDA * 10000 + JGVDM * 100 + JGVDJ) = 0) ' ; 
	 --END IF ; 
	SET V_QUERY = V_QUERY CONCAT ' INNER JOIN HPOBJ ON KACIPB = JGIPB AND KACALX = JGALX AND KACTYP = KABTYP AND KACRSQ = JGRSQ AND KACOBJ = JGOBJ AND KACAVN = JGAVN
			LEFT JOIN HPINVEN ON KBEIPB = KACIPB AND KBEALX = KACALX AND KBETYP = KACTYP AND KBEID = KACINVEN AND KBEAVN = KACAVN
			LEFT JOIN HPOPTAP APRSQ ON APRSQ.KDDIPB = KACIPB AND APRSQ.KDDALX = KACALX AND APRSQ.KDDTYP = KACTYP AND APRSQ.KDDPERI = ''RQ'' AND APRSQ.KDDRSQ = KACRSQ AND APRSQ.KDDAVN = KACAVN' ; 
			 
	SET V_QUERY = V_QUERY CONCAT ' LEFT JOIN HPOPTAP APOBJ ON APOBJ.KDDIPB = KACIPB AND APOBJ.KDDALX = KACALX AND APOBJ.KDDTYP = KACTYP AND APOBJ.KDDPERI = ''OB'' AND APOBJ.KDDRSQ = KACRSQ AND APOBJ.KDDOBJ = KACOBJ AND APOBJ.KDDAVN = KACAVN' ; 
	 
	SET V_QUERY = V_QUERY CONCAT ' WHERE TRIM(PBIPB) = ''' CONCAT TRIM ( P_CODEOFFRE ) CONCAT ''' AND PBALX = ' CONCAT P_VERSION CONCAT ' AND PBTYP = ''' CONCAT P_TYPE CONCAT '''' ; 
			 
	IF ( ( V_AVTCREATE <> P_CODEAVT OR V_AVTMODIF = P_CODEAVT ) ) THEN  -- AND P_CODEAVT > 0 ) THEN 
		SET V_QUERY = V_QUERY CONCAT ' AND PBAVN = ' CONCAT P_CODEAVT CONCAT ' AND (IFNULL(APRSQ.KDDFOR, IFNULL(APOBJ.KDDFOR, 0)) = ' CONCAT P_CODEFORMULE CONCAT ' OR IFNULL(APRSQ.KDDFOR, IFNULL(APOBJ.KDDFOR, 0)) = 0)
			AND (IFNULL(APRSQ.KDDOPT, IFNULL(APOBJ.KDDOPT, 0)) = ' CONCAT P_CODEOPTION CONCAT ' OR IFNULL(APRSQ.KDDOPT, IFNULL(APOBJ.KDDOPT, 0)) = 0)' ; 
	END IF ; 
  
	SET V_QUERY = V_QUERY CONCAT ' ORDER BY KABRSQ, KACOBJ' ; 
		 
	PREPARE SQL_STATEMENT FROM V_QUERY ; 
	OPEN CURSORRSQ ; 
  
	 
END P1  ; 
  

  

