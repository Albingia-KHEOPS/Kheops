CREATE TABLE ZALBINKHEO.YLGNSTP ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YLGNSTP de ZALBINKHEO ignoré. 
	SLSOU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	SLAFF NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	SSCAP DECIMAL(9, 0) NOT NULL DEFAULT 0 , 
	SSADD CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	SSTAU DECIMAL(5, 3) NOT NULL DEFAULT 0 , 
	SSPRI DECIMAL(9, 0) NOT NULL DEFAULT 0 , 
	SSCOP DECIMAL(9, 0) NOT NULL DEFAULT 0 , 
	SSTPR DECIMAL(9, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FLGNSTP    ; 
  
LABEL ON TABLE ZALBINKHEO.YLGNSTP 
	IS 'Lignes Affaires stat pros : physique' ; 
  
LABEL ON COLUMN ZALBINKHEO.YLGNSTP 
( SLSOU IS 'Code souscripteur' , 
	SLAFF IS 'N° affaire / gest.' , 
	SSCAP IS 'Capital Ligne' , 
	SSADD IS 'Addition capital O/N' , 
	SSTAU IS 'Taux de primes' , 
	SSPRI IS 'Primes lignes Affair' , 
	SSCOP IS 'Complément de prime' , 
	SSTPR IS 'Total prime statprod' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YLGNSTP 
( SLSOU TEXT IS 'Code souscripteur' , 
	SLAFF TEXT IS 'N° affaire par gestionnaire STATPRO' , 
	SSCAP TEXT IS 'Capital Lignes affaires Statprod' , 
	SSADD TEXT IS 'Capital à additionner O/N Statprod' , 
	SSTAU TEXT IS 'Taux de primes Statprod' , 
	SSPRI TEXT IS 'Prime Lignes affaires statprod' , 
	SSCOP TEXT IS 'Complément de prime  Lignes affaire' , 
	SSTPR TEXT IS 'Total prime ligne affaire statprod' ) ; 
  
