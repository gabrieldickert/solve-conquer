using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTexts : MonoBehaviour
{
    public float destroyTime = 3f;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

}
