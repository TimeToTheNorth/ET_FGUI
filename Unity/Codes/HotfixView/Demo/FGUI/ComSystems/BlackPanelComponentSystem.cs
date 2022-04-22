using FairyGUI;

namespace ET
{
    public class BlackPanelComponentAwakeSystem: AwakeSystem<BlackPanelComponent>
    {
        public override void Awake(BlackPanelComponent self)
        {
            self.Awake();
        }
    }

    public static class BlackPanelComponentSystem
    {
        public static void Awake(this BlackPanelComponent self)
        {
            self.blackPanel = UIPackage.CreateObject(UIPackageEnum.Public.ToString(), "Com_Black") as GComponent;

            //全屏界面适配
            self.blackPanel.SetSize(GRoot.inst.width, GRoot.inst.height);

            GRoot.inst.AddChild(self.blackPanel);
        }
        
        
        public static void Push(this BlackPanelComponent self, GObject p)
        {
            self.parents.Add(p);
            self.blackPanel.visible = true;
            self.blackPanel.sortingOrder = p.sortingOrder - 1;
        }

        public static void Pop(this BlackPanelComponent self)
        {
            if (self.parents.Count > 0)
            {
                self.parents.RemoveAt(self.parents.Count - 1);
                if (self.parents.Count > 0)
                {
                    self.blackPanel.sortingOrder = self.parents[self.parents.Count - 1].sortingOrder - 1;
                }
                else
                {
                    self.blackPanel.visible = false;
                }
            }
            else
            {
                self.blackPanel.visible = false;
            }
        }
        
        
        
    }
}