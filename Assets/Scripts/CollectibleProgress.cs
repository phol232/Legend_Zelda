using UnityEngine;

/// <summary>
/// Sistema estático que guarda el progreso de objetos recogidos.
/// Persiste entre escenas usando variables estáticas.
/// </summary>
public static class CollectibleProgress
{
    // Arrays para cada escena (hasta 20 objetos por escena)
    private static bool[] sampleSceneCollected = new bool[20];
    private static bool[] tallerCollected = new bool[20];
    private static bool[] prueba1Collected = new bool[20];
    private static bool[] prueba2Collected = new bool[20];
    private static bool[] zonaFinalCollected = new bool[20];

    /// <summary>
    /// Marca un objeto como recogido
    /// </summary>
    public static void MarcarRecogido(string escena, int index)
    {
        if (index < 0 || index >= 20) return;

        bool[] array = GetArrayForScene(escena);
        if (array != null)
        {
            array[index] = true;
            Debug.Log($"[CollectibleProgress] Objeto {index} en {escena} marcado como recogido");
        }
    }

    /// <summary>
    /// Verifica si un objeto ya fue recogido
    /// </summary>
    public static bool FueRecogido(string escena, int index)
    {
        if (index < 0 || index >= 20) return false;

        bool[] array = GetArrayForScene(escena);
        if (array != null)
        {
            return array[index];
        }
        return false;
    }

    /// <summary>
    /// Obtiene el array correspondiente a una escena
    /// </summary>
    private static bool[] GetArrayForScene(string escena)
    {
        switch (escena)
        {
            case "SampleScene":
                return sampleSceneCollected;
            case "Taller":
                return tallerCollected;
            case "PRUEBA 1":
                return prueba1Collected;
            case "PRUEBA 2":
                return prueba2Collected;
            case "ZonaFinal":
                return zonaFinalCollected;
            default:
                Debug.LogWarning($"[CollectibleProgress] Escena no reconocida: {escena}");
                return null;
        }
    }

    /// <summary>
    /// Reinicia todos los objetos recogidos (para nuevo juego)
    /// </summary>
    public static void ReiniciarTodo()
    {
        for (int i = 0; i < 20; i++)
        {
            sampleSceneCollected[i] = false;
            tallerCollected[i] = false;
            prueba1Collected[i] = false;
            prueba2Collected[i] = false;
            zonaFinalCollected[i] = false;
        }
        Debug.Log("[CollectibleProgress] Progreso de objetos reiniciado");
    }
}
