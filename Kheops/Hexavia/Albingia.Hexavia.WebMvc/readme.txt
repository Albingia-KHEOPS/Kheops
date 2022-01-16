Pour pouvoir utiliser le modèle intranet, vous devez activer l'authentification 
Windows et désactiver l'authentification anonyme.

Pour obtenir des instructions détaillées (notamment des instructions pour 
IIS 6.0), consultez
http://go.microsoft.com/fwlink/?LinkID=213745

IIS 7
1. Ouvrez le Gestionnaire des services Internet et accédez à votre site Web.
2. Dans Affichage des fonctionnalités, double-cliquez sur Authentification.
3. Dans la page Authentification, sélectionnez Authentification Windows. Si 
   l'authentification Windows n'est pas envisageable, vous devez vous assurer 
   que l'authentification Windows est installée sur le serveur.
        Pour activer l'authentification Windows :
 a) Dans le Panneau de configuration, ouvrez « Programmes et fonctionnalités ».
 b) Sélectionnez « Activer ou désactiver des fonctionnalités Windows ».
 c) Accédez à Services Internet (IIS) | Services World Wide Web  | Sécurité
    et vérifiez que le nœud Authentification Windows est sélectionné.
4. Dans le volet Actions, cliquez sur Activer pour utiliser l'authentification 
   Windows.
5. Dans la page Authentification, sélectionnez Authentification anonyme.
6. Dans le volet Actions, cliquez sur Désactiver pour désactiver 
   l'authentification anonyme.

IIS Express
1. Dans Visual Studio, cliquez avec le bouton droit sur le projet, puis 
   sélectionnez Utiliser IIS Express.
2. Dans l'Explorateur de solutions, cliquez avec le bouton droit sur le projet 
   pour le sélectionner.
3. Si le volet Propriétés n'est pas ouvert, ouvrez-le (F4).
4. Dans le volet Propriétés de votre projet :
 a) Attribuez la valeur « Désactivé » à l'option « Authentification anonyme ».
 b) Attribuez la valeur « Activé » à l'option « Authentification Windows ».

Vous pouvez installer IIS Express à l'aide de Microsoft Web Platform Installer :
    Pour Visual Studio : http://go.microsoft.com/fwlink/?LinkID=214802
    Pour Visual Web Developer : http://go.microsoft.com/fwlink/?LinkID=214800
