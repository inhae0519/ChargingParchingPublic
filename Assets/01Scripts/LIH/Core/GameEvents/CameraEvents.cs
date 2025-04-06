using UnityEngine;

public static class CameraEvents
{
    public static CamDistanceChange CamDistanceChangeEvent = new CamDistanceChange();
    public static CamShake CamShakeEvent = new CamShake();
    public static CamDistanceReset CamDistanceResetEvent = new CamDistanceReset();
    public static CamOffsetChange CamOffsetChangeEvent = new CamOffsetChange();
}

public class CamOffsetChange : GameEvent
{
    public Vector2 postion;
    public Vector2 targetPos;
    public float radius;
}

public class CamDistanceChange : GameEvent
{
    public float distance;
    public float speed;
}

public class CamDistanceReset : GameEvent
{
    public float speed;
}

public class CamShake : GameEvent
{
    public float intensity;
}