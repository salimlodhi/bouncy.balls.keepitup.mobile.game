using CocosSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace bouncy.balls.keepitup.Entities
{
    public class BallSprite : CCSprite
    {
        public float ballXVelocity;

        public float ballYVelocity;

        public BallSprite(string fileName, float rotateDuration, float rotateDelta, CCRect? texRectInPixels = default(CCRect?)) : base(fileName, texRectInPixels)
        {
            CCRotateBy rotateBall = new CCRotateBy(rotateDuration, rotateDelta);
           this.RepeatForever(rotateBall);
        }
    }
}
