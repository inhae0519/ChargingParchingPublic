using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GolemBossAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator BossAnimator;
    [SerializeField] private int explosionCount;
        
    [SerializeField] private GameEventChannelSO ChannelSo;
    [SerializeField] private GolemBoss GolemBoss;

    [SerializeField] private SpriteRenderer[] parts;
    [SerializeField] private Material BlinkMat;
    private Material[] originsPartMats;
    
    private void Start()
    {
        originsPartMats = new Material[parts.Length];
        
        for (int i = 0; i < parts.Length; i++)
        {
            originsPartMats[i] = parts[i].material;
        }
        
    }


    public async void StartExplosion()
    {
        await Task.Delay(500); 
        
        for (int i = 0; i < explosionCount; i += 3) 
        {
            for (int j = 0; j < 3 && i + j < explosionCount; j++)
            {
                var evt = SpawnEvents.ExplosionCreate;
                evt.position = transform.position + new Vector3(Random.Range(-2,2) , Random.Range(-2,2) , 0);
                evt.poolType = PoolType.ExplosionParticle;
                
                ChannelSo.RaiseEvent(evt);
            }
            await Task.Delay(400); 
        }
    }
    
    public void SetDead()
    {
        BossAnimator.enabled = true;
        BossAnimator.SetBool("Dead",true);
    }

    public async void BlinkGolem()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].material = BlinkMat;
        }

        await Task.Delay(100);
        
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].material = originsPartMats[i];
        }
    }
    
}
