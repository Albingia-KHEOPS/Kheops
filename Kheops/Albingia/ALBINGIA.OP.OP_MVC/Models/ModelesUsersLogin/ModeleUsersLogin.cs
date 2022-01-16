using EmitMapper;
using OP.WSAS400.DTO.User;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesUsersLogin
{
    public class ModeleUsersLogin
    {
        public List<UsersLogin> Users { get; set; }
    }

    public class UsersLogin
    {
        public string UserWin { get; set; }
        public string UserNom { get; set; }
        public string UserPnom { get; set; }
        public string UserLogin { get; set; }
        public string UserMail { get; set; }
        public string UserGroup { get; set; }
        public string UserSGroup { get; set; }

        public static explicit operator UsersLogin(AlbUserDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AlbUserDto, UsersLogin>().Map(modelDto);
        }
    }

}