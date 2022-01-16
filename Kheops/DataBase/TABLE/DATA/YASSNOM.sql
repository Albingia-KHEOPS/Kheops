CREATE TABLE ZALBINKHEO.YASSNOM ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YASSNOM de ZALBINKHEO ignoré. 
	ANIAS NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	ANINL NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	ANTNM CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	ANORD NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne ANORD. 
	ANNOM CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	ANTIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FASSNOM    ; 
  
LABEL ON TABLE ZALBINKHEO.YASSNOM 
	IS 'Assuré/Interloc.  Noms                          An' ; 
  
LABEL ON COLUMN ZALBINKHEO.YASSNOM 
( ANIAS IS 'Identifiant Assuré' , 
	ANINL IS 'Code interlocuteur  Assuré' , 
	ANTNM IS 'Type de nom' , 
	ANORD IS 'N° ordre' , 
	ANNOM IS 'Nom' , 
	ANTIT IS 'Titre' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YASSNOM 
( ANIAS TEXT IS 'Identifiant Assuré 10/00' , 
	ANINL TEXT IS 'Code interlocuteur Assuré' , 
	ANTNM TEXT IS 'Type de nom' , 
	ANORD TEXT IS 'N° ordre' , 
	ANNOM TEXT IS 'Nom' , 
	ANTIT TEXT IS 'Titre' ) ; 
  
