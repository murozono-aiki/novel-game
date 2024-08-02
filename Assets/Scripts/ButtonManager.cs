using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] GameManagerScript gameManager;

    [SerializeField] ButtonUI[] buttons;
    
    [System.Serializable, SerializeField]
    class ButtonUI
    {
        public GameObject buttonObject;
        public Button button;
        public TextMeshProUGUI text;
        [System.NonSerialized] public int nextLineNumber;
    }
    
    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].button.onClick.AddListener(() => OnButtonClick(index));
            buttons[i].buttonObject.SetActive(false);
        }
    }

    public void OnButtonClick(int index)
    {
        SetButton(new List<ScenarioManager.Choice>(0));
        gameManager.lineNumber = buttons[index].nextLineNumber;
        gameManager.isWaitingButtonClick = false;
        gameManager.mainTextController.GoToTheNextLine();  // 選択肢の命令がある行（&choice）から、次の（命令でない）行まで移動する。
        // この直後、ボタンを押したため Input.GetMouseButtonUp(0) がtrueの状態でMainTextControllerのUpdateが実行される。
        // すなわち、MainTextController.DisplayNextLineが実行される
    }

    public void SetButton(List<ScenarioManager.Choice> actions)
    {
        gameManager.isWaitingButtonClick = true;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < actions.Count)
            {
                int actionIndex = actions.Count - 1 - i;
                buttons[i].text.text = actions[actionIndex].name;
                buttons[i].nextLineNumber = actions[actionIndex].nextLineNumber;
                buttons[i].buttonObject.SetActive(true);
            }
            else
            {
                buttons[i].buttonObject.SetActive(false);
            }
        }
    }
}
