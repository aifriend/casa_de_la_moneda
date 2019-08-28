using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.View;
using Idpsa.Control.Sequence;
using Idpsa.Control.Component;

namespace Idpsa.Control.Manuals
{
    internal class ManualManagerWithSuperGroups : ManualManager
    {
        private readonly Dictionary<string, Dictionary<string, List<UserControl>>> _manualControls;
        private readonly Dictionary<string, Dictionary<string, List<IRefrescable>>> _manualsToRefresh;
        private readonly Dictionary<string, Dictionary<string, List<IRun>>> _manualsToRun;

        public ManualManagerWithSuperGroups(Panel panel, TreeView treeView, IManualsProvider manualsOwner,
                                            IManualControlFactory manualFactory)
            : base(panel, treeView, manualsOwner, manualFactory)
        {
            _manualControls = new Dictionary<string, Dictionary<string, List<UserControl>>>();
            _manualsToRun = new Dictionary<string, Dictionary<string, List<IRun>>>();
            _manualsToRefresh = new Dictionary<string, Dictionary<string, List<IRefrescable>>>();
            TreeViewItemHeight = 60;
            TeeViewSizeFont = 12;

            foreach (Manual manual in Manuales)
            {
                UserControl manualControl = CreateManualControls(manual);

                if (!_manualControls.ContainsKey(manual.SuperGroup))
                {
                    _manualControls.Add(manual.SuperGroup, new Dictionary<string, List<UserControl>>());
                    _manualsToRefresh.Add(manual.SuperGroup, new Dictionary<string, List<IRefrescable>>());
                    _manualsToRun.Add(manual.SuperGroup, new Dictionary<string, List<IRun>>());
                }
                if (!_manualControls[manual.SuperGroup].ContainsKey(manual.Group))
                {
                    _manualControls[manual.SuperGroup].Add(manual.Group, new List<UserControl>());
                    _manualsToRefresh[manual.SuperGroup].Add(manual.Group, new List<IRefrescable>());
                    _manualsToRun[manual.SuperGroup].Add(manual.Group, new List<IRun>());
                    _manualControls[manual.SuperGroup][manual.Group].Add(
                        new ControlTitle(manual.SuperGroup.ToUpper() + " - " + manual.Group));
                }

                _manualControls[manual.SuperGroup][manual.Group].Add(manualControl);
                if (manualControl is IRefrescable)
                    _manualsToRefresh[manual.SuperGroup][manual.Group].Add((IRefrescable)manualControl);
                if (manualControl is IRun) _manualsToRun[manual.SuperGroup][manual.Group].Add((IRun)manualControl);
            }
        }

        public string CurrentSuperGroup { get; set; }
        public string CurrentGroup { get; set; }


        public IEnumerable<string> AllSuperGroups()
        {
            return _manualControls.Keys;
        }

        public override IEnumerable<string> AllGroups()
        {
            var values = new List<string>();
            foreach (string superGroup in _manualControls.Keys)
            {
                values.AddRange(_manualControls[superGroup].Keys);
            }
            return values;
        }


        public override void LoadTreeView()
        {
            TreeView.Font = new Font(TreeView.Font.FontFamily, TeeViewSizeFont);
            TreeView.Nodes.Clear();

            foreach (string superGroup in _manualControls.Keys)
            {
                var superNode = new TreeNode(superGroup);
                TreeView.Nodes.Add(superNode);
                foreach (string group in _manualControls[superGroup].Keys)
                    superNode.Nodes.Add(new TreeNode(group));
            }

            TreeView.ItemHeight = TreeViewItemHeight;
            TreeView.ExpandAll();
        }

        protected override void Run()
        {
            if (CurrentSuperGroup != null && CurrentGroup != null)
                foreach (IRun runner in _manualsToRun[CurrentSuperGroup][CurrentGroup])
                    runner.Run();
        }

        protected override void Refresh()
        {
            if (CurrentSuperGroup != null && CurrentGroup != null)
                foreach (IRefrescable refresher in _manualsToRefresh[CurrentSuperGroup][CurrentGroup])
                    refresher.RefreshView();
        }


        protected override void TreeViewClickHandler(object sender, EventArgs e)
        {
            try
            {
                if (TreeView.SelectedNode.Level == 1)
                {
                    string group = TreeView.SelectedNode.Text.Trim();
                    string superGroup = TreeView.SelectedNode.Parent.Text.Trim();

                    if ((superGroup != CurrentSuperGroup) || (group != CurrentGroup))
                    {
                        CurrentSuperGroup = superGroup;
                        CurrentGroup = group;
                        int posY = 0;
                        Panel.Controls.Clear();

                        foreach (UserControl manualControl in _manualControls[CurrentSuperGroup][CurrentGroup])
                        {
                            int posX = (Panel.Width - manualControl.Width) / 2;
                            manualControl.Location = new Point(posX, posY);
                            Panel.Controls.Add(manualControl);
                            posY += manualControl.Size.Height;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}