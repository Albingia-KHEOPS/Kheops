using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.GestionDocument
{
    public class GestionDocumentTableWDto
    {
        [Column(Name="CODEDOCUMENT")]
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

        [Column(Name = "CODEDOCJOINTS")]
        public int CodeDocJoints { get; set; }
        [Column(Name = "DOCJOINTSCODEDOCUMENT")]
        public int DocJointsCodeDocument { get; set; }
        [Column(Name = "DOCJOINTSORDRE")]
        public int DocJointsOrdre { get; set; }
        [Column(Name = "DOCJOINTSCHEMINCOMPLET")]
        public string DocJointsCheminComplet { get; set; }

        [Column(Name = "CODETEXTEDOC")]
        public int CodeTexteDoc { get; set; }
        [Column(Name = "TEXTEDOC")]
        public string TexteDoc { get; set; }

        [Column(Name = "CODEDIFFUSION")]
        public int CodeDiffusion { get; set; }
        [Column(Name = "DIFFUSIONCODEDOC")]
        public int DiffusionCodeDoc { get; set; }
        [Column(Name = "DIFFUSIONORDRE")]
        public int DiffusionOrdre { get; set; }
        [Column(Name = "DIFFUSIONTYPEENVOI")]
        public string DiffusionTypeEnvoi { get; set; }
        [Column(Name = "DIFFUSIONTYPEDESTINATAIRE")]
        public string DiffusionTypeDestinataire { get; set; }
        [Column(Name = "DIFFUSIONTYPEINTERVENANT")]
        public string DiffusionTypeIntervenant { get; set; }
        [Column(Name = "DIFFUSIONCODEDESTINATAIRE")]
        public int DiffusionCodeDestinataire { get; set; }
        [Column(Name = "DIFFUSIONCODEINTERLOCUTEUR")]
        public int DiffusionCodeInterlocuteur { get; set; }
        [Column(Name = "DIFFUSIONNATURE")]
        public string DiffusionNature { get; set; }
        [Column(Name = "DIFFUSIONNBEXEMPLAIRE")]
        public int DiffusionNbExemplaire { get; set; }
        [Column(Name = "DIFFUSIONCODEACCOMPAGNANT")]
        public int DiffusionCodeAccompagnant { get; set; }
        [Column(Name = "TYPEDIFFUSION")]
        public string TypeDiffusion { get; set; }
        [Column(Name = "DIFFUSIONOBJET")]
        public string DiffusionObjet { get; set; }
        [Column(Name = "DIFFUSIONADRESSE")]
        public string DiffusionAdresse { get; set; }
        [Column(Name = "DIFFUSIONCODETEXTE")]
        public int DiffusionCodeTexte { get; set; }
    }
}
