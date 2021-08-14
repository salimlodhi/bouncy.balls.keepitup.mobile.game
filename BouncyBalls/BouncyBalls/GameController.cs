using System;
using System.Collections.Generic;
using CocosSharp;
using CocosDenshion;
using bouncy.balls.keepitup.Scenes;

namespace bouncy.balls.keepitup
{
    public static class GameController
    {
        public static CCGameView GameView
        {
            get;
            private set;
        }

        public static void Initialize(CCGameView gameView)
        {
            if (gameView != null)
            {
                CCSizeI viewSize = gameView.ViewSize;

                var contentSearchPaths = new List<string>() { "Fonts", "Sounds" };

#if __IOS__
                      contentSearchPaths.Add("Sounds/iOS/");
#else // android
                contentSearchPaths = new List<string>() { "Fonts", "Sounds" };
#endif
                //CCSizeI smallResource = new CCSizeI(320, 480);
                //CCSizeI mediumResource = new CCSizeI(768, 1024);
                //CCSizeI largeResource = new CCSizeI(1536, 2048);

                //int desireWidth = 768;
                //int desireHeight = 1024;

                CCSizeI smallResource = new CCSizeI(360, 570);
                CCSizeI mediumResource = new CCSizeI(720, 1140);
                CCSizeI largeResource = new CCSizeI(1440, 2280);

                int desireWidth = 720;
                int desireHeight = 1140;

                if (viewSize.Height > mediumResource.Height)
                {
                    contentSearchPaths.Add("Images/Hd");
                    CCSprite.DefaultTexelToContentSizeRatio = (largeResource.Height / desireHeight);
                }
                else //if (viewSize.Height > smallResource.Height)
                {
                    contentSearchPaths.Add("Images/Ld");
                    CCSprite.DefaultTexelToContentSizeRatio = (mediumResource.Height / desireHeight);
                }
                //else
                //{
                //    contentSearchPaths.Add("Images/Sd");
                //    CCSprite.DefaultTexelToContentSizeRatio = (smallResource.Height / desireHeight);
                //}
                gameView.ContentManager.SearchPaths = contentSearchPaths;

                // Set world dimensions 
                gameView.DesignResolution = new CCSizeI(desireWidth, desireHeight);
                gameView.ResolutionPolicy = CCViewResolutionPolicy.ExactFit;
                GameView = gameView;

                InitializeAudio();

                var scene = new TitleScene(GameView);
                GameView.RunWithScene(scene);
            }
        }

        private static void InitializeAudio()
        {
            //CCAudioEngine.SharedEngine.PlayBackgroundMusic("FruityFallsSong");
        }

        public static void GoToScene(CCScene scene)
        {
            GameView.Director.ReplaceScene(scene);
        }
    }
}