using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie.GarantieModele
{
    public class KpgaranDto
    {
        public long KDEID { get; set; }
        public string KDETYP { get; set; }
        public string KDEIPB { get; set; }
        public int KDEALX { get; set; }
        public int KDEFOR { get; set; }
        public int KDEOPT { get; set; }
        public long KDEKDCID { get; set; }
        public string KDEGARAN { get; set; }
        public long KDESEQ { get; set; }
        public int KDENIVEAU { get; set; }
        public long KDESEM { get; set; }
        public long KDESE1 { get; set; }
        public string KDETRI { get; set; }
        public double KDENUMPRES { get; set; }
        public string KDEAJOUT { get; set; }
        public string KDECAR { get; set; }
        public string KDENAT { get; set; }
        public string KDEGAN { get; set; }
        public long KDEKDFID { get; set; }
        public string KDEDEFG { get; set; }
        public long KDEKDHID { get; set; }
        public long KDEDATDEB { get; set; }
        public long KDEHEUDEB { get; set; }
        public long KDEDATFIN { get; set; }
        public long KDEHEUFIN { get; set; }
        public int KDEDUREE { get; set; }
        public string KDEDURUNI { get; set; }
        public string KDEPRP { get; set; }
        public string KDETYPEMI { get; set; }
        public string KDEALIREF { get; set; }
        public string KDECATNAT { get; set; }
        public string KDEINA { get; set; }
        public string KDETAXCOD { get; set; }
        public double KDETAXREP { get; set; }
        public int KDECRAVN { get; set; }
        public string KDECRU { get; set; }
        public long KDECRD { get; set; }
        public int KDEMAJAVN { get; set; }
        public double KDEASVALO { get; set; }
        public double KDEASVALA { get; set; }
        public double KDEASVALW { get; set; }
        public string KDEASUNIT { get; set; }
        public string KDEASBASE { get; set; }
        public string KDEASMOD { get; set; }
        public string KDEASOBLI { get; set; }
        public string KDEINVSP { get; set; }
        public long KDEINVEN { get; set; }
        public long KDEWDDEB { get; set; }
        public long KDEWHDEB { get; set; }
        public long KDEWDFIN { get; set; }
        public long KDEWHFIN { get; set; }
        public string KDETCD { get; set; }
        public string KDEMODI { get; set; }
        public string KDEPIND { get; set; }
        public string KDEPCATN { get; set; }
        public string KDEPREF { get; set; }
        public string KDEPPRP { get; set; }
        public string KDEPEMI { get; set; }
        public string KDEPTAXC { get; set; }
        public string KDEPNTM { get; set; }
        public string KDEALA { get; set; }
        public string KDEPALA { get; set; }
        public string KDEALO { get; set; }


        public string Nature { get; set; }
        public bool IsChecked
        {
            get
            {
                if (((KDECAR == "O" && (Nature != "E" || KDEGAN != "E")) || (KDECAR == "B" && Nature == "C")) && KDEGAN != "E")
                    return true;
                if (KDECAR == "P" && ((Nature != "E" && KDEGAN != "" && KDEGAN != "E") || (Nature == "E" && KDEGAN != "E")))
                    return true;

                if (KDECAR == "F" && (Nature == "C" || Nature == "A") && !string.IsNullOrEmpty(KDEGAN) && KDEGAN != "E")
                    return true;
                if (KDECAR == "F" && Nature == "E" && KDEGAN == "A")
                    return true;
                return false;
            }
        }
    }
}
