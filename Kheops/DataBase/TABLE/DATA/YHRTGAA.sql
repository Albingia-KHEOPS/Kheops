CREATE TABLE ZALBINKHEO.YHRTGAA ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YHRTGAA de ZALBINKHEO ignoré. 
	JRIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	JRALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JRALX. 
	JRAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	JEHIN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	JRRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JRRSQ. 
	JRFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	JRGAR CHAR(6) CCSID 297 NOT NULL DEFAULT '' , 
	JRTRT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JRAPC CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JRAPA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	JRAPM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JRAPJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JRLCV DECIMAL(11, 0) NOT NULL DEFAULT 0 , 
	JRLCU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JRLCE CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	JRLOV DECIMAL(11, 0) NOT NULL DEFAULT 0 , 
	JRLOU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JRLOE CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	JRFHV DECIMAL(9, 0) NOT NULL DEFAULT 0 , 
	JRFHU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JRFHE CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	JRPRV DECIMAL(11, 3) NOT NULL DEFAULT 0 , 
	JRPRU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JRPRG CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JRTRA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	JRTRM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JRTRJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JRTRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FHRTGAA    ; 
  
LABEL ON TABLE ZALBINKHEO.YHRTGAA 
	IS 'H-Poli.RT:Garantie Anticipée                    JR' ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTGAA 
( JRIPB IS 'N° de police' , 
	JRALX IS 'N° Aliment' , 
	JRAVN IS 'N° avenant' , 
	JEHIN IS 'N° historique avenan' , 
	JRRSQ IS 'Identifiant risque' , 
	JRFOR IS 'Identifiant formule' , 
	JRGAR IS 'Code garantie' , 
	JRTRT IS 'Traitée (O/N)' , 
	JRAPC IS 'Code application' , 
	JRAPA IS 'Année Application' , 
	JRAPM IS 'Mois Application' , 
	JRAPJ IS 'Jour Application' , 
	JRLCV IS 'LCI : Valeur' , 
	JRLCU IS 'LCI : Unité' , 
	JRLCE IS 'Expression LCI' , 
	JRLOV IS 'LCI objet :  Valeur' , 
	JRLOU IS 'LCI objet : Unité' , 
	JRLOE IS 'Exp.Comp objet  LCI' , 
	JRFHV IS 'Valeur franchise' , 
	JRFHU IS 'Unité franchise' , 
	JRFHE IS 'Expression complexe Franchise' , 
	JRPRV IS 'Valeur prime' , 
	JRPRU IS 'Unité prime' , 
	JRPRG IS 'Code augmentation' , 
	JRTRA IS 'Année traitement' , 
	JRTRM IS 'Mois Traitement' , 
	JRTRJ IS 'Jour traitement' , 
	JRTRU IS 'User traitement' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTGAA 
( JRIPB TEXT IS 'N° de Police' , 
	JRALX TEXT IS 'N° Aliment' , 
	JRAVN TEXT IS 'N° avenant' , 
	JEHIN TEXT IS 'N° historique par avenant' , 
	JRRSQ TEXT IS 'Identifiant Risque' , 
	JRFOR TEXT IS 'Identifiant formule' , 
	JRGAR TEXT IS 'Code garantie' , 
	JRTRT TEXT IS 'Anticipation traitée (O/N)' , 
	JRAPC TEXT IS 'Code application (1 2 ou '' '')' , 
	JRAPA TEXT IS 'Application : Année' , 
	JRAPM TEXT IS 'Application : Mois' , 
	JRAPJ TEXT IS 'Application : Jour' , 
	JRLCV TEXT IS 'LCI : Valeur' , 
	JRLCU TEXT IS 'LCI : Unité' , 
	JRLCE TEXT IS 'Expression complexe LCI' , 
	JRLOV TEXT IS 'Spécif objet LCI : Valeur' , 
	JRLOU TEXT IS 'Spécif objet LCI : Unité' , 
	JRLOE TEXT IS 'Spécif objet LCI : Exp complexe' , 
	JRFHV TEXT IS 'Valeur de la franchise' , 
	JRFHU TEXT IS 'Unité de la franchise' , 
	JRFHE TEXT IS 'Expression complexe Franchise' , 
	JRPRV TEXT IS 'Valeur prime' , 
	JRPRU TEXT IS 'Unité prime' , 
	JRPRG TEXT IS 'Code augmentation  (+ ; - ou '' '')' , 
	JRTRA TEXT IS 'Traitement : Année' , 
	JRTRM TEXT IS 'Traitement : Mois' , 
	JRTRJ TEXT IS 'Traitement : Jour' , 
	JRTRU TEXT IS 'Traitement : User' ) ; 
  
