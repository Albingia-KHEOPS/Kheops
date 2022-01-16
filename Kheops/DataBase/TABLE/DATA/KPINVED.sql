CREATE TABLE ZALBINKHEO.KPINVED ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPINVED de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPINVED de ZALBINKHEO. 
	KBFID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFID. 
	KBFTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KBFIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KBFALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFALX. 
	KBFKBEID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFKBEID. 
	KBFNUMLGN NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFNUMLGN. 
	KBFDESC CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KBFKADID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFKADID. 
	KBFSITE CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KBFNTLI CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KBFCP NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFCP. 
	KBFVILLE CHAR(30) CCSID 297 NOT NULL DEFAULT '' , 
	KBFADH NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFADH. 
	KBFDATDEB NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFDATDEB. 
	KBFDEBHEU NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFDEBHEU. 
	KBFDATFIN NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFDATFIN. 
	KBFFINHEU NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFFINHEU. 
	KBFMNT1 NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFMNT1. 
	KBFMNT2 NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFMNT2. 
	KBFNBEVN NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFNBEVN. 
	KBFNBPER NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFNBPER. 
	KBFNOM CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KBFPNOM CHAR(30) CCSID 297 NOT NULL DEFAULT '' , 
	KBFDATNAI NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFDATNAI. 
	KBFFONC CHAR(30) CCSID 297 NOT NULL DEFAULT '' , 
	KBFCDEC NUMERIC(13, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFCDEC. 
	KBFCIP NUMERIC(13, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFCIP. 
	KBFACCS CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KBFAVPR CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KBFMSR CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KBFCMAT CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KBFSEX CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KBFMDQ CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KBFMDA NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFMDA. 
	KBFACTP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KBFKADFH NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFKADFH. 
	KBFEXT CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KBFMNT3 NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFMNT3. 
	KBFMNT4 NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFMNT4. 
	KBFQUA CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KBFREN CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KBFRLO CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KBFPAY CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KBFSIT2 NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KBFADH2 NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KBFADH2. 
	KBFSIT3 NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KBFADH3 NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KBFDES2 NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KBFDES3 NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KBFDES4 NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KBFMRQ CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KBFMOD CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KBFMIM CHAR(20) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPINVED    ; 
  
LABEL ON TABLE ZALBINKHEO.KPINVED 
	IS 'Inventaire Détail                              KBF' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPINVED 
( KBFID IS 'ID Unique' , 
	KBFTYP IS 'Type' , 
	KBFIPB IS 'Contrat             Offre' , 
	KBFALX IS 'Aliment             Version' , 
	KBFKBEID IS 'Lien KPINVEN' , 
	KBFNUMLGN IS 'ligne' , 
	KBFDESC IS 'Description' , 
	KBFKADID IS 'Lien KPDESI' , 
	KBFSITE IS 'Nom du site' , 
	KBFNTLI IS 'Nature              lieu' , 
	KBFCP IS 'CP' , 
	KBFVILLE IS 'Ville' , 
	KBFADH IS 'Lien YADRESS' , 
	KBFDATDEB IS 'Date début' , 
	KBFDEBHEU IS 'Heure Début' , 
	KBFDATFIN IS 'Date Fin' , 
	KBFFINHEU IS 'Heure Fin' , 
	KBFMNT1 IS 'Montant 1' , 
	KBFMNT2 IS 'Montant 2' , 
	KBFNBEVN IS 'Nb                  Evénmt' , 
	KBFNBPER IS 'Nb                  Pers' , 
	KBFNOM IS 'Nom' , 
	KBFPNOM IS 'Prénom' , 
	KBFDATNAI IS 'Date                Naiss' , 
	KBFFONC IS 'Fonction' , 
	KBFCDEC IS 'Capital Décès' , 
	KBFCIP IS 'Capital IP' , 
	KBFACCS IS 'Accid               seul                O/N' , 
	KBFAVPR IS 'Avant               Prod                O/N' , 
	KBFMSR IS 'N°de série' , 
	KBFCMAT IS 'Code matériel' , 
	KBFSEX IS 'Sexe' , 
	KBFMDQ IS 'Quest               médical             O/N' , 
	KBFMDA IS 'Antécédents         médicaux            (Lien KPDESI)' , 
	KBFACTP IS 'Activités           prof.               O/N' , 
	KBFKADFH IS 'Lien KPDESI         franchise' , 
	KBFEXT IS 'Code Extension' , 
	KBFMNT3 IS 'Montant 3' , 
	KBFMNT4 IS 'Montant 4' , 
	KBFQUA IS 'Qual                assuré' , 
	KBFREN IS 'Renonc              recours' , 
	KBFRLO IS 'Risque              locatif             O/N/Exo' , 
	KBFPAY IS 'pays' , 
	KBFSIT2 IS 'Site 2              Lien KPDESI' , 
	KBFADH2 IS 'YADRESS             2' , 
	KBFSIT3 IS 'Site 3              Lien KPDESI' , 
	KBFADH3 IS 'Lien                YADRESS             3' , 
	KBFDES2 IS 'Désignation 2       Lien KPDESI' , 
	KBFDES3 IS 'Désignation 3       Lien KPDESI' , 
	KBFDES4 IS 'Désignation 4       Lien KPDESI' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPINVED 
( KBFID TEXT IS 'ID uniqye' , 
	KBFTYP TEXT IS 'Type  O Offre  P Police  E à établir' , 
	KBFIPB TEXT IS 'Contrat Offre' , 
	KBFALX TEXT IS 'Aliment Version' , 
	KBFKBEID TEXT IS 'Lien KPINVEN' , 
	KBFNUMLGN TEXT IS 'N° de ligne' , 
	KBFDESC TEXT IS 'Description' , 
	KBFKADID TEXT IS 'Lien KPDESI' , 
	KBFSITE TEXT IS 'Nom du site' , 
	KBFNTLI TEXT IS 'Nature du lieu' , 
	KBFCP TEXT IS 'CP' , 
	KBFVILLE TEXT IS 'Ville' , 
	KBFADH TEXT IS 'Lien YADRESS' , 
	KBFDATDEB TEXT IS 'Date début' , 
	KBFDEBHEU TEXT IS 'Heure début' , 
	KBFDATFIN TEXT IS 'Date Fin' , 
	KBFFINHEU TEXT IS 'Heure Fin' , 
	KBFMNT1 TEXT IS 'Montant 1' , 
	KBFMNT2 TEXT IS 'Montant 2' , 
	KBFNBEVN TEXT IS 'Nb Evénements' , 
	KBFNBPER TEXT IS 'Nb personnes' , 
	KBFNOM TEXT IS 'Nom' , 
	KBFPNOM TEXT IS 'Prénom' , 
	KBFDATNAI TEXT IS 'Date de Naissance' , 
	KBFFONC TEXT IS 'Fonction' , 
	KBFCDEC TEXT IS 'Capital Décès' , 
	KBFCIP TEXT IS 'Capital IP' , 
	KBFACCS TEXT IS 'Accident seul O/N' , 
	KBFAVPR TEXT IS 'Avant Prod O/N' , 
	KBFMSR TEXT IS 'N°de série' , 
	KBFCMAT TEXT IS 'Code Matériel' , 
	KBFSEX TEXT IS 'Sexe' , 
	KBFMDQ TEXT IS 'Questionnaire médical' , 
	KBFMDA TEXT IS 'Antécédents médicaux (Lien KPDESI)' , 
	KBFACTP TEXT IS 'Activités prof O/N' , 
	KBFKADFH TEXT IS 'Lien KPDESI pour Franchise' , 
	KBFEXT TEXT IS 'Code Extension' , 
	KBFMNT3 TEXT IS 'Montant 3' , 
	KBFMNT4 TEXT IS 'Montant 4' , 
	KBFQUA TEXT IS 'Qualité de l''assuré      ALSPK-QJUR' , 
	KBFREN TEXT IS 'Renonc.à recours         ALSPK-REN' , 
	KBFRLO TEXT IS 'Risq locatif O/N/Exonéré KHEOP-BFRLO' , 
	KBFPAY TEXT IS 'pays' , 
	KBFSIT2 TEXT IS 'Site 2                   Lien KPDESI' , 
	KBFADH2 TEXT IS 'Lien YADRESS 2' , 
	KBFSIT3 TEXT IS 'Site 3                   Lien KPDESI' , 
	KBFADH3 TEXT IS 'Lien YADRESS 3' , 
	KBFDES2 TEXT IS 'Dési2                    Lien KPDESI' , 
	KBFDES3 TEXT IS 'Dési3                    Lien KPDESI' , 
	KBFDES4 TEXT IS 'Dési4                    Lien KPDESI' , 
	KBFMRQ TEXT IS 'Marque' , 
	KBFMOD TEXT IS 'Modèle' , 
	KBFMIM TEXT IS 'Immatriculation' ) ; 
  
