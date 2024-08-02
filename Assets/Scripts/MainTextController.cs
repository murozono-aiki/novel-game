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

    List<string> statements = new();  // 次の行を表示する際に実行する命令のリスト
    int choiceStatementLineNumber = -1;  // 選択肢を表示しなければならない場合、１つ目の選択肢がある行番号（選択肢がなければ-1）
    int displayedSentenceLength;
    int sentenceLength;
    float time = 0f;
    readonly float feedTime = 0.05f;  // １文字表示するのにかける時間
    bool canGoToTheNextLine = true;  // 現在の行の全ての文字が表示され、次の行へ進むことが可能になったかどうか
    
    void Start()
    {
        scenarioManager = gameManager.scenarioManager;
        
        // 画面の横幅によって文字サイズを変更する
        float screenWidth = Screen.width;
        mainText.fontSize = fontSize_fullHD * (screenWidth / 1920);
        
        gameManager.lineNumber = -1;  // 次にGoToTheNextLineを実行することで0になる
        GoToTheNextLine();
        DisplayNextLine();
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
                    if (choiceStatementLineNumber >= 0)
                    {
                        scenarioManager.ExecuteChoiceStatement(choiceStatementLineNumber);  // 選択肢がある場合は選択肢を表示
                    }
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
                    DisplayNextLine();
                }
                else
                {
                    displayedSentenceLength = sentenceLength;
                }
            }
        }
    }

    // 次の行の内容を表示する際の処理
    public void DisplayNextLine()
    {
        ExecuteStatements();
        DisplayText();
        GoToTheNextLine();
    }

    // 次の行へ移動
    public void GoToTheNextLine()
    {
        choiceStatementLineNumber = -1;  // 選択肢がない場合は-1

        int currentIndent = scenarioManager.GetIndent(scenarioManager.GetCurrentSentence());
        gameManager.lineNumber++;
        string sentence = scenarioManager.GetCurrentSentence();
        int nextLineIndent = scenarioManager.GetIndent(sentence);
        /*
            以下のような状態にあるときに、「次の文」まで飛ばす処理（「分岐点の合流」）
            &choice 選択肢x
                ・・・
                現在の文←今ここ
            &choice 選択肢y
                ・・・
            &choice 選択肢z
                ・・・
            次の文←ここまで飛ばす
        */
        if (nextLineIndent < currentIndent && scenarioManager.IsChoiceStatement(sentence))
        {
            while (true)
            {
                gameManager.lineNumber++;
                sentence = scenarioManager.GetCurrentSentence();
                if (sentence == "") break;  // もし最終行を過ぎた場合はループを抜ける（無限ループ防止）
                if (scenarioManager.GetIndent(sentence) <= nextLineIndent && !scenarioManager.IsChoiceStatement(sentence)) break;
            }
        }
        // 選択肢を表示する場合を除き、gameManager.lineNumberが命令でない行に到達するまで次の行に進む
        if (scenarioManager.IsStatement(sentence))
        {
            if (scenarioManager.IsChoiceStatement(sentence))
            {
                choiceStatementLineNumber = gameManager.lineNumber;  // １つ目の選択肢があるlineNumberを記録して終了
            }
            else if (scenarioManager.IsJumpStatement(sentence))
            {
                // ジャンプ先までlineNumberを移動させてから次の行に進む
                scenarioManager.ExecuteJumpStatement(sentence);
                GoToTheNextLine();
            }
            else  // 特殊な命令でない場合
            {
                // 命令を記録してから次の行に進む
                statements.Add(sentence);
                GoToTheNextLine();
            }
        }
    }

    // statementsリストにある命令を全て実行
    public void ExecuteStatements()
    {
        for (int i = 0; i < statements.Count; i++)
        {
            scenarioManager.ExecuteStatement(statements[i]);
        }
        statements.Clear();
    }

    // テキストを表示
    public void DisplayText()
    {
        // 各値を初期化
        displayedSentenceLength = 0;
        time = 0f;
        mainText.maxVisibleCharacters = 0;
        canGoToTheNextLine = false;

        string sentence = scenarioManager.NormalizeSentence(scenarioManager.GetCurrentSentence());
        mainText.text = sentence;
        sentenceLength = sentence.Length;
    }
}
