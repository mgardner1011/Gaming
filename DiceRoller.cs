using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public bool IsDoneRolling = false;

    public Sprite[] DiceImageOne;
    public Sprite[] DiceImageZero;

    public void NewTurn()
        //Start of a players turn
        //No roll yet
    {
        IsDoneRolling = false;
    }

    public void RollTheDice()
    {

        // Random number generator chooses between "0" and "1" four times to simulate the four dice in Ur
        // and output a number between "0" and "4" (inclusive)

        DiceTotal = 0;
        for (int i = 0; i < DiceValues.Length; i++)
        {
            DiceValues[i] = Random.Range(0, 2);
            DiceTotal += DiceValues[i];

            //Displays sprite coresponding to the number generated. 
            //Since there are three versions of a "1" or "0" it randomly chooses the sprite to display
            //from avaiolable versions

            if (DiceValues[i] == 0)
            {
                this.transform.GetChild(i).GetComponent<Image>().sprite =
                    DiceImageZero[Random.Range(0, DiceImageZero.Length)];
            }
            else
            {
                this.transform.GetChild(i).GetComponent<Image>().sprite =
                    DiceImageOne[Random.Range(0, DiceImageOne.Length)];
            }
            //setting doneRolling to true as a place holder for possible future animation
            IsDoneRolling = true;
        }

       // Debug.Log("Rolled: " + DiceTotal);
    }
}
