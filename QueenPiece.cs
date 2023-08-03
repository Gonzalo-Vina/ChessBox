using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenPiece : MonoBehaviour
{
    private GameController gameController;

    //Materials
    [SerializeField] Material queenWhite_backBlack;
    [SerializeField] Material queenWhite_backWhite;
    [SerializeField] Material queenBlack_backBlack;
    [SerializeField] Material queenBlack_backWhite;

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
                this.GetComponent<MeshRenderer>().material = queenWhite_backBlack;
            }
            else
            {
                this.GetComponent<MeshRenderer>().material = queenWhite_backWhite;
            }
        }

        if (gameObject.tag == "Player2")
        {
            if ((transform.position.x + transform.position.z) % 2 == 0)
            {
                this.GetComponent<MeshRenderer>().material = queenBlack_backBlack;
            }
            else
            {
                this.GetComponent<MeshRenderer>().material = queenBlack_backWhite;
            }
        }
    }


    private void OnMouseEnter()
    {
        if (this.gameObject.CompareTag("Player1") && !gameController.inMenuPause)
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
