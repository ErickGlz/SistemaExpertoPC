using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SistemaExpertoPC.Models;

/// <summary>
/// Claves (identificadores) de los 8 síntomas que reconoce el sistema.
/// Las reglas y el motor usan estas claves como "hechos".
/// </summary>
public static class ClavesSintoma
{
    public const string NoEnciende = "NoEnciende";
    public const string Lenta = "Lenta";
    public const string Sobrecalienta = "Sobrecalienta";
    public const string SinInternet = "SinInternet";
    public const string PantallaAzul = "PantallaAzul";
    public const string Ruidos = "Ruidos";
    public const string SinImagen = "SinImagen";
    public const string ApagaSola = "ApagaSola";
}

/// <summary>Definición fija de un síntoma: clave, texto completo y etiqueta corta.</summary>
public record DefinicionSintoma(string Clave, string Texto, string Etiqueta);

/// <summary>Catálogo con todos los síntomas disponibles.</summary>
public static class CatalogoSintomas
{
    public static readonly IReadOnlyList<DefinicionSintoma> Todos = new List<DefinicionSintoma>
    {
        new(ClavesSintoma.NoEnciende,    "La computadora no enciende",                  "No enciende"),
        new(ClavesSintoma.Lenta,         "La computadora funciona lentamente",          "Funciona lenta"),
        new(ClavesSintoma.Sobrecalienta, "La computadora se sobrecalienta",             "Se sobrecalienta"),
        new(ClavesSintoma.SinInternet,   "La computadora no tiene conexión a Internet", "Sin Internet"),
        new(ClavesSintoma.PantallaAzul,  "Aparece una pantalla azul",                   "Pantalla azul"),
        new(ClavesSintoma.Ruidos,        "La computadora produce ruidos inusuales",     "Ruidos inusuales"),
        new(ClavesSintoma.SinImagen,     "La computadora no muestra imagen",            "No muestra imagen"),
        new(ClavesSintoma.ApagaSola,     "La computadora se apaga sola",                "Se apaga sola"),
    };

    /// <summary>Devuelve la etiqueta corta de un síntoma a partir de su clave.</summary>
    public static string Etiqueta(string clave) =>
        Todos.FirstOrDefault(s => s.Clave == clave)?.Etiqueta ?? clave;
}

/// <summary>
/// Síntoma mostrado en la interfaz como una casilla de verificación.
/// </summary>
public class Sintoma : INotifyPropertyChanged
{
    private bool _seleccionado;

    public Sintoma(string clave, string descripcion)
    {
        Clave = clave;
        Descripcion = descripcion;
    }

    public string Clave { get; }
    public string Descripcion { get; }

    public bool Seleccionado
    {
        get => _seleccionado;
        set
        {
            if (_seleccionado == value) return;
            _seleccionado = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? nombre = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
}
