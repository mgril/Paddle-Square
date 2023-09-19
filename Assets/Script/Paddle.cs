using UnityEngine;
using TMPro;


public class Paddle : MonoBehaviour 
{
	[SerializeField, Min(0f)]
	float
		extents = 4f,
		speed = 10f;
        maxTargetingBias = 0.75f;

    [SerializeField]
	bool isAI;

    [SerializeField]
	TextMeshPro scoreText;

    int score;
    float targetingBias;

    public void StartNewGame ()
	{
		SetScore(0);
        ChangeTargetingBias();
	}

    void SetScore (int newScore)
	{
		score = newScore;
		scoreText.SetText("{0}", newScore);
	}

	public bool ScorePoint (int pointsToWin)
	{
		SetScore(score + 1);
		return score >= pointsToWin;
	}

    public void Move (float target, float arenaExtents)
	{
		Vector3 p = transform.localPosition;
        p.x = isAI ? AdjustByAI(p.x, target) : AdjustByPlayer(p.x);
		float limit = arenaExtents - extents;
		p.x = Mathf.Clamp(p.x, -limit, limit);
		transform.localPosition = p;
	}
    float AdjustByAI (float x, float target)    //stupid AI with no prediction , difficulty only depends on its speed
	{
        target += targetingBias * extents;
		if (x < target)
		{
			return Mathf.Min(x + speed * Time.deltaTime, target);
		}
		return Mathf.Max(x - speed * Time.deltaTime, target);
	}
    float AdjustByPlayer (float x)
	{
		bool goRight = Input.GetKey(KeyCode.RightArrow);
		bool goLeft = Input.GetKey(KeyCode.LeftArrow);
		if (goRight && !goLeft)
		{
			return x + speed * Time.deltaTime;
		}
		else if (goLeft && !goRight)
		{
			return x - speed * Time.deltaTime;
		}
		return x;
	}

    public bool HitBall (float ballX, float ballExtents, out float hitFactor) // between [-1; 1]  if paddle hit the ball 
	{
        ChangeTargetingBias();
		hitFactor =
			(ballX - transform.localPosition.x) /
			(extents + ballExtents);
		return -1f <= hitFactor && hitFactor <= 1f;
	}

    void ChangeTargetingBias () =>
		targetingBias = Random.Range(-maxTargetingBias, maxTargetingBias);

}