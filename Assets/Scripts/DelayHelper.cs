using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DelayHelper
{
    private class MonoBehaviourHook : MonoBehaviour { }
    private static MonoBehaviourHook hook;
    private static Dictionary<string, Coroutine> runningCoroutines = new Dictionary<string, Coroutine>();

    static DelayHelper()
    {
        GameObject gameObject = new GameObject("DelayHelper_Runner");
        hook = gameObject.AddComponent<MonoBehaviourHook>();
        GameObject.DontDestroyOnLoad(gameObject); 
        gameObject.hideFlags = HideFlags.HideAndDontSave; 
    }
    public static void CallDelayed(MonoBehaviour behaviour, string methodName, float delay, string coroutineId = null)
    {
        Coroutine coroutine = hook.StartCoroutine(DelayedCallCoroutine(behaviour, methodName, delay, coroutineId));

        if (!string.IsNullOrEmpty(coroutineId))
        {
            runningCoroutines[coroutineId] = coroutine;
        }
    }
    public static void CallDelayed(System.Action action, float delay, string coroutineId = null)
    {
        Coroutine coroutine = hook.StartCoroutine(DelayedCallCoroutine(action, delay, coroutineId));

        if (!string.IsNullOrEmpty(coroutineId))
        {
            runningCoroutines[coroutineId] = coroutine;
        }
    }
    public static void CallDelayedFrames(System.Action action, int frameDelay, string coroutineId = null)
    {
        Coroutine coroutine = hook.StartCoroutine(DelayedCallFramesCoroutine(action, frameDelay, coroutineId));

        if (!string.IsNullOrEmpty(coroutineId))
        {
            runningCoroutines[coroutineId] = coroutine;
        }
    }
    public static void StopDelayedCall(string coroutineId)
    {
        if (runningCoroutines.TryGetValue(coroutineId, out Coroutine coroutine))
        {
            if (hook != null && coroutine != null)
            {
                hook.StopCoroutine(coroutine);
            }
            runningCoroutines.Remove(coroutineId);
        }
    }
    public static void StopAllDelayedCalls()
    {
        if (hook != null)
        {
            hook.StopAllCoroutines();
        }
        runningCoroutines.Clear();
    }
    public static bool IsDelayedCallRunning(string coroutineId)
    {
        return runningCoroutines.ContainsKey(coroutineId);
    }

    private static IEnumerator DelayedCallCoroutine(MonoBehaviour behaviour, string methodName, float delay, string coroutineId)
    {
        yield return new WaitForSeconds(delay);

        if (behaviour != null)
        {
            behaviour.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
        }

        if (!string.IsNullOrEmpty(coroutineId) && runningCoroutines.ContainsKey(coroutineId))
        {
            runningCoroutines.Remove(coroutineId);
        }
    }

    private static IEnumerator DelayedCallCoroutine(System.Action action, float delay, string coroutineId)
    {
        yield return new WaitForSeconds(delay);

        action?.Invoke();

        if (!string.IsNullOrEmpty(coroutineId) && runningCoroutines.ContainsKey(coroutineId))
        {
            runningCoroutines.Remove(coroutineId);
        }
    }

    private static IEnumerator DelayedCallFramesCoroutine(System.Action action, int frameDelay, string coroutineId)
    {
        for (int i = 0; i < frameDelay; i++)
        {
            yield return null;
        }

        action?.Invoke();

        if (!string.IsNullOrEmpty(coroutineId) && runningCoroutines.ContainsKey(coroutineId))
        {
            runningCoroutines.Remove(coroutineId);
        }
    }
}