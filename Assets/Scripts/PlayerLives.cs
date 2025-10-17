using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    public static int Lives;
    public int startLives = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Lives = startLives;
    }

    // this is only here to check if the enemies actually reduce lives when they reach the crystal
    void Update()
    {
        if (Lives > 0)
        {
            print(Lives);
        }

        if (Lives == 0)
        {
            print("I'm dead!!!");
        }
    }
}
