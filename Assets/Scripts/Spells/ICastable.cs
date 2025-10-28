using System;
using UnityEngine;

public interface ICastable
{
    public bool readied { get; set; }
    public bool casted { get; set; }

    public void ReadyCast(GameObject character);

    public void DoCast();
}

