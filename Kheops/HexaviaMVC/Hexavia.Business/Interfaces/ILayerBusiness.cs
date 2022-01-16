using Hexavia.Models;
using System.Collections.Generic;

namespace Hexavia.Business.Interfaces
{
    /// <summary>
    /// Layer service interface
    /// </summary>
    public interface ILayerBusiness
    {
        /// <summary>
        /// Get all layers
        /// </summary>
        /// <returns></returns>
        List<Layer> GetAll();

        /// <summary>
        /// Verify layer name exist or not
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool Exist(string name);

        /// <summary>
        /// Insert one layer
        /// </summary>
        /// <param name="layer"></param>
        void Insert(Layer layer);

        /// <summary>
        /// Update multi-layers
        /// </summary>
        /// <param name="layers"></param>
        void Update(List<Layer> layers);

        /// <summary>
        /// Update layer
        /// </summary>
        /// <param name="layer"></param>
        void Update(Layer layer);
        /// <summary>
        ///  Delete multi-layers with ids
        /// </summary>
        /// <param name="ids"></param>
        void Delete(List<string> ids);
        /// <summary>
        ///  Delete layer by id
        /// </summary>
        /// <param name="id"></param>
        void Delete(string id);
    }
}
