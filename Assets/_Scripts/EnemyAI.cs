using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField]
    private float moveForce = 2.0f;
    private GameObject _target;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _target=GameObject.Find("Player");
        float scale = UnityEngine.Random.Range(0.95f, 1.15f);
        _rigidbody.mass*=scale;
        this.transform.localScale *= scale*1.1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 toTarget;     
        if (transform.position.magnitude > 11){
            toTarget = (-transform.position*0.2f + (_target.transform.position - transform.position)).normalized;
        }else {
            toTarget = (_target.transform.position - transform.position).normalized;
        }
        toTarget.y *= 0.1f;
        _rigidbody.AddForce(toTarget * moveForce);
        if (transform.position.y < -14) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Player")) {
            moveForce /= 3.5f;
        }
    }
    void OnCollisionExit(Collision collision){
        if (collision.gameObject.CompareTag("Player")){
            moveForce *= 3.5f;
        }
    }
}
