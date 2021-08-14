using System;
using CocosSharp;
using bouncy.balls.keepitup.Entities;
using System.Collections.Generic;

namespace bouncy.balls.keepitup.Scenes
{
    public class GameScene : CCScene
    {
        float elapsedTime = 0;
        double time = 0;
        CCLayer layer;
        List<BallSprite> visibleBalls;
        CCLabel scoreText, coinsText, levelText, timeText;
        CCRect bounds;
        CCSprite kidSprite;

        public GameScene(CCGameView gameView) : base(gameView)
        {
            GameSettings.CurrentScore = 0;

            layer = new CCLayer();
            this.AddLayer(layer);
            bounds = layer.VisibleBoundsWorldspace;

            CreateBackground();
            AddToScene();
            AddBalls();
            AddKid();
            AddedSnow();

            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesMoved = HandleTouchesMoved;
            layer.AddEventListener(touchListener);

            Schedule(RunGameLogic);
        }

        private void CreateBackground()
        {
            string bgFileName = string.Format("level{0}bg", GameSettings.CurrentStage.ToString());
            var background = new CCSprite(bgFileName);
            background.AnchorPoint = new CCPoint(0, 0);
            background.IsAntialiased = true;
            layer.AddChild(background);
        }
        private void AddBalls()
        {
            visibleBalls = new List<BallSprite>();

            for (int i = 1; i <= GameSettings.CurrentStage; i++)
            {
                BallSprite ballSprite;
                if (i % 2 == 0)
                    ballSprite = new BallSprite("ball", 1.4f, 360);
                else
                    ballSprite = new BallSprite("ball", 1.6f, -360);

                ballSprite.PositionY = (bounds.Size.Height);//(bounds.Size.Height - ballSprite.ContentSize.Height - 10);
                ballSprite.PositionX = 200; // - (ballSprite.ContentSize.Width / 2);
                ballSprite.Visible = false;
                layer.AddChild(ballSprite);
                visibleBalls.Add(ballSprite);
            }
        }
        private void AddKid()
        {
            string kidFileName = string.Format("kid{0}", GameSettings.CurrentStage.ToString());
            kidSprite = new CCSprite(kidFileName);
            kidSprite.IsAntialiased = true;
            kidSprite.PositionX = 200;
            kidSprite.PositionY = 104;
            layer.AddChild(kidSprite);

            if (GameSettings.CurrentStage == 4)
            {
                var carSprite = new CCSprite("car");
                layer.AddChild(carSprite);
                carSprite.PositionX = 400;
                carSprite.PositionY = 48;
            }
        }

        private void AddedSnow()
        {
            if (GameSettings.CurrentStage == 3 || GameSettings.CurrentStage == 4)
            {
                var snow = new CCParticleSnow(new CCPoint(bounds.Size.Width, 20));
                snow.Position = new CCPoint(bounds.Size.Width / 2, bounds.Size.Height + 1);
                snow.StartColor = new CCColor4F(CCColor4B.White);
                snow.EndColor = new CCColor4F(CCColor4B.LightGray);

                if (GameSettings.CurrentStage == 3)
                {
                    snow.Texture = new CCTexture2D("dot");
                    snow.ContentSize = new CCSize(2, 2);
                    snow.StartSize = 5f;
                    snow.StartSizeVar = 1f;
                    snow.EndSize = 5f;
                    snow.EndSizeVar = 1f;
                    snow.Speed = 10f;
                }
                else if (GameSettings.CurrentStage == 4)
                {

                    snow.Texture = new CCTexture2D("dot2");
                    snow.ContentSize = new CCSize(10, 10);
                    snow.StartSize = 10f;
                    snow.StartSizeVar = 2f;
                    snow.EndSize = 8f;
                    snow.EndSizeVar = 1f;
                }
                // snow.Speed = 2f;
                snow.Gravity = new CCPoint(0.5f, -2);
                snow.EmissionRate = 2.5f;
                snow.Life = 50f;
                layer.AddChild(snow);
            }
        }


