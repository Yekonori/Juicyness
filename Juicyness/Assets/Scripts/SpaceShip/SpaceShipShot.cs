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

    private Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.canPlay)
        {
            if (Input.GetKeyDown(KeyCode.Space) && canShoot)
            {
                canShoot = false;

                Animator.SetBool("IsShooting", true);

                if (FeatureManager.instance.isParticleEffectsOn)
                {
                    particleObject.SetActive(false);
                    particleObject.SetActive(true);
                }
                AudioManager.instance.Play("BananaShoot");
                Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);
                StartCoroutine(WaitBeforeShootingAgain());
            }
        }
    }

    IEnumerator WaitBeforeShootingAgain()
    {
        yield return new WaitForSeconds(timeBetweenBullet);
        canShoot = true;
        Animator.SetBool("IsShooting", false);
    }
}
