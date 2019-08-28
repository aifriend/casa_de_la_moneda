using System.Threading;
using System.Windows.Forms;
using Idpsa.Control;
using Idpsa.Control.Engine;
using Idpsa.Control.Component;


namespace Idpsa.Control.View
{
    public class FormDIOSignals : System.Windows.Forms.Form, IViewTaskOwner
	{
		private bool Inicializate = false;
        private Bus bus = null;
		private Thread trdLoadDIO;
		#region " Windows Form Designer generated code "

        private FormDIOSignals()
            : base()
        {
            //This call is required by the Windows Form Designer.
            InitializeComponent();
            
            //Add any initialization after the InitializeComponent() call
        }
		public FormDIOSignals(Bus bus) : base()
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
		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		internal System.Windows.Forms.TreeView treeSimbolic;
		internal System.Windows.Forms.ListView ltvOutputs;
		internal System.Windows.Forms.ColumnHeader ColumnHeader1;
		internal System.Windows.Forms.ListView ltvInputs;
		internal System.Windows.Forms.ColumnHeader Input;
		internal System.Windows.Forms.ImageList imgStatus;
		internal System.Windows.Forms.ImageList imgSimbolico;
		internal System.Windows.Forms.Splitter Splitter1;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormDIOSignals));
			this.treeSimbolic = new System.Windows.Forms.TreeView();
			this.imgSimbolico = new System.Windows.Forms.ImageList(this.components);
			this.ltvOutputs = new System.Windows.Forms.ListView();
			this.ColumnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.imgStatus = new System.Windows.Forms.ImageList(this.components);
			this.ltvInputs = new System.Windows.Forms.ListView();
			this.Input = new System.Windows.Forms.ColumnHeader();
			this.Splitter1 = new System.Windows.Forms.Splitter();
			this.Load += new System.EventHandler( frmDIOSignals_Load );
			ltvInputs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler( ltvInputs_ItemCheck );
			ltvOutputs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler( ltvOutputs_ItemCheck );
			this.Closing += new System.ComponentModel.CancelEventHandler( frmDIOSignals_Closing );
			this.Resize += new System.EventHandler( frmDIOSignals_Resize );
			this.SuspendLayout();
			//
			//treeSimbolic
			//
			this.treeSimbolic.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeSimbolic.ImageList = this.imgSimbolico;
			this.treeSimbolic.ItemHeight = 16;
			this.treeSimbolic.Location = new System.Drawing.Point(0, 0);
			this.treeSimbolic.Name = "treeSimbolic";
			this.treeSimbolic.Size = new System.Drawing.Size(208, 526);
			this.treeSimbolic.TabIndex = 5;
			//
			//imgSimbolico
			//
			this.imgSimbolico.ImageSize = new System.Drawing.Size(16, 16);
			this.imgSimbolico.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgSimbolico.ImageStream");
			this.imgSimbolico.TransparentColor = System.Drawing.Color.Transparent;
			//
			//ltvOutputs
			//
			this.ltvOutputs.BackColor = System.Drawing.Color.Azure;
			this.ltvOutputs.CheckBoxes = true;
			this.ltvOutputs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {this.ColumnHeader1});
			this.ltvOutputs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ltvOutputs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
			this.ltvOutputs.FullRowSelect = true;
			this.ltvOutputs.GridLines = true;
			this.ltvOutputs.Location = new System.Drawing.Point(208, 258);
			this.ltvOutputs.Name = "ltvOutputs";
			this.ltvOutputs.Size = new System.Drawing.Size(544, 268);
			this.ltvOutputs.SmallImageList = this.imgStatus;
			this.ltvOutputs.TabIndex = 15;
			this.ltvOutputs.View = System.Windows.Forms.View.Details;
			//
			//ColumnHeader1
			//
			this.ColumnHeader1.Text = "Outputs";
			this.ColumnHeader1.Width = 600;
			//
			//imgStatus
			//
			this.imgStatus.ImageSize = new System.Drawing.Size(16, 16);
			this.imgStatus.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgStatus.ImageStream");
			this.imgStatus.TransparentColor = System.Drawing.Color.Transparent;
			//
			//ltvInputs
			//
			this.ltvInputs.BackColor = System.Drawing.Color.Azure;
			this.ltvInputs.CheckBoxes = true;
			this.ltvInputs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {this.Input});
			this.ltvInputs.Dock = System.Windows.Forms.DockStyle.Top;
			this.ltvInputs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
			this.ltvInputs.FullRowSelect = true;
			this.ltvInputs.GridLines = true;
			this.ltvInputs.Location = new System.Drawing.Point(208, 0);
			this.ltvInputs.Name = "ltvInputs";
			this.ltvInputs.Size = new System.Drawing.Size(544, 258);
			this.ltvInputs.SmallImageList = this.imgStatus;
			this.ltvInputs.TabIndex = 17;
			this.ltvInputs.View = System.Windows.Forms.View.Details;
			//
			//Input
			//
			this.Input.Text = "Inputs";
			this.Input.Width = 600;
			//
			//Splitter1
			//
			this.Splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.Splitter1.Location = new System.Drawing.Point(208, 258);
			this.Splitter1.Name = "Splitter1";
			this.Splitter1.Size = new System.Drawing.Size(544, 3);
			this.Splitter1.TabIndex = 18;
			this.Splitter1.TabStop = false;
			//
			//frmDIOSignals
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(752, 526);
			this.Controls.Add(this.Splitter1);
			this.Controls.Add(this.ltvOutputs);
			this.Controls.Add(this.ltvInputs);
			this.Controls.Add(this.treeSimbolic);
			this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			this.Name = "frmDIOSignals";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Señales Digitales (Profibus)";
			this.TopMost = true;
			this.ResumeLayout(false);
		}
		#endregion
		private void frmDIOSignals_Load(object sender, System.EventArgs e)
		{
		}
		private void LoadTree()
		{
			
			Input X;
			Output Y;
			int IntByte;
			int IntBit;
			TreeNode oNode = new TreeNode();
			string StrNodeText;
			treeSimbolic.Nodes.Clear();
			oNode = new TreeNode();
			oNode.Text = "Simbólico";
			oNode.ImageIndex = 0;
			oNode.SelectedImageIndex = 0;
			treeSimbolic.Nodes.Add(oNode);
			oNode = new TreeNode();
			oNode.Text = "Inputs";
			oNode.ImageIndex = 1;
			oNode.SelectedImageIndex = 1;
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
			oNode = new TreeNode();
			oNode.Text = "Outputs";
			oNode.ImageIndex = 1;
			oNode.SelectedImageIndex = 1;
			treeSimbolic.Nodes[0].Nodes.Add(oNode);
            foreach (IOSignal myIOSignal in bus.OutCollection.AllValues)
            {
				Y = bus.Out(myIOSignal.Name);
				IntByte = Y.Address.Byte;
				IntBit = Y.Address.Bit ;
				StrNodeText = "(" + IntByte.ToString() + "." + IntBit.ToString() + ") " + Y.Symbol + " / " + Y.Description;
				oNode = new TreeNode();
				oNode.Text = StrNodeText;
				oNode.ImageIndex = 3;
				oNode.SelectedImageIndex = 3;
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
		private void ltvInputs_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
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
		private void ltvOutputs_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
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
				if (trdLoadDIO.ThreadState == System.Threading.ThreadState.Suspended | trdLoadDIO.IsAlive == false)
				{
					trdLoadDIO = new Thread(ThreadTask);
					trdLoadDIO.IsBackground = true;
					trdLoadDIO.Start();
				}
			}
		}
		private void frmDIOSignals_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
		}
		private void frmDIOSignals_Resize(object sender, System.EventArgs e)
		{
			ltvInputs.Height = (this.Height / 2) - 10;
			ltvOutputs.Height = (this.Height / 2) - 10;
		}
		private void LoadProcess()
		{
			trdLoadDIO = new Thread(ThreadTask);
			trdLoadDIO.IsBackground = true;
			trdLoadDIO.Start();
		}
		private void lSubLoadDIO()
		{
			
			Input lInput;
			Output lOutput;
			string StrItem;
			int i;
			if (this.Visible == true)
			{
				i = 0;
				foreach (ListViewItem myItem in ltvInputs.Items) {
					i = i + 1;
					if (i > 50)
					{
						System.Threading.Thread.Sleep(5);
						i = 0;
					}
					StrItem = myItem.Text;
                    StrItem = StrItem.Split(new char[] { '/', '-' })[1].Trim();
					lInput = bus.In(StrItem);
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
						System.Threading.Thread.Sleep(5);
						i = 0;
					}
					StrItem = myItem.Text;
                    StrItem = StrItem.Split(new char[] { '/', '-' })[1].Trim();
					lOutput = bus.Out(StrItem);
					if (StrItem != "")
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
		}
		private void ThreadTask()
		{			
			do {
                try
                {
                    lSubLoadDIO();
                }
                catch { }
				System.Threading.Thread.Sleep(10);

			}
			while (true);
		}

        #region IViewTaskOwner Members

        public System.Collections.Generic.IEnumerable<ViewTask> GetViewTasks()
        {
            return new System.Collections.Generic.List<ViewTask> { Run };
        }

        #endregion
    }
}
