CREATE TABLE ZALBINKHEO.YPRTFNC ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPRTFNC de ZALBINKHEO ignoré. 
	JNIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	JNALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JNALX. 
	JNRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JNRSQ. 
	JNOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne JNOBJ. 
	JNOBP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JNMFI CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	JNMFR CHAR(25) CCSID 297 NOT NULL DEFAULT '' , 
	JNMFB NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	JNMFA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	JNMFM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JNMFJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	JNMFV DECIMAL(11, 0) NOT NULL DEFAULT 0 , 
	JNMFU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	JNMFT CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	JNREL CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPRTFNC    ; 
  
LABEL ON TABLE ZALBINKHEO.YPRTFNC 
	IS 'Poli.RT : Financement                           JN' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTFNC 
( JNIPB IS 'N° de police' , 
	JNALX IS 'N° Aliment' , 
	JNRSQ IS 'Identifiant risque' , 
	JNOBJ IS 'Identifiant objet' , 
	JNOBP IS 'Objet ds police O/N' , 
	JNMFI IS 'Code financement' , 
	JNMFR IS 'Référence contrat Fi' , 
	JNMFB IS 'Crédit bailleur' , 
	JNMFA IS 'Année main levée' , 
	JNMFM IS 'Mois main levée' , 
	JNMFJ IS 'Jour main levée' , 
	JNMFV IS 'Montant financé' , 
	JNMFU IS 'Unité de la valeur' , 
	JNMFT IS 'Type de valeur' , 
	JNREL IS 'Code relance' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTFNC 
( JNIPB TEXT IS 'N° de Police' , 
	JNALX TEXT IS 'N° Aliment' , 
	JNRSQ TEXT IS 'Identifiant risque' , 
	JNOBJ TEXT IS 'Identifiant objet' , 
	JNOBP TEXT IS 'Objet dans la police O/N' , 
	JNMFI TEXT IS 'Code financement' , 
	JNMFR TEXT IS 'Référence dossier financement' , 
	JNMFB TEXT IS 'Crédit bailleur (Id intervenant CB)' , 
	JNMFA TEXT IS 'Date main levée : Année' , 
	JNMFM TEXT IS 'Date main levée : Mois' , 
	JNMFJ TEXT IS 'Date main levée : Jour' , 
	JNMFV TEXT IS 'Montant financé' , 
	JNMFU TEXT IS 'Unité Montant financé' , 
	JNMFT TEXT IS 'Type de valeur Montant Financé' , 
	JNREL TEXT IS 'Code relance' ) ; 
  
