using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundStarMovement : MonoBehaviour
{
    private int _speed;
   // Start is called before the first frame update
    void Start()
    {
        _speed = Random.Range(2, 12);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -9)
        {
            Destroy(this.gameObject);
        }
    }
}
