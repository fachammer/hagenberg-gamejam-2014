using System;
using System.Collections.Generic;
using System.Linq;

namespace ModestTree.Zenject
{
    public class SingletonProviderMap
    {
        Dictionary<Type, SingletonLazyCreator> _creators = new Dictionary<Type, SingletonLazyCreator>();
        DiContainer _container;

        public SingletonProviderMap(DiContainer container)
        {
            _container = container;
        }

        internal void RemoveCreator(Type instanceType)
        {
            bool success = _creators.Remove(instanceType);
            Assert.That(success);
        }

        SingletonLazyCreator AddCreator<TConcrete>(Func<DiContainer, TConcrete> method)
        {
            SingletonLazyCreator creator;

            if (_creators.ContainsKey(typeof(TConcrete)))
            {
                throw new ZenjectBindException(
                    "Found multiple singleton instances bound to type '{0}'".With(typeof(TConcrete)));
            }

            creator = new SingletonLazyCreator(
                _container, this, typeof(TConcrete), (container) => method(container));
            _creators.Add(typeof(TConcrete), creator);

            creator.IncRefCount();
            return creator;
        }

        SingletonLazyCreator AddCreator(Type concreteType)
        {
            SingletonLazyCreator creator;

            if (!_creators.TryGetValue(concreteType, out creator))
            {
                creator = new SingletonLazyCreator(_container, this, concreteType);
                _creators.Add(concreteType, creator);
            }

            creator.IncRefCount();
            return creator;
        }

        public ProviderBase CreateProvider<TConcrete>()
        {
            return CreateProvider(typeof(TConcrete));
        }

        public ProviderBase CreateProvider(Type concreteType)
        {
            return new SingletonProvider(_container, AddCreator(concreteType));
        }

        public ProviderBase CreateProvider<TConcrete>(Func<DiContainer, TConcrete> method)
        {
            return new SingletonProvider(_container, AddCreator(method));
        }

        public ProviderBase CreateProvider<TConcrete>(TConcrete instance)
        {
            return CreateProvider(typeof(TConcrete), instance);
        }

        public ProviderBase CreateProvider(Type concreteType, object instance)
        {
            Assert.That(instance != null || _container.AllowNullBindings);

            if (instance != null)
            {
                Assert.That(instance.GetType() == concreteType);
            }

            var creator = AddCreator(concreteType);

            if (creator.HasInstance())
            {
                throw new ZenjectBindException("Found multiple singleton instances bound to the type '{0}'".With(concreteType.Name()));
            }

            creator.SetInstance(instance);

            return new SingletonProvider(_container, creator);
        }
    }
}
