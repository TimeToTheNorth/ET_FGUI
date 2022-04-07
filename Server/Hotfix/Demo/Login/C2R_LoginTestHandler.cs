using System;

namespace ET.Login
{
    
    [MessageHandler]
    public class C2R_LoginTestHandler:AMRpcHandler<C2R_LoginTest,R2C_LoginTest>
    {
        protected override async ETTask Run(Session session, C2R_LoginTest request, R2C_LoginTest response, Action reply)
        {
            response.key = "111111111111111";
            reply();//会向客户端发送R2C_LoginTest
            await ETTask.CompletedTask;
        }
    }
}