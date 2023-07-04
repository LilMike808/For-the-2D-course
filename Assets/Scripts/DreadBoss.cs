using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreadBoss : MonoBehaviour
{
    private Vector3 _currentPos;
    private Vector3 _startPosition = new Vector3(0, 0, 0);
    private Vector3 _leftPoint = new Vector3(-7, 0, 0);
    private Vector3 _rightPoint = new Vector3(7, 0, 0);
    private Vector3 _upRightPoint = new Vector3(4.3f, 2f, 0);
    private Vector3 _upLeftPoint = new Vector3(0, 5, 0);
    private Vector3 _topRightPoint = new Vector3(7, 5, 0);
    private Vector3 _topLeftPoint = new Vector3(-7, 5, 0);
    private Vector3 _zipDownLeftPoint = new Vector3(-7, -3, 0);
    private Vector3 _zipUpLeftPoint = new Vector3(2, 3.7f, 0);
    private Vector3 _zipDownRightPoint = new Vector3(6.3f, -3, 0);
    private Vector3 _zipUpRightPoint = new Vector3(0, 7, 0);

    private Vector3 _deathJoltRight1 = new Vector3(3, 2f, 0);
    private Vector3 _deathJoltLeft1 = new Vector3(-3, -2f, 0);
    private Vector3 _deathJoltRight2 = new Vector3(3, -2f, 0);
    private Vector3 _deathJoltLeft2 = new Vector3(-3, 2, 0);
        
    private Vector3 _explosionPoint1 = new Vector3(-.7f, 1, 0);
    private Vector3 _explosionPoint2 = new Vector3(-.3f, -.6f, 0);
    private Vector3 _explosionPoint3 = new Vector3(.2f, -1, 0);
    private Vector3 _explosionPoint4 = new Vector3(0f, 1.5f, 0);
    private Vector3 _explosionPoint5 = new Vector3(.8f, -1.5f, 0);
    private Vector3 _explosionPoint6 = new Vector3(0, 0, 0);
    private Vector3 _explosionPoint7 = new Vector3(1, 1.5f, 0);
    private Vector3 _explosionPoint8 = new Vector3(1, 0, 0);
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _smallExplosion;
    [SerializeField]
    private GameObject _shieldVisualiser;
    [SerializeField]
    private GameObject _basicLasersPrefab;
    [SerializeField]
    private GameObject _bigBasicLaser;
    [SerializeField]
    private GameObject _grenadePrefab;
    [SerializeField]
    private GameObject _asteroidPrefab;
    [SerializeField]
    private GameObject _specialLazers;
    [SerializeField]
    private GameObject _dialupAnim;
    [SerializeField]
    private AudioSource _laserCrashSound;
    [SerializeField]
    private AudioSource _shieldHitSound;
    [SerializeField]
    private AudioSource _grenadeLaunchSound;
    [SerializeField]
    private AudioSource _bigLaserFireSound;
    [SerializeField]
    private AudioSource _specialLasersSound;
    [SerializeField]
    private AudioSource _normalLaserSound;
    [SerializeField]
    private AudioSource _malfunctionSound;
    [SerializeField]
    private AudioSource _dialupSound;
    [SerializeField]
    private AudioSource _smallExplosionSound;
    private SpriteRenderer _spriteRenderer;
    SpriteRenderer _turnOff;
    private UI_Manager _uiManager;
    private GameManager _gameManager;
    private Player _player;
    private SpawnManager _spawnManager;
    private Animator _anim;
    [SerializeField]
    private int _healthPercentage;
    private float _fireRate;
    private float _bigFireRate;
    private float _asteroidFireRate;
    private float _specialFireRate;
    private float _canFire = -1f;
    private float _bigCanFire = -1f;
    private float _asteroidCanFire = -1f;
    private float _specialCanFire = -1f;
    private float _speed = 3f;
    [SerializeField]
    private int _shieldStrength;
    private bool _isShieldsActive = false;
    private bool _movedRight;
    private bool _movedUpRight;
    private bool _movedTopRight;
    private bool _zippedDownLeft;
    private bool _zippedUpLeft;
    private bool _zippedUpRight;
    private bool _zippedDownRight;
    private bool _wasHitByMissile = false;
    private bool _isBigLaserActive = false;
    private bool _areBasicLasersActive = false;
    private bool _isRainDownAsteroidsActive = false;
    private bool _areSpecialLasersActive = false;
    private bool _isVulnerable = false;
    private bool _isEnumWaitingActive = false;
    private bool _isEnumMoveAndShootActive = false;
    private bool _isEnumMovingUpActive = false;
    private bool _isEnumZipAndLaunchGrenadeActive = false;
    private bool _isEnumZipMovementsActive = false;
    private bool _isEnumAsteroidAttackActive = false;
    private bool _isGrenadeActive = false;
    private bool _isDamagedActive = false;
    private bool _isDeathAnimationActive = false;
    private Grenade _grenade;
    public enum BossState
    {
        Start,
        Waiting,
        MovingAndShooting,
        MovingUp,
        ZippingAndLaunchingGrenades,
        stopallcoroutinesb4destroydot,
        DestroyDotExe,
        ZipDownAndUpOnBothSides,
        StopAllCoroutines,
        AsteroidAttack,
        stopallcoroutinesb4zoomdown,
        ZoomDown,
        SpecialAttack,
        Vulnerable
    }
    public BossState currentState;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _currentPos = transform.position;
        currentState = BossState.Start;

        _basicLasersPrefab.gameObject.SetActive(true);
        _bigBasicLaser.gameObject.SetActive(true);
        _asteroidPrefab.gameObject.SetActive(true);
        _specialLazers.gameObject.SetActive(true);
        _grenadePrefab.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Shields();
        if (_isBigLaserActive == true)
        {
            FireBigLaser();
        }
        if (_areBasicLasersActive == true)
        {
            FireBasicLasers();
        }
        if (_isGrenadeActive == true)
        {
            GrenadeLaunch();
        }
        if (_isRainDownAsteroidsActive == true)
        {
            RainDownAsteroids();
        }
        if (_areSpecialLasersActive == true)
        {
            SpecialLasers();
        }
        if(_healthPercentage < 1 && _isDeathAnimationActive == true)
        {
            StartCoroutine(ViolentJerks());
        }
        switch (currentState)
        {
            case BossState.Start:
                if (transform.position != _startPosition)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _startPosition, _speed * Time.deltaTime);
                }
                else
                {
                    currentState = BossState.Waiting;                   
                }
                break;
            case BossState.Waiting:
                _isEnumWaitingActive = true;
                _movedRight = false;
                StartCoroutine(WaitThenMoveShoot(2));              
                break;
            case BossState.MovingAndShooting:
                StopCoroutine(WaitThenMoveShoot(.00f));
                _isEnumWaitingActive = false;
                _isEnumMoveAndShootActive = true;
                _speed = 3;
                _isBigLaserActive = true;
                _areBasicLasersActive = true;
                if (_movedRight == false)
                {
                    MoveRight();
                }
                else
                {
                    MoveLeft();
                }
                StartCoroutine(SegueToMovingOnUp(7.9f));
                break;
            case BossState.MovingUp:
                _isEnumMoveAndShootActive = false;
                _isEnumMovingUpActive = true;
                if (_movedUpRight == false)
                {
                    MoveUpRight();
                }
                else
                {
                    MoveUpLeft();
                    if (_movedUpRight == true)
                    {
                        _areBasicLasersActive = false;
                    }
                }
                StartCoroutine(SegueToLaunchingGrenades(8f));
                break;
            case BossState.ZippingAndLaunchingGrenades:
                _isEnumMovingUpActive = false;
                _isEnumZipAndLaunchGrenadeActive = true;
                _isGrenadeActive = true;
                _isBigLaserActive = false;
                if (_movedTopRight == false)
                {
                    MoveTopRight();
                }
                else
                {
                    MoveTopLeft();
                }
                _zippedDownLeft = false;
                _zippedUpLeft = false;
                _zippedDownRight = false;
                _zippedUpRight = false;
                StartCoroutine(SegueIntoZipDown(8));
                break;
            case BossState.stopallcoroutinesb4destroydot:
                StopAllCoroutines();
                currentState = BossState.DestroyDotExe;
                break;
            case BossState.DestroyDotExe:
                currentState = BossState.ZipDownAndUpOnBothSides;
                break;
            case BossState.ZipDownAndUpOnBothSides:
                _isEnumZipAndLaunchGrenadeActive = false;
                _isEnumZipMovementsActive = true;

                _isGrenadeActive = false;
                if (_zippedDownLeft == false)
                {
                    ZipDownLeft();
                    _areBasicLasersActive = true;
                }
                else if (_zippedDownLeft == true && _zippedUpLeft == false)
                {
                    ZipUpOnTheLeft();
                    _areBasicLasersActive = false;
                }
                else if (_zippedDownRight == false)
                {
                    ZipDownRight();
                    _areBasicLasersActive = true;
                }
                else if (_zippedDownRight == true && _zippedUpRight == false)
                {
                    ZipUpOnTheRight();
                    _areBasicLasersActive = false;
                }
                StartCoroutine(TriggerAsteroidAttack(3.2f));
                break;
            case BossState.StopAllCoroutines:
                StopAllCoroutines();
                currentState = BossState.AsteroidAttack;
                break;
            case BossState.AsteroidAttack:
                if (_healthPercentage > 0)
                {
                    _dialupAnim.gameObject.SetActive(true);
                    _isEnumZipMovementsActive = false;
                    _isEnumAsteroidAttackActive = true;
                    _isRainDownAsteroidsActive = true;
                    StartCoroutine(SegueIntoZoomDownBehavior(4));
                }
                break;
            case BossState.stopallcoroutinesb4zoomdown:
                StopAllCoroutines();
                currentState = BossState.ZoomDown;
                break;
            case BossState.ZoomDown:
                _dialupAnim.gameObject.SetActive(false);
                _isEnumAsteroidAttackActive = false;
                ZoomDirectlyDown();
                if (transform.position == _startPosition && _healthPercentage > 0)
                {
                    currentState = BossState.SpecialAttack;
                }
                break;
            case BossState.SpecialAttack:
                _isRainDownAsteroidsActive = false;
                if (_healthPercentage > 0)
                {
                    StartCoroutine(SpecialAttack(2));
                }
                break;
            case BossState.Vulnerable:                
                if (_isVulnerable == true)
                {
                    _areSpecialLasersActive = false;
                }
                break;
            default:
                break;
        }
        if (_asteroidPrefab.gameObject.transform.position.y < -9)
        {
            Destroy(_asteroidPrefab.gameObject);
        }
        if (_basicLasersPrefab.gameObject.transform.position.y < -8)
        {
            Destroy(_basicLasersPrefab.gameObject);
        }
        if (_bigBasicLaser.gameObject.transform.position.y < -8)
        {
            Destroy(_bigBasicLaser.gameObject);
        }

    }
    void FireBasicLasers()
    {
        if (Time.time > _canFire && _speed > 0 && _healthPercentage > 0)
        {
            _fireRate = 1.4f;
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_basicLasersPrefab, transform.position, Quaternion.identity);
            Laser[] Lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < Lasers.Length; i++)
            {
                Lasers[i].AssignEnemyLaser();
            }
            _normalLaserSound.Play();
        }
    }
    void FireBigLaser()
    {
        if (Time.time > _bigCanFire && _speed > 0 && _healthPercentage > 0)
        {
            _bigFireRate = 3f;
            _bigCanFire = Time.time + _bigFireRate;
            GameObject enemyLaser = Instantiate(_bigBasicLaser, transform.position, Quaternion.identity);
            Laser[] Lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < Lasers.Length; i++)
            {
                Lasers[i].AssignEnemyLaser();
            }
            _bigLaserFireSound.Play();
        }
    }
    void SpecialLasers()
    {
        if (Time.time > _specialCanFire && transform.position.y == 0 && _isRainDownAsteroidsActive == false && _healthPercentage > 0)
        {
            _specialFireRate = 0.2f;
            _specialCanFire = Time.time + _specialFireRate;
            GameObject enemyLaser = Instantiate(_specialLazers, transform.position, Quaternion.identity);
            Laser[] Lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < Lasers.Length; i++)
            {
                Lasers[i].AssignEnemyLaser();
            }
            _specialLasersSound.Play();
        }
        if(_specialLazers.gameObject.transform.position.x < -11 || _specialLazers.gameObject.transform.position.x > 11)
        {
            Destroy(_specialLazers.gameObject);
        }
    }
    void RainDownAsteroids()
    {
        if (Time.time > _asteroidCanFire && transform.position.y >= 6 && _healthPercentage > 0)
        {
            _asteroidFireRate = 0.8f;
            _asteroidCanFire = Time.time + _asteroidFireRate;
            GameObject bossAsteroid = Instantiate(_asteroidPrefab, transform.position, Quaternion.identity);
            Asteroid[] Asteroids = bossAsteroid.GetComponentsInChildren<Asteroid>();

            for (int i = 0; i < Asteroids.Length; i++)
            {
                Asteroids[i].AssignBossAsteroid();
            }
        }      
    }
    void GrenadeLaunch()
    {
        if (Time.time > _canFire && _speed > 0 && _healthPercentage > 0)
        {
            _fireRate = 0.8f;
            _canFire = Time.time + _fireRate;
            GameObject bossGrenade = Instantiate(_grenadePrefab, transform.position, Quaternion.identity);
            Grenade[] Grenades = bossGrenade.GetComponentsInChildren<Grenade>();

            for (int i = 0; i < Grenades.Length; i++)
            {
                Grenades[i].AssignBossGrenade();
            }
            _grenadeLaunchSound.Play();
        }
    }
    private IEnumerator WaitThenMoveShoot(float timetowait)
    {
        if (_isEnumWaitingActive == true)
        {
            yield return new WaitForSeconds(timetowait);
            currentState = BossState.MovingAndShooting;
        }
    }
    private IEnumerator SegueToMovingOnUp(float timetowait)
    {
        if (_isEnumMoveAndShootActive == true)
        {
            _movedUpRight = false;
            yield return new WaitForSeconds(timetowait);
            currentState = BossState.MovingUp;
        }
    }
    private IEnumerator SegueToLaunchingGrenades(float timetowait)
    {
        if(_isEnumMovingUpActive)
        {
            _movedTopRight = false;
            yield return new WaitForSeconds(8.2f);
            currentState = BossState.ZippingAndLaunchingGrenades;
        }
    }
    private IEnumerator SegueIntoZipDown(float timetowait)
    {
        if (_isEnumZipAndLaunchGrenadeActive == true)
        {
            yield return new WaitForSeconds(timetowait);
            currentState = BossState.stopallcoroutinesb4destroydot;
        }
    }
    private IEnumerator TriggerAsteroidAttack(float timetowait)
    {
        if(_isEnumZipMovementsActive == true)
        {
            yield return new WaitForSeconds(timetowait);
            currentState = BossState.StopAllCoroutines;
        }
    }
    private IEnumerator SegueIntoZoomDownBehavior(float timetowait)
    {
        if (_isEnumAsteroidAttackActive == true && _healthPercentage > 0)
        {
            _dialupSound.Play();
            yield return new WaitForSeconds(timetowait);
            currentState = BossState.stopallcoroutinesb4zoomdown;
        }
    }
    private IEnumerator SpecialAttack(float timetowait)
    {
        _areSpecialLasersActive = true;
        _malfunctionSound.Play();
        yield return new WaitForSeconds(timetowait);
        StartCoroutine(VulnerableCountDown());
    }
    private IEnumerator VulnerableCountDown()
    { 
        currentState = BossState.Vulnerable;        
        _isVulnerable = true;
        StartCoroutine(SystemsAreDown());
        yield return new WaitForSeconds(5);
        _isVulnerable = false;
        _malfunctionSound.Stop();
        currentState = BossState.Waiting;
       
    }
    private IEnumerator SystemsAreDown()
    {
        while(_isVulnerable == true)
        {
            yield return new WaitForSeconds(0.5f);
            _spriteRenderer.color = new Color(1, 0.240556f, 0.240556f);
            yield return new WaitForSeconds(0.5f);
            _spriteRenderer.color = new Color(1, 1, 1);
        }
    }
    private IEnumerator DeathColors()
    {
        while(_isDeathAnimationActive == true)
        {
            _spriteRenderer.color = new Color(1, 0.240556f, 0.240556f);
            yield return new WaitForSeconds(.02f);
            _spriteRenderer.color = new Color(1, 1, 1);
            yield return new WaitForSeconds(.02f);
        }
    }
    private void MoveRight()
    {
        if (transform.position != _rightPoint && _healthPercentage >= 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, _rightPoint, _speed * Time.deltaTime);
        }
        else
        {
            _movedRight = true;
        }
    }
    private void MoveLeft()
    {
        if (transform.position != _leftPoint && _healthPercentage >= 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, _leftPoint, _speed * Time.deltaTime);
        }
        else
        {
            _movedRight = false;
        }
    }
    private void MoveUpRight()
    {
        if (transform.position != _upRightPoint && _healthPercentage >= 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(4.3f, 2f, 0), _speed * Time.deltaTime);
        }
        else
        {
            _movedUpRight = true;
        }
    }
    private void MoveUpLeft()
    {
        if (transform.position != _upLeftPoint && _healthPercentage >= 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 5, 0), (_speed = 1f) * Time.deltaTime);
        }
        else
        {
            _movedUpRight = false;
        }
    }
    private void MoveTopRight()
    {
        if(transform.position != _topRightPoint && _healthPercentage >= 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, _topRightPoint, (_speed = 8f) * Time.deltaTime);
        }
        else
        {
            _movedTopRight = true;
        }
    }
    private void MoveTopLeft()
    {
        if (transform.position != _topLeftPoint && _healthPercentage >= 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, _topLeftPoint, (_speed = 8f) * Time.deltaTime);
        }
        else
        {
            _movedTopRight = false;
        }
    }
    private void ZipDownLeft()
    {
        if(transform.position != _zipDownLeftPoint && _healthPercentage >= 1)      
        {
            transform.position = Vector3.MoveTowards(transform.position, _zipDownLeftPoint, (_speed = 15f) * Time.deltaTime);
        }
        else
        {
            _zippedDownLeft = true;
        }
    }
    private void ZipUpOnTheLeft()
    {
        if(transform.position != _zipUpLeftPoint && _healthPercentage >= 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, _zipUpLeftPoint, (_speed = 15f) * Time.deltaTime);
        }
        else
        {
            _zippedUpLeft = true;
        }
       
    }
    private void ZipDownRight()
    {
        if (transform.position != _zipDownRightPoint && _healthPercentage >= 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, _zipDownRightPoint, (_speed = 15f) * Time.deltaTime);
        }
        else
        {
            _zippedDownRight = true;
        }
    }
    private void ZipUpOnTheRight()
    {
        if(transform.position != _zipUpRightPoint && _healthPercentage >= 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, _zipUpRightPoint, (_speed = 15f) * Time.deltaTime);
        }
        else
        {
            _zippedUpRight = false;
        }
    }
    private void ZoomDirectlyDown()
    {
        if (transform.position != _startPosition && _healthPercentage >= 1)
        { 
            transform.position = Vector3.MoveTowards(transform.position, _startPosition, (_speed = 15f) * Time.deltaTime);
        }
           
    }
    IEnumerator ViolentJerks()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, 0), (_speed = 22) * Time.deltaTime);
        if (transform.position.y == 0 && transform.position.x == 0)
        {
            StartCoroutine(ViolentJerks2());
        }    
        yield return new WaitForSeconds(.01f);
    }
    IEnumerator ViolentJerks2()
    {
        while(_isDeathAnimationActive == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _deathJoltLeft1, (_speed = 10) * Time.deltaTime);
            yield return new WaitForSeconds(.4f);
            transform.position = Vector3.MoveTowards(transform.position, _deathJoltRight1, (_speed = 10) * Time.deltaTime);
            yield return new WaitForSeconds(.4f);
            transform.position = Vector3.MoveTowards(transform.position, _deathJoltLeft2, (_speed = 10) * Time.deltaTime);
            yield return new WaitForSeconds(.4f);
            transform.position = Vector3.MoveTowards(transform.position, _deathJoltRight2, (_speed = 10) * Time.deltaTime);
            yield return new WaitForSeconds(.4f);
        }

    }
    private void Shields()
    {
        if (_isShieldsActive == true && _healthPercentage > 0)
        {
            _shieldVisualiser.gameObject.SetActive(true);
        }
        else if (_isShieldsActive == false)
        {
            _shieldVisualiser.gameObject.SetActive(false);

        }
        if(currentState == BossState.MovingAndShooting)
        {
            _isShieldsActive = true;
            if(_shieldStrength < 1)
            {
                _isShieldsActive = false;
            }
        }
        else if(currentState == BossState.AsteroidAttack)
        {
            _isShieldsActive = false;
            _shieldStrength = 0;
        }
        else if (currentState == BossState.Waiting)
        {
            _shieldStrength = 6;
        }
           
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
        while (_isDamagedActive == true)
        {
            yield return new WaitForSeconds(.13f);
            _spriteRenderer.color = new Color(1, 0.240556f, 0.240556f);
            yield return new WaitForSeconds(.13f);
            _spriteRenderer.color = new Color(3, 1, 2);
        }
    }
    public void Damage()
    {
        if (_isShieldsActive == true)
        {
            _shieldStrength--;
            _shieldHitSound.Play();
        }
        if (_shieldStrength < 1 && _healthPercentage > 0 || _isShieldsActive == false && _healthPercentage > 0 && currentState == BossState.Start || _isShieldsActive == false && _healthPercentage > 0 && currentState == BossState.Waiting)
        {
            _isShieldsActive = false;
            _shieldStrength = _shieldStrength * 0;
            _laserCrashSound.Play();
            StartCoroutine(WhileDamaged());
        }
        if (_wasHitByMissile == false)
        {
            if (_healthPercentage > 0 && _healthPercentage <= 100 && _isShieldsActive != true)
            {
                _healthPercentage -= Random.Range(1, 3);
                _uiManager.UpdateBossHealth(_healthPercentage);
            }
        }
        else if (_healthPercentage > 0 && _healthPercentage <= 100 && _wasHitByMissile == true && _isShieldsActive != true)
        {
            _healthPercentage -= Random.Range(5, 9);
            _uiManager.UpdateBossHealth(_healthPercentage);
        }
        else if(_isShieldsActive == true && _wasHitByMissile == true)
        {
            _shieldStrength -= 2;
            if(_shieldStrength == 1)
            {
                _shieldStrength -= 1;
                _healthPercentage -= Random.Range(1, 3);
                _uiManager.UpdateBossHealth(_healthPercentage);
            }
        }
        if (_healthPercentage < 1)
        {
            StopAllCoroutines();
            StartCoroutine(YieldForDeathAnimation());
        }
    }
    void Dead()
    {
        if (_player != null)
        {
            StopAllCoroutines();
            _malfunctionSound.Stop();
            _anim.SetTrigger("OnEnemyDeath");
            _gameManager.GameWon();
            _uiManager.VictorySequence();
            _spawnManager.StopSpawningPowerups();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.3f);
        }
    }
    IEnumerator YieldForDeathAnimation()
    {
        if (_player != null)
        {
            _dialupAnim.gameObject.SetActive(false);
            _smallExplosion.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            _isDeathAnimationActive = true;
            StartCoroutine(DeathColors());
            StartCoroutine(DeathAnimation());
            yield return new WaitForSeconds(5);
            _player.AddScore(700);
            _isDeathAnimationActive = false;
            Dead();
        }
    }
    IEnumerator DeathAnimation()
    {
        while (_isDeathAnimationActive == true)
        {
            if (_isDeathAnimationActive == true)
            {
                Instantiate(_smallExplosion, transform.position + _explosionPoint1, Quaternion.identity);
                _smallExplosionSound.Play();             
                yield return new WaitForSeconds(.2f);
                Instantiate(_smallExplosion, transform.position + _explosionPoint2, Quaternion.identity);
                _smallExplosionSound.Play();
                yield return new WaitForSeconds(.1f);
                Instantiate(_smallExplosion, transform.position + _explosionPoint3, Quaternion.identity);
                _smallExplosionSound.Play();
                yield return new WaitForSeconds(.1f);
                Instantiate(_smallExplosion, transform.position + _explosionPoint4, Quaternion.identity);
                _smallExplosionSound.Play();
                yield return new WaitForSeconds(.08f);
                Instantiate(_smallExplosion, transform.position + _explosionPoint5, Quaternion.identity);
                _smallExplosionSound.Play();
                yield return new WaitForSeconds(.18f);
                Instantiate(_smallExplosion, transform.position + _explosionPoint6, Quaternion.identity);
                _smallExplosionSound.Play();
                yield return new WaitForSeconds(.08f);
                Instantiate(_smallExplosion, transform.position + _explosionPoint7, Quaternion.identity);
                _smallExplosionSound.Play();
                yield return new WaitForSeconds(.02f);
                Instantiate(_smallExplosion, transform.position + _explosionPoint8, Quaternion.identity);
                _smallExplosionSound.Play();
                yield return new WaitForSeconds(.1f);
            }
        }
    }
    public void MissileHit()
    {
        _wasHitByMissile = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _wasHitByMissile = false;
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }
        if(other.tag == "Laser")
        {
            _wasHitByMissile = false;
        }
        if(other.tag == "Rod")
        {
            _wasHitByMissile = false;
            Damage();
        }
    }
}

