using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Extensions {

    public static class Extensions {

        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action) {
            return source.DoWithIndex((item, _) => action(item));
        }

        public static IEnumerable<T> DoWithIndex<T>(this IEnumerable<T> source, Action<T, int> action) {
            int i = 0;
            foreach(T item in source)
                action(item, i++);

            return source;
        }

        public static T MaxSelect<T>(this IEnumerable<T> source, Func<T, float> score) {
            T maxElement = default(T);
            float maxScore = float.MaxValue;
            foreach(T element in source) {
                float currentScore = score(element);
                if(currentScore > maxScore) {
                    maxElement = element;
                    maxScore = currentScore;
                }
            }

            return maxElement;
        }

        public static T MinSelect<T>(this IEnumerable<T> source, Func<T, float> score) {
            T minElement = default(T);
            float minScore = float.MaxValue;
            foreach(T element in source) {
                float currentScore = score(element);
                if(currentScore < minScore) {
                    minElement = element;
                    minScore = currentScore;
                }
            }

            return minElement;
        }

        public static T GetNearestToPosition<T>(this IEnumerable<T> components, Vector3 position) where T : Component {
            return components.MinSelect(c => Vector3.Distance(c.Position(), position));
        }

        public static IObservable<TResult> CombineLatestOnLeft<TLeft, TRight, TResult>(this IObservable<TLeft> leftSource, IObservable<TRight> rightSource, Func<TLeft, TRight, TResult> selector) {
            return leftSource
                .Select<TLeft, Tuple<TLeft, int>>(Tuple.Create<TLeft, int>)
                .CombineLatest(rightSource,
                    (l, r) => new { Index = l.Item2, Left = l.Item1, Right = r })
                .DistinctUntilChanged(x => x.Index)
                .Select(x => selector(x.Left, x.Right));
        }

        public static IObservable<int> DiscreteValues(this IObservable<float> source) {
            return source.Select(x => (int) x).DistinctUntilChanged();
        }

        public static IObservable<T> FirstFromMultiple<T>(this IEnumerable<IObservable<T>> sources) {
            return sources.Merge().First();
        }

        public static Vector3 Position(this Component component) {
            return component.transform.position;
        }

        public static float DistanceTo(this Transform transform, Transform otherTransform) {
            return Vector3.Distance(transform.position, otherTransform.position);
        }

        public static T TryGetComponent<T>(this Component component) where T : Component {
            T tryComponent = component.GetComponent<T>();
            ThrowMissingComponentExceptionIfNull(tryComponent,
                "Component " + component + " is trying to access Component " + typeof(T).FullName + ", but it is missing");

            return tryComponent;
        }

        public static T TryGetComponentInParent<T>(this Component component) where T : Component {
            T tryComponent = component.GetComponentInParent<T>();
            ThrowMissingComponentExceptionIfNull(tryComponent,
                "Component " + component + " is trying to access Component " + typeof(T).FullName + " in a parent, but it is missing");

            return tryComponent;
        }

        public static IEnumerable<T> TryGetComponentsInChildren<T>(this Component component) where T : Component {
            IEnumerable<T> children = component.GetComponentsInChildren<T>();
            if(children.Count() == 0)
                throw new MissingComponentException("Component " + component + " is trying to access Components of type " + typeof(T).FullName + " in children of itself, but it can't find any");

            return children;
        }

        public static void ThrowMissingComponentExceptionIfNull(object obj, string exceptionMessage) {
            if(obj == null)
                throw new MissingComponentException(exceptionMessage);
        }

        public static T GetClass<T>(this Component component) where T : class {
            return component.GetComponent(typeof(T)) as T;
        }

        public static T TryGetClass<T>(this Component component) where T : class {
            T tryComponent = GetClass<T>(component);
            ThrowMissingComponentExceptionIfNull(tryComponent, "Component " + component + " is trying to access Component " + typeof(T).FullName + ", but it is missing");

            return tryComponent;
        }
    }
}