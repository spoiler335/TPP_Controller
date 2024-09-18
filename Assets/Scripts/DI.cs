using UnityEngine;

public class DI
{
    public static readonly DI di = new DI();

    public InputManager input = new InputManager();

    private DI()
    {
        Debug.Log("DI");
    }
}
