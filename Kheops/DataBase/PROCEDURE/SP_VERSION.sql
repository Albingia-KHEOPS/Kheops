﻿
CREATE OR REPLACE PROCEDURE SP_VERSION(
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE CHAR(1) , 
	IN P_DATESAISIE CHAR(8) , 
	IN P_HEURESAISIE CHAR(4) , 
	IN P_DATESYSTEME CHAR(8) , 
	IN P_USER CHAR(15) , 
	IN P_TRAITEMENT CHAR(1) ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
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
  
--VALEURS POSSIBLES DANS P_TRAITEMENT : V (VERSIONNING D'UNE OFFRE) 
DECLARE V_ETAT VARCHAR ( 1 ) DEFAULT '' ; 
DECLARE V_SITUATION VARCHAR ( 1 ) DEFAULT '' ; 
DECLARE V_NEWVERSION INTEGER DEFAULT 0 ; 
DECLARE V_NEWCODEINVEN INTEGER DEFAULT 0 ; 
DECLARE V_MODE_COPIE CHAR ( 7 ) DEFAULT 'VERSION' ; 
DECLARE V_NEWID INTEGER DEFAULT 0 ; 
DECLARE V_NBLIGNE INTEGER DEFAULT 0 ; 
DECLARE V_YEARNOW VARCHAR ( 4 ) DEFAULT '' ; 
DECLARE V_MONTHNOW VARCHAR ( 2 ) DEFAULT '' ; 
DECLARE V_DAYNOW VARCHAR ( 2 ) DEFAULT '' ; 
DECLARE V_NEWDESIASSU INTEGER DEFAULT 0 ; 
DECLARE V_COURTGES INTEGER DEFAULT 0 ; 
DECLARE V_COURTAPP INTEGER DEFAULT 0 ; 
  
SET V_YEARNOW = SUBSTR ( P_DATESYSTEME , 1 , 4 ) ; 
SET V_MONTHNOW = SUBSTR ( P_DATESYSTEME , 5 , 2 ) ; 
SET V_DAYNOW = SUBSTR ( P_DATESYSTEME , 7 , 2 ) ; 

SET P_CODEOFFRE = LPAD ( TRIM ( P_CODEOFFRE ) , 9 , ' ');
  
SELECT PBETA , PBSIT INTO V_ETAT , V_SITUATION FROM YPOBASE WHERE PBIPB = P_CODEOFFRE AND PBALX = P_VERSION AND PBTYP = P_TYPE ; 
  
IF ( ( V_ETAT = 'V' ) OR ( (V_ETAT = 'N' OR V_ETAT = 'A') AND V_SITUATION = 'W' ) ) THEN 
  
	SELECT MAX ( PBALX ) + 1 INTO V_NEWVERSION FROM YPOBASE WHERE PBIPB = P_CODEOFFRE AND PBTYP = P_TYPE ; 
	 
	DELETE FROM KPCOPID WHERE KFLTYP = P_TYPE AND KFLIPB = P_CODEOFFRE AND KFLALX = P_VERSION ; 
	 
	CALL SP_CPOBASE ( P_CODEOFFRE , P_VERSION , P_TYPE , P_DATESAISIE , P_HEURESAISIE , P_DATESYSTEME , P_USER , V_NEWVERSION , P_TRAITEMENT ) ; 
	 
	INSERT INTO KPCTRLE ( KEVTYP , KEVIPB , KEVALX , KEVETAPE , KEVETORD , KEVORDR , KEVPERI , KEVRSQ , KEVOBJ , KEVKBEID , KEVFOR , 
	KEVOPT , KEVNIVM , KEVCRU , KEVCRD , KEVCRH , KEVMAJU , KEVMAJD , KEVMAJH ) 
	( SELECT KEVTYP , KEVIPB , V_NEWVERSION , KEVETAPE , KEVETORD , KEVORDR , KEVPERI , KEVRSQ , KEVOBJ , KEVKBEID , KEVFOR , 
	KEVOPT , KEVNIVM , P_USER , P_DATESYSTEME , KEVCRH , P_USER , P_DATESYSTEME , KEVMAJH 
	FROM KPCTRLE WHERE KEVIPB = P_CODEOFFRE AND KEVALX = P_VERSION AND KEVPERI <> 'COT' AND KEVPERI <> 'FIN' ) ; 
	 
	INSERT INTO YPRTENT ( JDIPB , JDALX , JDSHT , JDENC , JDITC , JDVAL , JDVAA , JDVAW , JDVAT , JDVAU , JDVAH , JDDRQ , JDNBR , JDTXL , 
	JDTRR , JDXCM , JDNEX , JDNPA , JDAFC , JDAFR , JDATT , JDCNA , JDCNC , JDINA , JDIND , JDIXC , JDIXF , JDIXL , JDIXP , JDIVO , JDIVA , JDIVW , 
	JDMHT , JDREA , JDREB , JDREH , JDDPV , JDGAU , JDGVL , JDGUN , JDPBN , JDPBS , JDPBR , JDPBT , JDPBC , JDPBP , JDPBA , JDRCG , JDCCG , JDRCS , 
	JDCCS , JDCLV , JDAGM , JDLCV , JDLCA , JDLCW , JDLCU , JDLCE , JDDPA , JDDPM , JDDPJ , JDFPA , JDFPM , JDFPJ , JDPEA , JDPEM , JDPEJ , JDACQ , 
	JDTMC , JDTFO , JDTFT , JDTFF , JDTFP , JDPRO , JDTMI , JDTFM , JDTMA , JDTMG , JDCMC , JDCFO , JDCFT , JDCFH , JDCHT , JDCTT , JDCCP , JDEHH , 
	JDEHC , JDSMP , JDIVX , JDTCR , JDNPG , JDEDI , JDEDN , JDEDA , JDEDM , JDEDJ , JDEHI , JDIAX , JDTED , JDDOO , JDRUA , JDRUM , JDRUJ , JDECG , 
	JDECP , JDAPT , JDAPR , JDAAT , JDAAR , JDACR , JDACV , JDAXT , JDAXC , JDAXF , JDAXM , JDAXG , JDATA , JDATX , JDAUT , JDAUF , 
	JDLTA , JDLTASP , JDLDEB, JDLDEH, JDLFIN, JDLFIH, JDLDUR, JDLDUU ) 
	( SELECT JDIPB , V_NEWVERSION , JDSHT , JDENC , JDITC , JDVAL , JDVAA , JDVAW , JDVAT , JDVAU , JDVAH , JDDRQ , JDNBR , JDTXL , 
	JDTRR , JDXCM , JDNEX , JDNPA , JDAFC , JDAFR , JDATT , JDCNA , JDCNC , JDINA , JDIND , JDIXC , JDIXF , JDIXL , JDIXP , JDIVO , JDIVA , JDIVW , 
	JDMHT , JDREA , JDREB , JDREH , JDDPV , JDGAU , JDGVL , JDGUN , JDPBN , JDPBS , JDPBR , JDPBT , JDPBC , JDPBP , JDPBA , JDRCG , JDCCG , JDRCS , 
	JDCCS , JDCLV , JDAGM , JDLCV , JDLCA , JDLCW , JDLCU , JDLCE , JDDPA , JDDPM , JDDPJ , JDFPA , JDFPM , JDFPJ , JDPEA , JDPEM , JDPEJ , JDACQ , 
	JDTMC , JDTFO , JDTFT , JDTFF , JDTFP , JDPRO , JDTMI , JDTFM , JDTMA , JDTMG , JDCMC , JDCFO , JDCFT , JDCFH , JDCHT , JDCTT , JDCCP , JDEHH , 
	JDEHC , JDSMP , JDIVX , JDTCR , JDNPG , JDEDI , JDEDN , JDEDA , JDEDM , JDEDJ , JDEHI , JDIAX , JDTED , JDDOO , JDRUA , JDRUM , JDRUJ , JDECG , 
	JDECP , JDAPT , JDAPR , JDAAT , JDAAR , JDACR , JDACV , JDAXT , JDAXC , JDAXF , JDAXM , JDAXG , JDATA , JDATX , JDAUT , JDAUF , 
	JDLTA , JDLTASP , JDLDEB, JDLDEH, JDLFIN, JDLFIH, JDLDUR, JDLDUU 
	FROM YPRTENT WHERE JDIPB = P_CODEOFFRE AND JDALX = P_VERSION ) ; 
	 
	FOR CURSOR_ASSU AS FREE_LIST CURSOR FOR 
		SELECT PCIAS IDASSU , PCDESI DESIASSU FROM YPOASSU WHERE PCTYP = P_TYPE AND PCIPB = P_CODEOFFRE AND PCALX = P_VERSION 
	DO 
		SET V_NEWDESIASSU = 0 ; 
		IF ( DESIASSU > 0 ) THEN 
			CALL SP_NCHRONO ( 'KADCHR' , V_NEWDESIASSU ) ; 
			INSERT INTO KPDESI ( KADCHR , KADTYP , KADIPB , KADALX , KADPERI , KADRSQ , KADOBJ , KADDESI ) 
			( SELECT V_NEWDESIASSU , KADTYP , KADIPB , KADALX , KADPERI , KADRSQ , KADOBJ , KADDESI FROM KPDESI WHERE KADCHR = DESIASSU ) ; 
		END IF ; 
		INSERT INTO YPOASSU ( PCTYP , PCIPB , PCALX , PCIAS , PCPRI , PCQL1 , PCQL2 , PCQL3 , PCQLD , PCCNR , PCASS , PCSCP , PCDESI ) 
		( SELECT PCTYP , PCIPB , V_NEWVERSION , PCIAS , PCPRI , PCQL1 , PCQL2 , PCQL3 , PCQLD , PCCNR , PCASS , PCSCP , V_NEWDESIASSU 
		FROM YPOASSU WHERE PCTYP = P_TYPE AND PCIPB = P_CODEOFFRE AND PCALX = P_VERSION AND PCIAS = IDASSU AND PCDESI = DESIASSU ) ; 
	END FOR ; 

	INSERT INTO YPOCOAS ( PHTYP , PHIPB , PHALX , PHTAP , PHCIE , PHIN5 , PHPOL , PHAPP , PHCOM , PHTXF , PHAFR , PHEPA , PHEPM , PHEPJ , 
	PHFPA , PHFPM , PHFPJ ) 
	( SELECT PHTYP , PHIPB , V_NEWVERSION , PHTAP , PHCIE , PHIN5 , PHPOL , PHAPP , PHCOM , PHTXF , PHAFR , PHEPA , PHEPM , PHEPJ , 
	PHFPA , PHFPM , PHFPJ 
	FROM YPOCOAS WHERE PHTYP = P_TYPE AND PHIPB = P_CODEOFFRE AND PHALX = P_VERSION ) ; 
	 
	 --AMO 2365 
	INSERT INTO YPOCOUR ( PFTYP , PFIPB , PFALX , PFCTI , PFICT , PFSAA , PFSAM , PFSAJ , PFSAH , PFSIT , PFSTA , PFSTM , PFSTJ , PFGES , 
	PFSOU , PFCOM , PFOCT , PFXCM , PFXCN ) 
	( SELECT PFTYP , PFIPB , V_NEWVERSION , PFCTI , PFICT , V_YEARNOW , V_MONTHNOW , V_DAYNOW , PFSAH , PFSIT , PFSTA , PFSTM , PFSTJ , PFGES , 
	PFSOU , PFCOM , PFOCT , PFXCM , PFXCN 
	FROM YPOCOUR WHERE PFTYP = P_TYPE AND PFIPB = P_CODEOFFRE AND PFALX = P_VERSION ) ; 
	 --AMO 2365 
	SELECT PBICT , PBCTA INTO V_COURTGES , V_COURTAPP FROM YPOBASE WHERE PBIPB = P_CODEOFFRE AND PBTYP = P_TYPE AND PBALX = P_VERSION ; 
  
	IF ( V_COURTGES <> V_COURTAPP ) THEN 
		DELETE FROM YPOCOUR WHERE PFIPB = P_CODEOFFRE AND PFALX = V_NEWVERSION AND PFTYP = P_TYPE AND PFCTI = 'O' ; 
		 
		SET V_NBLIGNE = 0 ; 
		SELECT COUNT ( * ) INTO V_NBLIGNE FROM YPOCOUR 
		WHERE PFIPB = P_CODEOFFRE AND PFALX = V_NEWVERSION AND PFTYP = P_TYPE AND PFCTI = 'A' ; 
	 
		IF ( V_NBLIGNE > 0 ) THEN 
			UPDATE YPOCOUR SET PFICT = V_COURTGES 
			WHERE PFIPB = P_CODEOFFRE AND PFALX = V_NEWVERSION AND PFTYP = P_TYPE AND PFCTI = 'A' ; 
		ELSE 
			INSERT INTO YPOCOUR 
				( PFIPB , PFALX , PFTYP , PFCTI , PFICT ) 
			VALUES 
				( P_CODEOFFRE , V_NEWVERSION , P_TYPE , 'A' , V_COURTGES ) ;				 
		END IF ; 
		 
	END IF ; 
	 
	CALL SP_CKPENT ( P_CODEOFFRE , P_VERSION , P_TYPE , '' , V_NEWVERSION , P_TRAITEMENT , V_MODE_COPIE ) ; 
	 
	CALL SP_CRSQ ( P_CODEOFFRE , P_VERSION , P_TYPE , V_NEWVERSION , '' , 0 , P_TRAITEMENT , '' , 0 , V_MODE_COPIE ) ; 
	 
	CALL SP_ADR ( P_CODEOFFRE , P_VERSION , P_TYPE , V_NEWVERSION , '' , 0 , P_TRAITEMENT ) ; 
	 
	CALL SP_CFORMUL ( P_CODEOFFRE , P_VERSION , P_TYPE , V_NEWVERSION , '' , 0 , P_DATESYSTEME , P_USER , P_TRAITEMENT , '' , 0 , V_MODE_COPIE ) ; 
	 
	CALL SP_CINVEN ( P_CODEOFFRE , P_VERSION , P_TYPE , 0 , V_NEWVERSION , '' , 0 , P_TRAITEMENT , '' , 0 , V_MODE_COPIE , V_NEWCODEINVEN ) ; 
	 
	CALL SP_CKCOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , P_CODEOFFRE , V_NEWVERSION , P_TYPE ) ; 
	 
	CALL SP_CINFOSP ( P_CODEOFFRE , P_VERSION , P_TYPE , V_NEWVERSION , '' , 0 , P_TRAITEMENT , '' , 0 , '' ) ; 
	CALL SP_CCLAUSE ( P_CODEOFFRE , P_VERSION , P_TYPE , V_NEWVERSION , '' , 0 , P_TRAITEMENT , '' , 0 , V_MODE_COPIE ) ; 
	 
	CALL SP_COPYCLAUDT ( P_CODEOFFRE , P_VERSION , P_TYPE , P_CODEOFFRE , V_NEWVERSION , P_TYPE ) ; 
	 
	CALL SP_CINTERV ( P_CODEOFFRE , P_VERSION , P_TYPE , P_CODEOFFRE , V_NEWVERSION , P_TYPE , V_MODE_COPIE ) ; 
	CALL SP_COPOP ( P_CODEOFFRE , P_VERSION , P_TYPE , P_CODEOFFRE , P_TYPE , V_NEWVERSION ) ; 
	CALL SP_COPIEDOCEXT ( P_CODEOFFRE , P_VERSION , P_TYPE , P_CODEOFFRE , V_NEWVERSION , P_TYPE , V_MODE_COPIE , P_DATESYSTEME , P_USER ) ; 
	 
	FOR LOOP_DBL AS FREE_LIST CURSOR FOR 
		SELECT KAFTYP TYP , KAFIPB IPB , KAFALX ALX , KAFICT ICT , KAFSOU SOU , KAFSAID SAID , KAFSAIH SAIH , KAFSIT SIT , KAFSITD SITD , KAFSITH SITH , KAFSITU SITU , 
			KAFCRD CRD , KAFCRU CRU , KAFACT ACT , KAFMOT MOT , KAFIN5 IN5 , KAFOCT OCT 
		FROM KPODBLS 
		WHERE KAFIPB = P_CODEOFFRE AND KAFALX = P_VERSION AND KAFTYP = P_TYPE 
	DO 
		CALL SP_NCHRONO ( 'KAFID' , V_NEWID ) ; 
		INSERT INTO KPODBLS 
			( KAFID , KAFTYP , KAFIPB , KAFALX , KAFICT , KAFSOU , KAFSAID , KAFSAIH , KAFSIT , KAFSITD , KAFSITH , KAFSITU , 
				KAFCRD , KAFCRU , KAFACT , KAFMOT , KAFIN5 , KAFOCT ) 
		VALUES 
			( V_NEWID , TYP , IPB , V_NEWVERSION , ICT , SOU , SAID , SAIH , SIT , SITD , SITH , SITU , 
			CRD , CRU , ACT , MOT , IN5 , OCT ) ; 
	END FOR ; 
  
	--IF ( P_VERSION <> V_NEWVERSION ) THEN 
	--	UPDATE YPOBASE 
	--		SET PBRMP = 'O' , 
	--		PBREL = 'N' , 
	--		PBRLD = 0		 
	--	WHERE PBIPB = P_CODEOFFRE AND PBALX <> V_NEWVERSION ; 
			 
	--END IF ; 

	DELETE FROM YOFCOUL WHERE EAIPB = P_CODEOFFRE AND EAALX <> V_NEWVERSION ;	 
	DELETE FROM KPAVTRC WHERE KHOIPB = P_CODEOFFRE AND KHOALX = P_VERSION AND KHOTYP = P_TYPE ; 
	 
END IF ; 
  
END P1  ;

