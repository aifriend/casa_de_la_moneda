using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Runtime.CompilerServices;
namespace Pasaportes.Extensions
{
	public static class MyExtenxions
	{
        public static string Chars(this string  s,int index){
            return s.Substring(index, 1);
        }

        public static int ToInt(this bool b)
        {
            return ((b = true) ? 1 : 0);

        }
        
        public static bool ToBool(this int b)
        {
           return ((b!=0)?true:false);

        }
		//  Public Function SubGoStep(ByVal Cad As clsCadenas)
		//  End Function
	}
}
