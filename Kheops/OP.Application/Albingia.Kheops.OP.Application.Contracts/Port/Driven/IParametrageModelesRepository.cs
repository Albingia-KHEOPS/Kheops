
using OP.WSAS400.DTO.GarantieModele;
using System.Collections.Generic;

namespace Albingia.Kheops.OP.Application.Port.Driven
{
    public interface IParametrageModelesRepository
    {
        List<GarantieModeleDto> RechercherGarantieModele(string code, string description);
        GarantieModeleDto GetGarantieModele(string code);
        void InsertGarantieModele(string code, string description);
        void UpdateGarantieModele(string code, string description);
        void CopierGarantieModele(string code, string codeCopie);
        void SupprimerGarantieModele(string code, out string msgRetour);
        bool ExistCodeModele(string code);

        bool ExistDansContrat(string code);
        List<GarantieTypeDto> RechercherGarantieType(string codeModele);
        GarantieTypeDto GetGarantieType(long seq);
        GarantieTypeDto GetGarantieTypeLien(long seq);
        List<GarantieTypeDto> GetGarantieTypeAll();
        void InsertGarantieType(GarantieTypeDto garType, out string msgRetour);
        void UpdateGarantieType(GarantieTypeDto garType);
        void SupprimerGarantieType(long seq, out string msgRetour);
        bool ExistCodeGarantie(string codeModele, string codeGarantie, long seqM);
        bool ExistTri(long seq, string codeModele, string tri);
        void AjouterGarantieTypeLien(string type, long seqA, long seqB, out string msgRetour);
        void SupprimerGarantieTypeLien(string type, long seqA, long seqB, out string msgRetour);
    }
}

