using ALBINGIA.Framework.Common;
using System.Web;

namespace Albingia.Kheops.OP.Service
{
    internal class HttpSuccessIndicator : ISuccessIndicator
    {
        public bool ShouldCommit { get; set; } = true;
    }
}