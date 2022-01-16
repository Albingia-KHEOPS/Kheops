using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.AttentatGareat
{
    [DataContract]
    public class AttentatGareatDto : _AttentatGareat_Base
    {
        [DataMember]
        public string LCI { get; set; }
        [DataMember]
        public string Capitaux { get; set; }
        [DataMember]
        public string Surface { get; set; }
        [DataMember]
        public string CATNAT { get; set; }
        [DataMember]
        public string CapitauxForces { get; set; }
        [DataMember]
        public string LibelleConstruit { get; set; }
        [DataMember]
        public AttentatParametreDto ParamStandard { get; set; }
        [DataMember]
        public AttentatParametreDto ParamRetenus { get; set; }
        [DataMember]
        public string CommentForce { get; set; }
        [DataMember]
        public bool MontantForce { get; set; }

        public AttentatGareatDto()
        {
            this.ParamStandard = new AttentatParametreDto();
            this.ParamRetenus = new AttentatParametreDto();
        }
    }
}
