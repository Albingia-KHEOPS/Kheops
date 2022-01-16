using Hexavia.Business.Interfaces;
using Hexavia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Hexavia.Controllers
{
    /// <summary>
    /// LayerController : Layer management controller
    /// </summary>
    public class LayerController : BaseController
    {
        private readonly ILayerBusiness LayerBusiness;

        public LayerController(ILayerBusiness layerBusiness)
        {
            LayerBusiness = layerBusiness;

        }
        /// <summary>
        /// Get all layers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAll()
        {
            return LargeJson(LayerBusiness.GetAll());
        }
        /// <summary>
        /// Verify layer name exist or not
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exist(string name)
        {
            bool? result = null;
            if (!string.IsNullOrEmpty(name))
            {
                result = LayerBusiness.Exist(name);

            }
            return Json(result);
        }
        /// <summary>
        /// Save layer
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Save(Layer layer)
        {
            if (layer != null)
            {
                LayerBusiness.Insert(layer);
            }
            return Json(new EmptyResult());
        }
        /// <summary>
        /// Update layers
        /// </summary>
        /// <param name="layers"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Updates(List<Layer> layers)
        {
            if (layers?.Any() ?? false)
            {
                LayerBusiness.Update(layers);
            }
            return Json(new EmptyResult());
        }
        /// <summary>
        /// Update layer
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update(Layer layer)
        {
            if (layer!=null)
            {
                LayerBusiness.Update(layer);
            }
            return Json(new EmptyResult());
        }
        /// <summary>
        /// Delete layers
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Deletes(List<string> ids)
        {
            if (ids?.Any() ?? false)
            {
                LayerBusiness.Delete(ids);
            }
            return Json(new EmptyResult());
        }

        /// <summary>
        /// Delete layer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                LayerBusiness.Delete(id);
            }
            return Json(new EmptyResult());
        }
    }
}