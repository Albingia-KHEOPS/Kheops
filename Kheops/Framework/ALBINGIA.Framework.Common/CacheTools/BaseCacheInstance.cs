namespace ALBINGIA.Framework.Common.CacheTools
{
    public class BaseCacheInstance
    {
        // Types imbriquées
        internal static class CacheGlobalInit<T> where T : new()
        {
            // Properties
            public static T CacheInstance
            {
                get
                {
                    return TType.CacheInstance;
                }
            }
            public static ExternalProperties External
            {
                get
                {
                    return TType.External;
                }
            }

            // types embarqués
            private static class TType
            {
                // Fields
                public static readonly T CacheInstance;
                public static ExternalProperties External { get; set; }

                // Methodes
                static TType()
                {
                    BaseCacheInstance.CacheGlobalInit<T>.TType.CacheInstance = new T();
                    External=new ExternalProperties();
                }
            }
        }
    }
}
