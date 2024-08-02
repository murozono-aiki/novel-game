using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

public class ScenarioManager : MonoBehaviour
{
    [SerializeField] GameManagerScript gameManager;

    [SerializeField] TextAsset textFile;

    // 文章中の文（ここでは１行ごと）を入れておくためのリスト
    List<string> sentences = new();
    // jumpのキーを保管する辞書
    Dictionary<string, int> jumpKey = new();

    readonly Regex statementRegex = new("^ *&");
    readonly Regex indentRegex = new("^ *");
    readonly Regex choiceStatementRegex = new("^ *&choice");

    void Awake()
    {
        // シナリオファイルの読み込み
        StringReader reader = new(textFile.text);
        Regex jumpKeyStatementRegex = new("^ *&jump key ");
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            sentences.Add(line);  // テキストファイルの中身を、１行ずつリストに入れておく
            if (jumpKeyStatementRegex.IsMatch(line))
            {
                string key = NormalizeSentence(line).Split(" ")[2];
                if (!jumpKey.ContainsKey(key))
                {
                    jumpKey.Add(key, sentences.Count - 1);
                }
            }
        }
    }

    // 現在の行の文を取得する
    public string GetCurrentSentence()
    {
        if (sentences.Count > gameManager.lineNumber && gameManager.lineNumber >= 0)
        {
            return sentences[gameManager.lineNumber];
        }
        else
        {
            return "";
        }
    }

    // インデントを削除する
    public string NormalizeSentence(string sentence)
    {
        return indentRegex.Replace(sentence, "");
    }

    // 文が命令かどうか
    public bool IsStatement(string sentence)
    {
        return statementRegex.IsMatch(sentence);
    }

    // 文が選択肢の命令かどうか
    public bool IsChoiceStatement(string sentence)
    {
        return choiceStatementRegex.IsMatch(sentence);
    }

    // 命令を実行する
    public void ExecuteStatement(string sentence)
    {
        string[] words = NormalizeSentence(sentence).Split(' ');
        switch(words[0])
        {
            case "&image":
                if (words.Length >= 3)  // 第２引数が存在する場合は画像の挿入
                {
                    gameManager.imageManager.PutImage(words[1], words[2]);
                }
                else if (words.Length >= 2)  // 第２引数が存在しない場合は画像の削除
                {
                    gameManager.imageManager.RemoveImage(words[1]);
                }
                break;
            case "&audio":
                if (words.Length >= 3)  // 第２引数が存在する場合は音声の再生
                {
                    gameManager.audioManager.PlayAudio(words[1], words[2]);
                }
                else if (words.Length >= 2)  // 第２引数が存在しない場合は音声の停止
                {
                    gameManager.audioManager.StopAudio(words[1]);
                }
                break;
            case "&jump":
                if (words.Length >= 3)
                {
                    if (words[1] == "to")
                    {
                        string key = words[2];
                        if (jumpKey.ContainsKey(key)) {
                            gameManager.lineNumber = jumpKey[key];
                        }
                    }
                }
                break;
            case "&end":
                Application.Quit();
                gameManager.lineNumber = -1;
                break;
        }
    }

    // 選択肢の命令を実行する（特殊な命令なのでExecuteStatementから処理を分ける）
    public void ExecuteChoiceStatement()
    {
        List<Choice> choices = new();
        int indent = GetIndent(GetCurrentSentence());
        for (int currentLineNumber = gameManager.lineNumber + 1; currentLineNumber < sentences.Count; currentLineNumber++)
        {
            string sentence = sentences[currentLineNumber];
            int currentIndent = GetIndent(sentence);
            if (currentIndent <= indent && !statementRegex.IsMatch(sentence)) break;
            if (!IsChoiceStatement(sentence)) continue;
            if (currentIndent == indent)
            {
                choices.Add(new Choice(NormalizeSentence(sentence).Split(" ")[1], currentLineNumber));
            }
        }
        if (choices.Count > 0) gameManager.buttonManager.SetButton(choices);
    }

    public int GetIndent(string sentence)
    {
        return indentRegex.Match(sentence).Value.Length;
    }

    public class Choice
    {
        public Choice(string name, int nextLineNumber)
        {
            this.name = name;
            this.nextLineNumber = nextLineNumber;
        }
        
        public string name;
        public int nextLineNumber;
    }
}
