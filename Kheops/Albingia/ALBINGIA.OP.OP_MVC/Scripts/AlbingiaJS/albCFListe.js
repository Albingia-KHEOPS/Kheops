function GetConceptFamille(nomListe) {
    switch (nomListe) {
        case "branche":
            return "GENER/BRCHE";
            break;
        case "motscles":
            return "PRODU/POMOC";
            break;
        case "motifRefus":
            return "PRODU/PBSTF";
            break;
        case "devise":
            return "GENER/DEVIS";
            break;
        case "periodicite":
            return "PRODU/PBPER";
            break;
        case "uniteDuree":
            return "PRODU/PBCTU";
            break;
        case "indiceReference":
            return "GENER/INDIC";
            break;
        case "natureContrat":
            return "PRODU/PBNPL";
            break;
        case "fraisAperiteur":
            return "BCR/00347";
            break;
        case "contexte":
            return "PRODU/QECTX";
            break;
        case "etape":
            return "KHEOP/ETAPE";
            break;
        case "unite":
            return "PRODU/QCVAU";
            break;
        case "type":
            return "PRODU/QCVAT";
            break;
        case "codeAPE":
            return "ALSPK/NAF";
            break;
        case "codeActivite":
            return "KHEOP/TREAC";
            break;
        case "codeClasse":
            return "ALSPK/RSQ";
            break;
        case "territorialite":
            return "PRODU/QATRR";
            break;
        case "regimeTaxe":
            return "GENER/TAXRG";
            break;
        case "nature":
            return "PRODU/CBNAT";
            break;
        case "uniteDureeFormule":
            return "PRODU/QBVGU";
            break;
        case "application":
            return "PRODU/JHPRP";
            break;
        case "typeEmission":
            return "PRODU/JHPRE";
            break;
        case "codeTaxe":
            return "GENER/TAXEC";
            break;
        case "franchiseUnite":
            return "ALSPK/UNFRA";
            break;
        case "franchiseBase":
            return "ALSPK/BAFRA";
            break;
        case "LCIUnite":
            return "ALSPK/UNLCI";
            break;
        case "franchiseLCI":
            return "ALSPK/BALCI";
            break;
        case "assietteUnite":
            return "ALSPK/UNCAP";
            break;
        case "assietteBase":
            return "ALSPK/BACAP";
            break;
        case "primeUnite":
            return "PRODU/QBVGU";
            break;
        case "tranche":
            return "GAREA/TRCHE";
            break;
        case "antecedent":
            return "PRODU/PBANT";
            break;
        case "naturelieu":
            return "ALSPK/NLOC";
        case "codemateriel":
            return "KHEOP/MATRS";
        case "typeIntervenant":
            return "PRODU/INTYI";
        case "typologie":
            return "KHEOP/NMTYP";
        case "qualite":
            return "PRODU/OJQLT";
        case "fraisAcc":
            return "PRODU/FBAFC";
        case "motifavt":
            return "PRODU/PBAVC";
        case "stopavt":
            return "PRODU/PBSTP";
        case "stopan":
            return "PRODU/PBSTP";
        default:
            return "";
            break;
    }

}