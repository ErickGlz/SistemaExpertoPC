using SistemaExpertoPC.Models;

namespace SistemaExpertoPC.Services;


public class MotorInferencia
{
    private readonly List<Regla> _baseConocimientos;

    public MotorInferencia()
    {
        _baseConocimientos = CrearBaseConocimientos();
    }

    public IReadOnlyList<Regla> BaseConocimientos => _baseConocimientos;

   
    public List<Diagnostico> Evaluar(IEnumerable<string> sintomasSeleccionados)
    {
        Dictionary<string, bool> hechos = CatalogoSintomas.Todos.ToDictionary(s => s.Clave, _ => false);
        foreach (string clave in sintomasSeleccionados)
        {
            if (hechos.ContainsKey(clave))
                hechos[clave] = true;
        }

        List<Regla> activadas = _baseConocimientos.Where(r => r.SeCumple(hechos)).ToList();

      
        return activadas
            .GroupBy(r => r.Problema)
            .OrderByDescending(g => g.Max(r => r.Condiciones.Count))
            .Select(CrearDiagnostico)
            .ToList();
    }

    private static Diagnostico CrearDiagnostico(IGrouping<string, Regla> grupo)
    {
        Regla principal = grupo.OrderByDescending(r => r.Condiciones.Count).First();

        List<string> recomendaciones = grupo.Select(r => r.Recomendacion).Distinct().ToList();
        List<string> reglas = grupo.Select(DescribirRegla).ToList();

        return new Diagnostico(grupo.Key, principal.Explicacion, recomendaciones, reglas);
    }

    private static string DescribirRegla(Regla regla)
    {
        string condiciones = string.Join(" Y ", regla.Condiciones.Select(CatalogoSintomas.Etiqueta));
        return $"{regla.Nombre}: SI {condiciones} ENTONCES {regla.Problema.ToLowerInvariant()}";
    }

