using System;
using CocosSharp;
using bouncy.balls.keepitup.Entities;
using System.Collections.Generic;

namespace bouncy.balls.keepitup.Scenes
{
	public class GameOverScene : CCScene
	{
        CCLayer layer;
        CCLabel scoreText, coinsText, bestScoreText;
        CCRect bounds;
        CCSprite homeButtonSprite, replayButtonSprite, close;


        public GameOverScene(CCGameView gameView) : base(gameView)
        {
            layer = new CCLayer();
            this.AddLayer(layer);
            bounds = layer.VisibleBoundsWorldspace;

            CreateBackground();
            AddBall();
            AddToScene();

            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = HandleTouchesEnd;
            layer.AddEventListener(touchListener);
        }

        private void CreateBackground()
        {
            var background = new CCSprite("gameoverbg");
            background.AnchorPoint = new CCPoint(0, 0);
            background.IsAntialiased = true;
            layer.AddChild(background);

            close = new CCSprite("close");
            close.AnchorPoint = new CCPoint(0, 0);
            close.IsAntialiased = true;
            close.PositionY = bounds.MaxY -60;
            close.PositionX = bounds.MaxX - 60;
           // layer.AddChild(close);
        }

        private void AddBall()
        {
            var ballSprite = new BallSprite("ball", 2.0f, -360);
            ballSprite.PositionY = bounds.Center.Y + 375;
            ballSprite.PositionX = bounds.Center.X;
            layer.AddChild(ballSprite);
        }

        private void AddToScene()
        {
            #region Score and coins
            GameSettings.HighestScore = CCUserDefault.SharedUserDefault.GetIntegerForKey("HighScore");
            GameSettings.CoinsScore = CCUserDefault.SharedUserDefault.GetIntegerForKey("CoinsScore");
            if (GameSettings.HighestScore < GameSettings.CurrentScore)
            {
                GameSettings.HighestScore = GameSettings.CurrentScore;
                CCUserDefault.SharedUserDefault.SetIntegerForKey("HighScore", GameSettings.HighestScore);
                CCUserDefault.SharedUserDefault.Flush();
            }

            if (GameSettings.CurrentScore >= GameSettings.CoinValue)
            {
                GameSettings.CoinsScore = (GameSettings.CoinsScore + (GameSettings.CurrentScore / GameSettings.CoinValue));
                CCUserDefault.SharedUserDefault.SetIntegerForKey("CoinsScore", GameSettings.CoinsScore);
                CCUserDefault.SharedUserDefault.Flush();
            }

            scoreText = new CCLabel((GameSettings.CurrentScore.ToString()), "MarkerFelt-22", 22, CCLabelFormat.SpriteFont);
            bestScoreText = new CCLabel((GameSettings.HighestScore.ToString()), "MarkerFelt-22", 11, CCLabelFormat.SpriteFont);

            coinsText = new CCLabel(GameSettings.CoinsScore.ToString(), "MarkerFelt-22", 21, CCLabelFormat.SpriteFont);
            coinsText.HorizontalAlignment = CCTextAlignment.Left;
            scoreText.Color = CCColor3B.Red;
            bestScoreText.Color = CCColor3B.Black;
            coinsText.Color = CCColor3B.Black;

            scoreText.PositionX = bounds.MidX + 100;
            scoreText.PositionY = bounds.MidY + 35;

            bestScoreText.PositionX = bounds.MidX + 100;
            bestScoreText.PositionY = bounds.MidY - 60;

            coinsText.PositionX = bounds.MidX - 140;
            coinsText.PositionY = bounds.MidY - 60;

            layer.AddChild(scoreText);
            layer.AddChild(bestScoreText);
            layer.AddChild(coinsText);


            #endregion


            homeButtonSprite = new CCSprite("home");
            replayButtonSprite = new CCSprite("replay");

            homeButtonSprite.PositionX = bounds.MidX - 90;
            homeButtonSprite.PositionY = bounds.MidY - 220;

            replayButtonSprite.PositionX = bounds.MidX + 90;
            replayButtonSprite.PositionY = bounds.MidY - 220;

            layer.AddChild(homeButtonSprite);
            layer.AddChild(replayButtonSprite);
        }

        void HandleTouchesEnd(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (replayButtonSprite.BoundingBoxTransformedToWorld.ContainsPoint(touches[0].Location))
            {
                UnscheduleAll();
                var newScene = new GameScene(GameController.GameView);
                GameController.GoToScene(newScene);
            }
            if (homeButtonSprite.BoundingBoxTransformedToWorld.ContainsPoint(touches[0].Location))
            {
                UnscheduleAll();
                var newScene = new TitleScene(GameController.GameView);
                GameController.GoToScene(newScene);
            }
            if (close.BoundingBoxTransformedToWorld.ContainsPoint(touches[0].Location))
            {
              //  Finish();
            }
        }
    }
}

