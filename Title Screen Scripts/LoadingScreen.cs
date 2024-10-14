using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playButtonText;
    [SerializeField] private Image blackScreen;
    [SerializeField] private TextMeshProUGUI loadingText;
    void Start()
    {
        if (PlayerPrefs.GetInt("Win") == 1)
        {
            playButtonText.text = "Play (again)";
        }
        else
        {
            playButtonText.text = "Play";
        }
    }

    public void StartLoadingGame()
    {
        Debug.Log("Loading in Slime Auto-Battler!");
        playButtonText.transform.parent.gameObject.SetActive(false);
        StartCoroutine(LoadGame());
    }

    public IEnumerator LoadGame()
    {
        while (blackScreen.color.a < 1)
        {
            blackScreen.color += new Color(0, 0, 0, 0.05f);
            yield return new WaitForSeconds(0.05f);
        }

        loadingText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("SampleScene");
    }
}
