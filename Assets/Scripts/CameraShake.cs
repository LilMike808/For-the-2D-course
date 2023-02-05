using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float _newFOV = 10f;
    [SerializeField]
    private float _newXPos = 0.25f;
    [SerializeField]
    private float _newYPos = 0.25f;
    [SerializeField]
    private float _newRotation = 2f;
    [SerializeField]
    private float _duration = 0.1f;

    //original values
    private float _originalFOV;
    private Vector3 _originalPOS;

    //accompanying variables
    private WaitForSeconds _delay;
    private Vector3 _newPOS;
    private Vector3 _invertXYPos;
    private Vector3 _newRot;

    // Start is called before the first frame update
    void Start()
    {
        //initialize original camera values
        _originalFOV = Camera.main.fieldOfView;
        _originalPOS = Camera.main.transform.position;

        //initialize help variables with the new values
        _delay = new WaitForSeconds(_duration / 3f);
        _newPOS = new Vector3(_newXPos, -_newYPos, Camera.main.transform.position.z);
        _invertXYPos = new Vector3(0, 0, _newRotation);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator ShakeCamera()
    {
        //change values inbetween delays to simulate camera shake
        Camera.main.fieldOfView = _newFOV;
        yield return _delay;

        Camera.main.transform.position = Vector3.Scale(_invertXYPos, _newPOS);
        Camera.main.transform.eulerAngles = _newRot;
        yield return _delay;

        //return to normal state
        Camera.main.fieldOfView = _originalFOV;
        Camera.main.transform.position = _originalPOS;
        Camera.main.transform.eulerAngles = Vector3.zero;
    }
}
