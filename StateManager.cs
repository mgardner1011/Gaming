using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int NumberOfPlayers = 2;
    public int CurrentPlayerId = 0;

    public int DiceTotal;
    public bool IsDoneRolling = false;
    public bool IsDoneClicking = false;
    public bool IsDoneAnimatting= false;

    public GameObject NoLegalMovesPopup;

    public void NewTurn()
    //Start of a players turn
    //No roll yet
    {
        IsDoneRolling = false;
        IsDoneClicking = false;
        IsDoneAnimatting = false;

        CurrentPlayerId = (CurrentPlayerId + 1) % NumberOfPlayers;
    }


    public void RollAgain()
    {
        IsDoneRolling = false;
        IsDoneClicking = false;
        IsDoneAnimatting = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Is the turn done?
        if(IsDoneRolling && IsDoneClicking && IsDoneAnimatting)
        {
            NewTurn();
        }
    }

    public void CheckLegalMoves()
    {
        //if we roll zero we have no legal moves
        if(DiceTotal == 0)
        {
            StartCoroutine(NoLegalMoveCoroutine());
            return;
        }
        //loop through all of a players stones

        PlayerStone[] pss = GameObject.FindObjectsOfType<PlayerStone>();
        bool hasLegalMove = false;
        foreach(PlayerStone ps in pss)
        {
            if (ps.PlayerId == CurrentPlayerId)
            {
                if (ps.CanLegallyMoveAhead(DiceTotal))
                {
                    //Highlight legal stoned
                    hasLegalMove = true;
                }
            }
        }

        //if no legal moves, wait a sec then move to next player
        //(possibly give a message)
        if(hasLegalMove == false)
        {
            StartCoroutine(NoLegalMoveCoroutine());
            return;
        }
    }

    IEnumerator NoLegalMoveCoroutine()
    {
        //display message
        NoLegalMovesPopup.SetActive(true);
        //wait 1 second
        yield return new WaitForSeconds(1f);
        NoLegalMovesPopup.SetActive(false);
        NewTurn();
    }
}
