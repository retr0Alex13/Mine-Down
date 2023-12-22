using UnityEngine;

public class ParticleInstantiator : MonoBehaviour
{
    [SerializeField] private GameObject particle;

    public void InstantiateParticleInDirection(Vector2 direction)
    {
        Quaternion rotation = transform.rotation * GetRotationAngle(direction);
        Instantiate(particle, transform.position, rotation);
    }

    private Quaternion GetRotationAngle(Vector2 direction)
    {
        if (direction == Vector2.up || direction == Vector2.down)
        {
            return Quaternion.Euler(-90f, 0f, 0f);
        }
        else if (direction == Vector2.right)
        {
            return Quaternion.Euler(-50f, 70f, 0f);
        }
        else if (direction == Vector2.left)
        {
            return Quaternion.Euler(-150f, 70f, 0f);
        }

        return Quaternion.identity; // Default rotation
    }
}
