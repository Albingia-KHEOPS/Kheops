CREATE OR REPLACE VIEW ZALBINKHEO.V_RECHERCHEALL ( 
	PBTYP , 
	PBIPB , 
	ASSIR , 
	PBBRA , 
	BRALIB , 
	PBSAA , 
	PBSAM , 
	PBSAJ , 
	PBSAH , 
	PBCTA , 
	PBICT , 
	PBIAS , 
	PBALX , 
	PBSOU , 
	PBGES , 
	PBETA , 
	ETATLIB , 
	PBSIT , 
	SITLIB , 
	PBSTF , 
	ANIAS , 
	ANNOM , 
	PBSTQ , 
	QUALITELIB , 
	PBREF , 
	PBMJA , 
	PBMJM , 
	PBMJJ , 
	PBEFA , 
	PBEFM , 
	PBEFJ , 
	PBCRA , 
	PBCRM , 
	PBCRJ , 
	PBPER , 
	TNNOMCAB , 
	PBMER , 
	KAACIBLE , 
	KAHDESC , 
	PBAVN , 
	OFFRE_ABPCP6 FOR COLUMN OFFRE00001 , 
	OFFRE_ABPDP6 FOR COLUMN OFFRE00002 , 
	OFFRE_ABPVI6 FOR COLUMN OFFRE00003 , 
	COURT_ABPCHR FOR COLUMN COURT00001 , 
	COURT_ABPLG3 FOR COLUMN COURT00002 , 
	COURT_ABPEXT FOR COLUMN COURT00003 , 
	COURT_ABPNUM FOR COLUMN COURT00004 , 
	COURT_ABPLG4 FOR COLUMN COURT00005 , 
	COURT_ABPLG5 FOR COLUMN COURT00006 , 
	COURT_ABPDP6 FOR COLUMN COURT00007 , 
	COURT_ABPCP6 FOR COLUMN COURT00008 , 
	COURT_ABPVI6 FOR COLUMN COURT00009 , 
	COURT_ABPCEX FOR COLUMN COURT00010 , 
	COURT_ABPVIX FOR COLUMN COURT00011 , 
	COURT_ABPCDX FOR COLUMN COURT00012 , 
	COURT_ABPPAY FOR COLUMN COURT00013 , 
	ASSU_ABPCHR FOR COLUMN ASSU_00001 , 
	ASSU_ABPLG3 FOR COLUMN ASSU_00002 , 
	ASSU_ABPEXT FOR COLUMN ASSU_00003 , 
	ASSU_ABPNUM FOR COLUMN ASSU_00004 , 
	ASSU_ABPLG4 FOR COLUMN ASSU_00005 , 
	ASSU_ABPLG5 FOR COLUMN ASSU_00006 , 
	ASSU_ABPDP6 FOR COLUMN ASSU_00007 , 
	ASSU_ABPCP6 FOR COLUMN ASSU_00008 , 
	ASSU_ABPVI6 FOR COLUMN ASSU_00009 , 
	ASSU_ABPCEX FOR COLUMN ASSU_00010 , 
	ASSU_ABPVIX FOR COLUMN ASSU_00011 , 
	ASSU_ABPCDX FOR COLUMN ASSU_00012 , 
	ASSU_ABPPAY FOR COLUMN ASSU_00013 , 
	PBTTR , 
	PBTAC , 
	PBORK , 
	PBAVK , 
	GENERDOC , 
	PBMO1 , 
	PBMO2 , 
	PBMO3 , 
	KAJOBSV , 
	DATEFINEFFET FOR COLUMN DATEF00001 , 
	HEUREFINEFFET FOR COLUMN HEURE00001 , 
	HASSUSP ) 
	AS 
	(  
											 			SELECT PBTYP, B1.PBIPB, ASSIR, PBBRA, BRCHE.TPLIB BRALIB, PBSAA, PBSAM, PBSAJ, PBSAH, PBCTA, PBICT,  
											 			 PBIAS, PBALX, PBSOU, PBGES, PBETA, ETAT.TPLIB ETATLIB, PBSIT, SITUATION.TPLIB SITLIB, PBSTF CONCAT ' - ' CONCAT IFNULL(MOTIF.TPLIB, '') PBSTF,  
											 			 ANIAS, TRIM(ANNOM) ANNOM, PBSTQ, QUALITE.TPLIB QUALITELIB, PBREF, PBMJA, PBMJM, PBMJJ, PBEFA, PBEFM, PBEFJ,  
											 			 PBCRA, PBCRM, PBCRJ, PBPER,  
											 			 TNNOM TNNOMCAB,  
											 			 PBMER, KAACIBLE, KAHDESC, PBAVN,  
											 			  
											 			 OFFRE.ABPCP6 OFFRE_ABPCP6, OFFRE.ABPDP6 OFFRE_ABPDP6, OFFRE.ABPVI6 OFFRE_ABPVI6,  
											 			  
											 			 COURT.ABPCHR COURT_ABPCHR, COURT.ABPLG3 COURT_ABPLG3, COURT.ABPEXT COURT_ABPEXT, COURT.ABPNUM COURT_ABPNUM, COURT.ABPLG4 COURT_ABPLG4, COURT.ABPLG5 COURT_ABPLG5,  
											 			 COURT.ABPDP6 COURT_ABPDP6, COURT.ABPCP6 COURT_ABPCP6, COURT.ABPVI6 COURT_ABPVI6, COURT.ABPCEX COURT_ABPCEX, COURT.ABPVIX COURT_ABPVIX, COURT.ABPCDX COURT_ABPCDX, COURT.ABPPAY COURT_ABPPAY,  
											 			  
											 			 ASSU.ABPCHR ASSU_ABPCHR, ASSU.ABPLG3 ASSU_ABPLG3, ASSU.ABPEXT ASSU_ABPEXT, ASSU.ABPNUM ASSU_ABPNUM, ASSU.ABPLG4 ASSU_ABPLG4, ASSU.ABPLG5 ASSU_ABPLG5,  
											 			 ASSU.ABPDP6 ASSU_ABPDP6, ASSU.ABPCP6 ASSU_ABPCP6, ASSU.ABPVI6 ASSU_ABPVI6, ASSU.ABPCEX ASSU_ABPCEX, ASSU.ABPVIX ASSU_ABPVIX, ASSU.ABPCDX ASSU_ABPCDX, ASSU.ABPPAY ASSU_ABPPAY,  
											 			  
											 			 PBTTR, PBTAC, PBORK, PBAVK, IFNULL(KELID, 0) GENERDOC ,  
											 			 PBMO1, PBMO2, PBMO3, KAJOBSV,  
											 			 (PBFEA * 10000 + PBFEM * 100 + PBFEJ) DATEFINEFFET, CASE WHEN PBFEH = 2400 THEN 2359 ELSE PBFEH END HEUREFINEFFET,  
							  
														( SELECT COUNT ( * ) FROM ZALBINKHEO.KPSUSP WHERE KICIPB = B1.PBIPB AND KICALX = B1.PBALX AND KICTYP = B1.PBTYP) HASSUSP  
														  
											 			 FROM ZALBINKHEO.YPOBASE B1  
											 			 LEFT JOIN ZALBINKHEO.YASSNOM ON PBIAS = ANIAS AND ANINL = 0 AND ANTNM = 'A'  
											 			 LEFT JOIN ZALBINKHEO.YCOURTN ON PBICT = TNICT AND TNXN5 = 0 AND TNTNM = 'A'  
											 			 LEFT JOIN ZALBINKHEO.YCOURTI ON TCICT = TNICT  
											 			 LEFT JOIN ZALBINKHEO.YASSURE ON PBIAS = ASIAS  
											 			 LEFT JOIN ZALBINKHEO.YADRESS COURT ON TCADH = COURT.ABPCHR  
											 			 LEFT JOIN ZALBINKHEO.YADRESS ASSU ON ASADH = ASSU.ABPCHR  
											 			 LEFT JOIN ZALBINKHEO.YADRESS OFFRE ON PBADH = OFFRE.ABPCHR  
											 			 LEFT JOIN ZALBINKHEO.KPENT ON KAAIPB = B1.PBIPB AND KAAALX = PBALX AND KAATYP = PBTYP  
											 			 LEFT JOIN ZALBINKHEO.KPOBSV ON KAJCHR = KAAOBSV  
											 			 LEFT JOIN ZALBINKHEO.YYYYPAR MOTIF ON MOTIF.TCON = 'PRODU' AND MOTIF.TFAM = 'PBSTF' AND MOTIF.TCOD = PBSTF  
											 			 LEFT JOIN ZALBINKHEO.YYYYPAR BRCHE ON BRCHE.TCON = 'GENER' AND BRCHE.TFAM = 'BRCHE' AND BRCHE.TCOD = PBBRA AND BRCHE.TPCN2 = 1  
											 			 LEFT JOIN ZALBINKHEO.YYYYPAR ETAT ON ETAT.TCON = 'PRODU' AND ETAT.TFAM = 'PBETA' AND ETAT.TCOD = PBETA  
											 			 LEFT JOIN ZALBINKHEO.YYYYPAR SITUATION ON SITUATION.TCON = 'PRODU' AND SITUATION.TFAM = 'PBSIT' AND SITUATION.TCOD = PBSIT  
											 			 LEFT JOIN ZALBINKHEO.YYYYPAR QUALITE ON QUALITE.TCON = 'PRODU' AND QUALITE.TFAM = 'PBSTQ' AND QUALITE.TCOD = PBSTQ  
											 			 LEFT JOIN ZALBINKHEO.KCIBLE ON KAHCIBLE = KAACIBLE  
											 			 LEFT JOIN ZALBINKHEO.KPDOCLW ON KELIPB = PBIPB AND KELALX = PBALX AND KELTYP = PBTYP AND KELSIT = 'W') ; 
  
