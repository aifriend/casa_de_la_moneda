using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Idpsa.SimbolicBuilder;

namespace Idpsa.SimbolicBuilder
{
    public partial class frmSimbolicBuilder : Form
    {
        private String InputFile;
        private String OutputFile;
        private String InputText;
        private String OutputText;
        private DeviceCollection Devices;

        public frmSimbolicBuilder()
        {
            InitializeComponent();

        }

        private void btInput_Click(object sender, EventArgs e)
        {
            DialogInput.InitialDirectory = Application.StartupPath + "\\ScriptsSimbolico\\";
            FileInfo fI = new FileInfo(DialogInput.InitialDirectory);
            if (DialogInput.ShowDialog() == DialogResult.OK)
            {

                FileStream fS = new FileStream(DialogInput.FileName, FileMode.OpenOrCreate);
                StreamReader sR = new StreamReader(fS);
                RichInput.Text = sR.ReadToEnd();
                tbInput.Text = InputFile = DialogInput.FileName;
                sR.Close();
                fS.Close();

            }


        }

        private void SaveFile()
        {
            string dir = null;
            string Txt = null;

            if (Tab.SelectedIndex == 0)
            {
                if (tbInput.Text.Trim() != "")
                    dir = new FileInfo(tbInput.Text).Directory.FullName;
                else
                    dir = Application.StartupPath + "\\ScriptsSimbolico\\";

                DialogSave.InitialDirectory = dir;
                Txt = RichInput.Text;
            }
            else if (Tab.SelectedIndex == 1)
            {
                if (tbOutput.Text.Trim() != "")
                    dir = new FileInfo(tbOutput.Text).Directory.FullName;
                else
                    dir = Application.StartupPath + "\\Simbolicos\\";

                DialogSave.InitialDirectory = dir;
                Txt = RichOutput.Text;
            }
            if (DialogSave.ShowDialog() == DialogResult.OK)
            {

                StreamWriter sW = new StreamWriter(DialogSave.FileName, false);
                sW.Write(Txt);
                sW.Close();


            }

        }

        private void guardarcomoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void btOutput_Click(object sender, EventArgs e)
        {
            loadFileOut();
        }

        private void loadFileOut()
        {

            DialogOutput.InitialDirectory = Application.StartupPath + "\\Simbolicos\\";
            if (DialogOutput.ShowDialog() == DialogResult.OK)
            {
                tbOutput.Text = DialogOutput.FileName;
            }
        }

       
        private void btGenerar_Click(object sender, EventArgs e)
        {
            tbException.Text = "";            
            Devices = new DeviceCollection();
            
           

            try
            {
                RichOutput.Text = Devices.GetSimbolico(RichInput.Lines);
            }
            catch(Exception ex)
            { 
               tbException.Text = ex.Message;
                if (ex.Message.StartsWith("Error en línea:")){
                    int errorLine = int.Parse(ex.Message.Split(new string[]{ ":", ";" },StringSplitOptions.None)[1]) - 1;              
                    SelectLineRichInput(errorLine);
                                       
                   
                }
                return;
            }
            
            GenerateTree(Devices.Elements());


        }

        private void SelectLineRichInput(int line)
        {
            int start = 0;
            for (int j = 0; j < line; j++)
            {
                start += RichInput.Lines[j].Length + 1;
            }
            RichInput.Select(RichInput.Text.IndexOf(RichInput.Lines[line].Trim(), start), RichInput.Lines[line].Trim().Length);

            RichInput.Focus();
            RichInput.ScrollToCaret();
            Tab.SelectedIndex = 0;

        }


        private void GenerateTree(List<Device> devs)
        {

            string signal = null;
            tree.Nodes.Clear();

            foreach (Device dev in devs)
            {

                tree.Nodes.Add(dev.Name, dev.Name);
                
               
                int counter = 0;

                foreach (var input in dev.Inputs)
                {
                    tree.Nodes[dev.Name].Nodes.Add("Inputs"+counter, "Inputs (" + dev.StartInputs[counter] + ")" +"    "+dev.InputsComent[counter]);
                    
                    foreach (string str in input.Keys)
                    {
                        signal = (str + ";" + input[str].simbolico + ";" + input[str].comentario);
                        tree.Nodes[tree.Nodes.Count - 1].Nodes["Inputs"+counter].Nodes.Add(signal, signal);
                    }
                    counter++;
                }

                counter = 0;
                foreach (var output in dev.Outputs)
                {
                    tree.Nodes[dev.Name].Nodes.Add("Outputs" + counter, "Outputs (" + dev.StartOutputs[counter] + ")" + " " + dev.OutputsComent[counter]);

                    foreach (string str in output.Keys)
                    {
                        signal = (str + ";" + output[str].simbolico + ";" + output[str].comentario);
                        tree.Nodes[tree.Nodes.Count - 1].Nodes["Outputs"+counter].Nodes.Add(signal, signal);
                    }
                    counter++;
                }
                tree.Nodes[dev.Name].Expand();
            }

        }

        

