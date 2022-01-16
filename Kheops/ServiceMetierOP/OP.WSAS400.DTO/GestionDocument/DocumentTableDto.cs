using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.GestionDocument
{
    public class DocumentTableDto
    {
        [Column(Name = "CODEDOCUMENT")]
        public int CodeDocument { get; set; }
        [Column(Name = "TYPE")]
        public string Type { get; set; }
        [Column(Name = "CODEOFFRE")]
        public string CodeOffre { get; set; }
        [Column(Name = "VERSION")]
        public int Version { get; set; }
        [Column(Name = "SINISTREAA")]
        public int SinistreAA { get; set; }
        [Column(Name = "SINISTRENUM")]
        public int SinistreNum { get; set; }
        [Column(Name = "SINISTRESBR")]
        public string SinistreSbr { get; set; }
        [Column(Name = "SERVICE")]
        public string Service { get; set; }
        [Column(Name = "ACTEGESTION")]
        public string ActeGestion { get; set; }
        [Column(Name = "ETAPE")]
        public string Etape { get; set; }
        [Column(Name = "CODEDOCPARAM")]
        public int CodeDocParam { get; set; }
        [Column(Name = "NUMORDRE")]
        public int NumOrdre { get; set; }
        [Column(Name = "TYPEDOC")]
        public string TypeDoc { get; set; }
        [Column(Name = "CODEDOC")]
        public int CodeDoc { get; set; }
        [Column(Name = "NUMVERSION")]
        public int NumVersion { get; set; }
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
        [Column(Name = "NATUREGENERATION")]
        public string NatureGeneration { get; set; }
        [Column(Name = "AJOUTE")]
        public string Ajoute { get; set; }
        [Column(Name = "TRANSFORME")]
        public string Transforme { get; set; }
        [Column(Name = "ACCOMPAGNANT")]
        public string Accompagnant { get; set; }
        [Column(Name = "ACTIONENCHAINEE")]
        public string ActionEnchainee { get; set; }
        [Column(Name = "CODETEXTE")]
        public int CodeTexte { get; set; }
        [Column(Name = "NOMDOC")]
        public string NomDoc { get; set; }
        [Column(Name = "CHEMINCOMPLET")]
        public string CheminComplet { get; set; }
        [Column(Name = "USERSITUATION")]
        public string UserSituation { get; set; }
        [Column(Name = "CODESITUATION")]
        public int CodeSituation { get; set; }
        [Column(Name = "DATESITUATION")]
        public int DateSituation { get; set; }
        [Column(Name = "HEURESITUATION")]
        public int HeureSituation { get; set; }
        [Column(Name = "USERCREATION")]
        public string UserCreation { get; set; }
        [Column(Name = "DATECREATION")]
        public int DateCreation { get; set; }
        [Column(Name = "HEURECREATION")]
        public int HeureCreation { get; set; }
        [Column(Name = "USERMODIFICATION")]
        public string UserModification { get; set; }
        [Column(Name = "DATEMODIFICATION")]
        public int DateModification { get; set; }
        [Column(Name = "HEUREMODIFICATION")]
        public int HeureModification { get; set; }
    }
}
