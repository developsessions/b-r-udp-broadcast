#include <bur/plctypes.h>
#include <stdbool.h>

#ifdef _DEFAULT_INCLUDES
	#include <AsDefault.h>
#endif

void _INIT ProgramInit(void)
{
	UdpOpen_0.enable = true;
}

void _CYCLIC ProgramCyclic(void)
{
	//UdpOpen_0.pIfAddr = ;
	UdpOpen_0.port = 4711;
	//UdpOpen_0.options = ;
	UdpOpen(&UdpOpen_0);
	
	if(UdpOpen_0.status == ERR_OK)
		UdpOpen_0.enable = false;
	
	UdpRecv_0.enable = !UdpOpen_0.enable;
	UdpRecv_0.ident = UdpOpen_0.ident;
	UdpRecv_0.pData = (UDINT)&RecvBuffer[0];
	UdpRecv_0.datamax = sizeof(RecvBuffer);
	//UdpRecv_0.flags = ;
	UdpRecv_0.pIpAddr = (UDINT)&IpAddr;
	UdpRecv(&UdpRecv_0);
	
	if(UdpRecv_0.status == ERR_OK)
	{
		RecvCounter++;
		RecvLen = UdpRecv_0.recvlen;
		UdpSend_0.enable = true;
	}
	
	UdpSend_0.ident = UdpOpen_0.ident;
	UdpSend_0.pHost = (UDINT)&IpAddr;
	UdpSend_0.port = 4712;
	UdpSend_0.pData = (UDINT)&RecvBuffer[0];
	UdpSend_0.datalen = RecvLen;
	//UdpSend_0.flags = ;
	UdpSend(&UdpSend_0);
	
	if(UdpSend_0.status == ERR_OK)
		UdpSend_0.enable = false;
}

void _EXIT ProgramExit(void)
{
	do
	{
		UdpClose_0.enable = true;
		UdpClose_0.ident = UdpOpen_0.ident;
		UdpClose(&UdpClose_0);
	}while(UdpClose_0.status == ERR_FUB_BUSY);
}
