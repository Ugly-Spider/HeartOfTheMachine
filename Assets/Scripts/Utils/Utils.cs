namespace HeartOfTheMachine
{
    public static class Utils
    {
        public static void Assert(bool condition)
        {
            if (!condition)
            {
                LogError("Assert failed!");
            }
        }

        public static void Assert(bool condition, string msg)
        {
            if (!condition)
            {
                LogError($"Assert failed! {msg}");
            }
        }

        public static void Log(string msg)
        {
            UnityEngine.Debug.Log($"[HeartOfTheMachine]{msg}");
        }

        public static void LogError(string msg)
        {
            UnityEngine.Debug.LogError($"[HeartOfTheMachine]{msg}");
        }
    }
}