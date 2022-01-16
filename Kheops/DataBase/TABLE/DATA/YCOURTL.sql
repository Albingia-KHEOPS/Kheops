CREATE TABLE ZALBINKHEO.YCOURTL ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YCOURTL de ZALBINKHEO ignoré. 
	TLICT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	TLINL NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	TLFOC CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	TLTEL CHAR(13) CCSID 297 NOT NULL DEFAULT '' , 
	TLTLC CHAR(13) CCSID 297 NOT NULL DEFAULT '' , 
	TLACT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	TLFAA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	TLFAM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	TLFAJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	TLAEM CHAR(50) CCSID 297 NOT NULL DEFAULT '' , 
	TLIN5 NUMERIC(5, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FCOURTL    ; 
  
LABEL ON TABLE ZALBINKHEO.YCOURTL 
	IS 'Courtier : interlocuteurs                       TL' ; 
  
LABEL ON COLUMN ZALBINKHEO.YCOURTL 
( TLICT IS 'Identifiant courtier' , 
	TLINL IS 'Code interlocuteur' , 
	TLFOC IS 'Code fonction' , 
	TLTEL IS 'Téléphone' , 
	TLTLC IS 'Télécopie' , 
	TLACT IS 'Activité            Interlocuteur' , 
	TLFAA IS 'Année               Fin d''activité' , 
	TLFAM IS 'Mois                Fin d''activité' , 
	TLFAJ IS 'Jour                Fin d''activité' , 
	TLAEM IS 'Adresse Mail' , 
	TLIN5 IS 'Code interlocuteur' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YCOURTL 
( TLICT TEXT IS 'Identifiant courtier' , 
	TLINL TEXT IS 'Code interlocuteur' , 
	TLFOC TEXT IS 'Code fonction' , 
	TLTEL TEXT IS 'Téléphone' , 
	TLTLC TEXT IS 'Télécopie' , 
	TLACT TEXT IS 'Activité Interlocuteur' , 
	TLFAA TEXT IS 'Année fin d''activité' , 
	TLFAM TEXT IS 'Mois  fin d''activité' , 
	TLFAJ TEXT IS 'Jour  fin d''activité' , 
	TLAEM TEXT IS 'Adresse de messagerie (Mail)' , 
	TLIN5 TEXT IS 'Code interlocuteur sur 5' ) ; 
  
