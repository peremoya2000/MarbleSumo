using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RotateCamera : MonoBehaviour
{
    private float spd=45.0f;
    private float hInput;

    // Update is called once per frame
    void Update(){
        hInput = Input.GetAxis("HorizontalLook");
        if (hInput == 0) return;

        transform.Rotate(Vector3.up, hInput*spd*Time.deltaTime);

        GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
        powerUps = powerUps.Concat(GameObject.FindGameObjectsWithTag("NukePowerUp")).ToArray();
        powerUps = powerUps.Concat(GameObject.FindGameObjectsWithTag("MultiplierPowerUp")).ToArray();
        foreach (GameObject o in powerUps){
            o.transform.rotation = this.transform.rotation;
        }
    }
}
