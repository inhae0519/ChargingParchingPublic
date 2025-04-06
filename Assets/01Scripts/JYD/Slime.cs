using System;
using System.Threading.Tasks;
using UnityEngine;

public class Slime : Entity
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material blinkMat;
    private Material origin;

    private void Start()
    {
        origin = _spriteRenderer.material;
    }
    
    public async void Blink()
    {
        _spriteRenderer.material = blinkMat;
        await Task.Delay(100);
        _spriteRenderer.material = origin;
    }
        
}
