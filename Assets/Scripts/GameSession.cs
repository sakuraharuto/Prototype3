using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{   
    [SerializeField] int itemCount = 0;
    [SerializeField] TextMeshProUGUI itemText;

    // Start is called before the first frame update
    void Start()
    {
        itemText.text = itemCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addItemCount(int numToAdd){
        itemCount += numToAdd;
        itemText.text = itemCount.ToString();
    }
}
