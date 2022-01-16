using System;

namespace ALBINGIA.Framework.Common.AlbingiaExceptions
{
    public class AlbTechException:AlbException
    {
        
        #region constructeur
      
        public AlbTechException(Exception ex, bool trace = true, bool sendMail = true,string erreurParameters="",string callFuncTrace="E")
            : base(ex, trace, sendMail,callFuncTrace,erreurParameters:erreurParameters,onlyMesage:false)
        {
           
        }
        #endregion

        #region Méthodes surchargées 
        public override string Message
        {
            get
            {
       
                if (Trace)
                    TraceError(this);
                return string.IsNullOrEmpty(AlbExpMessage) ? base.Message : AlbExpMessage;
            }

        }
        #endregion
    }
}
