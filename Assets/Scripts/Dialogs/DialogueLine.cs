using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;
}
