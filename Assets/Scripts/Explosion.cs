using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {       
        Destroy(this.gameObject, 3.0f);
    }
    private void Update()
    {
        transform.Translate(Vector3.down * 12 * Time.deltaTime);
    }

}
