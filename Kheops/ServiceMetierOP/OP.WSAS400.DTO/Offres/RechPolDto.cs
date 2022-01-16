namespace OP.WSAS400.DTO.Offres
{
    public abstract partial class RechPolDto {
        public string Liaison { get; set; } = "and";
        public string Champ { get; set; } = "";
        public string Operateur { get; set; } = "=";
        public object Expression { get; set; } = "";
    }
    public partial class GroupingRechPolDto : RechPolDto {
        public GroupingRechPolDto(string operateur) : base() {
            this.Operateur = operateur;
        }
    }

    public partial class RechPolDto<T> : RechPolDto {
        public new T Expression { get { return (T)base.Expression; } set { base.Expression = value; } }
    }
}
