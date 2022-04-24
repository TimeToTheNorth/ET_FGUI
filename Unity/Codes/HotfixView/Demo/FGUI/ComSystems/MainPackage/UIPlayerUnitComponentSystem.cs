using FairyGUI;

namespace ET
{
    public class UIPlayerUnitComponentAwakeSystem: AwakeSystem<UIPlayerUnitComponent, FairyGUI.GComponent>
    {
        public override void Awake(UIPlayerUnitComponent self, FairyGUI.GComponent a)
        {
            self.Awake(a);
        }
    }

    public static class UIPlayerUnitComponentSystem
    {
        //todo 扩展组件的awake组件 并没有自动添加实现 因为组件的awake 往往是跟随ui的开的 根据实际情况自行添加扩展组件即可 这个时候数据已经被准备好了
        public static void Awake(this UIPlayerUnitComponent self, FairyGUI.GComponent gComponent)
        {
            self.UIPlayerDataUnitComponentData = (UIPlayerDataUnitComponentData)gComponent;
        }
    }
}