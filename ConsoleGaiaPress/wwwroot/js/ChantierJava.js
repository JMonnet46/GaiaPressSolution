// Variables globales
let currentChantier = null;
let currentForageIndex = null;
let lang = 1; // 1 = Français, 0 = Anglais (à adapter selon votre besoin)

// Initialisation au chargement de la page
document.addEventListener("DOMContentLoaded", function () {
    // Charger les données du chantier
    loadChantier();

    // Initialiser les événements
    initEventListeners();

    // Initialiser les onglets
    initiTabs();

    // Initialiser DataTables pour le iTableau des forages
    $('#dataGridForages').DataTable({
        columns: [
            { data: 'refForage', title: 'Référence' },
            { data: 'nomForage', title: 'Nom' },
            { data: 'dateForage', title: 'Date' }
        ],
        language: {
            url: '//cdn.DataTables.net/plug-ins/1.13.6/i18n/fr-FR.json' // Langue française
        }
    });
});

// Charger les données du chantier
function loadChantier() {
    // Récupérer l'ID du chantier depuis l'URL ou une variable globale
    const chantierId = getChantierIdFromUrl(); // À implémenter selon votre besoin

    fetch(`/api/chantier/${chantierId}`)
        .then(response => response.json())
        .then(data => {
            currentChantier = data;
            renderChantier(data);
            loadArborescence(data.refChantier);
            loadForages(data.refChantier);
        })
        .catch(error => {
            console.error("Erreur lors du chargement du chantier :", error);
            alert("Erreur lors du chargement du chantier.");
        });
}

// Rendre les données du chantier dans le formulaire
function renderChantier(chantier) {
    document.getElementById("txtRefChantier").value = chantier.refChantier || "";
    document.getElementById("txtNomChantier").value = chantier.nomChantier || "";
    document.getElementById("txtRefClient").value = chantier.refClient || "";
    document.getElementById("txtEntreprise").value = chantier.entreprise || "";
    document.getElementById("txtTelephone").value = chantier.telephone || "";
    document.getElementById("txtAdresse").value = chantier.adresse || "";
    document.getElementById("txtDescription").value = chantier.descriptifChantier || "";

    // Verrouiller les champs par défaut
    verrouilleChamps();
}

// Charger l'arborescence
function loadArborescence(refChantier) {
    fetch(`/api/chantier/${refChantier}/arborescence`)
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
function loadForages(refChantier) {
    fetch(`/api/chantier/${refChantier}/forages`)
        .then(response => response.json())
        .then(forages => {
            const iTable = $('#dataGridForages').DataTable();
            iTable.clear().rows.add(forages).draw();
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
        saveChantier();
    });

    // Bouton Annuler
    document.getElementById("btnAnnuler").addEventListener("click", function () {
        if (confirm("Voulez-vous annuler vos modifications ?")) {
            loadChantier(); // Recharger les données
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

    // Bouton Add Forage
    document.getElementById("btnAddForage").addEventListener("click", function () {
        AddForage();
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

    // Bouton Delete Forage
    document.getElementById("btnDeleteForage").addEventListener("click", function () {
        const row = $('#dataGridForages').DataTable().row('.selected').data();
        if (row) {
            DeleteForage(row.refForage);
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
function initiTabs() {
    const iTabEl = document.querySelectorAll('#chantieriTabs button');
    iTabEl.forEach(iTab => {
        iTab.addEventListener('shown.bs.iTab', event => {
            // Recharger les données si nécessaire
            if (event.target.id === 'iTabForages') {
                loadForages(currentChantier.refChantier);
            }
        });
    });
}

// Verrouiller les champs (équivalent de VerrouilleChamps)
function verrouilleChamps() {
    const fields = [
        "txtRefChantier", "txtNomChantier", "txtRefClient",
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
        "txtNomChantier", "txtRefClient", "txtEntreprise",
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

// Sauvegarder le chantier (équivalent de CmdOk_Click)
function saveChantier() {
    const chantier = {
        refChantier: document.getElementById("txtRefChantier").value,
        nomChantier: document.getElementById("txtNomChantier").value,
        refClient: document.getElementById("txtRefClient").value,
        entreprise: document.getElementById("txtEntreprise").value,
        telephone: document.getElementById("txtTelephone").value,
        adresse: document.getElementById("txtAdresse").value,
        descriptifChantier: document.getElementById("txtDescription").value
    };

    fetch('/api/chantier', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(chantier)
    })
        .then(response => response.json())
        .then(data => {
            alert("Chantier sauvegardé avec succès !");
            verrouilleChamps();
            // Recharger les données si nécessaire
            loadChantier();
        })
        .catch(error => {
            console.error("Erreur lors de la sauvegarde :", error);
            alert("Erreur lors de la sauvegarde du chantier.");
        });
}

// Imprimer le document (équivalent de CmdImprimer_Click)
// // function printDocument() {
    // Option 1 : Imprimer directement depuis le navigateur
    window.print();

    // Option 2 : Générer un PDF côté serveur et le télécharger
    // fetch(`/api/chantier/${currentChantier.refChantier}/pdf`)
    //     .then(response => response.blob())
    //     .then(blob => {
    //         const url = window.URL.createObjectURL(blob);
    //         const a = document.createElement('a');
    //         a.href = url;
    //         a.download = `Chantier_${currentChantier.refChantier}.pdf`;
    //         a.click();
    //         window.URL.revokeObjectURL(url);
    //     });
}

// Add un forage (équivalent de CmdAdd_Click)
function AddForage() {
    // Ouvrir une nouvelle fenêtre ou une modale pour Add un forage
    // Exemple : Rediriger vers une page de création de forage
    window.location.href = `/forage.html?chantierId=${currentChantier.refChantier}`;
}

// Ouvrir un forage (équivalent de CmdOuvrir_Click)
function ouvrirForage(refForage) {
    window.location.href = `/forage.html?chantierId=${currentChantier.refChantier}&forageId=${refForage}`;
}

// Delete un forage (équivalent de CmdDelete_Click)
function DeleteForage(refForage) {
    const modalBody = document.getElementById("confirmModalBody");
    modalBody.textContent = "Etes-vous sûr de vouloir Delete ce forage et tout ce qu'il contient ?";

    const confirmModal = new bootstrap.Modal(document.getElementById("confirmModal"));
    confirmModal.show();

    document.getElementById("confirmModalYes").onclick = function () {
        fetch(`/api/chantier/${currentChantier.refChantier}/forage/${refForage}`, {
            method: 'DELETE'
        })
            .then(response => response.json())
            .then(data => {
                confirmModal.hide();
                loadForages(currentChantier.refChantier); // Recharger les forages
                alert("Forage supprimé avec succès !");
            })
            .catch(error => {
                console.error("Erreur lors de la suppression :", error);
                alert("Erreur lors de la suppression du forage.");
            });
    };
}

// Récupérer l'ID du chantier depuis l'URL
function getChantierIdFromUrl() {
    const params = new URLSearchParams(window.location.search);
    return params.get('chantierId') || 'default'; // À adapter selon votre besoin
}