using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionPanel :MonoBehaviour
{
    public Toggle FullScreenToggle;
    Resolution[] AllResolutions;
    bool IsFullScreen;
    int SelectedResolustion;
    List<Resolution> SelctedResolutionList = new List<Resolution>();
    // Start is called before the first frame update
    void Start()
    {
        IsFullScreen = true;
        AllResolutions = Screen.resolutions;
        List<string> resolutuinStringList = new List<string>();
        string newRes;
        foreach(Resolution res in AllResolutions)
        {
            newRes = res.width.ToString() + "x" + res.height.ToString();
            if(!resolutuinStringList.Contains(newRes))
            {
            resolutuinStringList.Add(newRes);
            SelctedResolutionList.Add(res);
            }
        }
    }
    public void ChangeREsolution()
    {
        Screen.SetResolution(SelctedResolutionList[SelectedResolustion].width, SelctedResolutionList[SelectedResolustion].height, IsFullScreen);

    }
    public void ChangeFullScreen()
    {
        IsFullScreen = FullScreenToggle.isOn;
        Screen.SetResolution(SelctedResolutionList[SelectedResolustion].width, SelctedResolutionList[SelectedResolustion].height, IsFullScreen);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
