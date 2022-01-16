using ALBINGIA.Framework.Common.CacheTools;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;
namespace ALBINGIA.Framework.Common.Tools
{
    public class ContextMessageInspector : IClientMessageInspector
    {
        void IClientMessageInspector.AfterReceiveReply(ref Message reply, object correlationState)
        {
        }
        object IClientMessageInspector.BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            request.Headers.Add(MessageHeader.CreateHeader("User", "http://albingia.fr/kheops/2018", AlbSessionHelper.UserLogin));
            request.Headers.Add(MessageHeader.CreateHeader("UserAS400", "http://albingia.fr/kheops/2018", AlbSessionHelper.ConnectedUser));
            return null;
        }
    }
    public class HttpUserEndpointBehavior : IEndpointBehavior
    {
        public HttpUserEndpointBehavior()
        {
        }
        #region IEndpointBehavior Members
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }
        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            ContextMessageInspector inspector = new ContextMessageInspector();
            clientRuntime.MessageInspectors.Add(inspector);
        }
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
        }
        public void Validate(ServiceEndpoint endpoint)
        {
        }
        #endregion
    }
    public class ContextBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(HttpUserEndpointBehavior);
            }
        }
        protected override object CreateBehavior()
        {
            return new HttpUserEndpointBehavior();
        }
    }
}