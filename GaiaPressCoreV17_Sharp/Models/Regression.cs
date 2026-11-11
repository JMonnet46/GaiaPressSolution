namespace [PROJET].Logic
{
    public class Regression
{
    public float A { get; set; }      // Pente
    public float B { get; set; }      // Ordonnée à l'origine (attention : 'b' en VB)
    public bool Erreur { get; set; }

    public Regression()
    {
        A = 0;
        B = 0;
        Erreur = false;
    }
}
}