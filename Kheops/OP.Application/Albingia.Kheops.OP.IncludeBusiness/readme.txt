
KheopsFull.sln
Albingia.Kheops.OP.IncludeBusiness

---------------------------
Notice d'utilisation
---------------------------

Ce programme Console permet d'ajouter aux projets l'ensemble des services, classes et intefaces pour pouvoir mettre en place un Service WCF.

La solution compatible est KheopsFull.sln.

Le nom des fichiers est déduis à partir d'un libellé métiers (<label>).
Les fichiers sont générés dans les projets suivant :
  - OP.IOWebService :
    + fichier <label>.svc ajouté
	+ fichier Configs\Dev\system.serviceModel.services.xml modifié
	
  - OP.Services :
    + fichier <label>Service.cs ajouté
	
  - OPServiceContract
    + fichier I<label>.cs ajouté
	
  - Albingia.Kheops.OP.Application
    + fichier Port\Driver\I<label>Port.cs ajouté
	+ fichier Port\Driven\I<label>Repository.cs ajouté
	+ fichier Infrastructure\Services\<label>Service.cs ajouté
	
  - Albingia.Kheops.OP.DataAdapter
    + fichier <label>Repository.cs ajouté
	
---------------------------
Lancer le programme
---------------------------

- Compiler le projet Albingia.Kheops.OP.IncludeBusiness
- A partir du dossier debug lancer dans une CMD ou PowerShell :

.\Albingia.Kheops.OP.IncludeBusiness.exe <label>

- Si la solution ou un des projets sont ouverts, une notification de rechargement est demandée, cliquer sur "Recharger tout".


---------------------------
AVERTISSEMENT
---------------------------
Si un controle de code source est utilisé, il est nécessaire d'y ajouter les nouveaux fichiers manuellement. (voir doc pour TFS)

