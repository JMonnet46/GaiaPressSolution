namespace [PROJET].Models
{
    public class Cisaillement
{
    public float Plim { get; set; }
    public float Pf { get; set; }
    public float Coh { get; set; }
    public float Phi { get; set; }
    public bool Erreur { get; set; }

    public Cisaillement()
    {
        Plim = 0;
        Pf = 0;
        Coh = 0;
        Phi = 0;
        Erreur = false;
    }
}
}