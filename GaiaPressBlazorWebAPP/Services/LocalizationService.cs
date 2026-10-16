using System.Globalization;

namespace GaiaPressBlazorWebAPP.Services
{
    public class LocalizationService
    {
        // Événement déclenché quand la langue change
        public event Action? LanguageChanged;

        // Langue actuelle
        public string CurrentLanguage { get; private set; } = "fr";

        // Dictionnaire des traductions
        private readonly Dictionary<string, Dictionary<string, string>> _translations = new()
        {
            ["fr"] = new Dictionary<string, string>
            {
                // Page d'accueil
                ["HelloWorldTitle"] = "Bienvenue sur GaiaPress",
                ["HelloWorldDescription"] = "GaiaPress est votre solution avancée pour l'interprétation des essais pressiométriques, conçue pour déterminer efficacement l'angle de frottement et la cohésion des sols. Que vous soyez ingénieur géotechnicien, chercheur ou étudiant, notre logiciel intuitif optimisera votre travail et vous fournira des résultats précis.",
                ["HelloWorldFonctionnalite"] = "Fonctionnalités clés :",
                ["HelloWorldFeature1"] = "✓ Interface conviviale – Facile à naviguer, même pour les débutants.",
                ["HelloWorldFeature2"] = "✓ Outils d'analyse puissants – Obtenez des valeurs précises de l'angle de frottement et de la cohésion.",
                ["HelloWorldFeature3"] = "✓ Support multilingue – Disponible en anglais et en français.",
                ["HelloWorldFeature4"] = "✓ Compatibilité multiplateforme – Fonctionne parfaitement sur Windows 11",
                ["HelloWorldFeature5"] = "Commencez à explorer GaiaPress dès aujourd'hui et découvrez tout le potentiel de vos essais pressiométriques !",
                ["RequestedPlatform"] = "Requested platform: Windows 11",
                ["Version"] = "Version x64 bits",
                ["Languages"] = "Anglais – Français",

                // Navigation
                ["Home"] = "Accueil",
                ["Borehole"] = "Forage",
                ["ConstructionSite"] = "Chantier",
                ["Tests"] = "Essais",
                ["File"] = "Fichier",
                ["Language"] = "Langue",
                ["LanguageSelection"] = "Sélection de la langue",

                // Menu Fichier
                ["New"] = "Nouveau",
                ["Open"] = "Ouvrir",
                ["Save"] = "Enregistrer",
                ["SaveAs"] = "Enregistrer sous...",
                ["Export"] = "Exporter",
                ["Import"] = "Importer",
                ["Exit"] = "Quitter",

                // Actions générales
                ["Add"] = "Ajouter",
                ["Edit"] = "Modifier",
                ["Delete"] = "Supprimer",
                ["Cancel"] = "Annuler",
                ["Confirm"] = "Confirmer",
                ["Close"] = "Fermer",
                ["Search"] = "Rechercher",
                ["Filter"] = "Filtrer",
                ["Refresh"] = "Actualiser",

                // Messages
                ["Loading"] = "Chargement...",
                ["Saving"] = "Enregistrement...",
                ["Success"] = "Succès",
                ["Error"] = "Erreur",
                ["Warning"] = "Attention",
                ["Info"] = "Information",

                // Formulaires
                ["Name"] = "Nom",
                ["Description"] = "Description",
                ["Date"] = "Date",
                ["Type"] = "Type",
                ["Value"] = "Valeur",
                ["Unit"] = "Unité",

                // Chantier
                ["SiteName"] = "Nom du chantier",
                ["SiteLocation"] = "Localisation",
                ["Client"] = "Client",
                ["Project"] = "Projet",

                // Forage
                ["BoreholeName"] = "Nom du forage",
                ["Depth"] = "Profondeur",
                ["Diameter"] = "Diamètre",
                ["Coordinates"] = "Coordonnées",

                // Essais
                ["TestType"] = "Type d'essai",
                ["TestDate"] = "Date d'essai",
                ["TestResult"] = "Résultat",
                ["Pressure"] = "Pression",
                ["Volume"] = "Volume",

                // À propos
                ["AboutTitle"] = "À propos de GaiaPress",
                ["AboutDescription"] = "Application de gestion géotechnique",
                ["AboutPlatform"] = "Plateforme : Blazor WebAssembly",
                ["AboutTechnology"] = "Technologie : .NET 8",
                ["AboutVersion"] = "Version : 1.0.0",
            },

            ["en"] = new Dictionary<string, string>
            {
                // Home page
                ["HelloWorld"] = "Welcome to GaiaPress",
                ["HelloWorldTitle"] = "Welcome to GaiaPress",
                ["HelloWorldDescription"] = "GaiaPress is your advanced solution for pressuremeter test interpretation, designed to efficiently determine the friction angle and cohesion of soils. Whether you are a geotechnical engineer, researcher, or student, our intuitive software will optimize your work and provide you with accurate results.",
                ["HelloWorldFonctionnalite"] = "Key features:",
                ["HelloWorldFeature1"] = "✓ User-friendly interface – Easy to navigate, even for beginners.",
                ["HelloWorldFeature2"] = "✓ Powerful analysis tools – Get accurate values for friction angle and cohesion.",
                ["HelloWorldFeature3"] = "✓ Multilingual support – Available in English and French.",
                ["HelloWorldFeature4"] = "✓ Cross-platform compatibility – Works perfectly on Windows 11.",
                ["HelloWorldFeature5"] = "Start exploring GaiaPress today and discover the full potential of your pressuremeter tests!",
                ["RequestedPlatform"] = "Requested platform: Windows 11",
                ["Version"] = "x64 bit version",
                ["Languages"] = "English – French",

                // Navigation
                ["Home"] = "Home",
                ["Borehole"] = "Borehole",
                ["ConstructionSite"] = "Site",
                ["Tests"] = "Tests",
                ["File"] = "File",
                ["Language"] = "Language",
                ["LanguageSelection"] = "Language selection",

                // Menu Fichier
                ["New"] = "New",
                ["Open"] = "Open",
                ["Save"] = "Save",
                ["SaveAs"] = "Save as...",
                ["Export"] = "Export",
                ["Import"] = "Import",
                ["Exit"] = "Exit",

                // Actions générales
                ["Add"] = "Add",
                ["Edit"] = "Edit",
                ["Delete"] = "Delete",
                ["Cancel"] = "Cancel",
                ["Confirm"] = "Confirm",
                ["Close"] = "Close",
                ["Search"] = "Search",
                ["Filter"] = "Filter",
                ["Refresh"] = "Refresh",

                // Messages
                ["Loading"] = "Loading...",
                ["Saving"] = "Saving...",
                ["Success"] = "Success",
                ["Error"] = "Error",
                ["Warning"] = "Warning",
                ["Info"] = "Information",

                // Formulaires
                ["Name"] = "Name",
                ["Description"] = "Description",
                ["Date"] = "Date",
                ["Type"] = "Type",
                ["Value"] = "Value",
                ["Unit"] = "Unit",

                // Chantier
                ["SiteName"] = "Site name",
                ["SiteLocation"] = "Location",
                ["Client"] = "Client",
                ["Project"] = "Project",

                // Forage
                ["BoreholeName"] = "Borehole name",
                ["Depth"] = "Depth",
                ["Diameter"] = "Diameter",
                ["Coordinates"] = "Coordinates",

                // Essais
                ["TestType"] = "Test type",
                ["TestDate"] = "Test date",
                ["TestResult"] = "Result",
                ["Pressure"] = "Pressure",
                ["Volume"] = "Volume",

                // À propos
                ["AboutTitle"] = "About GaiaPress",
                ["AboutDescription"] = "Geotechnical management application",
                ["AboutPlatform"] = "Platform: Blazor WebAssembly",
                ["AboutTechnology"] = "Technology: .NET 8",
                ["AboutVersion"] = "Version: 1.0.0",
            }
        };

