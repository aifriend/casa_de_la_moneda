using System;

namespace Idpsa
{
    //Ocr
    [Serializable]
    public class OcrPassPorts //:System.IDisposable
    {
        //private SLXOcr ocr;
        //private SLXDatamanController.Program datamanController;

        //public delegate void NewIdReadedHandler(string Id);

        //[field: System.NonSerialized()]
        //public event NewIdReadedHandler NewIdReaded;
        //public bool Connecteted { get; private set; }

        //public OcrPassPorts()
        //{
        //    //Ocr
        //    datamanController = new SLXDatamanController.Program();
        //    datamanController.ImageReady += new SLXDatamanController.Program.MyFirstDelegate(datamanController_ImageReady);
        //    datamanController.ReaderDisconnected += new System.EventHandler(datamanController_ReaderDisconnected);
        //    datamanController.ReaderConnected += new SLXDatamanController.Program.MySecondDelegate(datamanController_ReaderConnected);
        //    datamanController.ConnectDataman();
        //    ocr = new SLXOcr();
        //    ocr.ReadDone += new SLXOcr.MyFirstDelegate(ocr_ReadDone);
        //    ocr.LoadToolGroup();
        //}

        //void ocr_ReadDone(object data)
        //{
        //    string aux = data.ToString().Split(new char[] { ' ' })[0];
        //    if (Pasaporte.IsIdValid(aux, TrdControl.Instance.Sys.Production.Catalog.Tipo))
        //        OnNewIdReaded(aux);
        //}

        //private void OnNewIdReaded(string id)
        //{
        //    if (NewIdReaded != null)
        //    {
        //        NewIdReaded(id);
        //    }
        //}

        //void datamanController_ImageReady(object data)
        //{
        //    ocr.RunToolGroup((System.Drawing.Bitmap)data);
        //}

        //void datamanController_ReaderConnected()
        //{
        //    Connecteted = true;
        //    Console.WriteLine("conectado");
        //}

        //void datamanController_ReaderDisconnected(object sender, System.EventArgs e)
        //{
        //    Connecteted = false;
        //}


        //#region Miembros de IDisposable

        //public void Dispose()
        //{
        //    datamanController.Disconnect();
        //}

        //#endregion
    }
}