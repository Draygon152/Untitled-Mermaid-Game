using UnityEngine;

// Contains static functions for managing cursor visibility and lockState
public static class CursorUtility
{
    private static CursorLockMode _defaultLockMode = CursorLockMode.Confined;


#region Utility Functions
    public static void ToggleCursorVisibility(bool arg)
    {
        Cursor.visible = arg;
    }

    public static void ApplyCursorLockState()
    {
        // Confined to window by default, use different one if there's a saved entry for it
        if (!SaveDataUtility.HasKey(SaveDataUtility.CURSOR_LOCK_MODE))
        {
            Cursor.lockState = _defaultLockMode;
        }

        else
        {
            Cursor.lockState = (CursorLockMode)SaveDataUtility.LoadInt(SaveDataUtility.CURSOR_LOCK_MODE);
        }
    }

    public static void ApplyCursorLockState(CursorLockMode newLockMode)
    {
        Cursor.lockState = newLockMode;
    }
#endregion
}