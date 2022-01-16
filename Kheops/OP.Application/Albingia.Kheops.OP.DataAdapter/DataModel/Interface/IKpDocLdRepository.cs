using System.Collections.Generic;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Interface
{
    public interface IKpDocLdRepository
    {
        void Delete(KpDocLD value);
        KpDocLD Get(long id);
        void Insert(KpDocLD value);
        int NewId();
        void Update(KpDocLD value);
        IEnumerable<KpDocLD> GetByAffaire(string typeAffaire, string codeAffaire, int numeroAliment, int numeroAvenant);
    }
}