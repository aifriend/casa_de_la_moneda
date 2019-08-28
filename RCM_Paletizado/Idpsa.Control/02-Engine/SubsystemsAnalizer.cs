using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Idpsa.Control.Component;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Manuals;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Sequence;
using Idpsa.Control.Tool;

namespace Idpsa.Control.Engine
{
    internal class SubsystemsAnalizer : IAutomaticRunnable, IBackToOriginRunnable, IFreeRunnable, IRi, IManagerRunnable,
                                      IDiagnosisOwner, IOriginDefiner, ISpecialDeviceOwner, IManualsProvider,IDisposable,
                                        IAutomaticRunnable2//MDG.2012-07-23
    {
        private readonly Tree<AnalizerElement> _analizerElements;
        private readonly SystemController _systemController;
        private readonly SubsystemContainer _groupContainer;

        public SubsystemsAnalizer(IDPSASystem sys)
        {
            _analizerElements = InitializeAnalizerElementsTree();
            _groupContainer = new SubsystemContainer();
            _systemController = new SystemController(sys);
            ExtractSubSystems(sys, _analizerElements);           
            AnalizeSubsystems();
            DisposeFunctorConstructor();            
        }

        internal void ConstructFunctors()
        {
            RiFunctorConstructor();
            RiFunctorConstructor2();
            InOriginFunctorConstructor(); 
        }
        internal IEnumerable<TreeNode<AnalizerElement>> Nodes
        {
            get { return _analizerElements; }
        }
        internal IEnumerable<TreeNode<AnalizerElement>> Roots
        {
            get { return _analizerElements.Where(n => n.Parent.Parent == null); }
        }        

        private Tree<AnalizerElement> InitializeAnalizerElementsTree()
        {
            return new Tree<AnalizerElement>
                (new AnalizerElement(null, new SubsystemAttribute(), null) 
                    { State = SubsystemState.Activated });     
        }
        private void ExtractSubSystems(object obj, TreeNode<AnalizerElement> parentNode)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                foreach (
                    SubsystemAttribute attribute in property.GetCustomAttributes(typeof (SubsystemAttribute), false))
                {
                    var othersAtributes =
                        property.GetCustomAttributes(false).OfType<Attribute>().Where(ob => !(ob is SubsystemAttribute))
                            .DefaultIfEmpty();
                    object value = property.GetValue(obj, null);
                    if (value != null)
                    {                        
                        attribute.Filter = attribute.Filter | parentNode.Value.SubSystemAttribute.Filter;
                        var node = new TreeNode<AnalizerElement>(new AnalizerElement(value, attribute, othersAtributes));               
                        ExtractSubSystems(value, node);
                        parentNode.Children.Add(node);
                    }
                }
            }
        }
        private void AnalizeSubsystems()
        {
            foreach (AnalizerElement element in _analizerElements.Select(v=>v.Value))
            {
                FindAutoChains(element);
                FindAutoChains2(element);//MDG.2012-07-23
                FindBackToOriginChains(element);
                FindFreeChains(element);
                FindRiFunctors(element);
                FindRiFunctors2(element);
                FindManagers(element);
                FindSecurityDiagnosis(element);
                FindOriginFunctors(element);
                FindSpecialDevices(element);
                FindSubsysteStateAware(element);
            }
        }           

        #region Miembros de IAutoRunnable

        private void FindAutoChains(AnalizerElement element)
        {  
            if ((element.SubSystemAttribute.Filter & SubsystemFilter.AutoRun) == 0)
            {
                if (element.SubSystem is IAutomaticRunnable)
                {                       
                    element.Services.AutomaticRunnable =
                        ((IAutomaticRunnable)element.SubSystem).GetAutoChains().ToList();                            
                }
            }                     
        }

        public IEnumerable<Chain> GetAutoChains()
        {
            var values = new List<Chain>();
            foreach (var element in _analizerElements.GetDescendants(v => v.State == SubsystemState.Activated))
            {                
                if (element.Services.AutomaticRunnable != null)
                    values.AddRange(element.Services.AutomaticRunnable);
            }
            return values;
        }

        #endregion


        #region Miembros de IAutoRunnable2

        private void FindAutoChains2(AnalizerElement element)
        {  
            if ((element.SubSystemAttribute.Filter & SubsystemFilter.AutoRun2) == 0)
            {
                if (element.SubSystem is IAutomaticRunnable2)
                {                       
                    element.Services.AutomaticRunnable2 =
                        ((IAutomaticRunnable2)element.SubSystem).GetAutoChains2().ToList();                            
                }
            }                     
        }
        
        public IEnumerable<Chain> GetAutoChains2()
        {
            var values = new List<Chain>();
            foreach (var element in _analizerElements.GetDescendants(v => v.State == SubsystemState.Activated))
            {                
                if (element.Services.AutomaticRunnable2 != null)
                    values.AddRange(element.Services.AutomaticRunnable2);
            }
            return values;
        }

        #endregion

        #region Miembros de IVOriginRunnable


        private void FindBackToOriginChains(AnalizerElement element)
        {

            if ((element.SubSystemAttribute.Filter & SubsystemFilter.BackToOriginRun) == 0)
            {
                if (element.SubSystem is IBackToOriginRunnable)
                {
                    element.Services.BackToOriginRunnable =
                        ((IBackToOriginRunnable)element.SubSystem).GetBackToOriginChains().ToList();
                }
            }  
        }

        public IEnumerable<Chain> GetBackToOriginChains()
        {
            var values = new List<Chain>();
            foreach (var element in _analizerElements.GetDescendants(v => v.State == SubsystemState.Activated))
            {
                if (element.Services.BackToOriginRunnable != null)
                    values.AddRange(element.Services.BackToOriginRunnable);
            }
            return values;          
        }

        #endregion

        #region Miembros de IFreeRunnable

        private void FindFreeChains(AnalizerElement element)
        {
            if ((element.SubSystemAttribute.Filter & SubsystemFilter.FreeRun) == 0)
            {
                if (element.SubSystem is IFreeRunnable)
                {
                    element.Services.FreeRunnable =
                        ((IFreeRunnable)element.SubSystem).GetFreeChains().ToList();
                }
            }  
        }

        public IEnumerable<Chain> GetFreeChains()
        {
           var values = new List<Chain>();
           foreach (var element in _analizerElements.GetDescendants(v => v.State == SubsystemState.Activated))
            {
                if (element.Services.FreeRunnable!=null)  
                    values.AddRange(element.Services.FreeRunnable);
            }            
            return values;
        }

        #endregion

        #region Miembros de IRi

        private Action _riFunctor;
        private Action _riFunctor2;

        private void FindRiFunctors(AnalizerElement element)
        {
            if ((element.SubSystemAttribute.Filter & SubsystemFilter.Ri) == 0)
            {
                if (element.SubSystem is IRi)
                {
                    element.Services.Ri +=
                        ((IRi)element.SubSystem).Ri;
                }
            }                    
        }
        private void FindRiFunctors2(AnalizerElement element)
        {
            if ((element.SubSystemAttribute.Filter & SubsystemFilter.Ri2) == 0)
            {
                if (element.SubSystem is IRi2)
                {
                    element.Services.Ri2 +=
                        ((IRi2)element.SubSystem).Ri2;
                }
            }
        }

        private void RiFunctorConstructor()
        {
            _riFunctor = null;
            var values = new List<Action>();

            foreach (var element in _analizerElements.GetDescendants(v => v.State == SubsystemState.Activated))
                _riFunctor += element.Services.Ri;   
         
            if (_riFunctor == null) _riFunctor = (() => { });
        }

        private void RiFunctorConstructor2()
        {
            _riFunctor2 = null;
            var values = new List<Action>();

            foreach (var element in _analizerElements.GetDescendants(v => v.State == SubsystemState.Activated))
                _riFunctor2 += element.Services.Ri2;

            if (_riFunctor2 == null) _riFunctor2 = (() => { });
        }


        public void Ri()
        {
            _riFunctor();
        }
        public void Ri2()
        {
            _riFunctor2();
        }

        #endregion

        #region Miembros de IManagerRunnable

        private void FindManagers(AnalizerElement element)
        {
            if ((element.SubSystemAttribute.Filter & SubsystemFilter.Manager) == 0)
            {
                if (element.SubSystem is IManagerRunnable)
                {
                    ((IManagerRunnable)element.SubSystem)
                    .GetManagers().
                    ForEach(manager => element.Services.ManagerRunnable += manager);
                }
            }                 
        }

        public IEnumerable<Action> GetManagers()
        {                       
            var values = new List<Action>();
            foreach (var element in _analizerElements.GetDescendants(v => v.State == SubsystemState.Activated))
            {
                if (element.Services.ManagerRunnable != null)
                    values.Add(element.Services.ManagerRunnable);
            }            

            if (values.Count == 0) 
                values.Add(() => { });

            return values;
        }

        #endregion

        #region Miembros de IOriginDefiner

        private Func<bool> _inOriginFunctor;

        private void FindOriginFunctors(AnalizerElement element)
        {
            if ((element.SubSystemAttribute.Filter & SubsystemFilter.Origin) == 0)
            {
                if (element.SubSystem is IOriginDefiner)
                {
                    element.Services.OriginDefiner +=
                        ((IOriginDefiner)element.SubSystem).InOrigin;
                }
            }                  
        }

        private void InOriginFunctorConstructor()
        {
            _inOriginFunctor = null;
            var values = new List<Func<bool>>();

            foreach (var element in _analizerElements.GetDescendants(v => v.State == SubsystemState.Activated))
                _inOriginFunctor += element.Services.OriginDefiner;     

            if (_inOriginFunctor == null) _inOriginFunctor = (() => true);
        }


        public bool InOrigin()
        {
            foreach (Func<bool> inOriginSubSystem in _inOriginFunctor.GetInvocationList())
                if (!inOriginSubSystem())
                    return false;

            return true;
        }

        #endregion

        #region Miembros de ISecurityDiagnosisOwner


        private void FindSecurityDiagnosis(AnalizerElement element)
        {
            if ((element.SubSystemAttribute.Filter & SubsystemFilter.Diagnosis) == 0)
            {
                if (element.SubSystem is IDiagnosisOwner)
                {
                    element.Services.SecurityDiagnosisOwner =
                        ((IDiagnosisOwner)element.SubSystem).GetSecurityDiagnosis().ToList();
                }
            }  
        }

        public IEnumerable<SecurityDiagnosis> GetSecurityDiagnosis()
        {
            var values = new List<SecurityDiagnosis>();
            foreach (var element in _analizerElements.GetDescendants(v => v.State == SubsystemState.Activated))
            {
                if (element.Services.SecurityDiagnosisOwner != null)  
                        values.AddRange(element.Services.SecurityDiagnosisOwner);
            }
            return values;       
        }
    

        #endregion

        #region Miembros de ISpecialDeviceOwner

        private void FindSpecialDevices(AnalizerElement element)
        {
            if ((element.SubSystemAttribute.Filter & SubsystemFilter.SpecialDevice) == 0)
            {
                if (element.SubSystem is ISpecialDeviceOwner)
                {
                    element.Services.SpecialDeviceOwner =
                        ((ISpecialDeviceOwner)element.SubSystem).GetSpecialDevices().ToList();
                }
            }       
        }

        public IEnumerable<ISpecialDevice> GetSpecialDevices()
        {
            var values = new List<ISpecialDevice>();
            foreach (var element in _analizerElements.GetDescendants(v => v.State == SubsystemState.Activated))
            {
                if (element.Services.SpecialDeviceOwner != null)                
                    values.AddRange(element.Services.SpecialDeviceOwner);                
            }
            return values;
        }

        public IEnumerable<ISpecialDevice> GetAllSpecialDevices()
        {
            var values = new List<ISpecialDevice>();
            foreach (var element in _analizerElements.GetDescendants(v=>true))
            {
                if (element.Services.SpecialDeviceOwner != null)
                    values.AddRange(element.Services.SpecialDeviceOwner);
            }
            return values;
        }

        #endregion

        #region Miembros de IManualProvider

        public IEnumerable<Manual> GetManualsRepresentations()
        {
            var manualProviders = new List<Pair<ManualAttribute, IManualsProvider>>();

            var values = new List<Manual>();
            foreach (AnalizerElement element in _analizerElements.Select(v => v.Value))
            {
                if ((element.SubSystemAttribute.Filter & SubsystemFilter.Manuals) == 0)
                {
                    ManualAttribute manualAttribute = element.OtherAttributes.OfType<ManualAttribute>().FirstOrDefault();
                    Pair<ManualAttribute, IManualsProvider> manualProvider =
                        GetManualProvidersFromObject(element.SubSystem, manualAttribute);
                    if (manualProvider != null) manualProviders.Add(manualProvider);
                    manualProviders.AddRange(GetManualProvidersFormFields(element.SubSystem));
                    manualProviders.AddRange(GetManualProvidersFromProperties(element.SubSystem));
                }
            }

            foreach (var attributeManualProvider in manualProviders)
            {
                ManualAttribute attribute = attributeManualProvider.Value1;
                IEnumerable<Manual> manuales = attributeManualProvider.Value2.GetManualsRepresentations();

                foreach (Manual manual in manuales)
                {
                    if(attribute.SuperGroup !=null)  
                        manual.SuperGroup = attribute.SuperGroup;
                    if(attribute.Group!=null)
                        manual.Group = attribute.Group;
                    if (attribute.Description != null)
                        manual.Description = attribute.Description;

                    values.Add(manual);
                }
            }

            return values;
        }

        private static Pair<ManualAttribute, IManualsProvider> GetManualProvidersFromObject(object obj,
                                                                                            ManualAttribute
                                                                                                manualAttribute)
        {
            Pair<ManualAttribute, IManualsProvider> value = null;
            var manualProvider = obj as IManualsProvider;
            if (manualAttribute != null && manualProvider != null)
            {
                value = new Pair<ManualAttribute, IManualsProvider>(manualAttribute, manualProvider);
            }

            return value;
        }

        private static IEnumerable<Pair<ManualAttribute, IManualsProvider>> GetManualProvidersFromProperties(object obj)
        {
            var values = new List<Pair<ManualAttribute, IManualsProvider>>();
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            foreach (PropertyInfo property in obj.GetType().GetProperties(flags))
            {
                foreach (ManualAttribute attribute in property.GetCustomAttributes(typeof (ManualAttribute), false))
                {
                    object objectRepresented = property.GetValue(obj, null);
                    if (objectRepresented != null)
                    {
                        var manualProvider = objectRepresented as IManualsProvider;
                        if (manualProvider != null)
                        {
                            values.Add(new Pair<ManualAttribute, IManualsProvider>(attribute, manualProvider));
                        }
                    }
                }
            }

            return values;
        }

        private static IEnumerable<Pair<ManualAttribute, IManualsProvider>> GetManualProvidersFormFields(object obj)
        {
            var values = new List<Pair<ManualAttribute, IManualsProvider>>();

            ICollection<FieldInfo> fields = null;
            ReflexionHelper.FindFields(ref fields, obj.GetType());
            foreach (FieldInfo field in fields)
            {
                foreach (ManualAttribute attribute in field.GetCustomAttributes(typeof (ManualAttribute), false))
                {
                    object objectRepresented = field.GetValue(obj);
                    if (objectRepresented != null)
                    {
                        var manualProvider = objectRepresented as IManualsProvider;
                        if (manualProvider != null)
                        {
                            values.Add(new Pair<ManualAttribute, IManualsProvider>(attribute, manualProvider));
                        }
                    }
                }
            }

            return values;
        }

        #endregion

        #region SubsystemStateAware
        internal IDictionary<object,SubsystemState> ActiveSubystems()
        {
            return _analizerElements
                .ToDictionary(n => n.Value.SubSystem, n => n.Value.State);
        }   
        private void FindSubsysteStateAware(AnalizerElement element)
        {
            var stateAware = element.SubSystem as ISubsystemStateAware;
            if (stateAware != null)
            {
                element.Services.StateObserver =
                    stateAware.SetSubsystemStateController(
                        new SubsystemStateController(
                            () => _systemController.ActiveSubsystems(element.SubSystem),
                            () => _systemController.DeactiveSubsystems(element.SubSystem))
                            );
                element.Services.StateObserver.OnStateChanged(element.State); 
            }
        }
        #endregion

        #region Nested type: AnalizerElement

        internal class AnalizerElement
        {
            private SubsystemState _state; 
            public object SubSystem;
            public SubsystemAttribute SubSystemAttribute { get; set; }
            public IEnumerable<Attribute> OtherAttributes { get; set; }
            public SubsystemContainer Services { get; set; }
            public SubsystemState State
            {
                get { return _state; }
                set
                {
                    if (_state != value)
                    {
                        _state = value;
                        if (Services.StateObserver != null)
                        {
                            Services.StateObserver.OnStateChanged(value);
                        }
                    }
                }
            }

            public AnalizerElement(object subSystem, SubsystemAttribute subSystemAttribute,
                                   IEnumerable<Attribute> otherAttributes)
            {
                SubSystem = subSystem;
                SubSystemAttribute = subSystemAttribute;
                OtherAttributes = otherAttributes;
                Services = new SubsystemContainer();
                State = SubsystemState.Deactivated; 
            }
        }

        #endregion

        # region Nested type: SubsystemStateSolicitor

        private class SubsystemStateController: ISubsystemStateController
        {
            private Action _activateRequest;
            private Action _deactivateRequest;

            public SubsystemStateController(Action activateRequest, Action deactivateRequest)
            {
                _activateRequest = activateRequest;
                _deactivateRequest = deactivateRequest; 
            }

            public void Activate()
            {
                _activateRequest();
            }

            public void Deactivate()
            {
                _deactivateRequest();
            }
        }
        #endregion

        #region Nested type: SubsystemContainer
        internal class SubsystemContainer
        {
            public List<Chain> AutomaticRunnable { get; set; }
            public List<Chain> AutomaticRunnable2 { get; set; }//MDG.2012-07-23
            public List<Chain> BackToOriginRunnable { get; set; }
            public List<Chain> FreeRunnable { get; set; }
            public Action Ri { get; set; }
            public Action Ri2 { get; set; }
            public Action ManagerRunnable { get; set; }
            public List<SecurityDiagnosis> SecurityDiagnosisOwner { get; set; }
            public Func<bool> OriginDefiner { get; set; }
            public List<ISpecialDevice> SpecialDeviceOwner { get; set; }
            public ISubsystemStateObserver StateObserver { get; set; }

            public SubsystemContainer()
            {
                Ri = null;
                Ri2 = null;
                ManagerRunnable = null;                
                OriginDefiner = null;            
            }
        }
        #endregion

        #region Miembros de IDisposable

        private Action _disposeFunctor;
        private Action _disposeFunctor2;

        private void DisposeFunctorConstructor()
        {
            foreach (AnalizerElement element in _analizerElements.Select(v => v.Value))
            {
                if (element.SubSystem is IDisposable)
                {
                    _disposeFunctor += ((IDisposable)element.SubSystem).Dispose;
                }
            }

            if (_disposeFunctor == null) _disposeFunctor = (() => { });
        }

        private void DisposeFunctorConstructor2()
        {
            foreach (AnalizerElement element in _analizerElements.Select(v => v.Value))
            {
                if (element.SubSystem is IDisposable)
                {
                    _disposeFunctor2 += ((IDisposable)element.SubSystem).Dispose;
                }
            }

            if (_disposeFunctor2 == null) _disposeFunctor2 = (() => { });
        }

        public void Dispose()
        {
            _disposeFunctor();
            _disposeFunctor2();
        }
        #endregion 
    }
}