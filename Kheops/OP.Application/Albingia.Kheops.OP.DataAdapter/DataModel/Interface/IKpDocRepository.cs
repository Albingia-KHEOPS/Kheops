using System.Collections.Generic;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Interface
{
    public interface IKpDocRepository
    {
        void Delete(KpDoc value);
        KpDoc Get(long id);
        IEnumerable<KpDoc> GetByAffaire(string typeAffaire, string numeroAffaire, int numeroAliment, int numeroAvenant);
        void Insert(KpDoc value);
        int NewId();
        void Update(KpDoc value);
    }
}