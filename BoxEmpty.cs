using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEmpty : MonoBehaviour
{
    private GameController gameController;

    // Material and textures
    [SerializeField] Material ligthBox;
    [SerializeField] Material darkBox;

    private GameObject pieceUpBoxEmpty;
    private GameObject pieceDownBoxEmpty;
    private GameObject pieceLeftBoxEmpty;
    private GameObject pieceRightBoxEmpty;
    private GameObject pieceUpLeftBoxEmpty;
    private GameObject pieceUpRightBoxEmpty;
    private GameObject pieceDownLeftBoxEmpty;
    private GameObject pieceDownRightBoxEmpty;


    [SerializeField] public bool isThreatenedPlayer1;
    [SerializeField] public bool isThreatenedPlayer2;

    void Start()
    {
        this.tag = "Chessboard";
        isThreatenedPlayer1 = false;
        isThreatenedPlayer2 = false;
        ChangeMaterial();

        pieceUpBoxEmpty = null;
        pieceDownBoxEmpty = null;
        pieceLeftBoxEmpty = null;
        pieceRightBoxEmpty = null;
        pieceUpLeftBoxEmpty = null;
        pieceUpRightBoxEmpty = null;
        pieceDownLeftBoxEmpty = null;
        pieceDownRightBoxEmpty = null;

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void ChangeMaterial()
    {
        if ((transform.position.x + transform.position.z) % 2 == 0 || (transform.position.x + transform.position.z) == 0)
        {
            this.GetComponent<MeshRenderer>().material = darkBox;
        }
        else
        {
            this.GetComponent<MeshRenderer>().material = ligthBox;
        }
    }
    public void ChangeName()
    {
        this.name = transform.position.x + ":" + transform.position.z;
    }
    public void CheckThreat()
    {
        GetGameObjectAround(this.gameObject);

        int i = 0;

        while (i < 8)
        {
            //UP
            if(pieceUpBoxEmpty != null)
            {
                if (pieceUpBoxEmpty.CompareTag("Chessboard"))
                {
                    GetGameObjectUp(pieceUpBoxEmpty);
                }
                else if(pieceUpBoxEmpty.CompareTag("Player1"))
                {
                    if(pieceUpBoxEmpty.GetComponent<RookPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if(pieceUpBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceUpBoxEmpty.GetComponent<KingPiece>() != null && i==0)
                    {
                        isThreatenedPlayer1 = true;
                    }

                    pieceUpBoxEmpty = null;
                } 
                else if (pieceUpBoxEmpty.CompareTag("Player2"))
                {
                    if (pieceUpBoxEmpty.GetComponent<RookPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceUpBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceUpBoxEmpty.GetComponent<KingPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer2 = true;
                    }

                    pieceUpBoxEmpty = null;
                }
            }
            //DOWN
            if (pieceDownBoxEmpty != null)
            {
                if (pieceDownBoxEmpty.CompareTag("Chessboard"))
                {
                    GetGameObjectDown(pieceDownBoxEmpty);
                }
                else if (pieceDownBoxEmpty.CompareTag("Player1"))
                {
                    if (pieceDownBoxEmpty.GetComponent<RookPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceDownBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceDownBoxEmpty.GetComponent<KingPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer1 = true;
                    }

                    pieceDownBoxEmpty = null;
                }
                else if (pieceDownBoxEmpty.CompareTag("Player2"))
                {
                    if (pieceDownBoxEmpty.GetComponent<RookPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceDownBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceDownBoxEmpty.GetComponent<KingPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer2 = true;
                    }

                    pieceDownBoxEmpty = null;
                }
            }
            //LEFT
            if (pieceLeftBoxEmpty != null)
            {
                if (pieceLeftBoxEmpty.CompareTag("Chessboard"))
                {
                    GetGameObjectLeft(pieceLeftBoxEmpty);
                }
                else if (pieceLeftBoxEmpty.CompareTag("Player1"))
                {
                    if (pieceLeftBoxEmpty.GetComponent<RookPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceLeftBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceLeftBoxEmpty.GetComponent<KingPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer1 = true;
                    }

                    pieceLeftBoxEmpty = null;
                }
                else if (pieceLeftBoxEmpty.CompareTag("Player2"))
                {
                    if (pieceLeftBoxEmpty.GetComponent<RookPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceLeftBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceLeftBoxEmpty.GetComponent<KingPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer2 = true;
                    }

                    pieceLeftBoxEmpty = null;
                }
            }
            //RIGHT
            if (pieceRightBoxEmpty != null)
            {
                if (pieceRightBoxEmpty.CompareTag("Chessboard"))
                {
                    GetGameObjectRight(pieceRightBoxEmpty);
                }
                else if (pieceRightBoxEmpty.CompareTag("Player1"))
                {
                    if (pieceRightBoxEmpty.GetComponent<RookPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceRightBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceRightBoxEmpty.GetComponent<KingPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer1 = true;
                    }

                    pieceRightBoxEmpty = null;
                }
                else if (pieceRightBoxEmpty.CompareTag("Player2"))
                {
                    if (pieceRightBoxEmpty.GetComponent<RookPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceRightBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceRightBoxEmpty.GetComponent<KingPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer2 = true;
                    }

                    pieceRightBoxEmpty = null;
                }
            }
            //UP-LEFT
            if (pieceUpLeftBoxEmpty != null)
            {
                if (pieceUpLeftBoxEmpty.CompareTag("Chessboard"))
                {
                    GetGameObjectUpLeft(pieceUpLeftBoxEmpty);
                }
                else if (pieceUpLeftBoxEmpty.CompareTag("Player1"))
                {
                    if (pieceUpLeftBoxEmpty.GetComponent<BishopPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceUpLeftBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceUpLeftBoxEmpty.GetComponent<KingPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer1 = true;
                    }

                    pieceUpLeftBoxEmpty = null;
                }
                else if (pieceUpLeftBoxEmpty.CompareTag("Player2"))
                {
                    if (pieceUpLeftBoxEmpty.GetComponent<BishopPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceUpLeftBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceUpLeftBoxEmpty.GetComponent<KingPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceUpLeftBoxEmpty.GetComponent<PawnPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer2 = true;
                    }

                    pieceUpLeftBoxEmpty = null;
                }
            }
            //UP-RIGHT
            if (pieceUpRightBoxEmpty != null)
            {
                if (pieceUpRightBoxEmpty.CompareTag("Chessboard"))
                {
                    GetGameObjectUpRight(pieceUpRightBoxEmpty);
                }
                else if (pieceUpRightBoxEmpty.CompareTag("Player1"))
                {
                    if (pieceUpRightBoxEmpty.GetComponent<BishopPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceUpRightBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceUpRightBoxEmpty.GetComponent<KingPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer1 = true;
                    }

                    pieceUpRightBoxEmpty = null;
                }
                else if (pieceUpRightBoxEmpty.CompareTag("Player2"))
                {
                    if (pieceUpRightBoxEmpty.GetComponent<BishopPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceUpRightBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceUpRightBoxEmpty.GetComponent<KingPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceUpRightBoxEmpty.GetComponent<PawnPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer2 = true;
                    }

                    pieceUpRightBoxEmpty = null;
                }
            }
            //DOWN-LEFT
            if (pieceDownLeftBoxEmpty != null)
            {
                if (pieceDownLeftBoxEmpty.CompareTag("Chessboard"))
                {
                    GetGameObjectDownLeft(pieceDownLeftBoxEmpty);
                }
                else if (pieceDownLeftBoxEmpty.CompareTag("Player1"))
                {
                    if (pieceDownLeftBoxEmpty.GetComponent<BishopPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceDownLeftBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceDownLeftBoxEmpty.GetComponent<KingPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceDownLeftBoxEmpty.GetComponent<PawnPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer1 = true;
                    }

                    pieceDownLeftBoxEmpty = null;
                }
                else if (pieceDownLeftBoxEmpty.CompareTag("Player2"))
                {
                    if (pieceDownLeftBoxEmpty.GetComponent<BishopPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceDownLeftBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceDownLeftBoxEmpty.GetComponent<KingPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer2 = true;
                    }

                    pieceDownLeftBoxEmpty = null;
                }
            }
            //DOWN-RIGHT
            if (pieceDownRightBoxEmpty != null)
            {
                if (pieceDownRightBoxEmpty.CompareTag("Chessboard"))
                {
                    GetGameObjectDownRight(pieceDownRightBoxEmpty);
                }
                else if (pieceDownRightBoxEmpty.CompareTag("Player1"))
                {
                    if (pieceDownRightBoxEmpty.GetComponent<BishopPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceDownRightBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceDownRightBoxEmpty.GetComponent<KingPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer1 = true;
                    }
                    else if (pieceDownRightBoxEmpty.GetComponent<PawnPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer1 = true;
                    }

                    pieceDownRightBoxEmpty = null;
                }
                else if (pieceDownRightBoxEmpty.CompareTag("Player2"))
                {
                    if (pieceDownRightBoxEmpty.GetComponent<BishopPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceDownRightBoxEmpty.GetComponent<QueenPiece>() != null)
                    {
                        isThreatenedPlayer2 = true;
                    }
                    else if (pieceDownRightBoxEmpty.GetComponent<KingPiece>() != null && i == 0)
                    {
                        isThreatenedPlayer2 = true;
                    }

                    pieceDownRightBoxEmpty = null;
                }
            }

            i++;
        }

        KnightThreat(this.gameObject);

    }
    private void GetGameObjectAround(GameObject piece)
    {
        GetGameObjectUp(piece);
        GetGameObjectDown(piece);
        GetGameObjectLeft(piece);
        GetGameObjectRight(piece);
        GetGameObjectUpLeft(piece);
        GetGameObjectUpRight(piece);
        GetGameObjectDownLeft(piece);
        GetGameObjectDownRight(piece);
    }
    private void GetGameObjectUp(GameObject piece)
    {
        pieceUpBoxEmpty = null;

        Ray rayUp = new Ray(piece.transform.position, piece.transform.forward);
        RaycastHit hitUp;
        if (Physics.Raycast(rayUp, out hitUp, 10f))
        {
            pieceUpBoxEmpty = hitUp.transform.gameObject;
        }
        else
        {
            pieceUpBoxEmpty = null;
        }
    }
    private void GetGameObjectDown(GameObject piece)
    {
        pieceDownBoxEmpty = null;

        Ray rayDown = new Ray(piece.transform.position, piece.transform.forward*-1);
        RaycastHit hitDown;
        if (Physics.Raycast(rayDown, out hitDown, 10f))
        {
            pieceDownBoxEmpty = hitDown.transform.gameObject;
        }
        else
        {
            pieceDownBoxEmpty = null;
        }
    }
    private void GetGameObjectLeft(GameObject piece)
    {
        pieceLeftBoxEmpty = null;

        Ray rayLeft = new Ray(piece.transform.position, piece.transform.right * -1);
        RaycastHit hitLeft;
        if (Physics.Raycast(rayLeft, out hitLeft, 10f))
        {
            pieceLeftBoxEmpty = hitLeft.transform.gameObject;
        }
        else
        {
            pieceLeftBoxEmpty = null;
        }
    }
    private void GetGameObjectRight(GameObject piece)
    {
        pieceRightBoxEmpty = null;

        Ray rayRight = new Ray(piece.transform.position, piece.transform.right);
        RaycastHit hitRight;
        if (Physics.Raycast(rayRight, out hitRight, 10f))
        {
            pieceRightBoxEmpty = hitRight.transform.gameObject;
        }
        else
        {
            pieceRightBoxEmpty = null;
        }
    }
    private void GetGameObjectUpLeft(GameObject piece)
    {
        pieceUpLeftBoxEmpty = null;

        Ray rayUp = new Ray(piece.transform.position, piece.transform.forward);
        RaycastHit hitUp;
        if (Physics.Raycast(rayUp, out hitUp, 10f))
        {
            Ray rayLeft = new Ray(hitUp.transform.position, hitUp.transform.right * -1);
            RaycastHit hitLeft;
            if (Physics.Raycast(rayLeft, out hitLeft, 10f))
            {
                pieceUpLeftBoxEmpty = hitLeft.transform.gameObject;
            }
            else
            {
                pieceUpLeftBoxEmpty = null;
            }
        }
        else
        {
            pieceUpLeftBoxEmpty = null;
        }
    }
    private void GetGameObjectUpRight(GameObject piece)
    {
        pieceUpRightBoxEmpty = null;

        Ray rayUp = new Ray(piece.transform.position, piece.transform.forward);
        RaycastHit hitUp;
        if (Physics.Raycast(rayUp, out hitUp, 10f))
        {
            Ray rayRight = new Ray(hitUp.transform.position, hitUp.transform.right);
            RaycastHit hitRight;
            if (Physics.Raycast(rayRight, out hitRight, 10f))
            {
                pieceUpRightBoxEmpty = hitRight.transform.gameObject;
            }
            else
            {
                pieceUpRightBoxEmpty = null;
            }
        }
        else
        {
            pieceUpRightBoxEmpty = null;
        }
    }
    private void GetGameObjectDownLeft(GameObject piece)
    {
        pieceDownLeftBoxEmpty = null;

        Ray rayDown = new Ray(piece.transform.position, piece.transform.forward * -1);
        RaycastHit hitDown;
        if (Physics.Raycast(rayDown, out hitDown, 10f))
        {
            Ray rayLeft = new Ray(hitDown.transform.position, hitDown.transform.right * -1);
            RaycastHit hitLeft;
            if (Physics.Raycast(rayLeft, out hitLeft, 10f))
            {
                pieceDownLeftBoxEmpty = hitLeft.transform.gameObject;
            }
            else
            {
                pieceDownLeftBoxEmpty = null;
            }
        }
        else
        {
            pieceDownLeftBoxEmpty = null;
        }
    }
    private void GetGameObjectDownRight(GameObject piece)
    {
        pieceDownRightBoxEmpty = null;

        Ray rayDown = new Ray(piece.transform.position, piece.transform.forward * -1);
        RaycastHit hitDown;
        if (Physics.Raycast(rayDown, out hitDown, 10f))
        {
            Ray rayRight = new Ray(hitDown.transform.position, hitDown.transform.right);
            RaycastHit hitRight;
            if (Physics.Raycast(rayRight, out hitRight, 10f))
            {
                pieceDownRightBoxEmpty = hitRight.transform.gameObject;
            }
            else
            {
                pieceDownRightBoxEmpty = null;
            }
        }
        else
        {
            pieceDownRightBoxEmpty = null;
        }
    }
    private void KnightThreat(GameObject piece)
    {
        KnightThreatCASE1(piece);
        KnightThreatCASE2(piece);
        KnightThreatCASE3(piece);
        KnightThreatCASE4(piece);
        KnightThreatCASE5(piece);
        KnightThreatCASE6(piece);
        KnightThreatCASE7(piece);
        KnightThreatCASE8(piece);
    }
    private void KnightThreatCASE1(GameObject piece)
    {
        pieceUpBoxEmpty = null;
        pieceDownBoxEmpty = null;
        pieceLeftBoxEmpty = null;
        pieceRightBoxEmpty = null;

        GetGameObjectUp(piece);
        if(pieceUpBoxEmpty != null)
        {
            GetGameObjectUp(pieceUpBoxEmpty);
            if(pieceUpBoxEmpty != null)
            {
                GetGameObjectLeft(pieceUpBoxEmpty);
                if(pieceLeftBoxEmpty != null)
                {
                    if (pieceLeftBoxEmpty.GetComponent<KnightPiece>() != null)
                    {
                        if (pieceLeftBoxEmpty.CompareTag("Player1"))
                        {
                            isThreatenedPlayer1 = true;
                        }
                        else if (pieceLeftBoxEmpty.CompareTag("Player2"))
                        {
                            isThreatenedPlayer2 = true;
                        }
                    }
                }
            }
        }
    }
    private void KnightThreatCASE2(GameObject piece)
    {
        pieceUpBoxEmpty = null;
        pieceDownBoxEmpty = null;
        pieceLeftBoxEmpty = null;
        pieceRightBoxEmpty = null;

        GetGameObjectUp(piece);
        if (pieceUpBoxEmpty != null)
        {
            GetGameObjectUp(pieceUpBoxEmpty);
            if (pieceUpBoxEmpty != null)
            {
                GetGameObjectRight(pieceUpBoxEmpty);
                if (pieceRightBoxEmpty != null)
                {
                    if (pieceRightBoxEmpty.GetComponent<KnightPiece>() != null)
                    {
                        if (pieceRightBoxEmpty.CompareTag("Player1"))
                        {
                            isThreatenedPlayer1 = true;
                        }
                        else if (pieceRightBoxEmpty.CompareTag("Player2"))
                        {
                            isThreatenedPlayer2 = true;
                        }
                    }
                }
            }
        }
    }
    private void KnightThreatCASE3(GameObject piece)
    {
        pieceUpBoxEmpty = null;
        pieceDownBoxEmpty = null;
        pieceLeftBoxEmpty = null;
        pieceRightBoxEmpty = null;

        GetGameObjectDown(piece);
        if (pieceDownBoxEmpty != null)
        {
            GetGameObjectDown(pieceDownBoxEmpty);
            if (pieceDownBoxEmpty != null)
            {
                GetGameObjectLeft(pieceDownBoxEmpty);
                if (pieceLeftBoxEmpty != null)
                {
                    if (pieceLeftBoxEmpty.GetComponent<KnightPiece>() != null)
                    {
                        if (pieceLeftBoxEmpty.CompareTag("Player1"))
                        {
                            isThreatenedPlayer1 = true;
                        }
                        else if (pieceLeftBoxEmpty.CompareTag("Player2"))
                        {
                            isThreatenedPlayer2 = true;
                        }
                    }
                }
            }
        }
    }
    private void KnightThreatCASE4(GameObject piece)
    {
        pieceUpBoxEmpty = null;
        pieceDownBoxEmpty = null;
        pieceLeftBoxEmpty = null;
        pieceRightBoxEmpty = null;

        GetGameObjectDown(piece);
        if (pieceDownBoxEmpty != null)
        {
            GetGameObjectDown(pieceDownBoxEmpty);
            if (pieceDownBoxEmpty != null)
            {
                GetGameObjectRight(pieceDownBoxEmpty);
                if (pieceRightBoxEmpty != null)
                {
                    if (pieceRightBoxEmpty.GetComponent<KnightPiece>() != null)
                    {
                        if (pieceRightBoxEmpty.CompareTag("Player1"))
                        {
                            isThreatenedPlayer1 = true;
                        }
                        else if (pieceRightBoxEmpty.CompareTag("Player2"))
                        {
                            isThreatenedPlayer2 = true;
                        }
                    }
                }
            }
        }
    }
    private void KnightThreatCASE5(GameObject piece)
    {
        pieceUpBoxEmpty = null;
        pieceDownBoxEmpty = null;
        pieceLeftBoxEmpty = null;
        pieceRightBoxEmpty = null;

        GetGameObjectLeft(piece);
        if (pieceLeftBoxEmpty != null)
        {
            GetGameObjectLeft(pieceLeftBoxEmpty);
            if (pieceLeftBoxEmpty != null)
            {
                GetGameObjectUp(pieceLeftBoxEmpty);
                if (pieceUpBoxEmpty != null)
                {
                    if (pieceUpBoxEmpty.GetComponent<KnightPiece>() != null)
                    {
                        if (pieceUpBoxEmpty.CompareTag("Player1"))
                        {
                            isThreatenedPlayer1 = true;
                        }
                        else if (pieceUpBoxEmpty.CompareTag("Player2"))
                        {
                            isThreatenedPlayer2 = true;
                        }
                    }
                }
            }
        }
    }
    private void KnightThreatCASE6(GameObject piece)
    {
        pieceUpBoxEmpty = null;
        pieceDownBoxEmpty = null;
        pieceLeftBoxEmpty = null;
        pieceRightBoxEmpty = null;

        GetGameObjectLeft(piece);
        if (pieceLeftBoxEmpty != null)
        {
            GetGameObjectLeft(pieceLeftBoxEmpty);
            if (pieceLeftBoxEmpty != null)
            {
                GetGameObjectDown(pieceLeftBoxEmpty);
                if (pieceDownBoxEmpty != null)
                {
                    if (pieceDownBoxEmpty.GetComponent<KnightPiece>() != null)
                    {
                        if (pieceDownBoxEmpty.CompareTag("Player1"))
                        {
                            isThreatenedPlayer1 = true;
                        }
                        else if (pieceDownBoxEmpty.CompareTag("Player2"))
                        {
                            isThreatenedPlayer2 = true;
                        }
                    }
                }
            }
        }
    }
    private void KnightThreatCASE7(GameObject piece)
    {
        pieceUpBoxEmpty = null;
        pieceDownBoxEmpty = null;
        pieceLeftBoxEmpty = null;
        pieceRightBoxEmpty = null;

        GetGameObjectRight(piece);
        if (pieceRightBoxEmpty != null)
        {
            GetGameObjectRight(pieceRightBoxEmpty);
            if (pieceRightBoxEmpty != null)
            {
                GetGameObjectUp(pieceRightBoxEmpty);
                if (pieceUpBoxEmpty != null)
                {
                    if (pieceUpBoxEmpty.GetComponent<KnightPiece>() != null)
                    {
                        if (pieceUpBoxEmpty.CompareTag("Player1"))
                        {
                            isThreatenedPlayer1 = true;
                        }
                        else if (pieceUpBoxEmpty.CompareTag("Player2"))
                        {
                            isThreatenedPlayer2 = true;
                        }
                    }
                }
            }
        }
    }
    private void KnightThreatCASE8(GameObject piece)
    {
        pieceUpBoxEmpty = null;
        pieceDownBoxEmpty = null;
        pieceLeftBoxEmpty = null;
        pieceRightBoxEmpty = null;

        GetGameObjectRight(piece);
        if (pieceRightBoxEmpty != null)
        {
            GetGameObjectRight(pieceRightBoxEmpty);
            if (pieceRightBoxEmpty != null)
            {
                GetGameObjectDown(pieceRightBoxEmpty);
                if (pieceDownBoxEmpty != null)
                {
                    if (pieceDownBoxEmpty.GetComponent<KnightPiece>() != null)
                    {
                        if (pieceDownBoxEmpty.CompareTag("Player1"))
                        {
                            isThreatenedPlayer1 = true;
                        }
                        else if (pieceDownBoxEmpty.CompareTag("Player2"))
                        {
                            isThreatenedPlayer2 = true;
                        }
                    }
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        if (!gameController.inMenuPause)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Chessboard");
        }
    }
    private void OnMouseExit()
    {
            this.gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
