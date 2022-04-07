using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DiceValues = new int[4];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int[] DiceValues;
    public int DiceTotal;
    public void RollTheDice()
    {

        // Random number generator chooses between "0" and "1" four times to simulate the four dice in Ur
        // and output a number between "0" and "4" (inclusive)

        DiceTotal = 0;
        for (int i = 0; i < DiceValues.Length; i++)
        {
            DiceValues[i] = Random.Range(0, 2);
            DiceTotal += DiceValues[i];
        }

        Debug.Log("Rolled" + DiceValues + "(" + DiceTotal + ")");
    }
}
