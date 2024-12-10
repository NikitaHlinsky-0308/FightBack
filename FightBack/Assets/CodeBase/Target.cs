using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Target : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage(Vector3 punchDirection)
    {
        Debug.Log("TakeDamage");
        transform.DOPunchPosition(punchDirection * 0.3f, 0.4f);
    }
}
