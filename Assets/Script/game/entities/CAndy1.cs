﻿using UnityEngine;
using System.Collections;

public class CAndy : CAnimatedSprite
{
	private const int WIDTH = 64 * 2;
	private const int HEIGHT = 74 * 2;

	private const int STATE_STAND = 0;
	private const int STATE_WALKING = 1;
	private const int STATE_JUMPING = 2;
	private const int STATE_FALLING = 3;
    private const int STATE_STAND_WALL = 4;

	private CSprite mRect;
	private CSprite mRect2;

	// coordenada y que tenia en el frame anterior. Usada para chequear en la horizontal antes que en la vertical...
	private float mOldY;

	private const int X_OFFSET_BOUNDING_BOX = 8 * 2;
	private const int Y_OFFSET_BOUNDING_BOX = 13 * 2;

	public CAndy()
	{
        //setFrames (Resources.LoadAll<Sprite> ("Sprites/andy"));
        setFrames(Resources.LoadAll<Sprite>("Sprites/youkai"));
        setName ("andy");
		setSortingLayerName ("Player");

		//setScale (2.0f);

		setRegistration (CSprite.REG_TOP_LEFT);

		setWidth (WIDTH);
		setHeight (HEIGHT);

		setState (STATE_STAND);

		setXOffsetBoundingBox (X_OFFSET_BOUNDING_BOX);
		setYOffsetBoundingBox (Y_OFFSET_BOUNDING_BOX);

		mRect = new CSprite ();
		mRect.setImage (Resources.Load<Sprite> ("Sprites/ui/pixel"));
		mRect.setSortingLayerName ("Player");
		mRect.setSortingOrder (20);
		mRect.setAlpha (0.5f);
		mRect.setName ("player_debug_rect");

		mRect2 = new CSprite ();
		mRect2.setImage (Resources.Load<Sprite> ("Sprites/ui/pixel"));
		mRect2.setSortingLayerName ("Player");
		mRect2.setSortingOrder (20);
		mRect2.setColor (Color.red);
		mRect2.setAlpha (0.5f);
		mRect2.setName ("player_debug_rect2");

	}

	private void setOldYPosition()
	{
		mOldY = getY ();
	}

