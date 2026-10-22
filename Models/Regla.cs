namespace SistemaExpertoPC.Models;


public class Regla
{
    public Regla(string nombre, string[] condiciones, string problema, string explicacion, string recomendacion)
    {
        Nombre = nombre;
        Condiciones = condiciones;
        Problema = problema;
        Explicacion = explicacion;
        Recomendacion = recomendacion;
    }

    public string Nombre { get; }

    public IReadOnlyList<string> Condiciones { get; }

    public string Problema { get; }

    public string Explicacion { get; }

    public string Recomendacion { get; }

  
    public bool SeCumple(IReadOnlyDictionary<string, bool> hechos) =>
        Condiciones.All(c => hechos.TryGetValue(c, out var presente) && presente);
}
