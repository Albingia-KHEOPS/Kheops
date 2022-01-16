using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class SqlMapperConfig {
        public SqlMapperConfig() {}

        private static object syncRoot = new Object();
        private static bool inited = false;
        static public void Init()
        {
            if (!inited)
            {
                lock (syncRoot)
                {
                    if (!inited)
                    {
                        FluentMapper.Initialize(config =>
                        {
                            config.AddMap(new KJobSortiMapper());
                            config.AddMap(new KpClauseMapper());
                            config.AddMap(new KpCotgaMapper());
                            config.AddMap(new KpCotisMapper());
                            config.AddMap(new KpCtrlMapper());
                            config.AddMap(new KpCtrlEMapper());
                            config.AddMap(new KpDesiMapper());
                            config.AddMap(new KpEngMapper());
                            config.AddMap(new KpEngFamMapper());
                            config.AddMap(new KpEngGarMapper());
                            config.AddMap(new KpEngRsqMapper());
                            config.AddMap(new KpEngVenMapper());
                            config.AddMap(new KpEntMapper());
                            config.AddMap(new KpExpFrhMapper());
                            config.AddMap(new KpExpFrhDMapper());
                            config.AddMap(new KpExpLCIMapper());
                            config.AddMap(new KpExpLCIDMapper());
                            config.AddMap(new KpForMapper());
                            config.AddMap(new Kpgaran_GarContratMapper());
                            config.AddMap(new KpGaranMapper());
                            config.AddMap(new KpGarApMapper());
                            config.AddMap(new KpGarTarMapper());
                            config.AddMap(new KpInvAppMapper());
                            config.AddMap(new KpInveDMapper());
                            config.AddMap(new KpInvenMapper());
                            config.AddMap(new KpIoptMapper());
                            config.AddMap(new KpIrSGaMapper());
                            config.AddMap(new KpIrsObMapper());
                            config.AddMap(new KPISValMapper());
                            config.AddMap(new KpMatFfMapper());
                            config.AddMap(new KpMatFlMapper());
                            config.AddMap(new KpMatFrMapper());
                            config.AddMap(new KpMatGgMapper());
                            config.AddMap(new KpMatGlMapper());
                            config.AddMap(new KpMatGrMapper());
                            config.AddMap(new KpObjMapper());
                            config.AddMap(new KpObsvMapper());
                            config.AddMap(new KpOdblsMapper());
                            config.AddMap(new KpOppMapper());
                            config.AddMap(new KpOppApMapper());
                            config.AddMap(new KpOptMapper());
                            config.AddMap(new KpOptApMapper());
                            config.AddMap(new KpOptDMapper());
                            config.AddMap(new KpRsqMapper());
                            config.AddMap(new KblocMapper());
                            config.AddMap(new KblorelMapper());
                            config.AddMap(new KCanevMapper());
                            config.AddMap(new KCatBloc_BrancheCibleMapper());
                            config.AddMap(new KcatblocMapper());
                            config.AddMap(new KcatModeleBrancheCibleMapper());
                            config.AddMap(new KcatmodeleMapper());
                            config.AddMap(new KcatVolet_BrancheCibleMapper());
                            config.AddMap(new KcatvoletMapper());
                            config.AddMap(new KCheminMapper());
                            config.AddMap(new KcibleMapper());
                            config.AddMap(new KcliblefMapper());
                            config.AddMap(new KClauseMapper());
                            config.AddMap(new KDesiMapper());
                            config.AddMap(new KedilogMapper());
                            config.AddMap(new KExpFrhMapper());
                            config.AddMap(new KExpFrhDMapper());
                            config.AddMap(new KExpLciMapper());
                            config.AddMap(new KExpLciDMapper());
                            config.AddMap(new KFiltreMapper());
                            config.AddMap(new KFiltrLMapper());
                            config.AddMap(new KganparMapper());
                            config.AddMap(new KGaranMapper());
                            config.AddMap(new KGarFamMapper());
                            config.AddMap(new KInvTypMapper());
                            config.AddMap(new KISModMapper());
                            config.AddMap(new KISModlMapper());
                            config.AddMap(new KISRefMapper());
                            config.AddMap(new KnmGriMapper());
                            config.AddMap(new KnmRefMapper());
                            config.AddMap(new KnmValfMapper());
                            config.AddMap(new KpAvTrcMapper());
                            config.AddMap(new KpCopDcMapper());
                            config.AddMap(new KpCopidMapper());
                            config.AddMap(new KpCtrlAMapper());
                            config.AddMap(new KpDocMapper());
                            config.AddMap(new KpDocExtMapper());
                            config.AddMap(new KpDocLMapper());
                            config.AddMap(new KpDocLDMapper());
                            config.AddMap(new Kpdobls_ListeMapper());
                            config.AddMap(new KpdoblsMapper());
                            config.AddMap(new KpOfEntMapper());
                            config.AddMap(new KpOfOptMapper());
                            config.AddMap(new KpOfRsqMapper());
                            config.AddMap(new KpOfTarMapper());
                            config.AddMap(new KpRguMapper());
                            config.AddMap(new KpRguGMapper());
                            config.AddMap(new KpRguRMapper());
                            config.AddMap(new KpRguWeMapper());
                            config.AddMap(new KpRguWpMapper());
                            config.AddMap(new KpSelWMapper());
                            config.AddMap(new KpSuspMapper());
                            config.AddMap(new KReavenMapper());
                            config.AddMap(new KUsrDrtMapper());
                            config.AddMap(new KVerrouMapper());
                            config.AddMap(new KVoletMapper());
                            config.AddMap(new YAdressMapper());
                            config.AddMap(new YAssNomMapper());
                            config.AddMap(new YAssureMapper());
                            config.AddMap(new YcategoMapper());
                            config.AddMap(new CourrierContratMapper());
                            config.AddMap(new YcourriMapper());
                            config.AddMap(new CourtierMapper());
                            config.AddMap(new YcourtiMapper());
                            config.AddMap(new YCourtNMapper());
                            config.AddMap(new YpoAssuMapper());
                            config.AddMap(new YpoAssxMapper());
                            config.AddMap(new YpoBaseMapper());
                            config.AddMap(new YpoCoasMapper());
                            config.AddMap(new YpoConxMapper());
                            config.AddMap(new YpoCourMapper());
                            config.AddMap(new YpoEcheMapper());
                            config.AddMap(new YpoInteMapper());
                            config.AddMap(new YprtAdrMapper());
                            config.AddMap(new YprtEntMapper());
                            config.AddMap(new YprtFooMapper());
                            config.AddMap(new YprtForMapper());
                            config.AddMap(new YprtGarMapper());
                            config.AddMap(new YprtObjMapper());
                            config.AddMap(new YprtObtMapper());
                            config.AddMap(new YprtRsqMapper());
                            config.AddMap(new YPlmgaMapper());
                            config.AddMap(new YpltGaaMapper());
                            config.AddMap(new YpltGalMapper());
                            config.AddMap(new YpltGarMapper());
                            config.AddMap(new YpoTracMapper());
                            config.AddMap(new YPrimeMapper());
                            config.AddMap(new YPrimGaMapper());
                            config.AddMap(new YPriPGaMapper());
                            config.AddMap(new YprtPerMapper());
                            config.AddMap(new CourrierSinistreContratMapper());
                            config.AddMap(new YsincouMapper());
                            config.AddMap(new YSinCumMapper());
                            config.AddMap(new YSinistMapper());
                            config.AddMap(new Utilisateur_ListeNomMapper());
                            config.AddMap(new UtilisateurMapper());
                            config.AddMap(new YyyyParMapper());
                            config.AddMap(new KPTraceMapper());
                        });
                    }
                }
            }
        }
    }


}
