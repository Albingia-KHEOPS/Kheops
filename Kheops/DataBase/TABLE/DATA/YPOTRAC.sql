CREATE TABLE ZALBINKHEO.YPOTRAC ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPOTRAC de ZALBINKHEO ignoré. 
	PYTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PYIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	PYALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne PYALX. 
	PYAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	PYTTR CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	PYVAG CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PYORD DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	PYTRA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	PYTRM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PYTRJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PYTRH DECIMAL(4, 0) NOT NULL DEFAULT 0 , 
	PYLIB CHAR(30) CCSID 297 NOT NULL DEFAULT '' , 
	PYINF CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PYSDA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	PYSDM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PYSDJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PYSFA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	PYSFM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PYSFJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PYMJU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	PYMJA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	PYMJM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PYMJJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PYMJH DECIMAL(4, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPOTRAC    ; 
  
LABEL ON TABLE ZALBINKHEO.YPOTRAC 
	IS 'Delphi suivi Evénements                         PY' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOTRAC 
( PYTYP IS 'Type' , 
	PYIPB IS 'N° Offre ou police' , 
	PYALX IS 'N° Aliment' , 
	PYAVN IS 'N° avenant' , 
	PYTTR IS 'Type de traitement' , 
	PYVAG IS 'Gestion/Validation' , 
	PYORD IS 'N° Ordre' , 
	PYTRA IS 'Traitement Année' , 
	PYTRM IS 'Traitement : Mois' , 
	PYTRJ IS 'Traitement Jour' , 
	PYTRH IS 'Traitement Heure' , 
	PYLIB IS 'Libellé' , 
	PYINF IS 'Type Info I/S ...' , 
	PYSDA IS 'Suspension Année déb' , 
	PYSDM IS 'Suspension Mois déb.' , 
	PYSDJ IS 'Suspension Jour déb' , 
	PYSFA IS 'Suspension Année Fin' , 
	PYSFM IS 'Suspension Mois Fin' , 
	PYSFJ IS 'Suspension Jour Fin' , 
	PYMJU IS 'Màj : User' , 
	PYMJA IS 'Màj : Année' , 
	PYMJM IS 'Màj : Mois' , 
	PYMJJ IS 'Màj : Jour' , 
	PYMJH IS 'Màj : Heure' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOTRAC 
( PYTYP TEXT IS 'Type  O Offre  P Police' , 
	PYIPB TEXT IS 'N° Offre ou Police' , 
	PYALX TEXT IS 'N° Aliment' , 
	PYAVN TEXT IS 'N° avenant' , 
	PYTTR TEXT IS 'Type de traitement (Affnouv/avenant)' , 
	PYVAG TEXT IS 'Gestion G ou Validation V' , 
	PYORD TEXT IS 'N° ordre' , 
	PYTRA TEXT IS 'Traitement : Année' , 
	PYTRM TEXT IS 'Traitement : Mois' , 
	PYTRJ TEXT IS 'Traitement : Jour' , 
	PYTRH TEXT IS 'Traitement Heure' , 
	PYLIB TEXT IS 'Libellé' , 
	PYINF TEXT IS 'Type Info : I Info / S Suspension' , 
	PYSDA TEXT IS 'Garantie suspendue : Année Début' , 
	PYSDM TEXT IS 'Garantie suspendue : Mois  Début' , 
	PYSDJ TEXT IS 'Garantie suspendue : Jour  Début' , 
	PYSFA TEXT IS 'Garantie suspendue : Année Fin' , 
	PYSFM TEXT IS 'Garantie suspendue : Mois  Fin' , 
	PYSFJ TEXT IS 'Garantie suspendue : Jour  Fin' , 
	PYMJU TEXT IS 'Màj : User' , 
	PYMJA TEXT IS 'Màj : Année' , 
	PYMJM TEXT IS 'Màj : Mois' , 
	PYMJJ TEXT IS 'Màj : Jour' , 
	PYMJH TEXT IS 'Màj : Heure' ) ; 
  
