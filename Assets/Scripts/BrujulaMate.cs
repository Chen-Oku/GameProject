using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Sistema de brújula matemático que no depende de objetos 3D.
/// Puede usar UI o simplemente cálculos para proporcionar información de dirección.
/// </summary>
public class BrujulaMate : MonoBehaviour
{
    public static BrujulaMate instance;

    [Header("Configuración")]
    public Transform objetivo;                // Objetivo hacia donde apuntar
    public Transform jugador;                 // Posición del jugador/cámara
    public string tagJugador = "Player";     // Tag para encontrar al jugador automáticamente
    public string tagObjetivo = "Objetivo";  // Tag para encontrar el objetivo automáticamente

    [Header("Visualización")]
    public bool usarUI = false;                    // Si es true, actualizará un elemento UI
    public Image imagenFlechaUI;                   // Imagen UI que servirá como flecha
    public Transform flechaTransform;              // Alternativa: un transform que rotaremos
    public bool mostrarSoloEnCiertoRango = false;  // Si es verdadero, solo muestra la dirección en cierto rango
    public float rangoVisibilidad = 50f;           // Distancia máxima a la que se muestra

    [Header("Seguimiento")]
    public bool seguirAlJugador = true;            // Si es true, la flecha 3D seguirá al jugador
    public float alturaSobreJugador = 2.0f;        // Altura sobre el jugador para la flecha
    public float distanciaDelante = 1.0f;          // Distancia delante del jugador
    public bool usarCamaraPerspectiva = false;     // Usar la cámara principal para determinar "delante"
    public float velocidadSeguimiento = 5.0f;      // Velocidad a la que la flecha sigue al jugador
    public float distanciaObjetivoParaDesactivar = 3.0f; // Cuando el jugador se acerque a esta distancia, la brújula se desactivará
    public Transform spawnPoint;                   // Punto específico donde aparecerá la flecha (opcional)

    [Header("Comportamiento")]
    public bool ignorarEjeY = true;          // Si es verdadero, solo considera dirección horizontal
    public float duracionVisible = 10f;      // Tiempo que permanece activa tras activación
    public bool permanente = false;          // Si es verdadero, se mantiene activa siempre

    [Header("Depuración")]
    public bool mostrarLineaDebug = true;    // Muestra una línea entre jugador y objetivo
    public bool activarAlInicio = false;     // Activa la brújula desde el inicio

    // Variables privadas
    private bool activa = false;
    private float tiempoRestante = 0f;
    private Vector3 direccionObjetivo = Vector3.zero;
    private float distanciaAlObjetivo = 0f;
    
    void Awake()
    {
        // Patrón singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    
    void Start()
    {
        // Buscar referencias si no están asignadas
        if (jugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag(tagJugador);
            if (playerObj != null)
                jugador = playerObj.transform;
            else
                Debug.LogWarning("BrujulaMate: No se encontró el jugador con tag " + tagJugador);
        }
        
        if (objetivo == null)
        {
            GameObject targetObj = GameObject.FindGameObjectWithTag(tagObjetivo);
            if (targetObj != null)
                objetivo = targetObj.transform;
            else
                Debug.LogWarning("BrujulaMate: No se encontró el objetivo con tag " + tagObjetivo);
        }
        
        // Verificar elementos de UI
        if (usarUI && imagenFlechaUI == null)
        {
            Debug.LogWarning("BrujulaMate: Se ha activado UI pero no se asignó una imagen.");
            usarUI = false;
        }
        if (!usarUI && flechaTransform == null)
        {
            Debug.LogWarning("BrujulaMate: No se asignó flechaTransform para la brújula 3D.");
        }
        
        // Estado inicial: Desactivada
        activa = false;
        SetVisibilidad(false); // Asegura que la flecha no sea visible al inicio
        
        // Si se configura para activar al inicio
        if (activarAlInicio)
            Activar();
    }

    void Update()
    {
        // No hacer nada si no está activa
        if (!activa)
            return;
            
        // Verificar si se debe desactivar por tiempo
        if (!permanente && duracionVisible > 0)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante <= 0)
            {
                Desactivar();
                return;
            }
        }

