
using System;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;
namespace OP.WSAS400.DTO.ExcelDto.tst {
//Class Dto générée 
//[KnownType(typeof(RS))]
[DataContract]
public class RS 
{
		
	[DataMember]
    [Column(Name = "KGNR1DES")]
    public string LibelleRisque  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO11DS")]
    public string Objet1  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO11VL")]
    public Int64 ValeurObjet1  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO11VU")]
    public string UniteValeurObjet1  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO11VT")]
    public string TypeValeurObjet1  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO12DS")]
    public string Objet2  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO12VL")]
    public Int64 ValeurObjet2  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO12VU")]
    public string UniteValeurObjet2  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO12VT")]
    public string TypeValeurObjet2  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO13DS")]
    public string Objet3  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO13VL")]
    public Int64 ValeurObjet3  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO13VU")]
    public string UniteValeurObjet3  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO13VT")]
    public string TypeValeurObjet3  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO14DS")]
    public string Objet4  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO14VL")]
    public Int64 ValeurObjet4  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO14VU")]
    public string UniteValeurObjet4  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO14VT")]
    public string TypeValeurObjet4  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO15DS")]
    public string Objet5  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO15VL")]
    public Int64 ValeurObjet5  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO15VU")]
    public string UniteValeurObjet5  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNO15VT")]
    public string TypeValeurObjet5  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNF1DES")]
    public string LibelleFormuleA  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNF1AO1")]
    public string AppliqueObj1  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNF1AO2")]
    public string AppliqueObj2  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNF1AO3")]
    public string AppliqueObj3  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNF1AO4")]
    public string AppliqueObj4  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNF1AO5")]
    public string AppliqueObj5  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11OR")]
    public string GarantieAnnorgAcquise  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11NO")]
    public string GarantieAnnotsAcquise  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11VL")]
    public Int64 ValeurAssietteAnnul  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11VU")]
    public string KGNG11VU  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11VT")]
    public string TypeAssietteAnnul  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11FV")]
    public Int64 ValeurFranchiseAnnul  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11FU")]
    public string UniteFranchiseAnnul  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11FT")]
    public string TypeFranchiseAnnul  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11PV")]
    public decimal ValeurPrmeAnnul  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11PU")]
    public string UnitePrimeAnnul  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11PM")]
    public decimal MinimumPrimeAnnul  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11E1")]
    public string ExcluObj1Annul  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11E2")]
    public string ExcluObj2Annul  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11E3")]
    public string ExcluObj3Annul  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11E4")]
    public string ExcluObj4Annul  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG11E5")]
    public string ExcluObj5Annul  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG12IN")]
    public string GarantieIndispoAcquise  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG12VL")]
    public Int64 ValeurAssietteIndispo  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG12VU")]
    public string UniteAssietteIndispo  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG12VT")]
    public string TypeAssietteIndispo  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG12FV")]
    public Int64 ValurFranchiseIndispo  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG12FU")]
    public string UniteFranchiseIndispo  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG12FT")]
    public string TypeFranchiseIndispo  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG12PV")]
    public decimal ValeurPrimeIndispo  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG12PU")]
    public string UnitePrimeIndispo  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG12PM")]
    public decimal MinimumPrimeIndispo  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG12E1")]
    public string ExcluObj1PrimeIndispo  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG12E2")]
    public string ExcluObj2PrimeIndispo  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG12E3")]
    public string ExcluObj3PrimeIndispo  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG12E4")]
    public string ExcluObj4PrimeIndispo  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG12E5")]
    public string ExcluObj5PrimeIndispo  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG13IM")]
    public string GarantieIntemperieAcquise  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG13VL")]
    public Int64 ValeurAssietteIntemperie  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG13VU")]
    public string UniteAsietteIntemperie  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG13VT")]
    public string TypeAssietteIntemperie  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG13FV")]
    public Int64 ValeurFranchiseIntemperie  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG13FU")]
    public string UniteFranchiseIntemperie  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG13FT")]
    public string TypeFranchiseIntemperie  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG13PV")]
    public decimal ValeurPrimeItemperie  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG13PU")]
    public string UnitePrimeIntemperie  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG13PM")]
    public decimal MinimumPrimeItemperie  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG13E1")]
    public string ExcluObj1Itemperie  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG13E2")]
    public string ExcluObj2Itemperie  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG13E3")]
    public string ExcluObj3Itemperie  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG13E4")]
    public string ExcluObj4Itemperie  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG13E5")]
    public string ExcluObj5Itemperie  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG14AT")]
    public string GarantieAttentatAcquise  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG14VL")]
    public Int64 ValeurAssietteAttentat  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG14VU")]
    public string UniteAssietteAttentat  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG14VT")]
    public string TypeAssietteAttentat  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG14FV")]
    public Int64 ValeurFranchiseAttentat  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG14FU")]
    public string UniteFranchiseAttentat  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG14FT")]
    public string TypeFranchiseAttentat  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG14PV")]
    public decimal ValeurPrimeAttentat  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG14PU")]
    public string UnitePrimeAttentat  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG14PM")]
    public decimal MinimumPrimeAttentat  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG14E1")]
    public string ExcluObj1Attentat  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG14E2")]
    public string ExcluObj2Attentat  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG14E3")]
    public string ExcluObj3Attentat  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG14E4")]
    public string ExcluObj4Attentat  {get;set;}
	
	
		
