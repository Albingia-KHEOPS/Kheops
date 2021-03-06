CREATE OR REPLACE PROCEDURE [ENVCIBLE].SP_COPYCANEVAS ( 
	IN P_DATESYSTEME CHAR(8) , 
	IN P_USER CHAR(15))
	LANGUAGE SQL 
	SPECIFIC [ENVCIBLE].SP_COPYCANEVAS 
	NOT DETERMINISTIC 
	MODIFIES SQL DATA 
	CALLED ON NULL INPUT 
	SET OPTION  ALWBLK = *ALLREAD , 
	ALWCPYDTA = *OPTIMIZE , 
	COMMIT = *CHG , 
	DECRESULT = (31 , 31 , 00) , 
	DFTRDBCOL = *NONE , 
	DYNDFTCOL = *NO , 
	DYNUSRPRF = *USER , 
	SRTSEQ = *HEX   
	P1 : BEGIN ATOMIC 
	DECLARE AT_END INTEGER DEFAULT 0 ; 
	DECLARE V_QUERY CHAR ( 255 ) DEFAULT '' ; 
	DECLARE V_NEWID INTEGER DEFAULT 0 ; 
	DECLARE V_OLDCHEMIN CHAR ( 1000 ) DEFAULT '' ;
	DECLARE V_NEWCHEMIN CHAR ( 1000 ) DEFAULT '' ;
	
	-- Copie des préfixes des chemins  
	 SELECT '\\' CONCAT TRIM( KHMSRV ) CONCAT '\' CONCAT TRIM( KHMRAC ) CONCAT '\' CONCAT TRIM( KHMENV ) CONCAT TRIM( KHMCHM ) CONCAT '\' 
	 INTO V_OLDCHEMIN
	 FROM  [ENVSOURCE] . KCHEMIN WHERE KHMCLE = 'DOC_INTER' ;
	 	 	
	SELECT '\\' CONCAT TRIM( KHMSRV ) CONCAT '\' CONCAT TRIM( KHMRAC ) CONCAT '\' CONCAT TRIM( KHMENV ) CONCAT TRIM( KHMCHM ) CONCAT '\' 
	INTO V_NEWCHEMIN  FROM [ENVCIBLE] . KCHEMIN WHERE KHMCLE = 'DOC_INTER'; 
	
	-- Nettoyage des  tables 
	FOR LOOP_CNVPROD AS FREE_LIST CURSOR FOR 
		SELECT KGOID CNVID , KGOTYP CNVTYP , KGOCNVA CNVA FROM [ENVCIBLE] . KCANEV 
	DO 
		CALL [ENVCIBLE] . SP_DELETEOFFRE_DOC ( CNVA , 0 , CNVTYP ) ; 
		DELETE FROM [ENVCIBLE] . KCANEV WHERE KGOID = CNVID ; 
	END FOR ; 
	
	FOR LOOP_CANEVAS AS FREE_LIST CURSOR FOR 
		SELECT KGOID CNVID , KGOTYP CNVTYP , KGOCNVA CNVA , KGODESC CNVDESC , KGOKAIID CNVCIBLEID , KGOCDEF CNVCDEF , KGOCRU CNVCRU , 
			KGOCRD CNVCRD , KGOCRH CNVCRH , KGOMAJU CNVMAJU , KGOMAJD CNVMAJD , KGOMAJH CNVMAJH , KGOSIT CNVSIT 
		FROM [ENVSOURCE] . KCANEV 
	DO 
		CALL [ENVCIBLE] . SP_DELETEOFFRE_DOC ( CNVA , 0 , CNVTYP ) ; 
		CALL [ENVCIBLE] . SP_NCHRONO ( 'KGOID' , V_NEWID ) ;
		INSERT INTO [ENVCIBLE] . KCANEV 
			( KGOID , KGOTYP , KGOCNVA , KGODESC , KGOKAIID , KGOCDEF , KGOCRU , 
			KGOCRD , KGOCRH , KGOMAJU , KGOMAJD , KGOMAJH , KGOSIT ) 
		VALUES 
			( V_NEWID , CNVTYP , CNVA , CNVDESC , CNVCIBLEID , CNVCDEF , CNVCRU , 
			CNVCRD , CNVCRH , CNVMAJU , CNVMAJD , CNVMAJH , CNVSIT ) ; 
			
		CALL [ENVCIBLE] . SP_COPYINFOCNV ( CNVA , CNVTYP ) ;			
		
		CALL [ENVCIBLE] . SP_COPIEDOCEXT_DOC ( CNVA, 0 , CNVTYP , V_OLDCHEMIN , V_NEWCHEMIN , P_DATESYSTEME , P_USER ) ;	
			
	END FOR ; 
	
END P1 
