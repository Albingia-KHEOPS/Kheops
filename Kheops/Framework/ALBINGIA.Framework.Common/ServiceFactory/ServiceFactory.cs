using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.ServiceFactory
{
    public class ServiceClientFactory
    {
        private static object syncRoot = new Object();

        private static Dictionary<Type, ChannelFactory> cache = new Dictionary<Type, ChannelFactory>();
        public static Client<T> GetClient<T>() {
            var t = typeof(T);
            ChannelFactory<T> factory = null;
            if (!cache.ContainsKey(t))
            {
                lock (syncRoot)
                {
                    if (!cache.ContainsKey(t))
                    {
                        factory = new ChannelFactory<T>("*");
                        if (factory.State != CommunicationState.Opened && factory.State != CommunicationState.Opening)
                        {
                            factory.Open();
                        }
                        cache.Add(t, factory);
                    }
                }
            }
            if(factory is null)
            {
                factory = (ChannelFactory<T>)cache[t];
            }
            return new Client<T>(factory.CreateChannel());
        }

        public class Client<T> : IDisposable {
            private readonly T channel;
            public T Channel { get { return channel; } }

            public ICommunicationObject Communication {get { return (ICommunicationObject) channel; } }
            public Client(T channel)
            {
                this.channel = channel;
            }

            #region IDisposable Support
            
            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                (this.channel as IDisposable)?.Dispose();
            }

            public static implicit operator T(Client<T> d) {
                return d.Channel;
            }
            #endregion


        }
    }
}
