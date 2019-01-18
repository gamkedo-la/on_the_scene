using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{

    // Scroll main texture based on time
    public float scrollSpeed = 0.02f;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        Vector2 textureOffset = new Vector2(Time.time * scrollSpeed, Time.time * -scrollSpeed);
        rend.material.mainTextureOffset = textureOffset;
    }
}