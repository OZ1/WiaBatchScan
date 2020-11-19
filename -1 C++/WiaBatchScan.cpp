#include <Windows.h>
#include <ShObjIdl.h>
#include <AtlBase.h>
#include <AtlComCli.h>
#include <ComDef.h>
#include <Wia.h>
#include <Sti.h>
#include <stdio.h>

#include <wil\resource.h>

_COM_SMARTPTR_TYPEDEF(ITaskbarList3      , __uuidof(ITaskbarList3      ));
_COM_SMARTPTR_TYPEDEF(IWiaDevMgr2        , __uuidof(IWiaDevMgr2        ));
_COM_SMARTPTR_TYPEDEF(IWiaItem2          , __uuidof(IWiaItem2          ));
_COM_SMARTPTR_TYPEDEF(IWiaPropertyStorage, __uuidof(IWiaPropertyStorage));
_COM_SMARTPTR_TYPEDEF(IWiaTransfer       , __uuidof(IWiaTransfer       ));
_COM_SMARTPTR_TYPEDEF(IEnumSTATPROPSTG   , __uuidof(IEnumSTATPROPSTG   ));

bool          volatile g_bCancel = false;
IWiaTransfer* volatile g_pWiaTransfer = nullptr;

BOOL WINAPI OnCtrl(_In_ DWORD CtrlType)
{
	g_bCancel = true;
	if (CComPtr<IWiaTransfer> pWiaTransfer = g_pWiaTransfer)
		LOG_IF_FAILED(pWiaTransfer->Cancel());
	return false;
}

class CWiaTransferCallback : public IWiaTransferCallback
{
	ULONG            m_cRef = 0;
public:
	ULONG            m_nFileNumber = 0;
	const wchar_t*   m_wszFileFormat;
	ITaskbarList3Ptr m_pTaskbarList3;
	HWND             m_hWnd = GetConsoleWindow();

	CWiaTransferCallback(const wchar_t* wszFileFormat = L"%u.jpg") : m_wszFileFormat(wszFileFormat)
	{
		wchar_t wszFileName[MAX_PATH];
		do swprintf_s(wszFileName, wszFileFormat, ++m_nFileNumber);
		while (GetFileAttributes(wszFileName) != INVALID_FILE_ATTRIBUTES);
		auto dwError = GetLastError();
		if  (dwError != ERROR_FILE_NOT_FOUND)
			THROW_WIN32(dwError);
		THROW_IF_FAILED(m_pTaskbarList3.CreateInstance(CLSID_TaskbarList));
		THROW_IF_FAILED(m_pTaskbarList3->HrInit());
	}

	virtual HRESULT __stdcall QueryInterface(REFIID riid, void** ppvObject) override
	{
		if (IsEqualIID(riid, IID_IUnknown))
			*ppvObject = static_cast<IUnknown*>(this);
		else
		if (IsEqualIID(riid, IID_IWiaTransferCallback))
			*ppvObject = static_cast<IWiaTransferCallback*>(this);
		else return E_NOTIMPL;
		return S_OK;
	}
	virtual ULONG __stdcall AddRef(void) override
	{
		return InterlockedIncrement(&m_cRef);
	}
	virtual ULONG __stdcall Release(void) override
	{
		if (auto cRef = InterlockedDecrement(&m_cRef))
			return cRef;
	//	delete this;
		return 0;
	}

private:
	virtual HRESULT __stdcall TransferCallback(LONG lFlags, WiaTransferParams* pWiaTransferParams) override
	{
		switch (pWiaTransferParams->lMessage)
		{
		case WIA_TRANSFER_MSG_STATUS:
			LOG_IF_FAILED(m_pTaskbarList3->SetProgressValue(m_hWnd, pWiaTransferParams->lPercentComplete, 100));
			break;
		case WIA_TRANSFER_MSG_END_OF_STREAM:
			LOG_IF_FAILED(m_pTaskbarList3->SetProgressState(m_hWnd, TBPF_NOPROGRESS));
			break;
		case WIA_TRANSFER_MSG_END_OF_TRANSFER:
			m_nFileNumber++;
			break;
		case WIA_TRANSFER_MSG_DEVICE_STATUS:
			wprintf(L"WIA_TRANSFER_MSG_DEVICE_STATUS 0x%X\n", pWiaTransferParams->hrErrorStatus);
			break;
		case WIA_TRANSFER_MSG_NEW_PAGE:
			_putws(L"WIA_TRANSFER_MSG_NEW_PAGE");
			break;
		}
		return g_bCancel ? S_FALSE : S_OK;
	}

