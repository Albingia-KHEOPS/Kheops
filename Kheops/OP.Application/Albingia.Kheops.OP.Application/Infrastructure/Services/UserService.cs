using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Infrastructure.Services {
    public class UserService : IUserPort {
        const string UserProfileCacheKey = "userProfile_";
        private readonly ISessionContext sessionContext;
        private readonly IUserRepository userRepository;
        private readonly ILiveDataCache cache;
        public UserService(ILiveDataCache liveDataCache, ISessionContext sessionContext, IUserRepository userRepository) {
            this.sessionContext = sessionContext;
            this.cache = liveDataCache;
            this.userRepository = userRepository;
        }
        public ProfileKheops GetProfile() {
            var profile = this.cache.Get<ProfileKheops>($"{UserProfileCacheKey}{this.sessionContext.UserId}");
            if (profile is null) {
                profile = this.userRepository.GetProfile(this.sessionContext.UserId);
                UpdateCacheProfile(profile);
            }
            return profile;
        }
        public ProfileKheops SetProfile(ProfileKheops profile, IEnumerable<ProfileKheopsData> specificUpdate = null) {
            if (specificUpdate?.Contains(ProfileKheopsData.None) ?? false) {
                return profile;
            }
            else if (!specificUpdate?.Any() ?? true) {
                UpdateCacheProfile(profile);
            }
            else {
                var currentProfile = this.cache.Get<ProfileKheops>($"{UserProfileCacheKey}{this.sessionContext.UserId}");
                currentProfile.Update(profile, specificUpdate);
                UpdateCacheProfile(currentProfile);
            }
            return GetProfile();
        }

        private void UpdateCacheProfile(ProfileKheops profile)
        {
            this.cache.SetPolicy<ProfileKheops>(() => new System.Runtime.Caching.CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddDays(1).Date });
            this.cache.Set($"{UserProfileCacheKey}{this.sessionContext.UserId}", profile);
        }
    }
}
