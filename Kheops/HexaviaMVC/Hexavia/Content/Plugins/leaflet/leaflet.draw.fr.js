var drawToolbar = {
    actions: {
        title: "Annuler le dessin",
        text: "Annuler",
    },
    finish: {
        title: "Terminer le dessin",
        text: "Terminer",
    },
    undo: {
        title: "Supprimer le dernier point tir�",
        text: "Supprimer le dernier point",
    },
    buttons: {
        polyline: "Dessinez une polyligne",
        polygon: "Dessinez un polygone",
        rectangle: "Dessinez un rectangle",
        circle: "Dessiner un cercle",
        marker: "Dessinez un marqueur",
        circlemarker: "Dessinez un marqueur circulaire",
    },
};
var drawHandlers = {
    circle: {
        tooltip: {
            start: "Cliquez et faites glisser pour dessiner le cercle.",
        },
        radius: "Rayon",
    },
    circlemarker: {
        tooltip: {
            start: "Cliquez sur la carte pour placer le marqueur circulaire.",
        },
    },
    marker: {
        tooltip: {
            start: "Cliquez sur la carte pour placer le marqueur.",
        },
    },
    polygon: {
        tooltip: {
            start: "Cliquez pour commencer � dessiner.",
            cont: "Cliquez pour continuer � dessiner.",
            end: "Cliquez sur le premier point pour fermer cette forme.",
        },
    },
    polyline: {
        error: "<strong>Erreur:<strong> les polyligne ne peuvent pas traverser!",
        tooltip: {
            start: "Cliquez pour commencer � dessiner.",
            cont: "Cliquez pour continuer � dessiner.",
            end: "Cliquez sur le dernier point pour fermer cette forme.",
        },
    },
    rectangle: {
        tooltip: {
            start: "Cliquez et faites glisser pour dessiner le rectangle.",
        },
    },
    simpleshape: {
        tooltip: {
            end: "Rel�chez la souris pour terminer le dessin.",
        },
    },
};
var editToolbar = {
    actions: {
        save: {
            title: "Sauvegarder les modifications.",
            text: "Sauvegarder",
        },
        cancel: {
            title: "Annuler l'�dition, rejette toutes les modifications.",
            text: "Annuler",
        },
        clearAll: {
            title: "Effacez toutes les collections.",
            text: "Tout effacer",
        },
    },
    buttons: {
        edit: "Modifier les collections.",
        editDisabled: "Pas de collections � �diter.",
        remove: "Supprimez les collections.",
        removeDisabled: "Pas de collections � supprimer.",
    },
};
var editHandlers = {
    edit: {
        tooltip: {
            text: "S�lectionnez les poign�es ou le marqueur pour modifier l'entit�.",
            subtext: "Cliquez sur annuler pour r�tablir les modifications.",
        },
    },
    remove: {
        tooltip: {
            text: "Cliquez sur un entit� pour supprimer",
        },
    },
};
var locale = {
    draw: {
        toolbar: drawToolbar,
        handlers: drawHandlers,
    },
    edit: {
        toolbar: editToolbar,
        handlers: editHandlers,
    },
};


function drawFrLocales () {
   
   
    try {
        if (L && L.drawLocal) {
            L.drawLocal = locale;
        }
    }
    catch (e) {
        // Did not modify Leaflet.draw global
    }
   
}