LABEL ON COLUMN ZALBINKHEO.V_RECHERCHEALL 
( PBTYP IS 'Type                O/P' , 
	PBIPB IS 'N° de police / Offre' , 
	ASSIR IS 'Siren' , 
	PBBRA IS 'Branche' , 
	BRALIB IS 'Libellé' , 
	PBSAA IS 'Année saisie /accord' , 
	PBSAM IS 'Mois saisie / accord' , 
	PBSAJ IS 'Jour saisie / accord' , 
	PBSAH IS 'Heure de saisie' , 
	PBCTA IS 'Id courtier apport' , 
	PBICT IS 'Courtier' , 
	PBIAS IS 'Identifiant Assuré' , 
	PBALX IS 'N° Aliment / connexe' , 
	PBSOU IS 'Souscripteur' , 
	PBGES IS 'Gestionnaire' , 
	PBETA IS 'Etat police' , 
	ETATLIB IS 'Libellé' , 
	PBSIT IS 'Code situation' , 
	SITLIB IS 'Libellé' , 
	ANIAS IS 'Identifiant Assuré' , 
	PBSTQ IS 'Qualité situation' , 
	QUALITELIB IS 'Libellé' , 
	PBREF IS 'Référence' , 
	PBMJA IS 'Année Màj' , 
	PBMJM IS 'Mois Màj' , 
	PBMJJ IS 'Jour Màj' , 
	PBEFA IS 'Année Date effet pol' , 
	PBEFM IS 'Mois Date effet pol' , 
	PBEFJ IS 'Jour Date effet pol.' , 
	PBCRA IS 'Année création' , 
	PBCRM IS 'Mois création' , 
	PBCRJ IS 'Jour création' , 
	PBPER IS 'Code périodicité' , 
	TNNOMCAB IS 'Nom' , 
	PBMER IS 'Police mère M/A' , 
	KAACIBLE IS 'Cible' , 
	KAHDESC IS 'Description' , 
	PBAVN IS 'N° avenant' , 
	OFFRE_ABPCP6 IS 'Code postal ligne 6' , 
	OFFRE_ABPDP6 IS 'Département ligne 6' , 
	OFFRE_ABPVI6 IS 'Acheminement Ligne 6' , 
	COURT_ABPCHR IS 'N° chrono' , 
	COURT_ABPLG3 IS 'Ligne 3 AFNOR' , 
	COURT_ABPEXT IS 'Extension abrégée' , 
	COURT_ABPNUM IS 'N° dans la voie' , 
	COURT_ABPLG4 IS 'Ligne 4 AFNOR' , 
	COURT_ABPLG5 IS 'Ligne 5 AFNOR' , 
	COURT_ABPDP6 IS 'Département ligne 6' , 
	COURT_ABPCP6 IS 'Code postal ligne 6' , 
	COURT_ABPVI6 IS 'Acheminement Ligne 6' , 
	COURT_ABPCEX IS 'Code Cedex' , 
	COURT_ABPVIX IS 'Ville Cedex' , 
	COURT_ABPCDX IS 'Cedex Cidex ....' , 
	COURT_ABPPAY IS 'Code pays' , 
	ASSU_ABPCHR IS 'N° chrono' , 
	ASSU_ABPLG3 IS 'Ligne 3 AFNOR' , 
	ASSU_ABPEXT IS 'Extension abrégée' , 
	ASSU_ABPNUM IS 'N° dans la voie' , 
	ASSU_ABPLG4 IS 'Ligne 4 AFNOR' , 
	ASSU_ABPLG5 IS 'Ligne 5 AFNOR' , 
	ASSU_ABPDP6 IS 'Département ligne 6' , 
	ASSU_ABPCP6 IS 'Code postal ligne 6' , 
	ASSU_ABPVI6 IS 'Acheminement Ligne 6' , 
	ASSU_ABPCEX IS 'Code Cedex' , 
	ASSU_ABPVIX IS 'Ville Cedex' , 
	ASSU_ABPCDX IS 'Cedex Cidex ....' , 
	ASSU_ABPPAY IS 'Code pays' , 
	PBTTR IS 'Type de traitement' , 
	PBTAC IS 'Type accord' , 
	PBORK IS 'KHEOPS statut' , 
	PBAVK IS 'N° Avenant Externe' , 
	PBMO1 IS 'Mot clé 1' , 
	PBMO2 IS 'Mot clé 2' , 
	PBMO3 IS 'Mot clé 3' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.V_RECHERCHEALL 
( PBTYP TEXT IS 'Type  O Offre  P Police' , 
	PBIPB TEXT IS 'N° de Police / Offre' , 
	ASSIR TEXT IS 'Siren' , 
	PBBRA TEXT IS 'Branche' , 
	BRALIB TEXT IS 'Libellé' , 
	PBSAA TEXT IS 'Année de saisie Cie ou accord' , 
	PBSAM TEXT IS 'Mois de saisie ou accord' , 
	PBSAJ TEXT IS 'Jour de saisie ou accord' , 
	PBSAH TEXT IS 'Heure de saisie' , 
	PBCTA TEXT IS 'Id courtier Apporteur' , 
	PBICT TEXT IS 'Courtier' , 
	PBIAS TEXT IS 'Identifiant Assuré 10/00' , 
	PBALX TEXT IS 'N° Aliment ou Connexe et version' , 
	PBSOU TEXT IS 'Souscripteur' , 
	PBGES TEXT IS 'Gestionnaire' , 
	PBETA TEXT IS 'Etat police (V validé A / N / R)' , 
	ETATLIB TEXT IS 'Libellé' , 
	PBSIT TEXT IS 'Code situation' , 
	SITLIB TEXT IS 'Libellé' , 
	ANIAS TEXT IS 'Identifiant Assuré 10/00' , 
	PBSTQ TEXT IS 'Qualité sit Police-Fini-Régul-Note.c' , 
	QUALITELIB TEXT IS 'Libellé' , 
	PBREF TEXT IS 'Référence' , 
	PBMJA TEXT IS 'Année Màj' , 
	PBMJM TEXT IS 'Mois Màj' , 
	PBMJJ TEXT IS 'Jour Màj' , 
	PBEFA TEXT IS 'Année Date effet police' , 
	PBEFM TEXT IS 'Mois  Date effet police' , 
	PBEFJ TEXT IS 'Jour  Date effet police' , 
	PBCRA TEXT IS 'Année Création(Validation souscript)' , 
	PBCRM TEXT IS 'Mois création (Validation souscript)' , 
	PBCRJ TEXT IS 'Jour création (Validation souscript)' , 
	PBPER TEXT IS 'Code périodicité' , 
	TNNOMCAB TEXT IS 'Nom' , 
	PBMER TEXT IS 'Police mère ou aliment (M/A/ )' , 
	KAACIBLE TEXT IS 'Cible' , 
	KAHDESC TEXT IS 'Description' , 
	PBAVN TEXT IS 'N° avenant' , 
	OFFRE_ABPCP6 TEXT IS 'Code postal  Ligne 6' , 
	OFFRE_ABPDP6 TEXT IS 'Département ligne 6' , 
	OFFRE_ABPVI6 TEXT IS 'Libellé acheminement Ligne 6' , 
	COURT_ABPCHR TEXT IS 'N° Chrono unique' , 
	COURT_ABPLG3 TEXT IS 'Ligne 3 AFNOR' , 
	COURT_ABPEXT TEXT IS 'Extension abrégée(Bis Ter .....' , 
	COURT_ABPNUM TEXT IS 'N° dans la voie' , 
	COURT_ABPLG4 TEXT IS 'Ligne 4 AFNOR' , 
	COURT_ABPLG5 TEXT IS 'Ligne 5 AFNOR' , 
	COURT_ABPDP6 TEXT IS 'Département ligne 6' , 
	COURT_ABPCP6 TEXT IS 'Code postal  Ligne 6' , 
	COURT_ABPVI6 TEXT IS 'Libellé acheminement Ligne 6' , 
	COURT_ABPCEX TEXT IS 'Code cedex' , 
	COURT_ABPVIX TEXT IS 'Ville  Cedex' , 
	COURT_ABPCDX TEXT IS 'Type Cedex Cidex BP' , 
	COURT_ABPPAY TEXT IS 'Code pays' , 
	ASSU_ABPCHR TEXT IS 'N° Chrono unique' , 
	ASSU_ABPLG3 TEXT IS 'Ligne 3 AFNOR' , 
	ASSU_ABPEXT TEXT IS 'Extension abrégée(Bis Ter .....' , 
	ASSU_ABPNUM TEXT IS 'N° dans la voie' , 
	ASSU_ABPLG4 TEXT IS 'Ligne 4 AFNOR' , 
	ASSU_ABPLG5 TEXT IS 'Ligne 5 AFNOR' , 
	ASSU_ABPDP6 TEXT IS 'Département ligne 6' , 
	ASSU_ABPCP6 TEXT IS 'Code postal  Ligne 6' , 
	ASSU_ABPVI6 TEXT IS 'Libellé acheminement Ligne 6' , 
	ASSU_ABPCEX TEXT IS 'Code cedex' , 
	ASSU_ABPVIX TEXT IS 'Ville  Cedex' , 
	ASSU_ABPCDX TEXT IS 'Type Cedex Cidex BP' , 
	ASSU_ABPPAY TEXT IS 'Code pays' , 
	PBTTR TEXT IS 'Type de traitement (Affnouv/avenant)' , 
	PBTAC TEXT IS 'Type accord S Signée N Non signée ..' , 
	PBORK TEXT IS 'KHEOPS Statut ''  '' ''REC'' ''REP'' ''KHE''' , 
	PBAVK TEXT IS 'N° Avenant Externe SPAL' , 
	PBMO1 TEXT IS 'Mot clé 1' , 
	PBMO2 TEXT IS 'Mot clé 2' , 
	PBMO3 TEXT IS 'Mot clé 3' ) ; 
  
