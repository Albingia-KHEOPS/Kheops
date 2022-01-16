﻿CREATE OR REPLACE VIEW ZALBINKHEO.V_FORMGARREADONLY ( 
	CODEOFFRE , 
	"VERSION" , 
	TYPEOFFRE , 
	CODEFOR , 
	CODEOPT , 
	CODECIBLE , 
	TYPEENRG , 
	MODELEID , 
	MODELE1 , 
	MODELE2 , 
	MODELE3 , 
	MODELE4 , 
	VOLETORDRE , 
	BLOCORDRE , 
	TRI1 , 
	TRI2 , 
	TRI3 , 
	TRI4 , 
	CODEOPTION , 
	GUIDV , 
	CODEVOLET , 
	DESCRVOLET , 
	CARACVOLET , 
	GUIDVOLET , 
	VOLETCOLLAPSE FOR COLUMN VOLET00001 , 
	CHECKV , 
	GUIDB , 
	CODEBLOC , 
	DESCRBLOC , 
	CARACBLOC , 
	GUIDBLOC , 
	CHECKB , 
	CODEBLOCINCOMP FOR COLUMN CODEB00001 , 
	BLOCINCOMP , 
	CODEBLOCASSOC FOR COLUMN CODEB00002 , 
	BLOCASSOC , 
	GUIDNIV1 , 
	ACTIONNIV1 , 
	CODENIV1 , 
	CODEGARNIV1 FOR COLUMN CODEG00001 , 
	DESCRNIV1 , 
	CARACNIV1 , 
	NATURENIV1 , 
	NATUREPARAMNIV1 FOR COLUMN NATUR00001 , 
	CODEPARENTNIV1 FOR COLUMN CODEP00001 , 
	CODENIV1NIV1 FOR COLUMN CODEN00001 , 
	FLAGMODIFNIV1 FOR COLUMN FLAGM00001 , 
	PARAMNATMODNIV1 FOR COLUMN PARAM00001 , 
	CODEGARINCOMPNIV1 FOR COLUMN CODEG00002 , 
	CODEGARASSOCNIV1 FOR COLUMN CODEG00003 , 
	CODEGARAALTNIV1 FOR COLUMN CODEG00004 , 
	INVENPOSSIBLE1 FOR COLUMN INVEN00001 , 
	TYPEINVEN1 , 
	CODEINVEN1 , 
	DATDEB1 , 
	DATFIN1 , 
	HEUFIN1 , 
	DUREE1 , 
	DURUNI1 , 
	ALIMNIV1 , 
	GUIDNIV2 , 
	CODENIV2 , 
	CODEGARNIV2 FOR COLUMN CODEG00005 , 
	DESCRNIV2 , 
	CARACNIV2 , 
	NATURENIV2 , 
	NATUREPARAMNIV2 FOR COLUMN NATUR00002 , 
	CODEPARENTNIV2 FOR COLUMN CODEP00002 , 
	CODENIV1NIV2 FOR COLUMN CODEN00002 , 
	FLAGMODIFNIV2 FOR COLUMN FLAGM00002 , 
	PARAMNATMODNIV2 FOR COLUMN PARAM00002 , 
	CODEGARINCOMPNIV2 FOR COLUMN CODEG00006 , 
	CODEGARASSOCNIV2 FOR COLUMN CODEG00007 , 
	CODEGARAALTNIV2 FOR COLUMN CODEG00008 , 
	INVENPOSSIBLE2 FOR COLUMN INVEN00002 , 
	TYPEINVEN2 , 
	CODEINVEN2 , 
	DATDEB2 , 
	DATFIN2 , 
	HEUFIN2 , 
	DUREE2 , 
	DURUNI2 , 
	GUIDNIV3 , 
	CODENIV3 , 
	CODEGARNIV3 FOR COLUMN CODEG00009 , 
	DESCRNIV3 , 
	CARACNIV3 , 
	NATURENIV3 , 
	NATUREPARAMNIV3 FOR COLUMN NATUR00003 , 
	CODEPARENTNIV3 FOR COLUMN CODEP00003 , 
	CODENIV1NIV3 FOR COLUMN CODEN00003 , 
	FLAGMODIFNIV3 FOR COLUMN FLAGM00003 , 
	PARAMNATMODNIV3 FOR COLUMN PARAM00003 , 
	CODEGARINCOMPNIV3 FOR COLUMN CODEG00010 , 
	CODEGARASSOCNIV3 FOR COLUMN CODEG00011 , 
	CODEGARAALTNIV3 FOR COLUMN CODEG00012 , 
	INVENPOSSIBLE3 FOR COLUMN INVEN00003 , 
	TYPEINVEN3 , 
	CODEINVEN3 , 
	DATDEB3 , 
	DATFIN3 , 
	HEUFIN3 , 
	DUREE3 , 
	DURUNI3 , 
	GUIDNIV4 , 
	CODENIV4 , 
	CODEGARNIV4 FOR COLUMN CODEG00013 , 
	DESCRNIV4 , 
	CARACNIV4 , 
	NATURENIV4 , 
	NATUREPARAMNIV4 FOR COLUMN NATUR00004 , 
	CODEPARENTNIV4 FOR COLUMN CODEP00004 , 
	CODENIV1NIV4 FOR COLUMN CODEN00004 , 
	FLAGMODIFNIV4 FOR COLUMN FLAGM00004 , 
	PARAMNATMODNIV4 FOR COLUMN PARAM00004 , 
	CODEGARINCOMPNIV4 FOR COLUMN CODEG00014 , 
	CODEGARASSOCNIV4 FOR COLUMN CODEG00015 , 
	CODEGARAALTNIV4 FOR COLUMN CODEG00016 , 
	INVENPOSSIBLE4 FOR COLUMN INVEN00004 , 
	TYPEINVEN4 , 
	CODEINVEN4 , 
	DATDEB4 , 
	DATFIN4 , 
	HEUFIN4 , 
	DUREE4 , 
	DURUNI4 , 
	APPLIQUEA ) 
	AS 
	(SELECT OPTV.KDCIPB CODEOFFRE, OPTV.KDCALX VERSION, OPTV.KDCTYP TYPEOFFRE, OPTV.KDCFOR CODEFOR, OPTV.KDCOPT CODEOPT, KAPKAIID CODECIBLE, OPTV.KDCTENG TYPEENRG,  
																		OPTB.KDCKARID MODELEID, GAR1.C2MGA MODELE1, GAR2.C2MGA MODELE2, GAR3.C2MGA MODELE3, GAR4.C2MGA MODELE4,  
																		KAPORDRE VOLETORDRE, KAQORDRE BLOCORDRE, GAR1.C2TRI TRI1, GAR2.C2TRI TRI2, GAR3.C2TRI TRI3, GAR4.C2TRI TRI4,  
																			  
																		OPTV.KDCKDBID CODEOPTION , OPTV.KDCID GUIDV , KAKVOLET CODEVOLET , KAKDESC DESCRVOLET , KAPCAR CARACVOLET , KAKID GUIDVOLET , KAKPRES VOLETCOLLAPSE, IFNULL ( OPTV.KDCFLAG , 0 ) CHECKV ,  
																		OPTB.KDCID GUIDB , KAEBLOC CODEBLOC , KAEDESC DESCRBLOC , KAQCAR CARACBLOC , KAEID GUIDBLOC , IFNULL ( OPTB.KDCFLAG , 0 ) CHECKB , INCOMP.KGJIDBLO1 CODEBLOCINCOMP , INCOMP.KGJBLO1 BLOCINCOMP , ASSOCIE.KGJIDBLO1 CODEBLOCASSOC , ASSOCIE.KGJBLO1 BLOCASSOC ,  
																		  
																		GARW1.KDEID GUIDNIV1 , KDFGAN ACTIONNIV1 , GAR1.C2SEQ CODENIV1 , GAR1.C2GAR CODEGARNIV1 , GARAN1.GADEA DESCRNIV1 , GAR1.C2CAR CARACNIV1 , GAR1.C2NAT NATURENIV1 , GARW1.KDEGAN NATUREPARAMNIV1 , GAR1.C2SEM CODEPARENTNIV1 , GAR1.C2SE1 CODENIV1NIV1 , GARW1.KDEMODI FLAGMODIFNIV1 , GARW1.KDEPNTM PARAMNATMODNIV1 , GAAI1.C5SEM CODEGARINCOMPNIV1 , GAAA1.C5SEM CODEGARASSOCNIV1 , GAR1.C2ALT CODEGARAALTNIV1 ,  
																		GARAN1.GAINV INVENPOSSIBLE1 , GARAN1.GATYI TYPEINVEN1 ,  
																		IFNULL ( ( SELECT KBGKBEID FROM ZALBINKHEO.KPINVAPP WHERE KBGIPB = OPTV.KDCIPB AND KBGALX = OPTV.KDCALX AND KBGTYP = OPTV.KDCTYP AND KBGPERI = 'GA' AND KBGFOR = OPTV.KDCFOR AND KBGGAR = GARW1.KDEGARAN ) , 0 ) CODEINVEN1 ,  
																		( SELECT ZALBINKHEO.F_GTIESORTIEREADONLY ( CAST ( OPTV.KDCIPB AS VARCHAR ( 9 ) ) , CAST ( OPTV.KDCALX AS INTEGER ) , CAST ( OPTV.KDCTYP AS VARCHAR ( 1 ) ) , CAST ( OPTV.KDCFOR AS INTEGER ) , CAST ( OPTV.KDCOPT AS INTEGER ) , CAST ( GARW1.KDEID AS INTEGER ) ) FROM SYSIBM.SYSDUMMY1 ) DATDEB1 , GARW1.KDEDATFIN DATFIN1 , GARW1.KDEHEUFIN HEUFIN1 , GARW1.KDEDUREE DUREE1 , GARW1.KDEDURUNI DURUNI1 ,  
																		/* 2016-02-07 : CORRECTION BUG 2246 */  
																		/* CASE WHEN GARW1.KDEALA = 'B' OR GARW1.KDEALA = 'C' THEN GARW1.KDEALA ELSE GARW1.KDEPALA END ALIMNIV1,   */  
																		GARW1.KDEALA ALIMNIV1,  
																		  
																		GARW2.KDEID GUIDNIV2 , GAR2.C2SEQ CODENIV2 , GAR2.C2GAR CODEGARNIV2 , GARAN2.GADEA DESCRNIV2 , GAR2.C2CAR CARACNIV2 , GAR2.C2NAT NATURENIV2 , GARW2.KDEGAN NATUREPARAMNIV2 , GAR2.C2SEM CODEPARENTNIV2 , GAR2.C2SE1 CODENIV1NIV2 , GARW2.KDEMODI FLAGMODIFNIV2 , GARW2.KDEPNTM PARAMNATMODNIV2 , GAAI2.C5SEM CODEGARINCOMPNIV2 , GAAA2.C5SEM CODEGARASSOCNIV2 , GAR2.C2ALT CODEGARAALTNIV2 ,  
																		GARAN2.GAINV INVENPOSSIBLE2 , GARAN2.GATYI TYPEINVEN2 ,  
																		IFNULL ( ( SELECT KBGKBEID FROM ZALBINKHEO.KPINVAPP WHERE KBGIPB = OPTV.KDCIPB AND KBGALX = OPTV.KDCALX AND KBGTYP = OPTV.KDCTYP AND KBGPERI = 'GA' AND KBGFOR = OPTV.KDCFOR AND KBGGAR = GARW2.KDEGARAN ) , 0 ) CODEINVEN2 ,  
																		( SELECT ZALBINKHEO.F_GTIESORTIEREADONLY ( CAST ( OPTV.KDCIPB AS VARCHAR ( 9 ) ) , CAST ( OPTV.KDCALX AS INTEGER ) , CAST ( OPTV.KDCTYP AS VARCHAR ( 1 ) ) , CAST ( OPTV.KDCFOR AS INTEGER ) , CAST ( OPTV.KDCOPT AS INTEGER ) , CAST ( GARW2.KDEID AS INTEGER ) ) FROM SYSIBM.SYSDUMMY1 ) DATDEB2 , GARW2.KDEDATFIN DATFIN2 , GARW2.KDEHEUFIN HEUFIN2 , GARW2.KDEDUREE DUREE2 , GARW2.KDEDURUNI DURUNI2 ,  
																		  
																		GARW3.KDEID GUIDNIV3 , GAR3.C2SEQ CODENIV3 , GAR3.C2GAR CODEGARNIV3 , GARAN3.GADEA DESCRNIV3 , GAR3.C2CAR CARACNIV3 , GAR3.C2NAT NATURENIV3 , GARW3.KDEGAN NATUREPARAMNIV3 , GAR3.C2SEM CODEPARENTNIV3 , GAR3.C2SE1 CODENIV1NIV3 , GARW3.KDEMODI FLAGMODIFNIV3 , GARW3.KDEPNTM PARAMNATMODNIV3 , GAAI3.C5SEM CODEGARINCOMPNIV3 , GAAA3.C5SEM CODEGARASSOCNIV3 , GAR3.C2ALT CODEGARAALTNIV3 ,  
																		GARAN3.GAINV INVENPOSSIBLE3 , GARAN3.GATYI TYPEINVEN3 ,  
																		IFNULL ( ( SELECT KBGKBEID FROM ZALBINKHEO.KPINVAPP WHERE KBGIPB = OPTV.KDCIPB AND KBGALX = OPTV.KDCALX AND KBGTYP = OPTV.KDCTYP AND KBGPERI = 'GA' AND KBGFOR = OPTV.KDCFOR AND KBGGAR = GARW3.KDEGARAN ) , 0 ) CODEINVEN3 ,  
																		( SELECT ZALBINKHEO.F_GTIESORTIEREADONLY ( CAST ( OPTV.KDCIPB AS VARCHAR ( 9 ) ) , CAST ( OPTV.KDCALX AS INTEGER ) , CAST ( OPTV.KDCTYP AS VARCHAR ( 1 ) ) , CAST ( OPTV.KDCFOR AS INTEGER ) , CAST ( OPTV.KDCOPT AS INTEGER ) , CAST ( GARW3.KDEID AS INTEGER ) ) FROM SYSIBM.SYSDUMMY1 ) DATDEB3 , GARW3.KDEDATFIN DATFIN3 , GARW3.KDEHEUFIN HEUFIN3 , GARW3.KDEDUREE DUREE3 , GARW3.KDEDURUNI DURUNI3 ,  
																		  
																		GARW4.KDEID GUIDNIV4 , GAR4.C2SEQ CODENIV4 , GAR4.C2GAR CODEGARNIV4 , GARAN4.GADEA DESCRNIV4 , GAR4.C2CAR CARACNIV4 , GAR4.C2NAT NATURENIV4 , GARW4.KDEGAN NATUREPARAMNIV4 , GAR4.C2SEM CODEPARENTNIV4 , GAR4.C2SE1 CODENIV1NIV4 , GARW4.KDEMODI FLAGMODIFNIV4 , GARW4.KDEPNTM PARAMNATMODNIV4 , GAAI4.C5SEM CODEGARINCOMPNIV4 , GAAA4.C5SEM CODEGARASSOCNIV4 , GAR4.C2ALT CODEGARAALTNIV4 ,  
																		GARAN4.GAINV INVENPOSSIBLE4 , GARAN4.GATYI TYPEINVEN4 ,  
																		IFNULL ( ( SELECT KBGKBEID FROM ZALBINKHEO.KPINVAPP WHERE KBGIPB = OPTV.KDCIPB AND KBGALX = OPTV.KDCALX AND KBGTYP = OPTV.KDCTYP AND KBGPERI = 'GA' AND KBGFOR = OPTV.KDCFOR AND KBGGAR = GARW4.KDEGARAN ) , 0 ) CODEINVEN4 ,  
																		( SELECT ZALBINKHEO.F_GTIESORTIEREADONLY ( CAST ( OPTV.KDCIPB AS VARCHAR ( 9 ) ) , CAST ( OPTV.KDCALX AS INTEGER ) , CAST ( OPTV.KDCTYP AS VARCHAR ( 1 ) ) , CAST ( OPTV.KDCFOR AS INTEGER ) , CAST ( OPTV.KDCOPT AS INTEGER ) , CAST ( GARW4.KDEID AS INTEGER ) ) FROM SYSIBM.SYSDUMMY1 ) DATDEB4 , GARW4.KDEDATFIN DATFIN4 , GARW4.KDEHEUFIN HEUFIN4 , GARW4.KDEDUREE DUREE4 , GARW4.KDEDURUNI DURUNI4 ,  
																		  
																		( SELECT COUNT ( * ) FROM ZALBINKHEO.KPOPTAP WHERE KDDIPB = OPTV.KDCIPB AND KDDALX = OPTV.KDCALX AND KDDTYP = OPTV.KDCTYP AND KDDFOR = OPTV.KDCFOR AND KDDOPT = OPTV.KDCOPT ) APPLIQUEA  
																		FROM ZALBINKHEO.KPOPTD OPTV  
																		INNER JOIN ZALBINKHEO.KVOLET ON OPTV.KDCKAKID = KAKID  
																		INNER JOIN ZALBINKHEO.KCATVOLET ON KAKID = KAPKAKID  
																		  
																		INNER JOIN ZALBINKHEO.KPOPTD OPTB ON OPTV.KDCIPB = OPTB.KDCIPB AND OPTV.KDCALX = OPTB.KDCALX AND OPTV.KDCTYP = OPTB.KDCTYP  
																		AND OPTV.KDCFOR = OPTB.KDCFOR AND OPTV.KDCOPT = OPTB.KDCOPT AND OPTB.KDCTENG = 'B' AND OPTV.KDCKAKID = OPTB.KDCKAKID  
																		INNER JOIN ZALBINKHEO.KBLOC ON OPTB.KDCKAEID = KAEID  
																		INNER JOIN ZALBINKHEO.KCATBLOC ON KAEID = KAQKAEID AND KAQKAPID = KAPID  
																		LEFT JOIN ZALBINKHEO.KBLOREL INCOMP ON KAEID = INCOMP.KGJIDBLO2 AND INCOMP.KGJREL = 'I'  
																		LEFT JOIN ZALBINKHEO.KBLOREL ASSOCIE ON KAEID = ASSOCIE.KGJIDBLO2 AND ASSOCIE.KGJREL = 'A'	  
																		  
																		LEFT JOIN ZALBINKHEO.KPGARAN GARW1 ON GARW1.KDEIPB = OPTB.KDCIPB AND GARW1.KDEALX = OPTB.KDCALX AND GARW1.KDETYP = OPTB.KDCTYP  
																		AND GARW1.KDEFOR = OPTB.KDCFOR AND GARW1.KDEOPT = OPTB.KDCOPT AND GARW1.KDEKDCID = OPTB.KDCID AND GARW1.KDENIVEAU = 1  
																		LEFT JOIN ZALBINKMOD.YPLTGAR GAR1 ON GAR1.C2SEQ = GARW1.KDESEQ AND GAR1.C2NIV = GARW1.KDENIVEAU  
																		LEFT JOIN ZALBINKHEO.KGARAN GARAN1 ON GAR1.C2GAR = GARAN1.GAGAR  
																		LEFT JOIN ZALBINKMOD.YPLTGAL GAL1 ON GAR1.C2SEQ = GAL1.C4SEQ AND GAL1.C4TYP = 0  
																		LEFT JOIN ZALBINKMOD.YPLTGAA GAAI1 ON GARW1.KDESEQ = GAAI1.C5SEQ AND GAAI1.C5TYP = 'I'  
																		LEFT JOIN ZALBINKMOD.YPLTGAA GAAA1 ON GARW1.KDESEQ = GAAA1.C5SEQ AND GAAA1.C5TYP = 'A'  
																		LEFT JOIN ZALBINKHEO.KPGARAP ON KDFKDEID = GARW1.KDEID  
																		  
																		LEFT JOIN ZALBINKHEO.KPGARAN GARW2 ON GARW2.KDEIPB = GARW1.KDEIPB AND GARW2.KDEALX = GARW1.KDEALX AND GARW2.KDETYP = GARW1.KDETYP  
																		AND GARW2.KDEFOR = GARW1.KDEFOR AND GARW2.KDEOPT = GARW1.KDEOPT AND GARW2.KDESEM = GARW1.KDESEQ AND GARW2.KDENIVEAU = 2  
																		LEFT JOIN ZALBINKMOD.YPLTGAR GAR2 ON GAR2.C2SEQ = GARW2.KDESEQ AND GAR2.C2NIV = GARW2.KDENIVEAU  
																		LEFT JOIN ZALBINKHEO.KGARAN GARAN2 ON GAR2.C2GAR = GARAN2.GAGAR  
																		LEFT JOIN ZALBINKMOD.YPLTGAL GAL2 ON GAR2.C2SEQ = GAL2.C4SEQ AND GAL2.C4TYP = 0  
																		LEFT JOIN ZALBINKMOD.YPLTGAA GAAI2 ON GARW2.KDESEQ = GAAI2.C5SEQ AND GAAI2.C5TYP = 'I'  
																		LEFT JOIN ZALBINKMOD.YPLTGAA GAAA2 ON GARW2.KDESEQ = GAAA2.C5SEQ AND GAAA2.C5TYP = 'A'  
																		  
																		LEFT JOIN ZALBINKHEO.KPGARAN GARW3 ON GARW3.KDEIPB = GARW2.KDEIPB AND GARW3.KDEALX = GARW2.KDEALX AND GARW3.KDETYP = GARW2.KDETYP  
																		AND GARW3.KDEFOR = GARW2.KDEFOR AND GARW3.KDEOPT = GARW2.KDEOPT AND GARW3.KDESEM = GARW2.KDESEQ AND GARW3.KDENIVEAU = 3  
																		LEFT JOIN ZALBINKMOD.YPLTGAR GAR3 ON GAR3.C2SEQ = GARW3.KDESEQ AND GAR3.C2NIV = GARW3.KDENIVEAU  
																		LEFT JOIN ZALBINKHEO.KGARAN GARAN3 ON GAR3.C2GAR = GARAN3.GAGAR  
																		LEFT JOIN ZALBINKMOD.YPLTGAL GAL3 ON GAR3.C2SEQ = GAL3.C4SEQ AND GAL3.C4TYP = 0  
																		LEFT JOIN ZALBINKMOD.YPLTGAA GAAI3 ON GARW3.KDESEQ = GAAI3.C5SEQ AND GAAI3.C5TYP = 'I'  
																		LEFT JOIN ZALBINKMOD.YPLTGAA GAAA3 ON GARW3.KDESEQ = GAAA3.C5SEQ AND GAAA3.C5TYP = 'A'  
																		  
																		LEFT JOIN ZALBINKHEO.KPGARAN GARW4 ON GARW4.KDEIPB = GARW3.KDEIPB AND GARW4.KDEALX = GARW3.KDEALX AND GARW4.KDETYP = GARW3.KDETYP  
																		AND GARW4.KDEFOR = GARW3.KDEFOR AND GARW4.KDEOPT = GARW3.KDEOPT AND GARW4.KDESEM = GARW3.KDESEQ AND GARW4.KDENIVEAU = 4  
																		LEFT JOIN ZALBINKMOD.YPLTGAR GAR4 ON GAR4.C2SEQ = GARW4.KDESEQ AND GAR4.C2NIV = GARW4.KDENIVEAU  
																		LEFT JOIN ZALBINKHEO.KGARAN GARAN4 ON GAR4.C2GAR = GARAN4.GAGAR  
																		LEFT JOIN ZALBINKMOD.YPLTGAL GAL4 ON GAR4.C2SEQ = GAL4.C4SEQ AND GAL4.C4TYP = 0  
																		LEFT JOIN ZALBINKMOD.YPLTGAA GAAI4 ON GARW4.KDESEQ = GAAI4.C5SEQ AND GAAI4.C5TYP = 'I'  
																		LEFT JOIN ZALBINKMOD.YPLTGAA GAAA4 ON GARW4.KDESEQ = GAAA4.C5SEQ AND GAAA4.C5TYP = 'A'  
																		  
																		WHERE OPTV.KDCTENG = 'V') ; 
  
