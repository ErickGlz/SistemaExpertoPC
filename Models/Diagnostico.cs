namespace SistemaExpertoPC.Models;

/// <summary>
/// Resultado del motor de inferencia: un problema posible, su explicación,
/// las recomendaciones y las reglas que lo activaron.
/// </summary>
public class Diagnostico
{
    public Diagnostico(
        string problema,
        string explicacion,
        IReadOnlyList<string> recomendaciones,
        IReadOnlyList<string> reglasActivadas)
    {
        Problema = problema;
        Explicacion = explicacion;
        Recomendaciones = recomendaciones;
        ReglasActivadas = reglasActivadas;
    }

    public string Problema { get; }
    public string Explicacion { get; }
    public IReadOnlyList<string> Recomendaciones { get; }
    public IReadOnlyList<string> ReglasActivadas { get; }
}
