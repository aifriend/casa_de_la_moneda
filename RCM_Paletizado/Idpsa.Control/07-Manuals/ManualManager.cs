using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.View;

namespace Idpsa.Control.Manuals
{   
    public abstract class ManualManager
    {       
        protected ManualManager(Panel panel, TreeView treeView, IManualsProvider manualsProvider,
                                IManualControlFactory manualFactory)
        {            
            if (panel == null)
                throw new ArgumentNullException("panel");
            if (treeView == null)
                throw new ArgumentNullException("treeView");

            ManualControlFactoryMethod = delegate { return null; };
            Panel = panel;
            TreeView = treeView;
            treeView.DoubleClick += TreeViewClickHandler;
            Manuales = new ManualCollection();
            SetManualsFromManualsProvider(manualsProvider);
            SetManualControlExtendedFactory(manualFactory);            
        }
        
        protected ManualCollection Manuales { get; private set; }
        protected Panel Panel { get; private set; }
        protected TreeView TreeView { get; private set; }
        protected int TreeViewItemHeight { get; set; }
        protected int TeeViewSizeFont { get; set; }
        protected Func<Manual, UserControl> ManualControlFactoryMethod { get; private set; }
        //protected TON timer;//MDG.2011-06-20

        public ManualManager WithTreeViewSizeFont(int size)
        {
            TeeViewSizeFont = size;
            return this;
        }

        public ManualManager WithTreeViewItemHeight(int height)
        {
            TreeViewItemHeight = height;
            return this;
        }

        private void SetManualsFromManualsProvider(IManualsProvider manualsProvider)
        {
            if (manualsProvider != null)
            {
                Manuales.AddRange(manualsProvider.GetManualsRepresentations()
                    .OrderBy(m => m.Description)
                    .OrderBy(m => m.RepresentedInstance.GetType().Name)
                    );
            }
        }

        private void SetManualControlExtendedFactory(IManualControlFactory manualFactory)
        {
            if (manualFactory != null)
                ManualControlFactoryMethod = manualFactory.ManualControlFactoryMethod;
        }

        public abstract IEnumerable<string> AllGroups();
        public abstract void LoadTreeView();
        protected abstract void Run();
        protected abstract void Refresh();       

        public virtual void EnableManualPanel(bool enable)
        {
            Panel.Enabled = enable;
        }

        public virtual void RefreshManual(bool enable)
        {
            if (enable)
            {
                if (Panel.Enabled)
                    Run();
                        
                //if (timer.Timing(200))//MDG.2011-06-20
                    Refresh();                
            }
        }

        protected abstract void TreeViewClickHandler(object sender, EventArgs e);

        protected UserControl CreateManualControls(Manual manual)
        {
            if (manual == null)
                throw new Exception("A manual provided can't be null");

            UserControl userControl = null;
                                  
            if ((userControl = ManualControlFactoryMethod(manual)) == null)
            {
                userControl = DefaultManualControlFactoryMethod(manual);
            }

            if (userControl == null)
                throw new Exception("A manual control provided can't be created");
            
            return userControl;
        }
              


        private static UserControl DefaultManualControlFactoryMethod(Manual manual)
        {
            UserControl mControl = null;

            if (manual.RepresentedInstance is GeneralManual)
            {
                mControl = new ControlGeneralManual(manual);
            }
            else if (manual.RepresentedInstance is CompaxC3I20T11)
            {
                mControl = new ControlC3I20T11(manual);
            }
            else if (manual.RepresentedInstance is IReader)
            {
                mControl = new ControlReader(manual);
            }
            else if (manual.RepresentedInstance is ISocket)
            {
                mControl = new ControlEthernet(manual);
            }
            else if (manual.RepresentedInstance is IDataNotifierProvider<string>)
            {
                mControl = new ControlDataDysplayer<string>(manual);
            }
            else if (manual.RepresentedInstance is IJog)
            {
                mControl = new ControlJog(manual);
            }
            else if (manual.RepresentedInstance is IDynamicManual)
            {
                mControl = new ControlDynamic(manual);
            }
            else if (manual.RepresentedInstance is IEvaluable)
            {
                mControl = new ControlEvaluable(manual);
            }

            return mControl;
        }

        public static ManualManager Create(TreeViewLevels levels, Bus bus, Panel manualPanel,
                                           TreeView treeView,
                                           IManualControlFactory manualFactory)
        {
            if (manualFactory == null)
                throw new ArgumentNullException("manualFactory");

            return CreateCore(levels, manualPanel, treeView, null, manualFactory);
        }

        

        public static ManualManager Create(TreeViewLevels levels,  Panel manualPanel,
                                           TreeView treeView,
                                           IDPSASystem sys,
                                           IManualControlFactory manualFactory)
        {

            if (sys == null)
                throw new ArgumentNullException("sys");

            if (sys.Subsystems == null)
                throw new ArgumentNullException("sys.Subsystems");

             return CreateCore(levels,  manualPanel, treeView, (IManualsProvider)sys.Subsystems, manualFactory);
            
        }

         public static ManualManager Create(TreeViewLevels levels,  Panel manualPanel,
                                           TreeView treeView,
                                           IDPSASystem sys)
        {
            if (sys == null)
                throw new ArgumentNullException("sys");

            if (sys.Subsystems == null)
                throw new ArgumentNullException("sys.Subsystems");

            return CreateCore(levels,  manualPanel, treeView, (IManualsProvider)sys.Subsystems, null);
            
            
        }

         public static ManualManager Create(TreeViewLevels levels, Panel manualPanel,
                                            TreeView treeView,
                                            IManualsProvider manualsProvider)
         {
             if (manualsProvider == null)
                 throw new ArgumentNullException("manualsProvider");

             return CreateCore(levels, manualPanel, treeView, manualsProvider, null);
         }


        private static ManualManager CreateCore(TreeViewLevels levels, Panel manualPanel,
                                           TreeView treeView,
                                           IManualsProvider manualsProvider,
                                           IManualControlFactory manualFactory)
        {            
            ManualManager value;

            switch (levels)
            {
                case TreeViewLevels.One:
                    value = new ManualManagerWithOutSuperGroups(manualPanel, treeView,
                                                             manualsProvider,
                                                             manualFactory);
                    break;
                case TreeViewLevels.Two:
                    value = new ManualManagerWithSuperGroups(manualPanel, treeView,
                                                                manualsProvider,
                                                                manualFactory);
                    break;
                default:
                    throw new NotSupportedException(levels.ToString());

            }

            return value;


        }


    }
}