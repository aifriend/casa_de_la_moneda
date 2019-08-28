#ifndef HKETN20H_H
  #define HKETN20H_H

#define ERR_SUCCES		0
#define ERR_USERCANCEL		1
#define ERR_INVALID_PARAM	2
#define ERR_INVALID_MEMORY	3

#define ERR_ENDTIMEOUT		-1

#define HKETN20H_API _stdcall

#ifdef __cplusplus
extern "C" {
#endif

HANDLE HKETN20H_API HKEtn20_Open(unsigned short wPort, int iRetry, int iRecvTime);

void HKETN20H_API HKEtn20_Close(HANDLE hHandle);

int HKETN20H_API HKEtn20_ReadInternalMemory(HANDLE hHandle,
		 			char* lpAddr,
		 			unsigned short wPort,
					WORD* pValue, 
					unsigned short iWordcnt, 
					int  iDeviceType, 
					DWORD dwAddr);

int HKETN20H_API HKEtn20_WriteInternalMemory(HANDLE hHandle,
 					char* lpAddr,
 					unsigned short wPort,
					WORD* pValue, 
					unsigned short iWordcnt, 
					int  iDeviceType, 
					DWORD dwAddr);

int HKETN20H_API HKEtn20_ReadInternalMemory2(HANDLE hHandle,
		 			char* lpAddr,
		 			unsigned short wPort,
					DWORD* pValue, 
					unsigned short iWordcnt, 
					int  iDeviceType, 
					DWORD dwAddr);

int HKETN20H_API HKEtn20_WriteInternalMemory2(HANDLE hHandle,
 					char* lpAddr,
 					unsigned short wPort,
					DWORD* pValue, 
					unsigned short iWordcnt, 
					int  iDeviceType, 
					DWORD dwAddr);

int HKETN20H_API HKEtn20_WriteInternalBitMemory(HANDLE hHandle,
 					char* lpAddr,
			 		unsigned short wPort,
					int  iDeviceType, 
					DWORD dwAddr,
					int  iBitNo,
					int  iOn);


int HKETN20H_API HKEtn20_ReadCardMemory(HANDLE hHandle, 
 					char* lpAddr, 
 					unsigned short wPort, 
					WORD* pValue, 
					unsigned short iWordcnt, 
					int iFileNo, 
					int iRecordNo, 
					DWORD dwAddr);


int HKETN20H_API HKEtn20_WriteCardMemory(HANDLE hHandle,
 					char* lpAddr,
 					unsigned short wPort,
					WORD* pValue, 
					unsigned short iWordcnt, 
					int iFileNo, 
					int iRecordNo, 
					DWORD dwAddr);


int HKETN20H_API HKEtn20_WriteCardBitMemory(HANDLE hHandle,
 					char* lpAddr,
 					unsigned short wPort,
					int iFileNo, 
					int iRecordNo, 
					DWORD dwAddr,
					int  iBitNo,
					int  iOn);


int HKETN20H_API HKEtn20_ReadPlcMemory(HANDLE hHandle,
 					char* lpAddr,
 					unsigned short wPort,
					WORD* pValue, 
					unsigned short iWordcnt, 
					int iMemType , 
					int iTermNo,
					int iDeviceType, 
					DWORD dwAddr, 
					int iExNo);


int HKETN20H_API HKEtn20_WritePlcMemory(HANDLE hHandle,
 					char* lpAddr,
 					unsigned short wPort,
					WORD* pValue, 
					unsigned short iWordcnt, 
					int iMemType , 
					int iTermNo,
					int iDeviceType, 
					DWORD dwAddr, 
					int iExNo);


int HKETN20H_API HKEtn20_ReadPlcMemory2(HANDLE hHandle,
 					char* lpAddr,
 					unsigned short wPort,
					DWORD* pValue, 
					unsigned short iWordcnt, 
					int iMemType , 
					int iTermNo,
					int iDeviceType, 
					DWORD dwAddr, 
					int iExNo);


int HKETN20H_API HKEtn20_WritePlcMemory2(HANDLE hHandle,
 					char* lpAddr,
 					unsigned short wPort,
					DWORD* pValue, 
					unsigned short iWordcnt, 
					int iMemType , 
					int iTermNo,
					int iDeviceType, 
					DWORD dwAddr, 
					int iExNo);


int HKETN20H_API HKEtn20_WritePlcBitMemory(HANDLE hHandle,
 					char* lpAddr,
 					unsigned short wPort,
					int iMemType , 
					int iTermNo,
					int iDeviceType, 
					DWORD dwAddr,
					int iBitNo,
					int iOn, 
					int iExNo);


int HKETN20H_API HKEtn20_Recvfrom(HANDLE hHandle,
				char *lpIPStr, 
				unsigned short 	*lpPort,
				unsigned char 	*lpRcvData,
				unsigned long	*lpdwSize);


int HKETN20H_API HKEtn20_ReadSampleMemory(HANDLE hHandle,
				 	char* lpAddr,
				 	unsigned short wPort,
					WORD* pValue,
					int iValueSize,
					int iBufNo,
					int iSmpNo,
					int iBlockCnt,
					int iFg,
					BYTE* pSubCode,
					int* pReadBlockCnt,
					long* pReadCnt);


int HKETN20H_API HKEtn20_ReadInternalMemoryEx(HANDLE hHandle,
		 			char* lpAddr,
		 			unsigned short wPort,
					WORD* pValue, 
					unsigned short iWordcnt, 
					int  iDeviceType, 
					DWORD dwAddr,
					int iExNo);


int HKETN20H_API HKEtn20_WriteInternalMemoryEx(HANDLE hHandle,
 					char* lpAddr,
 					unsigned short wPort,
					WORD* pValue, 
					unsigned short iWordcnt, 
					int  iDeviceType, 
					DWORD dwAddr,
					int iExNo);


int HKETN20H_API HKEtn20_ReadInternalMemoryEx2(HANDLE hHandle,
		 			char* lpAddr,
		 			unsigned short wPort,
					DWORD* pValue, 
					unsigned short iWordcnt, 
					int  iDeviceType, 
					DWORD dwAddr,
					int iExNo);


int HKETN20H_API HKEtn20_WriteInternalMemoryEx2(HANDLE hHandle,
 					char* lpAddr,
 					unsigned short wPort,
					DWORD* pValue, 
					unsigned short iWordcnt, 
					int  iDeviceType, 
					DWORD dwAddr,
					int iExNo);


int HKETN20H_API HKEtn20_WriteInternalBitMemoryEx(HANDLE hHandle,
 					char* lpAddr,
			 		unsigned short wPort,
					int  iDeviceType, 
					DWORD dwAddr,
					int  iExNo,
					int  iBitNo,
					int  iOn);


void HKETN20H_API HKEtn20_Cancel();


int HKETN20H_API HKEtn20_GetLastError();


#ifdef __cplusplus
}
#endif

#endif // HKETN20H_H