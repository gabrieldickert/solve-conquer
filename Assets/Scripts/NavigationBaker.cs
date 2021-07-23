using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{
    public NavMeshSurface[] surfaces;

    float TimeInterval;
    int secondsBetweenBakes = 1;

    void LateUpdate()
    {
        // ones per in seconds
        TimeInterval += Time.deltaTime;
        if (TimeInterval >= secondsBetweenBakes)
        {
            TimeInterval = 0;
            Bake();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Bake();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void Bake()
    {
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }
}
