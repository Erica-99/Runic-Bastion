using System.Collections.Generic;
using UnityEngine;

public class VineTowerSpell : Rune
{
    public override int Priority { get; set; } = 0;

    private void Awake()
    {
        // Scaled to [0,1] x and y on the drawable section of the paper. This will be resized automatically to the paper.
        // (0,0) is bottom right and (1,1) is top left.
        customRune = new List<Vector2>() {
        new Vector2(0.5f, 0.9f),
        new Vector2(0.2f, 0.7f),
        new Vector2(0.8f, 0.5f),
        new Vector2(0.5f, 0.3f),
        new Vector2(0.5f, 0.1f)
        };
    }

    public override void DoSpell()
    {
        Debug.Log("VINE TOWER :)");
    }
}
