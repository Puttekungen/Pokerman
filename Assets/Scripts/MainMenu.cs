using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public RectTransform cursor;
    public RectTransform[] options;

    private int currentIndex = 0;
    private bool inputLocked = false;

    void Start()
    {
        UpdateCursor();
    }

    void Update()
    {
        if (inputLocked) return;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex++;
            if (currentIndex >= options.Length)
                currentIndex = 0;

            UpdateCursor();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = options.Length - 1;

            UpdateCursor();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            SelectOption();
        }
    }

    void UpdateCursor()
    {
        cursor.anchoredPosition = new Vector2(
            cursor.anchoredPosition.x,
            options[currentIndex].anchoredPosition.y + 5f
        );
    }


    void SelectOption()
    {
        inputLocked = true;

        if (currentIndex == 0)
        {
            // START
            SceneManager.LoadScene("Game");
        }
        else if (currentIndex == 1)
        {
            // EXIT
            Application.Quit();
        }
    }
}
