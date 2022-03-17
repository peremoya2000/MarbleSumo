using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemyAI : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField]
    private float _moveForce = 2.0f;
    [SerializeField]
    private float _safetyRadius;
    private GameObject _target;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _target = GameObject.Find("Player");
        float scale = UnityEngine.Random.Range(0.95f, 1.05f);
        _rigidbody.mass *= scale * scale;
        this.transform.localScale *= scale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float position = transform.position.magnitude;
        if (position < _safetyRadius){
            Vector3 dirToTarget = (_target.transform.position - transform.position).normalized;
            dirToTarget.y *= 0.1f;
            _rigidbody.AddForce(dirToTarget * _moveForce);
        }else {
            _rigidbody.AddForce(-transform.position.normalized * _moveForce * 0.35f);
        }

        if (transform.position.y < -15){
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Player")){
            _moveForce /= 3f;
        }
    }
    void OnCollisionExit(Collision collision){
        if (collision.gameObject.CompareTag("Player")){
            _moveForce *= 3f;
        }
    }
}
