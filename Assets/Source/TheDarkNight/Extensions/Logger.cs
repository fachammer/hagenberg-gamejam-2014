using UniRx;
using UnityEngine;

namespace PRO3.Utility {

    public static class Logging {

        public static IObservable<T> DebugLog<T>(this IObservable<T> source, string message) {
            return source.Do(x => Debug.Log(string.Format(message, x)));
        }
    }
}