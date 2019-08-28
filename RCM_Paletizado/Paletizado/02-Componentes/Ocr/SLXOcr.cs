using System;

//Ocr
//using Cognex.VisionPro.ToolGroup;
//using Cognex.VisionPro.PMAlign;

namespace Idpsa
{
    //Ocr
    [Serializable]
    internal class SLXOcr
    {
        //#region private members
        //private CogToolGroup toolGroup;
        //#endregion

        //#region delegates
        //public delegate void MyFirstDelegate(object data);
        //#endregion
        //#region events
        //public event MyFirstDelegate ReadDone;
        //#endregion

        //public void LoadToolGroup()
        //{

        //    toolGroup = CogSerializer.LoadObjectFromFile("ocr.vpp") as CogToolGroup;
        //}
        //public void RunToolGroup(Bitmap bmp)
        //{

        //    object data = null;
        //    CogPMAlignTool aux;
        //    if (bmp != null)
        //    {
        //        CogImage8Grey image = new CogImage8Grey(bmp);
        //        foreach (ICogTool tool in toolGroup.Tools)
        //        {
        //            if (tool.GetType().Name.Equals("CogPMAlignTool"))
        //            {
        //                aux = tool as CogPMAlignTool;
        //                aux.InputImage = image;
        //            }
        //        }
        //        //toolGroup.DefineScriptTerminal(bmp, "InputImage", true);
        //        //toolGroup.SetScriptTerminalData("InputImage", bmp);            
        //        toolGroup.Run();
        //        toolGroup.GetScriptTerminalData("Decoded_String", ref data);
        //        if (data != null)
        //        {
        //            //Console.WriteLine("datos leídos:" + data.ToString());
        //            OnImageReady(data);
        //        }
        //    }

        //    //OnImageReady(data);
        //    ////CogSerializer.SaveObjectToFile(toolGroup, "c:\\tool.vpp");
        //}

        //public void OnImageReady(object data)
        //{
        //    if (ReadDone != null)
        //    {
        //        ReadDone(data);
        //    }
        //}
    }
}