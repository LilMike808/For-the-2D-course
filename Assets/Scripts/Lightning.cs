using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void LightningIntercepts()
    {
        Enemy5 enemy5 = FindObjectOfType<Enemy5>();
        if(enemy5 != null)
        {
            enemy5.LaserHitsLightning();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