	virtual HRESULT __stdcall GetNextStream(LONG lFlags, BSTR bstrItemName, BSTR bstrFullItemName, IStream** ppDestination) override
	{
		wchar_t wszFileName[MAX_PATH];
		for (;;)
		{
			swprintf_s(wszFileName, m_wszFileFormat, m_nFileNumber);
			if (GetFileAttributes(wszFileName) == INVALID_FILE_ATTRIBUTES) break;
			else m_nFileNumber++;
		}
		wprintf(L"GetNextStream(%s, %s) > %ws\n", bstrItemName, bstrFullItemName, wszFileName);

		SetConsoleTitle(wszFileName);
		LOG_IF_FAILED(m_pTaskbarList3->SetProgressState(m_hWnd, TBPF_INDETERMINATE));
		return SHCreateStreamOnFile(wszFileName, STGM_CREATE | STGM_READWRITE, ppDestination);
	}
};

static wil::unique_cotaskmem_string StringFromCLSID(REFCLSID clsid)
{
	wil::unique_cotaskmem_string wsz;
	THROW_IF_FAILED(StringFromCLSID(clsid, &wsz));
	return wsz;
}

void Print(IWiaPropertyStorage* pWiaPropertyStorage)
{
	CComPtr<IEnumSTATPROPSTG> pEnum;
	THROW_IF_FAILED(pWiaPropertyStorage->Enum(&pEnum));
	THROW_IF_FAILED(pEnum->Reset());
	for (HRESULT hr;;)
	{
		ULONG celtFetched;
		STATPROPSTG StatPropStg{};
		THROW_IF_FAILED(hr = pEnum->Next(1, &StatPropStg, &celtFetched));
		wil::unique_cotaskmem_string wstrName(StatPropStg.lpwstrName);
		auto vt = static_cast<VARENUM>(StatPropStg.vt);
		if (hr != S_OK) break;

		wprintf(L"%4x %-30s ", StatPropStg.propid, StatPropStg.lpwstrName);
			
		wil::unique_prop_variant PropVariant;
		PROPSPEC PropSpec = { PRSPEC_PROPID, StatPropStg.propid };
		THROW_IF_FAILED(hr = pWiaPropertyStorage->ReadMultiple(1, &PropSpec, &PropVariant));
		switch (PropVariant.vt)
		{
		case VT_I1               : wprintf(L"VT_I1       %hhd\n"       , PropVariant.cVal); break;
		case VT_I2               : wprintf(L"VT_I2       %hd\n"        , PropVariant.iVal); break;
		case VT_I4               : wprintf(L"VT_I4       %d\n"         , PropVariant.lVal); break;
		case VT_INT              : wprintf(L"VT_INT      %ld\n"        , PropVariant.intVal); break;
		case VT_I8               : wprintf(L"VT_I8       %lld\n"       , PropVariant.hVal.QuadPart); break;
		case VT_UI1              : wprintf(L"VT_UI1      0x%hhX %hhu\n", PropVariant.bVal          , PropVariant.bVal          ); break;
		case VT_UI2              : wprintf(L"VT_UI2      0x%hX %hu\n"  , PropVariant.uiVal         , PropVariant.uiVal         ); break;
		case VT_UI4              : wprintf(L"VT_UI4      0x%X %u\n"    , PropVariant.ulVal         , PropVariant.ulVal         ); break;
		case VT_UINT             : wprintf(L"VT_UINT     0x%lX %lu\n"  , PropVariant.uintVal       , PropVariant.uintVal       ); break;
		case VT_UI8              : wprintf(L"VT_UI8      0x%llX %llu\n", PropVariant.uhVal.QuadPart, PropVariant.uhVal.QuadPart); break;
		case VT_R4               : wprintf(L"VT_R4       %g\n"         , PropVariant.fltVal); break;
		case VT_R8               : wprintf(L"VT_R8       %g\n"         , PropVariant.dblVal); break;
		case VT_BSTR             : wprintf(L"VT_BSTR     %s\n"         , PropVariant.bstrVal); break;
		case VT_BOOL             : wprintf(L"VT_BOOL     %s\n"         , PropVariant.boolVal ? L"VARIANT_TRUE" : L"VARIANT_FALSE"); break;
		case VT_ERROR            : wprintf(L"VT_ERROR    0x%X\n"       , PropVariant.scode); break;
		case VT_CY               : wprintf(L"VT_CY       %lld\n"       , PropVariant.cyVal.int64); break;
		case VT_DATE             : wprintf(L"VT_DATE     %g\n"         , PropVariant.date); break;
		case VT_UNKNOWN          : wprintf(L"VT_UNKNOWN  %p\n"         , PropVariant.punkVal); break;
		case VT_DISPATCH         : wprintf(L"VT_DISPATCH %p\n"         , PropVariant.pdispVal); break;
	//	case VT_DECIMAL          : wprintf(L"VT_DECIMAL  %p\n"         , PropVariant.decVal); break;
	//	case VT_RECORD           : wprintf(L"VT_RECORD   %p\n"         , PropVariant.brecVal); break;
	//	case VT_ARRAY            : wprintf(L"VT_ARRAY    %p\n"         , PropVariant.parray); break;
		case VT_LPWSTR           : wprintf(L"VT_LPWSTR   %s\n"         , PropVariant.pwszVal); break;
		case VT_LPSTR            : wprintf(L"VT_LPSTR    %S\n"         , PropVariant.pszVal); break;
		case VT_CLSID            : wprintf(L"VT_CLSID    %s\n"         , StringFromCLSID(*PropVariant.puuid).get()); break;
		case VT_I1      |VT_BYREF: wprintf(L"VT_I1      |VT_BYREF %hhd\n"       , *PropVariant.pcVal); break;
		case VT_I2      |VT_BYREF: wprintf(L"VT_I2      |VT_BYREF %hd\n"        , *PropVariant.piVal); break;
		case VT_I4      |VT_BYREF: wprintf(L"VT_I4      |VT_BYREF %d\n"         , *PropVariant.plVal); break;
		case VT_INT     |VT_BYREF: wprintf(L"VT_INT     |VT_BYREF %ld\n"        , *PropVariant.pintVal); break;
		case VT_UI1     |VT_BYREF: wprintf(L"VT_UI1     |VT_BYREF 0x%hhX %hhu\n", *PropVariant.pbVal   , *PropVariant.pbVal   ); break;
		case VT_UI2     |VT_BYREF: wprintf(L"VT_UI2     |VT_BYREF 0x%hX %hu\n"  , *PropVariant.puiVal  , *PropVariant.puiVal  ); break;
		case VT_UI4     |VT_BYREF: wprintf(L"VT_UI4     |VT_BYREF 0x%X %u\n"    , *PropVariant.pulVal  , *PropVariant.pulVal  ); break;
		case VT_UINT    |VT_BYREF: wprintf(L"VT_UINT    |VT_BYREF 0x%lX %lu\n"  , *PropVariant.puintVal, *PropVariant.puintVal); break;
		case VT_R4      |VT_BYREF: wprintf(L"VT_R4      |VT_BYREF %g\n"         , *PropVariant.pfltVal); break;
		case VT_R8      |VT_BYREF: wprintf(L"VT_R8      |VT_BYREF %g\n"         , *PropVariant.pdblVal); break;
		case VT_BSTR    |VT_BYREF: wprintf(L"VT_BSTR    |VT_BYREF %s\n"         , *PropVariant.pbstrVal); break;
		case VT_BOOL    |VT_BYREF: wprintf(L"VT_BOOL    |VT_BYREF %s\n"         , *PropVariant.pboolVal ? L"VARIANT_TRUE" : L"VARIANT_FALSE"); break;
		case VT_ERROR   |VT_BYREF: wprintf(L"VT_ERROR   |VT_BYREF 0x%X\n"       , *PropVariant.pscode); break;
		case VT_CY      |VT_BYREF: wprintf(L"VT_CY      |VT_BYREF %lld\n"       ,  PropVariant.pcyVal->int64); break;
		case VT_DATE    |VT_BYREF: wprintf(L"VT_DATE    |VT_BYREF %g\n"         , *PropVariant.pdate); break;
		case VT_UNKNOWN |VT_BYREF: wprintf(L"VT_UNKNOWN |VT_BYREF %p\n"         , *PropVariant.ppunkVal); break;
		case VT_DISPATCH|VT_BYREF: wprintf(L"VT_DISPATCH|VT_BYREF %p\n"         , *PropVariant.ppdispVal); break;
	//	case VT_ARRAY   |VT_BYREF: wprintf(L"VT_ARRAY   |VT_BYREF %p\n"         %p\n"         , *PropVariant.pparray); break;
	//	case VT_VARIANT |VT_BYREF: wprintf(L"VT_VARIANT |VT_BYREF %p\n"         %p\n"         , *PropVariant.pvarVal); break;
	//	case VT_DECIMAL |VT_BYREF: wprintf(L"VT_DECIMAL |VT_BYREF %p\n"         %p\n"         , *PropVariant.pdecVal); break;
	//	case VT_RECORD  |VT_BYREF: wprintf(L"VT_RECORD  |VT_BYREF , *PropVariant.pbrecVal); break;
		case VT_EMPTY            : wprintf(L"VT_EMPTY\n"); break;
		case VT_NULL             : wprintf(L"VT_NULL\n"); break;
		default                  : wprintf(L"VT_%u\n", StatPropStg.vt); break;
		}
	}
}

