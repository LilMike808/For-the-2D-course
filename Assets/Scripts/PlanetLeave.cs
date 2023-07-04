using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetLeave : MonoBehaviour
{
    private bool _isPlanetDownActive = false;
    private bool _isPlanetDownSlowerActive = false;
    private bool _isGreenPlanetActive = false;
    [SerializeField]
    private GameObject _wave3Planet;
    [SerializeField]
    private GameObject _introPlanet;
    [SerializeField]
    private GameObject _greenPlanet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (_isPlanetDownSlowerActive == true)
        {
            if (_wave3Planet != null && _wave3Planet.transform.position.y > -15)
            {
                _wave3Planet.gameObject.transform.Translate(Vector3.down * 0.3f * Time.deltaTime);
            }
        }
        if(_isGreenPlanetActive == true)
        {
            if(_greenPlanet != null && _greenPlanet.transform.position.y > -18)
            {
                _greenPlanet.gameObject.transform.Translate(Vector3.down * .15f * Time.deltaTime);
            }
        }
        if (_isPlanetDownActive == true)
        {
            if (_introPlanet != null && _introPlanet.transform.position.y > -20)
            {
                _introPlanet.gameObject.transform.Translate(Vector3.down * 12 * Time.deltaTime);
            }
        }

        if(_introPlanet.transform.position.y == -20)
        {
            Destroy(_introPlanet.gameObject);
            if(_introPlanet.transform.position.y == -20)
            {
                _introPlanet = null;
            }
        }
        if(_wave3Planet.transform.position.y == -15)
        {
            Destroy(_wave3Planet.gameObject);
            if(_wave3Planet.transform.position.y == -15)
            {
                _wave3Planet = null;
            }
        }
        if(_greenPlanet.transform.position.y == -18)
        {
            Destroy(_greenPlanet.gameObject);
            if(_greenPlanet.transform.position.y == -18)
            {
                _greenPlanet = null;
            }
        }
    }
    public void Down()
    {
        _isPlanetDownActive = true;
    }
    public void DownSlower()
    {
        _isPlanetDownSlowerActive = true;
    }
    public void DownLater()
    {
        _isGreenPlanetActive = true;
    }
}
