using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cosmos : MonoBehaviour
{
    MeshRenderer tempRenderer;
    public bool blackHole;
    void Start()
    {
        tempRenderer = GetComponent<MeshRenderer>();
        blackHole = false;
        Vector3 screen = Camera.main.ScreenToWorldPoint(Vector3.zero);
        screen.z = -0.5f;
        screen -= new Vector3(0.1f, 0.1f, 0f);
        transform.localScale = screen * -2;
    }

    // Update is called once per frame
    void Update()
    {

        float speed = GameManager.instance.speedWorld / transform.localScale.y;
        tempRenderer.material.mainTextureOffset += Vector2.up * speed * Time.deltaTime * 0.1f;


        float tilingY = tempRenderer.material.mainTextureScale.y;
        if(blackHole && tilingY >= 0.1f)
        {
            tempRenderer.material.mainTextureScale += Vector2.down * speed * Time.deltaTime * 2;
        }
        else if(!blackHole && tilingY < 1f)
        {
            tempRenderer.material.mainTextureScale += Vector2.up * speed * Time.deltaTime * 2;
        }
    }

}
