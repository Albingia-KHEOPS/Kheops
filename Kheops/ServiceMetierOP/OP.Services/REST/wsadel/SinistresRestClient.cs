using Albingia.Kheops.DTO;
using ALBINGIA.Framework.Common.Tools;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace OP.Services.REST.wsadel {
    public class SinistresRestClient {
        internal static readonly string wsadelUrl = ConfigurationManager.AppSettings["UrlRESTwsadel"];

        public async Task<SinistreDto> GetCalculsChargementAsync(SinistreDto sinistre, DateTime? dateDebut = null, DateTime? dateFin = null) {
            using (var client = new RestfulServiceHelper<CalculChargementRequest>(wsadelUrl)) {
                var response = await client.PostAsJsonAsync<CalculChargementResponse>(new CalculChargementRequest {
                    Donnees = new[] { new CalculChargementRequestData {
                        SinistreAnnee = sinistre.DateSurvenance.Year,
                        SinistreNumero = sinistre.Numero,
                        SinistreSousBranche = sinistre.CodeSousBranche,
                        DateDebut = dateDebut.HasValue ? dateDebut.Value.ToIntYMD() : 0,
                        DateFin = dateFin.HasValue ? dateFin.Value.ToIntYMD() : 99999999
                    } }
                });

                sinistre.CalculChargement = response.CALCUL_CHARGEMENTReturn.ResultatChgts.ResultatChgt;
            }
            return sinistre;
        }

        public SinistreDto GetCalculsChargement(SinistreDto sinistre, DateTime? dateDebut = null, DateTime? dateFin = null) {
            using (var client = new RestfulServiceHelper<CalculChargementRequest>(wsadelUrl)) {
                var response = client.Post<CalculChargementResponse>(new CalculChargementRequest {
                    Donnees = new[] { new CalculChargementRequestData {
                        SinistreAnnee = sinistre.DateSurvenance.Year,
                        SinistreNumero = sinistre.Numero,
                        SinistreSousBranche = sinistre.CodeSousBranche,
                        DateDebut = dateDebut.HasValue ? dateDebut.Value.ToIntYMD() : 0,
                        DateFin = dateFin.HasValue ? dateFin.Value.ToIntYMD() : 99999999
                    } }
                });

                sinistre.CalculChargement = response.CALCUL_CHARGEMENTReturn.ResultatChgts.ResultatChgt;
            }
            return sinistre;
        }
    }
}
