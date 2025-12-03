using UnityEngine;

public class PlayerWeaponHolder : MonoBehaviour
{
    [Header("Weapon Holder")]
    public Transform weaponAnchor;        // Empty donde se pega el arma
    public Vector3 weaponScale = new Vector3(0.3f, 0.3f, 1f);  // Escala del arma
    public GameObject weaponPrefab;       // Prefab del arma (asignar en Inspector)

    private GameObject currentWeapon;     // Instancia del arma equipada
    private bool hasWeapon = false;
    private static bool weaponEquippedGlobal = false;  // Persistir entre escenas

    void Start()
    {
        // Si ya tenía el arma equipada (cambio de escena), re-equipar visualmente
        if (weaponEquippedGlobal)
        {
            Debug.Log("Detectado que ya tenía arma, recreando visual...");
            
            // Asegurar que tenemos weaponAnchor
            if (weaponAnchor == null)
            {
                weaponAnchor = transform.Find("WeaponAnchor");
                if (weaponAnchor == null)
                {
                    GameObject anchor = new GameObject("WeaponAnchor");
                    anchor.transform.SetParent(transform);
                    anchor.transform.localPosition = new Vector3(0.3f, 0, 0);
                    weaponAnchor = anchor.transform;
                }
            }
            
            // Crear el arma visual usando el prefab asignado
            if (weaponPrefab != null)
            {
                EquipWeaponVisual(weaponPrefab);
            }
            else
            {
                Debug.LogWarning("weaponPrefab no está asignado en PlayerWeaponHolder. Asígnalo en el Inspector.");
            }
        }
    }

    // Esta función la llamará el cofre
    public void EquipWeapon(GameObject weaponPrefabParam)
    {
        if (weaponPrefabParam == null)
        {
            Debug.LogWarning("EquipWeapon llamado sin weaponPrefab");
            return;
        }

        weaponEquippedGlobal = true;
        
        EquipWeaponVisual(weaponPrefabParam);
    }

    void EquipWeaponVisual(GameObject weaponPrefab)
    {
        if (weaponAnchor == null)
        {
            Debug.LogError("WeaponAnchor no está asignado!");
            return;
        }

        // Si ya había un arma equipada, la destruimos
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // Instanciamos el arma como hija del WeaponAnchor
        currentWeapon = Instantiate(weaponPrefab, weaponAnchor);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
        
        // Aplicar escala pequeña para que no sea tan grande
        currentWeapon.transform.localScale = weaponScale;
        
        // Asegurarse de que esté activo y visible
        currentWeapon.SetActive(true);
        hasWeapon = true;

        Debug.Log("¡Arma equipada visualmente en el jugador!");
    }

    public bool HasWeapon()
    {
        return hasWeapon || weaponEquippedGlobal;
    }

    public GameObject GetCurrentWeapon()
    {
        return currentWeapon;
    }

    // Para verificar si tiene arma globalmente
    public static bool HasWeaponGlobal()
    {
        return weaponEquippedGlobal;
    }
}