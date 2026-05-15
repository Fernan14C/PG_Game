using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BoatController : MonoBehaviour
{
    [Header("Referencias de Arquitectura")]
    [Tooltip("Arrastrá aquí el objeto 'Referencia_Frente'")]
    public Transform referenciaFrente;

    [Header("Ajustes de Motor")]
    public float motorForce = 50000f;
    public float steerForce = 2500f;
    public float waterDrag = 1.5f;

    private Rigidbody rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = waterDrag;
        rb.angularDamping = 2f;

        // Validación de seguridad
        if (referenciaFrente == null)
        {
            Debug.LogError("¡Falta asignar la Referencia Frente en el Inspector!");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if (referenciaFrente == null) return;

        // 1. MOVIMIENTO (W/S)
        // Usamos la dirección 'forward' (azul) de tu objeto de referencia
        if (Mathf.Abs(moveInput.y) > 0.1f)
        {
            Vector3 direccionMovimiento = referenciaFrente.forward * moveInput.y * motorForce;
            rb.AddForce(direccionMovimiento, ForceMode.Force);
        }

        // 2. GIRO (A/D)
        // El torque siempre se aplica sobre el eje vertical (Y) del barco
        if (Mathf.Abs(moveInput.x) > 0.1f)
        {
            // Efecto de giro tipo GTA: más giro si hay algo de velocidad
            float speedFactor = Mathf.Clamp01(rb.linearVelocity.magnitude / 2f);
            float turnAmount = moveInput.x * steerForce * (speedFactor + 0.5f);

            rb.AddRelativeTorque(Vector3.up * turnAmount, ForceMode.Force);
        }
    }
}