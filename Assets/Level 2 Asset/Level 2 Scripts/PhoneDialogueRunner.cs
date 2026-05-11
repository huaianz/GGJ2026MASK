using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Branch
{
    None,
    A,
    B
}

public class PhoneDialogueRunner : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform content;

    [Header("Bubble Prefabs")]
    [SerializeField] private GameObject questionBubblePrefab;
    [SerializeField] private GameObject answerBubblePrefab;

    [Header("Choice Popup")]
    [SerializeField] private ChoicePopupUI choicePopup;

    private DialogueScript currentScript;
    private int sectionIndex;
    private int lineIndex;
    private Branch currentBranch = Branch.None;

    // ===================== 对外入口 =====================

    public void StartDialogue(DialogueScript script)
    {
        currentScript = script;
        sectionIndex = 0;
        lineIndex = 0;
        currentBranch = Branch.None;

        //ClearChat();
        ShowSectionPrompt();
    }

    // ===================== Section 流程 =====================

    private void ShowSectionPrompt()
    {
        if (sectionIndex >= currentScript.sections.Count)
            return;

        var section = currentScript.sections[sectionIndex];

        // 1. 显示 prompt（问/答气泡）
        AddDialogueLine(section.prompt);

        // 2. 弹出选择 Popup
        choicePopup.Show(
            section.choiceA.text,
            section.choiceB.text,
            OnChooseA,
            OnChooseB
        );
    }

    private void OnChooseA()
    {
        var section = currentScript.sections[sectionIndex];

        // 把选中的 choice 写入聊天记录（Answer 气泡）
        AddAnswerBubble(section.choiceA.text);

        currentBranch = Branch.A;
        lineIndex = 0;

        choicePopup.Hide();
        ShowNextLine();
    }

    private void OnChooseB()
    {
        var section = currentScript.sections[sectionIndex];

        AddAnswerBubble(section.choiceB.text);

        currentBranch = Branch.B;
        lineIndex = 0;

        choicePopup.Hide();
        ShowNextLine();
    }

    // ===================== 分支内容播放 =====================

    private void ShowNextLine()
    {
        var section = currentScript.sections[sectionIndex];
        List<DialogueLine> lines =
            currentBranch == Branch.A ? section.branchALines : section.branchBLines;

        if (lineIndex >= lines.Count)
        {
            // 当前 section 结束 → 进入下一个
            sectionIndex++;
            currentBranch = Branch.None;
            lineIndex = 0;

            ShowSectionPrompt();
            return;
        }

        AddDialogueLine(lines[lineIndex]);
        lineIndex++;
    }

    // ===================== UI 工具函数 =====================

    private void AddDialogueLine(DialogueLine line)
    {
        GameObject prefab =
            line.type == LineType.Question
                ? questionBubblePrefab
                : answerBubblePrefab;

        GameObject bubble = Instantiate(prefab, content);
        bubble.GetComponentInChildren<Text>().text = line.text;

        ScrollToBottom();
    }

    private void AddAnswerBubble(string text)
    {
        GameObject bubble = Instantiate(answerBubblePrefab, content);
        bubble.GetComponentInChildren<Text>().text = text;

        ScrollToBottom();
    }

    private void ClearChat()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }

    private void ScrollToBottom()
    {
        StartCoroutine(ScrollNextFrame());
    }

    private IEnumerator ScrollNextFrame()
    {
        yield return null;
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
