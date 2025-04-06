using UnityEngine;

public class ParticleEmisiionHandler : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;

    public void ChangeEmission(float _rateOverTime)
    {
        ParticleSystem.EmissionModule emission = _particleSystem.emission;
        emission.rateOverTime = _rateOverTime;
    }
}
