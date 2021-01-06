using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipShot : MonoBehaviour
{

    [SerializeField] 
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletSpawner;
    [SerializeField]
    private float timeBetweenBullet = 0.5f;
    private bool canShoot = true;
    [SerializeField] private GameObject particleObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.canPlay)
        {
            if (Input.GetMouseButtonDown(0) && canShoot)
            {
                canShoot = false;
                if (FeatureManager.instance.isParticleEffectsOn)
                {
                    particleObject.SetActive(false);
                    particleObject.SetActive(true);
                }
                Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);
                StartCoroutine(WaitBeforeShootingAgain());
            }
        }
    }

    IEnumerator WaitBeforeShootingAgain()
    {
        yield return new WaitForSeconds(timeBetweenBullet);
        canShoot = true;
    }
}
