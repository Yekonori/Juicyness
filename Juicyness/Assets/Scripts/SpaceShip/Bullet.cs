﻿using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool isEnemyBullet = false;
    [SerializeField] private float speed = 3;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Sprite baseSprite;
    [SerializeField] private Sprite coolSprite;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trail.enabled = false;
        if (FeatureManager.instance.isParticleEffectsOn)
        {
            trail.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.canPlay)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        if (FeatureManager.instance.isSpriteOn)
        {
            spriteRenderer.sprite = coolSprite;
        }
        else
        {
            spriteRenderer.sprite = baseSprite;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEnemyBullet)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                return;
            }
            else if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemyCollided = collision.GetComponent<Enemy>();

                if (enemyCollided != null)
                {
                    ScoreManager.instance.ChangeScore(collision.GetComponent<Enemy>().enemyValue);
                    collision.GetComponent<Enemy>().Die();
                }
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                return;
            }
        }

        if (isEnemyBullet)
        {
            Destroy(this.transform.parent.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