LABEL ON COLUMN ZALBINKHEO.V_FORMGARREADONLY 
( CODEOFFRE IS 'IPB' , 
	"VERSION" IS 'ALX' , 
	TYPEOFFRE IS 'Type O/P' , 
	CODEFOR IS 'Formule' , 
	CODEOPT IS 'Option' , 
	CODECIBLE IS 'ID KCIBLEF' , 
	TYPEENRG IS 'Type enregistrement' , 
	MODELEID IS 'Lien KCATMODELE' , 
	VOLETORDRE IS 'N° Ordre' , 
	BLOCORDRE IS 'N° ordre' , 
	CODEOPTION IS 'Lien KPOPT' , 
	GUIDV IS 'ID Unique' , 
	CODEVOLET IS 'Volet' , 
	DESCRVOLET IS 'Description' , 
	CARACVOLET IS 'Caractère' , 
	GUIDVOLET IS 'Id unique Volet' , 
	VOLETCOLLAPSE IS 'Présentat° / défaut' , 
	GUIDB IS 'ID Unique' , 
	CODEBLOC IS 'Bloc' , 
	DESCRBLOC IS 'Description' , 
	CARACBLOC IS 'Caractère' , 
	GUIDBLOC IS 'ID unique' , 
	CODEBLOCINCOMP IS 'ID bloc 1' , 
	BLOCINCOMP IS 'Bloc  1' , 
	CODEBLOCASSOC IS 'ID bloc 1' , 
	BLOCASSOC IS 'Bloc  1' , 
	GUIDNIV1 IS 'ID unique' , 
	ACTIONNIV1 IS 'Nature ''E'' / ''A''' , 
	DESCRNIV1 IS 'Abrégé garantie' , 
	NATUREPARAMNIV1 IS 'Nature Retenue' , 
	FLAGMODIFNIV1 IS 'Flag Modifié  O/N' , 
	PARAMNATMODNIV1 IS 'Param Nat modifiable' , 
	CODEGARINCOMPNIV1 IS 'Séq. gar. 2' , 
	CODEGARASSOCNIV1 IS 'Séq. gar. 2' , 
	INVENPOSSIBLE1 IS 'Invent Possible O/N' , 
	TYPEINVEN1 IS 'Type Inventaire' , 
	DATFIN1 IS 'Fin de garantie Date' , 
	HEUFIN1 IS 'Heure Fin' , 
	DUREE1 IS 'Durée' , 
	DURUNI1 IS 'Durée Unité' , 
	ALIMNIV1 IS 'Type Alimentation' , 
	GUIDNIV2 IS 'ID unique' , 
	DESCRNIV2 IS 'Abrégé garantie' , 
	NATUREPARAMNIV2 IS 'Nature Retenue' , 
	FLAGMODIFNIV2 IS 'Flag Modifié  O/N' , 
	PARAMNATMODNIV2 IS 'Param Nat modifiable' , 
	CODEGARINCOMPNIV2 IS 'Séq. gar. 2' , 
	CODEGARASSOCNIV2 IS 'Séq. gar. 2' , 
	INVENPOSSIBLE2 IS 'Invent Possible O/N' , 
	TYPEINVEN2 IS 'Type Inventaire' , 
	DATFIN2 IS 'Fin de garantie Date' , 
	HEUFIN2 IS 'Heure Fin' , 
	DUREE2 IS 'Durée' , 
	DURUNI2 IS 'Durée Unité' , 
	GUIDNIV3 IS 'ID unique' , 
	DESCRNIV3 IS 'Abrégé garantie' , 
	NATUREPARAMNIV3 IS 'Nature Retenue' , 
	FLAGMODIFNIV3 IS 'Flag Modifié  O/N' , 
	PARAMNATMODNIV3 IS 'Param Nat modifiable' , 
	CODEGARINCOMPNIV3 IS 'Séq. gar. 2' , 
	CODEGARASSOCNIV3 IS 'Séq. gar. 2' , 
	INVENPOSSIBLE3 IS 'Invent Possible O/N' , 
	TYPEINVEN3 IS 'Type Inventaire' , 
	DATFIN3 IS 'Fin de garantie Date' , 
	HEUFIN3 IS 'Heure Fin' , 
	DUREE3 IS 'Durée' , 
	DURUNI3 IS 'Durée Unité' , 
	GUIDNIV4 IS 'ID unique' , 
	DESCRNIV4 IS 'Abrégé garantie' , 
	NATUREPARAMNIV4 IS 'Nature Retenue' , 
	FLAGMODIFNIV4 IS 'Flag Modifié  O/N' , 
	PARAMNATMODNIV4 IS 'Param Nat modifiable' , 
	CODEGARINCOMPNIV4 IS 'Séq. gar. 2' , 
	CODEGARASSOCNIV4 IS 'Séq. gar. 2' , 
	INVENPOSSIBLE4 IS 'Invent Possible O/N' , 
	TYPEINVEN4 IS 'Type Inventaire' , 
	DATFIN4 IS 'Fin de garantie Date' , 
	HEUFIN4 IS 'Heure Fin' , 
	DUREE4 IS 'Durée' , 
	DURUNI4 IS 'Durée Unité' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.V_FORMGARREADONLY 
( CODEOFFRE TEXT IS 'IPB' , 
	"VERSION" TEXT IS 'ALX' , 
	TYPEOFFRE TEXT IS 'Type O/P' , 
	CODEFOR TEXT IS 'Formule' , 
	CODEOPT TEXT IS 'Option' , 
	CODECIBLE TEXT IS 'ID KCIBLEF' , 
	TYPEENRG TEXT IS 'Type enregistrement Volet Bloc' , 
	MODELEID TEXT IS 'Lien KCATMODELE' , 
	VOLETORDRE TEXT IS 'N° Ordre' , 
	BLOCORDRE TEXT IS 'N° Ordre' , 
	CODEOPTION TEXT IS 'Lien KPOPT' , 
	GUIDV TEXT IS 'ID unique' , 
	CODEVOLET TEXT IS 'Volet' , 
	DESCRVOLET TEXT IS 'Description' , 
	CARACVOLET TEXT IS 'Caractère' , 
	GUIDVOLET TEXT IS 'ID unique Volet' , 
	VOLETCOLLAPSE TEXT IS 'Présentation  par défaut' , 
	GUIDB TEXT IS 'ID unique' , 
	CODEBLOC TEXT IS 'Bloc' , 
	DESCRBLOC TEXT IS 'Description' , 
	CARACBLOC TEXT IS 'Caractère' , 
	GUIDBLOC TEXT IS 'ID unique' , 
	CODEBLOCINCOMP TEXT IS 'ID bloc  1  Lien KBLOC' , 
	BLOCINCOMP TEXT IS 'Bloc  1' , 
	CODEBLOCASSOC TEXT IS 'ID bloc  1  Lien KBLOC' , 
	BLOCASSOC TEXT IS 'Bloc  1' , 
	GUIDNIV1 TEXT IS 'ID unique' , 
	ACTIONNIV1 TEXT IS 'Nature  ''E'' Exclure   ''A'' Accordée' , 
	DESCRNIV1 TEXT IS 'Abrégé garantie' , 
	NATUREPARAMNIV1 TEXT IS 'Nature retenue' , 
	FLAGMODIFNIV1 TEXT IS 'Flag Modifié  O/N' , 
	PARAMNATMODNIV1 TEXT IS 'Paramétrage Nature modifiable' , 
	CODEGARINCOMPNIV1 TEXT IS 'Séquence garantie 2' , 
	CODEGARASSOCNIV1 TEXT IS 'Séquence garantie 2' , 
	INVENPOSSIBLE1 TEXT IS 'Inventaire possible O/N' , 
	TYPEINVEN1 TEXT IS 'Type Inventaire lien KINVTYP' , 
	DATFIN1 TEXT IS 'Fin de garantie Date' , 
	HEUFIN1 TEXT IS 'Heure Fin' , 
	DUREE1 TEXT IS 'Durée' , 
	DURUNI1 TEXT IS 'Durée Unité' , 
	ALIMNIV1 TEXT IS 'Type Alimentation' , 
	GUIDNIV2 TEXT IS 'ID unique' , 
	DESCRNIV2 TEXT IS 'Abrégé garantie' , 
	NATUREPARAMNIV2 TEXT IS 'Nature retenue' , 
	FLAGMODIFNIV2 TEXT IS 'Flag Modifié  O/N' , 
	PARAMNATMODNIV2 TEXT IS 'Paramétrage Nature modifiable' , 
	CODEGARINCOMPNIV2 TEXT IS 'Séquence garantie 2' , 
	CODEGARASSOCNIV2 TEXT IS 'Séquence garantie 2' , 
	INVENPOSSIBLE2 TEXT IS 'Inventaire possible O/N' , 
	TYPEINVEN2 TEXT IS 'Type Inventaire lien KINVTYP' , 
	DATFIN2 TEXT IS 'Fin de garantie Date' , 
	HEUFIN2 TEXT IS 'Heure Fin' , 
	DUREE2 TEXT IS 'Durée' , 
	DURUNI2 TEXT IS 'Durée Unité' , 
	GUIDNIV3 TEXT IS 'ID unique' , 
	DESCRNIV3 TEXT IS 'Abrégé garantie' , 
	NATUREPARAMNIV3 TEXT IS 'Nature retenue' , 
	FLAGMODIFNIV3 TEXT IS 'Flag Modifié  O/N' , 
	PARAMNATMODNIV3 TEXT IS 'Paramétrage Nature modifiable' , 
	CODEGARINCOMPNIV3 TEXT IS 'Séquence garantie 2' , 
	CODEGARASSOCNIV3 TEXT IS 'Séquence garantie 2' , 
	INVENPOSSIBLE3 TEXT IS 'Inventaire possible O/N' , 
	TYPEINVEN3 TEXT IS 'Type Inventaire lien KINVTYP' , 
	DATFIN3 TEXT IS 'Fin de garantie Date' , 
	HEUFIN3 TEXT IS 'Heure Fin' , 
	DUREE3 TEXT IS 'Durée' , 
	DURUNI3 TEXT IS 'Durée Unité' , 
	GUIDNIV4 TEXT IS 'ID unique' , 
	DESCRNIV4 TEXT IS 'Abrégé garantie' , 
	NATUREPARAMNIV4 TEXT IS 'Nature retenue' , 
	FLAGMODIFNIV4 TEXT IS 'Flag Modifié  O/N' , 
	PARAMNATMODNIV4 TEXT IS 'Paramétrage Nature modifiable' , 
	CODEGARINCOMPNIV4 TEXT IS 'Séquence garantie 2' , 
	CODEGARASSOCNIV4 TEXT IS 'Séquence garantie 2' , 
	INVENPOSSIBLE4 TEXT IS 'Inventaire possible O/N' , 
	TYPEINVEN4 TEXT IS 'Type Inventaire lien KINVTYP' , 
	DATFIN4 TEXT IS 'Fin de garantie Date' , 
	HEUFIN4 TEXT IS 'Heure Fin' , 
	DUREE4 TEXT IS 'Durée' , 
	DURUNI4 TEXT IS 'Durée Unité' ) ; 
  