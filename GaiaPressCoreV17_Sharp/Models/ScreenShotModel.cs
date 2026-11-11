using GaiaPressCoreV17.Services;
using GaiaPressCoreV17.Logic;

namespace GaiaPressCoreV17.Models
{
    public class ScreenshotModel
    {
        public string Image { get; set; }      // Ajoutez cette ligne
        public byte[] ImageData { get; set; }
        public string Format { get; set; }
    }
}