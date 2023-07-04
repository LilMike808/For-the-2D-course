using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    private Player _player;
    private HomingMissile _homingMissile;
    private Animator _anim;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioSource _launchOrbSound;
    private SpawnManager _spawnManager;
    private float _speed = 2.5f;
    private float _range = 4.4f;
    private int _enemyMovement;
    private bool _isFireOrbActive = false;
    private bool _isStopHammertimeActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isSlowSpeedActive = false;
    private bool _isShieldsActive = false;
    private bool _wasHitByMissile = false;
    private bool _isTookAHitActive = false;
    private bool _isRamActive = true;
    [SerializeField]
    private float _lives = 4;
    private float _shieldStrength;
    SpriteRenderer _shieldColor;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _bangarangAnimation;
    private SpriteRenderer _spriteRenderer;
    private float _canFire = 2.8f;
    private float _fireRate = 6;
    [SerializeField]
    private GameObject _phantasmicOrb;
    [SerializeField]
    private GameObject _plasmaBlast;
    [SerializeField]
    private AudioSource _boomSound;
    [SerializeField]
    private AudioSource _whooshSound;
    [SerializeField]
    private AudioSource _laserHitSound;
    [SerializeField]
    private AudioSource _shieldHitSound;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _shieldColor = _shieldVisualizer.GetComponent<SpriteRenderer>();
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        _enemyMovement = Random.Range(1, 3);
        _player = GameObject.FindObjectOfType<Player>();
        _homingMissile = GameObject.FindObjectOfType<HomingMissile>();
        _anim = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        RamPlayer();
        if(_isFireOrbActive == true)
        {
            FireOrb();
        }
        if(_isStopHammertimeActive == true && _lives >= 1)
        {
            StartCoroutine(Stop_HammerTime());
        }
    }
    public void IsEnemyLaser()
    {
        _laserHitSound.Pause();
    }
    public void IsNotEnemyLaser()
    {
        if(_isShieldsActive == true)
        {
            _laserHitSound.Pause();
            _shieldHitSound.Play();
            return;
        }
        else if(_isShieldsActive == false && _lives > 0)
        {
            _laserHitSound.Play();
        }
    }
    void FireOrb()
    {
        if (Time.time > _canFire && _isStopHammertimeActive == true && _lives >= 1 && _player != null)
        {
            _canFire = Time.time + _fireRate;
            GameObject enemyorb = Instantiate(_phantasmicOrb, transform.position, Quaternion.identity);
            _launchOrbSound.Play();
        }
    }
    void RamPlayer()
    {
        if (_player != null)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < _range && _lives >= 1 && _isRamActive == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * 7f * Time.deltaTime);
                if (transform.position == _player.transform.position)
                {
                    _isRamActive = false;
                }
                if (transform.position == _player.transform.position)
                {
                    StartCoroutine(Teleport());
                }

                _whooshSound.Play();
            }
        }
    }
    void CalculateMovement()
    {

        if (_enemyMovement == 1 && _lives >= 1)
        {
            transform.Translate((Vector3.down + Vector3.left) * _speed * Time.deltaTime);
        }
        if (_enemyMovement == 2 && _lives >= 1)
        {
            transform.Translate((Vector3.down + Vector3.right) * _speed * Time.deltaTime);
        }        
        if (transform.position.x > 11.75f)
        {
            transform.position = new Vector3(Random.Range(0, -11.75f), 7, 0);
        }
        if (transform.position.x < -11.75f)
        {
            transform.position = new Vector3(Random.Range(0, 11.75f), 7, 0);
        }
        if (transform.position.y <= 0 && transform.position.x < 7 && transform.position.x > -7)
        {
            _isStopHammertimeActive = true;
        }
    }
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
        _isSpeedBoostActive = false;
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        while (_isSpeedBoostActive == true)
        {
            _speed = _speed * 3;
            yield return new WaitForSeconds(5.0f);
        }
        if(_isSpeedBoostActive == false)
        {
            _speed = 2.5f;
        }       
    }
    public void SlowSpeedActive()
    {
        _isSlowSpeedActive = true;
        StartCoroutine(SlowSpeedPowerDown());
    }
    IEnumerator SlowSpeedPowerDown()
    {
        StartCoroutine(WhileFrozen());
        yield return new WaitForSeconds(5.0f);
        _isSlowSpeedActive = false;
    }
    IEnumerator WhileFrozen()
    {
        while (_isSlowSpeedActive == true)
        {
            _speed = _speed / 1.2f;
            _spriteRenderer.color = new Color(0, 5, 255, 255);
            yield return new WaitForSeconds(0.1f);
        }
        if (_isSlowSpeedActive == false)
        {
            _speed = 2.5f;
            _spriteRenderer.color = new Color(255, 255, 255, 255);
        }
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

    void NormalizeSpeed()
    {
        _phantasmicOrb.gameObject.SetActive(false);
        _speed = 2.5f;
        _isRamActive = true;
        _isFireOrbActive = false;
        _isStopHammertimeActive = false;
    }
    IEnumerator Stop_HammerTime()
    {
        _phantasmicOrb.gameObject.SetActive(true);
        _speed = 0;
        _isRamActive = false;
        _isFireOrbActive = true;
        yield return new WaitForSeconds(3.5f);
        NormalizeSpeed();
        
    }
    private IEnumerator GotDamaged()
    {
        _isTookAHitActive = true;
        StartCoroutine(TookAHit());
        yield return new WaitForSeconds(0.8f);
        _isTookAHitActive = false;
    }
    private IEnumerator TookAHit()
    {
        while (_isTookAHitActive == true)
        {
            yield return new WaitForSeconds(0.13f);
            _spriteRenderer.color = new Color(1, 0.240556f, 0.240556f);
            yield return new WaitForSeconds(0.13f);
            _spriteRenderer.color = new Color(1, 1, 1);
        }
    }
    private IEnumerator PlasmicDeathBlast()
    {
        _plasmaBlast.gameObject.SetActive(true);
        _boomSound.Play();
        yield return new WaitForSeconds(0.4f);
        _plasmaBlast.gameObject.SetActive(false);
        _anim.SetTrigger("OnEnemyDeath");
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
    public void Damage()
    {
        if (_wasHitByMissile == false)
        {
            if (_isShieldsActive == false)
            {
                _lives--;
                StartCoroutine(GotDamaged());
            }
            else if (_isShieldsActive == true)
            {
                _shieldStrength--;
                _shieldHitSound.Play();
            }
        }
        else if (_wasHitByMissile == true)
        {
            if (_isShieldsActive == false)
            {
                _lives -= 2;
                StartCoroutine(GotDamaged());
            }
            else if (_isShieldsActive == true)
            {
                if (_shieldStrength != 1)
                {
                    _shieldStrength -= 2;
                    _shieldHitSound.Play();
                }
                else if(_shieldStrength == 1)
                {
                    StartCoroutine(GotDamaged());
                    _shieldStrength -= 1;
                    _lives -= 1;
                }
            }            
        }

        if (_isShieldsActive == true)
        {
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
            else if (_shieldStrength == 3)
            {
                _shieldColor.color = Color.cyan;
                return;
            }
        }
        if (_lives < 1)
        {
            _speed = 0;
            _isTookAHitActive = false;           
            StartCoroutine(PlasmicDeathBlast());
            _player.AddScore(Random.Range(8, 10));
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            _spawnManager.EnemyDeath();
            Destroy(this.gameObject, 3.2f);
        }
    }
    IEnumerator Teleport()
    {
        _spriteRenderer.color = new Color(0, 0, 0, 255);
        _bangarangAnimation.gameObject.SetActive(true);
        yield return new WaitForSeconds(.2f);
        _bangarangAnimation.gameObject.SetActive(false);
        transform.position = new Vector3(Random.Range(-9, 9), Random.Range(3, 7), 0);
        _bangarangAnimation.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        _bangarangAnimation.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        _spriteRenderer.color = new Color(255, 255, 255, 255);
        _isRamActive = true;
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
        if (other.tag == "Thruster")
        {
            _wasHitByMissile = false;
            if (_lives > 0)
            {
                _laserHitSound.Play();
            }
            Damage();
        }
        if(other.tag == "Laser")
        {
            _wasHitByMissile = false;
        }
        if(other.tag == "Rod")
        {
            _wasHitByMissile = false;
            IsNotEnemyLaser();
            Damage();
        }
    }
}
