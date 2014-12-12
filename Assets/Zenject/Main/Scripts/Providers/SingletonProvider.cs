using System;
using System.Collections.Generic;
using System.Linq;

namespace ModestTree.Zenject
{
    // NOTE: we need the provider seperate from the creator because
    // if we return the same provider multiple times then the condition
    // will get over-written
    internal class SingletonProvider : ProviderBase
    {
        SingletonLazyCreator _creator;
        DiContainer _container;

        public SingletonProvider(
            DiContainer container, SingletonLazyCreator creator)
        {
            _creator = creator;
            _container = container;
        }

        public override void Dispose()
        {
            _creator.DecRefCount();
        }

        public override bool HasInstance(Type contractType)
        {
            return _creator.HasInstance();
        }

        public override Type GetInstanceType()
        {
            return _creator.GetInstanceType();
        }

        public override object GetInstance(Type contractType, InjectContext context)
        {
            return _creator.GetInstance(contractType);
        }

        public override IEnumerable<ZenjectResolveException> ValidateBinding(Type contractType, InjectContext context)
        {
            // Can't validate custom methods so assume they work
            if (_creator.HasInstance() || _creator.HasCustomCreateMethod)
            {
                // This would be the case if given an instance at binding time with ToSingle(instance)
                return Enumerable.Empty<ZenjectResolveException>();
            }

            return BindingValidator.ValidateObjectGraph(_container, GetInstanceType());
        }
    }
}
