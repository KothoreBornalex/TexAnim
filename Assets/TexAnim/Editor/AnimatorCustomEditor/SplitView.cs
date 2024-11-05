using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class SplitView : TwoPaneSplitView
{
    public static SplitView instance;
    public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {

        }
    }
}
