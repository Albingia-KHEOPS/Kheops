CREATE TABLE ZALBINKHEO.YHPCOUR ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YHPCOUR de ZALBINKHEO ignoré. 
	PFIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	PFALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne PFALX. 
	PFAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	PFHIN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	PFCTI CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PFICT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	PFSAA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	PFSAM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PFSAJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PFSAH DECIMAL(4, 0) NOT NULL DEFAULT 0 , 
	PFSIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PFSTA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	PFSTM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PFSTJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PFGES CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	PFSOU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	PFCOM DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	PFOCT CHAR(25) CCSID 297 NOT NULL DEFAULT '' , 
	PFXCM DECIMAL(5, 3) NOT NULL DEFAULT 0 , 
	PFXCN DECIMAL(5, 3) NOT NULL DEFAULT 0 , 
	PFTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FHPCOUR    ; 
  
LABEL ON TABLE ZALBINKHEO.YHPCOUR 
	IS 'Histo Polices : Courtiers' ; 
  
LABEL ON COLUMN ZALBINKHEO.YHPCOUR 
( PFIPB IS 'N° de police' , 
	PFALX IS 'N° Aliment' , 
	PFAVN IS 'N° avenant' , 
	PFHIN IS 'N° historique avenan' , 
	PFCTI IS 'Courtier Titulaire O' , 
	PFICT IS 'Identifiant courtier' , 
	PFSAA IS 'Année de saisie' , 
	PFSAM IS 'Mois de saisie' , 
	PFSAJ IS 'Jour de saisie' , 
	PFSAH IS 'Heure de saisie' , 
	PFSIT IS 'Code situation' , 
	PFSTA IS 'Année de situation' , 
	PFSTM IS 'Mois de situation' , 
	PFSTJ IS 'Jour de situation' , 
	PFGES IS 'Gestionnaire' , 
	PFSOU IS 'Souscripteur' , 
	PFCOM IS '% commissionnement' , 
	PFOCT IS 'Référence police' , 
	PFXCM IS 'Taux commission HCN' , 
	PFXCN IS 'Taux commission CN' , 
	PFTYP IS 'Type' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YHPCOUR 
( PFIPB TEXT IS 'N° de Police' , 
	PFALX TEXT IS 'N° Aliment' , 
	PFAVN TEXT IS 'N° avenant' , 
	PFHIN TEXT IS 'N° historique par avenant' , 
	PFCTI TEXT IS 'Courtier Titulaire => O' , 
	PFICT TEXT IS 'Identifiant courtier' , 
	PFSAA TEXT IS 'Année de saisie' , 
	PFSAM TEXT IS 'Mois de saisie' , 
	PFSAJ TEXT IS 'Jour de saisie' , 
	PFSAH TEXT IS 'Heure de saisie' , 
	PFSIT TEXT IS 'Code situation' , 
	PFSTA TEXT IS 'Année de situation' , 
	PFSTM TEXT IS 'Mois de situation' , 
	PFSTJ TEXT IS 'Jour de situation' , 
	PFGES TEXT IS 'Gestionnaire' , 
	PFSOU TEXT IS 'Souscripteur' , 
	PFCOM TEXT IS '% de commissionnement' , 
	PFOCT TEXT IS 'Référence police chez courtier' , 
	PFXCM TEXT IS 'Taux de commission hors catnat SPAL' , 
	PFXCN TEXT IS 'Taux de commission Catnat' , 
	PFTYP TEXT IS 'Type  O Offre  P Police  E à établir' ) ; 
  
