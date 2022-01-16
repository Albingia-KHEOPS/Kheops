using System.Collections.Generic;
using Hexavia.Business.Interfaces;
using Hexavia.Models;
using Hexavia.Models.EnumDir;
using Hexavia.Repository.Interfaces;

namespace Hexavia.Business
{
    /// <summary>
    /// Layer service
    /// </summary>
    public class LayerBusiness : ILayerBusiness
    {
        private readonly ILayerRepository LayerRepository;
        public LayerBusiness(ILayerRepository layerRepository)
        {
            LayerRepository = layerRepository;
        }
        /// <summary>
        /// Get all layers
        /// </summary>
        /// <returns></returns>
        public List<Layer> GetAll()
        {
            return LayerRepository.GetAll();
        }
        /// <summary>
        /// Verify layer name exist or not
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exist(string name)
        {
            return LayerRepository.Exist(name);
        }
        /// <summary>
        ///  Insert one layer
        /// </summary>
        /// <param name="layer"></param>
        public void Insert(Layer layer)
        {
            LayerRepository.Insert(layer);
        }
       
        /// <summary>
        /// Update multi-layers
        /// </summary>
        /// <param name="layers"></param>
        public void Update(List<Layer> layers)
        {
            LayerRepository.Update(layers);
        }

        /// <summary>
        /// Update layer
        /// </summary>
        /// <param name="layer"></param>
        public void Update(Layer layer)
        {
            LayerRepository.Update(layer);
        }
        /// <summary>
        /// Delete multi-layers with ids
        /// </summary>
        /// <param name="ids"></param>
        public void Delete(List<string> ids)
        {
            LayerRepository.Delete(ids);
        }

        /// <summary>
        ///  Delete layer by id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id)
        {
            LayerRepository.Delete(id);
        }
    }
}