        private void AddToScene()
        {
            scoreText = new CCLabel("0", "MarkerFelt-22", 22, CCLabelFormat.SpriteFont);
            coinsText = new CCLabel("0", "MarkerFelt-22", 22, CCLabelFormat.SpriteFont);
            levelText = new CCLabel((GameSettings.CurrentStage-1).ToString(), "MarkerFelt-32", 22);
            timeText = new CCLabel("00:00", "MarkerFelt-22", 22);

            var ballScoreBGSprite = new CCSprite("ballscorebg");
            var coinScoreBGSprite = new CCSprite("coinscorebg");

            scoreText.Color = CCColor3B.Black;
            scoreText.PositionY = bounds.Center.Y - 50;
            scoreText.PositionX = bounds.Center.X + 270;
            scoreText.HorizontalAlignment = CCTextAlignment.Center;

            coinsText.Color = CCColor3B.Black;
            coinsText.PositionY = bounds.Center.Y - 150;
            coinsText.PositionX = bounds.Center.X + 270;
            coinsText.HorizontalAlignment = CCTextAlignment.Center;

            levelText.Color = CCColor3B.White;
            levelText.PositionX = (bounds.MaxX - 20);
            levelText.PositionY = bounds.MaxY - 40;

            timeText.Color = CCColor3B.White;
            timeText.PositionX = (50);
            timeText.PositionY = bounds.MaxY - 40;

            ballScoreBGSprite.PositionX = scoreText.PositionX - 15;
            ballScoreBGSprite.PositionY = scoreText.PositionY;

            coinScoreBGSprite.PositionX = coinsText.PositionX - 15;
            coinScoreBGSprite.PositionY = coinsText.PositionY;

            layer.AddChild(levelText);
            layer.AddChild(timeText);
            layer.AddChild(coinScoreBGSprite);
            layer.AddChild(ballScoreBGSprite);
            layer.AddChild(scoreText);
            layer.AddChild(coinsText);
        }

        void HandleTouchesMoved(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
        {
            var locationOnScreen = touches[0].Location;
            kidSprite.PositionX = locationOnScreen.X;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        void EndGame()
        {
            UnscheduleAll();
            var gameOverScene = new GameOverScene(GameController.GameView);
            GameController.GoToScene(gameOverScene);
        }

        void ShowTime()
        {
            if (elapsedTime == 60)
            {
                time++;
                elapsedTime = 0;
            }
            if (time <= visibleBalls.Count)
            {
                for (int i = 0; i < visibleBalls.Count; i++)
                {
                    if (time == (i + 1))
                        visibleBalls[i].Visible = true;
                }
            }
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            string str = timeSpan.ToString(@"mm\:ss");
            timeText.Text = str;
        }

        void UpdateScore()
        {
            GameSettings.CurrentScore++;
            scoreText.Text = GameSettings.CurrentScore.ToString();
            coinsText.Text = (Convert.ToInt32(GameSettings.CurrentScore / GameSettings.CoinValue)).ToString();
        }

        void RunGameLogic(float frameTimeInSeconds)
        {
            elapsedTime++;
            ShowTime();
            float gravity = 900;
            foreach (var ballSprite in visibleBalls)
            {
                if (ballSprite.Visible)
                {
                    gravity += 50;

                    ballSprite.ballYVelocity += frameTimeInSeconds * -gravity;
                    ballSprite.PositionX += ballSprite.ballXVelocity * frameTimeInSeconds;
                    ballSprite.PositionY += ballSprite.ballYVelocity * frameTimeInSeconds;

                    float fudgeFactor = 0.9f;

                    var rect = kidSprite.BoundingBoxTransformedToParent;
                    var smRect = new CCRect(rect.LowerLeft.X, rect.LowerLeft.Y, rect.Size.Width * fudgeFactor, rect.Size.Height * fudgeFactor);

                    bool doesBallOverlapPaddle = ballSprite.BoundingBoxTransformedToParent.IntersectsRect(smRect);

                    bool isMovingDownward = ballSprite.ballYVelocity < 0;
                    if (doesBallOverlapPaddle && isMovingDownward)
                    {
                        if (GameSettings.IsSound)
                            CCAudioEngine.SharedEngine.PlayEffect("soccer");

                        ballSprite.ballYVelocity *= -1;
                        if (ballSprite.ballYVelocity < 1000)
                        {
                            ballSprite.ballYVelocity = 1150f;
                        }

                        //  ballSprite.ballYVelocity = viewSize.Height;
                        // Then let's assign a random to the ball's x velocity:
                        const float minXVelocity = -400;
                        const float maxXVelocity = 400;
                        ballSprite.ballXVelocity = CCRandom.GetRandomFloat(minXVelocity, maxXVelocity);

                        UpdateScore();
                    }
                    else if (ballSprite.PositionY < 30)
                    {
                        if (GameSettings.IsSound)
                            CCAudioEngine.SharedEngine.PlayEffect("tap");

                        EndGame();
                    }
                    // First let’s get the ball position:   
                    float ballRight = ballSprite.BoundingBoxTransformedToParent.MaxX;
                    float ballLeft = ballSprite.BoundingBoxTransformedToParent.MinX;
                    // Then let’s get the screen edges
                    float screenRight = layer.VisibleBoundsWorldspace.MaxX;
                    float screenLeft = layer.VisibleBoundsWorldspace.MinX;

                    // Check if the ball is either too far to the right or left:    
                    bool shouldReflectXVelocity =
                        (ballRight > screenRight && ballSprite.ballXVelocity > 0) ||
                        (ballLeft < screenLeft && ballSprite.ballXVelocity < 0);

                    if (shouldReflectXVelocity)
                    {
                        ballSprite.ballXVelocity *= -1;
                    }
                }
            }
        }

    }
}

