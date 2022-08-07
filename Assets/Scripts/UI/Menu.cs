using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Menu : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text txt;
    public bool onMenu = true;

    private void Start()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && onMenu)
        {
            // Check if the mouse was clicked over a UI element
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                StartAction();
            }
        }
        SliderAction();
        txt.text = slider.value.ToString();
    }

    private void SliderAction()
    {
        int sliderValue = (int)slider.value;
        for (int i = 10; i > sliderValue; i--)
        {
            GameManager.instance.players[i].gameObject.SetActive(false);
        }
        for (int i = 1; i <= sliderValue; i++)
        {
            GameManager.instance.players[i].gameObject.SetActive(true);
        }
    }

    void StartAction()
    {
        Time.timeScale = 1;
        GameManager.instance.ToggleMenuCanvas();
       // Destroy(gameObject);
    }
}
