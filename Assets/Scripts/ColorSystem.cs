using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSystem : MonoBehaviour
{
    public static ColorSystem instance;
    public Color color;
    public Color[] colors;
    public List<int> colorIdx;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            for (int i = 0; i < colors.Length; i++)
            {
                colorIdx.Add(i);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
