using Hexavia.Business.Interfaces;
using Hexavia.Repository.Interfaces;
using Hexavia.Models;
using Hexavia.Models.EnumDir;
using Hexavia.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Hexavia.Business
{
    public class PaysBusiness : IPaysBusiness
    {
        private static readonly string CacheKeyPays = CacheKeys.Pays.ToDescription();
        private IPaysRepository PaysRepository { get; }

        public PaysBusiness(IPaysRepository paysRepository)
        {
            PaysRepository = paysRepository;
        }

        private DateTimeOffset AbsoluteExpirationPays()
        {
            return CacheManager.GetAbsoluteExpirationConfiguration(CacheKeyPays);
        }
        public List<Pays> GetPays()
        {
            List<Pays> pays;
            if (CacheManager.TryGet(CacheKeyPays, out pays))
            {
                return pays;
            }

            pays = PaysRepository.GetPays();
            if (pays.Count == 0)
            {
                return new List<Pays>();
            }

            CacheManager.Add(CacheKeyPays, pays, AbsoluteExpirationPays(), CacheItemRemoved);

            return pays;
        }

        private void CacheItemRemoved(CacheEntryUpdateArguments args)
        {
            if (args.RemovedReason != CacheEntryRemovedReason.Expired && args.RemovedReason != CacheEntryRemovedReason.Removed)
            {
                return;
            }

            var updatedEntity = PaysRepository.GetPays();
            args.UpdatedCacheItem = new CacheItem(args.Key, updatedEntity);
            args.UpdatedCacheItemPolicy = CacheManager.GetCacheItemPolicy(AbsoluteExpirationPays(), CacheItemRemoved);
        }
    }
}
