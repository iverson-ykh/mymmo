using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



    public class UISetting:UIWindow
    {
    public void ExitToCharSelect() {
        //UIPopCharMenu menu = UIManager.Instance.Show<UIPopCharMenu>();

        SceneManager.Instance.LoadScene("CharSelect");
        SoundManager.Instance.PlayMusic(SoundDefine.Music_Select);
        Services.UserService.Instance.SendGameLeave();
    }
    public void SystemConfig() {
        UIManager.Instance.Show<UISystemConfig>();
        this.Close();    
    }

    public void ExitGame() {
        Services.UserService.Instance.SendGameLeave(true);
    }
    }

