using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        theDiceRoller = GameObject.FindObjectOfType<DiceRoller>();

        targetPosition = this.transform.position;
    }
    public Tile StartingTile;
    Tile currentTile;

    bool scoreMe = false;

    DiceRoller theDiceRoller;

    Tile[] moveQueue;
    int moveQueueIndex;

    Vector3 targetPosition;
    Vector3 velocity;
    float smoothTime = .25f;
    float smoothTimeVertricle = .1f;
    float smoothDistance = 0.01f;
    float smoothHeight = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(
            new Vector3(this.transform.position.x, targetPosition.y, this.transform.position.z),
            targetPosition) < smoothDistance)
        {
             if(moveQueue != null && moveQueueIndex == (moveQueue.Length) && this.transform.position.y > smoothDistance)
            {
                this.transform.position = Vector3.SmoothDamp(
                               this.transform.position,
                               new Vector3(this.transform.position.x, 0, this.transform.position.z),
                               ref velocity,
                               smoothTimeVertricle);
            }
            else
            {
                AdvanceMoveQueue();
            }
        }
        else if(this.transform.position.y < (smoothHeight - smoothDistance))
        {
            this.transform.position = Vector3.SmoothDamp(
                this.transform.position,
                new Vector3(this.transform.position.x, smoothHeight, this.transform.position.z),
                ref velocity,
                smoothTimeVertricle);
        }
        else
        {
            this.transform.position = Vector3.SmoothDamp(
                this.transform.position,
                new Vector3(targetPosition.x, smoothHeight, targetPosition.z),
                ref velocity,
                smoothTime);
        }

    }

    void AdvanceMoveQueue()
    {
        if (moveQueue != null && moveQueueIndex < moveQueue.Length)
        {
            Tile nextTile = moveQueue[moveQueueIndex];
            if (nextTile == null)
            {
                //TODO: move to scored piles 
                SetNewTargetPositon(this.transform.position + Vector3.left * 10f);
            }
            else
            {
                SetNewTargetPositon(nextTile.transform.position);
                moveQueueIndex++;
            }

        }
    }
    void SetNewTargetPositon(Vector3 pos)
    {
        targetPosition = pos;
        velocity = Vector3.zero;
    }
    void OnMouseUp()
    {
        //TODO: is the mouse over a UI object?
        // if so ignore

        if(theDiceRoller.IsDoneRolling == false)
        {
            return;
        }
        int spacesToMove = theDiceRoller.DiceTotal;

        if (spacesToMove == 0)
        {
            return;
        }

        moveQueue = new Tile[spacesToMove];
        Tile finalTile = currentTile;

        for (int i = 0; i < spacesToMove; i++)
        {
            if(finalTile == null && scoreMe == false)
            {
                finalTile = StartingTile;
            }
            else
            {
                if(finalTile.NextTiles == null || finalTile.NextTiles.Length == 0 )
                {
                    //TODO: fidn a way to score
                    // Debug.Log("Score!");
                    //Destroy(gameObject);
                    //return;
                    scoreMe = true;
                    finalTile = null;
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
            moveQueue[i] = finalTile;
        }

        moveQueueIndex = 0;
        currentTile = finalTile;
    }
}
