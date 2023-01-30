using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private int powerupID;
    private float _speed = 3f;
    [SerializeField]
    private AudioClip _clip;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        //move down at the speed of 3
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //destroy object once we leave screen
        if (transform.position.y < -5f)
        {
            Destroy(this.gameObject);
        }
    }

    //You got it correct save for your Collider 2D is of type "other" which allows you to store tag "Player". 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
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
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            
            Destroy(this.gameObject);
        }
    }
    
}
