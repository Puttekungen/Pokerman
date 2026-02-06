using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class TitleScreen : MonoBehaviour
{
    public AudioSource confirmSound;
    public TextMeshProUGUI pressZText;
    public BlinkText blinkText;

    private bool pressed = false;

    void Update()
    {
        if (pressed) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            pressed = true;
            StartCoroutine(Continue());
        }
    }

    IEnumerator Continue()
    {
        if (confirmSound != null)
            confirmSound.PlayOneShot(confirmSound.clip);


        if (blinkText != null)
            blinkText.StopBlinking();

        pressZText.enabled = false;

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("MainMenu");
    }
}
