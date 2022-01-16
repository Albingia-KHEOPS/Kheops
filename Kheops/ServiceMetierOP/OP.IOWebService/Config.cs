using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Application.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OP.IOWebService
{
    public class Config : IConfig
    {
        public string Environement => "DEV";
    }
}