        // Indexeur pour accéder aux traductions : Loc["Key"]
        public string this[string key]
        {
            get
            {
                if (_translations.TryGetValue(CurrentLanguage, out var langDict))
                {
                    if (langDict.TryGetValue(key, out var translation))
                    {
                        return translation;
                    }
                }

                // Fallback sur le français si la clé n'existe pas dans la langue actuelle
                if (_translations.TryGetValue("fr", out var frDict) && frDict.TryGetValue(key, out var frTranslation))
                {
                    return frTranslation;
                }

                // Si la clé n'existe nulle part, retourner la clé elle-même
                return key;
            }
        }

        // Changer la langue
        public void SetLanguage(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode))
                return;

            var normalizedCode = languageCode.ToLowerInvariant();

            // Vérifier que la langue est supportée
            if (!_translations.ContainsKey(normalizedCode))
            {
                // Fallback sur le français
                normalizedCode = "fr";
            }

            if (CurrentLanguage != normalizedCode)
            {
                CurrentLanguage = normalizedCode;

                // Définir la culture pour le formatage des dates/nombres
                CultureInfo.CurrentCulture = new CultureInfo(normalizedCode);
                CultureInfo.CurrentUICulture = new CultureInfo(normalizedCode);

                // Déclencher l'événement
                OnLanguageChanged();
            }
        }

        // Méthode protégée pour déclencher l'événement
        protected virtual void OnLanguageChanged()
        {
            LanguageChanged?.Invoke();
        }

        // Obtenir la liste des langues disponibles
        public IEnumerable<string> GetAvailableLanguages()
        {
            return _translations.Keys;
        }

        // Vérifier si une clé existe
        public bool HasKey(string key)
        {
            return _translations.TryGetValue(CurrentLanguage, out var langDict) && langDict.ContainsKey(key);
        }
    }
}