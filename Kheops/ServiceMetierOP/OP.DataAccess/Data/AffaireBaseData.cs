using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.DataAccess.Data {
    public abstract class AffaireBaseData {
        private _AffaireBaseData affaire;
        virtual public string Ipb {
            get => affaire.Ipb;
            set => affaire.Ipb = value;
        }
        virtual public int Alx {
            get => affaire.Alx;
            set => affaire.Alx = value;
        }
        virtual public string Typ {
            get => affaire.Typ;
            set => affaire.Typ = value;
        }
        virtual public int Avn {
            get => affaire.Avn;
            set => affaire.Avn = value;
        }

        public override int GetHashCode() {
            return affaire.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (obj is AffaireBaseData a) {
                return affaire.Equals(a.affaire);
            }
            else if (obj is _AffaireBaseData data) {
                return affaire.Equals(data);
            }
            return false;
        }
    }
}
