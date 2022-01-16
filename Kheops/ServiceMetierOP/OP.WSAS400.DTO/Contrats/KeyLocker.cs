using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Contrats
{
    public interface IKeyLocker
    {
        string[] KeyValues { get; set; }
    }
}
