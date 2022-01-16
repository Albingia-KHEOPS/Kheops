using System.Collections.Generic;
using System.Data;
using System.Linq;
using Hexavia.Models;
using Hexavia.Repository.Interfaces;

namespace Hexavia.Repository
{
    /// <summary>
    /// Layer repository
    /// </summary>
    public class LayerRepository : BaseRepository, ILayerRepository
    {

        public LayerRepository(DataAccessManager dataAccessManager)
           : base(dataAccessManager)
        {
        }
        /// <summary>
        /// Get all layers
        /// </summary>
        /// <returns></returns>
        public List<Layer> GetAll()
        {
            var result = new List<Layer>();
            var cmd = DataAccessManager.CmdWrapper;
            cmd.CommandText = string.Format(@"SELECT ID, NAME, LAYERINFO, TYPOLOGIE, BRANCHE, MODIF, DATECREAT, DATEMODIF FROM KLAYER");
            var dataTable = DataAccessManager.ExecuteDataTable(cmd);

            foreach (DataRow row in dataTable.Rows)
            {
                var layer = new Layer
                {
                    Id = row["ID"].ToString(),
                    Name = row["NAME"].ToString(),
                    Shape = row["LAYERINFO"].ToString(),
                    Caracteristique = row["TYPOLOGIE"].ToString(),
                    Branche = row["BRANCHE"].ToString(),
                    Modifiable = row["MODIF"].ToString(),
                    Date = row["DATEMODIF"].ToString() == "0" ? row["DATECREAT"].ToString().PadLeft(8, '0') : row["DATEMODIF"].ToString().PadLeft(8, '0')
                };
                result.Add(layer);
            }
            return result.OrderByDescending(x => x.Date).ToList();
        }
        /// <summary>
        /// Get layer by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Layer Get(string name)
        {
            var result = new Layer();
            var cmd = DataAccessManager.CmdWrapper;
            cmd.CommandText = string.Format(@"SELECT ID, NAME, LAYERINFO FROM KLAYER WHERE NAME ='{0}'", name);
            var dataTable = DataAccessManager.ExecuteDataTable(cmd);
            if (dataTable.Rows.Count != 0)
            {
                var row = dataTable.Rows[0];
                result.Id = row["ID"].ToString();
                result.Name = row["NAME"].ToString();
                result.Shape = row["LAYERINFO"].ToString();
            }
            return result;
        }
        /// <summary>
        /// Verify layer name exist or not
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exist(string name)
        {
            var cmd = DataAccessManager.CmdWrapper;
            cmd.CommandText = string.Format(@"SELECT COUNT(*) FROM KLAYER WHERE NAME ='{0}'", name);
            return DataAccessManager.ExecuteExist(cmd);
        }

        /// <summary>
        /// Insert one layer
        /// </summary>
        /// <param name="layer"></param>
        public void Insert(Layer layer)
        {
            var cmd = DataAccessManager.CmdWrapper;
            var input = new List<Param>
            {
                new Param(DbType.String, "ID", layer.Id),
                new Param(DbType.String, "NAME", layer.Name),
                new Param(DbType.String, "LAYERINFO", layer.Shape),
                new Param(DbType.String, "TYPOLOGIE", layer.Caracteristique),
                new Param(DbType.String, "BRANCHE", layer.Branche),
                new Param(DbType.String, "MODIF", layer.Modifiable),
                new Param(DbType.String, "DATECREAT", layer.Date)
            };
            cmd.InsertInto("KLAYER", input);
            DataAccessManager.ExecuteNonQuery(cmd);
        }
        /// <summary>
        /// Insert multi-layers
        /// </summary>
        /// <param name="layers"></param>
        public void Insert(List<Layer> layers)
        {
            foreach (var layer in layers)
            {
                Insert(layer);
            }
        }
        /// <summary>
        ///  Update one layer
        /// </summary>
        /// <param name="layer"></param>
        public void Update(Layer layer)
        {
            var cmd = DataAccessManager.CmdWrapper;
            var inputs = new List<Param>
            {
                new Param(DbType.String, "ID", layer.Id),
                new Param(DbType.String, "NAME", layer.Name),
                new Param(DbType.String, "LAYERINFO", layer.Shape),
                new Param(DbType.String, "TYPOLOGIE", layer.Caracteristique),
                new Param(DbType.String, "BRANCHE", layer.Branche),
                new Param(DbType.String, "MODIF", layer.Modifiable),
                new Param(DbType.String, "DATEMODIF", layer.Date)

            };
            cmd.CommandText = string.Format(@"UPDATE KLAYER SET NAME = '{0}' , LAYERINFO = '{1}', TYPOLOGIE = '{2}', BRANCHE = '{3}', MODIF = '{4}', DATEMODIF = '{5}' WHERE ID = '{6}'"
                , layer.Name, layer.Shape, layer.Caracteristique, layer.Branche, layer.Modifiable, layer.Date, layer.Id);
            DataAccessManager.ExecuteNonQuery(cmd);
        }
        /// <summary>
        /// Update multi-layers
        /// </summary>
        /// <param name="layers"></param>
        public void Update(List<Layer> layers)
        {
            foreach (var layer in layers)
            {
                Update(layer);
            }
        }

        /// <summary>
        ///  Delete layer by id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id)
        {
            var cmd = DataAccessManager.CmdWrapper;
            cmd.CommandText = string.Format("DELETE FROM  KLAYER WHERE ID ='{0}'", id);
            DataAccessManager.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Delete multi-layers with ids
        /// </summary>
        /// <param name="ids"></param>
        public void Delete(List<string> ids)
        {
            foreach (var id in ids)
            {
                Delete(id);
            }
        }

    }
}
