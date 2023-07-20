using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : Asteroid
{
    void Start()
    {
        bottonEdge = GetEdge();
    }
    void Update()
    {
        Move();
        Rotation(1, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !GameManager.instance.inHole)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.StartBoost();
        }
    }


}
