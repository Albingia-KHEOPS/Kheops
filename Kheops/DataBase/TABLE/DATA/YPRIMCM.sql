CREATE TABLE ZALBINKHEO.YPRIMCM ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPRIMCM de ZALBINKHEO ignoré. 
	PNIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	PNALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne PNALX. 
	PNIPK NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	PNTYE CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PNICT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	PNCOM DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	PNXCM DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	PNCOT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PNCNM DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PNCNC DECIMAL(5, 3) NOT NULL DEFAULT 0 , 
	PNTCM DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PNKCO DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PNKNM DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PNKTC DECIMAL(11, 2) NOT NULL DEFAULT 0 )   
	RCDFMT FPRIMCM    ; 
  
LABEL ON TABLE ZALBINKHEO.YPRIMCM 
	IS 'Primes : Commissions/courtier                   PN' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRIMCM 
( PNIPB IS 'N° de police' , 
	PNALX IS 'N° Aliment' , 
	PNIPK IS 'N° de prime/ police' , 
	PNTYE IS 'Type courtier' , 
	PNICT IS 'Identifiant courtier' , 
	PNCOM IS 'Part commission.' , 
	PNXCM IS 'Taux de commission' , 
	PNCOT IS 'Total commission' , 
	PNCNM IS 'Commission CATNAT' , 
	PNCNC IS 'Taux commission CATNCat Nat' , 
	PNTCM IS 'Total commission' , 
	PNKCO IS 'Tot.commission  DevC' , 
	PNKNM IS 'CN: Mnt commiss.DevC' , 
	PNKTC IS 'Tot.commission  DevC' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRIMCM 
( PNIPB TEXT IS 'N° de Police' , 
	PNALX TEXT IS 'N° Aliment' , 
	PNIPK TEXT IS 'N° de prime / Police' , 
	PNTYE TEXT IS 'Type courtier : 1 Principal 2 Co-cou' , 
	PNICT TEXT IS 'Identifiant courtier' , 
	PNCOM TEXT IS 'Part de commissionnement' , 
	PNXCM TEXT IS 'Taux de commissions normal' , 
	PNCOT TEXT IS 'Mnt commissions (BM si RT)' , 
	PNCNM TEXT IS 'CATNAT : Mnt Commissions' , 
	PNCNC TEXT IS 'CATNAT : Taux de commission' , 
	PNTCM TEXT IS 'Total commissions (Normal + CATNAT)' , 
	PNKCO TEXT IS 'Mnt commissions (BM si RT)   Dev Cpt' , 
	PNKNM TEXT IS 'CATNAT : Mnt commissions     Dev Cpt' , 
	PNKTC TEXT IS 'Tot.commissions (Normal+ CN) Dev Cpt' ) ; 
  
