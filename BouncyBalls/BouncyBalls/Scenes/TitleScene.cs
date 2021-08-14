using CocosSharp;
using System.Collections.Generic;
using bouncy.balls.keepitup.Entities;

namespace bouncy.balls.keepitup.Scenes
{
    public class TitleScene : CCScene
    {
        CCLayer layer;
        List<BallSprite> visibleBalls;
        CCRect bounds;

        CCSprite level1, level2, level3, level4;


        public TitleScene(CCGameView gameView) : base(gameView)
        {
            layer = new CCLayer();
            this.AddLayer(layer);

            bounds = layer.VisibleBoundsWorldspace;
            GameSettings.IsSound = true;
            CreateBackground();
            AddBalls();

            CreateScene();

            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = HandleTouchesEnd;
            layer.AddEventListener(touchListener);

            Schedule(RunGameLogic);
        }

        private void CreateBackground()
        {
            var background = new CCSprite("titlebg.png");
            background.AnchorPoint = new CCPoint(0, 0);
            background.IsAntialiased = true;
            layer.AddChild(background);

            //help = new CCSprite("help");
            //help.AnchorPoint = new CCPoint(0, 0);
            //help.IsAntialiased = true;
            //help.PositionY = bounds.MaxY - 90;
            //help.PositionX = bounds.MaxX - 90;
           // layer.AddChild(help);
        }

        void AddBalls()
        {
            visibleBalls = new List<BallSprite>();

            for (int i = 1; i <= 2; i++)
            {
                BallSprite ballSprite;
                if (i % 2 == 0)
                    ballSprite = new BallSprite("ball", 1.4f, 360);
                else
                    ballSprite = new BallSprite("ball", 1.6f, -360);

                ballSprite.PositionY = (bounds.Size.Height - ballSprite.ContentSize.Height - 10);
                ballSprite.PositionX = (bounds.Size.Width / 3) + (80 * i); // - (ballSprite.ContentSize.Width / 2);

                layer.AddChild(ballSprite);
                visibleBalls.Add(ballSprite);
            }
        }


        private void CreateScene()
        {
           
            level1 = new CCSprite("button2ball");
            level2 = new CCSprite("button3ball");
            level3 = new CCSprite("button4ball");
            level4 = new CCSprite("button5ball");


            //add the game levels as a child to main Layer
            layer.AddChild(level1);
            layer.AddChild(level2);
            layer.AddChild(level3);
            layer.AddChild(level4);


            // position the  level on the center of the screen
            level1.PositionX = bounds.MidX - 100;
            level1.PositionY = bounds.MidY + 200;

            level2.PositionX = bounds.MidX + 100;
            level2.PositionY = bounds.MidY + 200;

            level3.PositionX = bounds.MidX - 100;
            level3.PositionY = bounds.MidY;

            level4.PositionX = bounds.MidX + 100;
            level4.PositionY = bounds.MidY;
        }

        void RunGameLogic(float frameTimeInSeconds)
        {
            float gravity = 800;
            foreach (var ballSprite in visibleBalls)
            {
                gravity += 50;

                ballSprite.ballYVelocity += frameTimeInSeconds * -gravity;
                ballSprite.PositionX += ballSprite.ballXVelocity * frameTimeInSeconds;
                ballSprite.PositionY += ballSprite.ballYVelocity * frameTimeInSeconds;

                bool isMovingDownward = ballSprite.ballYVelocity < 0;
                if (ballSprite.PositionY < 200 && isMovingDownward)
                {
                    if (GameSettings.IsSound)
                        CCAudioEngine.SharedEngine.PlayEffect("soccer");

                    // invert the velocity:
                    ballSprite.ballYVelocity *= -1;

                    ballSprite.ballYVelocity *= -1;
                    if (ballSprite.ballYVelocity < 1000)
                    {
                        ballSprite.ballYVelocity = 1150f;
                    }

                    // Assign a random to the ball's x velocity:
                    const float minXVelocity = -400;
                    const float maxXVelocity = 400;
                    ballSprite.ballXVelocity = CCRandom.GetRandomFloat(minXVelocity, maxXVelocity);
                }

                //// First get the ball position:   
                float ballRight = ballSprite.BoundingBoxTransformedToParent.MaxX;
                float ballLeft = ballSprite.BoundingBoxTransformedToParent.MinX;

                //// Then get the screen edges
                float screenRight = layer.VisibleBoundsWorldspace.MaxX;
                float screenLeft = layer.VisibleBoundsWorldspace.MinX;

                //// Check if the ball is either too far to the right or left:    
                bool shouldReflectXVelocity =
                    (ballRight > screenRight && ballSprite.ballXVelocity > 0) ||
                    (ballLeft < screenLeft && ballSprite.ballXVelocity < 0);

                if (shouldReflectXVelocity)
                {
                    ballSprite.ballXVelocity *= -1;
                }
            }
        }

        void HandleTouchesEnd(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (level1.BoundingBoxTransformedToWorld.ContainsPoint(touches[0].Location))
                GameSettings.CurrentStage = 2;
            else if (level2.BoundingBoxTransformedToWorld.ContainsPoint(touches[0].Location))
                GameSettings.CurrentStage = 3;
            else if (level3.BoundingBoxTransformedToWorld.ContainsPoint(touches[0].Location))
                GameSettings.CurrentStage = 4;
            else if (level4.BoundingBoxTransformedToWorld.ContainsPoint(touches[0].Location))
                GameSettings.CurrentStage = 5;
            else
                GameSettings.CurrentStage = 0;

            if (GameSettings.CurrentStage != 0)
            {
                UnscheduleAll();
                var newScene = new GameScene(GameController.GameView);
                GameController.GoToScene(newScene);
            }
        }
    }
}
