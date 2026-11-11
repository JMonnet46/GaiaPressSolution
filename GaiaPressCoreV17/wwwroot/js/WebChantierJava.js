// Variables globales
let currentConstructionSite = null;
let currentForageIndex = null;
let lang = 1; // 1 = Français, 0 = Anglais (à adapter selon votre besoin)

// Initialisation au chargement de la page
document.addEventListener("DOMContentLoaded", function () {
    // Charger les données du ConstructionSite
    loadConstructionSite();

    // Initialiser les événements
    initEventListeners();

    // Initialiser les onglets
    initTabs();

    // Initialiser DataTables pour le tableau des forages
    $('#dataGridForages').DataTable({
        columns: [
            { data: 'refForage', title: 'Référence' },
            { data: 'nomForage', title: 'Nom' },
            { data: 'dateForage', title: 'Date' }
        ],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/fr-FR.json' // Langue française
        }
    });
});

// Charger les données du ConstructionSite
function loadConstructionSite() {
    // Récupérer l'ID du ConstructionSite depuis l'URL ou une variable globale
    const ConstructionSiteId = getConstructionSiteIdFromUrl(); // À implémenter selon votre besoin

    fetch(`/api/ConstructionSite/${ConstructionSiteId}`)
        .then(response => response.json())
        .then(data => {
            currentConstructionSite = data;
            renderConstructionSite(data);
            loadArborescence(data.RefChantier);
            loadForages(data.RefChantier);
        })
        .catch(error => {
            console.error("Erreur lors du chargement du ConstructionSite :", error);
            alert("Erreur lors du chargement du ConstructionSite.");
        });
}

// Rendre les données du ConstructionSite dans le formulaire
function renderConstructionSite(ConstructionSite) {
    document.getElementById("txtRefChantier").value = ConstructionSite.RefChantier || "";
    document.getElementById("txtNomConstructionSite").value = ConstructionSite.NomConstructionSite || "";
    document.getElementById("txtRefClient").value = ConstructionSite.RefClient || "";
    document.getElementById("txtEntreprise").value = ConstructionSite.Entreprise || "";
    document.getElementById("txtTelephone").value = ConstructionSite.Telephone || "";
    document.getElementById("txtAdresse").value = ConstructionSite.Adresse || "";
    document.getElementById("txtDescription").value = ConstructionSite.DescriptifConstructionSite || "";

    // Verrouiller les champs par défaut
    verrouilleChamps();
}

// Charger l'arborescence
function loadArborescence(RefChantier) {
    fetch(`/api/ConstructionSite/${RefChantier}/arborescence`)
        .then(response => response.json())
        .then(data => {
            $('#tvArborescence').jstree({
                'core': {
                    'data': data,
                    'themes': {
                        'responsive': true
                    }
                }
            });
        })
        .catch(error => {
            console.error("Erreur lors du chargement de l'arborescence :", error);
        });
}

// Charger les forages dans le DataTable
function loadForages(RefChantier) {
    fetch(`/api/ConstructionSite/${RefChantier}/forages`)
        .then(response => response.json())
        .then(forages => {
            const table = $('#dataGridForages').DataTable();
            table.clear().rows.add(forages).draw();
        })
        .catch(error => {
            console.error("Erreur lors du chargement des forages :", error);
        });
}

