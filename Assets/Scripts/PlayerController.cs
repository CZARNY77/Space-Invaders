using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    Animator animator;
    float tempSpeedWorld;
    [SerializeField] Cosmos background;
    [SerializeField] GameObject bullet;
    public float fireRate;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        background = FindObjectOfType<Cosmos>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (GameManager.instance.endGame) return;
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = new Vector3(mousePosition.x - transform.position.x, 0, 0);
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void Shoot()
    {
        if(fireRate >= 0.2)
            fireRate -= (GameManager.instance.speedWorld * Time.deltaTime)/ 100;
        Instantiate(bullet, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        Invoke(nameof(Shoot), fireRate);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.CompareTag("Obstacle") || collider.CompareTag("Bullet")) && !GameManager.instance.immortality)
        {
            Destroy(collider.gameObject);
            animator.SetTrigger("broken");
            GameManager.instance.UseShield();
            Invoke(nameof(CancelImmortality), 0.5f);
            if(GameManager.instance.countShields > 0)
                animator.SetTrigger("newShield");
        }
    }

    void CancelImmortality()
    {
        GameManager.instance.immortality = false;
    }

    public void AnimSetTrigger(string name)
    {
        animator.ResetTrigger("broken");
        animator.SetTrigger(name);
    }
    public void PlayAnim()
    {
        animator.Play(0);
    }

    public void StartBoost()
    {
        stopShoot();
        tempSpeedWorld = GameManager.instance.speedWorld;
        GameManager.instance.inHole = true;
        GameManager.instance.immortality = true;
        GameManager.instance.speedWorld *= 10;
        background.blackHole = true;
        Invoke(nameof(StopBoost), 3f);
    }
    void StopBoost()
    {
        startShoot();
        background.blackHole = false;
        GameManager.instance.inHole = false;
        GameManager.instance.immortality = false;
        GameManager.instance.speedWorld = tempSpeedWorld;
    }

    public void startShoot()
    {
        Invoke(nameof(Shoot), fireRate);
    }
    public void stopShoot()
    {
        CancelInvoke(nameof(Shoot));
    }

}
