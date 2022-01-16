using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class FormuleGarantiePlatDto : ICloneable
    {
        [Column(Name = "CODEOPTION")]
        public Int64 GuidOption { get; set; }                   //Id de l'option

        [Column(Name = "APPLIQUEA")]
        public Int64 AppliqueA { get; set; }                    //S'applique à

        [Column(Name = "GUIDV")]
        public Int64 GuidV { get; set; }                          //Id du volet
        [Column(Name = "CODEVOLET")]
        public string CodeVolet { get; set; }                   //Code du volet
        [Column(Name = "DESCRVOLET")]
        public string DescrVolet { get; set; }                  //Description du volet
        [Column(Name = "CARACVOLET")]
        public string CaracVolet { get; set; }                  //Caractère du volet
        [Column(Name = "GUIDVOLET")]
        public Int64 GuidVolet { get; set; }                      //Guid du volet
        [Column(Name = "CHECKV")]
        public Int64 isCheckedV { get; set; }                    //Volet checké ?
        [Column(Name="VOLETCOLLAPSE")]
        public string VoletCollapse { get; set; }               //Volet replié
        public decimal VoletOrdre { get; set; }

        [Column(Name = "GUIDB")]
        public Int64 GuidB { get; set; }                          //Id du bloc
        [Column(Name = "CODEBLOC")]
        public string CodeBloc { get; set; }                    //Code du bloc
        [Column(Name = "DESCRBLOC")]
        public string DescrBloc { get; set; }                   //Description du bloc
        [Column(Name = "CARACBLOC")]
        public string CaracBloc { get; set; }                   //Caractère du bloc
        [Column(Name = "GUIDBLOC")]
        public Int64 GuidBloc { get; set; }                       //Guid du bloc
        [Column(Name = "CHECKB")]
        public Int64 isCheckedB { get; set; }                    //Bloc checké ?
        [Column(Name = "CODEBLOCINCOMP")]
        public Int64 CodeBlocIncomp { get; set; }               //Code Bloc incompatible
        [Column(Name = "BLOCINCOMP")]
        public string BlocIncomp { get; set; }                  //Bloc incompatible
        [Column(Name = "CODEBLOCASSOC")]
        public Int64 CodeBlocAssoc { get; set; }               //Code Bloc asscoié
        [Column(Name = "BLOCASSOC")]
        public string BlocAssoc { get; set; }                  //Bloc asscoié
        public decimal BlocOrdre { get; set; }

        [Column(Name = "GUIDM")]
        public Int64 GuidM { get; set; }                          //Id du modèle
        [Column(Name = "CODEMODELE")]
        public string CodeModele { get; set; }                  //Code du modèle
        [Column(Name = "DESCRMODELE")]
        public string DescrModele { get; set; }                 //Description du modèle

        [Column(Name = "GUIDNIV1")]
        public Int64 GuidNiv1 { get; set; }                     //Id du niveau 1
        [Column(Name = "ACTIONNIV1")]
        public string ActionNiv1 { get; set; }                 //Action du niveau 1
        [Column(Name = "CODENIV1")]
        public Int64 CodeNiv1 { get; set; }                       //Code du niveau 1
        [Column(Name = "CODEGARNIV1")]
        public string CodeGarNiv1 { get; set; }                 //Code garantie du niveau 1
        [Column(Name = "DESCRNIV1")]
        public string DescrNiv1 { get; set; }                   //Dscription du niveau 1
        [Column(Name = "CARACNIV1")]
        public string CaracNiv1 { get; set; }                   //Caractère du niveau 1
        [Column(Name = "NATURENIV1")]
        public string NatureNiv1 { get; set; }                  //Nature du niveau 1
        [Column(Name = "NATUREPARAMNIV1")]
        public string NatureParamNiv1 { get; set; }                  //Nature param du niveau 1
        [Column(Name = "CODEPARENTNIV1")]
        public Int64 CodeParentNiv1 { get; set; }                 //Code parent du niveau 1
        [Column(Name = "CODENIV1NIV1")]
        public Int64 CodeNiv1Niv1 { get; set; }                   //Code niveau 1 du niveau 1
        [Column(Name = "FLAGMODIFNIV1")]
        public string FlagModifNiv1 { get; set; }               //Flag modif du niveau 1
        [Column(Name="PARAMNATMODNIV1")]
        public string ParamNatModNiv1 { get; set; }                 //Param Nature Modifiée niveau 1
        [Column(Name="CODEGARINCOMPNIV1")]
        public Int64 CodeGarIncompNiv1 { get; set; }            //Code garantie incompatible
        [Column(Name="CODEGARASSOCNIV1")]
        public Int64 CodeGarAssocNiv1 { get; set; }             //Code garantie associée
        [Column(Name="CODEGARAALTNIV1")]
        public Int64 CodeGarAltNiv1 { get; set; }               //Code groupe alternatif
        [Column(Name = "INVENPOSSIBLE1")]                       //Inventaire possible sur la garantie du nivau 1? "N" ou "O" 
        public string InvenPossible1 { get; set; }
        [Column(Name = "CODEINVEN1")]                       //Code Inventaire de la garantie du niveau 1. 0 si aucun inventaire n'est défini sur la garantie
        public Int64 CodeInven1 { get; set; }
        [Column(Name = "TYPEINVEN1")]                       //Type Inventaire de la garantie du niveau 1
        public string TypeInven1 { get; set; }
        [Column(Name="CREATEAVN1")]                          //Code Avt de création
        public Int64 CreateAvt1 { get; set; }
        [Column(Name="MAJAVN1")]                             //Code Avt de modification
        public Int64 MajAvt1 { get; set; }
        [Column(Name = "DATDEB1")]                            //Date de début de garantie
        public string DateDeb1 { get; set; }
        [Column(Name = "HEUDEB1")]                            //Heure de début de garantie
        public Int64 HeureDeb1 { get; set; }
        [Column(Name = "DATFIN1")]                            //Date de fin de garantie
        public Int64 DateFin1 { get; set; }
        [Column(Name = "HEUFIN1")]                            //Heure de fin de garantie
        public Int64 HeureFin1 { get; set; }
        [Column(Name = "DUREE1")]                            //Durée de garantie
        public Int64 Duree1 { get; set; }
        [Column(Name = "DURUNI1")]                            //Unité de durée de garantie
        public string DureeUnite1 { get; set; }
        [Column(Name="ALIMNIV1")]                               //Alimentation de l'assiette/prime
        public string AlimNiv1 { get; set; }
        public string TriNiv1 { get; set; }


        [Column(Name = "GUIDNIV2")]
        public Int64 GuidNiv2 { get; set; }                     //Id du niveau 2
        [Column(Name = "CODENIV2")]
        public Int64 CodeNiv2 { get; set; }                       //Code du niveau 2
        [Column(Name = "CODEGARNIV2")]
        public string CodeGarNiv2 { get; set; }                 //Code garantie du niveau 2
        [Column(Name = "DESCRNIV2")]
        public string DescrNiv2 { get; set; }                   //Dscription du niveau 2
        [Column(Name = "CARACNIV2")]
        public string CaracNiv2 { get; set; }                   //Caractère du niveau 2
        [Column(Name = "NATURENIV2")]
        public string NatureNiv2 { get; set; }                  //Nature du niveau 2
        [Column(Name = "NATUREPARAMNIV2")]
        public string NatureParamNiv2 { get; set; }                  //Nature param du niveau 2
        [Column(Name = "CODEPARENTNIV2")]
        public Int64 CodeParentNiv2 { get; set; }                 //Code parent du niveau 2
        [Column(Name = "CODENIV1NIV2")]
        public Int64 CodeNiv1Niv2 { get; set; }                   //Code niveau 1 du niveau 2
        [Column(Name="FLAGMODIFNIV2")]
        public string FlagModifNiv2 { get; set; }               //Flag modif du niveau 2
        [Column(Name = "PARAMNATMODNIV2")]
        public string ParamNatModNiv2 { get; set; }                 //Param Nature Modifiée niveau 2
        [Column(Name = "CODEGARINCOMPNIV2")]
        public Int64 CodeGarIncompNiv2 { get; set; }            //Code garantie incompatible
        [Column(Name = "CODEGARASSOCNIV2")]
        public Int64 CodeGarAssocNiv2 { get; set; }             //Code garantie associée
        [Column(Name = "CODEGARAALTNIV2")]
        public Int64 CodeGarAltNiv2 { get; set; }               //Code groupe alternatif
        [Column(Name = "INVENPOSSIBLE2")]                       //Inventaire possible sur la garantie du niveau2? "N" ou "O" 
        public string InvenPossible2 { get; set; }
        [Column(Name = "CODEINVEN2")]                       //Code Inventaire de la garantie du niveau 2. 0 si aucun inventaire n'est défini sur la garantie
        public Int64 CodeInven2 { get; set; }
        [Column(Name = "TYPEINVEN2")]                       //Type Inventaire de la garantie du niveau 2
        public string TypeInven2 { get; set; }
        [Column(Name = "CREATEAVN2")]                          //Code Avt de création
        public Int64 CreateAvt2 { get; set; }
        [Column(Name = "MAJAVN2")]                             //Code Avt de modification
        public Int64 MajAvt2 { get; set; }
        [Column(Name = "DATDEB2")]                            //Date de début de garantie
        public string DateDeb2 { get; set; }
        [Column(Name = "HEUDEB2")]                            //Heure de début de garantie
        public Int64 HeureDeb2 { get; set; }
        [Column(Name = "DATFIN2")]                            //Date de fin de garantie
        public Int64 DateFin2 { get; set; }
        [Column(Name = "HEUFIN2")]                            //Heure de fin de garantie
        public Int64 HeureFin2 { get; set; }
        [Column(Name = "DUREE2")]                            //Durée de garantie
        public Int64 Duree2 { get; set; }
        [Column(Name = "DURUNI2")]                            //Unité de durée de garantie
        public string DureeUnite2 { get; set; }
        public string TriNiv2 { get; set; }


        [Column(Name = "GUIDNIV3")]
        public Int64 GuidNiv3 { get; set; }                     //Id du niveau 3
        [Column(Name = "CODENIV3")]
        public Int64 CodeNiv3 { get; set; }                       //Code du niveau 3
        [Column(Name = "CODEGARNIV3")]
        public string CodeGarNiv3 { get; set; }                 //Code garantie du niveau 3
        [Column(Name = "DESCRNIV3")]
        public string DescrNiv3 { get; set; }                   //Dscription du niveau 3
        [Column(Name = "CARACNIV3")]
        public string CaracNiv3 { get; set; }                   //Caractère du niveau 3
        [Column(Name = "NATURENIV3")]
        public string NatureNiv3 { get; set; }                  //Nature du niveau 3
        [Column(Name = "NATUREPARAMNIV3")]
        public string NatureParamNiv3 { get; set; }                  //Nature param du niveau 3
        [Column(Name = "CODEPARENTNIV3")]
        public Int64 CodeParentNiv3 { get; set; }                 //Code parent du niveau 3
        [Column(Name = "CODENIV1NIV3")]
        public Int64 CodeNiv1Niv3 { get; set; }                   //Code niveau 1 du niveau 3
        [Column(Name = "FLAGMODIFNIV3")]
        public string FlagModifNiv3 { get; set; }               //Flag modif du niveau 3
        [Column(Name = "PARAMNATMODNIV3")]
        public string ParamNatModNiv3 { get; set; }                 //Param Nature Modifiée niveau 3
        [Column(Name = "CODEGARINCOMPNIV3")]
        public Int64 CodeGarIncompNiv3 { get; set; }            //Code garantie incompatible
        [Column(Name = "CODEGARASSOCNIV3")]
        public Int64 CodeGarAssocNiv3 { get; set; }             //Code garantie associée
        [Column(Name = "CODEGARAALTNIV3")]
        public Int64 CodeGarAltNiv3 { get; set; }               //Code groupe alternatif
        [Column(Name = "INVENPOSSIBLE3")]                       //Inventaire possible sur la garantie du niveau 3? "N" ou "O" 
        public string InvenPossible3 { get; set; }
        [Column(Name = "CODEINVEN3")]                       //Code Inventaire de la garantie du niveau 3. 0 si aucun inventaire n'est défini sur la garantie
        public Int64 CodeInven3 { get; set; }
        [Column(Name = "TYPEINVEN3")]                       //Type Inventaire de la garantie du nivau 3
        public string TypeInven3 { get; set; }
        [Column(Name = "CREATEAVN3")]                          //Code Avt de création
        public Int64 CreateAvt3 { get; set; }
        [Column(Name = "MAJAVN3")]                             //Code Avt de modification
        public Int64 MajAvt3 { get; set; }
        [Column(Name = "DATDEB3")]                            //Date de début de garantie
        public string DateDeb3 { get; set; }
        [Column(Name = "HEUDEB3")]                            //Heure de début de garantie
        public Int64 HeureDeb3 { get; set; }
        [Column(Name = "DATFIN3")]                            //Date de fin de garantie
        public Int64 DateFin3 { get; set; }
        [Column(Name = "HEUFIN3")]                            //Heure de fin de garantie
        public Int64 HeureFin3 { get; set; }
        [Column(Name = "DUREE3")]                            //Durée de garantie
        public Int64 Duree3 { get; set; }
        [Column(Name = "DURUNI3")]                            //Unité de durée de garantie
        public string DureeUnite3 { get; set; }
        public string TriNiv3 { get; set; }


        [Column(Name = "GUIDNIV4")]
        public Int64 GuidNiv4 { get; set; }                     //Id du niveau 4
        [Column(Name = "CODENIV4")]
        public Int64 CodeNiv4 { get; set; }                       //Code du niveau 4
        [Column(Name = "CODEGARNIV4")]
        public string CodeGarNiv4 { get; set; }                 //Code garantie du niveau 4
        [Column(Name = "DESCRNIV4")]
        public string DescrNiv4 { get; set; }                   //Dscription du niveau 4
        [Column(Name = "CARACNIV4")]
        public string CaracNiv4 { get; set; }                   //Caractère du niveau 4
        [Column(Name = "NATURENIV4")]
        public string NatureNiv4 { get; set; }                  //Nature du niveau 4
        [Column(Name = "NATUREPARAMNIV4")]
        public string NatureParamNiv4 { get; set; }                  //Nature param du niveau 4
        [Column(Name = "CODEPARENTNIV4")]
        public Int64 CodeParentNiv4 { get; set; }                 //Code parent du niveau 4
        [Column(Name = "CODENIV1NIV4")]
        public Int64 CodeNiv1Niv4 { get; set; }                   //Code niveau 1 du niveau 4
        [Column(Name = "FLAGMODIFNIV4")]
        public string FlagModifNiv4 { get; set; }               //Flag modif du niveau 4
        [Column(Name = "PARAMNATMODNIV4")]
        public string ParamNatModNiv4 { get; set; }                 //Param Nature Modifiée niveau 4
        [Column(Name = "CODEGARINCOMPNIV4")]
        public Int64 CodeGarIncompNiv4 { get; set; }            //Code garantie incompatible
        [Column(Name = "CODEGARASSOCNIV4")]
        public Int64 CodeGarAssocNiv4 { get; set; }             //Code garantie associée
        [Column(Name = "CODEGARAALTNIV4")]
        public Int64 CodeGarAltNiv4 { get; set; }               //Code groupe alternatif
        [Column(Name = "INVENPOSSIBLE4")]                       //Inventaire possible sur la garantie du niveau 4? "N" ou "O" 
        public string InvenPossible4 { get; set; }
        [Column(Name = "CODEINVEN4")]                       //Code Inventaire de la garantie du niveau 4. 0 si aucun inventaire n'est défini sur la garantie
        public Int64 CodeInven4 { get; set; }
        [Column(Name = "TYPEINVEN4")]                       //Type Inventaire de la garantie du niveau 4
        public string TypeInven4 { get; set; }
        [Column(Name = "CREATEAVN4")]                          //Code Avt de création
        public Int64 CreateAvt4 { get; set; }
        [Column(Name = "MAJAVN4")]                             //Code Avt de modification
        public Int64 MajAvt4 { get; set; }
        [Column(Name = "DATDEB4")]                            //Date de début de garantie
        public string DateDeb4 { get; set; }
        [Column(Name = "HEUDEB4")]                            //Heure de début de garantie
        public Int64 HeureDeb4 { get; set; }
        [Column(Name = "DATFIN4")]                            //Date de fin de garantie
        public Int64 DateFin4 { get; set; }
        [Column(Name = "HEUFIN4")]                            //Heure de fin de garantie
        public Int64 HeureFin4 { get; set; }
        [Column(Name = "DUREE4")]                            //Durée de garantie
        public Int64 Duree4 { get; set; }
        [Column(Name = "DURUNI4")]                            //Unité de durée de garantie
        public string DureeUnite4 { get; set; }
        public string TriNiv4 { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
