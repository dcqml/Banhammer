using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float ScrollSpeed;
    public int Tiles;

    protected Vector2 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float shift = Mathf.Repeat(Time.time * ScrollSpeed, Tiles);
        transform.position = startPos + Vector2.down * shift;
    }
}
