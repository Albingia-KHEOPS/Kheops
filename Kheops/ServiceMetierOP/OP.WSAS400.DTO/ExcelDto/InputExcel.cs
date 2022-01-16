using System;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.ExcelDto
{
  //[DataContract]
  public class InputExcel
  {
    //[DataMember]
    [Column(Name = "NUMEROOFFRE")]
    public string NumeroOffre { get; set; }
    //[DataMember]
    [Column(Name = "VERSION")]
    public int Version { get; set; }
    //[DataMember]
    [Column(Name = "CODEAPPORTEUR")]
    public int CodeApporteur { get; set; }
    //[DataMember]
    [Column(Name = "NOMCOURTIER")]
    public string NomCourtier { get; set; }
    //[DataMember]
    [Column(Name = "CODEPRENEUR")]
    public int CodePreneur { get; set; }
    //[DataMember]
    [Column(Name = "NOMPRENEUR")]
    public string NomPreneur { get; set; }
    //[DataMember]
    [Column(Name = "REGIMETAXE")]
    public string RegimeTaxe { get; set; }
    //[DataMember]
    [Column(Name = "SOUMISCATNAT")]
    public string SoumisCatNat { get; set; }
    //[DataMember]
    [Column(Name = "INDICEREFERENCE")]
    public string IndiceReference { get; set; }
    //[DataMember]
    [Column(Name = "NATURECONTRAT")]
    public string NatureContrat { get; set; }
    //[DataMember]
    [Column(Name = "PARTALBINGIA")]
    public Single PartAlbingia { get; set; }
    //[DataMember]
    [Column(Name = "SOUSCRIPTEUR")]
    public string Souscripteur { get; set; }
    //[DataMember]
    [Column(Name = "DEVISE")]
    public string Devise { get; set; }
    [Column(Name = "DateEffetContrat")]
    public string DateEffetContrat { get; set; }
    [Column(Name = "DateFinContrat")]
    public string DateFinContrat { get; set; }

    //[DataMember]
    public string CellsNumeroOffre { get; set; }
    //[DataMember]
    public string CellsVersion { get; set; }
    //[DataMember]
    public string CellsCodeApporteur { get; set; }
    //[DataMember]
    public string CellsNomCourtier { get; set; }
    //[DataMember]
    public string CellsCodePreneur { get; set; }
    //[DataMember]
    public string CellsNomPreneur { get; set; }
    //[DataMember]
    public string CellsRegimeTaxe { get; set; }
    //[DataMember]
    public string CellsSoumisCatNat { get; set; }
    //[DataMember]
    public string CellsIndiceReference { get; set; }
    //[DataMember]
    public string CellsNatureContrat { get; set; }
    //[DataMember]
    public string CellsPartAlbingia { get; set; }
    //[DataMember]
    public string CellsSouscripteur { get; set; }
    //[DataMember]
    public string CellsDevise { get; set; }
    public string CellsDateEffetContrat { get; set; }

    public string CellsDateFinContrat { get; set; }
  }
}
