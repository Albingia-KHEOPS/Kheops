
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using OP.WSAS400.DTO.FormuleGarantie.GarantieModele;
using OP.WSAS400.DTO.GarantieModele;
using System.Collections.Generic;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter
{
    public class ParametrageModelesRepository : IParametrageModelesRepository
    {
        private readonly YPlmgaRepository yplmgaRepository;
        private readonly YpltGarRepository ypltgarRepository;
        private readonly YpltGalRepository ypltgalRepository;
        private readonly YpltGaaRepository ypltgaaRepository;
        private readonly KcatmodeleRepository kcatmodeleRepository;
        private readonly KcatblocRepository kcatblocRepository;

        //private readonly IReferentialRepository referentialRepository;
        public ParametrageModelesRepository(YPlmgaRepository yplmgaRepository, YpltGarRepository ypltgarRepository,
                                            YpltGalRepository ypltgalRepository, YpltGaaRepository ypltgaaRepository,
                                            KcatmodeleRepository kcatmodeleRepository, KcatblocRepository kcatblocRepository)
        {
            this.yplmgaRepository = yplmgaRepository;
            this.ypltgarRepository = ypltgarRepository;
            this.ypltgalRepository = ypltgalRepository;
            this.ypltgaaRepository = ypltgaaRepository;
            this.kcatmodeleRepository = kcatmodeleRepository;
            this.kcatblocRepository = kcatblocRepository;
        }

        #region Garantie Modele
        public List<GarantieModeleDto> RechercherGarantieModele(string code, string description)
        {
            List<GarantieModeleDto> toReturn = new List<GarantieModeleDto>();
            if (description == null) description = string.Empty;
            description = "%" + description.ToUpper() + "%";
            code = "%" + code.ToUpper() + "%";
            List<YPlmga> test = this.yplmgaRepository.RechercherGarantieModele(code, description).ToList();
            toReturn = test.Select(x => new GarantieModeleDto()
            {
                Code = x.D1mga,
                Description = x.D1lib,
            }).ToList();
            return toReturn;
        }

        public GarantieModeleDto GetGarantieModele(string code)
        {
            var model = new GarantieModeleDto();
            var result = this.yplmgaRepository.GetGarantieModele(code);
            if (result != null && result.Count > 0)
            {
                model = new GarantieModeleDto
                {
                    GuidId = result[0].Guid.ToString(),
                    Code = result[0].Code,
                    Description = result[0].Description
                };
                result.ForEach(g =>
                {
                    if (!string.IsNullOrEmpty(g.Branche))
                        model.LstModeleGarantie.Add(new ModeleGarantieDto
                        {
                            Branche = g.Branche,
                            Cible = g.Cible,
                            Volet = g.Volet,
                            Bloc = g.Bloc,
                            Typologie = g.Typologie
                        });
                });
            }
            return model;
        }

        public void InsertGarantieModele(string code, string description)
        {
            YPlmga modele = new YPlmga()
            {
                D1mga = code,
                D1lib = description,
            };
            this.yplmgaRepository.Insert(modele);
        }

        public void UpdateGarantieModele(string code, string description)
        {
            YPlmga modele = this.yplmgaRepository.Get(code);
            modele.D1lib = description;
            this.yplmgaRepository.Update(modele);
        }

        public void CopierGarantieModele(string code, string codeCopie)
        {
            this.yplmgaRepository.CopierGarantieModele(code, codeCopie);
        }

        public void SupprimerGarantieModele(string code, out string msgRetour)
        {
            msgRetour = "";
            var listCatModele = this.kcatmodeleRepository.GetByModele(code);
            if (listCatModele != null && listCatModele.Any())
            {
                msgRetour = string.Format(@"Le modèle {0} ne peut être supprimé car il est utilisé dans : <br/>", code);
                foreach (var catModele in listCatModele)
                {
                    Kcatbloc bloc = this.kcatblocRepository.Get(catModele.Karkaqid);
                    msgRetour += string.Format("\u2022 {0} - {1} - {2} - {3} <br/>", bloc.Kaqbra, bloc.Kaqcible, bloc.Kaqvolet, bloc.Kaqbloc);
                }
            }
            else
            {
                YPlmga modele = this.yplmgaRepository.Get(code);
                List<YpltGar> listGar = this.ypltgarRepository.GetByModele(code).ToList();
                long[] listSeq = listGar.Select(x => x.C2seq).ToArray();

                this.ypltgaaRepository.SupprimerBySeq(listSeq);
                this.ypltgalRepository.SupprimerBySeq(listSeq);
                this.ypltgarRepository.SupprimerBySeq(listSeq);
                this.yplmgaRepository.Delete(modele);
            }
        }

        public bool ExistCodeModele(string code)
        {
            var modele = this.yplmgaRepository.Get(code);
            if (modele == null) { return false; }
            else { return true; }
        }
        #endregion

        #region Garantie Type
        public bool ExistDansContrat(string code)
        {
            return this.yplmgaRepository.ExistDansContrat(code);
        }

        public List<GarantieTypeDto> RechercherGarantieType(string codeModele)
        {
            var garantieTypePlatDto = this.ypltgarRepository.RechercherGarantieType(codeModele);

            return GetListGarantieType(garantieTypePlatDto, !ExistDansContrat(codeModele));
        }

        public GarantieTypeDto GetGarantieType(long seq)
        {
            var model = new GarantieTypeDto();

            var result = this.ypltgarRepository.GetInfo(seq);

            if (result != null && result.Count > 0)
            {
                model = new GarantieTypeDto(result, true, false, !ExistDansContrat(result.FirstOrDefault().C2mga));
            }
            return model;
        }

        public List<GarantieTypeDto> GetGarantieTypeAll()
        {
            var model = new GarantieTypeDto();

            var result = this.ypltgarRepository.GetGarantieTypeAll().ToList();

            return GetListGarantieType(result, niveau: 0);
        }

        public GarantieTypeDto GetGarantieTypeLien(long seq)
        {
            var model = new GarantieTypeDto();
            var result = this.ypltgarRepository.GetInfoLien(seq);

            if (result != null && result.Count > 0)
            {
                model = new GarantieTypeDto(result, false, true, !ExistDansContrat(result.FirstOrDefault().C2mga));
            }
            return model;
        }

        public void InsertGarantieType(GarantieTypeDto garType, out string msgRetour)
        {
            msgRetour = "";
            int newSeq = this.ypltgarRepository.NewId();
            garType.NumeroSeq = newSeq;
            if (newSeq > 0)
            {
                YpltGar gar = GarantieDtoToYpltgar(garType);
                this.ypltgarRepository.Insert(gar);

                foreach (var lci in garType.ListLCI)
                {
                    YpltGal garlci = GarantieLCIDtoToYpltgal(lci, newSeq);
                    this.ypltgalRepository.Insert(garlci);
                }
            }
            else
            {
                msgRetour = "Le numéro de séquence est incorrect. Veuillez contacter un administrateur.";
            }
        }

        public void UpdateGarantieType(GarantieTypeDto garType)
        {
            YpltGar gar = GarantieDtoToYpltgar(garType);
            this.ypltgarRepository.Update(gar);

            foreach (var lci in garType.ListLCI)
            {
                YpltGal garlci = GarantieLCIDtoToYpltgal(lci, garType.NumeroSeq);
                this.ypltgalRepository.Update(garlci);
            }
        }

        public void SupprimerGarantieType(long seq, out string msgRetour)
        {
            msgRetour = "";
            if (seq > 0)
            {
                // récupérer les num de sequence des sous-garanties
                var result = this.ypltgarRepository.GetSousGarantie(seq);
                long[] listSeq = result.Select(x => x.C2seq).ToArray();

                this.ypltgaaRepository.SupprimerBySeq(listSeq);
                this.ypltgalRepository.SupprimerBySeq(listSeq);
                this.ypltgarRepository.SupprimerBySeq(listSeq);
            }
            else
            {
                msgRetour = "Le numéro de séquence est incorrect, impossible de supprimer la garantie. Veuillez contacter un administrateur.";
            }
        }

        public bool ExistCodeGarantie(string codeModele, string codeGarantie, long seqM)
        {
            return this.ypltgarRepository.ExistCodeGarantie(codeModele, codeGarantie, seqM);
        }
        public bool ExistTri(long seq, string codeModele, string tri)
        {
            return this.ypltgarRepository.ExistTri(seq, codeModele, tri).Any();
        }

        public void AjouterGarantieTypeLien(string type, long seqA, long seqB, out string msgRetour)
        {
            msgRetour = "";
            var types = new List<string>() { "A", "D", "I" };
            if (!types.Contains(type))
            {
                msgRetour = "Le type de liaison n'est pas reconnu.";
                return;
            }
            // vérifie si un autre lien est déjà présent présent pour les 2 garanties
            foreach (var item in types.Where(x => x != type))
            {
                YpltGaa lienCheck1 = this.ypltgaaRepository.GetLien(item, seqA, seqB).SingleOrDefault();
                YpltGaa lienCheck2 = this.ypltgaaRepository.GetLien(item, seqB, seqA).SingleOrDefault();
                if (lienCheck1 != null || lienCheck2 != null)
                {
                    msgRetour = "Impossible d'ajouter le lien car un autre type de lien est déjà présent entre ces 2 garanties.";
                    return;
                }
            }

            // vérifie une cohérence dans les relations "triangulaire"
            if (HasLienIncoherent(type, seqA, seqB))
            {
                msgRetour = "Impossible d'ajouter le lien car cela créerait une incohérence avec une autre garantie";
                return;
            }
            
            YpltGaa lienExist1 = this.ypltgaaRepository.GetLien(type, seqA, seqB).SingleOrDefault();
            YpltGaa lienExist2 = this.ypltgaaRepository.GetLien(type, seqB, seqA).SingleOrDefault();
            if (lienExist1 == null && lienExist2 == null)
            {
                YpltGaa lien = new YpltGaa()
                {
                    C5typ = type,
                    C5seq = seqA,
                    C5sem = seqB
                };
                this.ypltgaaRepository.Insert(lien);
            } else
            {
                switch (type)
                {
                    case "A":
                        msgRetour = "Cette association existe déjà.";
                        break;
                    case "D":
                        msgRetour = "Cette dépendance existe déjà.";
                        break;
                    case "I":
                        msgRetour = "Cette Incompatibilité existe déjà.";
                        break;
                }
            }
        }
        public void SupprimerGarantieTypeLien(string type, long seqA, long seqB, out string msgRetour)
        {
            msgRetour = "";
            YpltGaa lien = this.ypltgaaRepository.GetLien(type, seqA, seqB).SingleOrDefault();
            if (lien != null)
            {
                this.ypltgaaRepository.Delete(lien);
            } else
            {
                msgRetour = "La liaison à supprimer n'existe pas.";
            }
        }
        #endregion

        #region private methods 
        private static List<GarantieTypeDto> GetListGarantieType(List<GarantieTypePlatDto> listGarantieTypePlatDto, bool isModifiable = false, int niveau = 1, long sequence = 0)
        {
            var toReturn = new List<GarantieTypeDto>();
            if (niveau == 0) // retourne toutes les garanties sous forme d'une seule liste
            {
                if (listGarantieTypePlatDto != null && listGarantieTypePlatDto.Any())
                {
                    foreach (var garPlatDto in listGarantieTypePlatDto)
                    {
                        GarantieTypeDto garantieType = new GarantieTypeDto
                        {
                            NumeroSeq = garPlatDto.C2seq,
                            CodeModele = garPlatDto.C2mga,
                            NomModele = garPlatDto.C2obe,
                            Niveau = garPlatDto.C2niv,
                            CodeGarantie = garPlatDto.C2gar,
                            Ordre = garPlatDto.C2ord,
                            Description = garPlatDto.Gades,
                            NumeroSeqM = garPlatDto.C2sem,
                            NumeroSeq1 = garPlatDto.C2se1,
                            Caractere = garPlatDto.C2car,
                            CaractereLib = garPlatDto.C2carlib,
                            Nature = garPlatDto.C2nat,
                            NatureLib = garPlatDto.C2natlib,
                            IsIndexee = garPlatDto.C2inaBool,
                            SoumisCATNAT = garPlatDto.C2cnaBool,
                            CodeTaxe = garPlatDto.C2tax,
                            GroupeAlternative = garPlatDto.C2alt,
                            Conditionnement = garPlatDto.C2scr,
                            TypePrime = garPlatDto.C2prp,
                            TypeControleDate = garPlatDto.C2tcd,
                            IsMontantRef = garPlatDto.C2mrfBool,
                            IsNatureModifiable = garPlatDto.C2ntmBool,
                            IsMasquerCP = garPlatDto.C2masBool,
                            IsModifiable = isModifiable,
                        };
                        toReturn.Add(garantieType);
                    }
                }
                toReturn = toReturn.OrderBy(x => x.CodeModele).ToList();
            }
            else // sinon retourne un arbre des garanties 
            {
                if (listGarantieTypePlatDto != null && listGarantieTypePlatDto.Any() && niveau < 5)
                {
                    foreach (var garPlatDto in listGarantieTypePlatDto.Where(gar => gar.C2niv == niveau && gar.C2sem == sequence))
                    {
                        GarantieTypeDto garantieType = new GarantieTypeDto
                        {
                            NumeroSeq = garPlatDto.C2seq,
                            CodeModele = garPlatDto.C2mga,
                            NomModele = garPlatDto.C2obe,
                            Niveau = garPlatDto.C2niv,
                            CodeGarantie = garPlatDto.C2gar,
                            Ordre = garPlatDto.C2ord,
                            Description = garPlatDto.Gades,
                            NumeroSeqM = garPlatDto.C2sem,
                            NumeroSeq1 = garPlatDto.C2se1,
                            Caractere = garPlatDto.C2car,
                            CaractereLib = garPlatDto.C2carlib,
                            Nature = garPlatDto.C2nat,
                            NatureLib = garPlatDto.C2natlib,
                            IsIndexee = garPlatDto.C2inaBool,
                            SoumisCATNAT = garPlatDto.C2cnaBool,
                            CodeTaxe = garPlatDto.C2tax,
                            GroupeAlternative = garPlatDto.C2alt,
                            Conditionnement = garPlatDto.C2scr,
                            TypePrime = garPlatDto.C2prp,
                            TypeControleDate = garPlatDto.C2tcd,
                            IsMontantRef = garPlatDto.C2mrfBool,
                            IsNatureModifiable = garPlatDto.C2ntmBool,
                            IsMasquerCP = garPlatDto.C2masBool,
                            IsModifiable = isModifiable,
                        };
                        garantieType.ListSousGarantie = GetListGarantieType(listGarantieTypePlatDto, isModifiable, niveau + 1, garPlatDto.C2seq);

                        toReturn.Add(garantieType);
                    }
                }
                toReturn = toReturn.OrderBy(x => x.Ordre).ToList();
            }
            return toReturn;
        }
        
        private static YpltGar GarantieDtoToYpltgar(GarantieTypeDto garType)
        {
            YpltGar gar = new YpltGar();
            gar.C2seq = garType.NumeroSeq;
            gar.C2mga = garType.CodeModele;
            gar.C2niv = garType.Niveau;
            gar.C2gar = garType.CodeGarantie;
            gar.C2ord = garType.Ordre;
            gar.C2se1 = garType.NumeroSeq1;
            gar.C2sem = garType.NumeroSeqM;
            gar.C2car = garType.Caractere;
            gar.C2nat = garType.Nature;
            gar.C2ina = garType.IsIndexee.ToYesNo();
            gar.C2cna = garType.SoumisCATNAT.ToYesNo();
            gar.C2tax = garType.CodeTaxe;
            gar.C2alt = garType.GroupeAlternative;
            gar.C2scr = garType.Conditionnement;
            gar.C2prp = garType.TypePrime;
            gar.C2tcd = garType.TypeControleDate;
            gar.C2mrf = garType.IsMontantRef.ToYesNo();
            gar.C2ntm = garType.IsNatureModifiable.ToYesNo();
            gar.C2mas = garType.IsMasquerCP.ToYesNo();
            gar.C2tri = garType.Tri;
            gar.C2lib = garType.Description;
            gar.C2obe = garType.NomModele;
            return gar;
        }

        private static YpltGal GarantieLCIDtoToYpltgal(GarantieTypeLCIDto lci, long seq)
        {
            YpltGal garlci = new YpltGal();
            garlci.C4seq = seq;
            garlci.C4typ = lci.Type;
            garlci.C4bas = lci.Base;
            garlci.C4unt = lci.Unite;
            garlci.C4val = lci.Valeur;
            garlci.C4maj = lci.Modi;
            garlci.C4obl = lci.Obl.ToYesNo();
            garlci.C4ala = lci.Alim;
            return garlci;
        }

        private bool HasLienIncoherent(string type, long seqA, long seqB)
        {
            var hasLienIncoherent = false;
            var lienA = ypltgarRepository.GetInfoLien(seqA);
            var lienB = ypltgarRepository.GetInfoLien(seqB);

            if (lienA.FirstOrDefault().C2mga != lienB.FirstOrDefault().C2mga)
            {
                if (ypltgarRepository.ExistLienModeleIncoherent(type, seqA, seqB))
                {
                    hasLienIncoherent = true;
                }
            }

            lienA = ypltgarRepository.GetInfoLien(seqA).Where(x => !string.IsNullOrEmpty(x.C5typ)).ToList();
            lienB = ypltgarRepository.GetInfoLien(seqB).Where(x => !string.IsNullOrEmpty(x.C5typ)).ToList();

            switch (type)
            {
                case "A":
                case "D":
                    var incA = lienA.Where(x => x.C5typ == "I").Select(x => x.C5seq == seqA ? x.C5sem : x.C5seq);
                    var incB = lienB.Where(x => x.C5typ == "I").Select(x => x.C5seq == seqB ? x.C5sem : x.C5seq);
                    if (lienA.Select(x => x.C5seq == seqA ? x.C5sem : x.C5seq).Any(x => incB.Contains(x)) ||
                        lienB.Select(x => x.C5seq == seqB ? x.C5sem : x.C5seq).Any(x => incA.Contains(x)))
                    {
                        hasLienIncoherent = true;
                    }
                    break;
                case "I":
                    var assA = lienA.Where(x => x.C5typ == "A" || x.C5typ == "D").Select(x => x.C5seq == seqA ? x.C5sem : x.C5seq);
                    var assB = lienB.Where(x => x.C5typ == "A" || x.C5typ == "D").Select(x => x.C5seq == seqB ? x.C5sem : x.C5seq);
                    if (lienA.Select(x => x.C5seq == seqA ? x.C5sem : x.C5seq).Any(x => assB.Contains(x)) ||
                        lienB.Select(x => x.C5seq == seqB ? x.C5sem : x.C5seq).Any(x => assA.Contains(x)))
                    {
                        hasLienIncoherent = true;
                    }
                    break;
            }
            return hasLienIncoherent;
        }
        #endregion
    }
}

