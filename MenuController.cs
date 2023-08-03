using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] Animator animatorBackground;
    [SerializeField] Animator animatorPlay;
    [SerializeField] Animator animatorExit;
    [SerializeField] Animator animatorParlante;
    [SerializeField] Animator animatorVolumen;
    public void PlayButton()
    {
        StartCoroutine(LoadGameScene());
    }
    private IEnumerator LoadGameScene()
    {
        animatorBackground.Play("Move - Background");
        animatorExit.Play("Move - Exit");
        animatorParlante.Play("Move - Parlante");
        animatorPlay.Play("Move - Play");
        animatorVolumen.Play("Move - Volumen");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