	override public void update()
	{
		// Guardar la posicion anterior del objeto.
		setOldYPosition ();

		base.update ();

		if (getState () == STATE_STAND) 
		{
			// En stand no deberia pasar nunca que quede metido en una pared.
			// Si estamos en una pared, corregirnos. 
			if (isWallLeft (getX (), getY())) 
			{
				// Reposicionar el personaje contra la pared.
				setX (((mLeftX + 1) * CTileMap.TILE_WIDTH) - X_OFFSET_BOUNDING_BOX);
			} 
			if (isWallRight (getX (), getY())) 
			{
				// Reposicionar el personaje contra la pared.
				setX ((((mRightX) * CTileMap.TILE_WIDTH) - getWidth ()) + X_OFFSET_BOUNDING_BOX);
			}


			// Si en el pixel de abajo del jugador no hay piso, caemos.
			if (!isFloor (getX(), getY()+1)) 
			{
				setState (STATE_FALLING);
				return;
			}

			if (CKeyboard.firstPress (CKeyboard.SPACE)) 
			{
				setState (STATE_JUMPING);
				return;
			}

			if (CKeyboard.pressed (CKeyboard.LEFT) && !isWallLeft (getX () - 1, getY ())) 
			{
				setState (STATE_WALKING);
				return;
			}

			if (CKeyboard.pressed (CKeyboard.RIGHT) && !isWallRight (getX () + 1, getY ())) 
			{
				setState (STATE_WALKING);
				return;
			}
		} 
		else if (getState () == STATE_WALKING) 
		{
			if (isWallLeft (getX (), getY())) 
			{
				// Reposicionar el personaje contra la pared.
				setX (((mLeftX + 1) * CTileMap.TILE_WIDTH) - X_OFFSET_BOUNDING_BOX);
			} 
			if (isWallRight (getX (), getY())) 
			{
				// Reposicionar el personaje contra la pared.
				setX ((((mRightX) * CTileMap.TILE_WIDTH) - getWidth ()) + X_OFFSET_BOUNDING_BOX);
			}

			if (CKeyboard.firstPress (CKeyboard.SPACE)) 
			{
				setState (STATE_JUMPING);
				return;
			}

			// Si en el pixel de abajo del jugador no hay piso, caemos.
			if (!isFloor (getX(), getY()+1)) 
			{
				setState (STATE_FALLING);
				return;
			}

			if (!(CKeyboard.pressed (CKeyboard.LEFT) || CKeyboard.pressed (CKeyboard.RIGHT))) 
			{
				setState (STATE_STAND);
				return;
			} 
			else 
			{
				if (CKeyboard.pressed (CKeyboard.LEFT)) 
				{
					// Chequear pared a la izquierda.
					// Si hay pared a la izquierda vamos a stand.
					if (isWallLeft (getX (), getY ())) {
						// Reposicionar el personaje contra la pared.
						//setX((((int) getX ()/CTileMap.TILE_WIDTH)+1)*CTileMap.TILE_WIDTH);
						setX (((mLeftX + 1) * CTileMap.TILE_WIDTH) - X_OFFSET_BOUNDING_BOX);

						// Carlos version.
						//setX (getX()+CTileMap.TILE_WIDTH/(getWidth()-1));

						setState (STATE_STAND);
						return;
					} 
					else 
					{
						// No hay pared, se puede mover.
						setVelX (-400);
						setFlip (true);
					}
				} 
				else 
				{
					// Chequear pared a la derecha.
					// Si hay pared a la derecha vamos a stand.
					if (isWallRight (getX (), getY ())) 
					{
						// Reposicionar el personaje contra la pared.
						setX ((((mRightX) * CTileMap.TILE_WIDTH) - getWidth ()) + X_OFFSET_BOUNDING_BOX);

						setState (STATE_STAND);
						return;
					} 
					else 
					{
						// No hay pared, se puede mover.
						setVelX (400);
						setFlip (false);
					}
				}
			}
		} 
		else if (getState () == STATE_JUMPING) 
		{
			controlMoveHorizontal ();

			if (isFloor(getX(), getY()+1))
			{
				setY (mDownY * CTileMap.TILE_HEIGHT - getHeight());
				setState (STATE_STAND);
				return;
			}

			if (isRoof (getX (), getY () - 1)) 
			{
				setY ((mUpY+1) * CTileMap.TILE_HEIGHT);
				setVelY (0);
				setState (STATE_FALLING);
				return;
			}

            // NUEVO ESTADO

            

            if (isWallLeft(getX(), getY()))
                {
                Debug.Log("PARED IZQ 1");
                // Reposicionar el personaje contra la pared.
                setX(((mLeftX + 1) * CTileMap.TILE_WIDTH) - X_OFFSET_BOUNDING_BOX);
                    setState(STATE_STAND_WALL);
                    return;
                }
            if (isWallRight(getX(), getY()))
            {
                Debug.Log("PARED DER 1");
                // Reposicionar el personaje contra la pared.
                setX((((mRightX) * CTileMap.TILE_WIDTH) - getWidth()) + X_OFFSET_BOUNDING_BOX);
                setState(STATE_STAND_WALL);
                return;
            }
        }
		else if (getState () == STATE_FALLING) 
		{
			controlMoveHorizontal ();

			if (isFloor(getX(), getY()+1))
			{
				setY (mDownY * CTileMap.TILE_HEIGHT - getHeight());
				setState (STATE_STAND);
				return;
			}

            // NUEVO ESTADO



            if (isWallLeft(getX(), getY()))
            {
                Debug.Log("PARED IZQ");
                // Reposicionar el personaje contra la pared.
                setX(((mLeftX + 1) * CTileMap.TILE_WIDTH) - X_OFFSET_BOUNDING_BOX);
                setState(STATE_STAND_WALL);
                return;
            }
            if (isWallRight(getX(), getY()))
            {
                Debug.Log("PARED DER");
                // Reposicionar el personaje contra la pared.
                setX((((mRightX) * CTileMap.TILE_WIDTH) - getWidth()) + X_OFFSET_BOUNDING_BOX);
                setState(STATE_STAND_WALL);
                return;
            }


        }
        else if (getState() == STATE_STAND_WALL)
        {
            if (CKeyboard.firstPress(CKeyboard.SPACE))
            {
                setState(STATE_JUMPING);
                return;
            }

            if (CKeyboard.pressed(CKeyboard.LEFT) && !isWallLeft(getX() - 1, getY()))
            {
                setState(STATE_FALLING);
                return;
            }

            if (CKeyboard.pressed(CKeyboard.RIGHT) && !isWallRight(getX() + 1, getY()))
            {
                setState(STATE_FALLING);
                return;
            }

        }

		// Chequear el paso entre pantallas.
		controlRooms ();
	}

