using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] Sprite[] asteroidSprite;
    [SerializeField] ParticleSystem destroyEffect;
    SpriteRenderer spriteRenderer;
    protected float bottonEdge;
    [SerializeField] float speed;
    float size;
    int direction;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        int rand = Random.Range(0, asteroidSprite.Length);
        spriteRenderer.sprite = asteroidSprite[rand];

        bottonEdge = GetEdge();
        speed = Random.Range(1f, 2f);
        size = Random.Range(0.9f, 1.5f);
        direction = Random.Range(0, 2) == 0 ? 1 : -1;

        transform.localScale = new Vector3(size, size, size);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotation(direction, 1);
    }

    protected void Move()
    {
        if (transform.position.y < bottonEdge - 2f) Destroy(gameObject);

        transform.position -= new Vector3(0, 1f, 0) * GameManager.instance.speedWorld * Time.deltaTime * speed;

    }

    protected void Rotation(int d, float acceleration)
    {

        transform.Rotate(new Vector3(0, 0, GameManager.instance.speedWorld * 0.1f * speed * d * acceleration));
    }

    protected float GetEdge()
    {
        return Camera.main.ScreenToWorldPoint(Vector3.zero).y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrier") || collision.CompareTag("Bullet"))
        {
            ParticleSystem currentEffect = Instantiate(destroyEffect, transform.position, Quaternion.identity);
            currentEffect.GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(0, spriteRenderer.sprite);
            Destroy(gameObject);
        }
    }
}
