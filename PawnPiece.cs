using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnPiece : MonoBehaviour
{
    private GameController gameController;

    public bool firstMovement;

    //Materials
    [SerializeField] Material pawnWhite_backBlack;
    [SerializeField] Material pawnWhite_backWhite;
    [SerializeField] Material pawnBlack_backBlack;
    [SerializeField] Material pawnBlack_backWhite;

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        if (transform.position.z <= 1)
        { 
            gameObject.tag = "Player1";
        }
        if (transform.position.z >= 6) 
        { 
            gameObject.tag = "Player2";
        }
        
        ChangeMaterial();
        firstMovement = true;
    }

    private void Update()
    {
        ChangeMaterial();
    }

    
    private void ChangeMaterial()
    {
        if (gameObject.tag == "Player1")
        {
            if ((transform.position.x + transform.position.z) % 2 == 0)
            {
                this.GetComponent<MeshRenderer>().material = pawnWhite_backBlack;
            }
            else
            {
                this.GetComponent<MeshRenderer>().material = pawnWhite_backWhite;
            }
        }

        if (gameObject.tag == "Player2")
        {
            if ((transform.position.x + transform.position.z) % 2 == 0)
            {
                this.GetComponent<MeshRenderer>().material = pawnBlack_backBlack;
            }
            else
            {
                this.GetComponent<MeshRenderer>().material = pawnBlack_backWhite;
            }
        }
    }
        

    private void OnMouseEnter()
    {
        if(this.gameObject.CompareTag("Player1") && !gameController.inMenuPause)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Player1");
        }
        else if (this.gameObject.CompareTag("Player2") && !gameController.inMenuPause)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Player2");
        }
    }
    private void OnMouseExit()
    {
            this.gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
