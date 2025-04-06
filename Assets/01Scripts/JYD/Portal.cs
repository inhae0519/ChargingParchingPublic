using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;


public class Portal : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private FadeIn fade;
    
    private PlayerInputSO _playerInput;
    
    private readonly int _value = Shader.PropertyToID("_Value");
    private Material mat;
    private SpriteRenderer portalVisualSprite;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((whatIsTarget & (1 << other.gameObject.layer)) != 0)
        {
            if (_playerInput == null)
            {
                _playerInput = other.GetComponent<Player>().PlayerInput;
            }
            
            _playerInput.InteractEvent += TransitionScene;
        }  
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if ((whatIsTarget & (1 << other.gameObject.layer)) != 0)
        {
            _playerInput.InteractEvent -= TransitionScene;
            _playerInput = null;
        }  
                
    }

    private void TransitionScene()
    {
        fade.FadeStart(nextSceneName);
    }

    public void Spawn(float _duration)
    {
        if (mat == null)
            mat = GetComponent<SpriteRenderer>().material;

        if (portalVisualSprite == null)
            portalVisualSprite = transform.Find("Visual").GetComponent<SpriteRenderer>();
                    
        StartCoroutine(SpawnCoroutine(_duration));
    }

    private IEnumerator SpawnCoroutine(float duration)
    {
        portalVisualSprite.color = 
            new Color(portalVisualSprite.color.r , portalVisualSprite.color.g , portalVisualSprite.color.b , 0);
        float elapsedTime = 0f;
        
        if (mat.HasProperty(_value))
        {
            mat.SetFloat(_value, -2f);
        }
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            // -2에서 2로 값 변경
            float value = Mathf.Lerp(-2f, 2f, elapsedTime / duration);
            if (mat.HasProperty(_value))
            {
                mat.SetFloat(_value, value); 
            }
            yield return null;
        }

        if (mat.HasProperty(_value))
        {
            mat.SetFloat(_value, 2f);
          
            portalVisualSprite.DOFade(1,duration);
        }
    }
    
}
