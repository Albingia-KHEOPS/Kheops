CREATE PROCEDURE SP_RECHERCHEPRENEURASSURANCE ( 
	IN P_CODEASSUR INTEGER , 
	IN P_NOMASSUR CHAR(40) , 
	IN P_CP CHAR(5) , 
	IN P_STARTLINE INTEGER , 
	IN P_ENDLINE INTEGER , 
	IN P_SORTINGBY CHAR(40) , 
	IN P_ORDERBY CHAR(10) , 
	IN P_MODE CHAR(13) ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_RECHERCHEPRENEURASSURANCE 
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

DECLARE V_QUERY VARCHAR ( 8000 ) DEFAULT '' ; 
DECLARE V_SELECT_YASSNOM VARCHAR ( 500 ) DEFAULT '' ; 
DECLARE V_SELECT_YASSNOM1 VARCHAR ( 500 ) DEFAULT '' ; 
DECLARE V_SELECT_YASSURE VARCHAR ( 500 ) DEFAULT '' ; 
DECLARE V_SELECT_YADRESS VARCHAR ( 500 ) DEFAULT '' ; 
DECLARE V_SELECT_NBSIN VARCHAR ( 1000 ) DEFAULT '' ;
DECLARE V_SELECT_NBPRM_RETARD VARCHAR ( 2000 ) DEFAULT '' ;
DECLARE V_ROW_NUMBER VARCHAR ( 500 ) DEFAULT '' ; 
DECLARE V_CASE_EXACTMATCH VARCHAR ( 128 ) DEFAULT '' ; 
DECLARE CURSOR1 CURSOR WITH RETURN FOR SQL_STATEMENT ; 

IF ( P_STARTLINE > 0 AND P_ENDLINE > 0 AND P_SORTINGBY IS NOT NULL AND P_SORTINGBY <> '' AND P_ORDERBY IS NOT NULL AND P_ORDERBY <> ' ' ) THEN 

SET V_CASE_EXACTMATCH = 'CAST ( CASE WHEN UPPER ( TRIM ( ANNOM ) ) = ''' || UPPER ( TRIM ( P_NOMASSUR ) ) || ''' THEN 1 ELSE 0 END AS INTEGER ) EXACTMATCH' ;
SET V_SELECT_YASSURE = '( SELECT ASIAS , ASADH , ASAD1 , ASAD2 , ASSIR FROM YASSURE ) ASR' ; 
SET V_SELECT_NBSIN = '( 
SELECT PBIAS , COUNT(SINUM) NBSIN FROM YPOBASE INNER JOIN YSINIST ON ( SIIPB , SIALX ) = ( PBIPB , PBALX ) 
WHERE CAST ( SISUA || LPAD ( CASE SISUM WHEN 0 THEN 1 ELSE SISUM END , 2 , ''0'' ) || LPAD ( CASE SISUJ WHEN 0 THEN 1 ELSE SISUJ END , 2 , ''0'' ) || ''000000'' AS TIMESTAMP ) + 3 YEARS >= NOW()
GROUP BY PBIAS ) AS PO' ;
SET V_SELECT_NBPRM_RETARD = '( 
SELECT 
MAX(CASE WHEN PO.PKRLC NOT IN ( ''P'' , ''R'' , ''1'' , ''3'' , ''5'' ) AND NOW() > ( PO.DATEECH + ( CAST ( CASE WHEN PO.PBBRA = ''RS'' THEN 15 ELSE 30 END AS INTEGER ) ) DAYS ) THEN 1 ELSE 0 END) RETARDS , 
MAX(CASE WHEN PO.PKRLC IN ( ''P'' , ''R'' , ''1'' , ''3'' , ''5'' ) OR PO.PKMOT IN (''X'', ''Y'', ''R'') THEN 1 ELSE 0 END) IMPAYES , 
ASIAS CODE 
FROM YPRIMES P 
INNER JOIN ( 
	SELECT PBIPB , PBALX , PBIAS , PBBRA , PKIPK , 
		( CASE WHEN PBAVN = 0 THEN CAST ( PKEHA || LPAD ( PKEHM , 2 , ''0'' ) || LPAD ( PKEHJ , 2 , ''0'' ) || ''000000'' AS TIMESTAMP ) 
		ELSE CAST ( PKEMA || LPAD ( PKEMM , 2 , ''0'' ) || LPAD ( PKEMJ , 2 , ''0'' ) || ''000000'' AS TIMESTAMP ) 
		END ) DATEECH , PKRLC, PKMOT, PKSIT 
	FROM YPOBASE 
	INNER JOIN YPRIMES ON ( PBIPB , PBALX , PBETA , PBTYP ) = ( PKIPB , PKALX , ''V'' , ''P'' ) AND PBSTA > 0 AND PKMOT IN (''X'', ''Y'', ''R'') 
) AS PO ON ( P.PKIPK , P.PKIPB , P.PKALX ) = ( PO.PKIPK , PO.PBIPB , PO.PBALX ) AND P.PKMHT > 0
INNER JOIN YASSURE ON PBIAS = ASIAS GROUP BY ASIAS ) AS PRM' ;

IF P_MODE = 'AUTOCOMPLETE' THEN 
	SET V_QUERY = 'SELECT ANORD, ANIAS CODEASSU, ANNOM NOMASSURE , CAST(JSON_ARRAYAGG( ANNOM1 ) AS VARCHAR(10000)) AS NOMSECONDAIRE , ABPDP6 DEPARTEMENT , ABPCP6 CODEPOSTAL , ABPVI6 NOMVILLE , ASAD1 , ASAD2 , ASSIR SIREN , 
SUM ( IFNULL ( NBSIN , 0 ) ) NBSIN , 
IFNULL ( PRM.RETARDS , 0 ) RETARDS , IFNULL ( PRM.IMPAYES , 0 ) IMPAYES 
FROM ( SELECT ANORD , ANIAS , ANNOM , ' || REPLACE( V_CASE_EXACTMATCH , ' THEN 1 ' , ' THEN 1000 ' ) || ' FROM YASSNOM WHERE ANINL = 0 AND ANTNM = ''A'') YASSNOM1 
LEFT JOIN ( SELECT ANIAS ANIAS1 , ANNOM ANNOM1 , ' || V_CASE_EXACTMATCH || ' FROM YASSNOM WHERE ANINL = 0 AND ANTNM = ''S'' ) SNOM ON ANIAS = ANIAS1 AND ANNOM1 <> ANNOM 
INNER JOIN ' || V_SELECT_YASSURE || ' ON ASIAS = ANIAS 
LEFT JOIN ( SELECT ABPDP6 , ABPCP6 , ABPCHR , ABPVI6 FROM YADRESS ) ADR ON ASADH = ABPCHR 
LEFT JOIN ' || V_SELECT_NBSIN || ' ON PBIAS = ANIAS 
LEFT JOIN ' || V_SELECT_NBPRM_RETARD || ' ON CODE = ANIAS 
WHERE ' ; 
	IF P_CODEASSUR > 0 THEN 
		SET V_QUERY = V_QUERY || ' ANIAS = ' || P_CODEASSUR ; 
	ELSEIF P_NOMASSUR <> '' THEN 
		SET V_QUERY = V_QUERY || ' ( ANNOM LIKE ''' || TRIM ( P_NOMASSUR ) || '%'' OR ANNOM1 LIKE ''' || TRIM ( P_NOMASSUR ) || '%'' ) ' ; 
	END IF ;
	SET V_QUERY = V_QUERY || ' GROUP BY (IFNULL(SNOM.EXACTMATCH , 0) + YASSNOM1.EXACTMATCH) , ANIAS , ANORD , ANNOM , ABPDP6 , ABPCP6 , ABPVI6 , ASAD1 , ASAD2, ASSIR , RETARDS , IMPAYES ';
	SET V_QUERY = V_QUERY || ' ORDER BY (IFNULL(SNOM.EXACTMATCH , 0) + YASSNOM1.EXACTMATCH) DESC , ' || P_SORTINGBY || ' ' || P_ORDERBY || ' FETCH FIRST ' || P_ENDLINE || ' ROWS ONLY WITH NC '; 
ELSE 
	SET V_SELECT_YASSNOM = '( SELECT ANNOM , ANIAS , ANORD , ' || REPLACE( V_CASE_EXACTMATCH , ' THEN 1 ' , ' THEN 1000 ' ) || ' FROM YASSNOM WHERE ANINL = 0 AND ANTNM = ''A'' ) NOMS' ; 
	SET V_SELECT_YASSNOM1 = '( SELECT ANIAS ANIAS1 , ANNOM ANNOM1 , ' || V_CASE_EXACTMATCH || ' FROM YASSNOM WHERE ANINL = 0 AND ANTNM = ''S'' ) NOMS1' ; 

	SET V_SELECT_YADRESS = 'SELECT ABPDP6 , ABPCP6 , ABPCHR , ABPVI6 FROM YADRESS' ; 
	IF P_CP = '' THEN 
		SET V_SELECT_YADRESS = ' LEFT JOIN ( ' || V_SELECT_YADRESS || ' ) ADR' ;
	ELSE 
		SET V_SELECT_YADRESS = ' INNER JOIN ( ' || V_SELECT_YADRESS || ' WHERE ( LPAD( ABPDP6 , 2 , ''0'') || LPAD( CAST ( ABS ( ABPCP6 ) AS VARCHAR ( 3 ) ) , 3 , ''0'') ) LIKE ''' || TRIM ( P_CP ) || '%'' ) ADR';
	END IF ;

	SET V_ROW_NUMBER = 'ROW_NUMBER() OVER(ORDER BY EXACTMATCH DESC , ' ;
	IF P_SORTINGBY IN ( 'CODEPOSTAL' , 'NOMVILLE' ) THEN 
		IF P_SORTINGBY = 'CODEPOSTAL' THEN 
			SET V_ROW_NUMBER = V_ROW_NUMBER || '( LPAD( DEPARTEMENT , 2 , ''0'') || LPAD( CAST ( ABS ( CODEPOSTAL ) AS VARCHAR ( 3 ) ) , 3 , ''0'') )' || ' ' || P_ORDERBY || ' , ABPCHR) ROWNUM' ;
		ELSE 
			SET V_ROW_NUMBER = V_ROW_NUMBER || TRIM( P_SORTINGBY ) || ' ' || P_ORDERBY || ', IFNULL(ABPCHR, 0)) ROWNUM' ;
		END IF ;
	ELSEIF IFNULL( P_SORTINGBY , '' ) = '' THEN 
		SET V_ROW_NUMBER = V_ROW_NUMBER || 'CODEASSU ' || P_ORDERBY || ') ROWNUM' ;
	ELSEIF P_SORTINGBY = 'NOMSECONDAIRE' THEN 
		SET V_ROW_NUMBER = V_ROW_NUMBER || 'SNOM1 ' || P_ORDERBY || ') ROWNUM' ;
		SET V_QUERY = 'MIN ( IFNULL ( ANNOM1 , '''' ) ) SNOM1 , ' ;
	ELSE
		SET V_ROW_NUMBER = V_ROW_NUMBER || TRIM( P_SORTINGBY ) || ' ' || P_ORDERBY || ') ROWNUM' ;
	END IF ; 
	
	SET V_QUERY = V_QUERY || 'ASIAS CODEASSU ,
ANORD, ANNOM NOMASSURE , CAST(JSON_ARRAYAGG( ANNOM1 ) AS VARCHAR(10000)) AS NOMSECONDAIRE , 
ABPDP6 DEPARTEMENT , ABPCP6 CODEPOSTAL , ABPVI6 NOMVILLE , ASAD1 , ASAD2 , ASSIR SIREN , IFNULL(ABPCHR , 0) ABPCHR , 
SUM ( IFNULL( NBSIN , 0 ) ) NBSIN , 
IFNULL ( PRM.RETARDS , 0 ) RETARDS , IFNULL ( PRM.IMPAYES , 0 ) IMPAYES 
FROM ' || V_SELECT_YASSNOM || ' 
LEFT JOIN ' || V_SELECT_YASSNOM1 || ' ON ANIAS1 = ANIAS AND ANNOM1 <> ANNOM 
INNER JOIN ' || V_SELECT_YASSURE || ' ON ASIAS = ANIAS ' 
|| V_SELECT_YADRESS || ' ON ASADH = ABPCHR 
LEFT JOIN ' || V_SELECT_NBSIN || ' ON PBIAS = ANIAS 
LEFT JOIN ' || V_SELECT_NBPRM_RETARD || ' ON CODE = ANIAS ' ; 

	IF P_CODEASSUR > 0 THEN 
		SET V_QUERY = 'SELECT ' || V_QUERY || ' WHERE ANIAS = ' || P_CODEASSUR || ' GROUP BY ASIAS , ANORD , ANNOM , ABPDP6 , ABPCP6 , ABPVI6 , ASAD1 , ASAD2 , ASSIR , IFNULL(ABPCHR , 0) , RETARDS , IMPAYES WITH NC ' ; 
	ELSE 

		IF P_NOMASSUR <> '' THEN 
			SET V_QUERY = V_QUERY || ' WHERE ( ANNOM LIKE ''' || TRIM ( P_NOMASSUR ) || '%''' || ' OR ANNOM1 LIKE ''' || TRIM ( P_NOMASSUR ) || '%'' )' ; 
		END IF ;
		SET V_QUERY = 'SELECT * FROM ( SELECT ' || V_ROW_NUMBER || ' , T0.* FROM ( SELECT NOMS.EXACTMATCH + IFNULL(NOMS1.EXACTMATCH , 0) EXACTMATCH , '
			|| V_QUERY 
			|| ' GROUP BY ASIAS , ANORD , ANNOM , ABPDP6 , ABPCP6 , ABPVI6 , ASAD1 , ASAD2, ASSIR , NOMS.EXACTMATCH + IFNULL(NOMS1.EXACTMATCH , 0) , IFNULL(ABPCHR , 0) , RETARDS , IMPAYES ) AS T0 '
			|| ' ORDER BY ROWNUM FETCH FIRST 200 ROWS ONLY ) AS T'
			|| ' WHERE ROWNUM BETWEEN ' || P_STARTLINE || ' AND ' || P_ENDLINE 
			|| ' WITH NC ';
		
	END IF ; 
END IF ; 

PREPARE SQL_STATEMENT FROM V_QUERY ; 
OPEN CURSOR1 ; 
END IF ; 
END P1 ; 
