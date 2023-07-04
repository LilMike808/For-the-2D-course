using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private float _speed = 3f;
    private Animator _anim;
    [SerializeField]
    private AudioSource _audioSource;
    private bool _isBossGrenade = false;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       if(_isBossGrenade == false)
        {
            MoveUp();
        }
       else
        {
            MoveDown();
        }
    }
    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8)
        {
            Destroy(this.gameObject);
        }
    }
    void MoveDown()
    {
        transform.Translate(Vector3.down * (_speed * 1.2f) * Time.deltaTime);

        if (transform.position.y < -8)
        {
            Destroy(this.gameObject);
        }
    }
    public void AssignBossGrenade()
    {
        _isBossGrenade = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.3f);
        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            _anim.SetTrigger("OnEnemyDeath");
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.3f);
        }
        if(other.tag == "Thruster")
        {
            _anim.SetTrigger("OnEnemyDeath");
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.3f);
        }
    }
}
