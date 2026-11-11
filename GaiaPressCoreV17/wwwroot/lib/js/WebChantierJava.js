// Attendre que le DOM soit chargé
document.addEventListener("DOMContentLoaded", function () {
    // Données d'exemple pour l'arborescence
    const data = [
        {
            text: "ConstructionSite 1",
            id: "ConstructionSite1",
            children: [
                {
                    text: "Forage 1",
                    id: "forage1",
                    children: [
                        { text: "Essai 1", id: "essai1" },
                        { text: "Essai 2", id: "essai2" }
                    ]
                },
                {
                    text: "Forage 2",
                    id: "forage2"
                }
            ]
        },
        {
            text: "ConstructionSite 2",
            id: "ConstructionSite2"
        }
    ];

    // Initialiser jsTree
    $('#jstree').jstree({
        'core': {
            'data': data,
            'themes': {
                'responsive': true
            }
        }
    });
});
