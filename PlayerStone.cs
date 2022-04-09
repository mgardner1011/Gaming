using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        theDiceRoller = GameObject.FindObjectOfType<DiceRoller>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tile StartingTile;
    Tile currentTile;

    DiceRoller theDiceRoller;

    void OnMouseUp()
    {
        //TODO: is the mouse over a UI object?
        // if so ignore

        if(theDiceRoller.IsDoneRolling == false)
        {
            return;
        }
        int spacesToMove = theDiceRoller.DiceTotal;

        Tile finalTile = currentTile;

        for (int i = 0; i < spacesToMove; i++)
        {
            if(finalTile == null)
            {
                finalTile = StartingTile;
            }
            else
            {
                if(finalTile.NextTiles == null || finalTile.NextTiles.Length == 0 )
                {
                    //TODO: fidn a way to score
                    Debug.Log("Score!");
                    Destroy(gameObject);
                    return;
                }
                else if (finalTile.NextTiles.Length > 1)
                {
                    // TODO: branch based on player id
                    finalTile = finalTile.NextTiles[0];
                }
                else
                {
                    finalTile = finalTile.NextTiles[0];
                }
            }
        }
        if(finalTile == null)
        {
            return;
        }

        //moves piece to final tile in loop 

        this.transform.position = finalTile.transform.position;
        currentTile = finalTile;
    }
}
