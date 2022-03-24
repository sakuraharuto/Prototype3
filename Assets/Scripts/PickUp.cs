using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{   
    [SerializeField] AudioClip pickUpSFX;
    private void OnTriggerEnter2D(Collider2D other) {
        // Add score, play sound, destory object

        AudioSource.PlayClipAtPoint(pickUpSFX, Camera.main.transform.position);
        FindObjectOfType<GameSession>().addItemCount(1);  // We only picked up 1 item
        Destroy(gameObject);
    }
}
