using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    public SpriteRenderer rend;
    public Sprite flower;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        flower = Resources.Load<Sprite>("FlowerSprite");

    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) 
    {
        rend.sprite = flower;    
    }
}
