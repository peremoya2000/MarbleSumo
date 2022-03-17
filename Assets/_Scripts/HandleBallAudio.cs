using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class HandleBallAudio : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
    private SharedAudio _sharedAudio;
    private float _impactVolume = 1;
    [SerializeField]
    private float _maxSpeed=1;
    [SerializeField]
    private float _volumeMult=1;
    [SerializeField]
    private float _pitchOffset = 0;
    [SerializeField][Range(0,1)]
    private float _pitchShiftAmmount=0.5f;
    [SerializeField]
    private Vector2 _pitchBounds = new Vector2(-2,2);
    [SerializeField]
    private AudioClip[] _impactSounds;

    public void SetImpactVolume(float v) {
        _impactVolume = v;
    }
    public float GetImpactoVolume() {
        return _impactVolume;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _sharedAudio = GameObject.Find("SharedAudioSource").GetComponent<SharedAudio>();
    }

    // Update is called once per frame
    void FixedUpdate(){
        float speed = (GetSpeed() / _maxSpeed);
        _audioSource.volume = speed * speed * _volumeMult;
        _audioSource.pitch = Mathf.Clamp(
            1 + _pitchShiftAmmount * (speed - 0.45f) + _pitchOffset,
            _pitchBounds.x,
            _pitchBounds.y+1);
    }

    private void OnCollisionEnter(Collision other){           
        if (other.gameObject.CompareTag("Terrain") && _audioSource.isActiveAndEnabled){
            _audioSource.Play();
        }
        if (_impactSounds.Length > 0){
            _sharedAudio.PlaySharedSound(_impactSounds[Random.Range(0, _impactSounds.Length)], transform.position, _impactVolume, false);
        }
    }

    private void OnCollisionExit(Collision other){
        if (other.gameObject.CompareTag("Terrain"))
        {
            _audioSource.Stop();
        }
    }

    //Manhattan 3d speed
    public float GetSpeed(){
        Vector3 v = _rigidbody.velocity;
        return (Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z));
    }

    //Play one shot sound in this actors location
    public void PlayOneShotSound(AudioClip clip, float volume) {
        volume=Mathf.Clamp(volume, 0, 3);
        _sharedAudio.PlaySharedSound(clip, transform.position, volume, true);
    }
}