	[DataMember]
    [Column(Name = "KGNG14E5")]
    public string ExcluObj5Attentat  {get;set;}
	
	
	[DataMember]
	public string CellsLibelleRisque {get;set;}
	
	[DataMember]
	public string CellsObjet1 {get;set;}
	
	[DataMember]
	public string CellsValeurObjet1 {get;set;}
	
	[DataMember]
	public string CellsUniteValeurObjet1 {get;set;}
	
	[DataMember]
	public string CellsTypeValeurObjet1 {get;set;}
	
	[DataMember]
	public string CellsObjet2 {get;set;}
	
	[DataMember]
	public string CellsValeurObjet2 {get;set;}
	
	[DataMember]
	public string CellsUniteValeurObjet2 {get;set;}
	
	[DataMember]
	public string CellsTypeValeurObjet2 {get;set;}
	
	[DataMember]
	public string CellsObjet3 {get;set;}
	
	[DataMember]
	public string CellsValeurObjet3 {get;set;}
	
	[DataMember]
	public string CellsUniteValeurObjet3 {get;set;}
	
	[DataMember]
	public string CellsTypeValeurObjet3 {get;set;}
	
	[DataMember]
	public string CellsObjet4 {get;set;}
	
	[DataMember]
	public string CellsValeurObjet4 {get;set;}
	
	[DataMember]
	public string CellsUniteValeurObjet4 {get;set;}
	
	[DataMember]
	public string CellsTypeValeurObjet4 {get;set;}
	
	[DataMember]
	public string CellsObjet5 {get;set;}
	
	[DataMember]
	public string CellsValeurObjet5 {get;set;}
	
	[DataMember]
	public string CellsUniteValeurObjet5 {get;set;}
	
	[DataMember]
	public string CellsTypeValeurObjet5 {get;set;}
	
	[DataMember]
	public string CellsLibelleFormuleA {get;set;}
	
	[DataMember]
	public string CellsAppliqueObj1 {get;set;}
	
	[DataMember]
	public string CellsAppliqueObj2 {get;set;}
	
	[DataMember]
	public string CellsAppliqueObj3 {get;set;}
	
	[DataMember]
	public string CellsAppliqueObj4 {get;set;}
	
	[DataMember]
	public string CellsAppliqueObj5 {get;set;}
	
	[DataMember]
	public string CellsGarantieAnnorgAcquise {get;set;}
	
	[DataMember]
	public string CellsGarantieAnnotsAcquise {get;set;}
	
	[DataMember]
	public string CellsValeurAssietteAnnul {get;set;}
	
	[DataMember]
	public string CellsKGNG11VU {get;set;}
	
	[DataMember]
	public string CellsTypeAssietteAnnul {get;set;}
	
	[DataMember]
	public string CellsValeurFranchiseAnnul {get;set;}
	
	[DataMember]
	public string CellsUniteFranchiseAnnul {get;set;}
	
	[DataMember]
	public string CellsTypeFranchiseAnnul {get;set;}
	
	[DataMember]
	public string CellsValeurPrmeAnnul {get;set;}
	
	[DataMember]
	public string CellsUnitePrimeAnnul {get;set;}
	
	[DataMember]
	public string CellsMinimumPrimeAnnul {get;set;}
	
