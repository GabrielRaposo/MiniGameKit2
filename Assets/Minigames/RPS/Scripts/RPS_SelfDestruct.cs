using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPS_SelfDestruct : MonoBehaviour
{
    public float timer;
    void Start()
    {
        Invoke("Destroy", timer);
    }
}
