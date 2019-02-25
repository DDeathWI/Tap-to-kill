using System;

public static class Events{

    public static Action GameStart;
    public static Action<int> ChangeScore;
    public static Action GameOver;
}
