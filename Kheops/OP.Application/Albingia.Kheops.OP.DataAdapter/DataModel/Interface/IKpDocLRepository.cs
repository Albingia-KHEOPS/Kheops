using System.Collections.Generic;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Interface
{
    public interface IKpDocLRepository
    {
        void Delete(KpDocL value);
        KpDocL Get(long id);
        IEnumerable<KpDocL> GetByAffaire(string typeAffaire, string numeroAffaire, int numeroAliment, int numeroAvenant);
        void Insert(KpDocL value);
        int NewId();
        void Update(KpDocL value);
    }
}