CREATE TABLE ZALBINKHEO.KPOPTAPW ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPOPTAPW de ZALBINKHEO ignoré. 
	KDDID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDDTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDDIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KDDALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KDDFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDDOPT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDDKDBID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDDPERI CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KDDRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDDOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDDINVEN NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDDINVEP NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDDCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDDCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KDDMAJU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDDMAJD NUMERIC(8, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPOPTAPW   ; 
  
LABEL ON TABLE ZALBINKHEO.KPOPTAPW 
	IS 'KHEOPS Table_W KOPTAP                          KDD' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPOPTAPW 
( KDDID IS 'ID unique' , 
	KDDTYP IS 'Type O/P' , 
	KDDIPB IS 'IPB' , 
	KDDALX IS 'ALX' , 
	KDDFOR IS 'Formule' , 
	KDDOPT IS 'Option' , 
	KDDKDBID IS 'Lien KPOPT' , 
	KDDPERI IS 'Périmètre' , 
	KDDRSQ IS 'Risque' , 
	KDDOBJ IS 'Objet' , 
	KDDINVEN IS 'Lien KPINVEN' , 
	KDDINVEP IS 'Poste inventaire' , 
	KDDCRU IS 'Création User' , 
	KDDCRD IS 'Création Date' , 
	KDDMAJU IS 'MAJ User' , 
	KDDMAJD IS 'MAJ Date' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPOPTAPW 
( KDDID TEXT IS 'ID unique' , 
	KDDTYP TEXT IS 'Type O/P' , 
	KDDIPB TEXT IS 'IPB' , 
	KDDALX TEXT IS 'ALX' , 
	KDDFOR TEXT IS 'Formule' , 
	KDDOPT TEXT IS 'Option' , 
	KDDKDBID TEXT IS 'Lien KPOPT' , 
	KDDPERI TEXT IS 'Périmètre' , 
	KDDRSQ TEXT IS 'Risque' , 
	KDDOBJ TEXT IS 'Objet' , 
	KDDINVEN TEXT IS 'Lien KPINVEN' , 
	KDDINVEP TEXT IS 'Poste inventaire' , 
	KDDCRU TEXT IS 'Création User' , 
	KDDCRD TEXT IS 'Création Date' , 
	KDDMAJU TEXT IS 'MAJ User' , 
	KDDMAJD TEXT IS 'MAJ Date' ) ; 
  
