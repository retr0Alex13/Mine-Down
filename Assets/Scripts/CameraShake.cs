using UnityEngine;
using Cinemachine;

public class CameraShakeController : MonoBehaviour
{
    [SerializeField] private float maxShakeDistance = 5f;
    [SerializeField] private DistanceCalculator distanceCalculator;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        float distance = distanceCalculator.GetDistance();
        float normalizedDistance = Mathf.Clamp01(distance / maxShakeDistance);

        float amplitude = Mathf.Lerp(1f, 0f, normalizedDistance);
        perlinNoise.m_AmplitudeGain = amplitude;
    }
}
