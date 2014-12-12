using System;
using System.Collections.Generic;
using System.Linq;

namespace ModestTree.Zenject
{
    public class DisposableManager : IDisposable
    {
        List<DisposableInfo> _disposables = new List<DisposableInfo>();
        DiContainer _container;
        bool _disposed;

        public DisposableManager(
            [InjectOptional]
            List<IDisposable> disposables,
            [InjectOptional]
            List<Tuple<Type, int>> priorities,
            DiContainer container)
        {
            _container = container;
            var priorityMap = priorities.ToDictionary(x => x.First, x => x.Second);

            foreach (var disposable in disposables)
            {
                int priority;
                bool success = priorityMap.TryGetValue(disposable.GetType(), out priority);

                if (!success)
                {
                    //Log.Warn(
                        //String.Format("IDisposable with type '{0}' does not have a priority assigned", //disposable.GetType()));
                }

                _disposables.Add(
                    new DisposableInfo(disposable, success ? (int?)priority : null));
            }

            Log.Debug("Loaded {0} IDisposables to DisposablesHandler", _disposables.Count());
        }

        public void Dispose()
        {
            Assert.That(!_disposed);
            _disposed = true;

            _disposables.Sort(SortCompare);

            if (Assert.IsEnabled)
            {
                WarnForMissingBindings();

                foreach (var disposable in _disposables.Select(x => x.Disposable).GetDuplicates())
                {
                    Assert.That(false, "Found duplicate IDisposable with type '{0}'".With(disposable.GetType()));
                }
            }

            foreach (var disposable in _disposables)
            {
                try
                {
                    disposable.Disposable.Dispose();
                }
                catch (Exception e)
                {
                    throw new ZenjectException(
                        "Error occurred while disposing IDisposable with type '{0}'".With(disposable.Disposable.GetType().Name()), e);
                }
            }

            Log.Debug("Disposed of {0} disposables in DisposablesHandler", _disposables.Count());
        }

        void WarnForMissingBindings()
        {
            var ignoredTypes = new Type[] { typeof(DisposableManager) };

            var boundTypes = _disposables.Select(x => x.Disposable.GetType()).Distinct();

            var unboundTypes = _container.AllConcreteTypes.Where(x => x.DerivesFrom<IDisposable>() && !boundTypes.Contains(x) && !ignoredTypes.Contains(x));

            foreach (var objType in unboundTypes)
            {
                Log.Warn("Found unbound IDisposable with type '" + objType.Name() + "'");
            }
        }

        int SortCompare(DisposableInfo e1, DisposableInfo e2)
        {
            // Dispose disposables with null priorities first
            if (!e1.Priority.HasValue)
            {
                return -1;
            }

            if (!e2.Priority.HasValue)
            {
                return 1;
            }

            return e1.Priority.Value.CompareTo(e2.Priority.Value);
        }

        class DisposableInfo
        {
            public IDisposable Disposable;
            public int? Priority;

            public DisposableInfo(IDisposable disposable, int? priority)
            {
                Disposable = disposable;
                Priority = priority;
            }
        }
    }
}
