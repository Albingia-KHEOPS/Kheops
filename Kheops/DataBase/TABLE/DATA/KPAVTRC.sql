CREATE TABLE ZALBINKHEO.KPAVTRC ( 
	KHOID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHOTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHOIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KHOALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KHOPERI CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KHORSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KHOOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KHOFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KHOOPT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KHOETAPE CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KHOCHAM CHAR(20) CCSID 297 NOT NULL DEFAULT '' , 
	KHOACT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHOANV CHAR(100) CCSID 297 NOT NULL DEFAULT '' , 
	KHONVV CHAR(100) CCSID 297 NOT NULL DEFAULT '' , 
	KHOAVO CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHOOEF CHAR(20) CCSID 297 NOT NULL DEFAULT '' , 
	KHOCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KHOCRD NUMERIC(10, 0) NOT NULL DEFAULT 0 , 
	KHOCRH NUMERIC(6, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPAVTRC    ; 
  
LABEL ON TABLE ZALBINKHEO.KPAVTRC 
	IS 'Avenant Trace Modificat°                       KHO' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPAVTRC 
( KHOID IS 'ID unique' , 
	KHOTYP IS 'Type  O/P' , 
	KHOIPB IS 'N° de police / Offre' , 
	KHOALX IS 'N° Aliment / connexe' , 
	KHOPERI IS 'Périmètre' , 
	KHORSQ IS 'Risque' , 
	KHOOBJ IS 'Objet' , 
	KHOFOR IS 'Formule' , 
	KHOOPT IS 'Option' , 
	KHOETAPE IS 'Etape' , 
	KHOCHAM IS 'Champ concerné' , 
	KHOACT IS 'Type action' , 
	KHOANV IS 'Ancienne Valeur' , 
	KHONVV IS 'Nouvelle Valeur' , 
	KHOAVO IS 'Avenant Oblig O/N' , 
	KHOOEF IS 'Opération à Effect' , 
	KHOCRU IS 'Création User' , 
	KHOCRD IS 'Création Date' , 
	KHOCRH IS 'Création Heure' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPAVTRC 
( KHOID TEXT IS 'ID unique' , 
	KHOTYP TEXT IS 'Type  O Offre  P Police' , 
	KHOIPB TEXT IS 'N° de Police / Offre' , 
	KHOALX TEXT IS 'N° Aliment ou Connexe' , 
	KHOPERI TEXT IS 'Périmètre' , 
	KHORSQ TEXT IS 'Risque' , 
	KHOOBJ TEXT IS 'Objet' , 
	KHOFOR TEXT IS 'Formule' , 
	KHOOPT TEXT IS 'Option' , 
	KHOETAPE TEXT IS 'Etape' , 
	KHOCHAM TEXT IS 'Champ concerné' , 
	KHOACT TEXT IS 'Type action (Création/Modif/Xsupp)' , 
	KHOANV TEXT IS 'Ancienne Valeur' , 
	KHONVV TEXT IS 'Nouvelle Valeur' , 
	KHOAVO TEXT IS 'Avenant Obligatoire O/N' , 
	KHOOEF TEXT IS 'Opération à effectuer' , 
	KHOCRU TEXT IS 'Création User' , 
	KHOCRD TEXT IS 'Création Date' , 
	KHOCRH TEXT IS 'Création Heure' ) ; 
  
