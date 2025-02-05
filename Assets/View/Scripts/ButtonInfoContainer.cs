using UnityEngine;

[CreateAssetMenu(fileName = "ButtonInfoContainer", menuName = "New ButtonInfoContainer", order = 0)]
public class ButtonInfoContainer : ScriptableObject
{
    public string buttonName;
    public string title;
    public Sprite selectImg;
    //public GameObject prefab;
}