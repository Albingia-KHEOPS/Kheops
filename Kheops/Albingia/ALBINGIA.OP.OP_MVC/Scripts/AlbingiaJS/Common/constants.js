var customEvents = {
    entete: {
        initializing: "affaire.entete.initializing",
        loaded: "affaire.entete.loaded",
        rendered: "affaire.entete.rendered",
        error: "affaire.entete.error"
    },
    formule: {
        initializing: "affaire.formule.options.initializing",
        loaded: "affaire.formule.options.loaded",
        rendered: "affaire.formule.options.rendered",
        error: "affaire.formule.options.error",
        saving: "affaire.formule.options.saving",
        startedNewAvenantFormule: "affaire.formule.options.startedNewAvenantFormule",
        cancelledNewAvenantFormule: "affaire.formule.options.cancelledNewAvenantFormule",
        cancelApplication: "affaire.formule.options.cancelApplication",
        voletChecked: "affaire.formule.options.voletChecked",
        labelChanged: "affaire.formule.options.labelChanged"
    },
    detailsGarantie: {
        initializing: "affaire.formule.detailsGarantie.initializing",
        loaded: "affaire.formule.detailsGarantie.loaded",
        error: "affaire.formule.detailsGarantie.error",
        cancelledEdit: "affaire.formule.detailsGarantie.cancelledEdit",
        validatedEdit: "affaire.formule.detailsGarantie.validatedEdit"
    },
    inventaireGarantie: {
        initializing: "affaire.formule.inventaires.initializing",
        loaded: "affaire.formule.inventaires.loaded",
        error: "affaire.formule.inventaires.error",
        cancelledEdit: "affaire.formule.inventaires.cancelledEdit",
        validatedEdit: "affaire.formule.inventaires.validatedEdit",
        validatedDelete: "affaire.formule.inventaires.validatedDelete"
    },
    porteesGarantie: {
        validatedEdit: "affaire.formule.porteesGarantie.validatedEdit",
        cancelledEdit: "affaire.formule.porteesGarantie.cancelledEdit"
    },
    connexites: {
        initializing: "affaire.connexites.initializing",
        loaded: "affaire.connexites.loaded",
        endEdit: "affaire.connexites.endEdit",
        error: "affaire.connexites.error",
        fusion: {
            loaded: "affaire.connexites.fusion.loaded",
            cancel: "affaire.connexites.fusion.cancel",
            terminated: "affaire.connexites.fusion.terminated"
        }
    },
    recherche: {
        initializing: "affaire.recherche.initializing",
        loaded: "affaire.recherche.loaded",
        selected: "affaire.recherche.selected",
        cancelled: "affaire.recherche.cancelled",
        terminated: "affaire.recherche.terminated",
        error: "affaire.recherche.error"
    },
    relances: {
        loaded: "affaire.relances.loaded",
        hiding: "affaire.relances.hiding"
    },
    preneurs: {
        lists: {
            impayes: {
                loaded: "preneurs.lists.impayes.loaded"
            },
            sinistres: {
                loaded: "preneurs.lists.sinistres.loaded"
            },
            retardsPaiement: {
                loaded: "preneurs.lists.retardsPaiement.loaded"
            }
        },
        alertes: {
            hiding: "preneurs.alertes.hiding"
        }
    },
    paging: {
        change: "paging.change",
        dataLoaded: "paging.dataLoaded",
        initFirstPage: "paging.initFirstPage"
    },
    blacklist: {
        affaires: {
            loaded: "blacklist.affaires.loaded",
            hiding: "blacklist.affaires.hiding"
        }
    }
};
