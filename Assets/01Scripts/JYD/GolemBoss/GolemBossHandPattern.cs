using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GolemBossHandPattern : MonoBehaviour
{
    private GolemBoss golemBoss;
    private GolemBossBulletPattern bulletPattern;

    [SerializeField] private GameEventChannelSO spawnEventChannel;

    [SerializeField] private Vector2 minSize;
    [SerializeField] private Vector2 maxSize;

    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    private GolemHand leftHandCompo;
    private GolemHand rightHandCompo;

    private bool isLeftHandMoving;
    private bool isRightHandMoving;

    [SerializeField] private PlayerManagerSO playerManagerSo;
    private Transform player;

    private void Awake()
    {
        golemBoss = GetComponent<GolemBoss>();
    }

    private void Start()
    {
        player = playerManagerSo.PlayerTrm;
        bulletPattern = golemBoss.BulletPattern;

        leftHandCompo = leftHand.GetComponent<GolemHand>();
        rightHandCompo = rightHand.GetComponent<GolemHand>();
    }

    public void TakeDownHand(bool isLeft, float speed, float downSpeed)
    {
        if (isLeft)
        {
            if (isLeftHandMoving) return;
            isLeftHandMoving = true;
            StartCoroutine(TakeDownHandRoutine(leftHand, leftHandCompo, speed, downSpeed, () => isLeftHandMoving = false));
        }
        else
        {
            if (isRightHandMoving) return;
            isRightHandMoving = true;
            StartCoroutine(TakeDownHandRoutine(rightHand, rightHandCompo, speed, downSpeed, () => isRightHandMoving = false));
        }
    }

    private IEnumerator TakeDownHandRoutine(Transform hand, GolemHand handCompo, float speed, float downSpeed, System.Action onComplete)
    {
        Vector3 originalPosition = hand.position;
        Vector3 liftTarget = player.position + Vector3.up * 2.5f;
        Vector3 strikeTarget = player.position;

        yield return MoveToPosition(hand, liftTarget, speed);
        yield return new WaitForSeconds(0.3f);
        yield return MoveToPosition(hand, strikeTarget, downSpeed);

        handCompo.SetActiveCollider(0.3f);
        golemBoss.PlayImpacts(strikeTarget);
        yield return new WaitForSeconds(0.4f);
        yield return MoveToPosition(hand, originalPosition, speed);

        onComplete?.Invoke();
    }

    public void CrossHands(float speed, float crossSpeed)
    {
        if (isLeftHandMoving || isRightHandMoving) return;

        isLeftHandMoving = true;
        isRightHandMoving = true;

        StartCoroutine(CrossHandRoutine(speed, crossSpeed, () =>
        {
            isLeftHandMoving = false;
            isRightHandMoving = false;
        }));
    }

    private IEnumerator CrossHandRoutine(float speed, float crossSpeed, System.Action onComplete)
    {
        Vector3 originalLeftPosition = leftHand.position;
        Vector3 originalRightPosition = rightHand.position;

        Vector3 leftSidePosition = player.position + player.right * -5f;
        Vector3 rightSidePosition = player.position + player.right * 5f;

        yield return MoveHandsToPosition(leftHand, rightHand, leftSidePosition, rightSidePosition, speed);
        yield return new WaitForSeconds(0.2f);

        Vector3 leftEndPosition = player.position + player.right * -1f;
        Vector3 rightEndPosition = player.position + player.right * 1f;

        yield return MoveHandsToPosition(leftHand, rightHand, leftEndPosition, rightEndPosition, crossSpeed);

        golemBoss.PlayImpacts(leftHand.position);
        golemBoss.PlayImpacts(rightHand.position);

        leftHandCompo.SetActiveCollider(0.2f);
        rightHandCompo.SetActiveCollider(0.2f);
        
        yield return new WaitForSeconds(0.15f);
        yield return MoveHandsToPosition(leftHand, rightHand, originalLeftPosition, originalRightPosition, speed);

        onComplete?.Invoke();
    }

    public void TakeDownHandAndShoot(bool isLeft, int bulletCount, float speed, float downSpeed, Pattern pattern)
    {
        if (isLeft)
        {
            if (isLeftHandMoving) return;
            isLeftHandMoving = true;
            StartCoroutine(TakeDownHandAndShootRoutine(leftHand, leftHandCompo, bulletCount, speed, downSpeed, pattern, () => isLeftHandMoving = false));
        }
        else
        {
            if (isRightHandMoving) return;
            isRightHandMoving = true;
            StartCoroutine(TakeDownHandAndShootRoutine(rightHand, rightHandCompo, bulletCount, speed, downSpeed, pattern, () => isRightHandMoving = false));
        }
    }

    private IEnumerator TakeDownHandAndShootRoutine(Transform hand, GolemHand handCompo, int bulletCount, float speed, float downSpeed, Pattern pattern, System.Action onComplete)
    {
        Vector3 originalPosition = hand.position;
        yield return MoveToPosition(hand, hand.position + Vector3.up * 2, speed);
        yield return MoveToPosition(hand, originalPosition, downSpeed);
        golemBoss.PlayImpacts(hand.position);
        
        if (pattern == Pattern.Bullet)
            bulletPattern.ApplyCircleShot(hand, bulletCount);
        else if (pattern == Pattern.Rock)
            CreateRock(bulletCount);

        onComplete?.Invoke();
    }

    private void CreateRock(int rockCount)
    {
        for (int i = 0; i < rockCount; i++)
        {
            float randX = Random.Range(minSize.x, maxSize.x);
            float randY = Random.Range(minSize.y, maxSize.y);

            var evt = SpawnEvents.RockCreate;
            evt.position = new Vector2(randX, 12);
            evt.poolType = PoolType.Rock;
            evt.direction = new Vector2(randX, randY);
            evt.fallTime = 1.5f;

            spawnEventChannel.RaiseEvent(evt);
        }
    }

    private IEnumerator MoveHandsToPosition(Transform hand1, Transform hand2, Vector3 target1, Vector3 target2, float speed)
    {
        Coroutine move1 = StartCoroutine(MoveToPosition(hand1, target1, speed));
        Coroutine move2 = StartCoroutine(MoveToPosition(hand2, target2, speed));
        yield return move1;
        yield return move2;
    }

    private IEnumerator MoveToPosition(Transform obj, Vector3 target, float speed)
    {
        float journeyLength = Vector3.Distance(obj.position, target);
        float startTime = Time.time;

        while (obj.position != target)
        {
            float easingValue = golemBoss.GetEasing(startTime, journeyLength, speed);
            obj.position = Vector3.Lerp(obj.position, target, easingValue);
            yield return null;
        }
    }
}
