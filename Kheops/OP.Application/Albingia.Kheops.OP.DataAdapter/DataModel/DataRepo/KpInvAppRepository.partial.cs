using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Dapper.FluentMap.Mapping;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Extension;
using ALBINGIA.Framework.Common.Extensions;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {

    public partial class KpInvAppRepository {

        const string deleteInven = @"DELETE FROM KPINVAPP WHERE KBGKBEID=:idInven";
        const string selectNextNumInven = @"SELECT IFNULL(MAX(KBGNUM), 0) + 1 FROM KPINVAPP WHERE KBGIPB = :ipb AND KBGALX = :alx AND KBGTYP = :typ";

        public void DeleteInven(long idInven) {
            try {
                var parameters = new DynamicParameters();
                connection.EnsureOpened().Execute(deleteInven, new { idInven });
            }
            finally {
                connection.EnsureClosed();
            }
        }

        internal int GetNextNum(AffaireId affaire) {
            try {
                var parameters = new DynamicParameters();
                int result = connection.EnsureOpened().ExecuteScalar<int>(selectNextNumInven, new { ipb = affaire.CodeAffaire.PadLeft(9, ' '), alx = affaire.NumeroAliment, typ = affaire.TypeAffaire.AsCode() });
                return result;
            }
            finally {
                connection.EnsureClosed();
            }
        }
    }
}
