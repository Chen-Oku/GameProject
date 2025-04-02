using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEditor.PackageManager;
using UnityEngine;

public class CannonBallS : MonoBehaviour
{

    public float lifeTime = 5f;
    //public GameObject explosion;
    public float minY = -5f;

    void Start()
    {

    }

    void Update()
    {
        StatusCheck();

    }

    void StatusCheck()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            CannonBallSDestroy();
        }
        if (transform.position.y < minY)
        {
            CannonBallSDestroy();
        }
    }

    void CannonBallSDestroy()
    {
        //Instantiate(explosion, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CannonBallSDestroy();
       /* if (collision.gameObject.tag == "Target")
        {
            CannonBallSDestroy();
        }*/
    }

}
