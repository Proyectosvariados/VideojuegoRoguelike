using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Configuración del Arma")]
    public float damage = 10f;
    // Aquí puedes mantener otros parámetros si los necesitas

    [HideInInspector]
    public bool isEquipped = false;

    // Valores originales para restaurar al soltar el arma
    public Vector3 DefaultLocalPosition { get; private set; }
    public Quaternion DefaultLocalRotation { get; private set; }
    public Vector3 DefaultLocalScale { get; private set; }

    void Awake()
    {
        // Se almacenan los valores originales según el prefab
        DefaultLocalPosition = transform.localPosition;
        DefaultLocalRotation = transform.localRotation;
        DefaultLocalScale = transform.localScale;
        Debug.Log("Weapon Awake - Posición: " + DefaultLocalPosition +
                  ", Rotación: " + DefaultLocalRotation.eulerAngles +
                  ", Escala: " + DefaultLocalScale);
    }

    // Si prefieres mantener lógica de PerformAttack aquí, déjala vacía y se usará desde PlayerAttack.
    public void PerformAttack()
    {
        // Este método puede quedar vacío o llamarse desde PlayerAttack para propósitos de debug.
    }
}
