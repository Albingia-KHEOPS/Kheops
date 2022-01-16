using ALBINGIA.OP.OP_MVC.Models.ModelesUsersLogin;
using OP.WSAS400.DTO.User;
using OPServiceContract.ICommon;
using System.Collections.Generic;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Common
{
    public static class CacheUsersLogin
    {
        private static List<UsersLogin> _allUsersLogin;
        public static List<UsersLogin> AllUsersLogin
        {
            get
            {
                if (_allUsersLogin == null)
                    SetUserLogin();
                return _allUsersLogin;
            }

        }

        private static UsersDto _allUsersLoginDto;
        public static UsersDto AllUsersLoginDto
        {
            get
            {
                if (_allUsersLoginDto == null)
                    SetUsersLoginDto();
                return _allUsersLoginDto;
            }
        }


        public static void InitCacheUsersLogin()
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var serviceContext=client.Channel;
                serviceContext.GetUsersLogin();
            }
            _allUsersLoginDto = null;
            _allUsersLogin = null;
        }

        public static bool SetUsersLoginDto()
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var serviceContext=client.Channel;
                bool retVal = false;
                if (_allUsersLoginDto == null || _allUsersLoginDto.Users==null)
                {
                    _allUsersLoginDto = serviceContext.GetUsersLogin();
                    retVal = true;
                }

                return retVal;
            }
        }
        private static object _allUsersLoginSyncRoot = new object();
        public static void SetUserLogin()
        {
            SetUsersLoginDto();
            lock (_allUsersLoginSyncRoot)
            {
                _allUsersLogin = _allUsersLoginDto.Users.Select( x => (UsersLogin)x ).ToList();
            }
        }
    }
}