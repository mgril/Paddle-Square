using UnityEngine;

public class Game : MonoBehaviour 
{
    [SerializeField]
    Ball ball;

    [SerializeField]
    Paddle bottomPaddle, topPaddle;

    [SerializeField, Min(0f)]
	Vector2 arenaExtents = new Vector2(10f, 10f);

    void Awake () => ball.StartNewGame();
	
	void Update ()
	{
        bottomPaddle.Move(ball.Position.x, arenaExtents.x);
		topPaddle.Move(ball.Position.x, arenaExtents.x);
		ball.Move();
        BounceYIfNeeded();
        BounceXIfNeeded(ball.Position.x);
		ball.UpdateVisualization();
	}

    	void BounceYIfNeeded ()
	{
		float yExtents = arenaExtents.y - ball.Extents;
		if (ball.Position.y < -yExtents)
		{
			BounceY(-yExtents, bottomPaddle);
		}
		else if (ball.Position.y > yExtents)
		{
			BounceY(yExtents, topPaddle);
		}
	}
    	void BounceXIfNeeded (float x)
	{
		float xExtents = arenaExtents.x - ball.Extents;
		if (x < -xExtents)
		{
			ball.BounceX(-xExtents);
		}
		else if (x > xExtents)
		{
			ball.BounceX(xExtents);
		}
        void BounceY (float boundary, Paddle defender)
        {
            // go back in time to calc the bounce 
            float durationAfterBounce = (ball.Position.y - boundary) / ball.Velocity.y;
            float bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;

            BounceXIfNeeded(bounceX); //we take care of an X bounce only if it happened before the Y bounce
		    bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;
            // original bounce 
            ball.BounceY(boundary)
            //check whether the defending paddle hit the ball
            if (defender.HitBall(bounceX, ball.Extents, out float hitFactor))
            {
                ball.SetXPositionAndSpeed(bounceX, hitFactor, durationAfterBounce);
            }
        }
	}
    
}
