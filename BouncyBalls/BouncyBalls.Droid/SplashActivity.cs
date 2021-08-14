using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using bouncy.balls.keepitup;

namespace bouncy.balls.keepitup.Droid
{
    [Activity(
        Label = "Bouncy Balls - Keep it up!",
        Theme = "@style/Theme.Splash", //Indicates the theme to use for this activity
              MainLauncher = true, //Set it as boot activity
              NoHistory = true)] 
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            System.Threading.Thread.Sleep(2000); //Let's wait awhile...
            this.StartActivity(typeof(MainActivity));
        }
    }
}