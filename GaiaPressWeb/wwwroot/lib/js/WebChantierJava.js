// Attendre que le DOM soit chargé
document.addEventListener("DOMContentLoaded", function () {
    // Données d'exemple pour l'arborescence
    const data = [
        {
            text: "Chantier 1",
            id: "chantier1",
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
            text: "Chantier 2",
            id: "chantier2"
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
