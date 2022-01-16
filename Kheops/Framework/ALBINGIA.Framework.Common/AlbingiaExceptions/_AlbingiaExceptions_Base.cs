using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALBINGIA.Framework.Common.AlbingiaExceptions
{
    public enum NatureException
    {
        EnregNonTrouve,
        EnregModifieDepuisLecture,
        RuptureIntegrite
    }
    public abstract class _AlbingiaExceptions_Base : Exception
    {
        public string Method { get; set; }
        public object[] MethodParameters;
        public string FunctionalMessage { get; set; }
    }
}
