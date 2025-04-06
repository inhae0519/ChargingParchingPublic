using UnityEngine;

public class BashUtils
{
    public static Vector2 LimitedSpeed(Vector2 currentSpeed, Vector2 addSpeed,float maxSpeed)
    {
        Vector2 dir = addSpeed.normalized * Mathf.Lerp(1, 0, ((Vector2)Vector3.Project(addSpeed, currentSpeed) + currentSpeed).magnitude / maxSpeed);
        return dir;
    }
}
public class BashPair<T1,T2>
{
    public T1 First;
    public T2 Second;
    public BashPair(T1 first, T2 second)
    {
        First = first; Second = second;
    }

}