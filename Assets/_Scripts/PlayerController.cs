using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HandleBallAudio))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private GameObject _focalPoint;
    private bool _powerUp, _multiplier;
    private bool _canJump = true;
    private float _ogForce, _ogDuration;
    private bool _dead = false;
    private float _fade = 1;
    private Image _fadeImage;
    private HandleBallAudio _audioManager;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private GameObject _powerUpIndicator, _multIndicator;
    [SerializeField]
    private ParticleSystem _particles;
    [SerializeField]
    private AudioClip _explosionSound, _boltPickUpSound;
    [SerializeField]
    private GameObject _fadePanel;
    public float moveForce = 5.0f;
    public float powerUpForce;
    public float powerUpDuration;


    // Start is called before the first frame update
    void Start(){
        _rigidbody = GetComponent<Rigidbody>();
        _audioManager = GetComponent<HandleBallAudio>();
        _focalPoint = GameObject.Find("Center");
        Physics.gravity = new Vector3(0,-18,0);
        _ogForce = powerUpForce;
        _ogDuration = powerUpDuration;
        _fadeImage = _fadePanel.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update(){
        if (!_dead){
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0.1f, Input.GetAxis("Vertical"));
            _rigidbody.AddForce(Quaternion.LookRotation(_focalPoint.transform.forward, Vector3.up) * (moveForce * input * Time.deltaTime * 100));
            if (Input.GetAxis("Jump") > 0 && _canJump){
                _rigidbody.AddForce(_focalPoint.transform.up * 20 + input, ForceMode.Impulse);
                _canJump = false;
            }
            if (Input.GetAxis("Cancel")>0) {
                _gameManager.BackToMenu();
            }

            if (_fade > 0){
                _fade -= Time.deltaTime*3;
                _fadeImage.color = new Color(0,0,0,_fade);
                if (_fade < .05f) _fadePanel.gameObject.SetActive(false);
            }
        }else{
            _fade += Time.deltaTime*2;
            _fadeImage.color = new Color(0, 0, 0, _fade);
            if (_fade>1) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //Handle power up pick ups
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("PowerUp") && !_powerUp) {
            _powerUp = true;
            _powerUpIndicator.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountDown());
            _audioManager.PlayOneShotSound(_boltPickUpSound, (.6f + (powerUpForce / _ogForce) * .5f));
            _audioManager.SetImpactVolume(1.5f+ (powerUpForce / _ogForce) * .5f);
        } else if (other.CompareTag("NukePowerUp")) {
            Destroy(other.gameObject);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies) {
                Vector3 away = (enemy.transform.position - transform.position).normalized;
                enemy.GetComponent<Rigidbody>().AddForce(away * powerUpForce * 2, ForceMode.Impulse);
            }
            _particles.Play();
            _audioManager.PlayOneShotSound(_explosionSound, (.5f+(powerUpForce/_ogForce)*.6f) );
        } else if (other.CompareTag("MultiplierPowerUp")) {
            _multIndicator.SetActive(true);
            powerUpForce *= 3;
            powerUpDuration *= 1.5f;
            if (!_multiplier){ StartCoroutine(PowerUpMultiplier()); }
            _multiplier = true;
            Destroy(other.gameObject);
        }
    }

    //Check loose
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Finish")){
            StartCoroutine(GameEnd());
        }
    }

    //Finish game
    private IEnumerator GameEnd() {
        _gameManager.SendScore();
        yield return new WaitForSeconds(1.5f);
        _gameManager.OnDeath();
        yield return new WaitForSeconds(1.8f);
        _dead = true;
        _fadePanel.gameObject.SetActive(true);
    }

    //Power up push and jump reset
    private void OnCollisionEnter(Collision other) {
        if (_powerUp && other.gameObject.CompareTag("Enemy")){
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 repulsionDir = other.gameObject.transform.position - transform.position;
            enemyRigidbody.AddForce(repulsionDir * powerUpForce, ForceMode.Impulse);
        }else if (other.gameObject.CompareTag("Terrain")) {
            _canJump = true;
        }
    }

    IEnumerator PowerUpCountDown() {
        yield return new WaitForSeconds(powerUpDuration);
        _powerUpIndicator.SetActive(false);
        _powerUp = false;
        _audioManager.SetImpactVolume(1);
    }

    IEnumerator PowerUpMultiplier(){
        yield return new WaitForSeconds(powerUpDuration);
        powerUpForce = _ogForce;
        powerUpDuration = _ogDuration;
        _multIndicator.SetActive(false);
        _multiplier = false;
        _audioManager.SetImpactVolume(_powerUp ? 2 : 1);
    }

}
