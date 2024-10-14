using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreen : EndScreen
{
    public override void Enable()
    {
        base.Enable();
    }
    protected override IEnumerator ShowScreen()
    {
        float showRate = 0.05f;

        while (blackScreen.color.a < 1)
        {
            blackScreen.color += new Color(0, 0, 0, showRate);
            yield return new WaitForSeconds(showRate);
        }

        txt.gameObject.SetActive(true);

        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("SampleScene");
    }

}
