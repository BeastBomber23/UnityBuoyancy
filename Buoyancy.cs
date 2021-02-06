using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{

    public Vector3 force, lerpSpeed;
    public float rotateSpeed, floatOffset, viscosity;

    public float seaLevel;
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
        print(other.transform.name);

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
        if (transform.position.y < seaLevel & inWater)
        {

            rb.AddForce(Vector3.Lerp(new Vector3(0,Mathf.Abs(transform.position.y) * force.y, 0), rb.velocity, lerpSpeed.y));
            rb.AddForce(Vector3.Lerp(rb.velocity * -1 * viscosity, rb.velocity, lerpSpeed.y));
            Quaternion q = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * lerpSpeed.y);

        }
    }

}