// Initialiser les écouteurs d'événements
function initEventListeners() {
    // Bouton Modifier
    document.getElementById("btnModifier").addEventListener("click", function () {
        libereChamps();
    });

    // Bouton OK
    document.getElementById("btnOk").addEventListener("click", function () {
        saveConstructionSite();
    });

    // Bouton Annuler
    document.getElementById("btnAnnuler").addEventListener("click", function () {
        if (confirm("Voulez-vous annuler vos modifications ?")) {
            loadConstructionSite(); // Recharger les données
            verrouilleChamps();
        }
    });

    // Bouton Imprimer
    // document.getElementById("btnImprimer").addEventListener("click", function () {
    //     printDocument();
    // });

    // Bouton Fermer
    document.getElementById("btnFermer").addEventListener("click", function () {
        window.close(); // Ferme l'onglet (ou redirige)
    });

    // Bouton Ajouter Forage
    document.getElementById("btnAjouterForage").addEventListener("click", function () {
        ajouterForage();
    });

    // Bouton Ouvrir Forage
    document.getElementById("btnOuvrirForage").addEventListener("click", function () {
        const row = $('#dataGridForages').DataTable().row('.selected').data();
        if (row) {
            ouvrirForage(row.refForage);
        } else {
            alert("Veuillez sélectionner un forage.");
        }
    });

    // Bouton Supprimer Forage
    document.getElementById("btnSupprimerForage").addEventListener("click", function () {
        const row = $('#dataGridForages').DataTable().row('.selected').data();
        if (row) {
            supprimerForage(row.refForage);
        } else {
            alert("Veuillez sélectionner un forage.");
        }
    });

    // Sélection d'une ligne dans le DataTable
    $('#dataGridForages tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        } else {
            $('#dataGridForages tbody tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
}

// Initialiser les onglets
function initTabs() {
    const tabEl = document.querySelectorAll('#ConstructionSiteTabs button');
    tabEl.forEach(tab => {
        tab.addEventListener('shown.bs.tab', event => {
            // Recharger les données si nécessaire
            if (event.target.id === 'tabForages') {
                loadForages(currentConstructionSite.RefChantier);
            }
        });
    });
}

// Verrouiller les champs (équivalent de VerrouilleChamps)
function verrouilleChamps() {
    const fields = [
        "txtRefChantier", "txtNomConstructionSite", "txtRefClient",
        "txtEntreprise", "txtTelephone", "txtAdresse", "txtDescription"
    ];

    fields.forEach(fieldId => {
        const field = document.getElementById(fieldId);
        if (field) {
            field.readOnly = true;
            field.style.backgroundColor = "#e9ecef"; // Couleur grise claire
        }
    });

    // Désactiver les boutons OK et Annuler
    document.getElementById("btnOk").disabled = true;
    document.getElementById("btnAnnuler").disabled = true;
    document.getElementById("btnModifier").disabled = false;
}

// Libérer les champs (équivalent de LibereChamps)
function libereChamps() {
    const fields = [
        "txtNomConstructionSite", "txtRefClient", "txtEntreprise",
        "txtTelephone", "txtAdresse", "txtDescription"
    ];

    fields.forEach(fieldId => {
        const field = document.getElementById(fieldId);
        if (field) {
            field.readOnly = false;
            field.style.backgroundColor = ""; // Réinitialiser la couleur
        }
    });

    // Activer les boutons OK et Annuler
    document.getElementById("btnOk").disabled = false;
    document.getElementById("btnAnnuler").disabled = false;
    document.getElementById("btnModifier").disabled = true;
}

// Sauvegarder le ConstructionSite (équivalent de CmdOk_Click)
function saveConstructionSite() {
    const ConstructionSite = {
        RefChantier: document.getElementById("txtRefChantier").value,
        NomConstructionSite: document.getElementById("txtNomConstructionSite").value,
        RefClient: document.getElementById("txtRefClient").value,
        Entreprise: document.getElementById("txtEntreprise").value,
        Telephone: document.getElementById("txtTelephone").value,
        Adresse: document.getElementById("txtAdresse").value,
        DescriptifConstructionSite: document.getElementById("txtDescription").value
    };

    fetch('/api/ConstructionSite', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(ConstructionSite)
    })
        .then(response => response.json())
        .then(data => {
            alert("ConstructionSite sauvegardé avec succès !");
            verrouilleChamps();
            // Recharger les données si nécessaire
            loadConstructionSite();
        })
        .catch(error => {
            console.error("Erreur lors de la sauvegarde :", error);
            alert("Erreur lors de la sauvegarde du ConstructionSite.");
        });
}

// Imprimer le document (équivalent de CmdImprime Click)
// // function printDocument() {
    // Option 1 : Imprimer directement depuis le navigateur
    window.print();

    // Option 2 : Générer un PDF côté serveur et le télécharger
    // fetch(`/api/ConstructionSite/${currentConstructionSite.RefChantier}/pdf`)
    //     .then(response => response.blob())
    //     .then(blob => {
    //         const url = window.URL.createObjectURL(blob);
    //         const a = document.createElement('a');
    //         a.href = url;
    //         a.download = `Chantie ${currentConstructionSite.RefChantier}.pdf`;
    //         a.click();
    //         window.URL.revokeObjectURL(url);
    //     });
}

// Ajouter un forage (équivalent de CmdAjoute Click)
function ajouterForage() {
    // Ouvrir une nouvelle fenêtre ou une modale pour ajouter un forage
    // Exemple : Rediriger vers une page de création de forage
    window.location.href = `/forage.html?ConstructionSiteId=${currentConstructionSite.RefChantier}`;
}

// Ouvrir un forage (équivalent de CmdOuvri Click)
function ouvrirForage(refForage) {
    window.location.href = `/forage.html?ConstructionSiteId=${currentConstructionSite.RefChantier}&forageId=${refForage}`;
}

// Supprimer un forage (équivalent de CmdSupprime Click)
function supprimerForage(refForage) {
    const modalBody = document.getElementById("confirmModalBody");
    modalBody.textContent = "Etes-vous sûr de vouloir supprimer ce forage et tout ce qu'il contient ?";

    const confirmModal = new bootstrap.Modal(document.getElementById("confirmModal"));
    confirmModal.show();

    document.getElementById("confirmModalYes").onclick = function () {
        fetch(`/api/ConstructionSite/${currentConstructionSite.RefChantier}/forage/${refForage}`, {
            method: 'DELETE'
        })
            .then(response => response.json())
            .then(data => {
                confirmModal.hide();
                loadForages(currentConstructionSite.RefChantier); // Recharger les forages
                alert("Forage supprimé avec succès !");
            })
            .catch(error => {
                console.error("Erreur lors de la suppression :", error);
                alert("Erreur lors de la suppression du forage.");
            });
    };
}

// Récupérer l'ID du ConstructionSite depuis l'URL
function getConstructionSiteIdFromUrl() {
    const params = new URLSearchParams(window.location.search);
    return params.get('ConstructionSiteId') || 'default'; // À adapter selon votre besoin
}