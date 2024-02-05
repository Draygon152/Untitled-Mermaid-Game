using System;

public static class ActionExtensions
{
    public static void InvokeNullCheck(this Action argAction)
    {
        if (argAction != null)
        {
            argAction.Invoke();
        }
    }

    public static void InvokeNullCheck<T>(this Action<T> argAction, T argParam)
    {
        if (argAction != null)
        {
            argAction.Invoke(argParam);
        }
    }
}