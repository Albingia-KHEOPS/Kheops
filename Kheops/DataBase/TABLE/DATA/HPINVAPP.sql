CREATE TABLE ZALBINKHEO.HPINVAPP ( 
--  SQL150B   10   REUSEDLT(*NO) de la table HPINVAPP de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour HPINVAPP de ZALBINKHEO. 
	KBGTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KBGIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KBGALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KBGAVN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KBGHIN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KBGNUM NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KBGKBEID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KBGPERI CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KBGRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KBGOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KBGFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KBGGAR CHAR(10) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT HPINVAPP   ; 
  
LABEL ON TABLE ZALBINKHEO.HPINVAPP 
	IS 'Histo  Inventaire S''applique à' ; 
  
LABEL ON COLUMN ZALBINKHEO.HPINVAPP 
( KBGTYP IS 'Type O/P' , 
	KBGIPB IS 'IPB' , 
	KBGALX IS 'ALX' , 
	KBGAVN IS 'N° avenant' , 
	KBGHIN IS 'N° histo par avenant' , 
	KBGNUM IS 'N° Ordre' , 
	KBGKBEID IS 'Lien KPINVEN' , 
	KBGPERI IS 'Périmètre' , 
	KBGRSQ IS 'N° Risque' , 
	KBGOBJ IS 'Objet' , 
	KBGFOR IS 'Formule' , 
	KBGGAR IS 'Garantie' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.HPINVAPP 
( KBGTYP TEXT IS 'Type O/P' , 
	KBGIPB TEXT IS 'IPB' , 
	KBGALX TEXT IS 'ALX' , 
	KBGAVN TEXT IS 'N° avenant' , 
	KBGHIN TEXT IS 'N° histo par avenant' , 
	KBGNUM TEXT IS 'N° Ordre par Offre/Contrat' , 
	KBGKBEID TEXT IS 'Lien KPINVEN' , 
	KBGPERI TEXT IS 'Périmètre' , 
	KBGRSQ TEXT IS 'N° Risque' , 
	KBGOBJ TEXT IS 'Objet' , 
	KBGFOR TEXT IS 'Formule' , 
	KBGGAR TEXT IS 'Garantie' ) ; 
  
