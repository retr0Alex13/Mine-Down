using UnityEngine;

public class Block : MonoBehaviour, IDestroyable
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
