CREATE OR REPLACE VIEW ZALBINKHEO.V_ATTESTGARAN ( 
	ATTESTID , 
	CODECONTRAT FOR COLUMN CODEC00001 , 
	VERSCONTRAT FOR COLUMN VERSC00001 , 
	TYPECONTRAT FOR COLUMN TYPEC00001 , 
	CODEFOR , 
	IDGARAN , 
	CODERSQ , 
	CODEOBJ , 
	LETTREFOR , 
	LIBFOR , 
	CODEGARAN , 
	LIBGARAN , 
	NIVGARAN , 
	SEQGARAN , 
	MASTERGARAN FOR COLUMN MASTE00001 , 
	NATUREGARAN FOR COLUMN NATUR00001 , 
	VALGARAN , 
	UNITGARAN , 
	LIBUNTGAR , 
	BASEGARAN , 
	DATEDEBGARAN FOR COLUMN DATED00001 , 
	DATEFINGARAN FOR COLUMN DATEF00001 , 
	DUREEGARAN , 
	DURUNITEGARAN FOR COLUMN DURUN00001 , 
	DATEWDGARAN FOR COLUMN DATEW00001 , 
	DATEWFGARAN FOR COLUMN DATEW00002 , 
	TYPEPORTEE , 
	CODEOBJPORTEE FOR COLUMN CODEO00001 , 
	FORMUSE , 
	GARUSE , 
	TRIGAR , 
	VOLETORDRE , 
	BLOCORDRE , 
	GARORDRE ) 
	AS 
	(SELECT KHTID ATTESTID, KHTIPB CODECONTRAT, KHTALX VERSCONTRAT, KHTTYP TYPECONTRAT, KDEFOR CODEFOR, KDEID IDGARAN, IFNULL(RSQAP.KABRSQ, OBJAP.KACRSQ) CODERSQ, IFNULL(OBJAP.KACOBJ, 0) CODEOBJ, KDAALPHA LETTREFOR, KDADESC LIBFOR ,  
																					 KDEGARAN CODEGARAN, GADES LIBGARAN, KDENIVEAU NIVGARAN, KDESEQ SEQGARAN, KDESEM MASTERGARAN, KDEGAN NATUREGARAN,  
																					 CASE WHEN (KDGLCIVALA <> 0 OR KDGLCIUNIT ='CPX') THEN KDGLCIVALA ELSE KDEASVALA END VALGARAN,  
																					 CASE WHEN (KDGLCIVALA <> 0 OR KDGLCIUNIT ='CPX') THEN KDGLCIUNIT ELSE KDEASUNIT END UNITGARAN,  
																					 TPLIB LIBUNTGAR,  
																					 CASE WHEN (KDGLCIVALA <> 0 OR KDGLCIUNIT ='CPX') THEN KDGLCIBASE ELSE KDEASBASE END BASEGARAN,  
																					 /* KDEDATDEB DATEDEBGARAN, KDEDATFIN DATEFINGARAN, KDEDUREE DUREEGARAN, KDEDURUNI DURUNITEGARAN, KDEWDDEB DATEWDGARAN, KDEWDFIN DATEWFGARAN,   */  
																					 ATTFG.KHUDEB DATEDEBGARAN, ATTFG.KHUFIN DATEFINGARAN, KDEDUREE DUREEGARAN, KDEDURUNI DURUNITEGARAN, KDEWDDEB DATEWDGARAN, KDEWDFIN DATEWFGARAN,  
																					 IFNULL(KDFGAN, '') TYPEPORTEE, IFNULL(KDFOBJ, 0) CODEOBJPORTEE, ATTFF.KHUSIT FORMUSE, ATTFG.KHUSIT GARUSE , KDETRI TRIGAR , OPTDV.KDCORDRE VOLETORDRE, OPTDB.KDCORDRE BLOCORDRE, KDETRI GARORDRE  
																					 FROM ZALBINKHEO.KPATT  
																					 	 INNER JOIN ZALBINKHEO.KPATTF ATTFF ON ATTFF.KHUKHTID = KHTID AND ATTFF.KHUPERI = 'FO'  
															 INNER JOIN ZALBINKHEO.KPATTF ATTFR ON ATTFF.KHUKHTID = ATTFR.KHUKHTID AND ATTFR.KHURSQ = ATTFF.KHURSQ AND ATTFR.KHUSIT = 'V' AND ATTFR.KHUPERI = 'RQ'  
															 INNER JOIN ZALBINKHEO.KPFOR ON KDAIPB = KHTIPB AND KDAALX = KHTALX AND KDATYP = KHTTYP AND KDAFOR = ATTFF.KHUFOR  
															 INNER JOIN ZALBINKHEO.KPOPTAP ON KDAIPB = KDDIPB AND KDAALX = KDDALX AND KDATYP = KDDTYP AND KDAFOR = KDDFOR  
															 LEFT JOIN ZALBINKHEO.KPRSQ RSQAP ON KDDIPB = RSQAP.KABIPB AND KDDALX = RSQAP.KABALX AND KDDTYP = RSQAP.KABTYP AND KDDRSQ = RSQAP.KABRSQ AND KDDPERI = 'RQ'  
															 LEFT JOIN ZALBINKHEO.KPOBJ OBJAP ON KDDIPB = OBJAP.KACIPB AND KDDALX = OBJAP.KACALX AND KDDTYP = OBJAP.KACTYP AND KDDRSQ = OBJAP.KACRSQ AND KDDOBJ = OBJAP.KACOBJ AND KDDPERI = 'OB'  
															 INNER JOIN ZALBINKHEO.KPATTF ATTFG ON ATTFG.KHUKHTID = ATTFF.KHUKHTID AND ATTFG.KHURSQ = ATTFF.KHURSQ AND ATTFG.KHUOBJ = ATTFF.KHUOBJ AND ATTFG.KHUFOR = ATTFF.KHUFOR AND ATTFG.KHUPERI = 'GA'  
															 INNER JOIN ZALBINKHEO.KPGARAN ON KDEIPB = KHTIPB AND KDEALX = KHTALX AND KDETYP = KHTTYP AND KDEID = ATTFG.KHUKDEID AND KDEFOR = ATTFG.KHUFOR  
															 INNER JOIN ZALBINKHEO.KPGARTAR ON KDGKDEID = KDEID  
															 LEFT JOIN ZALBINKHEO.YYYYPAR ON TCON = 'PRODU' AND TFAM = 'QCVAU' AND TCOD = CASE WHEN KDGLCIVALA <> 0 THEN KDGLCIUNIT ELSE KDEASUNIT END  
															 INNER JOIN ZALBINKHEO.KGARAN ON TRIM(KDEGARAN) = TRIM(GAGAR)  
															 LEFT JOIN ZALBINKHEO.KPGARAP ON KDFKDEID = KDEID  
															 LEFT JOIN ZALBINKHEO.KPOBJ OBJP ON KDFIPB = OBJP.KACIPB AND KDFALX = OBJP.KACALX AND KDFTYP = OBJP.KACTYP AND KDFRSQ = OBJP.KACRSQ AND KDFOBJ = OBJP.KACOBJ  
															 INNER JOIN ZALBINKHEO.KPOPTD OPTDV  
																ON OPTDV.KDCIPB = KDEIPB  
																AND OPTDV.KDCTYP = KDETYP  
																AND OPTDV.KDCALX = KDEALX  
																AND OPTDV.KDCFOR = KDEFOR  
																AND OPTDV.KDCOPT = KDEOPT  
																AND OPTDV.KDCTENG = 'V'  
																	  
															 INNER JOIN ZALBINKHEO.KPOPTD OPTDB  
																ON OPTDB.KDCID = KDEKDCID  
																AND OPTDB.KDCKAKID = OPTDV.KDCKAKID  
																AND OPTDB.KDCTENG = 'B') ; 
  
