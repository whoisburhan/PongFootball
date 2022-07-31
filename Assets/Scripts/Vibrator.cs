using UnityEngine;

public static class Vibrator
{
#if UNITY_ANDROID
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService","vibrator");
#endif

#if UNITY_ANDROID || UNITYI_IOS
    public static void Vibrate(long miliSeconds = 250)
    {
        if (IsAndroid())
        {
            vibrator.Call("vibrate", miliSeconds);
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    public static void Cancel()
    {
        if (IsAndroid())
        {
            vibrator.Call("cancel");
        }
    }

#endif
    public static bool IsAndroid()
    {
#if UNITY_ANDROID
        return true;
#else
        return false;
#endif
    }
}