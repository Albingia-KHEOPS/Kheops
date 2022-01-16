CREATE TABLE ZALBINKHEO.KPSUSP ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPSUSP de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPSUSP de ZALBINKHEO. 
	KICID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KICTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KICIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KICALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICALX. 
	KICTYE CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KICIPK NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICIPK. 
	KICNUR DECIMAL(7, 0) NOT NULL DEFAULT 0 , 
	KICORI CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KICDEBM CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KICDEBD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICDEBD. 
	KICDEBH NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICDEBH. 
	KICFINM CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KICFIND NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICFIND. 
	KICFINH NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICFINH. 
	KICRSAD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICRSAD. 
	KICRSAH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICRSAH. 
	KICREVD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICREVD. 
	KICREVH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICREVH. 
	KICCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KICCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICCRD. 
	KICCRH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICCRH. 
	KICAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICAVN. 
	KICMJU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KICMJD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICMJD. 
	KICMJH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICMJH. 
	KICSIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KICSTU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KICSTD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICSTD. 
	KICSTH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KICSTH. 
	KICACA CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPSUSP     ; 
  
LABEL ON TABLE ZALBINKHEO.KPSUSP 
	IS 'Suspension Période     KICID                   KIC' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPSUSP 
( KICID IS 'ID unique' , 
	KICTYP IS 'O                   P' , 
	KICIPB IS 'Police              Offre' , 
	KICALX IS 'Aliment             vers' , 
	KICTYE IS 'Susp                NGar' , 
	KICIPK IS 'prim' , 
	KICNUR IS 'Rel' , 
	KICORI IS 'Origine' , 
	KICDEBM IS 'Motif               d''Entrée' , 
	KICDEBD IS 'déb                 susp' , 
	KICDEBH IS 'Déb                 susp                HHMN' , 
	KICFINM IS 'Motif               fin' , 
	KICFIND IS 'Fin                 susp' , 
	KICFINH IS 'Fin                 Sysp                HHMN' , 
	KICRSAD IS 'passat°             résile              auto' , 
	KICRSAH IS 'Résile              auto                heure' , 
	KICREVD IS 'remise              en vig' , 
	KICREVH IS 'remise              en vig              HHMNSS' , 
	KICCRU IS 'Création' , 
	KICCRD IS 'Création            Date' , 
	KICCRH IS 'Créat°              HHMNSS' , 
	KICAVN IS 'Avt                 susp' , 
	KICMJU IS 'Maj' , 
	KICMJD IS 'Maj                 Date' , 
	KICMJH IS 'Maj                 HHMNSS' , 
	KICSIT IS 'Sit' , 
	KICSTU IS 'User                sit' , 
	KICSTD IS 'Sit                 Date' , 
	KICSTH IS 'Sit                 HHMNSS' , 
	KICACA IS 'actu                auto                poss' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPSUSP 
( KICID TEXT IS 'ID unique' , 
	KICTYP TEXT IS 'Type  O Offre  P Police' , 
	KICIPB TEXT IS 'Police Offre' , 
	KICALX TEXT IS 'Aliment vers' , 
	KICTYE TEXT IS 'S=Susp N=Non gar' , 
	KICIPK TEXT IS 'prim' , 
	KICNUR TEXT IS 'Numéro chrono relance' , 
	KICORI TEXT IS 'Origine' , 
	KICDEBM TEXT IS 'Code motif d''entrée' , 
	KICDEBD TEXT IS 'Début suspension Date' , 
	KICDEBH TEXT IS 'Début suspension Heure HHMN' , 
	KICFINM TEXT IS 'Motif fin suspension (réSil/rGlt)' , 
	KICFIND TEXT IS 'Fin de suspension Date' , 
	KICFINH TEXT IS 'Fin Suspension Heure HHMN' , 
	KICRSAD TEXT IS 'Date passation résile auto' , 
	KICRSAH TEXT IS 'résile auto heure' , 
	KICREVD TEXT IS 'Date de Remise en vigueur' , 
	KICREVH TEXT IS 'Remise en vigueur heure HHMNSS' , 
	KICCRU TEXT IS 'Création User' , 
	KICCRD TEXT IS 'Création Date' , 
	KICCRH TEXT IS 'Création Heure HHMNSS' , 
	KICAVN TEXT IS 'N° Avenant courant à la suspension' , 
	KICMJU TEXT IS 'Maj User' , 
	KICMJD TEXT IS 'Maj Date' , 
	KICMJH TEXT IS 'Maj Heure HHMNSS' , 
	KICSIT TEXT IS 'Situation Code A=Actif N=non X=Annul' , 
	KICSTU TEXT IS 'User sit' , 
	KICSTD TEXT IS 'Situation Date' , 
	KICSTH TEXT IS 'Situation Heure HHMNSS' , 
	KICACA TEXT IS 'Actualisation auto possible O/N' ) ; 
  
