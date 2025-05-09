using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HalfDeadScreen : MonoBehaviour
{
    public static HalfDeadScreen Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private GameObject halfDeadScreenBrisa;
    [SerializeField] private TextMeshProUGUI halfDeadScreenTextBrisa;
    [SerializeField] private GameObject halfDeadScreenBestia;
    [SerializeField] private TextMeshProUGUI halfDeadScreenTextBestia;

    public void ShowHalfDeadScreenBrisa(string textTimer)
    {
        halfDeadScreenBrisa.SetActive(true);
        halfDeadScreenTextBrisa.text = float.Parse(textTimer).ToString("00");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideHalfDeadScreenBrisa()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        halfDeadScreenBrisa.SetActive(false);
    }

    public void ShowHalfDeadScreenBestia(string textTimer)
    {
        halfDeadScreenBestia.SetActive(true);
        halfDeadScreenTextBestia.text = float.Parse(textTimer).ToString("00");
    }

    public void HideHalfDeadScreenBestia()
    {
        halfDeadScreenBestia.SetActive(false);
    }
}
