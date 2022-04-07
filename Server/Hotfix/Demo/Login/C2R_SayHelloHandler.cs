namespace ET.Login
{
    [MessageHandler]
    public class C2R_SayHelloHandler: AMHandler<C2R_SayHello>
    {
        protected override async ETTask Run(Session session, C2R_SayHello message)
        {
            Log.Debug(message.Hello);
            
            session.Send(new R2C_SayGoodBye()
            {
                GoodBye = "goodbye"
            });
            await ETTask.CompletedTask;
        }
    }
}