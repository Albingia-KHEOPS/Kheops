
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Port.Driver;
using OP.WSAS400.DTO.GarantieModele;
using System.Collections.Generic;
using System.ServiceModel.Activation;

namespace Albingia.Kheops.OP.Application.Infrastructure.Services
{

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ParametrageModelesService : IParametrageModelesPort
    {
        private readonly IParametrageModelesRepository parametrageModeleRepository;
        public ParametrageModelesService(IParametrageModelesRepository parametrageModeleRepository)
        {
            this.parametrageModeleRepository = parametrageModeleRepository;
        }

        #region Garantie Modele
        public List<GarantieModeleDto> GarantieModeleGet(string code, string description)
        {
            List<GarantieModeleDto> garantieModeleDto = null;

            garantieModeleDto = parametrageModeleRepository.RechercherGarantieModele(code, description);

            return garantieModeleDto;
        }

        public GarantieModeleDto GarantieModeleInfoGet(string code)
        {
            return parametrageModeleRepository.GetGarantieModele(code);
        }

        public void EnregistrerGarantieModele(string code, string description, bool isNew, out string msgRetour)
        {
            msgRetour = "";
            if (isNew)
            {
                if (!parametrageModeleRepository.ExistCodeModele(code))
                {
                    parametrageModeleRepository.InsertGarantieModele(code, description);
                }
                else
                {
                    msgRetour = string.Format(@"Le code {0} est déjà utilisé par un autre modèle de garantie", code);
                }
            }
            else
            {
                parametrageModeleRepository.UpdateGarantieModele(code, description);
            }
        }

        public void CopierGarantieModele(string code, string codeCopie, out string msgRetour)
        {
            msgRetour = "";
            if (!parametrageModeleRepository.ExistCodeModele(codeCopie))
            {
                parametrageModeleRepository.CopierGarantieModele(code, codeCopie);
            }
            else
            {
                msgRetour = string.Format(@"Le code {0} est déjà utilisé par un autre modèle de garantie", codeCopie);
            }
        }

        public void SupprimerGarantieModele(string code, out string msgRetour)
        {
            parametrageModeleRepository.SupprimerGarantieModele(code, out msgRetour);
        }
        #endregion

        #region Garantie Type
        public bool ExistGarantieModeleDansContrat(string code)
        {
            return parametrageModeleRepository.ExistDansContrat(code);
        }

        public List<GarantieTypeDto> GarantieTypeGet(string codeModele)
        {
            List<GarantieTypeDto> garantieTypeDto;

            garantieTypeDto = parametrageModeleRepository.RechercherGarantieType(codeModele);

            return garantieTypeDto;
        }

        public GarantieTypeDto GarantieTypeInfoGet(long seq)
        {
            return parametrageModeleRepository.GetGarantieType(seq);
        }

        public GarantieTypeDto GarantieTypeLienInfoGet(long seq)
        {
            return parametrageModeleRepository.GetGarantieTypeLien(seq);
        }

        public List<GarantieTypeDto> GarantieTypeGetAll()
        {
            List<GarantieTypeDto> garantieTypeDto;

            garantieTypeDto = parametrageModeleRepository.GetGarantieTypeAll();

            return garantieTypeDto;
        }

        public void EnregistrerGarantieType(GarantieTypeDto garType, bool isNew, out string msgRetour)
        {
            msgRetour = "";

            if (!parametrageModeleRepository.ExistTri(garType.NumeroSeq, garType.CodeModele, garType.Tri))
            {
                if (isNew)
                {
                    if (!parametrageModeleRepository.ExistCodeGarantie(garType.CodeModele, garType.CodeGarantie, garType.NumeroSeqM))
                    {
                        parametrageModeleRepository.InsertGarantieType(garType, out msgRetour);
                    }
                    else
                    {
                        if (garType.Niveau == 1) { msgRetour = string.Format("La garantie {0} est déjà présente dans le modèle", garType.CodeGarantie); }
                        else { msgRetour = string.Format("La sous-garantie {0} est déjà présente", garType.CodeGarantie); }
                    }

                }
                else
                {
                    parametrageModeleRepository.UpdateGarantieType(garType);
                }
            }
            else
            {
                msgRetour = "Le numéro d'ordre est déjà utilisé par une autre garantie de même niveau";
            }
        }

        public void SupprimerGarantieType(long seq, out string msgRetour)
        {
            parametrageModeleRepository.SupprimerGarantieType(seq, out msgRetour);
        }

        public void AjouterGarantieTypeLien(string type, long seqA, long seqB, out string msgRetour)
        {
            parametrageModeleRepository.AjouterGarantieTypeLien(type, seqA, seqB, out msgRetour);
        }
        public void SupprimerGarantieTypeLien(string type, long seqA, long seqB, out string msgRetour)
        {
            parametrageModeleRepository.SupprimerGarantieTypeLien(type, seqA, seqB, out msgRetour);
        }
        #endregion
    }
}

