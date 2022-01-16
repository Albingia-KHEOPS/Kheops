using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.User
{
    [DataContract]
    public class UsersDto
    {
        [DataMember]
        public List<AlbUserDto> Users { get; set; }
    }


    [DataContract]
    public class AlbUserDto
    {
        [Column(Name="USERWIN")]
        [DataMember]
        public string UserWin { get; set; }
        [Column(Name = "USERNAME")]
        [DataMember]
        public string UserNom { get; set; }
        [Column(Name = "USERPNAME")]
        [DataMember]
        public string UserPnom { get; set; }
        [Column(Name = "USERLOGIN")]
        [DataMember]
        public string UserLogin { get; set; }
        [Column(Name = "USERMAIL")]
        [DataMember]
        public string UserMail { get; set; }
        [Column(Name = "USERGROUP")]
        [DataMember]
        public string UserGroup { get; set; }
        [Column(Name = "USERSGROUP")]
        [DataMember]
        public string UserSGroup { get; set; }
    }
}
