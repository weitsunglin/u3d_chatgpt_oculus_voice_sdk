using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AskButtonManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private DiscussionManager discussionManager;
    [SerializeField] private STTBridge sttBridge;
    [SerializeField] private TMP_InputField promptInputField;

    [Header("Settings")]
    private bool recording;

    [Header("Graphics")]
    [SerializeField] private GameObject askText;
    [SerializeField] private GameObject micImage;

    void Start()
    {
        ShowMicImage();
        promptInputField.onValueChanged.AddListener(InputFieldValueChangedCallback);
    }

    void Update()
    {
        if (promptInputField && recording == false)
        {
            if (promptInputField.text.Length != 0)
            {
                ShowAskText();
            }
        }
    }

    public void PointerDownCallback()
    {
        if (promptInputField.text.Length > 0)
        {
            discussionManager.AskButtonCallBack();
        }
        else
        {
            sttBridge.SetActivation(true);
            recording = true;
        }
    }

    public void PointerUpCallback()
    {
        sttBridge.SetActivation(false);
        recording = false;
        InputFieldValueChangedCallback(promptInputField.text);
    }

    private void InputFieldValueChangedCallback(string prompt)
    {
        if (recording)
            return;

        if (prompt.Length <= 0)
        {
            ShowMicImage();
        }
        else
        {
            ShowAskText();
        }
    }

    private void ShowMicImage()
    {
        askText.SetActive(false);
        micImage.SetActive(true);
    }

    private void ShowAskText()
    {
        askText.SetActive(true);
        micImage.SetActive(false);
    }
}
