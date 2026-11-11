using System;
using System.Collections.Generic;

namespace GaiaPressBlazorWebAPP.Services
{
    public class LocalizationService
    {
        public event Action? LanguageChanged;

        public string CurrentLanguage { get; private set; } = "en";

        public IReadOnlyList<string> AvailableLanguages { get; } = new[] { "en", "fr" };

        private static readonly Dictionary<string, Dictionary<string, string>> _texts = new()
        {
            ["en"] = new()
            {
                ["About"] = "About",
                ["AuthRequired"] = "Auth required",
                ["Borehole"] = "Borehole",
                ["CloseFile"] = "Close File",
                ["Counter"] = "Counter",
                ["Dashboard"] = "Dashboard",
                ["File"] = "File",
                ["HelloWorld"] = "Hello World",
                ["HelloWorldDescription"] = "Program for the interpretation of the pressuremeter test to find friction angle and/or cohesion.",
                ["HelloWorldTitle"] = "Hello, world!",
                ["Home"] = "Home",
                ["Language"] = "Language",
                ["Languages"] = "Languages : English–French",
                ["Login"] = "Login",
                ["NewFile"] = "New File",
                ["OpenFile"] = "Open File",
                ["Probe"] = "Probe",
                ["Quit"] = "Quit",
                ["RequestedPlatform"] = "Requested platform : Windows 11",
                ["Register"] = "Register",
                ["SaveFile"] = "Save File",
                ["SaveUnderFile"] = "Save Under File",
                ["Soil"] = "Soil",
                ["Test"] = "Test",
                ["Version"] = "Version : x64 bit",
                ["Weather"] = "Weather",
                ["Actions"] = "Actions",
                ["Cancel"] = "Cancel",
                ["ConfirmNewFile"] = "A file is already open. Do you want to continue?",
                ["ConfirmQuit"] = "Are you sure you want to quit the application?",
                ["Connect"] = "Connect",
                ["ConnectToDatabase"] = "Connect to DB",
                ["ConnectedToDatabase"] = "Connected to DB",
                ["ConstructionSite"] = "Construction Site",
                ["Database"] = "Database",
                ["DisconnectFromDatabase"] = "Disconnect from DB",
                ["DisconnectedFromDatabase"] = "Disconnected from DB",
                ["EditConstructionSite"] = "Edit construction site",
                ["Logout"] = "Log out",
                ["NewConstructionSite"] = "New construction site",
                ["OpenConstructionSite"] = "Open construction site",
                ["Password"] = "Password",
                ["Server"] = "Server",
                ["Username"] = "Username",
                ["ViewBoreholes"] = "View boreholes",
            },
            ["fr"] = new()
            {
                ["About"] = "À propos",
                ["AuthRequired"] = "Authentification requise",
                ["Borehole"] = "Forage",
                ["CloseFile"] = "Fermer le fichier",
                ["Counter"] = "Compteur",
                ["Dashboard"] = "Tableau de bord",
                ["File"] = "Fichier",
                ["HelloWorld"] = "Bonjour le monde",
                ["HelloWorldDescription"] = "Programme d'interprétation de l'Test au pressiomètre pour déterminer l'angle de frottement et/ou la cohésion.",
                ["HelloWorldTitle"] = "Bonjour, le monde !",
                ["Home"] = "Accueil",
                ["Language"] = "Langue",
                ["Languages"] = "Langues : anglais–français",
                ["Login"] = "Connexion",
                ["NewFile"] = "Nouveau fichier",
                ["OpenFile"] = "Ouvrir un fichier",
                ["Probe"] = "Sonde",
                ["Quit"] = "Quitter",
                ["RequestedPlatform"] = "Plateforme demandée : Windows 11",
                ["Register"] = "Inscription",
                ["SaveFile"] = "Enregistrer le fichier",
                ["SaveUnderFile"] = "Enregistrer sous",
                ["Soil"] = "Sol",
                ["Test"] = "Test",
                ["Version"] = "Version : x64 bits",
                ["Weather"] = "Météo",
                ["Actions"] = "Actions",
                ["Cancel"] = "Annuler",
                ["ConfirmNewFile"] = "Un fichier est déjà ouvert. Voulez-vous continuer ?",
                ["ConfirmQuit"] = "Voulez-vous vraiment quitter l'application ?",
                ["Connect"] = "Se connecter",
                ["ConnectToDatabase"] = "Se connecter à la Bd",
                ["ConnectedToDatabase"] = "Connecté à la Bd",
                ["ConstructionSite"] = "ConstructionSite",
                ["Database"] = "Base",
                ["DisconnectFromDatabase"] = "Se déconnecter de la Bd",
                ["DisconnectedFromDatabase"] = "Déconnecté de la Bd",
                ["EditConstructionSite"] = "Modifier le ConstructionSite",
                ["Logout"] = "Se déconnecter",
                ["NewConstructionSite"] = "Nouveau ConstructionSite",
                ["OpenConstructionSite"] = "Ouvrir un ConstructionSite",
                ["Password"] = "Mot de passe",
                ["Server"] = "Serveur",
                ["Username"] = "Identifiant",
                ["ViewBoreholes"] = "Voir les forages",
            }
        };

        public string this[string key] =>
            _texts[CurrentLanguage].TryGetValue(key, out var value) ? value : key;

        public void SetLanguage(string language)
        {
            if (!_texts.ContainsKey(language) || language == CurrentLanguage) return;
            CurrentLanguage = language;
            LanguageChanged?.Invoke();
        }
    }
}