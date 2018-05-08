using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeScript {
    private bool fade;
    public static bool Fading
    {
        get; set;
    }
    public static IEnumerator FadeOut(Image img, string LevelName)
    {
        Fading = true;
        float time = 0.0f;
        while (time < 1.0f)
        {
            yield return new WaitForEndOfFrame();
            time = Mathf.Clamp01(time + Time.deltaTime / 1.5f);
            var temp = img.color;
            temp.a = time;
            img.color = temp;
        }
        if (!string.IsNullOrEmpty(LevelName))
        {
            SceneManager.LoadScene(LevelName);
        }
        Fading = false;
    }

    public static IEnumerator FadeIn(Image img)
    {
        Fading = true;
        float time = 1.0f;
        while (time > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            time = Mathf.Clamp01(time - Time.deltaTime / 1.5f);
            var temp = img.color;
            temp.a = time;
            img.color = temp;
        }
        Fading = false;
    }
}
