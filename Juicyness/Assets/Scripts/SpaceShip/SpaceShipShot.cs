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
    private Vector3 originalPosition;
    [SerializeField] private float shootRecoil = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
        originalPosition = transform.position;
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
                AudioManager.instance.Play("BananaShoot", 1 + Random.Range(-0.5f, 0.5f));
                Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);
                if (FeatureManager.instance.isAnimationOn)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - shootRecoil, transform.position.z);
                    StartCoroutine(RecoilCoroutine());
                }
                StartCoroutine(WaitBeforeShootingAgain());
            }
        }
    }

    IEnumerator RecoilCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        transform.position = new Vector3(transform.position.x, originalPosition.y + (shootRecoil / 2), transform.position.z);
        yield return new WaitForSeconds(Mathf.Clamp(0.2f, 0, 15));
        transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
    }

    IEnumerator WaitBeforeShootingAgain()
    {
        yield return new WaitForSeconds(timeBetweenBullet);
        canShoot = true;
        Animator.SetBool("IsShooting", false);
    }
}
