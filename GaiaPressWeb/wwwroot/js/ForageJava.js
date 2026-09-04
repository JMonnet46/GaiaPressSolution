// ✅ Fonction pour l'action Modify
function modifyForage() {
    console.log("Action Modify: Modification des paramètres du forage.");
    // Exemple: Récupérer les valeurs du formulaire et les envoyer au backend
    const forageData = {
        boreholeDate: document.getElementById("boreholeDate").value,
        drillingEquipment: document.getElementById("drillingEquipment").value,
        drillingTool: document.getElementById("drillingTool").value,
        xCoordinate: document.getElementById("xCoordinate").value,
        yCoordinate: document.getElementById("yCoordinate").value,
        zAltitude: document.getElementById("zAltitude").value,
        initialElevation: document.getElementById("initialElevation").value,
        finalElevation: document.getElementById("finalElevation").value
    };
    console.log("Données du forage à modifier :", forageData);
    // Exemple: Envoyer les données au backend via fetch
    // fetch("/Forage/Modify", { method: "POST", body: JSON.stringify(forageData) });
}

// ✅ Fonction pour l'action OK
function validateForage() {
    console.log("Action OK: Validation des paramètres du forage.");
    // Exemple: Récupérer les valeurs du formulaire et les envoyer au backend
    const forageData = {
        boreholeDate: document.getElementById("boreholeDate").value,
        drillingEquipment: document.getElementById("drillingEquipment").value,
        drillingTool: document.getElementById("drillingTool").value,
        xCoordinate: document.getElementById("xCoordinate").value,
        yCoordinate: document.getElementById("yCoordinate").value,
        zAltitude: document.getElementById("zAltitude").value,
        initialElevation: document.getElementById("initialElevation").value,
        finalElevation: document.getElementById("finalElevation").value
    };
    console.log("Données du forage validées :", forageData);
    // Exemple: Envoyer les données au backend via fetch
    // fetch("/Forage/Validate", { method: "POST", body: JSON.stringify(forageData) });
}

// ✅ Fonction pour l'action Cancel
function cancelForage() {
    console.log("Action Cancel: Annulation des modifications.");
    // Exemple: Réinitialiser le formulaire
    document.getElementById("boreholeDate").value = "2026-08-05";
    document.getElementById("drillingEquipment").value = "Camion de forage X";
    document.getElementById("drillingTool").value = "Trépan de 100mm";
    document.getElementById("xCoordinate").value = "100";
    document.getElementById("yCoordinate").value = "200";
    document.getElementById("zAltitude").value = "50";
    document.getElementById("initialElevation").value = "0";
    document.getElementById("finalElevation").value = "10";
}
// ✅ Fonction pour fermer la fenêtre du navigateur
function closeWindow() {
    window.close(); // ✅ Ferme la fenêtre du navigateur
}