        // Calcular dirección y distancia solo si tenemos jugador y objetivo
        if (jugador && objetivo)
        {
            CalcularDireccionYDistancia();
            
            // Desactivar si el jugador está suficientemente cerca del objetivo
            if (distanciaAlObjetivo <= distanciaObjetivoParaDesactivar)
            {
                Debug.Log("BrujulaMate: Objetivo alcanzado, desactivando brújula.");
                Desactivar();
                return;
            }
            
            // Si está configurado para mostrarse solo en un rango
            if (mostrarSoloEnCiertoRango && distanciaAlObjetivo > rangoVisibilidad)
            {
                // Ocultar visualmente pero mantener activa
                SetVisibilidad(false);
            }
            else 
            {
                // Mostrar y actualizar la dirección
                SetVisibilidad(true);
                
                // Si la flecha debe seguir al jugador, actualizar su posición
                if (seguirAlJugador && flechaTransform != null)
                {
                    // Forzar actualización de posición cada frame para garantizar seguimiento
                    ActualizarPosicionFlecha();
                }
                
                ActualizarDireccionVisual();
            }
            
            // Dibujar líneas de debug
            if (mostrarLineaDebug && Application.isEditor)
            {
                Debug.DrawLine(jugador.position, objetivo.position, Color.yellow);
                Debug.DrawRay(jugador.position, direccionObjetivo.normalized * 2f, Color.cyan);
            }
        }
    }
    
    // Calcula la dirección hacia el objetivo y la distancia
    private void CalcularDireccionYDistancia()
    {
        // Vector del jugador al objetivo
        direccionObjetivo = objetivo.position - jugador.position;
        
        // Ignorar altura si está configurado así
        if (ignorarEjeY)
            direccionObjetivo.y = 0;
            
        // Calcular distancia
        distanciaAlObjetivo = direccionObjetivo.magnitude;
        
        // Normalizar para tener solo la dirección
        if (distanciaAlObjetivo > 0.001f)
            direccionObjetivo.Normalize();
    }
    
    // Actualiza la representación visual de la dirección
    private void ActualizarDireccionVisual()
    {
        if (usarUI && imagenFlechaUI != null)
        {
            // Convertir la dirección mundial a dirección de pantalla
            Vector3 direccionLocal = jugador.InverseTransformDirection(direccionObjetivo);
            float angulo = Mathf.Atan2(direccionLocal.x, direccionLocal.z) * Mathf.Rad2Deg;
            // Ajusta el signo si la flecha apunta al revés
            imagenFlechaUI.rectTransform.localRotation = Quaternion.Euler(0, 0, -angulo);
        }
        else if (flechaTransform != null)
        {
            // Rotar un objeto 3D (como una flecha) para apuntar en la dirección
            Vector3 lookDir = direccionObjetivo;
            if (ignorarEjeY)
                lookDir.y = 0;
            if (lookDir.sqrMagnitude > 0.001f)
                flechaTransform.rotation = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
        }
    }
    
    // Actualiza la posición de la flecha para que siga al jugador
    private void ActualizarPosicionFlecha()
    {
        if (flechaTransform == null || jugador == null)
            return;
            
        // Si hay un punto de spawn específico, usar ese en vez de calcular posición
        if (spawnPoint != null)
        {
            flechaTransform.position = spawnPoint.position;
            return;
        }
        
        Vector3 posicionObjetivo;
        
        if (usarCamaraPerspectiva && Camera.main != null)
        {
            Vector3 direccionCamara = Camera.main.transform.forward;
            if (ignorarEjeY)
            {
                direccionCamara.y = 0;
                if (direccionCamara.sqrMagnitude < 0.001f)
                    direccionCamara = jugador.forward;
            }
            direccionCamara.Normalize();
            posicionObjetivo = jugador.position + Vector3.up * alturaSobreJugador + direccionCamara * distanciaDelante;
        }
        else
        {
            Vector3 direccion = direccionObjetivo.sqrMagnitude < 0.001f && jugador.forward.sqrMagnitude > 0.001f
                ? jugador.forward
                : direccionObjetivo;
            if (ignorarEjeY)
            {
                direccion.y = 0;
                if (direccion.sqrMagnitude < 0.001f)
                    direccion = Vector3.forward;
                direccion.Normalize();
            }
            posicionObjetivo = jugador.position + Vector3.up * alturaSobreJugador + direccion * distanciaDelante;
        }
        flechaTransform.position = posicionObjetivo;
    }
    
    // Activa la brújula
    public void Activar()
    {
        if (!activa)
        {
            activa = true;
            tiempoRestante = duracionVisible;
            
            // Posicionar la flecha inicialmente si está configurada para seguir al jugador
            if (seguirAlJugador && flechaTransform != null && jugador != null)
            {
                if (spawnPoint != null)
                {
                    flechaTransform.position = spawnPoint.position;
                }
                else
                {
                    flechaTransform.position = jugador.position + Vector3.up * alturaSobreJugador;
                }
            }
            SetVisibilidad(true);
            Debug.Log("BrujulaMate: Activada");
        }
        else
        {
            // Si ya está activa, asegúrate de que la visibilidad esté correcta
            SetVisibilidad(true);
        }
    }
    
    // Desactiva la brújula
    public void Desactivar()
    {
        if (activa)
        {
            activa = false;
            SetVisibilidad(false);
            Debug.Log("BrujulaMate: Desactivada");
        }
    }
    
    // Activa la brújula con un retraso
    public void ActivarConRetraso(float segundos)
    {
        StartCoroutine(ActivarCoroutine(segundos));
    }
    
    // Corrutina para activación con retraso
    private IEnumerator ActivarCoroutine(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        Activar();
    }
    
    // Configura la visibilidad de los elementos visuales
    private void SetVisibilidad(bool visible)
    {
        if (usarUI && imagenFlechaUI != null)
        {
            Color nuevoColor = imagenFlechaUI.color;
            nuevoColor.a = visible ? 1f : 0f;
            imagenFlechaUI.color = nuevoColor;
        }
            
        if (flechaTransform != null)
        {
            // Activar/desactivar renderers en la flecha
            Renderer[] renderers = flechaTransform.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                r.enabled = visible;
            }
        }
    }
    
    // Devuelve la dirección normalizada hacia el objetivo
    public Vector3 ObtenerDireccion()
    {
        if (jugador && objetivo)
        {
            Vector3 dir = objetivo.position - jugador.position;
            if (ignorarEjeY)
                dir.y = 0;
            return dir.normalized;
        }
        return Vector3.zero;
    }
    
    // Devuelve la distancia al objetivo
    public float ObtenerDistancia()
    {
        if (jugador && objetivo)
        {
            Vector3 dir = objetivo.position - jugador.position;
            return dir.magnitude;
        }
        return 0f;
    }
    
    // Actualiza programáticamente el objetivo
    public void SetObjetivo(Transform nuevoObjetivo)
    {
        objetivo = nuevoObjetivo;
    }
    
    // Limpieza al destruir
    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
    
    // Verificar si el jugador ha llegado al objetivo
    public bool JugadorHaLlegadoAlObjetivo()
    {
        return jugador != null && objetivo != null && 
               Vector3.Distance(jugador.position, objetivo.position) <= distanciaObjetivoParaDesactivar;
    }

    // Establecer un nuevo punto de spawn para la flecha
    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
        
        // Si está activa, actualizar inmediatamente la posición
        if (activa && flechaTransform != null && spawnPoint != null)
        {
            flechaTransform.position = spawnPoint.position;
        }
    }
}
