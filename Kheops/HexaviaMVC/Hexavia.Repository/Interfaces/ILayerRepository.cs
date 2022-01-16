using Hexavia.Models;
using System.Collections.Generic;

namespace Hexavia.Repository.Interfaces
{
    /// <summary>
    /// Layer repository interface
    /// </summary>
    public interface ILayerRepository
    {
        /// <summary>
        /// Get all layers
        /// </summary>
        /// <returns></returns>
        List<Layer> GetAll();

        /// <summary>
        /// Get layer by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Layer Get(string name);

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
        /// Insert multi-layers
        /// </summary>
        /// <param name="layers"></param>
        void Insert(List<Layer> layers);

        /// <summary>
        /// Update one layer
        /// </summary>
        /// <param name="layer"></param>
        void Update(Layer layer);

        /// <summary>
        /// Update multi-layers
        /// </summary>
        /// <param name="layers"></param>
        void Update(List<Layer> layers);

        /// <summary>
        /// Delete layer by id
        /// </summary>
        /// <param name="id"></param>
        void Delete(string id);

        /// <summary>
        /// Delete multi-layers with ids
        /// </summary>
        /// <param name="ids"></param>
        void Delete(List<string> ids);
    }
}
