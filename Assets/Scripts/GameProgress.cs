using UnityEngine;

/// <summary>
/// Sistema de progreso del juego que persiste entre escenas.
/// Guarda qué pruebas se han completado y qué enemigos han sido derrotados.
/// </summary>
public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance { get; private set; }

    // Estado de las pruebas
    public static bool prueba1Completada = false;
    public static bool prueba2Completada = false;
    public static bool prueba3Completada = false;

    // Enemigos derrotados (por escena)
    public static bool[] enemigosSampleSceneDerrotados = new bool[10]; // Hasta 10 enemigos
    public static bool[] enemigosPrueba1Derrotados = new bool[10];
    public static bool[] enemigosPrueba2Derrotados = new bool[10];
    public static bool[] enemigosZonaFinalDerrotados = new bool[10];

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Marcar una prueba como completada
    public static void CompletarPrueba(int numeroPrueba)
    {
        switch (numeroPrueba)
        {
            case 1:
                prueba1Completada = true;
                Debug.Log("¡PRUEBA 1 COMPLETADA!");
                break;
            case 2:
                prueba2Completada = true;
                Debug.Log("¡PRUEBA 2 COMPLETADA!");
                break;
            case 3:
                prueba3Completada = true;
                Debug.Log("¡PRUEBA 3 COMPLETADA!");
                break;
        }
    }

    // Verificar si una prueba está completada
    public static bool PruebaCompletada(int numeroPrueba)
    {
        switch (numeroPrueba)
        {
            case 1: return prueba1Completada;
            case 2: return prueba2Completada;
            case 3: return prueba3Completada;
            default: return false;
        }
    }

    // Marcar un enemigo como derrotado
    public static void MarcarEnemigoDerrotado(string nombreEscena, int indiceEnemigo)
    {
        if (indiceEnemigo < 0 || indiceEnemigo >= 10) return;

        switch (nombreEscena)
        {
            case "SampleScene":
                enemigosSampleSceneDerrotados[indiceEnemigo] = true;
                break;
            case "PRUEBA 1":
                enemigosPrueba1Derrotados[indiceEnemigo] = true;
                break;
            case "PRUEBA 2":
                enemigosPrueba2Derrotados[indiceEnemigo] = true;
                break;
            case "ZonaFinal":
                enemigosZonaFinalDerrotados[indiceEnemigo] = true;
                break;
        }
        Debug.Log($"Enemigo {indiceEnemigo} de {nombreEscena} marcado como derrotado");
    }

    // Verificar si un enemigo fue derrotado
    public static bool EnemigoDerrotado(string nombreEscena, int indiceEnemigo)
    {
        if (indiceEnemigo < 0 || indiceEnemigo >= 10) return false;

        switch (nombreEscena)
        {
            case "SampleScene":
                return enemigosSampleSceneDerrotados[indiceEnemigo];
            case "PRUEBA 1":
                return enemigosPrueba1Derrotados[indiceEnemigo];
            case "PRUEBA 2":
                return enemigosPrueba2Derrotados[indiceEnemigo];
            case "ZonaFinal":
                return enemigosZonaFinalDerrotados[indiceEnemigo];
            default:
                return false;
        }
    }

    // Reiniciar todo el progreso (para nuevo juego)
    public static void ReiniciarProgreso()
    {
        prueba1Completada = false;
        prueba2Completada = false;
        prueba3Completada = false;
        
        for (int i = 0; i < 10; i++)
        {
            enemigosSampleSceneDerrotados[i] = false;
            enemigosPrueba1Derrotados[i] = false;
            enemigosPrueba2Derrotados[i] = false;
            enemigosZonaFinalDerrotados[i] = false;
        }
        
        Debug.Log("Progreso reiniciado");
    }
}
