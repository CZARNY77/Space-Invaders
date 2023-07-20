using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Vector3 edge;
    bool takePosition;
    [SerializeField] GameObject bullet;
    float direction = 1f;
    float speed;
    [SerializeField] ParticleSystem destroyEffect;
    [SerializeField] Sprite destroySprite;
    void Start()
    {
        edge = Camera.main.ScreenToWorldPoint(Vector3.down);
        takePosition = false;
        direction = (int)Random.Range(0, 2f) == 0 ? 1f : -1f;
        float fireRate = 2f;
        if (fireRate > 0.5)
            fireRate -= (GameManager.instance.speedWorld * Time.deltaTime) / 100;
        else fireRate = 0.5f;
        InvokeRepeating(nameof(Shoot), 2f, fireRate);
    }

    // Update is called once per frame
    void Update()
    {
        speed = GameManager.instance.speedWorld;
        if (takePosition)
        {
            Move();
            if(speed <= 0) CancelInvoke(nameof(Shoot));
        }
        else
        {
            TakePosition();
        }
    }

    void Move()
    {
        if (transform.position.x >= -edge.x - 1f) direction = -1f;
        else if (transform.position.x <= edge.x + 1f) direction = 1f;
        transform.position += new Vector3(direction, -0.5f, 0f) * Time.deltaTime * speed;

        if (transform.position.y < edge.y - 2f) Destroy(gameObject);
    }

    void TakePosition()
    {
        float edgeY = -edge.y - 1f;
        if(edgeY - transform.position.y >= -0.2) takePosition = true;
        transform.position += new Vector3(0, edgeY - transform.position.y, 0) * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrier") || collision.CompareTag("Bullet"))
        {
            ParticleSystem currentEffect = Instantiate(destroyEffect, transform.position, Quaternion.identity);
            currentEffect.GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(0, destroySprite);
            //currentEffect.GetComponent<ParticleSystem>().startColor = Color.red;
            ParticleSystem.MainModule mainModule = currentEffect.GetComponent<ParticleSystem>().main;
            mainModule.startColor = Color.red;
            Destroy(gameObject);
        }
    }

    void Shoot()
    {
        Instantiate(bullet, transform.position + new Vector3(0,-1f,0), Quaternion.identity);
    }
}
