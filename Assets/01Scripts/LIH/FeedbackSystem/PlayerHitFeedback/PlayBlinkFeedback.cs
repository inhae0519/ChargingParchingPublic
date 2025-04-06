using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBlinkFeedback : Feedback
{
    [SerializeField] private float _blinkTime = 0.2f;

    private SpriteRenderer[] _spriteRenderers;
    private readonly int _blinkValueHash = Shader.PropertyToID("_BlinkValue");

    private List<Material> _targetMaterials;
    private Coroutine _coroutine = null;

    protected override void Awake()
    {
        _spriteRenderers = transform.root.GetComponentsInChildren<SpriteRenderer>();
        _targetMaterials = new List<Material>();
        
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _targetMaterials.Add(_spriteRenderers[i].material);
        }
    }
    public override void CreateFeedback()
    {
        _coroutine = StartCoroutine(BlinkCoroutine());
    }
    
    private IEnumerator BlinkCoroutine()
    {
        Blink(0.5f);
        yield return new WaitForSeconds(_blinkTime);
        Blink(0f);
    }
    
    public override void FinishFeedback()
    {

    }

    private void Blink(float value)
    {
        foreach (var material in _targetMaterials)
        {
            material.SetFloat(_blinkValueHash, value);
        }
    }
}
