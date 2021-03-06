﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float movePower;
    public float presentForce;
    public float grappleTime;

    public GameObject present;

    public GameObject head;
    public GameObject torso;
    public GameObject neckStart;
    public GameObject neckEnd;
    public GameObject neckGoal;
    public GameObject presentArea;

    private float fx;
    private float fy;
    private bool grappling = true;
    private float grappleStartTime;
    private bool canPresent = true;
	
	// Update is called once per frame
	void Update ()
    {
        CheckInput();

        GetComponent<LineRenderer>().SetPosition(0, neckStart.transform.position);
        GetComponent<LineRenderer>().SetPosition(1, neckEnd.transform.position);
        GetComponent<LineRenderer>().material.mainTextureScale = new Vector2(Vector2.Distance(neckEnd.transform.position, neckStart.transform.position), 1);
    }

    void FixedUpdate()
    {
        torso.GetComponent<Rigidbody2D>().AddForce(new Vector2(fx * movePower, fy * movePower));

        if (grappling && !Grapple.attached && Time.time - grappleStartTime > grappleTime)
        {
            grappling = false;
            head.GetComponent<Grapple>().ReturnToStart(neckGoal, 0.3f);
        }
    }

    private void CheckInput()
    {
        fx = Input.GetAxis("Horizontal");
        fy = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Fire1"))
        {
            if (grappling)
            {
                grappling = false;
                head.GetComponent<Grapple>().ReturnToStart(neckGoal, 0.3f);
            }
            else if (head.transform.parent == this.transform)
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(neckEnd.transform.position);
                Vector3 direction = Input.mousePosition - pos;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                head.transform.parent = null;
                head.GetComponent<Rigidbody2D>().gravityScale = 0;
                head.GetComponent<Grapple>().Shoot(angle);
                torso.GetComponent<SpringJoint2D>().enabled = false;
                head.GetComponent<Collider2D>().isTrigger = true;

                grappling = true;
                grappleStartTime = Time.time;
                GetComponent<LineRenderer>().enabled = true;
            }
        }
        
        if (Input.GetButton("Fire2") && canPresent)
        {
            GameObject presentTemp = (GameObject)Instantiate(present, presentArea.transform.position, Quaternion.identity);
            presentTemp.GetComponent<Rigidbody2D>().AddForce(presentArea.transform.up * presentForce, ForceMode2D.Impulse);

            canPresent = false;
            StartCoroutine(Reload());
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GetComponentInChildren<TorsoScript>().Pop();
        }
    }

    IEnumerator Reload()
    {
            yield return new WaitForSeconds(0.25f);
            canPresent = true;
    }
}