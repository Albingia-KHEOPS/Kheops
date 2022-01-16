using Albingia.Kheops.OP.Domain;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.OP.OP_MVC.CustomResult;
using OPServiceContract.ICommon;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class UserController : BaseController {
        static IEnumerable<PropertyInfo> ProfileProps = typeof(ProfileKheops).GetProperties();

        [HttpGet]
        [ErrorHandler]
        public JsonResult GetProfile() {
            using (var client = ServiceClientFactory.GetClient<ICommonOffre>()) {
                return JsonNetResult.NewResultToGet(client.Channel.GetProfileKheops());
            }
        }

        [HttpPost]
        [ErrorHandler]
        public void UpdateProfileKheops(ProfileKheops profile) {
            using (var client = ServiceClientFactory.GetClient<ICommonOffre>()) {
                client.Channel.SetProfileKheops(profile);
            }
        }

        [HttpPost]
        [ErrorHandler]
        public bool UpdateProfileKheopsFlag(ProfileKheopsData profileData, bool value) {
            if (profileData == ProfileKheopsData.None) {
                return false;
            }
            var prop = ProfileProps.FirstOrDefault(p => p.Name == profileData.ToString() && p.PropertyType == typeof(bool));
            if (prop is null) {
                return false;
            }

            var profile = new ProfileKheops();
            prop.SetValue(profile, value);
            using (var client = ServiceClientFactory.GetClient<ICommonOffre>()) {
                client.Channel.SetProfileKheops(profile, new[] { profileData });
            }
            return true;
        }
    }
}