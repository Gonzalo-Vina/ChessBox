using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    float globalTimeElapsed;
    int minutes, seconds;

    [Header("Players")]
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private PlayerController player1Script;
    [SerializeField] private PlayerController player2Script;

    [Header("UI")]
    [SerializeField] private GameObject UI_MenuPause;
    public bool inMenuPause;
    [SerializeField] private GameObject UI_TurnBlack;
    [SerializeField] private GameObject UI_TurnWhite;
    [SerializeField] private GameObject UI_GameOver;
    [SerializeField] private GameObject UI_GameOver_WinWhite;
    [SerializeField] private GameObject UI_GameOver_WinBlack;
    [SerializeField] private GameObject UI_CrowningPawn;
    [SerializeField] private GameObject UI_BlockPanel;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image[] deadPiecesBlack;
    [SerializeField] private Sprite blackPawn;
    [SerializeField] private Sprite blackRook;
    [SerializeField] private Sprite blackKnight;
    [SerializeField] private Sprite blackBishop;
    [SerializeField] private Sprite blackQueen;
    [SerializeField] private Image[] deadPiecesWhite;
    [SerializeField] private Sprite whitePawn;
    [SerializeField] private Sprite whiteRook;
    [SerializeField] private Sprite whiteKnight;
    [SerializeField] private Sprite whiteBishop;
    [SerializeField] private Sprite whiteQueen;
    [SerializeField] private Slider sliderVolumen;

    void Awake()
    {
        Application.targetFrameRate = 60;
        Time.timeScale = 1;

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        player1Script = player1.GetComponent<PlayerController>();
        player2Script = player2.GetComponent<PlayerController>();
    }

    private void Start()
    {
        player1.SetActive(true);
        player2.SetActive(false);

        StartCoroutine(FirstUpdateBoxEmpty());

        UI_MenuPause.SetActive(false);
        UI_GameOver.SetActive(false);
        UI_TurnBlack.SetActive(false);
        UI_TurnWhite.SetActive(true);
        UI_BlockPanel.SetActive(false);
        UI_CrowningPawn.SetActive(false);
        DisableDeadPieces();

        sliderVolumen.value = AudioManager.Instance.GetVolumenAudio();
    }
    
    void Update()
    {
        globalTimeElapsed += Time.deltaTime;
        if (inMenuPause && !UI_GameOver.activeSelf) { TimerController(); }
        
        TurnController();
        if (Input.GetKeyDown(KeyCode.Escape) && !UI_GameOver.activeSelf) { MenuPauseActive();}
    }

    private void TurnController()
    {
        if (!player1Script.myTurn)
        {
            UI_TurnWhite.SetActive(false);
            player1.SetActive(false);
            player2.SetActive(true);
            UI_TurnBlack.SetActive(true);
            player2Script.UpdateBoxEmpty();
        }

        if (!player2Script.myTurn)
        {
            UI_TurnBlack.SetActive(false);
            player2.SetActive(false);
            player1.SetActive(true);
            UI_TurnWhite.SetActive(true);
            player1Script.UpdateBoxEmpty();
        }
    }
    public void MenuPauseActive()
    {
        if (UI_MenuPause.activeSelf)
        {
            UI_MenuPause.SetActive(false);
            inMenuPause = false;
        }
        else
        {
            inMenuPause = true;
            UI_MenuPause.SetActive(true);
        }
    }
    private void TimerController()
    {
        minutes = (int)(globalTimeElapsed / 60f);
        seconds = (int)(globalTimeElapsed - minutes * 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void SurrenderController()
    {
        UI_MenuPause.SetActive(false);
        UI_GameOver.SetActive(true);

        UI_GameOver_WinWhite.SetActive(false);
        UI_GameOver_WinBlack.SetActive(false);

        if (player1Script.myTurn)
        {
            player1Script.inCheckMate = true;
            UI_GameOver_WinBlack.SetActive(true);
        }
        else if (player2Script.myTurn)
        {
            player2Script.inCheckMate = true;
            UI_GameOver_WinWhite.SetActive(true);
        }
    }
    public void SetVolumenGlobal(float value)
    {
        AudioManager.Instance.SetVolumen(value);
    }
    public void LoadSceneMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    private void DisableDeadPieces()
    {
        for (int i = 0; i < deadPiecesBlack.Length; i++)
        {
            deadPiecesBlack[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < deadPiecesWhite.Length; i++)
        {
            deadPiecesWhite[i].gameObject.SetActive(false);
        }
    }
    public void AddDeadPieceBlack(GameObject pieceEnemy)
    {
        for (int i = 0; i < deadPiecesBlack.Length; i++)
        {
            if(!deadPiecesBlack[i].gameObject.activeSelf && pieceEnemy != null)
            {
                switch (pieceEnemy.name)
                {
                    case "Pawn":
                        deadPiecesBlack[i].gameObject.SetActive(true);
                        deadPiecesBlack[i].sprite = blackPawn;
                        break;
                    case "Rook":
                        deadPiecesBlack[i].gameObject.SetActive(true);
                        deadPiecesBlack[i].sprite = blackRook;
                        break;
                    case "Knight":
                        deadPiecesBlack[i].gameObject.SetActive(true);
                        deadPiecesBlack[i].sprite = blackKnight;
                        break;
                    case "Bishop":
                        deadPiecesBlack[i].gameObject.SetActive(true);
                        deadPiecesBlack[i].sprite = blackBishop;
                        break;
                    case "Queen":
                        deadPiecesBlack[i].gameObject.SetActive(true);
                        deadPiecesBlack[i].sprite = blackQueen;
                        break;
                    default:
                        break;
                }
                i = deadPiecesBlack.Length;
            }
        }
    }
    public void AddDeadPieceWhite(GameObject pieceEnemy)
    {
        for (int i = 0; i < deadPiecesWhite.Length; i++)
        {
            if (!deadPiecesWhite[i].gameObject.activeSelf && pieceEnemy != null)
            {
                switch (pieceEnemy.name)
                {
                    case "Pawn":
                        deadPiecesWhite[i].gameObject.SetActive(true);
                        deadPiecesWhite[i].sprite = whitePawn;
                        break;
                    case "Rook":
                        deadPiecesWhite[i].gameObject.SetActive(true);
                        deadPiecesWhite[i].sprite = whiteRook;
                        break;
                    case "Knight":
                        deadPiecesWhite[i].gameObject.SetActive(true);
                        deadPiecesWhite[i].sprite = whiteKnight;
                        break;
                    case "Bishop":
                        deadPiecesWhite[i].gameObject.SetActive(true);
                        deadPiecesWhite[i].sprite = whiteBishop;
                        break;
                    case "Queen":
                        deadPiecesWhite[i].gameObject.SetActive(true);
                        deadPiecesWhite[i].sprite = whiteQueen;
                        break;
                    default:
                        break;
                }

                i = deadPiecesBlack.Length;
            }
        }
    }
    public void CrowningPawnLogic(string crownTo)
    {
        if (player1.activeSelf)
        {
            StartCoroutine(player1Script.CrowningPawnDownLogic(crownTo));
        }
        else
        {
            StartCoroutine(player2Script.CrowningPawnDownLogic(crownTo));
        }
    }

    private IEnumerator FirstUpdateBoxEmpty()
    {
        yield return new WaitForSeconds(0.5f);
        player1Script.UpdateBoxEmpty();
    }
}
