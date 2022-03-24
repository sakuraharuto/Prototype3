using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanToxic : MonoBehaviour
{   
    [SerializeField] AudioClip toxicRemoveSFX;

    private void OnCollisionEnter2D(Collision2D other) {
        // If player has pickups, Minus score, play sound, destory object
        int playerItemCount = FindObjectOfType<GameSession>().getItemCount();
        //Debug.Log(playerItemCount);

        if (playerItemCount > 0){
            FindObjectOfType<GameSession>().minusItemCount(1);
            AudioSource.PlayClipAtPoint(toxicRemoveSFX, Camera.main.transform.position);
            Destroy(gameObject);
        } else
            Debug.Log("bonk");  
    }
}
