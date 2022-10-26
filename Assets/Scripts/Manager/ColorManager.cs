using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public List<Color> colors;

    public static List<Color> colorList;

    private void Awake()
    {
        colorList = colors;
    }

    public static Color GetColor()
    {
        int index=Random.Range(0, colorList.Count);
        Color result = colorList[index];
        colorList.RemoveAt(index);
        return result;
    }
}
