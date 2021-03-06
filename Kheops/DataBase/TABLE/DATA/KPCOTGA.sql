CREATE TABLE ZALBINKHEO.KPCOTGA ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPCOTGA de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPCOTGA de ZALBINKHEO. 
	KDNID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDNTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDNIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KDNALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KDNFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDNFOA CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KDNOPT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDNGARAN CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDNKDEID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDNNUMTAR NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KDNKDGID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDNTAROK CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDNKDMID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDNRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KDNTRI CHAR(18) CCSID 297 NOT NULL DEFAULT '' , 
	KDNHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNHF NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNKH NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNMHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNKHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNMTX NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNKTX NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNTTC NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNTTF NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNMTT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNKTC NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNCOT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNKCO NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNCNAMHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNCNAKHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNCNAMTX NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNCNAKTX NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNCNAMTT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNCNAKTT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNCNACOM NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNCNAKCM NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNATGMHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNATGKHT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNATGMTX NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNATGKTX NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNATGMTT NUMERIC(13, 2) NOT NULL DEFAULT 0 , 
	KDNATGKTT NUMERIC(13, 2) NOT NULL DEFAULT 0 )   
	RCDFMT FPCOTGA    ; 
  
LABEL ON TABLE ZALBINKHEO.KPCOTGA 
	IS 'KHEOPS Cotisation Garanties' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPCOTGA 
( KDNID IS 'ID unique' , 
	KDNTYP IS 'Type O/P' , 
	KDNIPB IS 'IPB' , 
	KDNALX IS 'ALX' , 
	KDNFOR IS 'Formule' , 
	KDNFOA IS 'Formule Code Alpha' , 
	KDNOPT IS 'Option' , 
	KDNGARAN IS 'Garantie' , 
	KDNKDEID IS 'Lien KPGARAN' , 
	KDNNUMTAR IS 'Numéro Tarif' , 
	KDNKDGID IS 'Lien KPGARTAR' , 
	KDNTAROK IS 'Flag Coché O/N' , 
	KDNKDMID IS 'Lien KPCOTIS' , 
	KDNRSQ IS 'Risque' , 
	KDNTRI IS 'Tri' , 
	KDNHT IS 'HT Hors CN Calculé' , 
	KDNHF IS 'HT Hors CN Forcé' , 
	KDNKH IS 'HT hors CN cpt' , 
	KDNMHT IS 'Total HT Hors CN dev' , 
	KDNKHT IS 'Total HT Hors CN cpt' , 
	KDNMTX IS 'TAxes Hors CN dev' , 
	KDNKTX IS 'Taxes Hors CN cpt' , 
	KDNTTC IS 'Total TTC cal HCNdev' , 
	KDNTTF IS 'TotalTTC Forcé HCN d' , 
	KDNMTT IS 'Total TTC HorsCN dev' , 
	KDNKTC IS 'Total TTC HorsCN cpt' , 
	KDNCOT IS 'Mnt Commission HCN' , 
	KDNKCO IS 'Mnt Comm HCN cpt' , 
	KDNCNAMHT IS 'CATNAT HT dev' , 
	KDNCNAKHT IS 'CATNAT HT cpt' , 
	KDNCNAMTX IS 'CATNAT Taxes dev' , 
	KDNCNAKTX IS 'CATNAT Taxes cpt' , 
	KDNCNAMTT IS 'CATNAT TTC dev' , 
	KDNCNAKTT IS 'CATNAT TTC cpt' , 
	KDNCNACOM IS 'CN Commission dev' , 
	KDNCNAKCM IS 'CN Commission cpt' , 
	KDNATGMHT IS 'ATG HT dev' , 
	KDNATGKHT IS 'ATG HT cpt' , 
	KDNATGMTX IS 'ATG Taxes dev' , 
	KDNATGKTX IS 'ATG Taxes cpt' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPCOTGA 
( KDNID TEXT IS 'ID unique' , 
	KDNTYP TEXT IS 'Type O/P' , 
	KDNIPB TEXT IS 'IPB' , 
	KDNALX TEXT IS 'ALX' , 
	KDNFOR TEXT IS 'Formule' , 
	KDNFOA TEXT IS 'Formule code Alpha' , 
	KDNOPT TEXT IS 'Option' , 
	KDNGARAN TEXT IS 'Garantie' , 
	KDNKDEID TEXT IS 'Lien KPGARAN' , 
	KDNNUMTAR TEXT IS 'Numéro Tarif' , 
	KDNKDGID TEXT IS 'Lien KPGARTAR' , 
	KDNTAROK TEXT IS 'Flag coché O/N' , 
	KDNKDMID TEXT IS 'Lien KPCOTIS' , 
	KDNRSQ TEXT IS 'Risque' , 
	KDNTRI TEXT IS 'Tri' , 
	KDNHT TEXT IS 'HT Hors CN Calculé dev' , 
	KDNHF TEXT IS 'HT Hors CN Forcé dev' , 
	KDNKH TEXT IS 'HT Hors CN cpt' , 
	KDNMHT TEXT IS 'Total HT Hors CN dev' , 
	KDNKHT TEXT IS 'Total HT Hors CN cpt' , 
	KDNMTX TEXT IS 'Taxes Hors CN dev' , 
	KDNKTX TEXT IS 'Taxes hors CN cpt' , 
	KDNTTC TEXT IS 'Total TTC Calculé Hors CN dev' , 
	KDNTTF TEXT IS 'Total TTC Forcé Hors CN dev' , 
	KDNMTT TEXT IS 'Total TTC Hors CN dev' , 
	KDNKTC TEXT IS 'Total TTC Hors CN cpt' , 
	KDNCOT TEXT IS 'Mnt Commissions Hors Catnat dev' , 
	KDNKCO TEXT IS 'Mnt Commissions Hors CN Cpt' , 
	KDNCNAMHT TEXT IS 'CATNAT HT dev' , 
	KDNCNAKHT TEXT IS 'CATNAT HT cpt' , 
	KDNCNAMTX TEXT IS 'CATNAT Taxes dev' , 
	KDNCNAKTX TEXT IS 'CATNAT Taxes cpt' , 
	KDNCNAMTT TEXT IS 'CATNAT TTC dev' , 
	KDNCNAKTT TEXT IS 'CATNAT TTC cpt' , 
	KDNCNACOM TEXT IS 'CATNAT Commission Dev' , 
	KDNCNAKCM TEXT IS 'CATNAT Commission cpt' , 
	KDNATGMHT TEXT IS 'ATG HT dev' , 
	KDNATGKHT TEXT IS 'ATG HT cpt' , 
	KDNATGMTX TEXT IS 'ATG Taxes dev' , 
	KDNATGKTX TEXT IS 'ATG Taxes cpt' , 
	KDNATGMTT TEXT IS 'ATG TTC dev' , 
	KDNATGKTT TEXT IS 'ATG TTC cpt' ) ; 
  
