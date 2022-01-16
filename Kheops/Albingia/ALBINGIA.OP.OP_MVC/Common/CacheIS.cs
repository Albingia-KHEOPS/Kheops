using ALBINGIA.OP.OP_MVC.DynamicGuiIS.Common;
using OP.WSAS400.DTO.ParamIS;
using OPServiceContract.IS;
using System.Collections.Generic;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Common
{
    public static class CacheIS
    {

        #region Propriétées
        /// <summary>
        /// Tous les modèles Is existant dans la BD
        /// </summary>
        private static List<ModeleISDto> _allISEnteteModelesDto;
        public static List<ModeleISDto> AllISEnteteModelesDto
        {
            get
            {
                if (_allISEnteteModelesDto == null)
                    SetIsModelsEntete();
                return _allISEnteteModelesDto;
            }

        }
        /// <summary>
        /// Tous les modèles Is existant dans la BD
        /// </summary>
        private static List<ParamISLigneInfo> _allISModeles;
        public static List<ParamISLigneInfo> AllISModeles
        {
            get
            {
                if (_allISModeles == null || !_allISModeles.Any())
                    SetIsModels();
                return _allISModeles;
            }

        }
        /// <summary>
        /// Tous les modèles Is existant dans la BD
        /// </summary>
        private static List<ParamISLigneInfoDto> _allISModelesDto;
        public static List<ParamISLigneInfoDto> AllISModelesDto
        {
            get
            {
                if (_allISModelesDto == null)
                    SetIsModelsDto();
                return _allISModelesDto;
            }

        }
        #endregion
        #region Méthodes publiques
        public static void InitCacheIS()
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfoSpecif>())
            {
                var isContext=client.Channel;
                isContext.InitISCache();
            }
            _allISEnteteModelesDto = null;
            _allISModelesDto = null;
            _allISModeles = null;
        }
        public static void SetIsModelsEntete()
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfoSpecif>())
            {
                var isContext=client.Channel;
                if (_allISEnteteModelesDto == null)
                    _allISEnteteModelesDto = new List<ModeleISDto>();
                else
                    _allISEnteteModelesDto.Clear();
                _allISEnteteModelesDto = isContext.GetParamEntetModIs(string.Empty);
            }
        }
        public static bool SetIsModelsDto()
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfoSpecif>())
            {
                var isContext=client.Channel;
                bool retVal = false;
                if (_allISModelesDto == null || !_allISModelesDto.Any())
                {
                    _allISModelesDto = new List<ParamISLigneInfoDto>();
                    _allISModelesDto = isContext.GetParamISLignesInfo(string.Empty);
                    retVal = true;
                }

                return retVal;
            }
        }
        public static void SetIsModels()
        {
            if (_allISModeles == null)
            {
                _allISModeles = new List<ParamISLigneInfo>();
            }
            SetIsModelsDto();
            if (_allISModeles.Any())
            {
                _allISModeles.Clear();
            }
            _allISModelesDto.ForEach(elm => _allISModeles.Add((ParamISLigneInfo)elm));
        }
        #endregion
    }
}