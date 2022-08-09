using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class UIGuildPopNoGuild:UIWindow
    {
    public void OnCreateGuild() {
        UIManager.Instance.Show<UIGuildPopCreate>();
        UIManager.Instance.Close(typeof(UIGuildPopNoGuild));
    }
    public void OnJoinGuild() {
        UIManager.Instance.Show<UIGuildList>();
        UIManager.Instance.Close(typeof(UIGuildPopNoGuild));
    }

    }

