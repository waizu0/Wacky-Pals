using UnityEngine;

/// <summary>
/// Permite tremor da c�mera, configur�vel por intensidade e dura��o.
/// </summary>
public class CameraShake : MonoBehaviour
{
    
    [Header("Configura��es do Tremor")]
    [Tooltip("Dura��o do tremor da c�mera em segundos.")]
    [SerializeField] private float shakeDuration = 0.5f;
    [Tooltip("Intensidade do tremor da c�mera.")]
    [SerializeField] private float shakeIntensity = 0.7f;

    private Vector3 originalPosition;
    private float currentShakeDuration;

    void Awake()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (currentShakeDuration > 0)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeIntensity;
            currentShakeDuration -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPosition;
            currentShakeDuration = 0;
        }
    }

    /// <summary>
    /// Inicia o tremor da c�mera.
    /// </summary>
    public void Shake()
    {
        currentShakeDuration = shakeDuration;
    }
}
