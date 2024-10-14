using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinSpin : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Spin());
    }

    private IEnumerator Spin()
    {
        float spinCooldown = 0.05f;
        float spinAmount = 5f; // In degrees

        gameObject.transform.Rotate(new Vector3(0, 0, spinAmount));
        yield return new WaitForSeconds(spinCooldown);

        StartCoroutine(Spin());
    }
}
