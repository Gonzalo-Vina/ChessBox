using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookPiece : MonoBehaviour
{
    private GameController gameController;

    public bool firstMovement;
    //Materials
    [SerializeField] Material rookWhite_backBlack;
    [SerializeField] Material rookWhite_backWhite;
    [SerializeField] Material rookBlack_backBlack;
    [SerializeField] Material rookBlack_backWhite;

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

        firstMovement = true;
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
                this.GetComponent<MeshRenderer>().material = rookWhite_backBlack;
            }
            else
            {
                this.GetComponent<MeshRenderer>().material = rookWhite_backWhite;
            }
        }

        if (gameObject.tag == "Player2")
        {
            if ((transform.position.x + transform.position.z) % 2 == 0)
            {
                this.GetComponent<MeshRenderer>().material = rookBlack_backBlack;
            }
            else
            {
                this.GetComponent<MeshRenderer>().material = rookBlack_backWhite;
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
