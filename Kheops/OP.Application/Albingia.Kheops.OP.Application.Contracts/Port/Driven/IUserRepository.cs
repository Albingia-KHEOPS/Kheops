﻿using Albingia.Kheops.OP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface IUserRepository {
        ProfileKheops GetProfile(string userId);
    }
}
