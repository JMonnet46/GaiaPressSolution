namespace GaiaPressCoreV17.Models.Dto
{
    public class ForagesDto
    {
        public string refChantier { get; set; }
        public string refForage { get; set; }
        public int numeroForage { get; set; }
        public string nomForage { get; set; }
        public DateTime dateForage { get; set; }
        public string materiel { get; set; }
        public string outil { get; set; }
        public float abscisseX { get; set; }
        public float ordonneeY { get; set; }
        public float altitudeZ { get; set; }
        public float coteInit { get; set; }
        public float coteFinale { get; set; }
    }
}