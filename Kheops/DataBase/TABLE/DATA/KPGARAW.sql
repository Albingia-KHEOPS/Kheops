﻿CREATE TABLE ZALBINKHEO.KPGARAW ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPGARAW de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPGARAW de ZALBINKHEO. 
	KDEID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDETYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KDEALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KDEFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDEOPT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDEKDCID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDEGARAN CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDESEQ NUMERIC(10, 0) NOT NULL DEFAULT 0 , 
	KDENIVEAU NUMERIC(1, 0) NOT NULL DEFAULT 0 , 
	KDESEM NUMERIC(10, 0) NOT NULL DEFAULT 0 , 
	KDESE1 NUMERIC(10, 0) NOT NULL DEFAULT 0 , 
	KDETRI CHAR(18) CCSID 297 NOT NULL DEFAULT '' , 
	KDENUMPRES NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDEAJOUT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDECAR CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDENAT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEGAN CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEKDFID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDEDEFG CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KDEKDHID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDEDATDEB NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KDEHEUDEB NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KDEDATFIN NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KDEHEUFIN NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KDEDUREE NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDEDURUNI CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEPRP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDETYPEMI CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEALIREF CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDECATNAT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEINA CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDETAXCOD CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KDETAXREP NUMERIC(5, 2) NOT NULL DEFAULT 0 , 
	KDECRAVN NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KDECRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDECRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KDEMAJAVN NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KDEASVALO NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDEASVALA NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDEASVALW NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDEASUNIT CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDEASBASE CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDEASMOD CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEASOBLI CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEINVSP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEINVEN NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDEWDDEB NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KDEWHDEB NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KDEWDFIN NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KDEWHFIN NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KDETCD CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KDEMODI CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEPIND CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEPCATN CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEPREF CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEPPRP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEPEMI CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEPTAXC CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KDEPNTM CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEALA CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEPALA CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDEALO CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPGARAW    ; 
  
LABEL ON TABLE ZALBINKHEO.KPGARAW 
	IS 'KHEOPS Garanties W' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPGARAW 
