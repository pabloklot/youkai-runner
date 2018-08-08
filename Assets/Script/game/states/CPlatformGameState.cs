using UnityEngine;
using System.Collections;

public class CPlatformGameState : CGameState
{
	private CTileMap mMap;

	private CEnemyManager mEnemyManager;
	private CBulletManager mBulletManager;
	private CParticleManager mParticleManger;

	private CAndy mAndy;




	public CPlatformGameState()
	{
	}

	override public void init()
	{
		base.init ();

		//CGame.inst ().setPlayer (mPlayer);

		mMap = new CTileMap ();
		CGame.inst ().setMap (mMap);

		mEnemyManager = new CEnemyManager ();
		mBulletManager = new CBulletManager ();
		mParticleManger = new CParticleManager ();

		mAndy = new CAndy ();
		mAndy.setXY (400, 400);

        CGame.inst().setAndy(mAndy);

		//createAsteroids ();

		/*CEnemyShip e = new CEnemyShip ();
		e.setXY (200, 200);
		CEnemyManager.inst ().add (e);*/


		CScorpion s = new CScorpion (CScorpion.TYPE_DONT_FALL);
		s.setXY (550, 300);
		CEnemyManager.inst ().add (s);

		s = new CScorpion (CScorpion.TYPE_FALL);
		s.setXY (700, 300);
		CEnemyManager.inst ().add (s);
	}

	override public void update()
	{
		base.update (); 

		mMap.update ();

		mAndy.update ();

		mEnemyManager.update ();
		mBulletManager.update ();
		mParticleManger.update ();

		if (CEnemyManager.inst ().getLength () == 0) 
		{
			//Debug.Log ("WIN");
		}
	}

	override public void render()
	{
		base.render ();

		mMap.render ();

		mAndy.render ();

		mEnemyManager.render ();
		mBulletManager.render ();
		mParticleManger.render ();
	}

	override public void destroy()
	{
		base.destroy ();

		mMap.destroy ();
		mMap = null;

		mAndy.destroy ();
		mAndy = null;

		mEnemyManager.destroy ();
		mEnemyManager = null;

		mBulletManager.destroy ();
		mBulletManager = null;

		mParticleManger.destroy ();
		mParticleManger = null;
	}

	/*private void createAsteroids()
	{
		CAsteroid asteroid;

		for (int i = 0; i < 10; i++) 
		{
			asteroid = new CAsteroid (CAsteroid.TYPE_BIG, CMath.randomIntBetween(1, 3));
			asteroid.setXY (CMath.randomIntBetween(0, CGameConstants.SCREEN_WIDTH), CMath.randomIntBetween(0, CGameConstants.SCREEN_HEIGHT));
			asteroid.setVelX (CMath.randomFloatBetween(-500, 500));
			asteroid.setVelY (CMath.randomFloatBetween(-500, 500));
			CEnemyManager.inst ().add (asteroid);
		}
	}*/
}
