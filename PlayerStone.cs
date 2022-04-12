using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        theStateManager = GameObject.FindObjectOfType<StateManager>();

        targetPosition = this.transform.position;

    }
    public Tile StartingTile;
    Tile currentTile;

    public int PlayerId;
    public StoneStorage MyStoneStorage;


    StateManager theStateManager;


    Tile[] moveQueue;
    int moveQueueIndex;

    bool isAnimatting = false;

    Vector3 targetPosition;
    Vector3 velocity;
    float smoothTime = .25f;
    float smoothTimeVertricle = .1f;
    float smoothDistance = 0.01f;
    float smoothHeight = 0.5f;

    PlayerStone stoneToBop;

    // Update is called once per frame
    void Update()
    {
        if(isAnimatting == false)
        {
            //we have moved and finished animatting.
            return;
        }
        if (Vector3.Distance(
            new Vector3(this.transform.position.x, targetPosition.y, this.transform.position.z),
            targetPosition) < smoothDistance)
        {
            //We've reached the target position -- Do we still have moves in the queue?
            if(
                (moveQueue == null || moveQueueIndex == (moveQueue.Length))
                &&
                ((this.transform.position.y-smoothDistance) > targetPosition.y)
               )
            {
                //We are totally out of moves and too high, the only thing left to do it drop
                this.transform.position = Vector3.SmoothDamp(
                           this.transform.position,
                           new Vector3(this.transform.position.x, targetPosition.y, this.transform.position.z),
                           ref velocity,
                           smoothTimeVertricle);

                //Check for bops
                if (stoneToBop != null)
                {
                    stoneToBop.ReturnToStorage();
                    stoneToBop = null;
                }
            }
            else
            {
                //right posotion right height -- advance queue
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
        else
        {
            // The move queue is empty so we are done animatting.
            this.isAnimatting = false;
            theStateManager.IsDoneAnimatting = true;

            //Are we on a roll again space
            if(currentTile != null && currentTile.IsRollAgain)
            {
                theStateManager.RollAgain();
            }
        }
    }
    void SetNewTargetPositon(Vector3 pos)
    {
        targetPosition = pos;
        velocity = Vector3.zero;
        isAnimatting = true;
    }
    void OnMouseUp()
    {
        //TODO: is the mouse over a UI object?
        // if so ignore

        //is this the right player
        if(theStateManager.CurrentPlayerId != PlayerId)
        {
            return;
        }

        if(theStateManager.IsDoneRolling == false)
        {
            //We can't move yet.
            return;
        }
        if(theStateManager.IsDoneClicking == true)
        {
            //we've already moved.
            return;
        }
        int spacesToMove = theStateManager.DiceTotal;

        if (spacesToMove == 0)
        {
            return;
        }

        moveQueue = GetTilesAhead(spacesToMove);
        Tile finalTile = moveQueue[moveQueue.Length - 1];
        
        //TODO: Check to see if destination is legal


        if(finalTile!= null)
        {
            if(CanLegallyMoveTo(finalTile)== false)
            {
                finalTile = currentTile;
                moveQueue = null;
                return;
            }
            // If there is an enemy stone in our legal space then kick it out
            if(finalTile.PlayerStone!= null)
            {
                //finalTile.PlayerStone.ReturnToStorage();
                stoneToBop = finalTile.PlayerStone;
                stoneToBop.currentTile.PlayerStone = null;
                stoneToBop.currentTile = null;
            }
        }

        this.transform.SetParent(null); //Become Batman

        if(currentTile != null)
        {
            currentTile.PlayerStone = null;
        }
        currentTile = finalTile;
        if (finalTile.IsScoringSpace == false)
        {
            finalTile.PlayerStone = this;
        }

        moveQueueIndex = 0;

        theStateManager.IsDoneClicking = true;
        this.isAnimatting = true;
    }

    //Return the list of tiles x moves ahead of us
    Tile[] GetTilesAhead(int spacesToMove)
    {

        if (spacesToMove == 0)
        {
            return null;
        }
        //Where should we end up?
        Tile[] listOfTiles = new Tile[spacesToMove];
        Tile finalTile = currentTile;

        for (int i = 0; i < spacesToMove; i++)
        {
            if (finalTile == null)
            {
                finalTile = StartingTile;
            }
            else
            {
                if (finalTile.NextTiles == null || finalTile.NextTiles.Length == 0)
                {
                    //We are overshooting the victory roll -- so return null in array
                    //break out of the loop and return array with nulls at end, and no legal moves for that stone
                    break;
                }
                else if (finalTile.NextTiles.Length > 1)
                {
                    //branch based on player id
                    finalTile = finalTile.NextTiles[PlayerId];
                }
                else
                {
                    finalTile = finalTile.NextTiles[0];
                }
            }
            listOfTiles[i] = finalTile;
        }
        return listOfTiles;
    }

    //Return the final tile we would lind on if we moved x spaces
    Tile GetTileAhead(int spacesToMove)
    {
        Tile[] tiles = GetTilesAhead(spacesToMove);

        if(tiles == null)
        {
            //We aren't moving at all, so return current tile
            return currentTile;
        }
        return tiles[tiles.Length-1];
    }

    public bool CanLegallyMoveAhead(int SpacesToMove)
    {
        Tile theTile = GetTileAhead(SpacesToMove);

        return CanLegallyMoveTo(theTile);
    }
    bool CanLegallyMoveTo( Tile destinaltionTile)
    {

        if(destinaltionTile == null)
        {

            //NOTE: null tile means that we are over shooting the visctory roll
            // and this is not legal
            return false;
        }
        //does the tile already have a stone
        if(destinaltionTile.PlayerStone == null)
        {
            return true;
        }
        //is it our stone or an enemie's stone
        if(destinaltionTile.PlayerStone.PlayerId == this.PlayerId)
        {
            //We can't land on our own stone
            return false;
        }

        if(destinaltionTile.IsRollAgain == true)
        {
            //can't bop someone on a safe tile
            return false;
        }
        //is it a safe stone
        return true;

    }

    public void ReturnToStorage()
    {
        //currentTile.PlayerStone = null;
        //currentTile = null;

        moveQueue = null;

        Vector3 savePosition = this.transform.position;

        MyStoneStorage.AddStoneToStorage( this.gameObject);

        SetNewTargetPositon(this.transform.position);

        this.transform.position = savePosition;
    }
}
