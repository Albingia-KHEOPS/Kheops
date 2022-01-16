using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class KpCtrlERepository {
        const string delete_by_etape_risque = "DELETE FROM KPCTRLE WHERE ( KEVIPB , KEVALX , KEVETAPE , KEVRSQ ) = ( :IPB , :ALX , :ETAPE , :RSQ ) ;";
        const string delete_by_option = "DELETE FROM KPCTRLE WHERE ( KEVIPB , KEVALX , KEVTYP , KEVOPT , KEVFOR ) = ( :IPB , :ALX , :TYP , :OPT , :FOR ) ;";
        const string delete_by_formule = "DELETE FROM KPCTRLE WHERE ( KEVIPB , KEVALX , KEVTYP , KEVFOR ) = ( :IPB , :ALX , :TYP , :FOR ) ;";

        public void DeleteFormuleCtrl(KpCtrlE data) {
            var parameters = new DynamicParameters();
            parameters.Add("IPB", data.Kevipb.ToIPB());
            parameters.Add("ALX", data.Kevalx);
            parameters.Add("TYP", data.Kevtyp);
            parameters.Add("FOR", data.Kevfor);
            this.connection.EnsureOpened().Execute(delete_by_formule, parameters);
        }

        public void DeleteOptionCtrl(KpCtrlE data) {
            var parameters = new DynamicParameters();
            parameters.Add("IPB", data.Kevipb.ToIPB());
            parameters.Add("ALX", data.Kevalx);
            parameters.Add("TYP", data.Kevtyp);
            parameters.Add("OPT", data.Kevopt);
            parameters.Add("FOR", data.Kevfor);
            this.connection.EnsureOpened().Execute(delete_by_option, parameters);
        }

        public void DeleteByEtapeRisque(string ipb, int alx, string etape, int risque) {
            var parameters = new DynamicParameters();
            parameters.Add("IPB", ipb);
            parameters.Add("ALX", alx);
            parameters.Add("ETAPE", etape);
            parameters.Add("RSQ", risque);
            this.connection.EnsureOpened().Execute(delete_by_etape_risque, parameters);
        }
    }
}
