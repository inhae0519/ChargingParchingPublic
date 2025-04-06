using UnityEngine;

[CreateAssetMenu(fileName = "AnimParamSO", menuName = "SO/Anim/ParamSO")]
public class AnimStateSO : ScriptableObject
{
    public string ParamName;
    public int HashValue;

    private void OnValidate()
    {
        HashValue = Animator.StringToHash(ParamName);
    }
}
