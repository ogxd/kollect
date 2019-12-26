using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMover : MonoBehaviour
{
    public void MoveIn(RectTransform target) {
        StartCoroutine(MoveInCoroutine(target, 0.3f));
    }

    public void MoveOut(RectTransform target) {
        StartCoroutine(MoveOutCoroutine(target, 0.3f));
    }

    IEnumerator MoveInCoroutine(RectTransform target, float time) {
        float width = Screen.width;
        target.gameObject.SetActive(true);
        for (float t = 0f; t < time; t+= Time.deltaTime) {
            target.position = new Vector3(Mathf.Lerp(1.5f * width, 0.5f * width, Mathf.SmoothStep(0.0f, 1.0f, t / time)), target.position.y);
            yield return null;
        }
        target.position = new Vector3(0.5f * width, target.position.y);
    }

    IEnumerator MoveOutCoroutine(RectTransform target, float time) {
        float width = Screen.width;
        for (float t = 0f; t < time; t += Time.deltaTime) {
            target.position = new Vector3(Mathf.Lerp(0.5f * width, -0.5f * width, Mathf.SmoothStep(0.0f, 1.0f, t / time)), target.position.y);
            yield return null;
        }
        target.position = new Vector3(0.5f * width, target.position.y);
        target.gameObject.SetActive(false);
    }
}
