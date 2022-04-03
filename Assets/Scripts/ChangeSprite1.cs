using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite1 : MonoBehaviour
{
    [SerializeField] Sprite changeToSprite;
    [SerializeField] AudioClip changeSFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<SpriteRenderer>().sprite = changeToSprite;
        AudioSource.PlayClipAtPoint(changeSFX, Camera.main.transform.position);
        Destroy(gameObject);
    }
}
