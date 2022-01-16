CREATE TRIGGER ZALBINKHEO.UPDATEYPOSTAT 
	AFTER UPDATE ON ZALBINKHEO.YPOSTAT 
	REFERENCING OLD AS ANCLINE 
	NEW AS NOUVLINE 
	FOR EACH ROW 
	MODE DB2SQL 
	SET OPTION  ALWBLK = *ALLREAD , 
	ALWCPYDTA = *OPTIMIZE , 
	COMMIT = *NONE , 
	CLOSQLCSR = *ENDMOD , 
	DECRESULT = (31, 31, 00) , 
	DFTRDBCOL = *NONE , 
	DYNDFTCOL = *NO , 
	DYNUSRPRF = *USER , 
	SRTSEQ = *HEX   
	BEGIN ATOMIC 
DECLARE V_TYP CHAR ( 1 ) DEFAULT '' ; 
DECLARE V_IPB CHAR ( 9 ) DEFAULT '' ; 
DECLARE V_VERSION NUMERIC DEFAULT 0 ; 
DECLARE V_CHAMPBD CHAR ( 12 ) DEFAULT '' ; 
  
SET SQLP_L3 . V_VERSION = NOUVLINE . AAPALX ; 
SET SQLP_L3 . V_IPB = NOUVLINE . AAPIPB ; 
SET SQLP_L3 . V_TYP = NOUVLINE . AAPTYP ; 
  
  
IF ( ANCLINE . AAPREJ <> NOUVLINE . AAPREJ ) THEN 
SET SQLP_L3 . V_CHAMPBD = 'AAPREJ' ; 
END IF ; 
  
  
IF ( ANCLINE . AAPREM <> NOUVLINE . AAPREM ) THEN 
SET SQLP_L3 . V_CHAMPBD = 'AAPREM' ; 
END IF ; 
  
  
IF ( ANCLINE . AAPREA <> NOUVLINE . AAPREA ) THEN 
SET SQLP_L3 . V_CHAMPBD = 'AAPREA' ; 
END IF ; 
  
IF ( NOUVLINE . AAPECO = 'O' ) THEN 
SET SQLP_L3 . V_CHAMPBD = 'AAPECO' ; 
END IF ; 
IF ( SQLP_L3 . V_CHAMPBD <> '' ) THEN 
CALL YALBINFILE . SP_UPDATEKGRPTRG ( SQLP_L3 . V_TYP , SQLP_L3 . V_IPB , SQLP_L3 . V_VERSION , 'YPOSTAT' , SQLP_L3 . V_CHAMPBD ) ; 
END IF ; 
  
END  ; 
  
LABEL ON TABLE ZALBINKHEO.YPOSTAT 
	IS 'Pilote stat                                    AAP' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOSTAT 
