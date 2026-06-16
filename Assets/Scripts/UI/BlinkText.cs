using System.Collections;
using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    public float interval = 0.5f;

    private TMP_Text _text;

    void Start()
    {
        _text = GetComponent<TMP_Text>();
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            Color c = _text.color;
            c.a = c.a > 0f ? 0f : 1f;
            _text.color = c;
            yield return new WaitForSecondsRealtime(interval);
        }
    }
}
