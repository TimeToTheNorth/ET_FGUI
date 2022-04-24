using System.Collections.Generic;
using FairyGUI;

namespace ET
{
    public class BlackPanelComponent:Entity,IAwake
    {
        public List<GObject> parents = new List<GObject>();
        public GComponent blackPanel;
    }
}