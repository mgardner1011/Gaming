using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneStorage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Create one stone for each placeholder spot
        for (int i = 0; i < this.transform.childCount; i++)
        {
            //Instantiate a new copy of the stone prefab
            GameObject theStone = Instantiate(StonePrefab);
            theStone.GetComponent<PlayerStone>().StartingTile = this.StartingTile;
            theStone.GetComponent<PlayerStone>().MyStoneStorage = this;

            AddStoneToStorage(theStone, this.transform.GetChild(i) );
        }
    }

    public GameObject StonePrefab;
    public Tile StartingTile;

    public void AddStoneToStorage(GameObject theStone, Transform thePlaceholder=null)
    {
        if(thePlaceholder == null)
        {
            //Find the first empty placeholder
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Transform p = this.transform.GetChild(i);
                if(p.childCount == 0)
                {
                    // The placeholder is empty
                    thePlaceholder = p;
                    break;
                }
            }
            if (thePlaceholder == null)
            {
                Debug.LogError("We are trying to add a stone, but don't have empty places.");
                    return;
            }
        }
        //parent the stone to the placeholder 
        theStone.transform.SetParent(thePlaceholder);
        //reset the stones local position to 0, 0, 0
        theStone.transform.localPosition = Vector3.zero;
    }
}
