using UnityEngine;

public class EnableToggle : MonoBehaviour
{
    public GameObject aa;
    private void Start()
    {
        Invoke("DIs", 1f);
    }
   void DIs()
    {
        gameObject.SetActive(false);
    }
    public void EnableToggler(bool aaa)
    {
        gameObject.SetActive(aaa);
        //aa.SetActive(!aa.activeInHierarchy);
    }
}
