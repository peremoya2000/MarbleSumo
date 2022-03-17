using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KamizakeAI : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField][Range (0,5)]
    private float moveForce = 1f;
    private GameObject _target;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _target = GameObject.Find("Player");
        float scale = UnityEngine.Random.Range(0.9f, 1.1f);
        _rigidbody.mass *= scale;
        this.transform.localScale *= scale;
    }

    // Update is called once per frame
    void FixedUpdate(){
        Vector3 dirToTarget = (_target.transform.position - transform.position);
        dirToTarget.y *= 0.1f;
        dirToTarget *= Mathf.Min(15,dirToTarget.magnitude);
        _rigidbody.AddForce(dirToTarget * moveForce);
        if (transform.position.y < -15)
        {
            Destroy(gameObject);
        }
    }
}
