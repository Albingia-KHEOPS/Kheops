using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using OP.WSAS400.DTO.NavigationArbre;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.DataAccess {
    public partial class NavigationArbreRepository {
        private const string NoAvnColumns = "-1 AVTFOR, 0 CREATEAVT, 0 MODIFAVT";
        internal static readonly string GlobalSelect = $@"SELECT KEVTYP TYPE , KEVIPB CODEOFFRE , KEVALX VERSION , PBREF DESCOFFRE , KEVETAPE ETAPE , 
KEVETORD ETAPEORD , KEVORDR ORDRE , KEVPERI PERIMETRE , 
KEVRSQ CODERSQ , IFNULL ( KABDESC , '' ) DESCRSQ , KEVOBJ CODEOBJ , 
KEVFOR CODEFOR , IFNULL ( KDAALPHA , '' ) LETTREFOR , IFNULL ( KDADESC , '' ) DESCFOR , KEVOPT CODEOPT , IFNULL ( JECCH , 0 ) CHRONORSQ , 
KEVTAG PASSAGETAG,  (PBAVA * 10000 + PBAVM * 100 + PBAVJ) DATEDEBAVN, DATEFIN DATEFINRSQ, PBAVN NUMAVN , {NoAvnColumns} 
/* IFNULL(KHOFOR, 0) AVTFOR, IFNULL(KDBAVE, 0) CREATEAVT, IFNULL(KDBAVG, 0) MODIFAVT */
FROM KPCTRLE 
INNER JOIN YPOBASE ON PBTYP = KEVTYP AND PBIPB = KEVIPB AND PBALX = KEVALX 
LEFT JOIN YPRTRSQ ON JERSQ = KEVRSQ AND JEIPB = KEVIPB AND JEALX = KEVALX 
LEFT JOIN KPRSQ ON KABTYP = KEVTYP AND KABIPB = KEVIPB AND KABALX = KEVALX AND KABRSQ = KEVRSQ 
LEFT JOIN KJOBSORTI ON ( :MHA , IPB, ALX, TYP, RSQ, AVN ) = ( 0 , KABIPB , KABALX , KABTYP  , KABRSQ , PBAVN ) 
LEFT JOIN KPOPTAP ON KDDTYP = KEVTYP AND KDDIPB = KEVIPB AND KDDALX = KEVALX AND KDDRSQ = KEVRSQ 
LEFT JOIN KPFOR ON KDATYP = KEVTYP AND KDAIPB = KEVIPB AND KDAALX = KEVALX AND KDAFOR = KEVFOR 
LEFT JOIN KPOPT ON KDBKDAID = KDAID 
/* LEFT JOIN KPAVTRC ON KHOIPB = KDAIPB AND KHOALX = KDAALX AND KHOTYP = KDATYP AND TRIM(KHOPERI) = 'OPT' AND KHOFOR = KDAFOR */
WHERE KEVIPB = :IPB AND KEVALX = :ALX AND KEVTYP = :TYP
AND IFNULL(sorti, 'N') <> 'O' ORDER BY KEVETORD , KEVRSQ , KEVOBJ , KEVFOR , KEVOPT ;";
        internal static readonly string SelectHistory = @"SELECT KEVTYP TYPE , KEVIPB CODEOFFRE , KEVALX VERSION , PBREF DESCOFFRE , KEVETAPE ETAPE , KEVETORD ETAPEORD , KEVORDR ORDRE , KEVPERI PERIMETRE , 
KEVRSQ CODERSQ , IFNULL ( KABDESC , '' ) DESCRSQ , KEVOBJ CODEOBJ , 
KEVFOR CODEFOR , IFNULL ( KDAALPHA , '' ) LETTREFOR , IFNULL ( KDADESC , '' ) DESCFOR , KEVOPT CODEOPT , IFNULL ( JECCH , 0 ) CHRONORSQ , 
KEVTAG PASSAGETAG 
FROM HPCTRLE 
INNER JOIN YHPBASE ON PBIPB = KEVIPB AND PBALX = KEVALX AND PBTYP = KEVTYP AND PBAVN = KEVAVN 
LEFT JOIN YHRTRSQ ON JERSQ = KEVRSQ AND JEIPB = KEVIPB AND JEALX = KEVALX AND JEAVN = KEVAVN 
LEFT JOIN HPRSQ ON KABTYP = KEVTYP AND KABIPB = KEVIPB AND KABALX = KEVALX AND KABRSQ = KEVRSQ AND KABAVN = KEVAVN 
LEFT JOIN HPOPTAP ON KDDTYP = KEVTYP AND KDDIPB = KEVIPB AND KDDALX = KEVALX AND KDDRSQ = KEVRSQ AND KDDAVN = KEVAVN 
LEFT JOIN HPFOR ON KDATYP = KEVTYP AND KDAIPB = KEVIPB AND KDAALX = KEVALX AND KDAFOR = KEVFOR AND KDAAVN = KEVAVN 
WHERE KEVIPB = :IPB AND KEVALX = :ALX AND KEVTYP = :TYP AND KEVAVN = :AVN 
ORDER BY KEVETORD , KEVRSQ , KEVOBJ , KEVFOR , KEVOPT ;";

        public NavigationArbreRepository(IDbConnection connection) : base(connection) { }

        private static string FormatGlobalSelect(Folder folder, ModeConsultation mode) {
            if (folder.NumeroAvenant < 1) {
                return GlobalSelect;
            }
            return GlobalSelect
                .Replace("/* ", string.Empty)
                .Replace(" */", string.Empty)
                .Replace(NoAvnColumns, $"/*{NoAvnColumns}*/");
        }

        public List<ArbreDto> GetFullTree(Folder folder, bool isModifHorsAvenant, ModeConsultation mode) {
            if (mode == ModeConsultation.Historique) {
                return Fetch<ArbreDto>(SelectHistory, folder.CodeOffre.ToIPB(), folder.Version, folder.Type, folder.NumeroAvenant).ToList();
            }
            return Fetch<ArbreDto>(FormatGlobalSelect(folder, mode), isModifHorsAvenant ? 1 : 0, folder.CodeOffre.ToIPB(), folder.Version, folder.Type).ToList();
        }
    }
}
