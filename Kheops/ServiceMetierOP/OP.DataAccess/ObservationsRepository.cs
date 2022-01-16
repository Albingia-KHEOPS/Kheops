using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common.Data;
using System.Data;

namespace OP.DataAccess
{
    public class ObservationsRepository : RepositoryBase {
        internal static readonly string ColumnId = "KAJCHR";
        internal static readonly string Insert = @"INSERT INTO KPOBSV 
( KAJCHR , KAJTYPOBS , KAJIPB , KAJALX , KAJTYP , KAJOBSV ) 
VALUES ( :CHR , :TYPE , :IPB , :ALX , :TYP , :OBSV ) ;";
        internal static readonly string Update = "UPDATE KPOBSV SET KAJOBSV = :OBSV WHERE KAJCHR = :CHR ;";

        public ObservationsRepository(IDbConnection connection) : base(connection) { }

        public int AddOrUpdate(Observation observation) {
            if (observation.Id.GetValueOrDefault() < 1) {
                observation.Id = CommonRepository.GetAS400Id(ColumnId, this.connection);
                using (var options = new DbExecOptions(this.connection == null) {
                    DbConnection = this.connection,
                    SqlText = FormatQuery(Insert)
                }) {
                    options.BuildParameters(observation.Id, observation.Type, observation.Folder.CodeOffre, observation.Folder.Version, observation.Folder.Type, observation.Texte ?? string.Empty);
                    options.Exec();
                }
            }
            else {
                using (var options = new DbExecOptions(this.connection == null) {
                    DbConnection = this.connection,
                    SqlText = FormatQuery(Update)
                }) {
                    options.BuildParameters(observation.Texte, observation.Id);
                    options.Exec();
                }
            }
            return observation.Id.GetValueOrDefault();
        }
    }
}
