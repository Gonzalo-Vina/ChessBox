using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private Animator animatorCamera;
    [SerializeField] private Animator animatorCanvas;

    private string playerTag;
    private string enemyTag;
    private int directionPiece;
    private float velMovement;

    public bool myTurn;
    private bool ICanMove;
    private bool ICanSelect;
    private bool ICanAttack;
    private bool ICanCreateBoxEmpty;
    private bool crowningPawn;

    [SerializeField] private AnimationCurve animationCurveMove;

    [SerializeField] private GameObject UI_CrowningPawn;
    [SerializeField] private GameObject UI_BlockPanel;

    private GameObject pieceKing;
    public bool inCheck;
    public bool inCheckMate;
    private GameObject enroquePieceKing;
    private GameObject enroqueDestinyKing;
    private GameObject enroquePieceRook;
    private GameObject enroqueDestinyRook;

    [SerializeField] private Chessboard chessboard;
    [SerializeField] public GameObject pieceSelected;
    [SerializeField] private GameObject boxBoardPosition;
    [SerializeField] private GameObject pieceEnemy;
    [SerializeField] private Vector3 positionOriginalEnemy;
    [SerializeField] private GameObject cameraGO;
    [SerializeField] private Vector3 positionOriginalCamera;
    [SerializeField] private Quaternion rotationOriginalCamera;
    [SerializeField] private Vector3 rotationTempCamera;
    [SerializeField] private bool havePiece;

    private Vector3 positionSave1;
    private Vector3 positionSave2;
    private Vector3 positionSave3;
    private Vector3 positionSave4;
    private Vector3 positionTemp1;
    private Vector3 positionTemp2;
    private Vector3 positionTemp3;
    private Vector3 positionTemp4;
    private Vector3 positionTemp5;
    private Vector3 positionTemp6;
    private Vector3 positionTemp7;
    private Vector3 positionTemp8;

    #region GameObject Around
    private GameObject pieceUp;
    private GameObject pieceDown;
    private GameObject pieceLeft;
    private GameObject pieceRight;
    private GameObject pieceUpLeft;
    private GameObject pieceUpRight;
    private GameObject pieceDownLeft;
    private GameObject pieceDownRight;
    private bool pieceBloquedUp;
    private bool pieceBloquedDown;
    private bool pieceBloquedLeft;
    private bool pieceBloquedRight;
    private bool pieceBloquedUpLeft;
    private bool pieceBloquedUpRight;
    private bool pieceBloquedDownLeft;
    private bool pieceBloquedDownRight;

    private GameObject pieceUpKing;
    private GameObject pieceDownKing;
    private GameObject pieceLeftKing;
    private GameObject pieceRightKing;
    private GameObject pieceUpLeftKing;
    private GameObject pieceUpRightKing;
    private GameObject pieceDownLeftKing;
    private GameObject pieceDownRightKing;
    #endregion

    [SerializeField] private GameObject[] boxsChessboardWhitTags;
    [SerializeField] private GameObject[] boxsEnemyWhitTags;
    private bool spawBoxsAvailable;
    [SerializeField] private GameObject lastBoxEmptyCreated;

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        positionOriginalCamera = cameraGO.transform.position;
        rotationOriginalCamera = cameraGO.transform.rotation;
        rotationTempCamera = new Vector3(0, 0, 0);

        if (this.tag == "Player1")
        {
            playerTag = "Player1";
            enemyTag = "Player2";
            directionPiece = 1;
        }
        else
        {
            playerTag = "Player2";
            enemyTag = "Player1";
            directionPiece = -1;
        }

        StartCoroutine(GetKingPieces());

        

        velMovement = 0.5f;
    }
    private void OnEnable()
    {
        myTurn = true;
        ICanMove = true;
        ICanSelect = true;
        ICanAttack = true;
        ICanCreateBoxEmpty = true;
        crowningPawn = false;
        inCheck = false;
        pieceSelected = null;
        boxBoardPosition = null;
        pieceEnemy = null;
        pieceUp = null;
        pieceDown = null;
        pieceLeft = null;
        pieceRight = null;
        pieceUpLeft = null;
        pieceUpRight = null;
        pieceDownLeft = null;
        pieceDownRight = null;
        havePiece = false;
        pieceBloquedUp = false;
        pieceBloquedDown = false;
        pieceBloquedLeft = false;
        pieceBloquedRight = false;
        pieceBloquedUpLeft = false;
        pieceBloquedUpRight = false;
        pieceBloquedDownLeft = false;
        pieceBloquedDownRight = false;
        chessboard = GameObject.FindObjectOfType<Chessboard>();

        if(pieceKing != null)
        {
            CheckThreatKing();

            if (inCheck)
            {
                GameObject redGreen = LigthPool.Instance.RequestLightRed();
                redGreen.transform.position = pieceKing.transform.position;
            }
        }
    }
    private void Update()
    {
        if (ICanSelect && gameController.inMenuPause == false) { SelectPieces(); }
        if (havePiece && spawBoxsAvailable) { CheckBoxAvailable(); }
        if (havePiece && boxBoardPosition != null) { LogicMovementPieces(); }
        if (havePiece && pieceEnemy != null) { LogicAttackPieces(); }
    }

    private void SelectPieces()
    {
        if (Input.GetMouseButtonDown(0))
        {
            boxBoardPosition = null;
            pieceEnemy = null;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == playerTag)
                {
                    if (pieceSelected == null)
                    {
                        pieceSelected = hit.transform.gameObject;
                        TakeLeavePiece();
                        UpDownPiece();
                        spawBoxsAvailable = true;
                    }
                    else
                    {
                        if(hit.transform.gameObject.transform.position == pieceSelected.transform.position)
                        {
                            TakeLeavePiece();
                            UpDownPiece();
                            pieceSelected = null;
                            LigthPool.Instance.DisableList();
                        }
                        else
                        {
                            LigthPool.Instance.DisableList();
                            TakeLeavePiece();
                            UpDownPiece();
                            pieceSelected = hit.transform.gameObject;
                            TakeLeavePiece();
                            UpDownPiece();
                            spawBoxsAvailable = true;
                        }
                    }
                    
                }
                else if (hit.transform.tag == "Chessboard" && pieceSelected != null && pieceEnemy == null)
                {
                    boxBoardPosition = hit.transform.gameObject;
                }
                else if (hit.transform.tag == enemyTag && pieceSelected != null && boxBoardPosition == null)
                {
                    pieceEnemy = hit.transform.gameObject;
                    positionOriginalEnemy = pieceEnemy.transform.position;
                }
            }
        }
    }
    private void CheckBoxAvailable()
    {
        LigthPool.Instance.DisableList();

        UpdateBoxEmpty();

        boxsEnemyWhitTags = GameObject.FindGameObjectsWithTag(enemyTag);

        //LOGIC OF PAWN
        if (pieceSelected.GetComponent<PawnPiece>() == true)
        {
            GetGameObjectUp(pieceSelected);
            CheckPieceBloquedUp();

            foreach (GameObject box in boxsChessboardWhitTags)
            {
                if (pieceSelected.GetComponent<PawnPiece>().firstMovement == true)
                {
                    if (box.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 2 * directionPiece) && !pieceBloquedUp)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = box.transform.position;
                    }
                    else if (box.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 1 * directionPiece))
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = box.transform.position;
                    }
                }
                else
                {
                    if (box.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 1 * directionPiece))
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = box.transform.position;
                    }
                }
            }

            foreach (GameObject box in boxsEnemyWhitTags)
            {
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1 * directionPiece, 0, pieceSelected.transform.position.z + 1 * directionPiece))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1 * directionPiece, 0, pieceSelected.transform.position.z + 1 * directionPiece))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
            }

            spawBoxsAvailable = false;
        }

        //LOGIC OF ROOK
        else if (pieceSelected.GetComponent<RookPiece>() == true)
        {
            GetGameObjectAround(pieceSelected);
            CheckPieceBloquedAround();

            //UP
            for (int z = 0; z < 8; z++)
            {
                if (pieceUp != null)
                {
                    GameObject boxUp = new GameObject();
                    boxUp.transform.position = new Vector3(pieceUp.transform.position.x, 0, pieceUp.transform.position.z);

                    if (boxUp.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + z * directionPiece) && !pieceBloquedUp)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxUp.transform.position;
                        GetGameObjectUp(boxUp);
                        CheckPieceBloquedUp();
                    }
                    else if (boxUp.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + z * directionPiece) && pieceBloquedUp 
                        && pieceUp.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxUp.transform.position;
                    }

                    Destroy(boxUp);
                }
            }


            //Down
            for (int z = 0; z < 8; z++)
            {
                if (pieceDown != null)
                {
                    GameObject boxDown = new GameObject();
                    boxDown.transform.position = new Vector3(pieceDown.transform.position.x, 0, pieceDown.transform.position.z);

                    if (boxDown.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - z * directionPiece) && !pieceBloquedDown)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxDown.transform.position;
                        GetGameObjectDown(boxDown);
                        CheckPieceBloquedDown();
                    }
                    else if (boxDown.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - z * directionPiece) && pieceBloquedDown
                        && pieceDown.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxDown.transform.position;
                    }

                    Destroy(boxDown);
                }

            }

            //Left
            for (int x = 0; x < 8; x++)
            {
                if(pieceLeft != null)
                {
                    GameObject boxLeft = new GameObject();
                    boxLeft.transform.position = new Vector3(pieceLeft.transform.position.x, 0, pieceLeft.transform.position.z);

                    if(boxLeft.transform.position == new Vector3(pieceSelected.transform.position.x - x * directionPiece, 0, pieceSelected.transform.position.z) && !pieceBloquedLeft)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxLeft.transform.position;
                        GetGameObjectLeft(boxLeft);
                        CheckPieceBloquedLeft();
                    }
                    else if (boxLeft.transform.position == new Vector3(pieceSelected.transform.position.x - x * directionPiece, 0, pieceSelected.transform.position.z) && pieceBloquedLeft
                        && pieceLeft.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxLeft.transform.position;
                    }

                    Destroy(boxLeft);
                }
            }

            //Right
            for (int x = 0; x < 8; x++)
            {
                if (pieceRight != null)
                {
                    GameObject boxRight = new GameObject();
                    boxRight.transform.position = new Vector3(pieceRight.transform.position.x, 0, pieceRight.transform.position.z);

                    if (boxRight.transform.position == new Vector3(pieceSelected.transform.position.x + x * directionPiece, 0, pieceSelected.transform.position.z) && !pieceBloquedRight)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxRight.transform.position;
                        GetGameObjectRight(boxRight);
                        CheckPieceBloquedRight();
                    }
                    else if (boxRight.transform.position == new Vector3(pieceSelected.transform.position.x + x * directionPiece, 0, pieceSelected.transform.position.z) && pieceBloquedRight
                        && pieceRight.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxRight.transform.position;
                    }

                    Destroy(boxRight);
                }
            }

            spawBoxsAvailable = false;
        }

        //LOGIC OF KNIGHT
        else if(pieceSelected.GetComponent<KnightPiece>() == true)
        {
            GetGameObjectUp(pieceSelected);
            CheckPieceBloquedUp();

            foreach (GameObject box in boxsChessboardWhitTags)
            {
                //CASE 1
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z + 2))
                {
                    GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                    lightGreen.transform.position = box.transform.position;
                }
                //CASE 2
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 2, 0, pieceSelected.transform.position.z + 1))
                {
                    GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                    lightGreen.transform.position = box.transform.position;
                }
                //CASE 3
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 2, 0, pieceSelected.transform.position.z -1))
                {
                    GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                    lightGreen.transform.position = box.transform.position;
                }
                //CASE 4
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z - 2))
                {
                    GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                    lightGreen.transform.position = box.transform.position;
                }
                //CASE 5
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z - 2))
                {
                    GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                    lightGreen.transform.position = box.transform.position;
                }
                //CASE 6
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 2, 0, pieceSelected.transform.position.z - 1))
                {
                    GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                    lightGreen.transform.position = box.transform.position;
                }
                //CASE 7
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 2, 0, pieceSelected.transform.position.z + 1))
                {
                    GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                    lightGreen.transform.position = box.transform.position;
                }
                //CASE 8
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z + 2))
                {
                    GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                    lightGreen.transform.position = box.transform.position;
                }
            }

            foreach (GameObject box in boxsEnemyWhitTags)
            {
                //CASE 1
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z + 2))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
                //CASE 2
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 2, 0, pieceSelected.transform.position.z + 1))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
                //CASE 3
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 2, 0, pieceSelected.transform.position.z - 1))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
                //CASE 4
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z - 2))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
                //CASE 5
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z - 2))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
                //CASE 6
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 2, 0, pieceSelected.transform.position.z - 1))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
                //CASE 7
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 2, 0, pieceSelected.transform.position.z + 1))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
                //CASE 8
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z + 2))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
            }

            spawBoxsAvailable = false;
            
        }

        //LOGIC OF BISHOP
        else if (pieceSelected.GetComponent<BishopPiece>() == true)
        {
            GetGameObjectAround(pieceSelected);
            CheckPieceBloquedAround();

            //UP-LEFT
            for (int i = 0; i < 8; i++)
            {
                if (pieceUpLeft != null)
                {
                    GameObject boxUpLeft = new GameObject();
                    boxUpLeft.transform.position = new Vector3(pieceUpLeft.transform.position.x, 0, pieceUpLeft.transform.position.z);

                    if (boxUpLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && !pieceBloquedUpLeft)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxUpLeft.transform.position;
                        GetGameObjectUpLeft(boxUpLeft);
                        CheckPieceBloquedUpLeft();
                    }
                    else if (boxUpLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && pieceBloquedUpLeft
                        && pieceUpLeft.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxUpLeft.transform.position;
                    }

                    Destroy(boxUpLeft);
                }
            }

            //UP-RIGHT
            for (int i = 0; i < 8; i++)
            {
                if (pieceUpRight != null)
                {
                    GameObject boxUpRight = new GameObject();
                    boxUpRight.transform.position = new Vector3(pieceUpRight.transform.position.x, 0, pieceUpRight.transform.position.z);

                    if (boxUpRight.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && !pieceBloquedUpRight)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxUpRight.transform.position;
                        GetGameObjectUpRight(boxUpRight);
                        CheckPieceBloquedUpRight();
                    }
                    else if (boxUpRight.transform.position == new Vector3(pieceSelected.transform.position.x + i *directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && pieceBloquedUpRight
                        && pieceUpRight.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxUpRight.transform.position;
                    }

                    Destroy(boxUpRight);
                }
            }

            //DOWN-LEFT
            for (int i = 0; i < 8; i++)
            {
                if (pieceDownLeft != null)
                {
                    GameObject boxDownLeft = new GameObject();
                    boxDownLeft.transform.position = new Vector3(pieceDownLeft.transform.position.x, 0, pieceDownLeft.transform.position.z);

                    if (boxDownLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && !pieceBloquedDownLeft)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxDownLeft.transform.position;
                        GetGameObjectDownLeft(boxDownLeft);
                        CheckPieceBloquedDownLeft();
                    }
                    else if (boxDownLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && pieceBloquedDownLeft
                        && pieceDownLeft.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxDownLeft.transform.position;
                    }

                    Destroy(boxDownLeft);
                }
            }

            //DOWN-RIGHT
            for (int i = 0; i < 8; i++)
            {
                if (pieceDownRight != null)
                {
                    GameObject boxDownRight = new GameObject();
                    boxDownRight.transform.position = new Vector3(pieceDownRight.transform.position.x, 0, pieceDownRight.transform.position.z);

                    if (boxDownRight.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && !pieceBloquedDownRight)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxDownRight.transform.position;
                        GetGameObjectDownRight(boxDownRight);
                        CheckPieceBloquedDownRight();
                    }
                    else if (boxDownRight.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && pieceBloquedDownRight
                        && pieceDownRight.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxDownRight.transform.position;
                    }

                    Destroy(boxDownRight);
                }
            }

            spawBoxsAvailable = false;
        }

        //LOGIC OF QUEEN
        else if (pieceSelected.GetComponent<QueenPiece>() == true)
        {
            GetGameObjectAround(pieceSelected);
            CheckPieceBloquedAround();

            //UP
            for (int z = 0; z < 8; z++)
            {
                if (pieceUp != null)
                {
                    GameObject boxUp = new GameObject();
                    boxUp.transform.position = new Vector3(pieceUp.transform.position.x, 0, pieceUp.transform.position.z);

                    if (boxUp.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + z * directionPiece) && !pieceBloquedUp)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxUp.transform.position;
                        GetGameObjectUp(boxUp);
                        CheckPieceBloquedUp();
                    }
                    else if (boxUp.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + z * directionPiece) && pieceBloquedUp
                        && pieceUp.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxUp.transform.position;
                    }

                    Destroy(boxUp);
                }
            }

            //Down
            for (int z = 0; z < 8; z++)
            {
                if (pieceDown != null)
                {
                    GameObject boxDown = new GameObject();
                    boxDown.transform.position = new Vector3(pieceDown.transform.position.x, 0, pieceDown.transform.position.z);

                    if (boxDown.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - z * directionPiece) && !pieceBloquedDown)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxDown.transform.position;
                        GetGameObjectDown(boxDown);
                        CheckPieceBloquedDown();
                    }
                    else if (boxDown.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - z * directionPiece) && pieceBloquedDown
                        && pieceDown.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxDown.transform.position;
                    }

                    Destroy(boxDown);
                }

            }

            //Left
            for (int x = 0; x < 8; x++)
            {
                if (pieceLeft != null)
                {
                    GameObject boxLeft = new GameObject();
                    boxLeft.transform.position = new Vector3(pieceLeft.transform.position.x, 0, pieceLeft.transform.position.z);

                    if (boxLeft.transform.position == new Vector3(pieceSelected.transform.position.x - x * directionPiece, 0, pieceSelected.transform.position.z) && !pieceBloquedLeft)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxLeft.transform.position;
                        GetGameObjectLeft(boxLeft);
                        CheckPieceBloquedLeft();
                    }
                    else if (boxLeft.transform.position == new Vector3(pieceSelected.transform.position.x - x * directionPiece, 0, pieceSelected.transform.position.z) && pieceBloquedLeft
                        && pieceLeft.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxLeft.transform.position;
                    }

                    Destroy(boxLeft);
                }
            }

            //Right
            for (int x = 0; x < 8; x++)
            {
                if (pieceRight != null)
                {
                    GameObject boxRight = new GameObject();
                    boxRight.transform.position = new Vector3(pieceRight.transform.position.x, 0, pieceRight.transform.position.z);

                    if (boxRight.transform.position == new Vector3(pieceSelected.transform.position.x + x * directionPiece, 0, pieceSelected.transform.position.z) && !pieceBloquedRight)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxRight.transform.position;
                        GetGameObjectRight(boxRight);
                        CheckPieceBloquedRight();
                    }
                    else if (boxRight.transform.position == new Vector3(pieceSelected.transform.position.x + x * directionPiece, 0, pieceSelected.transform.position.z) && pieceBloquedRight
                        && pieceRight.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxRight.transform.position;
                    }

                    Destroy(boxRight);
                }
            }

            //UP-LEFT
            for (int i = 0; i < 8; i++)
            {
                if (pieceUpLeft != null)
                {
                    GameObject boxUpLeft = new GameObject();
                    boxUpLeft.transform.position = new Vector3(pieceUpLeft.transform.position.x, 0, pieceUpLeft.transform.position.z);

                    if (boxUpLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && !pieceBloquedUpLeft)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxUpLeft.transform.position;
                        GetGameObjectUpLeft(boxUpLeft);
                        CheckPieceBloquedUpLeft();
                    }
                    else if (boxUpLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && pieceBloquedUpLeft
                        && pieceUpLeft.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxUpLeft.transform.position;
                    }

                    Destroy(boxUpLeft);
                }
            }

            //UP-RIGHT
            for (int i = 0; i < 8; i++)
            {
                if (pieceUpRight != null)
                {
                    GameObject boxUpRight = new GameObject();
                    boxUpRight.transform.position = new Vector3(pieceUpRight.transform.position.x, 0, pieceUpRight.transform.position.z);

                    if (boxUpRight.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && !pieceBloquedUpRight)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxUpRight.transform.position;
                        GetGameObjectUpRight(boxUpRight);
                        CheckPieceBloquedUpRight();
                    }
                    else if (boxUpRight.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && pieceBloquedUpRight
                        && pieceUpRight.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxUpRight.transform.position;
                    }

                    Destroy(boxUpRight);
                }
            }

            //DOWN-LEFT
            for (int i = 0; i < 8; i++)
            {
                if (pieceDownLeft != null)
                {
                    GameObject boxDownLeft = new GameObject();
                    boxDownLeft.transform.position = new Vector3(pieceDownLeft.transform.position.x, 0, pieceDownLeft.transform.position.z);

                    if (boxDownLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && !pieceBloquedDownLeft)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxDownLeft.transform.position;
                        GetGameObjectDownLeft(boxDownLeft);
                        CheckPieceBloquedDownLeft();
                    }
                    else if (boxDownLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && pieceBloquedDownLeft
                        && pieceDownLeft.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxDownLeft.transform.position;
                    }

                    Destroy(boxDownLeft);
                }
            }

            //DOWN-RIGHT
            for (int i = 0; i < 8; i++)
            {
                if (pieceDownRight != null)
                {
                    GameObject boxDownRight = new GameObject();
                    boxDownRight.transform.position = new Vector3(pieceDownRight.transform.position.x, 0, pieceDownRight.transform.position.z);

                    if (boxDownRight.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && !pieceBloquedDownRight)
                    {
                        GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                        lightGreen.transform.position = boxDownRight.transform.position;
                        GetGameObjectDownRight(boxDownRight);
                        CheckPieceBloquedDownRight();
                    }
                    else if (boxDownRight.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && pieceBloquedDownRight
                        && pieceDownRight.CompareTag(enemyTag))
                    {
                        GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                        lightYellow.transform.position = boxDownRight.transform.position;
                    }

                    Destroy(boxDownRight);
                }
            }

            spawBoxsAvailable = false;
        }

        //LOGIC OF KING
        else if (pieceSelected.GetComponent<KingPiece>() == true)
        {
            GetGameObjectAround(pieceSelected);
            CheckPieceBloquedAround();

            foreach (GameObject box in boxsChessboardWhitTags)
            {
                if (pieceSelected.GetComponent<KingPiece>().firstMovement == true)
                {
                    if (this.CompareTag("Player1"))
                    {
                        //ENROQUE-LEFT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GetGameObjectLeft(box);
                            if (pieceLeft.CompareTag("Chessboard") && pieceLeft.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                            {
                                GetGameObjectLeft(pieceLeft);
                                if (pieceLeft.CompareTag("Chessboard") && pieceLeft.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                                {
                                    GetGameObjectLeft(pieceLeft);
                                    if (pieceLeft.GetComponent<RookPiece>() == true && pieceLeft.GetComponent<RookPiece>().firstMovement == true)
                                    {
                                        GameObject lightGreen2 = LigthPool.Instance.RequestLightGreen();
                                        lightGreen2.transform.position = new Vector3(box.transform.position.x - 1, 0, box.transform.position.z);
                                    }
                                }
                            }
                        }
                        //ENROQUE-RIGHT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GetGameObjectRight(box);
                            if(pieceRight.CompareTag("Chessboard") && pieceRight.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                            {
                                GetGameObjectRight(pieceRight);
                                if (pieceRight.GetComponent<RookPiece>() == true && pieceRight.GetComponent<RookPiece>().firstMovement == true)
                                {
                                    GameObject lightGreen2 = LigthPool.Instance.RequestLightGreen();
                                    lightGreen2.transform.position = new Vector3(box.transform.position.x + 1, 0, box.transform.position.z);
                                }
                            }
                        }
                        //UP
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //DOWN
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //LEFT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1 * directionPiece, 0, pieceSelected.transform.position.z) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;

                        }
                        //RIGHT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1 * directionPiece, 0, pieceSelected.transform.position.z) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //UP-LEFT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z + 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //UP-RIGHT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z + 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //DOWN-LEFT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z - 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //DOWN-RIGHT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z - 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                    }
                    else if(this.CompareTag("Player2"))
                    {
                        //ENROQUE-LEFT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1 * directionPiece, 0, pieceSelected.transform.position.z) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GetGameObjectRight(box);
                            if (pieceRight.CompareTag("Chessboard") && pieceRight.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                            {
                                GetGameObjectRight(pieceRight);
                                if (pieceRight.CompareTag("Chessboard") && pieceRight.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                                {
                                    GetGameObjectRight(pieceRight);
                                    if (pieceRight.GetComponent<RookPiece>() == true && pieceRight.GetComponent<RookPiece>().firstMovement == true)
                                    {
                                        GameObject lightGreen2 = LigthPool.Instance.RequestLightGreen();
                                        lightGreen2.transform.position = new Vector3(box.transform.position.x - 1, 0, box.transform.position.z);
                                    }
                                }
                            }
                        }
                        //ENROQUE-RIGHT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1 * directionPiece, 0, pieceSelected.transform.position.z) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GetGameObjectLeft(box);
                            if (pieceLeft.CompareTag("Chessboard") && pieceLeft.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                            {
                                GetGameObjectLeft(pieceLeft);
                                if (pieceLeft.GetComponent<RookPiece>() == true && pieceLeft.GetComponent<RookPiece>().firstMovement == true)
                                {
                                    GameObject lightGreen2 = LigthPool.Instance.RequestLightGreen();
                                    lightGreen2.transform.position = new Vector3(box.transform.position.x + 1, 0, box.transform.position.z);
                                }
                            }
                        }
                        //UP
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //DOWN
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //LEFT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1 * directionPiece, 0, pieceSelected.transform.position.z) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;

                        }
                        //RIGHT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1 * directionPiece, 0, pieceSelected.transform.position.z) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //UP-LEFT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z + 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //UP-RIGHT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z + 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //DOWN-LEFT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z - 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //DOWN-RIGHT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z - 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                    }
                }
                else
                {
                    if (this.CompareTag("Player1"))
                    {
                        //UP
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //DOWN
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //LEFT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1 * directionPiece, 0, pieceSelected.transform.position.z) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;

                        }
                        //RIGHT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1 * directionPiece, 0, pieceSelected.transform.position.z) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //UP-LEFT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z + 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //UP-RIGHT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z + 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //DOWN-LEFT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z - 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //DOWN-RIGHT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z - 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                    }
                    else if (this.CompareTag("Player2"))
                    {
                        //UP
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //DOWN
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //LEFT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1 * directionPiece, 0, pieceSelected.transform.position.z) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;

                        }
                        //RIGHT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1 * directionPiece, 0, pieceSelected.transform.position.z) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //UP-LEFT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z + 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //UP-RIGHT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z + 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //DOWN-LEFT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z - 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                        //DOWN-RIGHT
                        if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z - 1) &&
                            box.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            GameObject lightGreen = LigthPool.Instance.RequestLightGreen();
                            lightGreen.transform.position = box.transform.position;
                        }
                    }
                }
            }

            foreach (GameObject box in boxsEnemyWhitTags)
            {
                //UP
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 1))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
                //DOWN
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - 1))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
                //LEFT
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
                //RIGHT
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
                //UP-LEFT
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z + 1))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
                //UP-RIGHT
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z + 1))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
                //DOWN-LEFT
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z - 1))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
                //DOWN-RIGHT
                if (box.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z - 1))
                {
                    GameObject lightYellow = LigthPool.Instance.RequestLightYellow();
                    lightYellow.transform.position = box.transform.position;
                }
            }

            spawBoxsAvailable = false;

        }
    }

    //MOVE
    private void LogicMovementPieces()
    {
        //LOGIC OF PAWN
        if (pieceSelected.GetComponent<PawnPiece>() == true)
        {
            if (pieceSelected.GetComponent<PawnPiece>().firstMovement == true)
            {
                if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 2 * directionPiece) && !pieceBloquedUp)
                {
                    StartCoroutine(MovePiece());
                    
                    pieceBloquedUp = false;
                }
                else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 1 * directionPiece))
                {
                    StartCoroutine(MovePiece());

                    pieceBloquedUp = false;
                }
            }
            else
            {
                if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 1 * directionPiece))
                {
                    StartCoroutine(MovePiece());

                    pieceBloquedUp = false;

                    if(playerTag == "Player1")
                    {
                        if(boxBoardPosition.transform.position.z >= 7)
                        {
                            crowningPawn = true;
                        }
                    }
                    else
                    {
                        if (boxBoardPosition.transform.position.z <= 0)
                        {
                            crowningPawn = true;
                        }
                    }
                }
            }
        }

        //LOGIC OF ROOK
        else if (pieceSelected.GetComponent<RookPiece>() == true)
        {
            GetGameObjectAround(pieceSelected);
            CheckPieceBloquedAround();

            int i = 0;

            while (i<8)
            {
                //UP
                if (pieceUp != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + i * directionPiece) && !pieceBloquedUp)
                    {
                        StartCoroutine(MovePiece());

                        pieceUp = null;
                        pieceDown = null;
                        pieceLeft = null;
                        pieceRight = null;
                        i = 8;
                    } 
                }

                //Down
                if (pieceDown != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - i * directionPiece) && !pieceBloquedDown)
                    {
                        StartCoroutine(MovePiece());

                        pieceUp = null;
                        pieceDown = null;
                        pieceLeft = null;
                        pieceRight = null;
                        i = 8;
                    }
                }

                //Left
                if (pieceLeft != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z) && !pieceBloquedLeft)
                    {
                        StartCoroutine(MovePiece());

                        pieceUp = null;
                        pieceDown = null;
                        pieceLeft = null;
                        pieceRight = null;
                        i = 8;
                    }
                }

                //Right
                if (pieceRight != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z) && !pieceBloquedRight)
                    {
                        StartCoroutine(MovePiece());

                        pieceUp = null;
                        pieceDown = null;
                        pieceLeft = null;
                        pieceRight = null;
                        i = 8;
                    }
                }

                i++;
            }
        }

        //LOGIC OF KNIGHT
        else if (pieceSelected.GetComponent<KnightPiece>() == true)
        {
            //CASE 1
            if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z + 2))
            {
                StartCoroutine(MovePiece());
            }
            //CASE 2
            else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 2, 0, pieceSelected.transform.position.z + 1))
            {
                StartCoroutine(MovePiece());
            }
            //CASE 3
            else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 2, 0, pieceSelected.transform.position.z - 1))
            {
                StartCoroutine(MovePiece());
            }
            //CASE 4
            else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z - 2))
            {
                StartCoroutine(MovePiece());
            }
            //CASE 5
            else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z - 2))
            {
                StartCoroutine(MovePiece());
            }
            //CASE 6
            else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 2, 0, pieceSelected.transform.position.z - 1))
            {
                StartCoroutine(MovePiece());
            }
            //CASE 7
            else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 2, 0, pieceSelected.transform.position.z + 1))
            {
                StartCoroutine(MovePiece());
            }
            //CASE 8
            else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z + 2))
            {
                StartCoroutine(MovePiece());
            }
        }

        //LOGIC OF BISHOP
        else if (pieceSelected.GetComponent<BishopPiece>() == true)
        {
            GetGameObjectAround(pieceSelected);
            CheckPieceBloquedAround();

            int i = 0;

            while(i<8)
            {
                //UP-LEFT
                if (pieceUpLeft != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && !pieceBloquedUpLeft)
                    {
                        StartCoroutine(MovePiece());

                        pieceUpLeft = null;
                        pieceUpRight = null;
                        pieceDownLeft = null;
                        pieceDownRight = null;
                        i = 8;
                    }
                }

                //UP-RIGHT
                if (pieceUpRight != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && !pieceBloquedUpRight)
                    {
                        StartCoroutine(MovePiece());

                        pieceUpLeft = null;
                        pieceUpRight = null;
                        pieceDownLeft = null;
                        pieceDownRight = null;
                        i = 8;
                    }
                }

                //DOWN-LEFT
                if (pieceDownLeft != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && !pieceBloquedDownLeft)
                    {
                        StartCoroutine(MovePiece());

                        pieceUpLeft = null;
                        pieceUpRight = null;
                        pieceDownLeft = null;
                        pieceDownRight = null;
                        i = 8;
                    }
                }

                //DOWN-RIGHT
                if (pieceDownRight != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && !pieceBloquedDownRight)
                    {
                        StartCoroutine(MovePiece());

                        pieceUpLeft = null;
                        pieceUpRight = null;
                        pieceDownLeft = null;
                        pieceDownRight = null;
                        i = 8;
                    }
                }

                i++;
            }
        }

        //LOGIC OF QUEEN
        else if (pieceSelected.GetComponent<QueenPiece>() == true)
        {
            GetGameObjectAround(pieceSelected);
            CheckPieceBloquedAround();

            int i = 0;

            while (i < 8)
            {
                //UP
                if (pieceUp != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + i * directionPiece) && !pieceBloquedUp)
                    {
                        StartCoroutine(MovePiece());

                        pieceUp = null;
                        pieceDown = null;
                        pieceLeft = null;
                        pieceRight = null; 
                        pieceUpLeft = null;
                        pieceUpRight = null;
                        pieceDownLeft = null;
                        pieceDownRight = null;
                        i = 8;
                    }
                }

                //Down
                if (pieceDown != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - i * directionPiece) && !pieceBloquedDown)
                    {
                        StartCoroutine(MovePiece());

                        pieceUp = null;
                        pieceDown = null;
                        pieceLeft = null;
                        pieceRight = null;
                        pieceUpLeft = null;
                        pieceUpRight = null;
                        pieceDownLeft = null;
                        pieceDownRight = null;
                        i = 8;
                    }
                }

                //Left
                if (pieceLeft != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z) && !pieceBloquedLeft)
                    {
                        StartCoroutine(MovePiece());

                        pieceUp = null;
                        pieceDown = null;
                        pieceLeft = null;
                        pieceRight = null;
                        pieceUpLeft = null;
                        pieceUpRight = null;
                        pieceDownLeft = null;
                        pieceDownRight = null;
                        i = 8;
                    }
                }

                //Right
                if (pieceRight != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z) && !pieceBloquedRight)
                    {
                        StartCoroutine(MovePiece());

                        pieceUp = null;
                        pieceDown = null;
                        pieceLeft = null;
                        pieceRight = null;
                        pieceUpLeft = null;
                        pieceUpRight = null;
                        pieceDownLeft = null;
                        pieceDownRight = null;
                        i = 8;
                    }
                }

                //UP-LEFT
                if (pieceUpLeft != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && !pieceBloquedUpLeft)
                    {
                        StartCoroutine(MovePiece());

                        pieceUp = null;
                        pieceDown = null;
                        pieceLeft = null;
                        pieceRight = null;
                        pieceUpLeft = null;
                        pieceUpRight = null;
                        pieceDownLeft = null;
                        pieceDownRight = null;
                        i = 8;
                    }
                }

                //UP-RIGHT
                if (pieceUpRight != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && !pieceBloquedUpRight)
                    {
                        StartCoroutine(MovePiece());

                        pieceUp = null;
                        pieceDown = null;
                        pieceLeft = null;
                        pieceRight = null;
                        pieceUpLeft = null;
                        pieceUpRight = null;
                        pieceDownLeft = null;
                        pieceDownRight = null;
                        i = 8;
                    }
                }

                //DOWN-LEFT
                if (pieceDownLeft != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && !pieceBloquedDownLeft)
                    {
                        StartCoroutine(MovePiece());

                        pieceUp = null;
                        pieceDown = null;
                        pieceLeft = null;
                        pieceRight = null;
                        pieceUpLeft = null;
                        pieceUpRight = null;
                        pieceDownLeft = null;
                        pieceDownRight = null;
                        i = 8;
                    }
                }

                //DOWN-RIGHT
                if (pieceDownRight != null)
                {
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && !pieceBloquedDownRight)
                    {
                        StartCoroutine(MovePiece());

                        pieceUp = null;
                        pieceDown = null;
                        pieceLeft = null;
                        pieceRight = null;
                        pieceUpLeft = null;
                        pieceUpRight = null;
                        pieceDownLeft = null;
                        pieceDownRight = null;
                        i = 8;
                    }
                }

                i++;
            }
        }

        //LOGIC OF KING
        else if (pieceSelected.GetComponent<KingPiece>() == true)
        {
            GetGameObjectAround(pieceSelected);
            CheckPieceBloquedAround();

            if (pieceSelected.GetComponent<KingPiece>().firstMovement == true && !inCheck)
            {
                if (this.CompareTag("Player1"))
                {
                    //ENROQUE-LEFT
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 2, 0, pieceSelected.transform.position.z) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false && !pieceBloquedLeft)
                    {
                        GetGameObjectLeft(boxBoardPosition);
                        GetGameObjectRight(boxBoardPosition);
                        if (pieceLeft.CompareTag("Chessboard") && pieceLeft.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false &&
                            pieceRight.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            enroqueDestinyKing = boxBoardPosition;
                            enroqueDestinyRook = pieceRight;
                            GetGameObjectLeft(pieceLeft);
                            if (pieceLeft.GetComponent<RookPiece>() == true && pieceLeft.GetComponent<RookPiece>().firstMovement == true)
                            {
                                enroquePieceKing = pieceSelected;
                                enroquePieceRook = pieceLeft;
                                StartCoroutine(EnroqueMoveLerp(enroquePieceKing, enroqueDestinyKing, enroquePieceRook, enroqueDestinyRook,1f));
                            }
                        }
                    }
                    //ENROQUE-RIGHT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 2, 0, pieceSelected.transform.position.z) &&
                         boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false && !pieceBloquedRight)
                    {
                        GetGameObjectRight(boxBoardPosition);
                        GetGameObjectLeft(boxBoardPosition);
                        if (pieceRight.GetComponent<RookPiece>() == true && pieceRight.GetComponent<RookPiece>().firstMovement == true &&
                            pieceLeft.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                        {
                            enroqueDestinyKing = boxBoardPosition;
                            enroqueDestinyRook = pieceLeft;
                            enroquePieceKing = pieceSelected;
                            enroquePieceRook = pieceRight;
                            StartCoroutine(EnroqueMoveLerp(enroquePieceKing, enroqueDestinyKing, enroquePieceRook, enroqueDestinyRook, 1f));
                        }

                    }
                    //UP
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //DOWN
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //LEFT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //RIGHT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //UP-LEFT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z + 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //UP-RIGHT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z + 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //DOWN-LEFT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z - 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //DOWN-RIGHT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z - 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                }
                else if (this.CompareTag("Player2"))
                {
                    //ENROQUE-LEFT
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 2, 0, pieceSelected.transform.position.z) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false && !pieceBloquedRight)
                    {
                        GetGameObjectLeft(boxBoardPosition);
                        GetGameObjectRight(boxBoardPosition);
                        if (pieceRight.CompareTag("Chessboard") && pieceRight.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false &&
                            pieceLeft.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            enroqueDestinyKing = boxBoardPosition;
                            enroqueDestinyRook = pieceLeft;
                            GetGameObjectRight(pieceRight);
                            if (pieceRight.GetComponent<RookPiece>() == true && pieceRight.GetComponent<RookPiece>().firstMovement == true)
                            {
                                enroquePieceKing = pieceSelected;
                                enroquePieceRook = pieceRight;
                                StartCoroutine(EnroqueMoveLerp(enroquePieceKing, enroqueDestinyKing, enroquePieceRook, enroqueDestinyRook, 1f));
                            }
                        }
                    }
                    //ENROQUE-RIGHT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 2, 0, pieceSelected.transform.position.z) &&
                         boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false && !pieceBloquedLeft)
                    {
                        GetGameObjectRight(boxBoardPosition);
                        GetGameObjectLeft(boxBoardPosition);
                        if (pieceLeft.GetComponent<RookPiece>() == true && pieceLeft.GetComponent<RookPiece>().firstMovement == true &&
                            pieceRight.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                        {
                            enroqueDestinyKing = boxBoardPosition;
                            enroqueDestinyRook = pieceRight;
                            enroquePieceKing = pieceSelected;
                            enroquePieceRook = pieceLeft;
                            StartCoroutine(EnroqueMoveLerp(enroquePieceKing, enroqueDestinyKing, enroquePieceRook, enroqueDestinyRook, 1f));
                        }
                    }
                    //UP
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //DOWN
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //LEFT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //RIGHT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //UP-LEFT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z + 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //UP-RIGHT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z + 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //DOWN-LEFT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z - 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //DOWN-RIGHT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z - 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                }
            }
            else
            {
                if (this.CompareTag("Player1"))
                {
                    //UP
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //DOWN
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //LEFT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //RIGHT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //UP-LEFT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z + 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //UP-RIGHT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z + 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //DOWN-LEFT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z - 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //DOWN-RIGHT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z - 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer2 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                }
                else if (this.CompareTag("Player2"))
                {
                    //UP
                    if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //DOWN
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //LEFT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //RIGHT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //UP-LEFT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z + 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //UP-RIGHT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z + 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //DOWN-LEFT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z - 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                    //DOWN-RIGHT
                    else if (boxBoardPosition.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z - 1) &&
                        boxBoardPosition.GetComponent<BoxEmpty>().isThreatenedPlayer1 == false)
                    {
                        StartCoroutine(MovePiece());
                    }
                }
            }

            inCheck = false;
        }
    }
    private IEnumerator MovePiece()
    {
        ICanSelect = false;

        if (ICanMove)
        {
            LigthPool.Instance.DisableList();
            StartCoroutine(MoveLerp(pieceSelected, boxBoardPosition, velMovement));

            yield return new WaitForSecondsRealtime(velMovement*4);

            CheckThreatKing();

            if (inCheck && !ICanMove && !crowningPawn)
            {
                ReverseMove();
                UpdateBoxEmpty();
            }
            else if(!inCheck && !ICanMove && !crowningPawn)
            {
                UpdateBoxEmpty();
                NextTurn();
            }
            else if(!inCheck && !ICanMove && crowningPawn)
            {
                StartCoroutine(CrowningPawnLerpUp(pieceSelected, cameraGO, 1f));
            }
        }
    }
    private IEnumerator MoveLerp(GameObject pieceSelectGO, GameObject destinyPosition, float lerpDuration)
    {
        positionSave1 = pieceSelectGO.transform.position;
        positionSave2 = destinyPosition.transform.position;

        positionTemp1 = new Vector3(positionSave1.x, 1, positionSave1.z);
        positionTemp2 = new Vector3(positionSave2.x, -1, positionSave2.z);

        positionTemp3 = new Vector3(positionSave2.x, 1, positionSave2.z);
        positionTemp4 = new Vector3(positionSave1.x, -1, positionSave1.z);


        //First movement
        float timeElapsed1 = 0;

        while (timeElapsed1 < lerpDuration)
        {
            pieceSelectGO.transform.position = Vector3.Lerp(positionSave1, positionTemp1, timeElapsed1 / lerpDuration);
            destinyPosition.transform.position = Vector3.Lerp(positionSave2, positionTemp2, timeElapsed1 / lerpDuration);
            timeElapsed1 += Time.deltaTime;
            yield return null;
        }
        pieceSelectGO.transform.position = positionTemp1;
        destinyPosition.transform.position = positionTemp2;

        //Second movement
        float timeElapsed2 = 0;

        while (timeElapsed2 < lerpDuration)
        {
            pieceSelectGO.transform.position = Vector3.Lerp(positionTemp1, positionTemp3, animationCurveMove.Evaluate(timeElapsed2 / lerpDuration));
            destinyPosition.transform.position = Vector3.Lerp(positionTemp2, positionTemp4, animationCurveMove.Evaluate(timeElapsed2 / lerpDuration));
            timeElapsed2 += Time.deltaTime;
            yield return null;
        }
        pieceSelectGO.transform.position = positionTemp3;
        destinyPosition.transform.position = positionTemp4;

        
        positionSave1 = new Vector3(positionSave1.x, 0, positionSave1.z);
        positionSave2 = new Vector3(positionSave2.x, 0, positionSave2.z);

        //Third movement
        float timeElapsed3 = 0;

        while (timeElapsed3 < lerpDuration)
        {
            pieceSelectGO.transform.position = Vector3.Lerp(positionTemp3, positionSave2, timeElapsed3 / lerpDuration);
            destinyPosition.transform.position = Vector3.Lerp(positionTemp4, positionSave1, timeElapsed3 / lerpDuration);
            timeElapsed3 += Time.deltaTime;
            yield return null;
        }
        pieceSelectGO.transform.position = positionSave2;
        destinyPosition.transform.position = positionSave1;

        ICanMove = false;
    }
    private void ReverseMove()
    {
        positionSave1 = boxBoardPosition.transform.position;
        boxBoardPosition.transform.position = pieceSelected.transform.position;
        pieceSelected.transform.position = new Vector3(positionSave1.x, 0, positionSave1.z);

        
        havePiece = false;
        boxBoardPosition = null;
        pieceSelected = null;
        LigthPool.Instance.DisableList();
        inCheck = false;
        ICanMove = true;
        ICanSelect = true;
        ICanAttack = true;

        GameObject redGreen = LigthPool.Instance.RequestLightRed();
        redGreen.transform.position = pieceKing.transform.position;
    }
    private IEnumerator EnroqueMoveLerp(GameObject pieceKing, GameObject destinyKing, GameObject pieceRook, GameObject destinyRook, float lerpDuration)
    {
        ICanSelect = false;
        animatorCamera.Play("Camera - Enroque");

        if (ICanMove)
        {
            LigthPool.Instance.DisableList();

            positionSave1 = pieceKing.transform.position;
            positionSave2 = destinyKing.transform.position;
            positionSave3 = pieceRook.transform.position;
            positionSave4 = destinyRook.transform.position;

            positionTemp1 = new Vector3(pieceKing.transform.position.x, 1, pieceKing.transform.position.z);
            positionTemp2 = new Vector3(destinyKing.transform.position.x, -1, destinyKing.transform.position.z);
            positionTemp3 = new Vector3(pieceRook.transform.position.x, 2, pieceRook.transform.position.z);
            positionTemp4 = new Vector3(destinyRook.transform.position.x, -2, destinyRook.transform.position.z);

            //First movement
            float timeElapsed1 = 0;

            while (timeElapsed1 < lerpDuration)
            {
                pieceKing.transform.position = Vector3.Lerp(positionSave1, positionTemp1, timeElapsed1 / lerpDuration);
                destinyKing.transform.position = Vector3.Lerp(positionSave2, positionTemp2, timeElapsed1 / lerpDuration);
                pieceRook.transform.position = Vector3.Lerp(positionSave3, positionTemp3, timeElapsed1 / lerpDuration);
                destinyRook.transform.position = Vector3.Lerp(positionSave4, positionTemp4, timeElapsed1 / lerpDuration);
                timeElapsed1 += Time.deltaTime;
                yield return null;
            }
            pieceKing.transform.position = positionTemp1;
            destinyKing.transform.position = positionTemp2;
            pieceRook.transform.position = positionTemp3;
            destinyRook.transform.position = positionTemp4;

            positionTemp5 = new Vector3(positionSave2.x, 1, positionSave2.z);
            positionTemp6 = new Vector3(positionSave1.x, -1, positionSave1.z);
            positionTemp7 = new Vector3(positionSave4.x, 2, positionSave4.z);
            positionTemp8 = new Vector3(positionSave3.x, -2, positionSave3.z);

            //Second movement
            float timeElapsed2 = 0;

            while (timeElapsed2 < lerpDuration)
            {
                pieceKing.transform.position = Vector3.Lerp(positionTemp1, positionTemp5, timeElapsed2 / lerpDuration);
                destinyKing.transform.position = Vector3.Lerp(positionTemp2, positionTemp6, timeElapsed2 / lerpDuration);
                pieceRook.transform.position = Vector3.Lerp(positionTemp3, positionTemp7, timeElapsed2 / lerpDuration);
                destinyRook.transform.position = Vector3.Lerp(positionTemp4, positionTemp8, timeElapsed2 / lerpDuration);
                timeElapsed2 += Time.deltaTime;
                yield return null;
            }
            pieceKing.transform.position = positionTemp5;
            destinyKing.transform.position = positionTemp6;
            pieceRook.transform.position = positionTemp7;
            destinyRook.transform.position = positionTemp8;

            positionSave1 = new Vector3(positionSave1.x, 0, positionSave1.z);
            positionSave2 = new Vector3(positionSave2.x, 0, positionSave2.z);
            positionSave3 = new Vector3(positionSave3.x, 0, positionSave3.z);
            positionSave4 = new Vector3(positionSave4.x, 0, positionSave4.z);

            //Third movement
            float timeElapsed3 = 0;

            while (timeElapsed3 < lerpDuration)
            {
                pieceKing.transform.position = Vector3.Lerp(positionTemp5, positionSave2, timeElapsed3 / lerpDuration);
                destinyKing.transform.position = Vector3.Lerp(positionTemp6, positionSave1, timeElapsed3 / lerpDuration);
                pieceRook.transform.position = Vector3.Lerp(positionTemp7, positionSave4, timeElapsed3 / lerpDuration);
                destinyRook.transform.position = Vector3.Lerp(positionTemp8, positionSave3, timeElapsed3 / lerpDuration);
                timeElapsed3 += Time.deltaTime;
                yield return null;
            }
            pieceKing.transform.position = positionSave2;
            destinyKing.transform.position = positionSave1;
            pieceRook.transform.position = positionSave4;
            destinyRook.transform.position = positionSave3;

            ICanMove = false;

            yield return new WaitForSecondsRealtime(velMovement * 4);

            pieceKing.GetComponent<KingPiece>().firstMovement = false;
            pieceRook.GetComponent<RookPiece>().firstMovement = false;

            UpdateBoxEmpty();
            NextTurn();
        }
    }
    private IEnumerator CrowningPawnLerpUp(GameObject pawn, GameObject camera, float lerpDuration)
    {
        UI_CrowningPawn.SetActive(true);
        UI_BlockPanel.SetActive(true);
        animatorCanvas.Play("Crowning Pawn - Intro");

        positionSave1 = pawn.transform.position;
        positionSave2 = camera.transform.position;

        positionTemp1 = new Vector3(pawn.transform.position.x, 5, pawn.transform.position.z);
        positionTemp2 = new Vector3(positionTemp1.x, positionTemp1.y, positionTemp1.z - 3.5f);

        positionTemp7 = new Vector3(0, 0, 0);

        if(playerTag == "Player2")
        {
            positionTemp8 = new Vector3(0, 90, 0);
        }
        else
        {
            positionTemp8 = new Vector3(0, 0, 0);
        }

        //First movement
        float timeElapsed1 = 0;

        while (timeElapsed1 < lerpDuration)
        {
            pawn.transform.position = Vector3.Lerp(positionSave1, positionTemp1, timeElapsed1 / lerpDuration);
            if (playerTag == "Player2") 
            { 
                pawn.transform.rotation = Quaternion.Lerp(Quaternion.Euler(positionTemp7), Quaternion.Euler(positionTemp8), timeElapsed1 / lerpDuration); 
            }
            camera.transform.position = Vector3.Lerp(positionSave2, positionTemp2, timeElapsed1 / lerpDuration);
            camera.transform.rotation = Quaternion.Lerp(rotationOriginalCamera, Quaternion.Euler(rotationTempCamera), timeElapsed1 / lerpDuration);
            timeElapsed1 += Time.deltaTime;
            yield return null;
        }
        pawn.transform.position = positionTemp1;
        camera.transform.position = positionTemp2;

        UI_BlockPanel.SetActive(false);
    }
    public IEnumerator CrowningPawnDownLogic(string CrowningTo)
    {
        positionSave3 = pieceSelected.transform.position;

        Destroy(pieceSelected);

        if (CrowningTo == "Queen") 
        {  
            pieceSelected = chessboard.GenerateQueen(positionSave3);
            yield return null;
            pieceSelected.tag = playerTag;
        }
        else if (CrowningTo == "Bishop")
        {  
            pieceSelected = chessboard.GenerateBishop(positionSave3);
            yield return null;
            pieceSelected.tag = playerTag;
        }
        else if (CrowningTo == "Knight") 
        {  
            pieceSelected = chessboard.GenerateKnight(positionSave3);
            yield return null;
            pieceSelected.tag = playerTag;
        }
        else if (CrowningTo == "Rook")
        {  
            pieceSelected = chessboard.GenerateRook(positionSave3);
            yield return null;
            pieceSelected.tag = playerTag;
        }

        if (!ICanMove)
        {
            StartCoroutine(CrowningPawnLerpDown(pieceSelected, positionSave1, cameraGO, 1f));
        } else if (!ICanAttack)
        {
            StartCoroutine(CrowningPawnLerpDown(pieceSelected, positionOriginalEnemy, cameraGO, 1f));
        }
        
    }
    private IEnumerator CrowningPawnLerpDown(GameObject pieceCrowned, Vector3 pieceCrownedDestiny, GameObject camera, float lerpDuration)
    {
        animatorCanvas.Play("Crowning Pawn - Ending");
        UI_BlockPanel.SetActive(true);

        positionTemp3 = pieceCrowned.transform.position;
        positionTemp4 = camera.transform.position;

        //Second movement
        float timeElapsed2 = 0;

        while (timeElapsed2 < lerpDuration)
        {
            pieceCrowned.transform.position = Vector3.Lerp(positionTemp3, pieceCrownedDestiny, timeElapsed2 / lerpDuration);
            camera.transform.position = Vector3.Lerp(positionTemp4, positionOriginalCamera, timeElapsed2 / lerpDuration);
            camera.transform.rotation = Quaternion.Lerp(Quaternion.Euler(rotationTempCamera), rotationOriginalCamera, timeElapsed2 / lerpDuration);
            timeElapsed2 += Time.deltaTime;
            yield return null;
        }
        pieceCrowned.transform.position = pieceCrownedDestiny;
        camera.transform.position = positionOriginalCamera;

        yield return null;

        crowningPawn = false;
        UI_BlockPanel.SetActive(false);
        UI_CrowningPawn.SetActive(false);
        UpdateBoxEmpty();
        NextTurn();
    }

    //ATTACK
    private void LogicAttackPieces()
    {
        if (havePiece && pieceEnemy != null)
        {
            //LOGIC OF PAWN
            if (pieceSelected.GetComponent<PawnPiece>() == true)
            {
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x + 1 * directionPiece, 0, pieceSelected.transform.position.z + 1 * directionPiece))
                {
                    StartCoroutine(AttackPiece()); 
                    
                    if (playerTag == "Player1")
                    {
                        if (pieceEnemy.transform.position.z >= 7)
                        {
                            crowningPawn = true;
                        }
                    }
                    else
                    {
                        if (pieceEnemy.transform.position.z <= 0)
                        {
                            crowningPawn = true;
                        }
                    }
                }
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x - 1 * directionPiece, 0, pieceSelected.transform.position.z + 1 * directionPiece))
                {
                    StartCoroutine(AttackPiece());

                    if (playerTag == "Player1")
                    {
                        if (pieceEnemy.transform.position.z >= 7)
                        {
                            crowningPawn = true;
                        }
                    }
                    else
                    {
                        if (pieceEnemy.transform.position.z <= 0)
                        {
                            crowningPawn = true;
                        }
                    }
                }
            }

            //LOGIC OF ROOK
            else if (pieceSelected.GetComponent<RookPiece>() == true)
            {
                GetGameObjectAround(pieceSelected);
                CheckPieceBloquedAround();

                //UP
                for (int z = 0; z < 8; z++)
                {
                    if (pieceUp != null)
                    {
                        GameObject boxEnemyUp = new GameObject();
                        boxEnemyUp.transform.position = new Vector3(pieceUp.transform.position.x, 0, pieceUp.transform.position.z);

                        if (boxEnemyUp.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + z * directionPiece) && !pieceBloquedUp)
                        {
                            GetGameObjectUp(boxEnemyUp);
                            CheckPieceBloquedUp();
                        }
                        else if (boxEnemyUp.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + z * directionPiece) && pieceBloquedUp
                            && pieceUp.CompareTag(enemyTag) && boxEnemyUp.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyUp);
                    }
                }

                //Down
                for (int z = 0; z < 8; z++)
                {
                    if (pieceDown != null)
                    {
                        GameObject boxEnemyDown = new GameObject();
                        boxEnemyDown.transform.position = new Vector3(pieceDown.transform.position.x, 0, pieceDown.transform.position.z);

                        if (boxEnemyDown.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - z * directionPiece) && !pieceBloquedDown)
                        {
                            GetGameObjectDown(boxEnemyDown);
                            CheckPieceBloquedDown();
                        }
                        else if (boxEnemyDown.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - z * directionPiece) && pieceBloquedDown
                            && pieceDown.CompareTag(enemyTag) && boxEnemyDown.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyDown);
                    }
                }

                //Left
                for (int x = 0; x < 8; x++)
                {
                    if (pieceLeft != null)
                    {
                        GameObject boxEnemyLeft = new GameObject();
                        boxEnemyLeft.transform.position = new Vector3(pieceLeft.transform.position.x, 0, pieceLeft.transform.position.z);

                        if (boxEnemyLeft.transform.position == new Vector3(pieceSelected.transform.position.x - x * directionPiece, 0, pieceSelected.transform.position.z) && !pieceBloquedLeft)
                        {
                            GetGameObjectLeft(boxEnemyLeft);
                            CheckPieceBloquedLeft();
                        }
                        else if (boxEnemyLeft.transform.position == new Vector3(pieceSelected.transform.position.x - x * directionPiece, 0, pieceSelected.transform.position.z) && pieceBloquedLeft
                            && pieceLeft.CompareTag(enemyTag) && boxEnemyLeft.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyLeft);
                    }
                }

                //Right
                for (int x = 0; x < 8; x++)
                {
                    if (pieceRight != null)
                    {
                        GameObject boxEnemyRight = new GameObject();
                        boxEnemyRight.transform.position = new Vector3(pieceRight.transform.position.x, 0, pieceRight.transform.position.z);

                        if (boxEnemyRight.transform.position == new Vector3(pieceSelected.transform.position.x + x, 0, pieceSelected.transform.position.z) && !pieceBloquedRight)
                        {
                            
                            GetGameObjectRight(boxEnemyRight);
                            CheckPieceBloquedRight();
                        }
                        else if (boxEnemyRight.transform.position == new Vector3(pieceSelected.transform.position.x + x, 0, pieceSelected.transform.position.z) && pieceBloquedRight
                            && pieceRight.CompareTag(enemyTag) && boxEnemyRight.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyRight);
                    }
                }
            }

            //LOGIC OF KNIGHT
            else if (pieceSelected.GetComponent<KnightPiece>() == true)
            {
                //CASE 1
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z + 2))
                {
                    StartCoroutine(AttackPiece());
                }
                //CASE 2
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x + 2, 0, pieceSelected.transform.position.z + 1))
                {
                    StartCoroutine(AttackPiece());
                }
                //CASE 3
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x + 2, 0, pieceSelected.transform.position.z - 1))
                {
                    StartCoroutine(AttackPiece());
                }
                //CASE 4
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z - 2))
                {
                    StartCoroutine(AttackPiece());
                }
                //CASE 5
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z - 2))
                {
                    StartCoroutine(AttackPiece());
                }
                //CASE 6
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x - 2, 0, pieceSelected.transform.position.z - 1))
                {
                    StartCoroutine(AttackPiece());
                }
                //CASE 7
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x - 2, 0, pieceSelected.transform.position.z + 1))
                {
                    StartCoroutine(AttackPiece());
                }
                //CASE 8
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z + 2))
                {
                    StartCoroutine(AttackPiece());
                }
            }

            //LOGIC OF BISHOP
            else if (pieceSelected.GetComponent<BishopPiece>() == true)
            {
                GetGameObjectAround(pieceSelected);
                CheckPieceBloquedAround();

                //UP-LEFT
                for (int i = 0; i < 8; i++)
                {
                    if (pieceUpLeft != null)
                    {
                        GameObject boxEnemyUpLeft = new GameObject();
                        boxEnemyUpLeft.transform.position = new Vector3(pieceUpLeft.transform.position.x, 0, pieceUpLeft.transform.position.z);

                        if (boxEnemyUpLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && !pieceBloquedUpLeft)
                        {
                            GetGameObjectUpLeft(boxEnemyUpLeft);
                            CheckPieceBloquedUpLeft();
                        }
                        else if (boxEnemyUpLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && pieceBloquedUpLeft
                            && pieceUpLeft.CompareTag(enemyTag) && boxEnemyUpLeft.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyUpLeft);
                    }
                }


                //UP-RIGHT
                for (int i = 0; i < 8; i++)
                {
                    if (pieceUpRight != null)
                    {
                        GameObject boxEnemyUpRight = new GameObject();
                        boxEnemyUpRight.transform.position = new Vector3(pieceUpRight.transform.position.x, 0, pieceUpRight.transform.position.z);

                        if (boxEnemyUpRight.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && !pieceBloquedUpRight)
                        {
                            GetGameObjectUpRight(boxEnemyUpRight);
                            CheckPieceBloquedUpRight();
                        }
                        else if (boxEnemyUpRight.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && pieceBloquedUpRight
                            && pieceUpRight.CompareTag(enemyTag) && boxEnemyUpRight.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyUpRight);
                    }
                }

                //DOWN-LEFT
                for (int i = 0; i < 8; i++)
                {
                    if (pieceDownLeft != null)
                    {
                        GameObject boxEnemyDownLeft = new GameObject();
                        boxEnemyDownLeft.transform.position = new Vector3(pieceDownLeft.transform.position.x, 0, pieceDownLeft.transform.position.z);

                        if (boxEnemyDownLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && !pieceBloquedDownLeft)
                        {
                            GetGameObjectDownLeft(boxEnemyDownLeft);
                            CheckPieceBloquedDownLeft();
                        }
                        else if (boxEnemyDownLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && pieceBloquedDownLeft
                            && pieceDownLeft.CompareTag(enemyTag) && boxEnemyDownLeft.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyDownLeft);
                    }
                }

                //DOWN-RIGHT
                for (int i = 0; i < 8; i++)
                {
                    if (pieceDownRight != null)
                    {
                        GameObject boxEnemyDownRight = new GameObject();
                        boxEnemyDownRight.transform.position = new Vector3(pieceDownRight.transform.position.x, 0, pieceDownRight.transform.position.z);

                        if (boxEnemyDownRight.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && !pieceBloquedDownRight)
                        {

                            GetGameObjectDownRight(boxEnemyDownRight);
                            CheckPieceBloquedDownRight();
                        }
                        else if (boxEnemyDownRight.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && pieceBloquedDownRight
                            && pieceDownRight.CompareTag(enemyTag) && boxEnemyDownRight.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyDownRight);
                    }
                }
            }

            //LOGIC OF QUEEN
            else if (pieceSelected.GetComponent<QueenPiece>() == true)
            {
                GetGameObjectAround(pieceSelected);
                CheckPieceBloquedAround();

                //UP
                for (int z = 0; z < 8; z++)
                {
                    if (pieceUp != null)
                    {
                        GameObject boxEnemyUp = new GameObject();
                        boxEnemyUp.transform.position = new Vector3(pieceUp.transform.position.x, 0, pieceUp.transform.position.z);

                        if (boxEnemyUp.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + z * directionPiece) && !pieceBloquedUp)
                        {
                            GetGameObjectUp(boxEnemyUp);
                            CheckPieceBloquedUp();
                        }
                        else if (boxEnemyUp.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + z * directionPiece) && pieceBloquedUp
                            && pieceUp.CompareTag(enemyTag) && boxEnemyUp.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyUp);
                    }
                }

                //Down
                for (int z = 0; z < 8; z++)
                {
                    if (pieceDown != null)
                    {
                        GameObject boxEnemyDown = new GameObject();
                        boxEnemyDown.transform.position = new Vector3(pieceDown.transform.position.x, 0, pieceDown.transform.position.z);

                        if (boxEnemyDown.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - z * directionPiece) && !pieceBloquedDown)
                        {
                            GetGameObjectDown(boxEnemyDown);
                            CheckPieceBloquedDown();
                        }
                        else if (boxEnemyDown.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - z * directionPiece) && pieceBloquedDown
                            && pieceDown.CompareTag(enemyTag) && boxEnemyDown.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyDown);
                    }
                }

                //Left
                for (int x = 0; x < 8; x++)
                {
                    if (pieceLeft != null)
                    {
                        GameObject boxEnemyLeft = new GameObject();
                        boxEnemyLeft.transform.position = new Vector3(pieceLeft.transform.position.x, 0, pieceLeft.transform.position.z);

                        if (boxEnemyLeft.transform.position == new Vector3(pieceSelected.transform.position.x - x * directionPiece, 0, pieceSelected.transform.position.z) && !pieceBloquedLeft)
                        {
                            GetGameObjectLeft(boxEnemyLeft);
                            CheckPieceBloquedLeft();
                        }
                        else if (boxEnemyLeft.transform.position == new Vector3(pieceSelected.transform.position.x - x * directionPiece, 0, pieceSelected.transform.position.z) && pieceBloquedLeft
                            && pieceLeft.CompareTag(enemyTag) && boxEnemyLeft.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyLeft);
                    }
                }

                //Right
                for (int x = 0; x < 8; x++)
                {
                    if (pieceRight != null)
                    {
                        GameObject boxEnemyRight = new GameObject();
                        boxEnemyRight.transform.position = new Vector3(pieceRight.transform.position.x, 0, pieceRight.transform.position.z);

                        if (boxEnemyRight.transform.position == new Vector3(pieceSelected.transform.position.x + x * directionPiece, 0, pieceSelected.transform.position.z) && !pieceBloquedRight)
                        {

                            GetGameObjectRight(boxEnemyRight);
                            CheckPieceBloquedRight();
                        }
                        else if (boxEnemyRight.transform.position == new Vector3(pieceSelected.transform.position.x + x * directionPiece, 0, pieceSelected.transform.position.z) && pieceBloquedRight
                            && pieceRight.CompareTag(enemyTag) && boxEnemyRight.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyRight);
                    }
                }

                //UP-LEFT
                for (int i = 0; i < 8; i++)
                {
                    if (pieceUpLeft != null)
                    {
                        GameObject boxEnemyUpLeft = new GameObject();
                        boxEnemyUpLeft.transform.position = new Vector3(pieceUpLeft.transform.position.x, 0, pieceUpLeft.transform.position.z);

                        if (boxEnemyUpLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && !pieceBloquedUpLeft)
                        {
                            GetGameObjectUpLeft(boxEnemyUpLeft);
                            CheckPieceBloquedUpLeft();
                        }
                        else if (boxEnemyUpLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && pieceBloquedUpLeft
                            && pieceUpLeft.CompareTag(enemyTag) && boxEnemyUpLeft.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyUpLeft);
                    }
                }

                //UP-RIGHT
                for (int i = 0; i < 8; i++)
                {
                    if (pieceUpRight != null)
                    {
                        GameObject boxEnemyUpRight = new GameObject();
                        boxEnemyUpRight.transform.position = new Vector3(pieceUpRight.transform.position.x, 0, pieceUpRight.transform.position.z);

                        if (boxEnemyUpRight.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && !pieceBloquedUpRight)
                        {
                            GetGameObjectUpRight(boxEnemyUpRight);
                            CheckPieceBloquedUpRight();
                        }
                        else if (boxEnemyUpRight.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z + i * directionPiece) && pieceBloquedUpRight
                            && pieceUpRight.CompareTag(enemyTag) && boxEnemyUpRight.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyUpRight);
                    }
                }

                //DOWN-LEFT
                for (int i = 0; i < 8; i++)
                {
                    if (pieceDownLeft != null)
                    {
                        GameObject boxEnemyDownLeft = new GameObject();
                        boxEnemyDownLeft.transform.position = new Vector3(pieceDownLeft.transform.position.x, 0, pieceDownLeft.transform.position.z);

                        if (boxEnemyDownLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && !pieceBloquedDownLeft)
                        {
                            GetGameObjectDownLeft(boxEnemyDownLeft);
                            CheckPieceBloquedDownLeft();
                        }
                        else if (boxEnemyDownLeft.transform.position == new Vector3(pieceSelected.transform.position.x - i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && pieceBloquedDownLeft
                            && pieceDownLeft.CompareTag(enemyTag) && boxEnemyDownLeft.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyDownLeft);
                    }
                }

                //DOWN-RIGHT
                for (int i = 0; i < 8; i++)
                {
                    if (pieceDownRight != null)
                    {
                        GameObject boxEnemyDownRight = new GameObject();
                        boxEnemyDownRight.transform.position = new Vector3(pieceDownRight.transform.position.x, 0, pieceDownRight.transform.position.z);

                        if (boxEnemyDownRight.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && !pieceBloquedDownRight)
                        {

                            GetGameObjectDownRight(boxEnemyDownRight);
                            CheckPieceBloquedDownRight();
                        }
                        else if (boxEnemyDownRight.transform.position == new Vector3(pieceSelected.transform.position.x + i * directionPiece, 0, pieceSelected.transform.position.z - i * directionPiece) && pieceBloquedDownRight
                            && pieceDownRight.CompareTag(enemyTag) && boxEnemyDownRight.transform.position == pieceEnemy.transform.position)
                        {
                            StartCoroutine(AttackPiece());
                        }

                        Destroy(boxEnemyDownRight);
                    }
                }
            }

            //LOGIC OF KING
            else if (pieceSelected.GetComponent<KingPiece>() == true)
            {
                //UP
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z + 1))
                {
                    StartCoroutine(AttackPiece());
                }
                //DOWN
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z - 1))
                {
                    StartCoroutine(AttackPiece());
                }
                //LEFT
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z))
                {
                    StartCoroutine(AttackPiece());
                }
                //RIGHT
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z))
                {
                    StartCoroutine(AttackPiece());
                }
                //UP-LEFT
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z + 1))
                {
                    StartCoroutine(AttackPiece());
                }
                //UP-RIGHT
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z + 1))
                {
                    StartCoroutine(AttackPiece());
                }
                //DOWN-LEFT
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x - 1, 0, pieceSelected.transform.position.z - 1))
                {
                    StartCoroutine(AttackPiece());
                }
                //DOWN-RIGHT
                if (pieceEnemy.transform.position == new Vector3(pieceSelected.transform.position.x + 1, 0, pieceSelected.transform.position.z - 1))
                {
                    StartCoroutine(AttackPiece());
                }
            }
        }
    }
    private IEnumerator AttackPiece()
    {
        ICanSelect = false;

        if (ICanAttack)
        {
            LigthPool.Instance.DisableList();
            StartCoroutine(AttackLerp(pieceSelected, pieceEnemy, velMovement));

            yield return new WaitForSecondsRealtime(velMovement * 5);

            CheckThreatKing();

            if (inCheck && !ICanAttack)
            {
                ReverseAttack();
                UpdateBoxEmpty();
            }
            else if (!inCheck && !ICanAttack && !crowningPawn)
            {
                if(playerTag == "Player1") 
                { 
                    gameController.AddDeadPieceBlack(pieceEnemy);

                    GameObject.Destroy(pieceEnemy);
                }
                else 
                { 
                    gameController.AddDeadPieceWhite(pieceEnemy);

                    GameObject.Destroy(pieceEnemy);
                }

                UpdateBoxEmpty();
                NextTurn();
            }
            else if (!inCheck && !ICanAttack && crowningPawn)
            {
                if (playerTag == "Player1")
                {
                    gameController.AddDeadPieceBlack(pieceEnemy);

                    GameObject.Destroy(pieceEnemy);
                }
                else
                {
                    gameController.AddDeadPieceWhite(pieceEnemy);

                    GameObject.Destroy(pieceEnemy);
                }

                StartCoroutine(CrowningPawnLerpUp(pieceSelected, cameraGO, 1f));
            }
        }
    }
    private IEnumerator AttackLerp(GameObject pieceSelectGO, GameObject destinyPosition, float lerpDuration)
    {
        positionSave1 = pieceSelectGO.transform.position;
        positionSave2 = destinyPosition.transform.position;

        positionTemp1 = new Vector3(positionSave1.x, 1, positionSave1.z);
        positionTemp2 = new Vector3(positionSave2.x, 1, positionSave2.z);

        //First movement
        float timeElapsed1 = 0;

        while (timeElapsed1 < lerpDuration)
        {
            pieceSelectGO.transform.position = Vector3.Lerp(positionSave1, positionTemp1, timeElapsed1 / lerpDuration);
            timeElapsed1 += Time.deltaTime;
            yield return null;
        }
        pieceSelectGO.transform.position = positionTemp1;

        //Second movement
        float timeElapsed2 = 0;

        while (timeElapsed2 < lerpDuration)
        {
            pieceSelectGO.transform.position = Vector3.Lerp(positionTemp1, positionTemp2, animationCurveMove.Evaluate(timeElapsed2 / lerpDuration));
            timeElapsed2 += Time.deltaTime;
            yield return null;
        }
        pieceSelectGO.transform.position = positionTemp2;

        positionSave1 = new Vector3(positionSave1.x, 0, positionSave1.z);
        positionSave2 = new Vector3(positionSave2.x, 0, positionSave2.z);

        if (ICanCreateBoxEmpty)
        {
            lastBoxEmptyCreated = chessboard.GenerateBoxChessEmpty(new Vector3(positionSave1.x, 0, positionSave1.z));
            lastBoxEmptyCreated.transform.localScale = new Vector3(0, 0, 0);
            destinyPosition.transform.localScale = new Vector3(0, 0, 0);

            ICanCreateBoxEmpty = false;
        }

        //Third movement
        float timeElapsed3 = 0;

        while (timeElapsed3 < lerpDuration)
        {
            pieceSelectGO.transform.position = Vector3.Lerp(positionTemp2, positionSave2, timeElapsed3 / lerpDuration);
            lastBoxEmptyCreated.transform.localScale = Vector3.Lerp(new Vector3(0,0,0), new Vector3(1, 1, 1), animationCurveMove.Evaluate(timeElapsed3 / lerpDuration));
            timeElapsed3 += Time.deltaTime;
            yield return null;
        }
        pieceSelectGO.transform.position = positionSave2;
        lastBoxEmptyCreated.transform.localScale = new Vector3(1, 1, 1);

        ICanAttack = false;
    }
    private void ReverseAttack()
    {
        positionSave1 = lastBoxEmptyCreated.transform.position;
        pieceSelected.transform.position = new Vector3(positionSave1.x, 0, positionSave1.z);

        pieceEnemy.transform.localScale = new Vector3(1, 1, 1);
        GameObject.Destroy(lastBoxEmptyCreated);

        havePiece = false;
        boxBoardPosition = null;
        pieceEnemy = null;
        pieceSelected = null;
        LigthPool.Instance.DisableList();
        inCheck = false;
        ICanMove = true;
        ICanSelect = true;
        ICanAttack = true;
        ICanCreateBoxEmpty = true;

        GameObject redGreen = LigthPool.Instance.RequestLightRed();
        redGreen.transform.position = pieceKing.transform.position;
    }

    //OTHERS
    private void DisableFirstMovement()
    {
        if (pieceSelected != null)
        {
            if (pieceSelected.GetComponent<PawnPiece>() != null) { pieceSelected.GetComponent<PawnPiece>().firstMovement = false; }
            else if (pieceSelected.GetComponent<RookPiece>() != null) { pieceSelected.GetComponent<RookPiece>().firstMovement = false; }
            else if (pieceSelected.GetComponent<KingPiece>() != null) { pieceSelected.GetComponent<KingPiece>().firstMovement = false; }
        }
    }
    public void NextTurn()
    {
        DisableFirstMovement();
        havePiece = false;
        boxBoardPosition = null;
        pieceEnemy = null;
        pieceSelected = null;
        LigthPool.Instance.DisableList();
        myTurn = false;
    }
    private void TakeLeavePiece()
    {
        if (havePiece) { havePiece = false; }
        else { havePiece = true; }
    }
    private void UpDownPiece()
    {
        if (havePiece)
        {
            pieceSelected.transform.position = new Vector3(pieceSelected.transform.position.x, 0.5f, pieceSelected.transform.position.z);
        } else
        {
            pieceSelected.transform.position = new Vector3(pieceSelected.transform.position.x, 0, pieceSelected.transform.position.z);
        }
    }
    public void UpdateBoxEmpty()
    {
        boxsChessboardWhitTags = GameObject.FindGameObjectsWithTag("Chessboard");
        foreach (GameObject boxEmpty in boxsChessboardWhitTags)
        {
            boxEmpty.GetComponent<BoxEmpty>().isThreatenedPlayer1 = false;
            boxEmpty.GetComponent<BoxEmpty>().isThreatenedPlayer2 = false;
            boxEmpty.GetComponent<BoxEmpty>().ChangeName();
            boxEmpty.GetComponent<BoxEmpty>().ChangeMaterial();
            boxEmpty.GetComponent<BoxEmpty>().transform.localScale = new Vector3(1, 1, 1);
            boxEmpty.GetComponent<BoxEmpty>().CheckThreat();
        }
    }
    private IEnumerator GetKingPieces()
    {
        yield return new WaitForSeconds(1);
        pieceKing = GameObject.Find("King" + this.tag);
    }

    #region Get GameObjects Around
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
        pieceUp = null;

        Ray rayUp = new Ray(piece.transform.position, piece.transform.forward* directionPiece);
        RaycastHit hitUp;
        if (Physics.Raycast(rayUp, out hitUp, 10f))
        {
            pieceUp = hitUp.transform.gameObject;
        }
        else
        {
            pieceUp = null;
        }
    }
    private void GetGameObjectDown(GameObject piece)
    {
        pieceDown = null;

        Ray rayDown = new Ray(piece.transform.position, piece.transform.forward * -directionPiece);
        RaycastHit hitDown;
        if (Physics.Raycast(rayDown, out hitDown, 10f))
        {
            pieceDown = hitDown.transform.gameObject;
        }
        else
        {
            pieceDown = null;
        }
    }
    private void GetGameObjectLeft(GameObject piece)
    {
        pieceLeft = null;

        Ray rayLeft = new Ray(piece.transform.position, piece.transform.right * -directionPiece);
        RaycastHit hitLeft;
        if (Physics.Raycast(rayLeft, out hitLeft, 10f))
        {
            pieceLeft = hitLeft.transform.gameObject;
        }
        else
        {
            pieceLeft = null;
        }
    }
    private void GetGameObjectRight(GameObject piece)
    {
        pieceRight = null;

        Ray rayRight = new Ray(piece.transform.position, piece.transform.right * directionPiece);
        RaycastHit hitRight;
        if (Physics.Raycast(rayRight, out hitRight, 10f))
        {
            pieceRight = hitRight.transform.gameObject;
        }
        else
        {
            pieceRight = null;
        }
    }
    private void GetGameObjectUpLeft(GameObject piece)
    {
        pieceUpLeft = null;

        Ray rayUp = new Ray(piece.transform.position, piece.transform.forward * directionPiece);
        RaycastHit hitUp;
        if (Physics.Raycast(rayUp, out hitUp, 10f))
        {
            Ray rayLeft = new Ray(hitUp.transform.position, hitUp.transform.right * -directionPiece);
            RaycastHit hitLeft;
            if (Physics.Raycast(rayLeft, out hitLeft, 10f))
            {
                pieceUpLeft = hitLeft.transform.gameObject;
            }
            else
            {
                pieceUpLeft = null;
            }
        }
        else
        {
            pieceUpLeft = null;
        }
    }
    private void GetGameObjectUpRight(GameObject piece)
    {
        pieceUpRight = null;

        Ray rayUp = new Ray(piece.transform.position, piece.transform.forward * directionPiece);
        RaycastHit hitUp;
        if (Physics.Raycast(rayUp, out hitUp, 10f))
        {
            Ray rayRight = new Ray(hitUp.transform.position, hitUp.transform.right * directionPiece);
            RaycastHit hitRight;
            if (Physics.Raycast(rayRight, out hitRight, 10f))
            {
                pieceUpRight = hitRight.transform.gameObject;
            }
            else
            {
                pieceUpRight = null;
            }
        }
        else
        {
            pieceUpRight = null;
        }
    }
    private void GetGameObjectDownLeft(GameObject piece)
    {
        pieceDownLeft = null;

        Ray rayDown = new Ray(piece.transform.position, piece.transform.forward * -directionPiece);
        RaycastHit hitDown;
        if (Physics.Raycast(rayDown, out hitDown, 10f))
        {
            Ray rayLeft = new Ray(hitDown.transform.position, hitDown.transform.right * -directionPiece);
            RaycastHit hitLeft;
            if (Physics.Raycast(rayLeft, out hitLeft, 10f))
            {
                pieceDownLeft = hitLeft.transform.gameObject;
            }
            else
            {
                pieceDownLeft = null;
            }
        }
        else
        {
            pieceDownLeft = null;
        }
    }
    private void GetGameObjectDownRight(GameObject piece)
    {
        pieceDownRight = null;

        Ray rayDown = new Ray(piece.transform.position, piece.transform.forward * -directionPiece);
        RaycastHit hitDown;
        if (Physics.Raycast(rayDown, out hitDown, 10f))
        {
            Ray rayRight = new Ray(hitDown.transform.position, hitDown.transform.right * directionPiece);
            RaycastHit hitRight;
            if (Physics.Raycast(rayRight, out hitRight, 10f))
            {
                pieceDownRight = hitRight.transform.gameObject;
            }
            else
            {
                pieceDownRight = null;
            }
        }
        else
        {
            pieceDownRight = null;
        }
    }
    #endregion

    #region Check Piece Bloqued
    private void CheckPieceBloquedAround()
    {
        CheckPieceBloquedUp();
        CheckPieceBloquedDown();
        CheckPieceBloquedLeft();
        CheckPieceBloquedRight();
        CheckPieceBloquedUpLeft();
        CheckPieceBloquedUpRight();
        CheckPieceBloquedDownLeft();
        CheckPieceBloquedDownRight();
    }
    private void CheckPieceBloquedUp()
    {
        pieceBloquedUp = false;

        if (pieceUp != null)
        {
            if (pieceUp.tag == enemyTag || pieceUp.tag == playerTag && pieceUp)
            {
                pieceBloquedUp = true;
            }
            else
            {
                pieceBloquedUp = false;
            }
        }
        else
        {
            pieceBloquedUp = true;
        }
    }
    private void CheckPieceBloquedDown()
    {
        pieceBloquedDown = false;

        if (pieceDown != null)
        {
            if (pieceDown.tag == enemyTag || pieceDown.tag == playerTag)
            {
                pieceBloquedDown = true;
            }
            else
            {
                pieceBloquedDown = false;
            }
        }
        else
        {
            pieceBloquedDown = true;
        }
    }
    private void CheckPieceBloquedLeft()
    {
        pieceBloquedLeft = false;

        if (pieceLeft != null)
        {
            if (pieceLeft.tag == enemyTag || pieceLeft.tag == playerTag)
            {
                pieceBloquedLeft = true;
            }
            else
            {
                pieceBloquedLeft = false;
            }
        }
        else
        {
            pieceBloquedLeft = true;
        }
    }
    private void CheckPieceBloquedRight()
    {
        pieceBloquedRight = false;

        if (pieceRight != null)
        {
            if (pieceRight.tag == enemyTag || pieceRight.tag == playerTag)
            {
                pieceBloquedRight = true;
            }
            else
            {
                pieceBloquedRight = false;
            }
        }
        else
        {
            pieceBloquedRight = true;
        }
    }
    private void CheckPieceBloquedUpLeft()
    {
        pieceBloquedUpLeft = false;

        if (pieceUpLeft != null)
        {
            if (pieceUpLeft.tag == enemyTag || pieceUpLeft.tag == playerTag && pieceUpLeft)
            {
                pieceBloquedUpLeft = true;
            }
            else
            {
                pieceBloquedUpLeft = false;
            }
        }
        else
        {
            pieceBloquedUpLeft = true;
        }
    }
    private void CheckPieceBloquedUpRight()
    {
        pieceBloquedUpRight = false;

        if (pieceUpRight != null)
        {
            if (pieceUpRight.tag == enemyTag || pieceUpRight.tag == playerTag && pieceUpRight)
            {
                pieceBloquedUpRight = true;
            }
            else
            {
                pieceBloquedUpRight = false;
            }
        }
        else
        {
            pieceBloquedUpRight = true;
        }
    }
    private void CheckPieceBloquedDownLeft()
    {
        pieceBloquedDownLeft = false;

        if (pieceDownLeft != null)
        {
            if (pieceDownLeft.tag == enemyTag || pieceDownLeft.tag == playerTag)
            {
                pieceBloquedDownLeft = true;
            }
            else
            {
                pieceBloquedDownLeft = false;
            }
        }
        else
        {
            pieceBloquedDownLeft = true;
        }
    }
    private void CheckPieceBloquedDownRight()
    {
        pieceBloquedDownRight = false;

        if (pieceDownRight != null)
        {
            if (pieceDownRight.tag == enemyTag || pieceDownRight.tag == playerTag)
            {
                pieceBloquedDownRight = true;
            }
            else
            {
                pieceBloquedDownRight = false;
            }
        }
        else
        {
            pieceBloquedDownRight = true;
        }
    }
    #endregion

    #region Get GameObjects Around KING
    private void GetGameObjectAroundKing(GameObject piece)
    {
        GetGameObjectUpKing(piece);
        GetGameObjectDownKing(piece);
        GetGameObjectLeftKing(piece);
        GetGameObjectRightKing(piece);
        GetGameObjectUpLeftKing(piece);
        GetGameObjectUpRightKing(piece);
        GetGameObjectDownLeftKing(piece);
        GetGameObjectDownRightKing(piece);
    }
    private void GetGameObjectUpKing(GameObject piece)
    {
        pieceUpKing = null;

        Ray rayUp = new Ray(piece.transform.position, piece.transform.forward * directionPiece);
        RaycastHit hitUp;
        if (Physics.Raycast(rayUp, out hitUp, 10f))
        {
            pieceUpKing = hitUp.transform.gameObject;
        }
        else
        {
            pieceUpKing = null;
        }
    }
    private void GetGameObjectDownKing(GameObject piece)
    {
        pieceDownKing = null;

        Ray rayDown = new Ray(piece.transform.position, piece.transform.forward * -directionPiece);
        RaycastHit hitDown;
        if (Physics.Raycast(rayDown, out hitDown, 10f))
        {
            pieceDownKing = hitDown.transform.gameObject;
        }
        else
        {
            pieceDownKing = null;
        }
    }
    private void GetGameObjectLeftKing(GameObject piece)
    {
        pieceLeftKing = null;

        Ray rayLeft = new Ray(piece.transform.position, piece.transform.right * -directionPiece);
        RaycastHit hitLeft;
        if (Physics.Raycast(rayLeft, out hitLeft, 10f))
        {
            pieceLeftKing = hitLeft.transform.gameObject;
        }
        else
        {
            pieceLeftKing = null;
        }
    }
    private void GetGameObjectRightKing(GameObject piece)
    {
        pieceRightKing = null;

        Ray rayRight = new Ray(piece.transform.position, piece.transform.right * directionPiece);
        RaycastHit hitRight;
        if (Physics.Raycast(rayRight, out hitRight, 10f))
        {
            pieceRightKing = hitRight.transform.gameObject;
        }
        else
        {
            pieceRightKing = null;
        }
    }
    private void GetGameObjectUpLeftKing(GameObject piece)
    {
        pieceUpLeftKing = null;

        Ray rayUp = new Ray(piece.transform.position, piece.transform.forward * directionPiece);
        RaycastHit hitUp;
        if (Physics.Raycast(rayUp, out hitUp, 10f))
        {
            Ray rayLeft = new Ray(hitUp.transform.position, hitUp.transform.right * -directionPiece);
            RaycastHit hitLeft;
            if (Physics.Raycast(rayLeft, out hitLeft, 10f))
            {
                pieceUpLeftKing = hitLeft.transform.gameObject;
            }
            else
            {
                pieceUpLeftKing = null;
            }
        }
        else
        {
            pieceUpLeftKing = null;
        }
    }
    private void GetGameObjectUpRightKing(GameObject piece)
    {
        pieceUpRightKing = null;

        Ray rayUp = new Ray(piece.transform.position, piece.transform.forward * directionPiece);
        RaycastHit hitUp;
        if (Physics.Raycast(rayUp, out hitUp, 10f))
        {
            Ray rayRight = new Ray(hitUp.transform.position, hitUp.transform.right * directionPiece);
            RaycastHit hitRight;
            if (Physics.Raycast(rayRight, out hitRight, 10f))
            {
                pieceUpRightKing = hitRight.transform.gameObject;
            }
            else
            {
                pieceUpRightKing = null;
            }
        }
        else
        {
            pieceUpRightKing = null;
        }
    }
    private void GetGameObjectDownLeftKing(GameObject piece)
    {
        pieceDownLeftKing = null;

        Ray rayDown = new Ray(piece.transform.position, piece.transform.forward * -directionPiece);
        RaycastHit hitDown;
        if (Physics.Raycast(rayDown, out hitDown, 10f))
        {
            Ray rayLeft = new Ray(hitDown.transform.position, hitDown.transform.right * -directionPiece);
            RaycastHit hitLeft;
            if (Physics.Raycast(rayLeft, out hitLeft, 10f))
            {
                pieceDownLeftKing = hitLeft.transform.gameObject;
            }
            else
            {
                pieceDownLeftKing = null;
            }
        }
        else
        {
            pieceDownLeftKing = null;
        }
    }
    private void GetGameObjectDownRightKing(GameObject piece)
    {
        pieceDownRightKing = null;

        Ray rayDown = new Ray(piece.transform.position, piece.transform.forward * -directionPiece);
        RaycastHit hitDown;
        if (Physics.Raycast(rayDown, out hitDown, 10f))
        {
            Ray rayRight = new Ray(hitDown.transform.position, hitDown.transform.right * directionPiece);
            RaycastHit hitRight;
            if (Physics.Raycast(rayRight, out hitRight, 10f))
            {
                pieceDownRightKing = hitRight.transform.gameObject;
            }
            else
            {
                pieceDownRightKing = null;
            }
        }
        else
        {
            pieceDownRightKing = null;
        }
    }
    #endregion

    #region Check or CheckMate
    public void CheckThreatKing()
    {
        inCheck = false;

        GetGameObjectAroundKing(pieceKing);

        int i = 0;

        while (i < 8)
        {
            //UP
            if (pieceUpKing != null)
            {
                if (pieceUpKing.CompareTag("Chessboard"))
                {
                    GetGameObjectUpKing(pieceUpKing);
                }
                else if (pieceUpKing.CompareTag(enemyTag))
                {
                    if (pieceUpKing.GetComponent<RookPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceUpKing.GetComponent<QueenPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceUpKing.GetComponent<KingPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }

                    pieceUpKing = null;
                }
                else if (pieceUpKing.CompareTag(playerTag))
                {
                    pieceUpKing = null;
                }
            }
            //DOWN
            if (pieceDownKing != null)
            {
                if (pieceDownKing.CompareTag("Chessboard"))
                {
                    GetGameObjectDownKing(pieceDownKing);
                }
                else if (pieceDownKing.CompareTag(enemyTag))
                {
                    if (pieceDownKing.GetComponent<RookPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceDownKing.GetComponent<QueenPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceDownKing.GetComponent<KingPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }

                    pieceDownKing = null;
                }
                else if (pieceDownKing.CompareTag(playerTag))
                {
                    pieceDownKing = null;
                }
            }
            //LEFT
            if (pieceLeftKing != null)
            {
                if (pieceLeftKing.CompareTag("Chessboard"))
                {
                    GetGameObjectLeftKing(pieceLeftKing);
                }
                else if (pieceLeftKing.CompareTag(enemyTag))
                {
                    if (pieceLeftKing.GetComponent<RookPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceLeftKing.GetComponent<QueenPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceLeftKing.GetComponent<KingPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }

                    pieceLeftKing = null;
                }
                else if (pieceLeftKing.CompareTag(playerTag))
                {
                    pieceLeftKing = null;
                }
            }
            //RIGHT
            if (pieceRightKing != null)
            {
                if (pieceRightKing.CompareTag("Chessboard"))
                {
                    GetGameObjectRightKing(pieceRightKing);
                }
                else if (pieceRightKing.CompareTag(enemyTag))
                {
                    if (pieceRightKing.GetComponent<RookPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceRightKing.GetComponent<QueenPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceRightKing.GetComponent<KingPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }

                    pieceRightKing = null;
                }
                else if (pieceRightKing.CompareTag(playerTag))
                {
                    pieceRightKing = null;
                }
            }
            //UP-LEFT
            if (pieceUpLeftKing != null)
            {
                if (pieceUpLeftKing.CompareTag("Chessboard"))
                {
                    GetGameObjectUpLeftKing(pieceUpLeftKing);
                }
                else if (this.CompareTag("Player1") && pieceUpLeftKing.CompareTag(enemyTag))
                {
                    if (pieceUpLeftKing.GetComponent<BishopPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceUpLeftKing.GetComponent<QueenPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceUpLeftKing.GetComponent<KingPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }
                    else if (pieceUpLeftKing.GetComponent<PawnPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }

                    pieceUpLeftKing = null;
                }
                else if(this.CompareTag("Player1") && pieceUpLeftKing.CompareTag(playerTag))
                {
                    pieceUpLeftKing = null;
                }
                else if (this.CompareTag("Player2") && pieceUpLeftKing.CompareTag(enemyTag))
                {
                    if (pieceUpLeftKing.GetComponent<BishopPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceUpLeftKing.GetComponent<QueenPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceUpLeftKing.GetComponent<KingPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }

                    pieceUpLeftKing = null;
                }
                else if (this.CompareTag("Player2") && pieceUpLeftKing.CompareTag(playerTag))
                {
                    pieceUpLeftKing = null;
                }
            }
            //UP-RIGHT
            if (pieceUpRightKing != null)
            {
                if (pieceUpRightKing.CompareTag("Chessboard"))
                {
                    GetGameObjectUpRightKing(pieceUpRightKing);
                }
                else if (this.CompareTag("Player1") && pieceUpRightKing.CompareTag(enemyTag))
                {
                    if (pieceUpRightKing.GetComponent<BishopPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceUpRightKing.GetComponent<QueenPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceUpRightKing.GetComponent<KingPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }
                    else if (pieceUpRightKing.GetComponent<PawnPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }

                    pieceUpRightKing = null;
                }
                else if (this.CompareTag("Player1") && pieceUpRightKing.CompareTag(playerTag))
                {
                    pieceUpRightKing = null;
                }
                else if (this.CompareTag("Player2") && pieceUpRightKing.CompareTag(enemyTag))
                {
                    if (pieceUpRightKing.GetComponent<BishopPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceUpRightKing.GetComponent<QueenPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceUpRightKing.GetComponent<KingPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }

                    pieceUpRightKing = null;
                }
                else if (this.CompareTag("Player2") && pieceUpRightKing.CompareTag(playerTag))
                {
                    pieceUpRightKing = null;
                }
            }
            //DOWN-LEFT
            if (pieceDownLeftKing != null)
            {
                if (pieceDownLeftKing.CompareTag("Chessboard"))
                {
                    GetGameObjectDownLeftKing(pieceDownLeftKing);
                }
                else if (this.CompareTag("Player1") && pieceDownLeftKing.CompareTag(enemyTag))
                {
                    if (pieceDownLeftKing.GetComponent<BishopPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceDownLeftKing.GetComponent<QueenPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceDownLeftKing.GetComponent<KingPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }

                    pieceDownLeftKing = null;
                }
                else if (this.CompareTag("Player1") && pieceDownLeftKing.CompareTag(playerTag))
                {
                    pieceDownLeftKing = null;
                }
                else if (this.CompareTag("Player2") && pieceDownLeftKing.CompareTag(enemyTag))
                {
                    if (pieceDownLeftKing.GetComponent<BishopPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceDownLeftKing.GetComponent<QueenPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceDownLeftKing.GetComponent<KingPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }
                    else if (pieceDownLeftKing.GetComponent<PawnPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }

                    pieceDownLeftKing = null;
                }
                else if (this.CompareTag("Player2") && pieceDownLeftKing.CompareTag(playerTag))
                {
                    pieceDownLeftKing = null;
                }
            }
            //DOWN-RIGHT
            if (pieceDownRightKing != null)
            {
                if (pieceDownRightKing.CompareTag("Chessboard"))
                {
                    GetGameObjectDownRightKing(pieceDownRightKing);
                }
                else if (this.CompareTag("Player1") && pieceDownRightKing.CompareTag(enemyTag))
                {
                    if (pieceDownRightKing.GetComponent<BishopPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceDownRightKing.GetComponent<QueenPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceDownRightKing.GetComponent<KingPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }

                    pieceDownRightKing = null;
                }
                else if (this.CompareTag("Player1") && pieceDownRightKing.CompareTag(playerTag))
                {
                    pieceDownRightKing = null;
                }
                else if (this.CompareTag("Player2") && pieceDownRightKing.CompareTag(enemyTag))
                {
                    if (pieceDownRightKing.GetComponent<BishopPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceDownRightKing.GetComponent<QueenPiece>() != null)
                    {
                        inCheck = true;
                    }
                    else if (pieceDownRightKing.GetComponent<KingPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }
                    else if (pieceDownRightKing.GetComponent<PawnPiece>() != null && i == 0)
                    {
                        inCheck = true;
                    }

                    pieceDownRightKing = null;
                }
                else if (this.CompareTag("Player2") && pieceDownRightKing.CompareTag(playerTag))
                {
                    pieceDownRightKing = null;
                }
            }

            i++;
        }

        KnightThreat(pieceKing);

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
        pieceUp = null;
        pieceDown = null;
        pieceLeft = null;
        pieceRight = null;

        GetGameObjectUp(piece);
        if (pieceUp != null)
        {
            GetGameObjectUp(pieceUp);
            if (pieceUp != null)
            {
                GetGameObjectLeft(pieceUp);
                if (pieceLeft != null)
                {
                    if (pieceLeft.GetComponent<KnightPiece>() != null)
                    {
                        if (this.CompareTag("Player1") && pieceLeft.CompareTag("Player2"))
                        {
                            inCheck = true;
                        }
                        else if (this.CompareTag("Player2") && pieceLeft.CompareTag("Player1"))
                        {
                            inCheck = true;
                        }
                    }
                }
            }
        }
    }
    private void KnightThreatCASE2(GameObject piece)
    {
        pieceUp = null;
        pieceDown = null;
        pieceLeft = null;
        pieceRight = null;

        GetGameObjectUp(piece);
        if (pieceUp != null)
        {
            GetGameObjectUp(pieceUp);
            if (pieceUp != null)
            {
                GetGameObjectRight(pieceUp);
                if (pieceRight != null)
                {
                    if (pieceRight.GetComponent<KnightPiece>() != null)
                    {
                        if (this.CompareTag("Player1") && pieceRight.CompareTag("Player2"))
                        {
                            inCheck = true;
                        }
                        else if (this.CompareTag("Player2") && pieceRight.CompareTag("Player1"))
                        {
                            inCheck = true;
                        }
                    }
                }
            }
        }
    }
    private void KnightThreatCASE3(GameObject piece)
    {
        pieceUp = null;
        pieceDown = null;
        pieceLeft = null;
        pieceRight = null;

        GetGameObjectDown(piece);
        if (pieceDown != null)
        {
            GetGameObjectDown(pieceDown);
            if (pieceDown != null)
            {
                GetGameObjectLeft(pieceDown);
                if (pieceLeft != null)
                {
                    if (pieceLeft.GetComponent<KnightPiece>() != null)
                    {
                        if (this.CompareTag("Player1") && pieceLeft.CompareTag("Player2"))
                        {
                            inCheck = true;
                        }
                        else if (this.CompareTag("Player2") && pieceLeft.CompareTag("Player1"))
                        {
                            inCheck = true;
                        }
                    }
                }
            }
        }
    }
    private void KnightThreatCASE4(GameObject piece)
    {
        pieceUp = null;
        pieceDown = null;
        pieceLeft = null;
        pieceRight = null;

        GetGameObjectDown(piece);
        if (pieceDown != null)
        {
            GetGameObjectDown(pieceDown);
            if (pieceDown != null)
            {
                GetGameObjectRight(pieceDown);
                if (pieceRight != null)
                {
                    if (pieceRight.GetComponent<KnightPiece>() != null)
                    {
                        if (this.CompareTag("Player1") && pieceRight.CompareTag("Player2"))
                        {
                            inCheck = true;
                        }
                        else if (this.CompareTag("Player2") && pieceRight.CompareTag("Player1"))
                        {
                            inCheck = true;
                        }
                    }
                }
            }
        }
    }
    private void KnightThreatCASE5(GameObject piece)
    {
        pieceUp = null;
        pieceDown = null;
        pieceLeft = null;
        pieceRight = null;

        GetGameObjectLeft(piece);
        if (pieceLeft != null)
        {
            GetGameObjectLeft(pieceLeft);
            if (pieceLeft != null)
            {
                GetGameObjectUp(pieceLeft);
                if (pieceUp != null)
                {
                    if (pieceUp.GetComponent<KnightPiece>() != null)
                    {
                        if (this.CompareTag("Player1") && pieceUp.CompareTag("Player2"))
                        {
                            inCheck = true;
                        }
                        else if (this.CompareTag("Player2") && pieceUp.CompareTag("Player1"))
                        {
                            inCheck = true;
                        }
                    }
                }
            }
        }
    }
    private void KnightThreatCASE6(GameObject piece)
    {
        pieceUp = null;
        pieceDown = null;
        pieceLeft = null;
        pieceRight = null;

        GetGameObjectLeft(piece);
        if (pieceLeft != null)
        {
            GetGameObjectLeft(pieceLeft);
            if (pieceLeft != null)
            {
                GetGameObjectDown(pieceLeft);
                if (pieceDown != null)
                {
                    if (pieceDown.GetComponent<KnightPiece>() != null)
                    {
                        if (this.CompareTag("Player1") && pieceDown.CompareTag("Player2"))
                        {
                            inCheck = true;
                        }
                        else if (this.CompareTag("Player2") && pieceDown.CompareTag("Player1"))
                        {
                            inCheck = true;
                        }
                    }
                }
            }
        }
    }
    private void KnightThreatCASE7(GameObject piece)
    {
        pieceUp = null;
        pieceDown = null;
        pieceLeft = null;
        pieceRight = null;

        GetGameObjectRight(piece);
        if (pieceRight != null)
        {
            GetGameObjectRight(pieceRight);
            if (pieceRight != null)
            {
                GetGameObjectUp(pieceRight);
                if (pieceUp != null)
                {
                    if (pieceUp.GetComponent<KnightPiece>() != null)
                    {
                        if (this.CompareTag("Player1") && pieceUp.CompareTag("Player2"))
                        {
                            inCheck = true;
                        }
                        else if (this.CompareTag("Player2") && pieceUp.CompareTag("Player1"))
                        {
                            inCheck = true;
                        }
                    }
                }
            }
        }
    }
    private void KnightThreatCASE8(GameObject piece)
    {
        pieceUp = null;
        pieceDown = null;
        pieceLeft = null;
        pieceRight = null;

        GetGameObjectRight(piece);
        if (pieceRight != null)
        {
            GetGameObjectRight(pieceRight);
            if (pieceRight != null)
            {
                GetGameObjectDown(pieceRight);
                if (pieceDown != null)
                {
                    if (pieceDown.GetComponent<KnightPiece>() != null)
                    {
                        if (this.CompareTag("Player1") && pieceDown.CompareTag("Player2"))
                        {
                            inCheck = true;
                        }
                        else if (this.CompareTag("Player2") && pieceDown.CompareTag("Player1"))
                        {
                            inCheck = true;
                        }
                    }
                }
            }
        }
    }
    #endregion
}
