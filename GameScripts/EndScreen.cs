using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] protected Image blackScreen;
    [SerializeField] protected TextMeshProUGUI txt;
    public virtual void Enable()
    {
        StartCoroutine(ShowScreen());
    }

    protected virtual IEnumerator ShowScreen()
    {
        float showRate = 0.05f;

        while (blackScreen.color.a < 1)
        {
            blackScreen.color += new Color(0, 0, 0, showRate);
            yield return new WaitForSeconds(showRate);
        }

        txt.gameObject.SetActive(true);
    }
}
