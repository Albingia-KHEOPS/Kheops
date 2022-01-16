namespace ALBINGIA.Framework.Common {
    public interface IScopedTransaction {
        bool IsTransactionShared { get; set; }
    }
}
