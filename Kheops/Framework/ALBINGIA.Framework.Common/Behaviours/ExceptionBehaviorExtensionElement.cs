using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Behaviours
{
    public class ExceptionBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType => typeof(ExceptionBehaviour);

        protected override object CreateBehavior()
        {
            return new ExceptionBehaviour();
        }
    }
}
