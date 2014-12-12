using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ModestTree.Zenject
{
    // Responsibilities:
    // - Expose methods to configure object graph via Bind() methods
    // - Build object graphs via Resolve() method
    public class DiContainer
    {
        readonly Dictionary<Type, List<ProviderBase>> _providers = new Dictionary<Type, List<ProviderBase>>();
        readonly SingletonProviderMap _singletonMap;

        static Stack<Type> _lookupsInProgress = new Stack<Type>();

        bool _allowNullBindings;

        ProviderBase _fallbackProvider;
        Instantiator _instantiator;

        readonly List<IInstaller> _installedInstallers = new List<IInstaller>();

        public DiContainer()
        {
            _singletonMap = new SingletonProviderMap(this);
            _instantiator = new Instantiator(this);

            Bind<DiContainer>().To(this);
            Bind<Instantiator>().To(_instantiator);
        }

        public IEnumerable<IInstaller> InstalledInstallers
        {
            get
            {
                return _installedInstallers;
            }
        }

        // This can be used to handle the case where the given contract is not
        // found in any other providers, and the contract is not optional
        // For example, to automatically mock-out missing dependencies, you can
        // do this:
        // _container.FallbackProvider = new TransientMockProvider(_container);
        // It can also be used to create nested containers:
        // var nestedContainer = new DiContainer();
        // nestedContainer.FallbackProvider = new DiContainerProvider(mainContainer);
        public ProviderBase FallbackProvider
        {
            get
            {
                return _fallbackProvider;
            }
            set
            {
                _fallbackProvider = value;
            }
        }

        // This flag is used during validation
        // in which case we use nulls to indicate whether we have an instance or not
        // Should be set to false otherwise
        public bool AllowNullBindings
        {
            get
            {
                return _allowNullBindings;
            }
            set
            {
                _allowNullBindings = value;
            }
        }

        public IEnumerable<Type> AllContracts
        {
            get
            {
                return _providers.Keys;
            }
        }

        // Note that this list is not exhaustive by any means
        // It is also not necessary accurate
        public IEnumerable<Type> AllConcreteTypes
        {
            get
            {
                return (from x in _providers from p in x.Value select p.GetInstanceType()).Where(x => x != null && !x.IsInterface && !x.IsAbstract).Distinct();
            }
        }

        IEnumerable<object> AllConcreteInstances
        {
            get
            {
                return (from x in _providers from p in x.Value where p.HasInstance(p.GetInstanceType()) select p.GetInstance(p.GetInstanceType(), new InjectContext(this)));
            }
        }

        // This is the list of concrete types that are in the current object graph
        // Useful for error messages (and complex binding conditions)
        internal static Stack<Type> LookupsInProgress
        {
            get
            {
                return _lookupsInProgress;
            }
        }

        internal static string GetCurrentObjectGraph()
        {
            if (_lookupsInProgress.Count == 0)
            {
                return "";
            }

            return _lookupsInProgress.Select(t => t.Name()).Reverse().Aggregate((i, str) => i + "\n" + str);
        }

        public BindingConditionSetter BindGameObjectFactory<T>(GameObject prefab)
            // This would be useful but fails with VerificationException's in webplayer builds for some reason
            //where T : GameObjectFactory
            where T : class
        {
            if (prefab == null)
            {
                throw new ZenjectBindException(
                    "Null prefab provided to BindGameObjectFactory for type '{0}'".With(typeof(T).Name()));
            }
            // We could bind the factory ToSingle but doing it this way is better
            // since it allows us to have multiple game object factories that
            // use different prefabs and have them injected into different places
            return Bind<T>().ToMethod(c => c.Instantiate<T>(prefab));
        }

        public BindingConditionSetter BindFactoryToMethodUntyped<TContract>(Func<DiContainer, object[], TContract> method)
        {
            return Bind<IFactoryUntyped<TContract>>().To(new FactoryMethodUntyped<TContract>(this, method));
        }

        public BindingConditionSetter BindFactoryToMethod<TContract>(Func<DiContainer, TContract> method)
        {
            return Bind<IFactory<TContract>>().ToMethod((c) => c.Instantiate<FactoryMethod<TContract>>(method));
        }

        public BindingConditionSetter BindFactoryToMethod<TParam1, TContract>(Func<DiContainer, TParam1, TContract> method)
        {
            return Bind<IFactory<TParam1, TContract>>().ToMethod((c) => c.Instantiate<FactoryMethod<TParam1, TContract>>(method));
        }

        public BindingConditionSetter BindFactoryToMethod<TParam1, TParam2, TContract>(Func<DiContainer, TParam1, TParam2, TContract> method)
        {
            return Bind<IFactory<TParam1, TParam2, TContract>>().ToMethod((c) => c.Instantiate<FactoryMethod<TParam1, TParam2, TContract>>(method));
        }

        public BindingConditionSetter BindFactoryToMethod<TParam1, TParam2, TParam3, TContract>(Func<DiContainer, TParam1, TParam2, TParam3, TContract> method)
        {
            return Bind<IFactory<TParam1, TParam2, TParam3, TContract>>().ToMethod((c) => c.Instantiate<FactoryMethod<TParam1, TParam2, TParam3, TContract>>(method));
        }

        public BindingConditionSetter BindFactory<TContract>()
        {
            return Bind<IFactory<TContract>>().ToSingle<Factory<TContract>>();
        }

        public BindingConditionSetter BindFactory<TParam1, TContract>()
        {
            return Bind<IFactory<TParam1, TContract>>().ToSingle<Factory<TParam1, TContract>>();
        }

        // Bind IFactory<TContract> such that it creates instances of type TConcrete
        public BindingConditionSetter BindFactoryToFactory<TContract, TConcrete>()
            where TConcrete : TContract
        {
            return BindFactoryToMethod<TContract>((c) => c.Resolve<IFactory<TConcrete>>().Create());
        }

        public BindingConditionSetter BindFactoryToFactory<TParam1, TContract, TConcrete>()
            where TConcrete : TContract
        {
            return BindFactoryToMethod<TParam1, TContract>((c, param1) => c.Resolve<IFactory<TParam1, TConcrete>>().Create(param1));
        }

        public BindingConditionSetter BindFactoryToCustomFactory<TContract, TConcrete, TFactory>()
            where TFactory : IFactory<TConcrete>
            where TConcrete : TContract
        {
            return BindFactoryToMethod<TContract>((c) => c.Resolve<TFactory>().Create());
        }

        // Bind IFactory<TContract> to the given factory
        public BindingConditionSetter BindFactoryToCustomFactory<TParam1, TContract, TConcrete, TFactory>()
            where TFactory : IFactory<TParam1, TConcrete>
            where TConcrete : TContract
        {
            return BindFactoryToMethod<TParam1, TContract>((c, param1) => c.Resolve<TFactory>().Create(param1));
        }

        // This occurs so often that we might as well have a convenience method
        public BindingConditionSetter BindFactoryUntyped<TContract>()
        {
            if (typeof(TContract).DerivesFrom(typeof(MonoBehaviour)))
            {
                throw new ZenjectBindException(
                    "Error while binding factory for type '{0}'. Must use version of BindFactory which includes a reference to a prefab you wish to instantiate"
                    .With(typeof(TContract).Name()));
            }

            return Bind<IFactoryUntyped<TContract>>().ToSingle<FactoryUntyped<TContract>>();
        }

        public BindingConditionSetter BindFactoryUntyped<TContract, TConcrete>() where TConcrete : TContract
        {
            if (typeof(TContract).DerivesFrom(typeof(MonoBehaviour)))
            {
                throw new ZenjectBindException(
                    "Error while binding factory for type '{0}'. Must use version of BindFactory which includes a reference to a prefab you wish to instantiate"
                    .With(typeof(TConcrete).Name()));
            }

            return Bind<IFactoryUntyped<TContract>>().ToSingle<FactoryUntyped<TContract, TConcrete>>();
        }

        public BindingConditionSetter BindFactoryForPrefab<TContract>(GameObject prefab) where TContract : Component
        {
            if (prefab == null)
            {
                throw new ZenjectBindException(
                    "Null prefab provided to BindFactoryForPrefab for type '{0}'".With(typeof(TContract).Name()));
            }

            // We could use ToSingleMethod here but then we'd have issues when using .When() conditionals to inject
            // multiple factories in different places
            return Bind<IFactory<TContract>>()
                .ToMethod((container) => container.Instantiate<GameObjectFactory<TContract>>(prefab));
        }

        public ValueBinder<TContract> BindValue<TContract>() where TContract : struct
        {
            return new ValueBinder<TContract>(this);
        }

        public ReferenceBinder<TContract> Rebind<TContract>() where TContract : class
        {
            Unbind<TContract>();
            return Bind<TContract>();
        }

        public ReferenceBinder<TContract> Bind<TContract>() where TContract : class
        {
            return new ReferenceBinder<TContract>(this, _singletonMap);
        }

        // Note that this can include open generic types as well such as List<>
        public BinderUntyped Bind(Type contractType)
        {
            return new BinderUntyped(this, contractType, _singletonMap);
        }

        public BindScope CreateScope()
        {
            return new BindScope(this, _singletonMap);
        }

        // See comment in LookupInProgressAdder
        internal LookupInProgressAdder PushLookup(Type type)
        {
            return new LookupInProgressAdder(this, type);
        }

        public void RegisterProvider(ProviderBase provider, Type contractType)
        {
            if (_providers.ContainsKey(contractType))
            {
                // Prevent duplicate singleton bindings:
                if (_providers[contractType].Find(item => ReferenceEquals(item, provider)) != null)
                {
                    throw new ZenjectException(
                        "Found duplicate singleton binding for contract '{0}'".With(contractType));
                }

                _providers[contractType].Add(provider);
            }
            else
            {
                _providers.Add(contractType, new List<ProviderBase> {provider});
            }
        }

        public int UnregisterProvider(ProviderBase provider)
        {
            int numRemoved = 0;

            foreach (var keyValue in _providers)
            {
                numRemoved += keyValue.Value.RemoveAll(x => x == provider);
            }

            Assert.That(numRemoved > 0, "Tried to unregister provider that was not registered");

            // Remove any empty contracts
            foreach (var contractType in _providers.Where(x => x.Value.IsEmpty()).Select(x => x.Key).ToList())
            {
                _providers.Remove(contractType);
            }

            provider.Dispose();

            return numRemoved;
        }

        public IEnumerable<ZenjectResolveException> ValidateValidatables(params Type[] ignoreTypes)
        {
            var injectCtx = new InjectContext(this);

            foreach (var pair in _providers)
            {
                if (ignoreTypes.Where(i => pair.Key.DerivesFromOrEqual(i)).Any())
                {
                    continue;
                }

                List<ProviderBase> validatableFactoryProviders;

                if (pair.Key.DerivesFrom<IValidatableFactory>())
                {
                    validatableFactoryProviders = pair.Value;
                }
                else
                {
                    validatableFactoryProviders = pair.Value.Where(x => x.GetInstanceType().DerivesFrom<IValidatableFactory>()).ToList();
                }

                foreach (var provider in validatableFactoryProviders)
                {
                    var factory = (IValidatableFactory)provider.GetInstance(pair.Key, injectCtx);

                    var type = factory.ConstructedType;
                    var providedArgs = factory.ProvidedTypes;

                    foreach (var error in ValidateObjectGraph(type, providedArgs))
                    {
                        yield return error;
                    }
                }

                List<ProviderBase> validatableProviders;

                if (pair.Key.DerivesFrom<IValidatable>())
                {
                    validatableProviders = pair.Value;
                }
                else
                {
                    validatableProviders = pair.Value.Where(x => x.GetInstanceType().DerivesFrom<IValidatable>()).ToList();
                }

                Assert.That(validatableFactoryProviders.Intersect(validatableProviders).IsEmpty(),
                    "Found provider implementing both IValidatable and IValidatableFactory.  This is not allowed.");

                foreach (var provider in validatableProviders)
                {
                    var factory = (IValidatable)provider.GetInstance(pair.Key, injectCtx);

                    foreach (var error in factory.Validate())
                    {
                        yield return error;
                    }
                }
            }
        }

        // Walk the object graph for the given type
        // Throws ZenjectResolveException if there is a problem
        // Note: If you just want to know whether a binding exists for the given TContract,
        // use HasBinding instead
        public IEnumerable<ZenjectResolveException> ValidateResolve<TContract>()
        {
            return ValidateResolve(typeof(TContract));
        }

        // Walk the object graph for the given type
        // Throws ZenjectResolveException if there is a problem
        public IEnumerable<ZenjectResolveException> ValidateResolve(Type contractType)
        {
            return ValidateResolve(contractType, new InjectContext(this));
        }

        public IEnumerable<ZenjectResolveException> ValidateResolve<TContract>(InjectContext context)
        {
            return ValidateResolve(typeof(TContract), context);
        }

        // Walk the object graph for the given type
        // Returns all ZenjectResolveExceptions found
        public IEnumerable<ZenjectResolveException> ValidateResolve(Type contractType, InjectContext context)
        {
            return BindingValidator.ValidateContract(this, contractType, context);
        }

        public IEnumerable<ZenjectResolveException> ValidateObjectGraph<TConcrete>(params Type[] extras)
        {
            return ValidateObjectGraph(typeof(TConcrete), extras);
        }

        public IEnumerable<ZenjectResolveException> ValidateObjectGraphsForTypes(params Type[] types)
        {
            foreach (var type in types)
            {
                foreach (var error in ValidateObjectGraph(type))
                {
                    yield return error;
                }
            }
        }

        public IEnumerable<ZenjectResolveException> ValidateObjectGraph(Type contractType, params Type[] extras)
        {
            Assert.That(!contractType.IsAbstract);
            return BindingValidator.ValidateObjectGraph(this, contractType, extras);
        }

        public List<TContract> ResolveMany<TContract>()
        {
            return ResolveMany<TContract>(false);
        }

        public List<TContract> ResolveMany<TContract>(bool optional)
        {
            var context = new InjectContext(this);
            context.Optional = optional;
            return ResolveMany<TContract>(context);
        }

        public List<TContract> ResolveMany<TContract>(InjectContext context)
        {
            return (List<TContract>) ResolveMany(typeof(TContract), context);
        }

        public object ResolveMany(Type contract)
        {
            return ResolveMany(contract, new InjectContext(this));
        }

        // Wrap IEnumerable<> to avoid LINQ mistakes
        internal List<ProviderBase> GetProviderMatches(Type contractType)
        {
            return GetProviderMatches(contractType, new InjectContext(this));
        }

        // Wrap IEnumerable<> to avoid LINQ mistakes
        internal List<ProviderBase> GetProviderMatches(Type contractType, InjectContext context)
        {
            return GetProviderMatchesInternal(contractType, context).ToList();
        }

        IEnumerable<ProviderBase> GetProviderMatchesInternal(Type contractType)
        {
            return GetProviderMatchesInternal(
                contractType, new InjectContext(this));
        }

        // Be careful with this method since it is a coroutine
        IEnumerable<ProviderBase> GetProviderMatchesInternal(
            Type contractType, InjectContext context)
        {
            return GetProvidersForContract(contractType).Where(x => x.Matches(context));
        }

        internal IEnumerable<ProviderBase> GetProvidersForContract(Type contractType)
        {
            List<ProviderBase> providers;

            if (_providers.TryGetValue(contractType, out providers))
            {
                return providers;
            }

            // If we are asking for a List<int>, we should also match for any providers that are bound to the open generic type List<>
            // Currently it only matches one and not the other - not totally sure if this is better than returning both
            if (contractType.IsGenericType && _providers.TryGetValue(contractType.GetGenericTypeDefinition(), out providers))
            {
                return providers;
            }

            return Enumerable.Empty<ProviderBase>();
        }

        public bool HasBinding(Type contract)
        {
            return HasBinding(contract, new InjectContext(this));
        }

        public bool HasBinding(Type contract, InjectContext context)
        {
            List<ProviderBase> providers;

            if (!_providers.TryGetValue(contract, out providers))
            {
                return false;
            }

            return providers.Where(x => x.Matches(context)).HasAtLeast(1);
        }

        public bool HasBinding<TContract>()
        {
            return HasBinding(typeof(TContract));
        }

        public object ResolveMany(Type contractType, InjectContext context)
        {
            // Note that different types can map to the same provider (eg. a base type to a concrete class and a concrete class to itself)

            var matches = GetProviderMatchesInternal(contractType, context).ToList();

            if (matches.Any())
            {
                return ReflectionUtil.CreateGenericList(
                    contractType, matches.Select(x => x.GetInstance(contractType, context)).ToArray());
            }

            if (!context.Optional)
            {
                if (_fallbackProvider != null)
                {
                    var listType = typeof(List<>).MakeGenericType(contractType);

                    return _fallbackProvider.GetInstance(listType, context);
                }

                throw new ZenjectResolveException(
                    "Could not find required dependency with type '" + contractType.Name() + "' \nObject graph:\n" + GetCurrentObjectGraph());
            }

            return ReflectionUtil.CreateGenericList(contractType, new object[] {});
        }

        public List<Type> ResolveTypeMany(Type contract)
        {
            if (_providers.ContainsKey(contract))
            {
                return _providers[contract].Select(x => x.GetInstanceType()).Where(x => x != null).ToList();
            }

            return new List<Type> {};
        }

        // Installing installers works a bit differently than just Resolving all of
        // them and then calling InstallBindings() on each
        // This is because we want to allow installers to "include" other installers
        // And we also want earlier installers to be able to configure later installers
        // So we need to Resolve() one at a time, removing each installer binding
        // as we go
        public void InstallInstallers()
        {
            while (true)
            {
                var provider = GetProviderMatchesInternal(typeof(IInstaller)).FirstOrDefault();

                if (provider == null)
                {
                    break;
                }

                var installer = (IInstaller)provider.GetInstance(typeof(IInstaller), new InjectContext(this));

                Assert.IsNotNull(installer);

                UnregisterProvider(provider);

                if (_installedInstallers.Where(x => x.GetType() == installer.GetType()).Any())
                {
                    // Do not install the same installer twice
                    continue;
                }

                installer.InstallBindings();
                _installedInstallers.Add(installer);
            }
        }

        internal object Resolve(InjectableInfo injectInfo)
        {
            return Resolve(injectInfo, null);
        }

        internal object Resolve(
            InjectableInfo injectInfo, object targetInstance)
        {
            var context = new InjectContext(
                this, injectInfo, LookupsInProgress.ToList(), targetInstance);

            return Resolve(injectInfo.ContractType, context);
        }

        // Return single instance of requested type or assert
        public TContract Resolve<TContract>()
        {
            return Resolve<TContract>(new InjectContext(this));
        }

        public TContract Resolve<TContract>(InjectContext context)
        {
            return (TContract) Resolve(typeof(TContract), context);
        }

        public object Resolve(Type contract)
        {
            return Resolve(contract, new InjectContext(this));
        }

        public object Resolve(Type contractType, InjectContext context)
        {
            // Note that different types can map to the same provider (eg. a base type to a concrete class and a concrete class to itself)

            var providers = GetProviderMatchesInternal(contractType, context).ToList();

            if (providers.IsEmpty())
            {
                // If it's a generic list then try matching multiple instances to its generic type
                if (ReflectionUtil.IsGenericList(contractType))
                {
                    var subType = contractType.GetGenericArguments().Single();
                    return ResolveMany(subType, context);
                }

                if (!context.Optional)
                {
                    if (_fallbackProvider != null)
                    {
                        return _fallbackProvider.GetInstance(contractType, context);
                    }

                    throw new ZenjectResolveException(
                        "Unable to resolve type '{0}'{1}. \nObject graph:\n{2}"
                        .With(
                            contractType.Name() + (context.Identifier == null ? "" : " with ID '" + context.Identifier.ToString() + "'"),
                            (context.EnclosingType == null ? "" : " while building object with type '{0}'".With(context.EnclosingType.Name())),
                            GetCurrentObjectGraph()));
                }

                return null;
            }
            else if (providers.Count > 1)
            {
                throw new ZenjectResolveException(
                    "Found multiple matches when only one was expected for type '{0}'{1}. \nObject graph:\n {2}"
                    .With(
                        contractType.Name(),
                        (context.EnclosingType == null ? "" : " while building object with type '{0}'".With(context.EnclosingType.Name())),
                        GetCurrentObjectGraph()));
            }
            else
            {
                return providers.Single().GetInstance(contractType, context);
            }
        }

        public bool Unbind<TContract>()
        {
            List<ProviderBase> providersToRemove;

            if (_providers.TryGetValue(typeof(TContract), out providersToRemove))
            {
                _providers.Remove(typeof(TContract));

                // Only dispose if the provider is not bound to another type
                foreach (var provider in providersToRemove)
                {
                    if (_providers.Where(x => x.Value.Contains(provider)).IsEmpty())
                    {
                        provider.Dispose();
                    }
                }

                return true;
            }

            return false;
        }

        public IEnumerable<Type> GetDependencyContracts<TContract>()
        {
            return GetDependencyContracts(typeof(TContract));
        }

        public IEnumerable<Type> GetDependencyContracts(Type contract)
        {
            foreach (var injectMember in TypeAnalyzer.GetInfo(contract).AllInjectables)
            {
                yield return injectMember.ContractType;
            }
        }

        // Same as Instantiate except you can pas in null value
        // however the type for each parameter needs to be explicitly provided in this case
        public T InstantiateExplicit<T>(List<TypeValuePair> extraArgList)
        {
            return _instantiator.InstantiateExplicit<T>(extraArgList);
        }

        public object InstantiateExplicit(
            Type concreteType, List<TypeValuePair> extraArgList)
        {
            return _instantiator.InstantiateExplicit(concreteType, extraArgList);
        }

        public T Instantiate<T>(params object[] constructorArgs)
        {
            return _instantiator.Instantiate<T>(constructorArgs);
        }

        public object Instantiate(
            Type concreteType, params object[] constructorArgs)
        {
            return _instantiator.Instantiate(concreteType, constructorArgs);
        }
    }
}
