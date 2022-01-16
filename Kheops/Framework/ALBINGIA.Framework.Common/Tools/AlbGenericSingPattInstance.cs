namespace ALBINGIA.Framework.Common.Tools
{
  
    public class AlbGenericSingPattInstance<T> where T : new()
    {
        // Types Embarqués
        public static class AlbSingGlobalInit<U> where U : new()
        {
            // Properties
            public static U UniqueInstance
            {
                get
                {
                    return TType.TypeInstance;
                }
            }
         
            // types embarqués
            private static class TType
            {
                // Fields
                public static readonly U TypeInstance;
               

                // Methodes
                static TType()
                {
                    AlbGenericSingPattInstance<U>.AlbSingGlobalInit<U>.TType.TypeInstance = new U();
                }
            }
        }
    }
}
