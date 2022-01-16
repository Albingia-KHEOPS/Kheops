using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Dapper.FluentMap.Mapping;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {

    public partial class KpRguWpRepository {
        const string delete_ByAffaire = "DELETE FROM KPRGUWP WHERE ( KHYIPB , KHYALX , KHYTYP ) = ( :IPB , :ALX , 'P' )";
        const string insert_blank = @"INSERT INTO KPRGUWP 
( KHYIPB , KHYALX , KHYTYP , KHYAVN , KHYRSQ , KHYFOR , KHYKDEID , KHYGARAN , KHYDEBP , KHYFINP , KHYTRG ) 
VALUES ( :ipb, :alx, 'P', 0, :rsq, :formule, :kdeid, :garan, :deb, :fin, :trg ) ";

        public int Purge(string codeAffaire, int version) {
            return this.connection.EnsureOpened().Execute(delete_ByAffaire, new { IPB = codeAffaire, ALX = version });
        }

        public int InsertBlank(string ipb, int alx, int rsq, int formule, long kdeid, string garan, int deb, int fin, string trg) {
            return this.connection.EnsureOpened().Execute(insert_blank, new { ipb, alx, rsq, formule, kdeid, garan, deb, fin, trg });
        }
    }
}
