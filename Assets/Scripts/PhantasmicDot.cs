using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantasmicDot : MonoBehaviour
{
    private float _speed = 9;
    private Player _player;
    private SpriteRenderer _spriteRenderer;
    private bool _isGotThePlayerActive = false;
    private bool _isGotThePlayer2Active = false;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
        }
        if(transform.position.x > 12)
        {
            Destroy(this.gameObject);
        }
        if(transform.position.x < -12f)
        {
            Destroy(this.gameObject);
        }
        if(transform.position.y > 9)
        {
            Destroy(this.gameObject);
        }
        if(transform.position.y < -9)
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator WhileGotThePlayerActive()
    {
        _isGotThePlayerActive = true;
        StartCoroutine(GotThePlayer());
        yield return new WaitForSeconds(1.3f);
        _isGotThePlayerActive = false;
        _isGotThePlayer2Active = true;
        StartCoroutine(GotThePlayer2());
        yield return new WaitForSeconds(0.7f);
        _isGotThePlayer2Active = false;
    }
    IEnumerator GotThePlayer()
    {
        while (_isGotThePlayerActive == true)
        {
            yield return new WaitForSeconds(0.13f);
            _spriteRenderer.color = new Color(0, 0, 0, 255);
            yield return new WaitForSeconds(0.13f);
            _spriteRenderer.color = new Color(1, 1, 1);
        }
    }
    IEnumerator GotThePlayer2()
    {
        while(_isGotThePlayer2Active == true)
        {
            yield return new WaitForSeconds(0.065f);
            _spriteRenderer.color = new Color(0, 0, 0, 255);
            yield return new WaitForSeconds(0.065f);
            _spriteRenderer.color = new Color(1, 1, 1);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.transform.GetComponent<Player>();
        if (player != null || _player != null)
        {
            StartCoroutine(WhileGotThePlayerActive());
            Destroy(this.gameObject, 2f);
        }
        if(other.tag == "Pulse")
        {
             _player.IfOrbIsDestroyed();
            Destroy(this.gameObject);          
        }
    }
}
