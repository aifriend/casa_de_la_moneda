using System.Windows.Forms;
using Idpsa.Control.Engine;
using Idpsa.Control.Sequence;

namespace Idpsa.Control.View
{
	public class FormCadenas : System.Windows.Forms.Form,IViewTaskOwner
	{
		
		private TON _timer1 = new TON();
		private TON _timer2 = new TON();
		private TON _timer3 = new TON();
        private IIDPSASystem _sys;
     
        
		#region " Código generado por el Diseñador de Windows Forms "
		private FormCadenas() : base()
		{
			//El Diseñador de Windows Forms requiere esta llamada.
			InitializeComponent();            
			//Agregar cualquier inicialización después de la llamada a InitializeComponent()
		}

        public FormCadenas(IIDPSASystem sys)
            : base()
        {
            //El Diseñador de Windows Forms requiere esta llamada.
            InitializeComponent();
            //Agregar cualquier inicialización después de la llamada a InitializeComponent()
            _sys = sys;

        }
		//Form reemplaza a Dispose para limpiar la lista de componentes.
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
		//Requerido por el Diseñador de Windows Forms
		private System.ComponentModel.IContainer components = null;
		//NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
		//Puede modificarse utilizando el Diseñador de Windows Forms. 
		//No lo modifique con el editor de código.
		internal System.Windows.Forms.ListView listViewAuto;
		internal System.Windows.Forms.ColumnHeader Input;
		internal System.Windows.Forms.ColumnHeader ColumnHeader1;
		internal System.Windows.Forms.ColumnHeader ColumnHeader2;
		internal System.Windows.Forms.TabControl tbCadenas;
		internal System.Windows.Forms.TabPage TabPage1;
		internal System.Windows.Forms.TabPage TabPage2;
		internal System.Windows.Forms.ListView listViewVOrigen;
		internal System.Windows.Forms.ColumnHeader ColumnHeader3;
		internal System.Windows.Forms.ColumnHeader ColumnHeader4;
		internal System.Windows.Forms.ColumnHeader ColumnHeader5;
		internal System.Windows.Forms.Panel Panel2;
		internal System.Windows.Forms.Panel Panel3;
		internal System.Windows.Forms.Button BtCongelar;
		internal System.Windows.Forms.TabPage TabPage3;
		internal System.Windows.Forms.ListView listViewLibres;
		internal System.Windows.Forms.ColumnHeader ColumnHeader6;
		internal System.Windows.Forms.ColumnHeader ColumnHeader7;
		internal System.Windows.Forms.ColumnHeader ColumnHeader8;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCadenas));
            this.listViewAuto = new System.Windows.Forms.ListView();
            this.Input = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.tbCadenas = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.Panel3 = new System.Windows.Forms.Panel();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.listViewVOrigen = new System.Windows.Forms.ListView();
            this.ColumnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.listViewLibres = new System.Windows.Forms.ListView();
            this.ColumnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.BtCongelar = new System.Windows.Forms.Button();
            this.tbCadenas.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.TabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ltvAuto
            // 
            this.listViewAuto.BackColor = System.Drawing.Color.Azure;
            this.listViewAuto.CheckBoxes = true;
            this.listViewAuto.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Input,
            this.ColumnHeader1,
            this.ColumnHeader2});
            this.listViewAuto.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewAuto.FullRowSelect = true;
            this.listViewAuto.GridLines = true;
            this.listViewAuto.Location = new System.Drawing.Point(0, 0);
            this.listViewAuto.Margin = new System.Windows.Forms.Padding(4);
            this.listViewAuto.Name = "ltvAuto";
            this.listViewAuto.Size = new System.Drawing.Size(937, 403);
            this.listViewAuto.TabIndex = 18;
            this.listViewAuto.UseCompatibleStateImageBehavior = false;
            this.listViewAuto.View = System.Windows.Forms.View.Details;
            // 
            // Input
            // 
            this.Input.Text = "Cadena";
            this.Input.Width = 379;
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "Paso Actual";
            this.ColumnHeader1.Width = 90;
            // 
            // ColumnHeader2
            // 
            this.ColumnHeader2.Text = "Descripción";
            this.ColumnHeader2.Width = 600;
            // 
            // tbCadenas
            // 
            this.tbCadenas.Controls.Add(this.TabPage1);
            this.tbCadenas.Controls.Add(this.TabPage2);
            this.tbCadenas.Controls.Add(this.TabPage3);
            this.tbCadenas.Location = new System.Drawing.Point(0, 0);
            this.tbCadenas.Margin = new System.Windows.Forms.Padding(4);
            this.tbCadenas.Name = "tbCadenas";
            this.tbCadenas.SelectedIndex = 0;
            this.tbCadenas.Size = new System.Drawing.Size(949, 453);
            this.tbCadenas.TabIndex = 19;
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.Panel3);
            this.TabPage1.Controls.Add(this.listViewAuto);
            this.TabPage1.Location = new System.Drawing.Point(4, 25);
            this.TabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Size = new System.Drawing.Size(941, 424);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Automático";
            // 
            // Panel3
            // 
            this.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Panel3.Location = new System.Drawing.Point(0, 414);
            this.Panel3.Margin = new System.Windows.Forms.Padding(4);
            this.Panel3.Name = "Panel3";
            this.Panel3.Size = new System.Drawing.Size(941, 10);
            this.Panel3.TabIndex = 21;
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.Panel2);
            this.TabPage2.Controls.Add(this.listViewVOrigen);
            this.TabPage2.Location = new System.Drawing.Point(4, 25);
            this.TabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Size = new System.Drawing.Size(941, 424);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "Vuelta Origen";
            // 
            // Panel2
            // 
            this.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Panel2.Location = new System.Drawing.Point(0, 414);
            this.Panel2.Margin = new System.Windows.Forms.Padding(4);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(941, 10);
            this.Panel2.TabIndex = 21;
            // 
            // ltvVOrig
            // 
            this.listViewVOrigen.BackColor = System.Drawing.Color.Azure;
            this.listViewVOrigen.CheckBoxes = true;
            this.listViewVOrigen.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader3,
            this.ColumnHeader4,
            this.ColumnHeader5});
            this.listViewVOrigen.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewVOrigen.FullRowSelect = true;
            this.listViewVOrigen.GridLines = true;
            this.listViewVOrigen.Location = new System.Drawing.Point(0, 0);
            this.listViewVOrigen.Margin = new System.Windows.Forms.Padding(4);
            this.listViewVOrigen.Name = "ltvVOrig";
            this.listViewVOrigen.Size = new System.Drawing.Size(937, 403);
            this.listViewVOrigen.TabIndex = 19;
            this.listViewVOrigen.UseCompatibleStateImageBehavior = false;
            this.listViewVOrigen.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader3
            // 
            this.ColumnHeader3.Text = "Cadena";
            this.ColumnHeader3.Width = 379;
            // 
            // ColumnHeader4
            // 
            this.ColumnHeader4.Text = "Paso Actual";
            this.ColumnHeader4.Width = 90;
            // 
            // ColumnHeader5
            // 
            this.ColumnHeader5.Text = "Descripción";
            this.ColumnHeader5.Width = 600;
            // 
            // TabPage3
            // 
            this.TabPage3.Controls.Add(this.listViewLibres);
            this.TabPage3.Location = new System.Drawing.Point(4, 25);
            this.TabPage3.Margin = new System.Windows.Forms.Padding(4);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Size = new System.Drawing.Size(941, 424);
            this.TabPage3.TabIndex = 2;
            this.TabPage3.Text = "Libres";
            // 
            // ltvLibres
            // 
            this.listViewLibres.BackColor = System.Drawing.Color.Azure;
            this.listViewLibres.CheckBoxes = true;
            this.listViewLibres.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader6,
            this.ColumnHeader7,
            this.ColumnHeader8});
            this.listViewLibres.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewLibres.FullRowSelect = true;
            this.listViewLibres.GridLines = true;
            this.listViewLibres.Location = new System.Drawing.Point(0, 9);
            this.listViewLibres.Margin = new System.Windows.Forms.Padding(4);
            this.listViewLibres.Name = "ltvLibres";
            this.listViewLibres.Size = new System.Drawing.Size(937, 403);
            this.listViewLibres.TabIndex = 20;
            this.listViewLibres.UseCompatibleStateImageBehavior = false;
            this.listViewLibres.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader6
            // 
            this.ColumnHeader6.Text = "Cadena";
            this.ColumnHeader6.Width = 381;
            // 
            // ColumnHeader7
            // 
            this.ColumnHeader7.Text = "Paso Actual";
            this.ColumnHeader7.Width = 90;
            // 
            // ColumnHeader8
            // 
            this.ColumnHeader8.Text = "Descripción";
            this.ColumnHeader8.Width = 600;
            // 
            // BtCongelar
            // 
            this.BtCongelar.Location = new System.Drawing.Point(837, 463);
            this.BtCongelar.Margin = new System.Windows.Forms.Padding(4);
            this.BtCongelar.Name = "BtCongelar";
            this.BtCongelar.Size = new System.Drawing.Size(107, 28);
            this.BtCongelar.TabIndex = 20;
            this.BtCongelar.Text = "Congelar";
            this.BtCongelar.Click += new System.EventHandler(this.BtCongelar_Click);
            // 
            // frmCadenas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 500);
            this.Controls.Add(this.BtCongelar);
            this.Controls.Add(this.tbCadenas);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmCadenas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Control Cadenas";
            this.TopMost = true;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmCadenas_Closing);
            this.tbCadenas.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage2.ResumeLayout(false);
            this.TabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
		
		private void Run()
		{			
			if (BtCongelar.Text == "Congelar"&&Visible)
			{
				RefreshAuto();
				RefreshVOrig();
				RefreshLibres();
			}			
		}

		private void RefreshView(ListView listView,ChainCollection cadenas)
		{	
			listView.Items.Clear();
          
			foreach (Chain myCadena in cadenas.AllValues) {					
                ListViewItem myLstItem = new ListViewItem(myCadena.ChainName);				
				myLstItem.SubItems.Add(myCadena.pCurrentStep.ToString());
				myLstItem.SubItems.Add(myCadena.CurrentStep.Comentario);
                listView.Items.AddRange(new ListViewItem[] { myLstItem });
			}
		}	
			
		
		private void LoadLstViewLibres()
		{
			//hueca
		}
		private void RefreshAuto()
		{
			if (((Width > 500) & TabPage1.Visible & _timer1.Timing(500)))
			{
                RefreshView(listViewAuto, _sys.Chains.AutoChains);
			}
		}
		private void RefreshVOrig()
		{
			if (((Width > 500) & TabPage2.Visible & _timer2.Timing(500)))
			{
                RefreshView(listViewVOrigen, _sys.Chains.VOrigChains);
			}
		}
		private void RefreshLibres()
		{
			if (((Width > 500) & TabPage3.Visible & _timer3.Timing(500)))
			{
               
                //hueca
			}
		}
		private void frmCadenas_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Hide();
			e.Cancel = true;
		}
		private void BtCongelar_Click(object sender, System.EventArgs e)
		{
			if (BtCongelar.Text == "Congelar")
			{
				BtCongelar.Text = "Descongelar";
			}
			else
			{
				BtCongelar.Text = "Congelar";
			}
		}

        #region IViewTaskOwner Members

        public System.Collections.Generic.IEnumerable<ViewTask> GetViewTasks()
        {
            return new System.Collections.Generic.List<ViewTask> { Run };
        }

        #endregion
	}
}
