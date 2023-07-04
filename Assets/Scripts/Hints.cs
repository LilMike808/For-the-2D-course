using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hints : MonoBehaviour
{
    [SerializeField]
    private Text _xRodHint;
    [SerializeField]
    private Text _mPulseHint;
    [SerializeField]
    private Text _powerupMagnetHint;
    [SerializeField]
    private Text _leftShiftThrusterHint;
    [SerializeField]
    private Text _enemy5Hint;
    [SerializeField]
    private Text _closingHint;
    private bool _isXRodHintDestroyed = false;
    private bool _isMPulseHintDestroyed = false;
    private bool _isPowerupMagnetHintDestroyed = false;
    private bool _isLeftShiftHintDestroyed = false;
    private bool _isEnemy5hintDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isXRodHintDestroyed == false && _isMPulseHintDestroyed == false &&
            _isPowerupMagnetHintDestroyed == false && _isLeftShiftHintDestroyed == false && _isEnemy5hintDestroyed == false)
        {
            _xRodHint.gameObject.SetActive(false);
            _mPulseHint.gameObject.SetActive(true);
            _isXRodHintDestroyed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _isXRodHintDestroyed == true && _isMPulseHintDestroyed == false &&
            _isPowerupMagnetHintDestroyed == false && _isLeftShiftHintDestroyed == false && _isEnemy5hintDestroyed == false)
        {
             _mPulseHint.gameObject.SetActive(false);
            _powerupMagnetHint.gameObject.SetActive(true);
            _isMPulseHintDestroyed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _isXRodHintDestroyed == true && _isMPulseHintDestroyed == true &&
            _isPowerupMagnetHintDestroyed == false && _isLeftShiftHintDestroyed == false && _isEnemy5hintDestroyed == false)
        {
            _powerupMagnetHint.gameObject.SetActive(false);
            _leftShiftThrusterHint.gameObject.SetActive(true);
            _isPowerupMagnetHintDestroyed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _isXRodHintDestroyed == true && _isMPulseHintDestroyed == true &&
            _isPowerupMagnetHintDestroyed == true && _isLeftShiftHintDestroyed == false && _isEnemy5hintDestroyed == false)
        {
            _enemy5Hint.gameObject.SetActive(true);
            _leftShiftThrusterHint.gameObject.SetActive(false);
            _isLeftShiftHintDestroyed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _isXRodHintDestroyed == true && _isMPulseHintDestroyed == true &&
            _isPowerupMagnetHintDestroyed == true && _isLeftShiftHintDestroyed == true && _isEnemy5hintDestroyed == false)
        {
            _closingHint.gameObject.SetActive(true);
            _enemy5Hint.gameObject.SetActive(false);
            _isXRodHintDestroyed = false;
            _isMPulseHintDestroyed = false;
            _isPowerupMagnetHintDestroyed = false;
            _isLeftShiftHintDestroyed = false;
            _isEnemy5hintDestroyed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _isXRodHintDestroyed == false && _isMPulseHintDestroyed == false &&
            _isPowerupMagnetHintDestroyed == false && _isLeftShiftHintDestroyed == false && _isEnemy5hintDestroyed == true)
        {
            _closingHint.gameObject.SetActive(false);
            _xRodHint.gameObject.SetActive(true);
            _isEnemy5hintDestroyed = false;
        }
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
