using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Dapper.FluentMap.Mapping;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{

    public partial class YPlmgaRepository : BaseTableRepository
    {

        private IdentifierGenerator idGenerator;

        #region Query texts        
        //ZALBINKMOD
        const string select = @"SELECT
D1MGA, D1LIB FROM YPLMGA
WHERE D1MGA = :modele
";
        const string update = @"UPDATE YPLMGA SET 
D1MGA = :D1MGA, D1LIB = :D1LIB
 WHERE 
D1MGA = :modele";
        const string delete = @"DELETE FROM YPLMGA WHERE D1MGA = :modele";
        const string insert = @"INSERT INTO  YPLMGA (
D1MGA, D1LIB
) VALUES (
:D1MGA, :D1LIB)";
        const string select_GetAll = @"SELECT
D1MGA, D1LIB FROM YPLMGA
";
        const string select_RechercherGarantieModele = @"SELECT
D1MGA, D1LIB FROM YPLMGA
WHERE D1MGA like :code
and D1LIB like :description
ORDER BY D1MGA
";
        #endregion

        public YPlmgaRepository(IDbConnection connection, IdentifierGenerator idGenerator) : base(connection)
        {
            this.idGenerator = idGenerator;
        }

        public YPlmga Get(string modele)
        {
            return connection.Query<YPlmga>(select, new { modele }).SingleOrDefault();
        }

        public string NewId()
        {
            return idGenerator.NewId("D1MGA").ToString();
        }

        public void Insert(YPlmga value)
        {
            if (value.D1mga == default(string))
            {
                value.D1mga = NewId();
            }
            var parameters = new DynamicParameters();
            parameters.Add("D1MGA", value.D1mga ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 15, scale: 0);
            parameters.Add("D1LIB", value.D1lib ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 30, scale: 0);

            connection.EnsureOpened().Execute(insert, parameters);
        }
        public void Delete(YPlmga value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("modele", value.D1mga);
            connection.EnsureOpened().Execute(delete, parameters);
        }

        public void Update(YPlmga value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("D1MGA", value.D1mga ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 15, scale: 0);
            parameters.Add("D1LIB", value.D1lib ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 30, scale: 0);
            parameters.Add("modele", value.D1mga);

            connection.EnsureOpened().Execute(update, parameters);
        }

        public IEnumerable<YPlmga> GetAll()
        {
            return connection.EnsureOpened().Query<YPlmga>(select_GetAll).ToList();
        }
        public IEnumerable<YPlmga> RechercherGarantieModele(string code, string description)
        {
            return connection.EnsureOpened().Query<YPlmga>(select_RechercherGarantieModele, new { code, description }).ToList();
        }
    }
}
