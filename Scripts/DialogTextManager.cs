using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class DialogTextManager : MonoBehaviour
{
    [SerializeField] private UnityEvent onCompletedEvents = new UnityEngine.Events.UnityEvent();
    [SerializeField] float eventDelayTime;
    bool isEnd;

    public UnityAction onClickText;
    public string[] scenarios;
    [SerializeField] Text uiText;
    [SerializeField]
    [Range(0.001f, 0.3f)]
    float intervalForCharacterDisplay = 0.05f;

    private string currentText = string.Empty;
    private float timeUntilDisplay = 0;
    private float timeElapsed = 1;
    private int currentLine = 0;
    private int lastUpdateCharacter = -1;

    public static DialogTextManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public bool IsCompleteDisplayText
    {
        get { return Time.time > timeElapsed + timeUntilDisplay; }
    }

    void Update()
    {
        if (IsCompleteDisplayText)
        {
            if (currentLine < scenarios.Length && Input.GetMouseButtonDown(0))
            {
                SetNextLine();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                timeUntilDisplay = 0;
            }
        }

        int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);
        if (displayCharacterCount != lastUpdateCharacter)
        {
            uiText.text = currentText.Substring(0, displayCharacterCount);
            lastUpdateCharacter = displayCharacterCount;
        }
        CheckCompletedText();
    }

    
    void CheckCompletedText()
    {
        if (isEnd == false && IsCompleteDisplayText && scenarios.Length == currentLine)
        {
            isEnd = true;
            Invoke("EventFunction", eventDelayTime);
        }
    }
    
    void EventFunction()
    {
        onCompletedEvents.Invoke();
    }


    public void SetNextLine()
    {
        isEnd = false;
        if (scenarios.Length - 1 < currentLine)
        {
            return;
        }
        currentText = scenarios[currentLine];
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;
        currentLine++;
        lastUpdateCharacter = -1;
    }
  
    public void SetScenarios(string[] sc)
    {
        scenarios = sc;
        currentLine = 0;
        SetNextLine();
    }
}