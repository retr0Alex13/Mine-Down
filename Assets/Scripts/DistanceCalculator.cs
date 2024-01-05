using UnityEngine;

public class DistanceCalculator : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerMove>().transform;
    }

    public float GetDistance()
    {
        if (player != null)
        {
            return Vector3.Distance(transform.position, player.position);
        }
        return 0f;
    }
}
