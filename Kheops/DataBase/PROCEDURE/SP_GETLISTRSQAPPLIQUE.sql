CREATE PROCEDURE SP_GETLISTRSQAPPLIQUE ( 
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_CODEAVT INTEGER , 
	IN P_CODEFORMULE INTEGER , 
	IN P_CODEOPTION INTEGER ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_GETLISTRSQAPPLIQUE 
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
	SELECT KDBAVE , KDBAVG INTO V_AVTCREATE , V_AVTMODIF FROM KPOPT WHERE TRIM ( KDBIPB ) = TRIM ( P_CODEOFFRE ) AND KDBALX = P_VERSION AND KDBTYP = P_TYPE 
		AND KDBFOR = P_CODEFORMULE AND KDBOPT = P_CODEOPTION ; 
	 
	SET V_QUERY = 'SELECT KABRSQ CODERSQ, KABDESC DESCRSQ, KABCIBLE CIBLERSQ, KAIID CODECIBLE, KABCIBLE DESCCIBLE, KACOBJ CODEOBJ, KACDESC DESCOBJ,
		KBEID CODEINVEN, KBEDESC DESCINVEN, 
		IFNULL(APRSQ.KDDID, 0) RSQUSED, CASE WHEN RSQ.DATEFIN >= PBAVA * 10000 + PBAVM * 100 + PBAVJ OR (RSQ.DATEFIN = 0 AND 1 = 1)THEN 0 ELSE 1 END RSQOUT,
		IFNULL(APOBJ.KDDID, 0) OBJUSED, CASE WHEN OBJ.DATEFIN >= PBAVA * 10000 + PBAVM * 100 + PBAVJ OR (OBJ.DATEFIN = 0 AND 1 = 1) THEN 0 ELSE 1 END OBJOUT
			FROM YPOBASE ' ; 
				/* INNER JOIN YPRTRSQ ON JEIPB = PBIPB AND JEALX = PBALX ' ;  */ 
				 
	SET V_QUERY = V_QUERY CONCAT ' INNER JOIN (SELECT JEIPB, JEALX, JERSQ, JEVDA * 10000 + JEVDM * 100 + JEVDJ DATEDEB, JEVFA * 10000 + JEVFM * 100 + JEVFJ DATEFIN FROM YPRTRSQ WHERE TRIM(JEIPB) = ''' CONCAT TRIM ( P_CODEOFFRE ) CONCAT ''' AND JEALX = ' CONCAT P_VERSION ; 
				 
	IF ( P_CODEAVT > 0 ) THEN 
		SET V_QUERY = V_QUERY CONCAT ' AND ((JEAVE = ' CONCAT P_CODEAVT CONCAT ' OR JEAVF = ' CONCAT P_CODEAVT CONCAT ' 
			OR ((JEVDA * 10000 + JEVDM * 100 + JEVDJ) >= (SELECT PBAVA * 10000 + PBAVM * 100 + PBAVJ FROM YPOBASE WHERE PBIPB = JEIPB AND PBALX = JEALX AND PBTYP = ''' CONCAT P_TYPE CONCAT ''')) 
			OR (JEVDA * 10000 + JEVDM * 100 + JEVDJ = 0)) OR JERSQ IN (SELECT KDDRSQ FROM KPOPTAP WHERE KDDIPB = JEIPB AND KDDALX = JEALX AND KDDTYP = ''' CONCAT P_TYPE CONCAT ''' AND KDDFOR = ' CONCAT P_CODEFORMULE CONCAT ' AND KDDOPT = ' CONCAT P_CODEOPTION CONCAT '))' ; 
	END IF ;	 
	 
	SET V_QUERY = V_QUERY CONCAT ' ) AS RSQ ON RSQ.JEIPB = PBIPB AND RSQ.JEALX = PBALX ' ; 
				 
/* 	IF ( P_CODEAVT > 0 ) THEN          
		SET V_QUERY = V_QUERY CONCAT ' AND (JEAVE = ' CONCAT P_CODEAVT CONCAT ' OR JEAVF = ' CONCAT P_CODEAVT CONCAT ' OR (JEVDA * 10000 + JEVDM * 100 + JEVDJ) >= (PBAVA * 10000 + PBAVM * 100 + PBAVJ) OR (JEVDA * 10000 + JEVDM * 100 + JEVDJ) = 0) ' ;          
	END IF ; */	 
	 
	SET V_QUERY = V_QUERY CONCAT ' INNER JOIN KPRSQ ON KABIPB = RSQ.JEIPB AND KABALX = RSQ.JEALX AND KABTYP = ''' CONCAT P_TYPE CONCAT ''' AND KABRSQ = RSQ.JERSQ 
			INNER JOIN KCIBLEF ON KAICIBLE = KABCIBLE AND KAIBRA = PBBRA ' ; 
			/* INNER JOIN YPRTOBJ ON JGIPB = KABIPB AND JGALX = KABALX AND JGRSQ = KABRSQ ' ;  */ 
			 
	SET V_QUERY = V_QUERY CONCAT ' INNER JOIN (SELECT JGIPB, JGALX, JGRSQ, JGOBJ, JGVDA * 10000 + JGVDM * 100 + JGVDJ DATEDEB, JGVFA * 10000 + JGVFM * 100 + JGVFJ DATEFIN FROM YPRTOBJ WHERE TRIM(JGIPB) = ''' CONCAT TRIM ( P_CODEOFFRE ) CONCAT ''' AND JGALX = ' CONCAT P_VERSION ; 
	 
	IF ( P_CODEAVT > 0 ) THEN 
		SET V_QUERY = V_QUERY CONCAT ' AND ((JGAVE = ' CONCAT P_CODEAVT CONCAT ' OR JGAVF = ' CONCAT P_CODEAVT CONCAT '
			OR ((JGVDA * 10000 + JGVDM * 100 + JGVDJ) >= (SELECT PBAVA * 10000 + PBAVM * 100 + PBAVJ FROM YPOBASE WHERE PBIPB = JGIPB AND PBALX = JGALX AND PBTYP = ''' CONCAT P_TYPE CONCAT '''))
			OR (JGVDA * 10000 + JGVDM * 100 + JGVDJ = 0)) OR JGRSQ IN (SELECT KDDRSQ FROM KPOPTAP WHERE KDDIPB = JGIPB AND KDDALX = JGALX AND KDDTYP = ''' CONCAT P_TYPE CONCAT ''' AND KDDFOR = ' CONCAT P_CODEFORMULE CONCAT ' AND KDDOPT = ' CONCAT P_CODEOPTION CONCAT '))' ; 
	END IF ; 
	 
	SET V_QUERY = V_QUERY CONCAT ' ) AS OBJ ON OBJ.JGIPB = KABIPB AND OBJ.JGALX = KABALX AND OBJ.JGRSQ = KABRSQ ' ; 
			 
/* 	IF ( P_CODEAVT > 0 ) THEN          
		SET V_QUERY = V_QUERY CONCAT ' AND (JGAVE = ' CONCAT P_CODEAVT CONCAT ' OR JGAVF = ' CONCAT P_CODEAVT CONCAT ' OR (JGVDA * 10000 + JGVDM * 100 + JGVDJ) >= (PBAVA * 10000 + PBAVM * 100 + PBAVJ) OR (JGVDA * 10000 + JGVDM * 100 + JGVDJ) = 0) ' ;          
	END IF ;  */ 
	 
	SET V_QUERY = V_QUERY CONCAT ' INNER JOIN KPOBJ ON KACIPB = OBJ.JGIPB AND KACALX = OBJ.JGALX AND KACTYP = KABTYP AND KACRSQ = OBJ.JGRSQ AND KACOBJ = OBJ.JGOBJ
			LEFT JOIN KPINVEN ON KBEIPB = KACIPB AND KBEALX = KACALX AND KBETYP = KACTYP AND KBEID = KACINVEN
			LEFT JOIN KPOPTAP APRSQ ON APRSQ.KDDIPB = KACIPB AND APRSQ.KDDALX = KACALX AND APRSQ.KDDTYP = KACTYP AND APRSQ.KDDPERI = ''RQ'' AND APRSQ.KDDRSQ = KACRSQ' ; 
			 
	SET V_QUERY = V_QUERY CONCAT ' LEFT JOIN KPOPTAP APOBJ ON APOBJ.KDDIPB = KACIPB AND APOBJ.KDDALX = KACALX AND APOBJ.KDDTYP = KACTYP AND APOBJ.KDDPERI = ''OB'' AND APOBJ.KDDRSQ = KACRSQ AND APOBJ.KDDOBJ = KACOBJ' ; 
	 
	SET V_QUERY = V_QUERY CONCAT ' WHERE TRIM(PBIPB) = ''' CONCAT TRIM ( P_CODEOFFRE ) CONCAT ''' AND PBALX = ' CONCAT P_VERSION CONCAT ' AND PBTYP = ''' CONCAT P_TYPE CONCAT '''' ; 
			 
	IF ( ( V_AVTCREATE <> P_CODEAVT OR V_AVTMODIF = P_CODEAVT ) AND P_CODEAVT > 0 ) THEN 
		SET V_QUERY = V_QUERY CONCAT ' AND PBAVN = ' CONCAT P_CODEAVT CONCAT ' AND (IFNULL(APRSQ.KDDFOR, IFNULL(APOBJ.KDDFOR, 0)) = ' CONCAT P_CODEFORMULE CONCAT ' OR IFNULL(APRSQ.KDDFOR, IFNULL(APOBJ.KDDFOR, 0)) = 0)
			AND (IFNULL(APRSQ.KDDOPT, IFNULL(APOBJ.KDDOPT, 0)) = ' CONCAT P_CODEOPTION CONCAT ' OR IFNULL(APRSQ.KDDOPT, IFNULL(APOBJ.KDDOPT, 0)) = 0)' ; 
			/*AND ((RSQ.DATEFIN >= PBAVA * 10000 + PBAVM * 100 + PBAVJ OR (RSQ.DATEFIN = 0 AND 1 = 1)))     
			AND ((OBJ.DATEFIN >= PBAVA * 10000 + PBAVM * 100 + PBAVJ OR (OBJ.DATEFIN = 0 AND 1 = 1)))' ; */ 
			/* AND ((RSQ.DATEDEB >= PBAVA * 10000 + PBAVM * 100 + PBAVJ OR (RSQ.DATEDEB = 0 AND 1 = 1)) AND (RSQ.DATEFIN >= PBAVA * 10000 + PBAVM * 100 + PBAVJ OR (RSQ.DATEFIN = 0 AND 1 = 1)))      
			AND ((OBJ.DATEDEB >= PBAVA * 10000 + PBAVM * 100 + PBAVJ OR (OBJ.DATEDEB = 0 AND 1 = 1)) AND (OBJ.DATEFIN >= PBAVA * 10000 + PBAVM * 100 + PBAVJ OR (OBJ.DATEFIN = 0 AND 1 = 1)))' ;  */ 
	END IF ; 
  
	SET V_QUERY = V_QUERY CONCAT ' ORDER BY KABRSQ, KACOBJ' ; 
		 
	PREPARE SQL_STATEMENT FROM V_QUERY ; 
	OPEN CURSORRSQ ; 
  
	 
END P1  ; 
  

  

