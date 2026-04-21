using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DashEvasion : MonoBehaviour
{
    float speed = 10.0f;
    int hitPoit;
    bool isHide;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitPoit = 10;
    }

    // Update is called once per frame
    void Update()
    {
     
        if (Input.GetKeyDown(KeyCode.Space) && !isHide)
        {
            StartCoroutine("StartUnrivaled");
        }
    }
    IEnumerator StartUnrivaled()
    {
        isHide = true;
        yield return new WaitForSeconds(3.0f);
        isHide = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            if (!isHide)
            {
                hitPoit--;
            }
            Destroy(other.gameObject);
            Debug.Log(hitPoit);
        }
    }
}


