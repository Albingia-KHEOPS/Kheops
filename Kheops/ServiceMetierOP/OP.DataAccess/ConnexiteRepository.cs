using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using OP.WSAS400.DTO.Engagement;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OP.DataAccess
{
    public class ConnexiteRepository : RepositoryBase {
        internal static readonly string SelectNumero = "SELECT PJCNX FROM YPOCONX WHERE PJIPB = :ipb AND PJALX = :alx AND PJTYP = :typ AND PJCCX = :typcnn ; ";
        internal static readonly string SelectContratsConnexes = @"
SELECT PJCNX NUMCONNEXITE , PJIDE IDECONNEXITE , PJCCX CODETYPECONNEXITE , PJIPB NUMCONTRAT , PJALX VERSIONCONTRAT , PJTYP TYPECONTRAT , 
    PBREF DESCRIPTIONCONTRAT , BASE.PBBRA CODEBRANCHE , TPLIL LIBELLEBRANCHE , BASE.PBIAS CODEPRENEUR , NOM.ANNOM NOMPRENEUR , 
    IFNULL ( KAACIBLE , '' ) CODECIBLE , IFNULL ( CIBLE.KAHDESC , '' ) LIBELLECIBLE , PJOBS CODEOBSERVATION , TRIM(OBSV.KAJOBSV) OBSERVATION , 
    ASSUR.ASAD1 AD1 , ASSUR.ASAD2 AD2 , ASSUR.ASDEP DEP , ASSUR.ASCPO CP , ASSUR.ASVIL VILLE , 
    PBSIT SITUATION , PBETA ETAT , PBCAT , PBSBR 
FROM YPOCONX 
INNER JOIN YPOBASE BASE ON BASE.PBIPB = PJIPB AND BASE.PBALX = PJALX AND BASE.PBTYP = PJTYP 
AND PJTYP = :typ AND PJCCX = :typcnn AND PJCNX = :numero 
LEFT JOIN KPENT ON ( BASE.PBIPB , BASE.PBTYP , BASE.PBALX ) = ( KAAIPB , KAATYP , KAAALX ) 
LEFT JOIN YYYYPAR ON TCON = 'GENER' AND TFAM = 'BRCHE' AND TCOD = PJBRA AND TPCN2 = 1 
LEFT JOIN KPRSQ RSQ ON RSQ.KABIPB=PJIPB AND RSQ.KABALX = PJALX AND RSQ.KABTYP = PJTYP AND KABRSQ = '1' 
LEFT JOIN KCIBLE CIBLE ON CIBLE.KAHCIBLE = RSQ.KABCIBLE 
LEFT JOIN KPOBSV OBSV ON OBSV.KAJCHR = PJOBS 
LEFT JOIN YASSNOM NOM  ON NOM.ANIAS = BASE.PBIAS AND NOM.ANINL = 0 AND NOM.ANTNM = 'A' 
LEFT JOIN YASSURE ASSUR ON ASSUR.ASIAS = BASE.PBIAS ; ";
        internal static readonly string SelectEngagementsConnexes = @"
SELECT PJCNX NUMCONNEXITE , PJIDE IDECONNEXITE , PJCCX CODETYPECONNEXITE , PJIPB NUMCONTRAT , PJALX VERSIONCONTRAT , PJTYP TYPECONTRAT , 
    PBREF DESCRIPTIONCONTRAT , PBBRA CODEBRANCHE , TPLIL LIBELLEBRANCHE , PBIAS CODEPRENEUR , NOM.ANNOM NOMPRENEUR , IFNULL ( KAACIBLE , '' ) CODECIBLE , 
    IFNULL ( KAHDESC , '' ) LIBELLECIBLE , PJOBS CODEOBSERVATION , IFNULL( OBSV.KAJOBSV , '' ) OBSERVATION , 
    PBSIT SITUATION , PBETA ETAT , KDPFAM CODEENGAGEMENT , KDPENA VALEURENGAGEMENT , KDPENG TOTALENGAGEMENT , PBCAT , PBSBR 
FROM YPOCONX 
INNER JOIN YPOBASE ON PBIPB = PJIPB AND PBALX = PJALX AND PBTYP = PJTYP 
AND PJTYP = :typ AND PJCCX = :typcnn AND PJCNX = :numero 
LEFT JOIN KPENG  ON KDOTYP = PJTYP AND KDOIPB = PJIPB AND  KDOALX = PJALX 
LEFT JOIN KPENGFAM ON KDPKDOID = KDOID 
LEFT JOIN YYYYPAR ON TCON = 'GENER' AND TFAM = 'BRCHE' AND TCOD = PBBRA 
LEFT JOIN KPENT ON KAAIPB = PJIPB AND KAAALX = PJALX AND KAATYP = PJTYP 
LEFT JOIN KCIBLE ON  KAACIBLE = KAHCIBLE 
LEFT JOIN KPOBSV OBSV ON OBSV.KAJCHR = PJOBS 
LEFT JOIN YASSNOM NOM  ON NOM.ANIAS = PBIAS AND NOM.ANINL = 0 AND NOM.ANTNM = 'A' ; ";
        internal static readonly string SelectFolders = @"SELECT PJIPB NUMCONTRAT , PJALX VERSIONCONTRAT , PJTYP TYPECONTRAT, PJIDE IDECONNEXITE FROM YPOCONX WHERE PJCCX = :TPCNX AND PJCNX = :NUM ;";
        internal static readonly string CountExisting = @"SELECT COUNT ( * ) FROM YPOCONX WHERE PJIPB = :IPB AND PJALX = :ALX AND PJTYP = :TYP AND PJCCX = :TPCNX AND PJCNX = :NUM ;";
        internal static readonly string InsertConnexite = @"INSERT INTO YPOCONX 
( PJIPB , PJALX , PJTYP , PJCCX , PJCNX , PJIDE , PJBRA , PJSBR , PJCAT , PJOBS ) 
VALUES ( :IPB , :ALX , :TYP , :TYPE , :NUM , :IDE , :BRC , :SBR , :CAT , :CODEOBSV ) ; ";
        internal static readonly string UpdateIdConnexiteEngagement = "UPDATE YPOCONX SET PJIDE = :ID WHERE PJCNX = :NUM ;";
        internal static readonly string DeleteConnexite = "DELETE FROM YPOCONX WHERE PJIPB = :IPB AND PJALX = :ALX AND PJTYP = :TYP AND PJCCX = :TPCNX AND PJCNX = :NUM ;";

        public ConnexiteRepository(IDbConnection connection) : base(connection) { }

        public int GetNumero(Folder folder, TypeConnexite type) {
            using (var options = new DbSelectInt32Options(this.connection == null) {
                DbConnection = this.connection,
                SqlText = FormatQuery(SelectNumero)
            }) {
                options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, folder.Type, ((int)type).ToString().PadLeft(2, '0'));
                options.PerformSelect();
                return options.IntegerList?.FirstOrDefault() ?? default;
            }
        }

        public IEnumerable<ContratConnexeDto> GetFolders(TypeConnexite type, int numero) {
            return Fetch<ContratConnexeDto>(SelectFolders, ((int)type).ToString().PadLeft(2, '0'), numero);
        }

        public IEnumerable<ContratConnexeDto> GetContrats(Folder folder, TypeConnexite type, int numero) {
            return GetContrats(folder.Type, type, numero);
        }

        public IEnumerable<ContratConnexeDto> GetContrats(string folderType, TypeConnexite type, int numero) {
            return Fetch<ContratConnexeDto>(type == TypeConnexite.Engagement ? SelectEngagementsConnexes : SelectContratsConnexes, folderType, ((int)type).ToString().PadLeft(2, '0'), numero);
        }

        public bool IsContratConnexe(Folder folder, TypeConnexite type, int numero) {
            using (var options = new DbCountOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = FormatQuery(CountExisting)
            }) {
                options.BuildParameters(folder.CodeOffre, folder.Version, folder.Type, ((int)type).ToString().PadLeft(2, '0'), numero);
                options.PerformCount();
                return options.Count > 0;
            }
        }

        public void Add(Folder folder, ContratConnexeDto contratConnexe) {
            if (!IsContratConnexe(folder, (TypeConnexite)int.Parse(contratConnexe.CodeTypeConnexite), contratConnexe.NumConnexite)) {
                using (var options = new DbExecOptions(this.connection == null) {
                    DbConnection = this.connection,
                    SqlText = FormatQuery(InsertConnexite)
                }) {
                    options.BuildParameters(
                        folder.CodeOffre,
                        folder.Version,
                        folder.Type,
                        contratConnexe.CodeTypeConnexite.ToString().PadLeft(2, '0'),
                        contratConnexe.NumConnexite,
                        contratConnexe.IdeConnexite,
                        contratConnexe.CodeBranche,
                        contratConnexe.CodeSousBranche,
                        contratConnexe.CodeCategorie,
                        contratConnexe.CodeObservation);
                    options.Exec();
                }
            }

            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = FormatQuery(InsertConnexite)
            }) {
                options.BuildParameters(
                    contratConnexe.NumContrat,
                    contratConnexe.VersionContrat,
                    contratConnexe.TypeContrat,
                    contratConnexe.CodeTypeConnexite.ToString().PadLeft(2, '0'),
                    //ToString().PadLeft(2, '0')
                    contratConnexe.NumConnexite,
                    contratConnexe.IdeConnexite,
                    contratConnexe.CodeBranche,
                    contratConnexe.CodeSousBranche,
                    contratConnexe.CodeCategorie,
                    contratConnexe.CodeObservation);
                options.Exec();
            }
        }

        public void SetIdConnexiteEngagement(int numero, int idConnexite) {
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = UpdateIdConnexiteEngagement
            }) {
                options.BuildParameters(idConnexite, numero);
                options.Exec();
            }
        }

        public void Delete(Folder folder, int type, int numero) {
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = DeleteConnexite
            }) {
                //var x = type.ToString().PadLeft(2, '0');
                options.BuildParameters(folder.CodeOffre, folder.Version, folder.Type, type.ToString().PadLeft(2, '0'), numero);
                options.Exec();
            }
        }
    }
}
