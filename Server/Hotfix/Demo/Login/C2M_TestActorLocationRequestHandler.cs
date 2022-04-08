using System;

namespace ET.Login
{
    [ActorMessageHandler]
    public class C2M_TestActorLocationRequestHandler:AMActorLocationRpcHandler<Unit,C2M_TestActorLocationReqeust,M2C_TestActorLocationResponse>
    {
        protected override async ETTask Run(Unit unit, C2M_TestActorLocationReqeust request, M2C_TestActorLocationResponse response, Action reply)
        {
            Log.Debug(request.Content);
            response.Content = "aaaaaaaa";
            reply();
            await ETTask.CompletedTask;
        }
    }
}