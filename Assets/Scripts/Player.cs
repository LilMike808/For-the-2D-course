using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour    
{
    //delete this comment when C# "Variables" is completed.
    //delete this comment when C# "If statements" is completed.
    //delete this comment when C# "Coroutines" is completed.
    //look up C# "Switch Statments".
    private CameraShake _camShake;
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private int _ammoCount = 15;
    [SerializeField]
    private GameObject _shieldVisualizer;
    private int _shieldStrength;
    SpriteRenderer _shieldColor;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _multiDirectionalLasersPrefab;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private float _fuelPercentage = 100f;
    [SerializeField]
    private float _refuelSpeed;
    private bool _isThrusterActive;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score;
    private SpawnManager _spawnManager;
    private UI_Manager _uiManager;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;
    private bool _isGiantLasersActive = false;
    private bool _isSlowSpeedActive = false;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        //snaps player to position on Game Start.
        transform.position = new Vector3(0, 0, 0);

        //Object in T brackets is what we have access to. In this case "SpawnManager". 
        //If "Transform", object in T bracket will say "Transform". private variable would read "private Transform _spawnManager"
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _shieldColor = _shieldVisualizer.GetComponent<SpriteRenderer>();
         _audioSource = GetComponent<AudioSource>();
        _camShake = Camera.main.GetComponent<CameraShake>();
        
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }
        if(_audioSource == null)
        {
            Debug.LogError("The Audio Source on the Player is NULL");
        }
        else
        {
            //.clip is a Unity function that allows audioclips to be played. 
            _audioSource.clip = _laserSoundClip;
        }
        if(_shieldColor == null)
        {
            Debug.LogError("The shield colors are NULL");
        }
        if(_camShake == null)
        {
            Debug.Log("Camera Shake script is NULL");
        }
       
       
    }
    // Update is called once per frame
    void Update()
    {
        //because of how update functions, this method must be called here.
        CalculateMovement();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if(_ammoCount == 0)
            {
                return;
            }
            FireLaser();
        }     
    }
    void CalculateMovement()
    {

        //Purely for key map inputs in Unity. Created local variables here which are executed in Player movement, thus global variables aren't needed.
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //The process of moving the player along X and Y axis is called "Translating"

        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        //prior if statement prohibited Player from moving above 0 and below -4.8f on the Y which our mathf.Clamp equation now executes.
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.8f, 0), 0);

        //Warps player to opposite side on X axis once past 11, -11. 
        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }

        if (_isSpeedBoostActive == true)
        {
            transform.Translate(direction * (_speed * 3f) * 1 * Time.deltaTime);
        }

        //left shift key 'booster' method
        if (Input.GetKey(KeyCode.LeftShift) && _fuelPercentage > 0 && _isSlowSpeedActive == false)
        {
            if (_isSpeedBoostActive)
            {
                ActivateThruster();
                StopCoroutine(ActivateRefuel());
            }
            else
            {
                ActivateThruster();
                StopCoroutine(ActivateRefuel());
                transform.Translate(direction * (_speed * 1.7f) * Time.deltaTime);
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isThrusterActive = false;
            {
                if (_isSpeedBoostActive)
                {
                    _thruster.SetActive(false);
                    StartCoroutine(ActivateRefuel());
                }
                else
                {
                    _thruster.SetActive(false);
                    StartCoroutine(ActivateRefuel());
                    transform.Translate(direction * (_speed * 1.7f) * Time.deltaTime);
                }
            }
        }
        else if (_isSlowSpeedActive == true)
        {
            transform.Translate(direction * (_speed / 10) * Time.deltaTime);
            if(Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(direction * (_speed / 7) * Time.deltaTime);
            }
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
    }
    void FireLaser()
    {

        if (_ammoCount <= 15)
        {
            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                _ammoCount = _ammoCount - 3;
                _uiManager.UpdateAmmo(_ammoCount);
            }
            else if (_isGiantLasersActive == true)
            {
                Instantiate(_multiDirectionalLasersPrefab, transform.position, Quaternion.identity);
                _ammoCount = _ammoCount - 1;
                _uiManager.UpdateAmmo(_ammoCount);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
                _ammoCount = _ammoCount - 1;
                _uiManager.UpdateAmmo(_ammoCount);
            }
            AmmoCap();
        }
        
        _canFire = Time.time + _fireRate;
        _audioSource.Play();
    }
    void AmmoCap()
    {
        if(_ammoCount <= 0)
        {
            _ammoCount = 0;
        }
        else if(_ammoCount >= 15)
        {
            _ammoCount = 15;
        }
    }
    void ActivateThruster()
    {
        _isThrusterActive = true;
        if(_fuelPercentage > 0)
        {
            _thruster.SetActive(true);
            _fuelPercentage -= 15 * 2 * Time.deltaTime;
            _uiManager.UpdateThruster(_fuelPercentage);
        }
        else if(_fuelPercentage <= 0)
        {
            _thruster.SetActive(false);
            _fuelPercentage = 0.0f;
            _uiManager.UpdateThruster(_fuelPercentage);
        }
    }
    IEnumerator ActivateRefuel()
    {
        while(_fuelPercentage != 100 && _isThrusterActive == false)
        {
            yield return new WaitForSeconds(1.0f);
            _fuelPercentage += 30 * _refuelSpeed * Time.deltaTime;
            _uiManager.UpdateThruster(_fuelPercentage);

            if (_fuelPercentage >= 100)
            {
                _fuelPercentage = 100;
                _uiManager.UpdateThruster(_fuelPercentage);
                break;
            }
            else if (_fuelPercentage <= 0)
            {
                _fuelPercentage = 0;
                _uiManager.UpdateThruster(_fuelPercentage);
                break;

            }
            
        }
    }
    public void AmmoCollected()
    {
        _ammoCount = 15;
        AmmoCap();
        _uiManager.UpdateAmmo(_ammoCount);
    }
    public void HealthCollected()
    {
        _lives = _lives + 1;
        if(_lives >= 3)
        {
            _lives = 3;
            _rightEngine.SetActive(false);
        }
        else if (_lives == 2)
        {
            _leftEngine.SetActive(false);
        }
        _uiManager.UpdateLives(_lives);
    }
    public void Damage()
    {
        StartCoroutine(_camShake.ShakeCamera());

        if (_isShieldsActive == true)
        {
            _shieldStrength--;
            if (_shieldStrength == 0)
            {
                //No method for pausing lives required. When Damage gets called, it asks if shields are active. if they are, 'return' stops Damage method after. 
                _isShieldsActive = false;
                _shieldVisualizer.SetActive(false);
                return;
            }
            else if (_shieldStrength == 1)
            {
                _shieldColor.color = Color.red;
                return;
            }
            else if (_shieldStrength == 2)
            {
                _shieldColor.color = Color.green;
                return;
            }
        }
        _lives --;

        if(_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
    }
    public void SlowSpeedActive()
    {
        _isSlowSpeedActive = true;
        StartCoroutine(SlowSpeedPowerDown());
    }
    IEnumerator SlowSpeedPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isSlowSpeedActive = false;
    }
    public void MultiDirectionalLasersActive()
    {
        _isGiantLasersActive = true;
        StartCoroutine(GiantLaserPowerDownRoutine());
    }
    IEnumerator GiantLaserPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isGiantLasersActive = false;
    }
    public void ShieldsActive()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);

        if (_shieldStrength < 3)
        {
            _shieldStrength++;
            _isShieldsActive = true;
            _shieldVisualizer.SetActive(true);
        }
        if (_shieldStrength == 1)
        {
            _shieldColor.color = Color.red;
        }
        else if (_shieldStrength == 2)
        {
            _shieldColor.color = Color.green;
        }
        else if (_shieldStrength == 3)
        {
            _shieldColor.color = Color.cyan;
        }
    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
       
    }
   
}
