using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseWindow : EditorWindow
{
    protected float _leftSpace = 20;
    protected float _ySpace = 10;
    protected int _oneLineHigh = 20;

    protected float GetY(float line)
    {
        return 20 + (line - 1) * _ySpace + (line - 1) * _oneLineHigh;
    }
}
