using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public static class TMPTextExtensions
{
    /// <summary>
    /// Uses the TextMeshScheduler to schedule text mesh updates in a more performant fashion.
    /// TextMeshScheduler must be a manager in the scene.
    /// </summary>
    public static void ScheduleText(this TMP_Text text, string newText)
    {
        // Note: best to replace this with reference so you don't invoke Find() every time
        TextMeshScheduler scheduler = Object.FindFirstObjectByType<TextMeshScheduler>();

        if (scheduler != null)
            scheduler.SetText(text, newText);
        else
            Debug.LogError("[TextMeshScheduler.ScheduleText] TextMeshScheduler must be a manager in the scene to use this extension method!");
    }
}

public class TextMeshScheduler : MonoBehaviour
{
    private readonly Dictionary<TMP_Text, string> textMeshUpdates = new(20);
    private Coroutine updateCoroutine = null;

    private void Awake()
    {
        if (updateCoroutine != null)
            StopCoroutine(updateCoroutine);

        updateCoroutine = StartCoroutine(ProcessUpdatesCo());
    }

    private void OnDestroy()
    {
        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
            updateCoroutine = null;
        }
    }

    /// <summary> Schedules update for the desired text value for a TMP_Text. </summary>
    public void SetText(TMP_Text textMesh, string newText)
    {
        if (textMesh == null)
            return;

        if (newText != textMesh.text)
            textMeshUpdates[textMesh] = newText;
        else
            textMeshUpdates.Remove(textMesh);
    }

    private IEnumerator ProcessUpdatesCo()
    {
        while (true)
        {
            if (textMeshUpdates.Count > 0)
            {
                var curTextMesh = textMeshUpdates.Keys.First();
                var newText = textMeshUpdates[curTextMesh];

                if (curTextMesh != null && curTextMesh.text != newText)
                    curTextMesh.SetText(newText);

                textMeshUpdates.Remove(curTextMesh);
            }

            yield return null;
        }
    }
}