LABEL ON COLUMN ZALBINKHEO.V_ATTESTGARAN 
( ATTESTID IS 'ID unique' , 
	CODECONTRAT IS 'IPB' , 
	VERSCONTRAT IS 'ALX' , 
	TYPECONTRAT IS 'Type O/P' , 
	CODEFOR IS 'Formule' , 
	IDGARAN IS 'ID unique' , 
	LETTREFOR IS 'Code Alpha' , 
	LIBFOR IS 'Description' , 
	CODEGARAN IS 'Garantie' , 
	LIBGARAN IS 'Désignation garantie' , 
	NIVGARAN IS 'Niveau' , 
	SEQGARAN IS 'Séquence' , 
	MASTERGARAN IS 'Séquence maitre' , 
	NATUREGARAN IS 'Nature Retenue' , 
	LIBUNTGAR IS 'Libellé paramètre' , 
	DATEDEBGARAN IS 'Plage début' , 
	DATEFINGARAN IS 'Plage Fin' , 
	DUREEGARAN IS 'Durée' , 
	DURUNITEGARAN IS 'Durée Unité' , 
	DATEWDGARAN IS 'Date standard début' , 
	DATEWFGARAN IS 'Date standard Fin' , 
	FORMUSE IS 'Top A/V' , 
	GARUSE IS 'Top A/V' , 
	TRIGAR IS 'Tri' , 
	VOLETORDRE IS 'N° ordre' , 
	BLOCORDRE IS 'N° ordre' , 
	GARORDRE IS 'Tri' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.V_ATTESTGARAN 
( ATTESTID TEXT IS 'ID Unique' , 
	CODECONTRAT TEXT IS 'IPB' , 
	VERSCONTRAT TEXT IS 'ALX' , 
	TYPECONTRAT TEXT IS 'Type O/P' , 
	CODEFOR TEXT IS 'Formule' , 
	IDGARAN TEXT IS 'ID unique' , 
	LETTREFOR TEXT IS 'Code alpha' , 
	LIBFOR TEXT IS 'Description' , 
	CODEGARAN TEXT IS 'Garantie' , 
	LIBGARAN TEXT IS 'Désignation garantie' , 
	NIVGARAN TEXT IS 'Niveau' , 
	SEQGARAN TEXT IS 'Séquence' , 
	MASTERGARAN TEXT IS 'Séquence garantie maitre' , 
	NATUREGARAN TEXT IS 'Nature retenue' , 
	LIBUNTGAR TEXT IS 'Libellé paramètre' , 
	DATEDEBGARAN TEXT IS 'Plage Début' , 
	DATEFINGARAN TEXT IS 'Plage Fin' , 
	DUREEGARAN TEXT IS 'Durée' , 
	DURUNITEGARAN TEXT IS 'Durée Unité' , 
	DATEWDGARAN TEXT IS 'Date standard Début' , 
	DATEWFGARAN TEXT IS 'Date standard Fin' , 
	FORMUSE TEXT IS 'Top A choix possible V sélectionné' , 
	GARUSE TEXT IS 'Top A choix possible V sélectionné' , 
	TRIGAR TEXT IS 'Tri' , 
	VOLETORDRE TEXT IS 'N° ordre  (KCATVOLET KCATBLOC)' , 
	BLOCORDRE TEXT IS 'N° ordre  (KCATVOLET KCATBLOC)' , 
	GARORDRE TEXT IS 'Tri' ) ; 
  
