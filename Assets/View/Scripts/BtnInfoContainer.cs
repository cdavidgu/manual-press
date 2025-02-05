using UnityEngine;
using UnityEngine.Events;
public class BttnInfoContainer
{
    public string Name { get { return buttonName; } }
    public string Title { get { return title; } }
    public string ImageName { get { return imageName; } }
    public UnityAction Callback { get { return buttonCallback; } }

    string buttonName;
    string title;
    string imageName;
    UnityAction buttonCallback;

    public BttnInfoContainer(string buttonName, string title, string btnImage, UnityAction callback)

    {
        this.buttonName = buttonName;
        this.imageName = btnImage;
        this.title = title;
        this.buttonCallback = callback;
    }
    public BttnInfoContainer(string buttonName, string title, string btnImage)

    {
        this.buttonName = buttonName;
        this.imageName = btnImage;
        this.title = title;
    }
    //public GameObject prefab;
}