    private static List<Regla> CrearBaseConocimientos()
    {
        return new List<Regla>
        {
            new("R01 - Sobrecalentamiento con apagado",
                new[] { ClavesSintoma.Sobrecalienta, ClavesSintoma.ApagaSola },
                "Posible sobrecalentamiento del equipo",
                "Cuando el procesador alcanza una temperatura muy alta, la computadora se apaga sola como medida de protección.",
                "Limpie el polvo de ventiladores y rejillas, verifique que los ventiladores giren y considere cambiar la pasta térmica."),

            new("R02 - Lentitud con ruidos",
                new[] { ClavesSintoma.Lenta, ClavesSintoma.Ruidos },
                "Posible problema de rendimiento o hardware",
                "La lentitud acompañada de ruidos inusuales puede indicar un disco duro desgastado o un ventilador en mal estado.",
                "Respalde su información cuanto antes, revise el estado del disco duro y verifique los ventiladores."),

            new("R03 - Sin conexión a Internet",
                new[] { ClavesSintoma.SinInternet },
                "Revisar la conexión de red",
                "Sin acceso a Internet, la causa suele estar en el cable, el Wi-Fi, el router o la configuración de red.",
                "Verifique cables y Wi-Fi, reinicie el router y ejecute el solucionador de problemas de red de Windows."),

            new("R04 - Pantalla azul",
                new[] { ClavesSintoma.PantallaAzul },
                "Investigar errores del sistema",
                "Una pantalla azul indica un error grave del sistema operativo, de un controlador o del hardware.",
                "Anote el código de error, actualice controladores y Windows, y revise la memoria RAM y el disco."),

            new("R05 - Sin encendido y sin imagen",
                new[] { ClavesSintoma.NoEnciende, ClavesSintoma.SinImagen },
                "Posible falla de alimentación o hardware interno",
                "Si el equipo no enciende y tampoco muestra imagen, es probable que no esté recibiendo energía o que falle un componente interno.",
                "Revise la alimentación y el hardware: fuente de poder, memoria RAM y conexiones internas."),

            new("R06 - Lentitud con calor",
                new[] { ClavesSintoma.Lenta, ClavesSintoma.Sobrecalienta },
                "Posible reducción de rendimiento por temperatura",
                "Cuando el procesador se calienta demasiado, reduce su velocidad para protegerse y el equipo se vuelve lento.",
                "Mejore la ventilación del equipo, limpie el polvo y cierre programas que consumen muchos recursos."),

            new("R07 - Pantalla azul con ruidos",
                new[] { ClavesSintoma.PantallaAzul, ClavesSintoma.Ruidos },
                "Posible falla del disco duro o de la memoria",
                "Los errores del sistema junto con ruidos inusuales pueden señalar un disco duro dañado.",
                "Respalde sus archivos, revise el disco con herramientas de diagnóstico y pruebe la memoria RAM."),

            new("R08 - Pantalla azul con apagado",
                new[] { ClavesSintoma.PantallaAzul, ClavesSintoma.ApagaSola },
                "Posible inestabilidad por hardware o controladores",
                "Errores del sistema junto con apagados repentinos suelen deberse a RAM defectuosa, controladores o fuente de poder inestable.",
                "Actualice controladores, pruebe la memoria RAM y verifique la fuente de poder."),

            new("R09 - Calor con ruidos",
                new[] { ClavesSintoma.Sobrecalienta, ClavesSintoma.Ruidos },
                "Posible falla del sistema de ventilación",
                "El calor excesivo con ruidos inusuales sugiere un ventilador sucio, desgastado o con un rodamiento dañado.",
                "Limpie los ventiladores y reemplácelos si hacen ruido constante."),

            new("R10 - No enciende",
                new[] { ClavesSintoma.NoEnciende },
                "Posible falla de alimentación o hardware interno",
                "Si el equipo no enciende, lo primero que se debe descartar es que no le llegue corriente eléctrica.",
                "Compruebe el cable, el enchufe, la regleta y el botón de encendido; pruebe con otro cable de corriente."),

            new("R11 - Sin imagen",
                new[] { ClavesSintoma.SinImagen },
                "Posible problema de video o del monitor",
                "Si no hay imagen, la falla puede estar en el monitor, el cable de video o la tarjeta gráfica.",
                "Revise el cable de video, pruebe otro monitor y vuelva a conectar la tarjeta gráfica o la memoria RAM."),

            new("R12 - Lentitud con problemas de red",
                new[] { ClavesSintoma.Lenta, ClavesSintoma.SinInternet },
                "Posible software no deseado o falla de configuración",
                "Un equipo lento y sin Internet puede tener programas en segundo plano, malware o una configuración de red dañada.",
                "Ejecute un análisis antivirus, revise los programas de inicio y restablezca la configuración de red."),

            new("R13 - Funcionamiento lento",
                new[] { ClavesSintoma.Lenta },
                "Posible falta de recursos o exceso de programas en segundo plano",
                "La lentitud suele deberse a poca memoria RAM, disco casi lleno o muchos programas abiertos.",
                "Cierre programas innecesarios, libere espacio en disco y desactive aplicaciones de inicio."),

            new("R14 - Calentamiento",
                new[] { ClavesSintoma.Sobrecalienta },
                "Posible refrigeración deficiente",
                "El calentamiento excesivo normalmente se debe a polvo acumulado o a mala ventilación.",
                "Use el equipo en una superficie firme y ventilada y limpie las rejillas de aire."),

            new("R15 - Apagados repentinos",
                new[] { ClavesSintoma.ApagaSola },
                "Posible falla en el suministro de energía",
                "Los apagados inesperados pueden deberse a una fuente de poder defectuosa, a una batería dañada o a cables flojos.",
                "Revise la batería, el cargador y las conexiones eléctricas, y pruebe con otra toma de corriente."),
        };
    }
}