int wmain(int argc, wchar_t* argv[])
{
	auto CoUnInit = wil::CoInitializeEx(COINIT_APARTMENTTHREADED);

	CComPtr<IWiaDevMgr2> pWiaDevMgr2;
	THROW_IF_FAILED(pWiaDevMgr2.CoCreateInstance(CLSID_WiaDevMgr2, nullptr, CLSCTX_LOCAL_SERVER));

	HRESULT hr;
	CComPtr<IWiaItem2> pWiaItem2;
	THROW_IF_FAILED(hr = pWiaDevMgr2->GetImageDlg(0, nullptr, GetConsoleWindow(), BSTR(L"."), BSTR(L"образец"), nullptr, nullptr, &pWiaItem2));
	if (hr == S_OK)
	{
		Print(CComQIPtr<IWiaPropertyStorage>(pWiaItem2));

		CComQIPtr<IWiaTransfer> pWiaTransfer = static_cast<IWiaItem2*>(pWiaItem2);

		g_pWiaTransfer = pWiaTransfer;

		LOG_IF_WIN32_BOOL_FALSE(SetConsoleCtrlHandler(OnCtrl, true));

		CWiaTransferCallback WiaTransferCallback(argc < 2 ? L"%u.jpg" : argv[1]);

		for(HRESULT hr = S_OK; hr == S_OK;)
			THROW_IF_FAILED(hr = pWiaTransfer->Download(0, &WiaTransferCallback));

		g_pWiaTransfer = nullptr;
	}
	return hr;
}
