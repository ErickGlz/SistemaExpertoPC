using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SistemaExpertoPC.Commands;
using SistemaExpertoPC.Models;
using SistemaExpertoPC.Services;

namespace SistemaExpertoPC.ViewModels;

/// <summary>
/// ViewModel de la ventana principal: conecta la interfaz con el motor de inferencia.
/// </summary>
public class MainViewModel : INotifyPropertyChanged
{
    private const string MensajeInicial =
        "Seleccione uno o más síntomas y pulse «Analizar síntomas» para obtener un diagnóstico orientativo.";
    private const string MensajeSinSeleccion =
        "No ha seleccionado ningún síntoma. Marque al menos uno y vuelva a pulsar «Analizar síntomas».";
    private const string MensajeSinCoincidencia =
        "Ninguna regla de la base de conocimientos coincide con los síntomas seleccionados. " +
        "Pruebe con otra combinación o consulte a un técnico.";

    private readonly MotorInferencia _motor = new();
    private string _mensaje = MensajeInicial;
    private bool _mostrarMensaje = true;

    public MainViewModel()
    {
        Sintomas = new ObservableCollection<Sintoma>(
            CatalogoSintomas.Todos.Select(d => new Sintoma(d.Clave, d.Texto)));

        AnalizarCommand = new RelayCommand(_ => Analizar());
        LimpiarCommand = new RelayCommand(_ => Limpiar());
    }

    public ObservableCollection<Sintoma> Sintomas { get; }

    public ObservableCollection<Diagnostico> Diagnosticos { get; } = new();

    public ICommand AnalizarCommand { get; }

    public ICommand LimpiarCommand { get; }

    /// <summary>Mensaje informativo (inicio, sin selección o sin coincidencias).</summary>
    public string Mensaje
    {
        get => _mensaje;
        private set => SetProperty(ref _mensaje, value);
    }

    /// <summary>Indica si se debe mostrar el mensaje en lugar de los diagnósticos.</summary>
    public bool MostrarMensaje
    {
        get => _mostrarMensaje;
        private set => SetProperty(ref _mostrarMensaje, value);
    }

    private void Analizar()
    {
        Diagnosticos.Clear();

        // 1. Leer los síntomas seleccionados.
        List<string> seleccionados = Sintomas
            .Where(s => s.Seleccionado)
            .Select(s => s.Clave)
            .ToList();

        if (seleccionados.Count == 0)
        {
            MostrarTexto(MensajeSinSeleccion);
            return;
        }

        // 2. Pedir al motor que evalúe la base de conocimientos.
        List<Diagnostico> resultados = _motor.Evaluar(seleccionados);

        if (resultados.Count == 0)
        {
            MostrarTexto(MensajeSinCoincidencia);
            return;
        }

        // 3. Mostrar los diagnósticos encontrados.
        foreach (Diagnostico diagnostico in resultados)
            Diagnosticos.Add(diagnostico);

        MostrarMensaje = false;
    }

    private void Limpiar()
    {
        foreach (Sintoma sintoma in Sintomas)
            sintoma.Seleccionado = false;

        Diagnosticos.Clear();
        MostrarTexto(MensajeInicial);
    }

    private void MostrarTexto(string texto)
    {
        Mensaje = texto;
        MostrarMensaje = true;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetProperty<T>(ref T campo, T valor, [CallerMemberName] string? nombre = null)
    {
        if (EqualityComparer<T>.Default.Equals(campo, valor)) return;
        campo = valor;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
