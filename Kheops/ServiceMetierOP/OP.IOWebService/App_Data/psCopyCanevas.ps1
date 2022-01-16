
using namespace System.IO;

	param(
		[Parameter(Mandatory=$true)][String]$SqlFolderPath, 
		[Parameter(Mandatory=$true)][ValidateSet('CONST','RECETTE','SUPPORT','FORM','PPROD','PROD','FIX','PARAME')][String]$Source, 
		[Parameter(Mandatory=$true)][ValidateSet('CONST','RECETTE','SUPPORT','FORM','PPROD','PROD','FIX','PARAME')][String]$Cible,
		[Parameter(Mandatory=$true)][String]$ConnectionString
	)	

 $environments = @{
    CONST =   @{ Data = "ZALBINKDEV"};
    RECETTE =   @{ Data = "ZALBINKQUA"};
    FORM =  @{ Data = "ZALBINKFRM"};
    SUPPORT =  @{ Data = "ZALBINKDEV"};
    FIX =  @{ Data = "ZALBINKPPR"};
    PPROD = @{ Data = "ZALBINKPPR"};
    PROD =  @{ Data = "YALBINFILE"};
    PARAME =  @{ Data = "ZLIVPARAM"};
}

$connection = new-object system.data.OleDb.OleDbConnection($ConnectionString);
$connection.Open();

function ReplaceVariables([String]$sqlFilePath, [ValidateSet('CONST','RECETTE','SUPPORT','FORM','PPROD','PROD','FIX','PARAME')][String]$envsource, [ValidateSet('CONST','RECETTE','SUPPORT','FORM','PPROD','PROD','FIX','PARAME')][String]$envcible){

	$from = $environments[$envsource]
	$to = $environments[$envcible]
	$SqlContent = (Get-Content -Raw $sqlFilePath).replace('[ENVSOURCE]', $from.Data).replace("[ENVCIBLE]", $to.Data)
	return $SqlContent
}

function ExecuteQuery([String]$sqlQuery, [System.Data.IDBCommand]$command) {
	$command.CommandText = $sqlQuery;
	$command.ExecuteNonQuery() | Out-Null
}

 $files = Get-ChildItem -Path $SqlFolderPath -filter '*.sql';
 $c=$connection.CreateCommand()
 #Parcours des fichiers SQL pour remplacer les paramètres "[envsource]" et  "[envcible"]"
 try {
	 $files | ForEach-Object {
	
			$s=$_
			$sqlFile = $($SqlFolderPath + '\' + $s)
			$content = ReplaceVariables $sqlFile -envsource $Source -envcible $Cible
			# Exécution de la requête sur le serveur cible 
			ExecuteQuery -sqlQuery $content -command  $c
	 } 
}
catch{
    # $Error[0].Exception
    Write-Error $Error[0].Exception
    Write-Error $s
    return $false;
}
finally {
	$connection.Close();
}
return $true