using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingPiece : MonoBehaviour
{
    private GameController gameController;

    public bool firstMovement;

    //Materials
    [SerializeField] Material kingWhite_backBlack;
    [SerializeField] Material kingWhite_backWhite;
    [SerializeField] Material kingBlack_backBlack;
    [SerializeField] Material kingBlack_backWhite;

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        if (transform.position.z <= 1)
        {
            gameObject.tag = "Player1";
            gameObject.name += gameObject.tag;
        }
        if (transform.position.z >= 6)
        {
            gameObject.tag = "Player2";
            gameObject.name += gameObject.tag;
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
                this.GetComponent<MeshRenderer>().material = kingWhite_backBlack;
            }
            else
            {
                this.GetComponent<MeshRenderer>().material = kingWhite_backWhite;
            }
        }

        if (gameObject.tag == "Player2")
        {
            if ((transform.position.x + transform.position.z) % 2 == 0)
            {
                this.GetComponent<MeshRenderer>().material = kingBlack_backBlack;
            }
            else
            {
                this.GetComponent<MeshRenderer>().material = kingBlack_backWhite;
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
