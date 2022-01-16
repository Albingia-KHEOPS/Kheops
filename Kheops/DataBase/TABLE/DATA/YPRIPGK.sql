CREATE TABLE ZALBINKHEO.YPRIPGK ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPRIPGK de ZALBINKHEO ignoré. 
	KVIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KVALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KVALX. 
	KVTYE CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KVGAR CHAR(6) CCSID 297 NOT NULL DEFAULT '' , 
	KVCNH DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KVCNT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KVCNL DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KVCNM DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KVKNH DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KVKNT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KVKNL DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KVKNM DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KVCOT DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	KVKCO DECIMAL(11, 2) NOT NULL DEFAULT 0 )   
	RCDFMT FPRIPGK    ; 
  
LABEL ON TABLE ZALBINKHEO.YPRIPGK 
	IS 'Primes-W SPK/Garantie Complément PRIPGA         KV' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRIPGK 
( KVIPB IS 'N° de police' , 
	KVALX IS 'N° Aliment' , 
	KVTYE IS 'Type enregistrement' , 
	KVGAR IS 'Code garantie' , 
	KVCNH IS 'CATNAT Montant HT' , 
	KVCNT IS 'CATNAT Mnt taxe' , 
	KVCNL IS 'CATNAT : TTC' , 
	KVCNM IS 'CATNAT Commission' , 
	KVKNH IS 'CATNAT HT    Dev Cpt' , 
	KVKNT IS 'CN Mmt taxes  DevCpt' , 
	KVKNL IS 'CATNAT TTC   Dev Cpt' , 
	KVKNM IS 'CATNAT Comm. Dev Cpt' , 
	KVCOT IS 'Mnt commission H CN' , 
	KVKCO IS 'Mnt comm HCN  DevCpt' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRIPGK 
( KVIPB TEXT IS 'N° de Police' , 
	KVALX TEXT IS 'N° Aliment' , 
	KVTYE TEXT IS 'Typ enreg: 1 Part Albing 2 Prime tot' , 
	KVGAR TEXT IS 'Code garantie' , 
	KVCNH TEXT IS 'CATNAT : Montant HT' , 
	KVCNT TEXT IS 'CATNAT : Montant de taxe' , 
	KVCNL TEXT IS 'CATNAT : Montant TTC' , 
	KVCNM TEXT IS 'CATNAT : Montant Commission' , 
	KVKNH TEXT IS 'CATNAT : Montant HT          Dev Cpt' , 
	KVKNT TEXT IS 'CATNAT : Montant de taxe     Dev Cpt' , 
	KVKNL TEXT IS 'CATNAT : Montant TTC         Dev Cpt' , 
	KVKNM TEXT IS 'CATNAT : Montant Commission  Dev Cpt' , 
	KVCOT TEXT IS 'Mnt commissions  Hors Catnat' , 
	KVKCO TEXT IS 'Mnt commission Hors Catnat   Dev cpt' ) ; 
  
