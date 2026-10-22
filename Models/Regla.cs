namespace SistemaExpertoPC.Models;

/// <summary>
/// Una regla de la base de conocimientos:
/// SI (todas las condiciones se cumplen) ENTONCES (problema identificado).
/// </summary>
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

    /// <summary>Nombre de la regla (por ejemplo "R01 - Sobrecalentamiento con apagado").</summary>
    public string Nombre { get; }

    /// <summary>Claves de los síntomas que deben estar presentes (unidas con Y).</summary>
    public IReadOnlyList<string> Condiciones { get; }

    /// <summary>Problema que se identifica cuando la regla se activa.</summary>
    public string Problema { get; }

    /// <summary>Explicación breve de por qué se llega a ese problema.</summary>
    public string Explicacion { get; }

    /// <summary>Acción recomendada al usuario.</summary>
    public string Recomendacion { get; }

    /// <summary>
    /// Evalúa la regla contra los hechos actuales (síntoma -> presente o no).
    /// Se cumple si TODAS sus condiciones son verdaderas.
    /// </summary>
    public bool SeCumple(IReadOnlyDictionary<string, bool> hechos) =>
        Condiciones.All(c => hechos.TryGetValue(c, out var presente) && presente);
}
