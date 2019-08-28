#include "stdafx.h"
#include "MonitorWrapper.h"
#include "HKEtn20h.h"

#pragma once
using namespace System;

namespace MonitorWrapper
{
	public ref class CheckPoint
	{
	public:
		CheckPoint () 
		{
		};

		static bool IsInstalled()
		{
			bool installed = false;
			HMODULE hLib = LoadLibrary(TEXT("HKEtn20.dll"));	// Search for instaled lib at \windows\system32
			if(hLib != NULL)
			{
				FARPROC proc = GetProcAddress(hLib, "HKEtn20_Open");	// Choose any function...
				installed = (proc != NULL);
				FreeLibrary(hLib);
			}
			return installed;
		}

	};

	public ref class Comunication 
	{
	public:
		static HANDLE hHandle;

		Comunication () 
		{
		};

        static HANDLE GetCom()
		{
			return hHandle;
		}

		static bool Open()
		{
			hHandle = HKEtn20_Open(10000, 1, 1);
			if (hHandle == NULL) {
				return false;
			}
			return true;
		}
		static void Close()
		{
			return HKEtn20_Close(hHandle);
		}
	};

	public ref class SendData
	{
	public:
		SendData () 
		{
		};

		static int Update(DWORD wValue, WORD usWordCnt, DWORD dwMemAdr)
		{
			int	iDevType = 0;
			char*	pIpAdr	= "192.168.1.42";
 			unsigned short	usPort	= 10000;
			HANDLE	hHandle	= HKEtn20_Open(usPort, 3, 1);

			int iRet = 0;
			if (hHandle != NULL) 
			{
				iRet = HKEtn20_WriteInternalMemory2(hHandle, pIpAdr, usPort, &wValue, usWordCnt, iDevType, dwMemAdr);
				HKEtn20_Close(hHandle);
			} 
			else {
				iRet = HKEtn20_GetLastError();
			}
			return iRet;
		}

		static int ReadData(WORD usWordCnt, DWORD dwMemAdr)
		{
			int	  iDevType  = 0;
			WORD	wValue	= 0;
			char*	pIpAdr	= "192.168.1.42";
 			unsigned short	usPort	= 10000;
			HANDLE	hHandle	= HKEtn20_Open(usPort, 3, 1);

			if (hHandle != NULL) 
			{
				int iRet = HKEtn20_ReadInternalMemory(hHandle, pIpAdr, usPort, &wValue, 1/*usWordCnt*/, iDevType, dwMemAdr);
				HKEtn20_Close(hHandle);
			} 
			return wValue;
		}
	};
}
