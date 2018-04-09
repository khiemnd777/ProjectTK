using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseAmountMotion : MonoBehaviour
{
    Text _text;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    public IEnumerator Reduce(Transform target, int amount)
    {
        _text.text = "+" + amount.ToString();
        var percent = 0f;
        var srcPos = target.position;
        var destPos = Random.insideUnitCircle * Random.Range(1, 2) + new Vector2(target.position.x, target.position.y);
        var trajectoryHeight = Random.Range(.2f, .6f);
        while (percent <= 1f)
        {
            percent += Time.deltaTime * 2.5f;
            var currentPos = Vector2.Lerp(srcPos, destPos, percent);
            currentPos.y += trajectoryHeight * Mathf.Sin(Mathf.Clamp01(percent) * Mathf.PI);
            transform.position = currentPos;
            yield return null;
        }
        Destroy(gameObject);
    }
}
