using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [Header("Configuración del Arma")]
    public float damage = 10f;           // Daño que causa el arma
    public float attackDuration = 0.2f;  // Duración del swing de ataque
    public float arcAngle = 45f;         // Ángulo total (en grados) del arco de ataque

    [HideInInspector]
    public bool isEquipped = false;      // Se marca como true cuando el arma está equipada

    // Se guarda la posición local por defecto (por ejemplo, (0.5, 0, 0))
    private Vector3 defaultLocalPosition;

    void Start()
    {
        // Se almacena la posición local inicial (definida al equiparse, p.ej. (0.5,0,0))
        defaultLocalPosition = transform.localPosition;
        Debug.Log("Weapon Start: defaultLocalPosition = " + defaultLocalPosition);
    }

    /// <summary>
    /// Se llama para realizar el ataque.
    /// </summary>
    public void PerformAttack()
    {
        if (!isEquipped)
        {
            Debug.Log("PerformAttack aborted: Weapon is not equipped.");
            return;
        }
        Debug.Log("PerformAttack initiated.");
        StartCoroutine(SwingAttack());
    }

    /// <summary>
    /// Realiza el ataque mediante un movimiento en arco.
    /// El arma se mueve de startAngle a endAngle en attackDuration segundos, siguiendo la dirección del mouse.
    /// </summary>
    IEnumerator SwingAttack()
    {
        float elapsed = 0f;
        float radius = defaultLocalPosition.magnitude;
        Vector3 pivot = transform.parent.position;

        // Se obtiene la posición del mouse en el mundo y se fija z = 0 (para 2D)
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Debug.Log("SwingAttack: mouseWorldPos = " + mouseWorldPos);

        // Dirección de ataque: desde el pivot (por ejemplo, la posición del WeaponHolder) hasta el mouse
        Vector2 attackDir = (mouseWorldPos - pivot).normalized;
        float targetAngle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;
        float startAngle = targetAngle - arcAngle / 2f;
        float endAngle = targetAngle + arcAngle / 2f;

        Debug.Log("SwingAttack: targetAngle = " + targetAngle + ", startAngle = " + startAngle + ", endAngle = " + endAngle);

        // Durante el tiempo de ataque se interpola el ángulo y se actualiza la posición local y rotación del arma
        while (elapsed < attackDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / attackDuration;
            float currentAngle = Mathf.Lerp(startAngle, endAngle, t);
            float rad = currentAngle * Mathf.Deg2Rad;
            Vector3 newLocalPos = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;
            transform.localPosition = newLocalPos;
            transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
            Debug.Log("SwingAttack t: " + t + ", currentAngle: " + currentAngle + ", newLocalPos: " + newLocalPos);
            yield return null;
        }
        // Al finalizar, se restablece la posición y rotación originales
        transform.localPosition = defaultLocalPosition;
        transform.localRotation = Quaternion.identity;
        Debug.Log("SwingAttack complete, reset position and rotation.");
    }
}
