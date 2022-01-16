CREATE TABLE ZALBINKHEO.YCOMPAL ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YCOMPAL de ZALBINKHEO ignoré. 
	CLICI CHAR(6) CCSID 297 NOT NULL DEFAULT '' , 
	CLINL NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	CLNOM CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	CLTIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	CLFOC CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	CLTEL CHAR(13) CCSID 297 NOT NULL DEFAULT '' , 
	CLTLC CHAR(13) CCSID 297 NOT NULL DEFAULT '' , 
	CLACT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	CLFAA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	CLFAM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	CLFAJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	CLIN5 NUMERIC(5, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FCOMPAL    ; 
  
LABEL ON TABLE ZALBINKHEO.YCOMPAL 
	IS 'Compagnie : Interlocuteur                       CL' ; 
  
LABEL ON COLUMN ZALBINKHEO.YCOMPAL 
( CLICI IS 'Identifiant Cie' , 
	CLINL IS 'Code interlocuteur' , 
	CLNOM IS 'Nom' , 
	CLTIT IS 'Titre' , 
	CLFOC IS 'Code fonction' , 
	CLTEL IS 'Téléphone' , 
	CLTLC IS 'Télécopie' , 
	CLACT IS 'Activité            Interlocuteur' , 
	CLFAA IS 'Année               Fin d''activité' , 
	CLFAM IS 'Mois                Fin d''activité' , 
	CLFAJ IS 'Jour                Fin d''activité' , 
	CLIN5 IS 'Code interlocuteur' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YCOMPAL 
( CLICI TEXT IS 'Identifiant Compagnie' , 
	CLINL TEXT IS 'Code interlocuteur' , 
	CLNOM TEXT IS 'Nom' , 
	CLTIT TEXT IS 'Titre' , 
	CLFOC TEXT IS 'Code fonction' , 
	CLTEL TEXT IS 'Téléphone' , 
	CLTLC TEXT IS 'Télécopie' , 
	CLACT TEXT IS 'Activité Interlocuteur' , 
	CLFAA TEXT IS 'Année fin d''activité' , 
	CLFAM TEXT IS 'Mois  fin d''activité' , 
	CLFAJ TEXT IS 'Jour  fin d''activité' , 
	CLIN5 TEXT IS 'Code interlocuteur sur 5' ) ; 
  
