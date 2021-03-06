﻿using UnityEngine;
using System.Collections;

public class LimbScript : MonoBehaviour
{
    public float maxVelocity;
    public GameObject particle;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != "Player" && collision.relativeVelocity.magnitude > maxVelocity)
        {
            LimbExplode();
        }
    }

    public void LimbExplode()
    {
        GetComponent<HingeJoint2D>().enabled = false;
        transform.parent = null;
        particle.SetActive(true);
    }
}
