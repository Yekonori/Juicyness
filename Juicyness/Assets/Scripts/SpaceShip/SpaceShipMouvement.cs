﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipMouvement : MonoBehaviour
{

    [SerializeField] private float speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Horizontal") > 0)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.position -= Vector3.right * speed * Time.deltaTime;
        }
    }
}