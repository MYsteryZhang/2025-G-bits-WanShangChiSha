using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueConfig", menuName = "Dialogue/New Dialogue")]
public class DialogueConfig : ScriptableObject
{
    [System.Serializable]
    public class DialogueData
    {
        public string dialogueID;       // 唯一标识（如 "Tutorial_1"）
        public string title;            // 对话框标题
        [TextArea(3, 10)]
        public string[] content;          // 正文内容
        public Sprite icon;             // 图标（可选）
        public bool hasConfirmButton = true; // 是否需要确认按钮
    }

    public List<DialogueData> dialogs = new List<DialogueData>();
   
}