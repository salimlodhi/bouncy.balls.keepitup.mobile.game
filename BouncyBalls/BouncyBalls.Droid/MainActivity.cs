using System;
using Android.App;
using Android.Content.PM;
using Android.OS;

using CocosSharp;
using bouncy.balls.keepitup;


namespace bouncy.balls.keepitup.Droid
{
    [Activity(
       Label = "Bouncy Balls - Keep it Up!",
       AlwaysRetainTaskState = true,
       Icon = "@drawable/icon",
       Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
       ScreenOrientation = ScreenOrientation.Portrait,
       LaunchMode = LaunchMode.SingleTask,
       MainLauncher = false,
       ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)
       ]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our game view from the layout resource,
            // and attach the view created event to it
            System.Threading.Thread.Sleep(500);
            CCGameView gameView = (CCGameView)FindViewById(Resource.Id.GameView);
            gameView.ViewCreated += LoadGame;

        }

        void LoadGame(object sender, EventArgs e)
        {
            CCGameView gameView = sender as CCGameView;

            if (gameView != null)
            {
                GameController.Initialize(gameView);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
           // CCAudioEngine.SharedEngine.PauseBackgroundMusic();
        }

        protected override void OnResume()
        {
            base.OnResume();
           // CCAudioEngine.SharedEngine.ResumeBackgroundMusic();
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}

