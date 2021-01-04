using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipShot : MonoBehaviour
{

    [SerializeField] 
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletSpawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);
        }
    }
}