( KDEID IS 'ID unique' , 
	KDETYP IS 'Type O/P' , 
	KDEIPB IS 'IPB' , 
	KDEALX IS 'ALX' , 
	KDEFOR IS 'Formule' , 
	KDEOPT IS 'Option' , 
	KDEKDCID IS 'Lien KPOPTD' , 
	KDEGARAN IS 'Garantie' , 
	KDESEQ IS 'Séquence' , 
	KDENIVEAU IS 'Niveau' , 
	KDESEM IS 'Séquence maitre' , 
	KDESE1 IS 'Séquence Niveau 1' , 
	KDETRI IS 'Tri' , 
	KDENUMPRES IS 'N° de présentation' , 
	KDEAJOUT IS 'Garantie Ajoutée O/N' , 
	KDECAR IS 'Caractère' , 
	KDENAT IS 'Nature' , 
	KDEGAN IS 'Nature Retenue' , 
	KDEKDFID IS 'Lien KPGARAP' , 
	KDEDEFG IS 'Définition Garantie' , 
	KDEKDHID IS 'Lien KPSPEC' , 
	KDEDATDEB IS 'Date début' , 
	KDEHEUDEB IS 'Heure début' , 
	KDEDATFIN IS 'Fin de garantie Date' , 
	KDEHEUFIN IS 'Heure Fin' , 
	KDEDUREE IS 'Durée' , 
	KDEDURUNI IS 'Durée Unité' , 
	KDEPRP IS 'Type Application' , 
	KDETYPEMI IS 'Type émission' , 
	KDEALIREF IS 'Alim Mnt réference' , 
	KDECATNAT IS 'Application CATNAT' , 
	KDEINA IS 'Indexé O/N' , 
	KDETAXCOD IS 'Code taxe' , 
	KDETAXREP IS 'Répartition Taxe' , 
	KDECRAVN IS 'Avenant de création' , 
	KDECRU IS 'Création USer' , 
	KDECRD IS 'Création Date' , 
	KDEMAJAVN IS 'Avenant maj' , 
	KDEASVALO IS 'Assiette Valeur Ori' , 
	KDEASVALA IS 'Assiette Valeur Act' , 
	KDEASVALW IS 'Assiette Valeur W' , 
	KDEASUNIT IS 'Assiette Unité' , 
	KDEASBASE IS 'Assiette Base' , 
	KDEASMOD IS 'Assiette Modif O/N' , 
	KDEASOBLI IS 'Assiette Obligatoire' , 
	KDEINVSP IS 'Inventaire Spécifiqu' , 
	KDEINVEN IS 'Lien KPINVEN' , 
	KDEWDDEB IS 'Date standard début' , 
	KDEWHDEB IS 'Heure Standard Début' , 
	KDEWDFIN IS 'Date standard Fin' , 
	KDEWHFIN IS 'Heure Standard Fin' , 
	KDETCD IS 'Type Contrôle Date' , 
	KDEMODI IS 'Flag Modifié O/N' , 
	KDEPIND IS 'Parame Index O/N' , 
	KDEPCATN IS 'Paramétrage CATNAT' , 
	KDEPREF IS 'Paramétrage Mnt Ref' , 
	KDEPPRP IS 'Paramétrage Applicat' , 
	KDEPEMI IS 'Paramétrage émission' , 
	KDEPTAXC IS 'Parame  Code taxe' , 
	KDEPNTM IS 'Param Nature modif' , 
	KDEALA IS 'Type d''alimentation' , 
	KDEPALA IS 'Param type ALimentat' , 
	KDEALO IS 'Alimentat° Ori '' ''/A' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPGARAW 
( KDEID TEXT IS 'ID unique' , 
	KDETYP TEXT IS 'Type O/P' , 
	KDEIPB TEXT IS 'IPB' , 
	KDEALX TEXT IS 'ALX' , 
	KDEFOR TEXT IS 'Formule' , 
	KDEOPT TEXT IS 'Option' , 
	KDEKDCID TEXT IS 'Lien KPOPTD' , 
	KDEGARAN TEXT IS 'Garantie' , 
	KDESEQ TEXT IS 'Séquence' , 
	KDENIVEAU TEXT IS 'Niveau' , 
	KDESEM TEXT IS 'Séquence garantie maitre' , 
	KDESE1 TEXT IS 'Séquence Niveau 1' , 
	KDETRI TEXT IS 'Tri' , 
	KDENUMPRES TEXT IS 'N° de présentation' , 
	KDEAJOUT TEXT IS 'Garantie Ajoutée O/N' , 
	KDECAR TEXT IS 'Caractère (Base Obligatoire ...)' , 
	KDENAT TEXT IS 'Nature' , 
	KDEGAN TEXT IS 'Nature retenue' , 
	KDEKDFID TEXT IS 'Lien KPGARAP' , 
	KDEDEFG TEXT IS 'Définition garantie (Maintenance ..' , 
	KDEKDHID TEXT IS 'Lien KPSPEC' , 
	KDEDATDEB TEXT IS 'Date début' , 
	KDEHEUDEB TEXT IS 'Heure début' , 
	KDEDATFIN TEXT IS 'Fin de garantie Date' , 
	KDEHEUFIN TEXT IS 'Heure Fin' , 
	KDEDUREE TEXT IS 'Durée' , 
	KDEDURUNI TEXT IS 'Durée Unité' , 
	KDEPRP TEXT IS 'Type Application' , 
	KDETYPEMI TEXT IS 'Type émission' , 
	KDEALIREF TEXT IS 'Alimentation mnt Référence O/N' , 
	KDECATNAT TEXT IS 'Application CATNAT' , 
	KDEINA TEXT IS 'Indexée O/N' , 
	KDETAXCOD TEXT IS 'Code taxe' , 
	KDETAXREP TEXT IS 'Répartition taxe' , 
	KDECRAVN TEXT IS 'Avenant de création' , 
	KDECRU TEXT IS 'Création User' , 
	KDECRD TEXT IS 'Création Date' , 
	KDEMAJAVN TEXT IS 'Avenant de mise à jour' , 
	KDEASVALO TEXT IS 'Assiette Valeur Origine' , 
	KDEASVALA TEXT IS 'Assiette Valeur actualisée' , 
	KDEASVALW TEXT IS 'Assiette Valeur de travail' , 
	KDEASUNIT TEXT IS 'Assiette Unité' , 
	KDEASBASE TEXT IS 'Assiette Base' , 
	KDEASMOD TEXT IS 'Assiette Modifiable O/N' , 
	KDEASOBLI TEXT IS 'Assiette Obligatoire' , 
	KDEINVSP TEXT IS 'Inventaire spécifique' , 
	KDEINVEN TEXT IS 'Lien KPINVEN' , 
	KDEWDDEB TEXT IS 'Date standard début' , 
	KDEWHDEB TEXT IS 'Heure standard Début' , 
	KDEWDFIN TEXT IS 'Date standard Fin' , 
	KDEWHFIN TEXT IS 'Heure Standard Fin' , 
	KDETCD TEXT IS 'Type de Contrôle de date' , 
	KDEMODI TEXT IS 'Flag Modifié O/N' , 
	KDEPIND TEXT IS 'Paramétrage Indexation O/N' , 
	KDEPCATN TEXT IS 'Paramétrage CATNAT' , 
	KDEPREF TEXT IS 'Paramétrage Mnt Ref' , 
	KDEPPRP TEXT IS 'Paramétrage Application' , 
	KDEPEMI TEXT IS 'Paramétrage Type émission' , 
	KDEPTAXC TEXT IS 'Paramétrage Code Taxe' , 
	KDEPNTM TEXT IS 'Paramétrage Nature Modifiable' , 
	KDEALA TEXT IS 'Type d''alimentation' , 
	KDEPALA TEXT IS 'Paramétrage Type Alimentation' , 
	KDEALO TEXT IS 'Alimentation Origine '' '' /  A Auto' ) ; 
  
