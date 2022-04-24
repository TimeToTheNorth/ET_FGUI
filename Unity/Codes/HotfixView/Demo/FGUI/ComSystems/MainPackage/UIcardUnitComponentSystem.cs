using FairyGUI;

namespace ET
{
    public class UIcardUnitComponentAwakeSystem: AwakeSystem<UIcardUnitComponent, FairyGUI.GButton>
    {
        public override void Awake(UIcardUnitComponent self, FairyGUI.GButton a)
        {
            self.Awake(a);
        }
    }

    public static class UIcardUnitComponentSystem
    {
        //todo 扩展组件的awake组件 并没有自动添加实现 因为组件的awake 往往是跟随ui的开的 根据实际情况自行添加扩展组件即可 这个时候数据已经被准备好了
        public static void Awake(this UIcardUnitComponent self, FairyGUI.GButton gComponent)
        {
            self.UIcardUnitComponentData = (UIcardUnitComponentData)gComponent;
        }
    }
}