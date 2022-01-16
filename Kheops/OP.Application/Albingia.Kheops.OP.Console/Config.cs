using Albingia.Kheops.OP.Application.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Kheops.OP.Test
{
    public class Config : IConfig
    {
        public string Environement => "DEV";
    }
}