using UnityEngine;
using System.Collections;
using UnityEditor.Search;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    [SerializeField] private DialogueConfig dialogueConfig;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Animator animator;
    [SerializeField] private float textSpeed = .1f;

    private Dictionary<string, DialogueConfig.DialogueData> dialogueDictionary;
    private Queue<string> sentences = new Queue<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        //使用Dictionary管理对话数据，类似SoundManager写法
        dialogueDictionary = new Dictionary<string, DialogueConfig.DialogueData>();

        foreach (var dialog in dialogueConfig.dialogs)
        {
            dialogueDictionary[dialog.dialogueID] = dialog;
        }

    }

    //其他脚本可调用该函数，传入所需对话的ID来显示对话
    public void StartDialogue(string _dialogueID, float _waitSecondsBetweenSentences)
    {
        if (!dialogueDictionary.TryGetValue(_dialogueID, out DialogueConfig.DialogueData dialogue))
        {
            Debug.LogWarning($"Dialogue Data {_dialogueID} not found!");
            return;
        }

        sentences.Clear();
        //为对话框的标题赋值
        Instance.titleText.text = dialogue.title;
        animator.SetBool("isOpen", true);
        //使用队列(FIFO)管理对话
        foreach (string sentence in dialogue.content)
        {
            sentences.Enqueue(sentence);
        }
        StartCoroutine(DisplayNextSentence(_waitSecondsBetweenSentences));

    }

    public IEnumerator DisplayNextSentence(float _waitSecondsBetweenSentences)
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            yield break;
        }
        //当队列数大于0时，继续显示接下来的对话
        while(sentences.Count > 0)
        {
            yield return new WaitForSeconds(1f);
            string sentence = sentences.Dequeue();

            //逐个显示sentence中的字符
            Instance.contentText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                Instance.contentText.text += letter;
                yield return new WaitForSeconds(textSpeed);
            }
            yield return new WaitForSeconds(_waitSecondsBetweenSentences);
        }
        //当队列为空时，关闭对话框
        EndDialogue();
       
    }

    private void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }

    private void Start()
    {

    }


}
