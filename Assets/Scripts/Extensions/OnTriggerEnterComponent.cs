using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterComponent : MonoBehaviour
{
    public UnityEvent OnTriggerEnter = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter.Invoke();
    }
}