( AAPTYP IS 'Type                O/P' , 
	AAPIPB IS 'N° de police / Offre' , 
	AAPALX IS 'N° Aliment / connexe' , 
	AAPECO IS 'En-cours O/N' , 
	AAPSTA IS 'Utilisé dans stapro' , 
	AAPAFA IS 'Affectation Année' , 
	AAPAFM IS 'Affectation Mois' , 
	AAPAFJ IS 'Affectation Jour' , 
	AAPBUR IS 'Bureau Affectation' , 
	AAPSEC IS 'Code Secteur affecT' , 
	AAPDEP IS 'Département Affecta' , 
	AAPPAY IS 'Code pays Affectat°' , 
	AAPCRU IS 'User Création' , 
	AAPCRA IS 'Année création' , 
	AAPCRM IS 'Mois création' , 
	AAPCRJ IS 'Jour création' , 
	AAPICT IS 'Courtier gestionnair' , 
	AAPBRA IS 'Branche' , 
	AAPSBR IS 'Sous-branche' , 
	AAPCAT IS 'Catégorie' , 
	AAPCTA IS 'Courtier Apporteur' , 
	AAPCTP IS 'Courtier payeur' , 
	AAPIAS IS 'Identifiant Assuré' , 
	AAPNPL IS 'Nature Police' , 
	AAPAPP IS '% Apérition' , 
	AAPSAA IS 'Année Date Affaire' , 
	AAPSAM IS 'Mois Date Affaire' , 
	AAPSAJ IS 'Jour Date Affaire' , 
	AAPEFA IS 'Date Effet Année' , 
	AAPEFM IS 'Date Effet Mois' , 
	AAPEFJ IS 'Date effet Jour' , 
	AAPFEA IS 'Fin Effet Année' , 
	AAPFEM IS 'Fin Effet Mois' , 
	AAPFEJ IS 'Fin Effet Jour' , 
	AAPREA IS 'Passation résil AA' , 
	AAPREM IS 'Passation résil MM' , 
	AAPREJ IS 'Passation Résil JJ' , 
	AAPGES IS 'Gestionnaire' , 
	AAPSOU IS 'Souscripteur Alb' , 
	AAPMJA IS 'Année Màj' , 
	AAPMJM IS 'Mois Màj' , 
	AAPMJJ IS 'Jour Màj' , 
	AAPMJH IS 'Maj Heure' , 
	AAPMJU IS 'User Màj' , 
	AAPPER IS 'Code périodicité' , 
	AAPICS IS 'Id courtier stats' , 
	AAPINS IS 'Code inspection' , 
	AAPIST IS 'Inséré' , 
	AAPETA IS 'Etat police' , 
	AAPSIT IS 'Code situation' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOSTAT 
( AAPTYP TEXT IS 'Type  O Offre  P Police' , 
	AAPIPB TEXT IS 'N° de Police / Offre' , 
	AAPALX TEXT IS 'N° Aliment ou Connexe et version' , 
	AAPECO TEXT IS 'En-cours O/N' , 
	AAPSTA TEXT IS 'Utilisé pour Statist prod POAFFN O/N' , 
	AAPAFA TEXT IS 'Affectation Année' , 
	AAPAFM TEXT IS 'Affectation Mois' , 
	AAPAFJ TEXT IS 'Affectation Jour' , 
	AAPBUR TEXT IS 'Bureau Affectation stat' , 
	AAPSEC TEXT IS 'Code Secteur Affectation' , 
	AAPDEP TEXT IS 'Département Affectation' , 
	AAPPAY TEXT IS 'Code pays Affectation' , 
	AAPCRU TEXT IS 'User Création (Validation souscript)' , 
	AAPCRA TEXT IS 'Année Création(Validation souscript)' , 
	AAPCRM TEXT IS 'Mois création (Validation souscript)' , 
	AAPCRJ TEXT IS 'Jour création (Validation souscript)' , 
	AAPICT TEXT IS 'Courtier gestionnaire' , 
	AAPBRA TEXT IS 'Branche' , 
	AAPSBR TEXT IS 'Sous-branche' , 
	AAPCAT TEXT IS 'Catégorie' , 
	AAPCTA TEXT IS 'Courtier Apporteur' , 
	AAPCTP TEXT IS 'Courtier payeur' , 
	AAPIAS TEXT IS 'Identifiant Assuré' , 
	AAPNPL TEXT IS 'Nature Police' , 
	AAPAPP TEXT IS '% Apérition' , 
	AAPSAA TEXT IS 'Année Date Affaire' , 
	AAPSAM TEXT IS 'Mois  Date Affaire' , 
	AAPSAJ TEXT IS 'Jour  Date Affaire' , 
	AAPEFA TEXT IS 'Date effet Année' , 
	AAPEFM TEXT IS 'Date effet Mois' , 
	AAPEFJ TEXT IS 'Date effet Jour' , 
	AAPFEA TEXT IS 'Fin Effet Année' , 
	AAPFEM TEXT IS 'Fin Effet Mois' , 
	AAPFEJ TEXT IS 'Fin Effet Jour' , 
	AAPREA TEXT IS 'Passation Résiliation Année' , 
	AAPREM TEXT IS 'Passation résiliation Mois' , 
	AAPREJ TEXT IS 'Passation résiliation Jour' , 
	AAPGES TEXT IS 'Gestionnaire' , 
	AAPSOU TEXT IS 'Souscripteur Alb' , 
	AAPMJA TEXT IS 'Année Màj' , 
	AAPMJM TEXT IS 'Mois Màj' , 
	AAPMJJ TEXT IS 'Jour Màj' , 
	AAPMJH TEXT IS 'Maj Heure' , 
	AAPMJU TEXT IS 'User Màj' , 
	AAPPER TEXT IS 'Code périodicité' , 
	AAPICS TEXT IS 'Id courtier Statistiques' , 
	AAPINS TEXT IS 'Code inspection' , 
	AAPIST TEXT IS 'O=enreg inséré à postériori' , 
	AAPETA TEXT IS 'Etat police (V validé N non validé)' , 
	AAPSIT TEXT IS 'Code situation' ) ; 
  