        private void btGuardar_Click(object sender, EventArgs e)
        {
            string file = "";

            if (tbOutput.Text.Trim() == "")
            {
                DialogSave.InitialDirectory = Application.StartupPath + "\\Simbolicos\\";
                if (DialogSave.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    WriteFile(DialogSave.FileName);
                }
               
            }
            else
            {
                file = tbOutput.Text;

                if (File.Exists(file))
                {
                    WriteFile(file);
                }
                else
                {

                    DialogSave.InitialDirectory = Application.StartupPath + "\\Simbolicos\\";
                    if (DialogSave.ShowDialog() == DialogResult.OK)
                    {
                        WriteFile(DialogSave.FileName);
                    }



                }

            }
                      
        }

        private void WriteFile(string textFile)
        {
            this.tbOutput.Text = textFile;
            StreamWriter sW = new StreamWriter(textFile, false);
            foreach( string line in RichOutput.Lines){
                sW.WriteLine(line+"\n");
            }
            sW.Close();
        }

        private void cortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cut(selectedRichText());
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copy(selectedRichText());
        }

        private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();
            paste(iData, selectedRichText());
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private RichTextBox selectedRichText()
        {
            
            return Tab.SelectedIndex == 0 ? RichInput : RichOutput;

           
               
            
        }
        

        public void cut(RichTextBox r)
        {
            copy(r);
            r.SelectedText = "";	// deleteSelection();
        }

        public void copy(RichTextBox r)
        {
            Clipboard.SetDataObject(r.SelectedText);
        }

        public void paste(IDataObject iData, RichTextBox r)
        {
            string str = (String)iData.GetData(DataFormats.Text);
            r.SelectedText = str;
        }

        private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedRichText().Undo();
        }

        private void NodeTreeClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
                if (e.Node.Parent ==null)
                {
                    for (int i = 0; i < RichInput.Lines.Length; i++)
                    {
                        string line = RichInput.Lines[i];
                        command value = command.None;

                        if (line.IndexOf("<") != -1 & line.IndexOf(">") != -1)
                        {
                            if (line.IndexOf("<") < line.IndexOf(">"))
                            {
                                string str = line.Substring(line.IndexOf("<") + 1, line.IndexOf(">") - line.IndexOf("<") - 1);
                                str = str.Trim();
                                string[] text = str.Split(new char[] { '(', ')' });
                                string comand = text[0].Trim().ToUpper();
                                if (comand == "DEVICE" && text[1].Trim() == e.Node.Text.Trim())
                                {
                                    SelectLineRichInput(i);
                                    break;
                                }

                            }

                        }
                    }
                }
                else
                {

                    string[] campos = e.Node.Text.Split(new char[] { ';' });

                    if (campos.Length == 3)
                    {

                        string address = e.Node.Text.Split(new char[] { ';' })[0].Trim();
                        string simbolic = e.Node.Text.Split(new char[] { ';' })[1].Trim();

                        if (address[0] == 'I' || address[0] == 'O')
                        {
                            int Byte;
                            int Bit;

                            address = address.Replace("I", "").Replace("O", "");
                            string[] str = address.Split(new char[] { '.' });
                            if ((int.TryParse(str[0], out Byte) && (int.TryParse(str[1], out Bit))))
                            {

                                for (int i = 0; i < RichInput.Lines.Length; i++)
                                    if (RichInput.Lines[i].Trim().Length > 0)
                                    {
                                        if ((Char.IsNumber(RichInput.Lines[i].Trim()[0])) && (RichInput.Lines[i].Contains(simbolic)))
                                        {

                                            SelectLineRichInput(i);


                                            break;
                                        }
                                    }


                            }
                        }
                    }

                }

                                
            }



            
        }

       

      

       

       














    }
}