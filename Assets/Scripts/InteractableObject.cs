using UnityEngine;

/// <summary>
/// Classe base para objetos interagíveis. Contém métodos que devem ser implementados por classes derivadas.
/// </summary>
public abstract class InteractableObject : MonoBehaviour
{
    /// <summary>
    /// Método a ser chamado quando o objeto é interagido.
    /// </summary>
    public abstract void Interact();
}
