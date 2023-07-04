using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private int powerupID;
    private float _speed = 3f;
    private float _rotateSpeed = 1000.0f;
    [SerializeField]
    private AudioClip _clip;
    [SerializeField]
    private bool _isMultiLaserPowerup;
    [SerializeField]
    private bool _isNegativePowerup;
    [SerializeField]
    private bool _isTripleShotPowerup;
    [SerializeField]
    private bool _isHealthPowerup;
    [SerializeField]
    private bool _isAmmoPowerup;
    [SerializeField]
    private bool _isMissilePowerup;
    private float _range = 5.5f;
    private Player _player;
    [SerializeField]
    private GameObject _upInSmoke;
    private SpriteRenderer _spriteRenderer;
    private bool _continuousFlashing = true;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindObjectOfType<Player>();
        if(_player == null)
        {
            Debug.Log("The Player is Null!");
        }
        if(_isMissilePowerup == true || _isMultiLaserPowerup == true)
        {
            StartCoroutine(FlashingRedRoutine());
        }
    }
    // Update is called once per frame
    void Update()
    {
        //move down at the speed of 3
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //destroy object once we leave screen
        if (transform.position.y < -7f)
        {
            Destroy(this.gameObject);
        }

        if(_isMultiLaserPowerup == true)
        {
            _speed = 1.5f;
            transform.Rotate(Vector3.back * _rotateSpeed * Time.deltaTime);
        }
        if(_isNegativePowerup == true)
        {
            _speed = 10;
        }
        
        if(Input.GetKey(KeyCode.C) && Vector3.Distance(_player.transform.position, transform.position) < _range)
        {
            if (_player.transform != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * 2 * Time.deltaTime);
            }
        }
    }

    public void UpInSmoke()
    {
        if (_isNegativePowerup == false && this.gameObject != null)
        {
            Instantiate(_upInSmoke, transform.position, Quaternion.identity);

        }
    }
    IEnumerator FlashingRedRoutine()
    {
        while (_continuousFlashing == true)
        {
            yield return new WaitForSeconds(.13f);
            _spriteRenderer.color = new Color(1, 0.240556f, 0.240556f);
            yield return new WaitForSeconds(.13f);
            _spriteRenderer.color = new Color(3, 1, 2);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    case 3:
                        player.AmmoCollected();
                        break;
                    case 4:
                        player.HealthCollected();
                        break;
                    case 5:
                        player.MultiDirectionalLasersActive();
                        break;
                    case 6:
                        player.SlowSpeedActive();
                        break;
                    case 7:
                        player.HomingMissileActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
        if (other.tag == "Enemy3" && _isAmmoPowerup == false && _isMultiLaserPowerup == false && _isHealthPowerup == false && _isTripleShotPowerup == false && _isMissilePowerup == false)
        {
            Enemy3 enemy3 = other.transform.GetComponent<Enemy3>();
            if (enemy3 != null)
            {
                switch (powerupID)
                {
                    case 1:
                        enemy3.SpeedBoostActive();
                        break;
                    case 2:
                        enemy3.ShieldsActive();
                        break;
                    case 6:
                        enemy3.SlowSpeedActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            Destroy(this.gameObject);
        }      
    }  
}
