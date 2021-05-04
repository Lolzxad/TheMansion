using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(2, 11, true);
        Physics2D.IgnoreLayerCollision(2, 10, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
