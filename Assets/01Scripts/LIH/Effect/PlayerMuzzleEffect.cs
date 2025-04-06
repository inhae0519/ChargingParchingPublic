using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMuzzleEffect : MonoBehaviour
{
    private ParticleSystem _parentParticle;
    private List<ParticleSystem> _particleSystems;

    private void Awake()
    {
        _parentParticle = GetComponent<ParticleSystem>();
        _particleSystems = GetComponentsInChildren<ParticleSystem>().ToList();
    }

    public void SetRotate(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
        _parentParticle.Play();
    }
}
