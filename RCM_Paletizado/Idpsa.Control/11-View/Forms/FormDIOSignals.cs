using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Threading;
using System.Windows.Forms; 
using Idpsa.Control.Component;
using ThreadState = System.Threading.ThreadState;

namespace Idpsa.Control.View
{
    public class frmDIOSignals : Form, IViewTasksOwner
	{
		private bool Inicializate = false;
        private Bus bus = null;
        private Button bRun;
		private Thread trdLoadDIO;

		#region " Windows Form Designer generated code "

        private frmDIOSignals()
            : base()
        {
            //This call is required by the Windows Form Designer.
            InitializeComponent();
            
            //Add any initialization after the InitializeComponent() call
        }
		public frmDIOSignals(Bus bus) : base()
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();
            this.bus = bus;
			//Add any initialization after the InitializeComponent() call
		}
		//Form overrides dispose to clean up the component list.
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if ((components != null))
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		//Required by the Windows Form Designer
		private IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		internal TreeView treeSimbolic;
		internal ListView ltvOutputs;
		internal ColumnHeader ColumnHeader1;
		internal ListView ltvInputs;
		internal ColumnHeader Input;
		internal ImageList imgStatus;
		internal ImageList imgSimbolico;
		internal Splitter Splitter1;
		[DebuggerStepThrough()]
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDIOSignals));
            this.treeSimbolic = new System.Windows.Forms.TreeView();
            this.imgSimbolico = new System.Windows.Forms.ImageList(this.components);
            this.ltvOutputs = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.imgStatus = new System.Windows.Forms.ImageList(this.components);
            this.ltvInputs = new System.Windows.Forms.ListView();
            this.Input = new System.Windows.Forms.ColumnHeader();
            this.Splitter1 = new System.Windows.Forms.Splitter();
            this.bRun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeSimbolic
            // 
            this.treeSimbolic.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeSimbolic.ImageIndex = 0;
            this.treeSimbolic.ImageList = this.imgSimbolico;
            this.treeSimbolic.ItemHeight = 16;
            this.treeSimbolic.Location = new System.Drawing.Point(0, 0);
            this.treeSimbolic.Name = "treeSimbolic";
            this.treeSimbolic.SelectedImageIndex = 0;
            this.treeSimbolic.Size = new System.Drawing.Size(208, 526);
            this.treeSimbolic.TabIndex = 5;
            // 
            // imgSimbolico
            // 
            this.imgSimbolico.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgSimbolico.ImageStream")));
            this.imgSimbolico.TransparentColor = System.Drawing.Color.Transparent;
            this.imgSimbolico.Images.SetKeyName(0, "");
            this.imgSimbolico.Images.SetKeyName(1, "");
            this.imgSimbolico.Images.SetKeyName(2, "");
            this.imgSimbolico.Images.SetKeyName(3, "");
            // 
            // ltvOutputs
            // 
            this.ltvOutputs.BackColor = System.Drawing.Color.Azure;
            this.ltvOutputs.CheckBoxes = true;
            this.ltvOutputs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1});
            this.ltvOutputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ltvOutputs.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltvOutputs.FullRowSelect = true;
            this.ltvOutputs.GridLines = true;
            this.ltvOutputs.Location = new System.Drawing.Point(208, 258);
            this.ltvOutputs.Name = "ltvOutputs";
            this.ltvOutputs.Size = new System.Drawing.Size(544, 268);
            this.ltvOutputs.SmallImageList = this.imgStatus;
            this.ltvOutputs.TabIndex = 15;
            this.ltvOutputs.UseCompatibleStateImageBehavior = false;
            this.ltvOutputs.View = System.Windows.Forms.View.Details;
            this.ltvOutputs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ltvOutputs_ItemCheck);
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "Outputs";
            this.ColumnHeader1.Width = 600;
            // 
            // imgStatus
            // 
            this.imgStatus.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgStatus.ImageStream")));
            this.imgStatus.TransparentColor = System.Drawing.Color.Transparent;
            this.imgStatus.Images.SetKeyName(0, "");
            this.imgStatus.Images.SetKeyName(1, "");
            this.imgStatus.Images.SetKeyName(2, "");
            this.imgStatus.Images.SetKeyName(3, "");
            // 
            // ltvInputs
            // 
            this.ltvInputs.BackColor = System.Drawing.Color.Azure;
            this.ltvInputs.CheckBoxes = true;
            this.ltvInputs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Input});
            this.ltvInputs.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltvInputs.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltvInputs.FullRowSelect = true;
            this.ltvInputs.GridLines = true;
            this.ltvInputs.Location = new System.Drawing.Point(208, 0);
            this.ltvInputs.Name = "ltvInputs";
            this.ltvInputs.Size = new System.Drawing.Size(544, 258);
            this.ltvInputs.SmallImageList = this.imgStatus;
            this.ltvInputs.TabIndex = 17;
            this.ltvInputs.UseCompatibleStateImageBehavior = false;
            this.ltvInputs.View = System.Windows.Forms.View.Details;
            this.ltvInputs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ltvInputs_ItemCheck);
            // 
            // Input
            // 
            this.Input.Text = "Inputs";
            this.Input.Width = 600;
            // 
            // Splitter1
            // 
            this.Splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.Splitter1.Location = new System.Drawing.Point(208, 258);
            this.Splitter1.Name = "Splitter1";
            this.Splitter1.Size = new System.Drawing.Size(544, 3);
            this.Splitter1.TabIndex = 18;
            this.Splitter1.TabStop = false;
            // 
            // bRun
            // 
            this.bRun.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.bRun.Location = new System.Drawing.Point(164, 2);
            this.bRun.Name = "bRun";
            this.bRun.Size = new System.Drawing.Size(42, 23);
            this.bRun.TabIndex = 19;
            this.bRun.Text = "RUN";
            this.bRun.UseVisualStyleBackColor = false;
            this.bRun.Click += new System.EventHandler(this.bRun_Click);
            // 
            // frmDIOSignals
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(752, 526);
            this.Controls.Add(this.bRun);
            this.Controls.Add(this.Splitter1);
            this.Controls.Add(this.ltvOutputs);
            this.Controls.Add(this.ltvInputs);
            this.Controls.Add(this.treeSimbolic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDIOSignals";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Señales Digitales (Profibus)";
            this.TopMost = true;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmDIOSignals_Closing);
            this.Resize += new System.EventHandler(this.frmDIOSignals_Resize);
            this.ResumeLayout(false);

		}
		#endregion

		private void LoadTree()
		{
			
			Input X;
			Output Y;
			int IntByte;
			int IntBit;
			var oNode = new TreeNode();
			string StrNodeText;
			treeSimbolic.Nodes.Clear();
		    oNode = new TreeNode {Text = "Simbólico", ImageIndex = 0, SelectedImageIndex = 0};
		    treeSimbolic.Nodes.Add(oNode);
		    oNode = new TreeNode {Text = "Inputs", ImageIndex = 1, SelectedImageIndex = 1};
		    treeSimbolic.Nodes[0].Nodes.Add(oNode);
			foreach (IOSignal myIOSignal in bus.InCollection.AllValues) {
				X = bus.In(myIOSignal.Name);
				IntByte = X.Address.Byte;
				IntBit = X.Address.Bit;
				StrNodeText = "(" + IntByte.ToString() + "." + IntBit.ToString() + ") " + X.Symbol + " / " + X.Description;
				oNode = new TreeNode();
				oNode.Text = StrNodeText;
				oNode.ImageIndex = 2;
				oNode.SelectedImageIndex = 2;
				treeSimbolic.Nodes[0].Nodes[0].Nodes.Add(oNode);
			}
		    oNode = new TreeNode {Text = "Outputs", ImageIndex = 1, SelectedImageIndex = 1};
		    treeSimbolic.Nodes[0].Nodes.Add(oNode);
            foreach (IOSignal myIOSignal in bus.OutCollection.AllValues)
            {
				Y = bus.Out(myIOSignal.Name);
				IntByte = Y.Address.Byte;
				IntBit = Y.Address.Bit ;
				StrNodeText = "(" + IntByte.ToString() + "." + IntBit.ToString() + ") " + Y.Symbol + " / " + Y.Description;
                oNode = new TreeNode {Text = StrNodeText, ImageIndex = 3, SelectedImageIndex = 3};
                treeSimbolic.Nodes[0].Nodes[1].Nodes.Add(oNode);
			}
			lSubLoadList();
		}
		private void lSubLoadList()
		{
			
			Input X;
			Output Y;
			string StrInput;
			string StrOutput;
			ListViewItem myLstItem;
			ltvInputs.Items.Clear();
			ltvOutputs.Items.Clear();
			foreach (IOSignal myIOSignal in bus.InCollection.AllValues) {
				X = bus.In(myIOSignal.Name);
				StrInput = "";
				StrInput = (X.Address.Byte).ToString() + "." + (X.Address.Bit).ToString() + " / " + X.Symbol + " - " + X.Description;
				myLstItem = new ListViewItem(StrInput);
				myLstItem.ImageIndex = 0;
				ltvInputs.Items.AddRange(new ListViewItem[] {myLstItem});
			}
            foreach (IOSignal myIOSignal in bus.OutCollection.AllValues)
            {
				Y = bus.Out(myIOSignal.Name);
				StrOutput = "";
				StrOutput = (Y.Address.Byte).ToString() + "." + (Y.Address.Bit).ToString() + " / " + Y.Symbol + " - " + Y.Description;
				myLstItem = new ListViewItem(StrOutput);
				myLstItem.ImageIndex = 0;
				ltvOutputs.Items.AddRange(new ListViewItem[] {myLstItem});
			}
		}
		private void ltvInputs_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			string StrItem;
			Input lInput;			
			StrItem = ltvInputs.Items[e.Index].Text;
            StrItem = StrItem.Split(new char[] { '/', '-' })[1].Trim();
			lInput =bus.In(StrItem);
			if (e.NewValue == CheckState.Checked)
			{
				lInput.FSet = true;
			}
			else
			{
				lInput.FSet = false;
			}
		}
		private void ltvOutputs_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			Output lOutput;
			string StrItem;
			StrItem = ltvOutputs.Items[e.Index].Text;
            StrItem = StrItem.Split(new char[] { '/', '-' })[1].Trim();
			lOutput = bus.Out(StrItem);
			if (e.NewValue == CheckState.Checked)
			{
				lOutput.FSet = true;
			}
			else
			{
				lOutput.FSet = false;
			}
		}
		private void Run()
		{
			if (Inicializate == false)
			{
				Inicializate = true;
				LoadTree();
				LoadProcess();
			}
			else
			{
				if (trdLoadDIO.ThreadState == ThreadState.Suspended | trdLoadDIO.IsAlive == false)
				{
				    trdLoadDIO = new Thread(ThreadTask) {IsBackground = true};
				    trdLoadDIO.Start();
				}
			}
		}
		private void frmDIOSignals_Closing(object sender, CancelEventArgs e)
		{
			Hide();
			e.Cancel = true;
		}
		private void frmDIOSignals_Resize(object sender, EventArgs e)
		{
			ltvInputs.Height = (Height / 2) - 10;
			ltvOutputs.Height = (Height / 2) - 10;
		}
		private void LoadProcess()
		{
		    trdLoadDIO = new Thread(ThreadTask) {IsBackground = true};
		    trdLoadDIO.Start();
		}
		private void LSubLoadDIO()
		{
		    if (Visible != true) return;
		    var i = 0;
		    string strItem;
		    foreach (ListViewItem myItem in ltvInputs.Items) {
		        i = i + 1;
		        if (i > 50)
		        {
		            Thread.Sleep(5);
		            i = 0;
		        }
		        strItem = myItem.Text;
		        strItem = strItem.Split(new char[] { '/', '-' })[1].Trim();
		        Input lInput = bus.In(strItem);
		        if (lInput.FSet == true)
		        {
		            if (myItem.ImageIndex != 2)
		            {
		                myItem.ImageIndex = 2;
		            }
		        }
		        else if (lInput.FReset == true) {
		            if (myItem.ImageIndex != 3)
		            {
		                myItem.ImageIndex = 3;
		            }
		        }
		        else
		        {
		            if (lInput.Value())
		            {
		                if (myItem.ImageIndex != 1)
		                {
		                    myItem.ImageIndex = 1;
		                }
		            }
		            else
		            {
		                if (myItem.ImageIndex != 0)
		                {
		                    myItem.ImageIndex = 0;
		                }
		            }
		        }
		    }
		    i = 0;
		    foreach (ListViewItem myItem in ltvOutputs.Items)
		    {
		        i = i + 1;
		        if (i > 50)
		        {
		            Thread.Sleep(5);
		            i = 0;
		        }
		        strItem = myItem.Text;
		        strItem = strItem.Split(new char[] { '/', '-' })[1].Trim();
		        Output lOutput = bus.Out(strItem);
		        if (strItem != "")
		        {
		            if (lOutput.FSet == true)
		            {
		                if (myItem.ImageIndex != 2)
		                {
		                    myItem.ImageIndex = 2;
		                }
		            }
		            else if (lOutput.FReset == true) {
		                if (myItem.ImageIndex != 3)
		                {
		                    myItem.ImageIndex = 3;
		                }
		            }
		            else
		            {
		                if (lOutput.Value())
		                {
		                    if (myItem.ImageIndex != 1)
		                    {
		                        myItem.ImageIndex = 1;
		                    }
		                }
		                else
		                {
		                    if (myItem.ImageIndex != 0)
		                    {
		                        myItem.ImageIndex = 0;
		                    }
		                }
		            }
		        }
		    }
		}
		private void ThreadTask()
		{			
			do {
                if (!_runDIO) continue;
                try
                {
                    LSubLoadDIO();
                }
                catch
                { }
				Thread.Sleep(10);
			}
			while (true);
		}

        #region IViewTaskOwner Members

        public IEnumerable<Action> GetViewTasks()
        {
            return new List<Action> { Run };
        }

        #endregion

        private bool _runDIO;

        private void bRun_Click(object sender, EventArgs e)
        {
            _runDIO = !_runDIO;
            bRun.BackColor = _runDIO ? Color.Salmon : Color.MediumSeaGreen;
        }
	}
}
