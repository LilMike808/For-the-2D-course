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
    private Animator _anim;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private int _ammoCount = 15;
    [SerializeField]
    private GameObject _shieldVisualizer;
    private int _shieldStrength;
    SpriteRenderer _shieldColor;
    private SpriteRenderer _spriteRenderer;
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
    private GameObject _speedFire;
    [SerializeField]
    private GameObject _homingMissilePrefab;
    [SerializeField]
    private GameObject _iceShards;
    [SerializeField]
    private GameObject _areaDisplacementAnimation;
    [SerializeField]
    private GameObject _mPulseAnimation;
    [SerializeField]
    private GameObject _xRod1;
    [SerializeField]
    private GameObject _xRod2;
    private int _missileCount = 3;
    private bool _isHomingMissileActive = false;
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
    private AudioManager _audioManager;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;
    private bool _isGiantLasersActive = false;
    private bool _isSlowSpeedActive = false;
    private bool _isDamagedActive = false;
    private bool _isMPulseRefilled = true;
    private bool _areXRodsRefilled = true;
    private bool _ifOrbIsDestroyed = false;
    private bool _isBossWaveActive = false;
    private bool _dontInterruptSpeedMusic = false;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioSource _alarmSoundEffect;
    [SerializeField]
    private AudioSource _jetSoundEffect;
    [SerializeField]
    private AudioSource _hitSoundEffect;
    [SerializeField]
    private AudioSource _laserCrashSound;
    [SerializeField]
    private AudioSource _shieldHitSound;
    [SerializeField]
    private AudioSource _repairSound;
    [SerializeField]
    private AudioSource _weaponReloadSound;
    [SerializeField]
    private AudioSource _outOfAmmoSound;
    [SerializeField]
    private AudioSource _collectMissileSound;
    [SerializeField]
    private AudioSource _freezeSound;
    [SerializeField]
    private AudioSource _superHeroMusic;
    [SerializeField]
    private AudioSource _shieldPowerupSound;
    [SerializeField]
    private AudioSource _MPulseSound;
    // Start is called before the first frame update
    void Start()
    {       
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _shieldColor = _shieldVisualizer.GetComponent<SpriteRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _audioManager = GameObject.FindObjectOfType<AudioManager>();
        _camShake = Camera.main.GetComponent<CameraShake>();
        _anim = GetComponent<Animator>();
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
        CalculateMovement();
        MPulse();
        XRods();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _isHomingMissileActive != true)
        {
            if (_ammoCount == 0)
            {
                _outOfAmmoSound.Play();
                return;
            }
            FireLaser();
        }
        if (Input.GetKeyDown(KeyCode.Space) && _isHomingMissileActive == true)
        {
            if(_missileCount == 0)
            {
                return;
            }
            HomingMissileFire();
        }
    }
    void CalculateMovement()
    {

        //Purely for key map inputs in Unity. Created local variables here which are executed in Player movement, thus global variables aren't needed.
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        //prior if statement prohibited Player from moving above 0 and below -4.8f on the Y which our mathf.Clamp equation now executes.
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.8f, 0), 0);

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }

        if (_isSpeedBoostActive == true && _isSlowSpeedActive == false)
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
                transform.Translate(direction * (_speed * 2.2f) * Time.deltaTime);
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && _lives > 0)
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
                    transform.Translate(direction * _speed * Time.deltaTime);
                }
            }
        }
        else if (_isSlowSpeedActive == true)
        {
            transform.Translate(direction * (_speed / 13) * Time.deltaTime);
            if(Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(direction * (_speed / 13) * Time.deltaTime);
            }
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
    }
    void FireLaser()
    {     
        if (_ammoCount <= 15 && _lives > 0)
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
        if (_lives > 0)
        {
            _canFire = Time.time + _fireRate;
            _audioSource.Play();
        }
    }
    public void KeepSpeedMusicFromPlaying()
    {
        _isBossWaveActive = true;
    }
    public void IsThePlayerLaser()
    {
        _laserCrashSound.Pause();
    }
    public void IsNotThePlayerLaser()
    {
        if(_isShieldsActive == true)
        {
            _laserCrashSound.Pause();
            _shieldHitSound.Play();
            return;
        }
        else if(_isShieldsActive == false)
        {
            if (_lives >= 1)
            {
                _laserCrashSound.Play();
            }
        }
    }
    void HomingMissileFire()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _isHomingMissileActive == true && _lives > 0)
        {
            Instantiate(_homingMissilePrefab, transform.position, Quaternion.identity);
            _missileCount = _missileCount - 1;

            if(_missileCount == 0)
            {
                _isHomingMissileActive = false;
            }
        }
    }
    void MPulse()
    {
        if (Input.GetKey(KeyCode.M) && _lives > 0)
        {
            StartCoroutine(MPulseRoutine());
        }
    }
    void XRods()
    {
        if(Input.GetKey(KeyCode.X) && _lives > 0)
        {
            StartCoroutine(XRodRoutine());
        }
    }
    IEnumerator MPulseRoutine()
    {
        if (_isMPulseRefilled == true)
        {
            _MPulseSound.Play();
            _mPulseAnimation.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            _mPulseAnimation.gameObject.SetActive(false);
            _isMPulseRefilled = false;
             _uiManager.MPulseUI();
            yield return new WaitForSeconds(6f);
           
            _isMPulseRefilled = true;
        }
    }
    IEnumerator XRodRoutine()
    {
        if(_areXRodsRefilled == true)
        {
            _xRod1.gameObject.SetActive(true);
            _xRod2.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            _xRod1.gameObject.SetActive(false);
            _xRod2.gameObject.SetActive(false);
            _areXRodsRefilled = false;
             _uiManager.XRodUI();
            yield return new WaitForSeconds(12f);          
            _areXRodsRefilled = true;
        }
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
        if(_fuelPercentage > 0 && _lives > 0)
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
        if (_lives > 0)
        {
            _ammoCount = 15;
            _weaponReloadSound.Play();
            AmmoCap();
            _uiManager.UpdateAmmo(_ammoCount);
        }
    }
    public void HealthCollected()
    {
        _lives = _lives + 1;
        if (_lives > 0)
        {
            _repairSound.Play();
        }
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
        _camShake.ShakeCamera();
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
        _lives--;

        if(_lives == 2)
        {
            StartCoroutine(WhileDamaged());
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            StartCoroutine(WhileDamaged());
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _speed = 0;
            _audioManager.KillBackgroundMusic();
            KeepSpeedMusicFromPlaying();
            _superHeroMusic.Stop();
            _isSlowSpeedActive = false;
            _isSpeedBoostActive = false;
            Destroy(GetComponent<Collider2D>());       
            _xRod1.gameObject.SetActive(false);
            _xRod2.gameObject.SetActive(false);
            _rightEngine.gameObject.SetActive(false);
            _leftEngine.gameObject.SetActive(false);
            _anim.SetTrigger("OnEnemyDeath");
            _thruster.SetActive(false);
            _camShake.GameOver();
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject, 2.3f);
        }
    }
    public void TripleShotActive()
    {
        if (_lives > 0)
        {
            _ammoCount = 15;
            AmmoCap();
            _uiManager.UpdateAmmo(_ammoCount);
            _isTripleShotActive = true;
            StartCoroutine(TripleShotPowerDownRoutine());
        }
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
    public void SpeedBoostActive()
    {
        if (_isSlowSpeedActive == false && _dontInterruptSpeedMusic == false && _lives > 0)
        {
            _isSpeedBoostActive = true;
            _speedFire.gameObject.SetActive(true);
            if (_isBossWaveActive == false && _dontInterruptSpeedMusic == false && _lives > 0)
            {
                //the bool is to keep superhero music from triggering again if another speed powerup is collected while boost is active.
                _dontInterruptSpeedMusic = true;              
                _superHeroMusic.Play();
            }
            else
            {
                _superHeroMusic.Pause();      
            }
            _audioManager.SimplyTurnOffBackgroundMusic();
            StartCoroutine(SpeedBoostPowerDownRoutine());
        }
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speedFire.gameObject.SetActive(false);
        _dontInterruptSpeedMusic = false;
        _isSpeedBoostActive = false;
        _audioManager.RestartBackgroundMusic();
        _superHeroMusic.Stop();
    }
    public void SlowSpeedActive()
    {
        if (_isSpeedBoostActive == false && _lives > 0)
        {
            _isSlowSpeedActive = true;
            _freezeSound.Play();
            StartCoroutine(SlowSpeedPowerDown());
        }
    }
    IEnumerator SlowSpeedPowerDown()
    {
        StartCoroutine(WhileFrozen());
        yield return new WaitForSeconds(5.0f);
        _isSlowSpeedActive = false;
    }
    IEnumerator WhileFrozen()
    {
        if (_lives > 0)
        {
            while (_isSlowSpeedActive == true)
            {
                _spriteRenderer.color = new Color(0, 109, 255, 255);
                _iceShards.SetActive(true);
                yield return new WaitForSeconds(0.1f);
            }
            if (_isSlowSpeedActive == false)
            {
                _spriteRenderer.color = new Color(255, 255, 255, 255);
                _iceShards.SetActive(false);
            }
        }
    }
    public void MultiDirectionalLasersActive()
    {
        if (_lives > 0)
        {
            _ammoCount = 15;
            AmmoCap();
            _uiManager.UpdateAmmo(_ammoCount);
            _isGiantLasersActive = true;
            StartCoroutine(GiantLaserPowerDownRoutine());
        }
    }
    IEnumerator GiantLaserPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isGiantLasersActive = false;
    }
    public void ShieldsActive()
    {
        _shieldPowerupSound.Play();
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);

        if (_shieldStrength < 3 && _lives > 0)
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
    public void HomingMissileActive()
    {
        _isHomingMissileActive = true;
        _collectMissileSound.Play();
        _missileCount = 3;
    }
    IEnumerator WhileDamaged()
    {
        _isDamagedActive = true;
        StartCoroutine(TookAHit());
        yield return new WaitForSeconds(0.8f);
        _isDamagedActive = false;
    }

    IEnumerator TookAHit()
    {
        while(_isDamagedActive == true)
        {
            yield return new WaitForSeconds(.13f);
            _spriteRenderer.color = new Color(1, 0.240556f, 0.240556f);
            yield return new WaitForSeconds(.13f);
            _spriteRenderer.color = new Color(3, 1, 2);
        }
    }
    public void IfOrbIsDestroyed()
    {
        _ifOrbIsDestroyed = true;
        StartCoroutine(TurnOrbBool());
    }
    IEnumerator TurnOrbBool()
    {
        yield return new WaitForSeconds(2f);
        _ifOrbIsDestroyed = false;
    }
    IEnumerator OrbBlink()
    {
        _alarmSoundEffect.Play();
        yield return new WaitForSeconds(2f);
        if(_ifOrbIsDestroyed == false && _lives > 0)
        {
            StartCoroutine(AreaDisplacement());
        }
    }
    IEnumerator AreaDisplacement()
    {
        _spriteRenderer.color = new Color(0, 0, 0, 255);
        Instantiate(_areaDisplacementAnimation, transform.position, Quaternion.identity);
        _jetSoundEffect.Play();
        yield return new WaitForSeconds(0.2f);
        transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-7.5f, 0), 0);
        Instantiate(_areaDisplacementAnimation, transform.position, Quaternion.identity);
        _jetSoundEffect.Play();
        _spriteRenderer.color = new Color(255, 255, 255, 255);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage();
                if (_lives > 0)
                {
                    _hitSoundEffect.Play();
                }
            }
        }
        if (other.tag == "Enemy2")
        {
            if (_lives > 0)
            {
                _hitSoundEffect.Play();
            }
        }
        if (other.tag == "Enemy3")
        {
            Enemy3 enemy3 = other.transform.GetComponent<Enemy3>();
            if (enemy3 != null)
            {
                enemy3.Damage();
                if (_lives > 0)
                {
                    _hitSoundEffect.Play();
                }
            }
        }
        if (other.tag == "Enemy4")
        {
            Enemy4 enemy4 = other.transform.GetComponent<Enemy4>();
            if (enemy4 != null)
            {
                enemy4.Damage();
                if (_lives > 0)
                {
                    _hitSoundEffect.Play();
                }
            }
        }
        if (other.tag == "Enemy5")
        {
            Enemy5 enemy5 = other.transform.GetComponent<Enemy5>();
            if (enemy5 != null)
            {
                enemy5.Damage();
                _hitSoundEffect.Play();
            }
        }
        if (other.tag == "Orb")
        {
            PhantasmicDot orb = other.transform.GetComponent<PhantasmicDot>();
            if (orb != null)
            {
                StartCoroutine(OrbBlink());
            }
        }
        if (other.tag == "Lightning")
        {
            Damage();
            _laserCrashSound.Play();
        }
        if (other.tag == "RotatingBox")
        {
            Damage();
            _laserCrashSound.Play();
        }
        if (other.tag == "Expansion Ring")
        {
            Damage();
        }
    }
}
