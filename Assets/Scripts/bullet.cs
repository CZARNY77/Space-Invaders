using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    float headerEdge;
    [SerializeField] float speed;
    [SerializeField] bool directionDown;
    float direction;
    // Start is called before the first frame update
    void Start()
    {
        headerEdge = GetEdge();
        direction = directionDown ? -1 : 1;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (transform.position.y > headerEdge + 2f || transform.position.y < -headerEdge - 2f) Destroy(gameObject);

        transform.position += new Vector3(0, direction) * Time.deltaTime * speed;

    }

    float GetEdge()
    {
        return -Camera.main.ScreenToWorldPoint(Vector3.down).y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle") || collision.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
