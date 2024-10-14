using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeSlime : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Resize());
    }

    private IEnumerator Resize()
    {
        float resizeCooldown = 0.05f;
        float resizeAmount = 0.2f;
        int resizeTimes = 10;
        float maxSizeCooldown = 0.25f;

        for (int i = 0; i < resizeTimes; i++)
        {
            gameObject.transform.localScale += new Vector3(resizeAmount, resizeAmount, 1);
            yield return new WaitForSeconds(resizeCooldown);
        }

        yield return new WaitForSeconds(maxSizeCooldown);

        for (int i = 0; i < resizeTimes; i++)
        {
            gameObject.transform.localScale -= new Vector3(resizeAmount, resizeAmount, 1);
            yield return new WaitForSeconds(resizeCooldown);
        }
        StartCoroutine(Resize());
    }
}
