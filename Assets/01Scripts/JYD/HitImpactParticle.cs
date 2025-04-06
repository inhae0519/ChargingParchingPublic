using System;
using UnityEngine;

public class HitImpactParticle : SmokeParticle
{
    private ParticleSystemRenderer _particleSystemRenderer;
    
    public void SetUpColor(Material _mat)
    {
        if(_particleSystemRenderer == null)
            _particleSystemRenderer = GetComponentInChildren<ParticleSystemRenderer>();
        
        _particleSystemRenderer.material = _mat;
    }
    
}
