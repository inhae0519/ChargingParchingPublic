using System.Collections.Generic;
using UnityEngine;

public class NoMom : MonoBehaviour
{
    private void OnEnable()
    {
        while(transform.childCount > 0)
        {
           transform.GetChild(0).parent = null;
        }

        Destroy(gameObject);
    }
}
