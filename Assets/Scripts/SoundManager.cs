using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantém o objeto entre as cenas, se necessário.
        }
    }

    public delegate void SoundEmittedDelegate(Vector3 location, float volume);
    public event SoundEmittedDelegate OnSoundEmitted;

    /// <summary>
    /// Emitir um som que pode ser ouvido pelos inimigos.
    /// </summary>
    /// <param name="location">A localização do som.</param>
    /// <param name="volume">O volume do som.</param>
    public void EmitSound(Vector3 location, float volume, AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, location, volume);
        OnSoundEmitted?.Invoke(location, volume); // Dispara o evento.
    }
}