	[DataMember]
	public string CellsExcluObj1Annul {get;set;}
	
	[DataMember]
	public string CellsExcluObj2Annul {get;set;}
	
	[DataMember]
	public string CellsExcluObj3Annul {get;set;}
	
	[DataMember]
	public string CellsExcluObj4Annul {get;set;}
	
	[DataMember]
	public string CellsExcluObj5Annul {get;set;}
	
	[DataMember]
	public string CellsGarantieIndispoAcquise {get;set;}
	
	[DataMember]
	public string CellsValeurAssietteIndispo {get;set;}
	
	[DataMember]
	public string CellsUniteAssietteIndispo {get;set;}
	
	[DataMember]
	public string CellsTypeAssietteIndispo {get;set;}
	
	[DataMember]
	public string CellsValurFranchiseIndispo {get;set;}
	
	[DataMember]
	public string CellsUniteFranchiseIndispo {get;set;}
	
	[DataMember]
	public string CellsTypeFranchiseIndispo {get;set;}
	
	[DataMember]
	public string CellsValeurPrimeIndispo {get;set;}
	
	[DataMember]
	public string CellsUnitePrimeIndispo {get;set;}
	
	[DataMember]
	public string CellsMinimumPrimeIndispo {get;set;}
	
	[DataMember]
	public string CellsExcluObj1PrimeIndispo {get;set;}
	
	[DataMember]
	public string CellsExcluObj2PrimeIndispo {get;set;}
	
	[DataMember]
	public string CellsExcluObj3PrimeIndispo {get;set;}
	
	[DataMember]
	public string CellsExcluObj4PrimeIndispo {get;set;}
	
	[DataMember]
	public string CellsExcluObj5PrimeIndispo {get;set;}
	
	[DataMember]
	public string CellsGarantieIntemperieAcquise {get;set;}
	
	[DataMember]
	public string CellsValeurAssietteIntemperie {get;set;}
	
	[DataMember]
	public string CellsUniteAsietteIntemperie {get;set;}
	
	[DataMember]
	public string CellsTypeAssietteIntemperie {get;set;}
	
	[DataMember]
	public string CellsValeurFranchiseIntemperie {get;set;}
	
	[DataMember]
	public string CellsUniteFranchiseIntemperie {get;set;}
	
	[DataMember]
	public string CellsTypeFranchiseIntemperie {get;set;}
	
	[DataMember]
	public string CellsValeurPrimeItemperie {get;set;}
	
	[DataMember]
	public string CellsUnitePrimeIntemperie {get;set;}
	
	[DataMember]
	public string CellsMinimumPrimeItemperie {get;set;}
	
	[DataMember]
	public string CellsExcluObj1Itemperie {get;set;}
	
	[DataMember]
	public string CellsExcluObj2Itemperie {get;set;}
	
	[DataMember]
	public string CellsExcluObj3Itemperie {get;set;}
	
	[DataMember]
	public string CellsExcluObj4Itemperie {get;set;}
	
	[DataMember]
	public string CellsExcluObj5Itemperie {get;set;}
	
	[DataMember]
	public string CellsGarantieAttentatAcquise {get;set;}
	
	[DataMember]
	public string CellsValeurAssietteAttentat {get;set;}
	
	[DataMember]
	public string CellsUniteAssietteAttentat {get;set;}
	
	[DataMember]
	public string CellsTypeAssietteAttentat {get;set;}
	
	[DataMember]
	public string CellsValeurFranchiseAttentat {get;set;}
	
	[DataMember]
	public string CellsUniteFranchiseAttentat {get;set;}
	
	[DataMember]
	public string CellsTypeFranchiseAttentat {get;set;}
	
	[DataMember]
	public string CellsValeurPrimeAttentat {get;set;}
	
	[DataMember]
	public string CellsUnitePrimeAttentat {get;set;}
	
	[DataMember]
	public string CellsMinimumPrimeAttentat {get;set;}
	
	[DataMember]
	public string CellsExcluObj1Attentat {get;set;}
	
	[DataMember]
	public string CellsExcluObj2Attentat {get;set;}
	
	[DataMember]
	public string CellsExcluObj3Attentat {get;set;}
	
	[DataMember]
	public string CellsExcluObj4Attentat {get;set;}
	
	[DataMember]
	public string CellsExcluObj5Attentat {get;set;}
	
}}