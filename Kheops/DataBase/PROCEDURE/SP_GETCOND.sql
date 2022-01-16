CREATE PROCEDURE SP_GETCOND ( 
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_CODEFORMULE INTEGER , 
	IN P_CODEOPTION INTEGER , 
	IN P_TYPEFILTRE CHAR(1) , 
	IN P_CODEGARANTIE CHAR(1) , 
	IN P_CODEVOLET INTEGER , 
	IN P_CODEBLOC INTEGER , 
	IN P_NIVEAU INTEGER ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_GETCOND 
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
DECLARE CURSOR1 CURSOR WITH RETURN FOR SQL_STATEMENT ; 
CASE P_TYPEFILTRE 
  
WHEN 'G' THEN 
SET V_QUERY = 'SELECT CAST(KDEID AS CHAR(500)) CODE, KDEGARAN LIBELLE, GADES DESCRIPTIF 
                                                      FROM KPGARAN
                                                                          INNER JOIN KGARAN ON GAGAR = KDEGARAN               
                                                                          INNER JOIN KPOPTD ON KDEKDCID = KDCID
                                                                          WHERE KDEIPB = ''' CONCAT P_CODEOFFRE CONCAT ''' AND KDEALX = ' CONCAT P_VERSION CONCAT ' AND KDETYP = ''' CONCAT P_TYPE CONCAT '''
                                                            AND KDEGAN IN (''A'', ''B'', ''C'') AND KDEFOR = ' CONCAT P_CODEFORMULE CONCAT ' AND KDEOPT = ' CONCAT P_CODEOPTION ; 
  
IF ( P_NIVEAU <> 0 ) THEN 
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' AND KDENIVEAU = ' CONCAT P_NIVEAU ; 
END IF ; 
  
IF ( P_CODEVOLET <> 0 ) THEN 
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' AND KDCKAKID = ' CONCAT P_CODEVOLET ; 
END IF ; 
  
IF ( P_CODEBLOC <> 0 ) THEN 
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' AND KDCKAEID = ' CONCAT P_CODEBLOC ; 
END IF ; 
  
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' ORDER BY KDETRI, KDEGARAN' ; 
  
WHEN 'V' THEN 
SET V_QUERY = 'SELECT KAKID CONCAT ''_'' CODE, ''Volet - '' CONCAT KAKVOLET LIBELLE, KAKDESC DESCRIPTIF
                                                                                                                      FROM KPGARAN G1
                                                                                                                                     INNER JOIN KPGARTAR ON KDGKDEID = G1.KDEID
                                                                                                                                     INNER JOIN KPOPTD ON G1.KDEKDCID = KDCID AND KDCFLAG = 1
                                                                                                                                     INNER JOIN KVOLET ON KDCKAKID = KAKID
                                                                                                                      WHERE G1.KDEIPB = ''' CONCAT P_CODEOFFRE CONCAT ''' AND G1.KDEALX = ' CONCAT P_VERSION CONCAT ' AND G1.KDETYP = ''' CONCAT P_TYPE CONCAT '''
                                                            AND G1.KDEGAN IN (''A'', ''B'', ''C'') AND G1.KDEFOR = ' CONCAT P_CODEFORMULE CONCAT ' AND G1.KDEOPT = ' CONCAT P_CODEOPTION ; 
  
IF ( P_CODEGARANTIE <> '' ) THEN 
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' AND (G1.KDEASMOD = ''O'' OR KDGLCIMOD = ''O'' OR KDGFRHMOD = ''O'' OR  KDGPRIMOD = ''O'')' ; 
END IF ; 
  
IF ( P_NIVEAU <> 0 ) THEN 
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' AND G1.KDENIVEAU = ' CONCAT P_NIVEAU ; 
END IF ; 
  
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' UNION SELECT KAPKAKID CONCAT ''_'' CONCAT KAEID CONCAT ''_'' CODE, ''            Bloc - '' CONCAT KAEBLOC LIBELLE, KAEDESC DESCRIPTIF
                                                                                                                                                                                 FROM KPGARAN G1
                                                                                                                                                                                                INNER JOIN KPGARTAR ON KDGKDEID = G1.KDEID
                                                                                                                                                                                                INNER JOIN KPOPTD ON G1.KDEKDCID = KDCID AND KDCFLAG = 1
                                                                                                                                                                                                INNER JOIN KVOLET ON KDCKAKID = KAKID
                                                                                                                                                                                                INNER JOIN KBLOC ON KDCKAEID = KAEID
                                                                                                                                                                                                INNER JOIN KCATBLOC ON KAQKAEID = KAEID
                                                                                                                                                                                                INNER JOIN KCATVOLET ON KAQKAPID = KAPID AND KAPKAKID = KAKID
                                                                                                                                                                                 WHERE G1.KDEIPB = ''' CONCAT P_CODEOFFRE CONCAT ''' AND G1.KDEALX = ' CONCAT P_VERSION CONCAT ' AND G1.KDETYP = ''' CONCAT P_TYPE CONCAT '''
                                                                                                                       AND G1.KDEGAN IN (''A'', ''B'', ''C'') AND G1.KDEFOR = ' CONCAT P_CODEFORMULE CONCAT ' AND G1.KDEOPT = ' CONCAT P_CODEOPTION ; 
  
IF ( P_CODEGARANTIE <> '' ) THEN 
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' AND (G1.KDEASMOD = ''O'' OR KDGLCIMOD = ''O'' OR KDGFRHMOD = ''O'' OR  KDGPRIMOD = ''O'')' ; 
END IF ; 
  
IF ( P_NIVEAU <> 0 ) THEN 
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' AND G1.KDENIVEAU = ' CONCAT P_NIVEAU ; 
END IF ; 
  
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' ORDER BY CODE' ; 
  
WHEN 'N' THEN 
SET V_QUERY = 'SELECT DISTINCT CAST(G1.KDENIVEAU AS CHAR(500)) CODE, ''Niveau '' CONCAT G1.KDENIVEAU LIBELLE
                                                      FROM KPGARAN G1
                                                                                                                                     INNER JOIN KPGARTAR ON KDGKDEID = G1.KDEID
                                                                                                                                     INNER JOIN KPOPTD ON G1.KDEKDCID = KDCID
                                                                          WHERE G1.KDEIPB = ''' CONCAT P_CODEOFFRE CONCAT ''' AND G1.KDEALX = ' CONCAT P_VERSION CONCAT ' AND G1.KDETYP = ''' CONCAT P_TYPE CONCAT '''
                                                            AND G1.KDEGAN IN (''A'', ''B'', ''C'') AND G1.KDEFOR = ' CONCAT P_CODEFORMULE CONCAT ' AND G1.KDEOPT = ' CONCAT P_CODEOPTION ; 
  
IF ( P_CODEGARANTIE <> '' ) THEN 
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' AND (G1.KDEASMOD = ''O'' OR KDGLCIMOD = ''O'' OR KDGFRHMOD = ''O'' OR  KDGPRIMOD = ''O'')' ; 
END IF ; 
  
IF ( P_CODEVOLET <> 0 ) THEN 
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' AND KDCKAKID = ' CONCAT P_CODEVOLET ; 
END IF ; 
  
IF ( P_CODEBLOC <> 0 ) THEN 
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' AND KDCKAEID = ' CONCAT P_CODEBLOC ; 
END IF ; 
  
SET V_QUERY = TRIM ( V_QUERY ) CONCAT ' AND EXISTS (SELECT G2.KDEID 
                                                                                                                                                                                                                             FROM KPGARAN G2 
                                                                                                                                                                                                                                            WHERE G1.KDEIPB = G2.KDEIPB AND G1.KDEALX = G2.KDEALX AND G1.KDETYP = G2.KDETYP 
                                                                                                                                                                                                                                                           AND G1.KDEGAN IN (''A'', ''B'', ''C'') AND G1.KDEFOR = G2.KDEFOR AND G1.KDEOPT = G2.KDEOPT
                                                                                                                                                                                                                                                           AND (G1.KDESEQ = G2.KDESEM OR G1.KDESEM = 0)) 
                                                                                                                                                                  ORDER BY CODE' ; 
END CASE ; 
  
  
  
PREPARE SQL_STATEMENT FROM V_QUERY ; 
OPEN CURSOR1 ; 
  
END P1  ; 
  

  

