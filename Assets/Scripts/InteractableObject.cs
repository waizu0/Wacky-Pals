using UnityEngine;

/// <summary>
/// Classe base para objetos interag�veis. Cont�m m�todos que devem ser implementados por classes derivadas.
/// </summary>
public abstract class InteractableObject : MonoBehaviour
{
    /// <summary>
    /// M�todo a ser chamado quando o objeto � interagido.
    /// </summary>
    public abstract void Interact();
}
