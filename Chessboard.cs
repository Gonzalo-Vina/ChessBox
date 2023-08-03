using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessboard : MonoBehaviour
{
    private const int TILE_COUNT_X = 8;
    private const int TILE_COUNT_Z = 8;

    //Chess Pieces
    [SerializeField] GameObject boxEmpty;
    [SerializeField] GameObject pawn;
    [SerializeField] GameObject knight;
    [SerializeField] GameObject bishop;
    [SerializeField] GameObject rook;
    [SerializeField] GameObject queen;
    [SerializeField] GameObject king;

    private float timeLerp = 1f;

    void Awake()
    {
        GenerateChessboard();
    }

    public GameObject GenerateBoxChessEmpty(Vector3 position)
    {
        GameObject chessBox = Instantiate(boxEmpty, position, Quaternion.identity);
        chessBox.name = position.x + ":" + position.z;
        chessBox.transform.parent = this.transform;

        return chessBox;
    }
    private void GenerateChessboard()
    {
        for (int x = 0; x < TILE_COUNT_X; x++)
        {
            for (int z = 0; z < TILE_COUNT_Z; z++)
            {
                GenerateFirtsChessBox(x,z);
            }
        }
    }
    private void GenerateFirtsChessBox(int x, int z)
    {
        //boxEmpty
        if(z>=2 && z <= 5)
        {
            GameObject chessBox = Instantiate(boxEmpty, new Vector3(x, 0, z), Quaternion.identity);
            chessBox.name = x + ":" + z;
            chessBox.transform.parent = this.transform;

            chessBox.transform.localScale = new Vector3(0, 0, 0);
            StartCoroutine(PopUpLerp(chessBox, timeLerp));
        }

        //Pawn
        if (z == 1 || z == 6)
        {
            GameObject chessBox = Instantiate(pawn, new Vector3(x, 0, z), Quaternion.identity);
            chessBox.name = "Pawn";
            chessBox.transform.parent = this.transform;

            chessBox.transform.localScale = new Vector3(0, 0, 0);
            StartCoroutine(PopUpLerp(chessBox, timeLerp));
        }

        //Rook
        if (x == 0 || x == 7)
        {
            if (z == 0 || z == 7)
            {
                GameObject chessBox = Instantiate(rook, new Vector3(x, 0, z), Quaternion.identity);
                chessBox.name = "Rook";
                chessBox.transform.parent = this.transform;

                chessBox.transform.localScale = new Vector3(0, 0, 0);
                StartCoroutine(PopUpLerp(chessBox, timeLerp));
            }
        }

        //KnightPiece
        if (x==1 || x == 6)
        {
            if(z==0 || z == 7)
            {
                GameObject chessBox = Instantiate(knight, new Vector3(x, 0, z), Quaternion.identity);
                chessBox.name = "Knight";
                chessBox.transform.parent = this.transform;

                chessBox.transform.localScale = new Vector3(0, 0, 0);
                StartCoroutine(PopUpLerp(chessBox, timeLerp));
            }
        }

        //Bishop
        if (x == 2 || x == 5)
        {
            if (z == 0 || z == 7)
            {
                GameObject chessBox = Instantiate(bishop, new Vector3(x, 0, z), Quaternion.identity);
                chessBox.name = "Bishop";
                chessBox.transform.parent = this.transform;

                chessBox.transform.localScale = new Vector3(0, 0, 0);
                StartCoroutine(PopUpLerp(chessBox, timeLerp));
            }
        }

        //Queen
        if (x == 3)
        {
            if (z == 0 || z == 7)
            {
                GameObject chessBox = Instantiate(queen, new Vector3(x, 0, z), Quaternion.identity);
                chessBox.name = "Queen";
                chessBox.transform.parent = this.transform;

                chessBox.transform.localScale = new Vector3(0, 0, 0);
                StartCoroutine(PopUpLerp(chessBox, timeLerp));
            }
        }

        //King
        if (x == 4)
        {
            if (z == 0 || z == 7)
            {
                GameObject chessBox = Instantiate(king, new Vector3(x, 0, z), Quaternion.identity);
                chessBox.name = "King";
                chessBox.transform.parent = this.transform;

                chessBox.transform.localScale = new Vector3(0, 0, 0);
                StartCoroutine(PopUpLerp(chessBox, timeLerp));
            }
        }
    }
    private IEnumerator PopUpLerp(GameObject target, float lerpDuration)
    {
        float timeElapsed1 = 0;

        while (timeElapsed1 < lerpDuration)
        {
            target.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), timeElapsed1 / lerpDuration);
            timeElapsed1 += Time.deltaTime;
            yield return null;
        }

        target.transform.localScale = new Vector3(1, 1, 1);
    }
    public GameObject GenerateQueen(Vector3 initialPosition)
    {
        GameObject chessBox = Instantiate(queen, initialPosition, Quaternion.identity);
        chessBox.name = "Queen";
        chessBox.transform.parent = this.transform;

        return chessBox;
    }
    public GameObject GenerateBishop(Vector3 initialPosition)
    {
        GameObject chessBox = Instantiate(bishop, initialPosition, Quaternion.identity);
        chessBox.name = "Bishop";
        chessBox.transform.parent = this.transform;

        return chessBox;
    }
    public GameObject GenerateKnight(Vector3 initialPosition)
    {
        GameObject chessBox = Instantiate(knight, initialPosition, Quaternion.identity);
        chessBox.name = "Knight";
        chessBox.transform.parent = this.transform;

        return chessBox;
    }
    public GameObject GenerateRook(Vector3 initialPosition)
    {
        GameObject chessBox = Instantiate(rook, initialPosition, Quaternion.identity);
        chessBox.name = "Rook";
        chessBox.transform.parent = this.transform;

        return chessBox;
    }
}
