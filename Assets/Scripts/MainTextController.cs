using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainTextController : MonoBehaviour
{
    [SerializeField] GameManagerScript gameManager;
    ScenarioManager scenarioManager;

    [SerializeField] TextMeshProUGUI mainText;

    [SerializeField] float fontSize_fullHD;

    int displayedSentenceLength;
    int sentenceLength;
    float time = 0f;
    readonly float feedTime = 0.05f;
    bool canGoToTheNextLine = true;
    
    void Start()
    {
        scenarioManager = gameManager.scenarioManager;
        
        float screenWidth = Screen.width;
        mainText.fontSize = fontSize_fullHD * (screenWidth / 1920);
        
        gameManager.lineNumber = -1;  // 次にGoToTheNextLineを実行することで0になる
        GoToTheNextLine();
        DisplayText();
    }

    void Update()
    {
        // 文章を１文字ずつ表示する
        if (!canGoToTheNextLine) {
            time += Time.deltaTime;
            if (time >= feedTime)
            {
                time -= feedTime;

                displayedSentenceLength++;
                mainText.maxVisibleCharacters = displayedSentenceLength;
                if (displayedSentenceLength > sentenceLength)
                {
                    time = 0;
                    canGoToTheNextLine = true;  // 現在の行の全ての文字が表示され、次の行へ行くことが可能に
                    scenarioManager.ExecuteChoiceStatement();  // 選択肢がある場合は選択肢を表示
                }
            }
        }

        // クリックされたとき、選択肢がなければ、次の行へ移動
        if (!gameManager.isWaitingButtonClick)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (canGoToTheNextLine)
                {
                    GoToTheNextLine();
                    DisplayText();
                }
                else
                {
                    displayedSentenceLength = sentenceLength;
                }
            }
        }
    }

    // 次の行へ移動
    public void GoToTheNextLine()
    {
        displayedSentenceLength = 0;
        time = 0f;
        mainText.maxVisibleCharacters = 0;
        canGoToTheNextLine = false;
        int currentIndent = scenarioManager.GetIndent(scenarioManager.GetCurrentSentence());
        gameManager.lineNumber++;
        string sentence = scenarioManager.GetCurrentSentence();
        int nextLineIndent = scenarioManager.GetIndent(sentence);
        if (nextLineIndent < currentIndent && scenarioManager.IsChoiceStatement(sentence))
        {
            while (true)
            {
                gameManager.lineNumber++;
                sentence = scenarioManager.GetCurrentSentence();
                if (sentence == "") break;
                if (scenarioManager.GetIndent(sentence) <= nextLineIndent && !scenarioManager.IsChoiceStatement(sentence)) break;
            }
        }
        if (scenarioManager.IsStatement(sentence))
        {
            scenarioManager.ExecuteStatement(sentence);
            GoToTheNextLine();
        }
    }

    // テキストを表示
    public void DisplayText()
    {
        string sentence = scenarioManager.NormalizeSentence(scenarioManager.GetCurrentSentence());
        mainText.text = sentence;
        sentenceLength = sentence.Length;
    }
}
