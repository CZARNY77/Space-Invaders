using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Asteroid
{

    void Start()
    {
        bottonEdge = GetEdge();
    }
    void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !GameManager.instance.inHole)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (GameManager.instance.countShields <= 0)
                player.AnimSetTrigger("newShield");
            else
                player.PlayAnim();
            GameManager.instance.AddShields(1);
            Destroy(gameObject);
        }
    }
}
