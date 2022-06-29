using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDelay : MonoBehaviour
{
    float fadeSpeed = 1;

    float beginTintAlpha = 0.5f;
    Color myColor;

    void Start()
    {
    }

    void Update()
    {

        beginTintAlpha -= Time.deltaTime * fadeSpeed;

    }
}
