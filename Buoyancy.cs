using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{

    public Vector3 force, lerpSpeed, normalRotation, noRotateAxis;
    public float floatOffset, viscosity;

    private float seaLevel;
    private GameObject main;
    private Rigidbody rb;
    private Collider collider;
    private bool inWater;

    void Awake()
    {

        if (transform.GetComponent<Rigidbody>())
        {
            rb = transform.GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("No Rigidbody found on Gameobject using Buoyancy script.");
        }

        if (GameObject.Find("Main")) //Replace main with the name of the player gameobject and in a player script put "public float seaLevel;"
        {
            main = GameObject.Find("Main");
            seaLevel = main.GetComponent<Main>().seaLevel;
            seaLevel = seaLevel + floatOffset;
        }
        else
        {
            Debug.LogError("No Main object found in scene.");
        }

        if (transform.GetComponent<Collider>())
        {
            collider = transform.GetComponent<Collider>();
        }
        else
        {
            Debug.LogError("No Collider found on Gameobject using Buoyancy script.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Water")
        {
            inWater = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Water") 
        {
            inWater = false;
        }
    }

    void FixedUpdate()
    {
        if (transform.position.y - floatOffset < seaLevel & inWater)
        {
            rb.AddForce(Vector3.Lerp(new Vector3(0,Mathf.Abs(transform.position.y) * force.y, 0), rb.velocity, lerpSpeed.y));
            rb.AddForce(Vector3.Lerp(rb.velocity * -1 * viscosity, rb.velocity, lerpSpeed.y));

            if(noRotateAxis.x == 1)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, normalRotation.y, normalRotation.z), Time.deltaTime * lerpSpeed.y);
            }
            else if(noRotateAxis.y == 1)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(normalRotation.x, transform.rotation.eulerAngles.y, normalRotation.z), Time.deltaTime * lerpSpeed.y);
            }
            else if (noRotateAxis.z == 1)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(normalRotation.x, normalRotation.y, transform.rotation.eulerAngles.z), Time.deltaTime * lerpSpeed.y);
            }
        }
    }

    void Update()
    {
        normalRotation.z = transform.rotation.z;
    }

}
