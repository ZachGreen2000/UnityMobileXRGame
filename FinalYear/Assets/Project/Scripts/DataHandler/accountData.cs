using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class accountData
{
    public int starCount;

    public accountData(int starCount)
    {
        this.starCount = starCount;
    }
}
