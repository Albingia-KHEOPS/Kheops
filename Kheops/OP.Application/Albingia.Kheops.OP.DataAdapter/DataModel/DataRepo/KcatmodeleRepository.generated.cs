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

    public partial class KcatmodeleRepository : BaseTableRepository
    {

        private IdentifierGenerator idGenerator;

        #region Query texts        
        //ZALBINKHEO
        const string select = @"SELECT
KARID, KARKAQID, KARDATEAPP, KARTYPO, KARMODELE
, KARCRU, KARCRD, KARCRH, KARMAJU, KARMAJD
, KARMAJH FROM KCATMODELE
WHERE KARID = :id
FETCH FIRST 200 ROWS ONLY
";
        const string update = @"UPDATE KCATMODELE SET 
KARID = :KARID, KARKAQID = :KARKAQID, KARDATEAPP = :KARDATEAPP, KARTYPO = :KARTYPO, KARMODELE = :KARMODELE, KARCRU = :KARCRU, KARCRD = :KARCRD, KARCRH = :KARCRH, KARMAJU = :KARMAJU, KARMAJD = :KARMAJD
, KARMAJH = :KARMAJH
 WHERE 
KARID = :id";
        const string delete = @"DELETE FROM KCATMODELE WHERE KARID = :id";
        const string insert = @"INSERT INTO  KCATMODELE (
KARID, KARKAQID, KARDATEAPP, KARTYPO, KARMODELE
, KARCRU, KARCRD, KARCRH, KARMAJU, KARMAJD
, KARMAJH
) VALUES (
:KARID, :KARKAQID, :KARDATEAPP, :KARTYPO, :KARMODELE
, :KARCRU, :KARCRD, :KARCRH, :KARMAJU, :KARMAJD
, :KARMAJH)";
        const string select_ListeBrancheCible = @"SELECT
KARID AS KARID, KARKAQID AS KARKAQID, KARDATEAPP AS KARDATEAPP, KARTYPO AS KARTYPO, KARMODELE AS KARMODELE
 FROM KCATMODELE
where karkaqid in
(Select KAQID from KCatBloc where KAQKAPID in
(Select KAPID from KCATVOLET where KAPBRA = :KAQBRA and KAPCIBLE = :KAQCIBLE))
FETCH FIRST 200 ROWS ONLY
";
        const string select_GetAll = @"SELECT
KARID, KARKAQID, KARDATEAPP, KARTYPO, KARMODELE
, KARCRU, KARCRD, KARCRH, KARMAJU, KARMAJD
, KARMAJH FROM KCATMODELE
";
        const string select_GetByModele = @"SELECT
KARID, KARKAQID, KARDATEAPP, KARTYPO, KARMODELE
, KARCRU, KARCRD, KARCRH, KARMAJU, KARMAJD
, KARMAJH FROM KCATMODELE
WHERE KARMODELE = :code
";
        #endregion

        public KcatmodeleRepository(IDbConnection connection, IdentifierGenerator idGenerator) : base(connection)
        {
            this.idGenerator = idGenerator;
        }

        public Kcatmodele Get(int id)
        {
            return connection.Query<Kcatmodele>(select, new { id }).SingleOrDefault();
        }

        public int NewId()
        {
            return idGenerator.NewId("KARID");
        }

        public void Insert(Kcatmodele value)
        {
            if (value.Karid == default(int))
            {
                value.Karid = NewId();
            }
            var parameters = new DynamicParameters();
            parameters.Add("KARID", value.Karid, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KARKAQID", value.Karkaqid, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KARDATEAPP", value.Kardateapp, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KARTYPO", value.Kartypo ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KARMODELE", value.Karmodele ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 15, scale: 0);
            parameters.Add("KARCRU", value.Karcru ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("KARCRD", value.Karcrd, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KARCRH", value.Karcrh, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KARMAJU", value.Karmaju ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("KARMAJD", value.Karmajd, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KARMAJH", value.Karmajh, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);

            connection.EnsureOpened().Execute(insert, parameters);
        }
        public void Delete(Kcatmodele value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("id", value.Karid);
            connection.EnsureOpened().Execute(delete, parameters);
        }

        public void Update(Kcatmodele value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("KARID", value.Karid, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KARKAQID", value.Karkaqid, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KARDATEAPP", value.Kardateapp, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KARTYPO", value.Kartypo ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KARMODELE", value.Karmodele ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 15, scale: 0);
            parameters.Add("KARCRU", value.Karcru ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("KARCRD", value.Karcrd, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KARCRH", value.Karcrh, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KARMAJU", value.Karmaju ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("KARMAJD", value.Karmajd, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KARMAJH", value.Karmajh, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("id", value.Karid);

            connection.EnsureOpened().Execute(update, parameters);
        }

        public IEnumerable<KcatModeleBrancheCible> ListeBrancheCible(string branche, string cible)
        {
            return connection.EnsureOpened().Query<KcatModeleBrancheCible>(select_ListeBrancheCible, new { branche, cible }).ToList();
        }
        public IEnumerable<Kcatmodele> GetAll()
        {
            return connection.EnsureOpened().Query<Kcatmodele>(select_GetAll).ToList();
        }
        public IEnumerable<Kcatmodele> GetByModele(string code)
        {
            return connection.EnsureOpened().Query<Kcatmodele>(select_GetByModele, new { code }).ToList();
        }
    }
}
