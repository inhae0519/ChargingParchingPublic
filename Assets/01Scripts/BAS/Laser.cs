using DG.Tweening;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LayerMask _whatisTarget;
    [SerializeField] private Transform _laserBody, _laserHit;

    [SerializeField] private float originSizeX;
    [SerializeField] private SpriteRenderer coreSprite;

    private Sequence sequence;
    private Transform player = null;

    [SerializeField] private float damage = 5;
    private float damageInterval = 0.01f;
    private float lastDamageTime = 0;

    [SerializeField] private ParticleSystem _laserCover;

    [SerializeField] private SoundSO laserSound;
    [SerializeField] private SoundSO laserHitSound;
    [SerializeField] private GameEventChannelSO soundChannel;

    private void Start()
    {
        sequence = DOTween.Sequence();

        sequence.Append(transform.DORotate(new Vector3(0, 0, 60), 8, RotateMode.FastBeyond360));
        sequence.Join(coreSprite.DOFade(1, 0.7f)).SetEase(Ease.InSine);
        sequence.SetLoops(-1, LoopType.Yoyo);
    }

    private void OnEnable()
    {
        player = null;
        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = laserSound;
        soundChannel.RaiseEvent(evt);

        transform.localScale = new Vector3(originSizeX, transform.localScale.y, transform.localScale.z);
        transform.rotation = Quaternion.Euler(0, 0, -60);
        sequence.Restart();
    }

    private void OnDisable()
    {
        coreSprite.color = new Color(coreSprite.color.r, coreSprite.color.g, coreSprite.color.b, 0);
    }

    void Update()
    {
        Vector2 resultPos = transform.position + transform.up * 200;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 100, _whatisTarget);

        if (hit)
        {
            var evt = SoundEvents.PlaySfxEvent;
            evt.clipData = laserHitSound;
            soundChannel.RaiseEvent(evt);

            if (hit.transform != null)
            {
                if (player == null && hit.transform.TryGetComponent<Player>(out Player playerCompo))
                {
                    player = hit.transform;

                    if (sequence.IsPlaying())
                        sequence.Pause();
                }

                if (hit.transform.TryGetComponent<IDamageable>(out IDamageable target))
                {
                    resultPos = hit.point;

                    // Damage application with interval check
                    if (Time.time >= lastDamageTime + damageInterval)
                    {
                        target.ApplyDamage(damage);
                        lastDamageTime = Time.time;
                    }
                }
            }
        }

        if (player != null)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            Quaternion targetRot = Quaternion.Euler(0, 0, angle - 90);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * 5);
        }

        float distance = Vector2.Distance(transform.position, resultPos);
        //Debug.Log(distance);

        _laserBody.up = (resultPos - (Vector2)transform.position).normalized;

        float laserSizeX = player == null ? originSizeX : 0.8f;
        _laserBody.localScale = new Vector3(laserSizeX, distance + 1, _laserBody.localScale.z);

        _laserHit.position = resultPos;

        ParticleSystem.ShapeModule shape = _laserCover.shape;
        ParticleSystem.EmissionModule emission = _laserCover.emission;
        shape.scale = new Vector3(laserSizeX, _laserBody.localScale.y, distance * 2.5f + 1);
        emission.rateOverTime = (int)(distance * 1.5f) + 15;
    }
}
