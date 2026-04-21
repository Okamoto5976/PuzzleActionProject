using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallShot : MonoBehaviour
{
    public GameObject prefab;
    float time;

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if(time<=0.0f)
        {
            Shot();
            time = 1.0f;
            
        }
    }

    void Shot()
    {
        GameObject ball=GameObject.Instantiate(prefab,this.transform.position,Quaternion.identity)as GameObject;
        ball.GetComponent<Rigidbody>().AddForce(transform.forward * -500);
    }
}
