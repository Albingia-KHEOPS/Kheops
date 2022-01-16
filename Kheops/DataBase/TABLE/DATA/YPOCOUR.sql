CREATE TABLE ZALBINKHEO.YPOCOUR ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPOCOUR de ZALBINKHEO ignoré. 
	PFTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PFIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	PFALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne PFALX. 
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
	PFXCN DECIMAL(5, 3) NOT NULL DEFAULT 0 )   
	RCDFMT FPOCOUR    ; 
  
LABEL ON TABLE ZALBINKHEO.YPOCOUR 
	IS 'Polices/Offres : Courtiers                      PF' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOCOUR 
( PFTYP IS 'Type' , 
	PFIPB IS 'N° de police / Offre' , 
	PFALX IS 'N° Aliment / connexe' , 
	PFCTI IS 'Courtier A;O ou N' , 
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
	PFCOM IS 'Part commissionnemnt' , 
	PFOCT IS 'Référence police' , 
	PFXCM IS 'Taux commission HCN' , 
	PFXCN IS 'Taux commission CN' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOCOUR 
( PFTYP TEXT IS 'Type  O Offre  P Police  E à établir' , 
	PFIPB TEXT IS 'N° de Police / Offre' , 
	PFALX TEXT IS 'N° Aliment ou Connexe' , 
	PFCTI TEXT IS 'Courtier Apporteur A,gestionn O, N' , 
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
	PFCOM TEXT IS 'Part commissionnemnt' , 
	PFOCT TEXT IS 'Référence police chez courtier' , 
	PFXCM TEXT IS 'Taux de commission hors Catnat SPAL' , 
	PFXCN TEXT IS 'Taux de commission CATNAT SPAL' ) ; 
  
