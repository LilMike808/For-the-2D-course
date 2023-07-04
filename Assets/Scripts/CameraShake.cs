using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 _initialPosition;
    private bool _isShaking;
    private bool _gameOver;

   
    void Start()
    {
        _initialPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_isShaking == true && _gameOver == false)
        {
            StartCoroutine(ShakingRoutine());
        }
        else if(_isShaking == false)
        {
            transform.position = _initialPosition;
        }
    }
    public void ShakeCamera()
    {
        _isShaking = true;
        StartCoroutine(ShakeTimeRoutine());
    }
    public void GameOver()
    {
        _gameOver = true;
    }
    IEnumerator ShakeTimeRoutine()
    {
        yield return new WaitForSeconds(0.45f);
        _isShaking = false;
    }
    IEnumerator ShakingRoutine()
    {
        while(_isShaking)
        {
            transform.position = _initialPosition;
            yield return new WaitForSeconds(0.1f);

            transform.position = new Vector3(transform.position.x + 0.03f, transform.position.y + 0.03f, transform.position.z + 0.03f);
            yield return new WaitForSeconds(0.2f);

            transform.position = _initialPosition;
            yield return new WaitForSeconds(0.1f);

            transform.position = new Vector3(transform.position.x + 0.04f, transform.position.y + 0.04f, transform.position.z + 0.04f);
            yield return new WaitForSeconds(0.2f);
        }
    }
   
}
