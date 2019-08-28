using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.View;
using Idpsa.Control.Component;
using Idpsa.Control.Sequence;

namespace Idpsa.Control.Manuals
{
    internal class ManualManagerWithOutSuperGroups : ManualManager
    {
        private readonly Dictionary<string, List<UserControl>> _manualControls;
        private readonly Dictionary<string, List<IRefrescable>> _manualsToRefresh;
        private readonly Dictionary<string, List<IRun>> _manualsToRun;


        public ManualManagerWithOutSuperGroups(Panel panel, TreeView treeView, IManualsProvider manualsOwner,
                                               IManualControlFactory manualFactory)
            : base(panel, treeView, manualsOwner, manualFactory)
        {
            _manualControls = new Dictionary<string, List<UserControl>>();
            _manualsToRun = new Dictionary<string, List<IRun>>();
            _manualsToRefresh = new Dictionary<string, List<IRefrescable>>();
            TreeViewItemHeight = 70;
            TeeViewSizeFont = 14;

            foreach (Manual manual in Manuales.Values)
            {
                UserControl manualControl = CreateManualControls(manual);

                if (!_manualControls.ContainsKey(manual.Group))
                {
                    _manualControls.Add(manual.Group, new List<UserControl>());
                    _manualsToRefresh.Add(manual.Group, new List<IRefrescable>());
                    _manualsToRun.Add(manual.Group, new List<IRun>());
                    _manualControls[manual.Group].Add(new ControlTitle(manual.Group));
                }

                _manualControls[manual.Group].Add(manualControl);
                if (manualControl is IRefrescable) _manualsToRefresh[manual.Group].Add((IRefrescable)manualControl);
                if (manualControl is IRun) _manualsToRun[manual.Group].Add((IRun)manualControl);
            }
        }

        public string CurrentGroup { get; set; }

        public override IEnumerable<string> AllGroups()
        {
            return _manualControls.Keys;
        }

        public override void LoadTreeView()
        {
            TreeView.Font = new Font(TreeView.Font.FontFamily, TeeViewSizeFont);
            TreeView.Nodes.Clear();

            foreach (string grupo in _manualControls.Keys)
                TreeView.Nodes.Add(new TreeNode(grupo.PadLeft(grupo.Length + 2, ' ').PadRight(grupo.Length + 4, ' ')));

            TreeView.ItemHeight = TreeViewItemHeight;
            TreeView.ExpandAll();
        }

        protected override void Run()
        {
            if (CurrentGroup != null)
                foreach (IRun runner in _manualsToRun[CurrentGroup])
                    runner.Run();
        }

        protected override void Refresh()
        {
            if (CurrentGroup != null)
                foreach (IRefrescable refresher in _manualsToRefresh[CurrentGroup])
                    refresher.RefreshView();
        }


        protected override void TreeViewClickHandler(object sender, EventArgs e)
        {
            try
            {
                string group = TreeView.SelectedNode.Text.Trim();
                if (group != CurrentGroup)
                {
                    CurrentGroup = group;
                    int posY = 0;
                    Panel.Controls.Clear();

                    foreach (UserControl manualControl in _manualControls[group])
                    {
                        int posX = (Panel.Width - manualControl.Width) / 2;
                        manualControl.Location = new Point(posX, posY);
                        Panel.Controls.Add(manualControl);
                        posY += manualControl.Size.Height;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}