	private void controlRooms()
	{
		CTileMap map = CGame.inst ().getMap ();

		if (getX () + getWidth () / 2 > CTileMap.WORLD_WIDTH) 
		{
			// Se fue por la derecha.
			map.changeRoom(CGameConstants.RIGHT);

			// Aparece por la izquierda.
			setX(-getWidth () / 2);
		} 
		else if (getX () + getWidth () / 2 < 0) 
		{
			// Se fue por la izquierda.
			map.changeRoom(CGameConstants.LEFT);

			setX (CTileMap.WORLD_WIDTH - getWidth () / 2);
		} 
		else if (getY () + getHeight () / 2 > CTileMap.WORLD_HEIGHT) 
		{
			// Se fue por abajo.
			map.changeRoom(CGameConstants.DOWN);

			setY (-getHeight () / 2);
		} 
		else if (getY () + getHeight () / 2 < 0) 
		{
			// Se fue por arriba.
			map.changeRoom(CGameConstants.UP);

			setY(CTileMap.WORLD_HEIGHT - getHeight() / 2);
		}
	}


	// Se llama desde los estados jumping y falling para movernos para los costados.
	private void controlMoveHorizontal()
	{
		// Si estamos en una pared, corregirnos.
		if (isWallLeft (getX (), mOldY)) 
		{
			// Reposicionar el personaje contra la pared.
			setX (((mLeftX + 1) * CTileMap.TILE_WIDTH) - X_OFFSET_BOUNDING_BOX);
		} 
		if (isWallRight (getX (), mOldY)) 
		{
			// Reposicionar el personaje contra la pared.
			setX ((((mRightX) * CTileMap.TILE_WIDTH) - getWidth ()) + X_OFFSET_BOUNDING_BOX);
		} 

		// Chequeamos si podemos movernos.
		if (!(CKeyboard.pressed (CKeyboard.LEFT) || CKeyboard.pressed (CKeyboard.RIGHT))) 
		{
			setVelX (0);
		} 
		else 
		{
			if (CKeyboard.pressed (CKeyboard.LEFT)) 
			{
				// Chequear pared a la izquierda.
				// Si hay pared a la izquierda vamos a stand.
				if (isWallLeft (getX ()-1, mOldY)) 
				{
					// Reposicionar el personaje contra la pared.
					setX (((mLeftX + 1) * CTileMap.TILE_WIDTH) - X_OFFSET_BOUNDING_BOX);
				} 
				else 
				{
					// No hay pared, se puede mover.
					setVelX (-400);
					setFlip (true);
				}
			} 
			else 
			{
				// Chequear pared a la derecha.
				// Si hay pared a la derecha vamos a stand.
				if (isWallRight (getX ()+1, mOldY)) 
				{
					// Reposicionar el personaje contra la pared.
					setX ((((mRightX) * CTileMap.TILE_WIDTH) - getWidth ()) + X_OFFSET_BOUNDING_BOX);
				} 
				else 
				{
					// No hay pared, se puede mover.
					setVelX (400);
					setFlip (false);
				}
			}
		}
	}
		
	override public void render()
	{
		base.render ();

		// MOSTRAR TODA EL AREA DEL DIBUJO.
		mRect.setXY (getX(), getY());
		mRect.setScaleX(WIDTH);
		mRect.setScaleY(HEIGHT);
		mRect.update ();

		mRect.render ();

		// Bounding box.
		mRect2.setXY (getX() + X_OFFSET_BOUNDING_BOX, getY() + Y_OFFSET_BOUNDING_BOX);
		mRect2.setScaleX(WIDTH - (X_OFFSET_BOUNDING_BOX * 2));
		mRect2.setScaleY(HEIGHT - Y_OFFSET_BOUNDING_BOX);
		mRect2.update ();

		mRect2.render ();
	}

	override public void destroy()
	{
		base.destroy ();	
		mRect.destroy ();
		mRect = null;
		mRect2.destroy ();
		mRect2 = null;
	}

	public override void setState (int aState)
	{
		base.setState (aState);

		if (getState () == STATE_STAND) 
		{
			stopMove ();
            //gotoAndStop (1);
            gotoAndStop(1);
        } 
		else if (getState () == STATE_WALKING) 
		{
			//initAnimation (2, 9, 12, true);
            initAnimation(0, 7, 12, true);
        }
		else if (getState () == STATE_JUMPING) 
		{
			//initAnimation (10, 17, 12, false);
            initAnimation(8, 16, 12, false);
            setVelY (CGameConstants.JUMP_SPEED);
			setAccelY (CGameConstants.GRAVITY);
		}
		else if (getState () == STATE_FALLING) 
		{
            //initAnimation (15, 17, 12, false);
            initAnimation(17, 25, 12, false);
            setAccelY (CGameConstants.GRAVITY);
		}
        else if (getState() == STATE_STAND_WALL)
        {
            
            gotoAndStop(26);
        }
        
	}
}
