using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectAfterTimer : MonoBehaviour
{
    [SerializeField] float timeToDestroyObject = 2f;

    private void Start()
    {
        Invoke(nameof(TimerToDestroy), timeToDestroyObject);
    }

    void TimerToDestroy()
    {
        Destroy(gameObject);
    }
}
