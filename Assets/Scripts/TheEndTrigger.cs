using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheEndTrigger : MonoBehaviour
{
    public GameObject UIObject;

    // Start is called before the first frame update
    void Start()
    {
        UIObject.SetActive(false);
    }


    void OnTriggerEnter2D(Collider2D other) {
        UIObject.SetActive(true);
        Destroy(gameObject);
    }
}
