using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.ParamIS
{
    [DataContract]
  public class AffichageISLineDto
    {
        [DataMember]
        public int IdLigne  { get; set; }
        [DataMember]
        public string Libelle { get; set; }
        [DataMember]
        public string Afficher { get; set; }

   
    }
}
