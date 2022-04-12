using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        theStateManager = GameObject.FindObjectOfType<StateManager>();
        myText = GetComponent <Text>();
    }

    Text myText;
    StateManager theStateManager;

    string[] numberWords = { "White", "Red" };

    // Update is called once per frame
    void Update()
    {
        myText.text = "Current Player: " + numberWords[theStateManager.CurrentPlayerId];
    }
}
