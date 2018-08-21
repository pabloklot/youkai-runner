using UnityEngine;
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
    private const int STATE_POSITIONING = 5;
    private const int STATE_ATTACKING = 6;
    private const int STATE_SECOND_JUMP = 7;
    private const int STATE_AIR_ATTACK = 8;
    

    private CSprite mRect;
	private CSprite mRect2;

	// coordenada y que tenia en el frame anterior. Usada para chequear en la horizontal antes que en la vertical...
	private float mOldY;

	private const int X_OFFSET_BOUNDING_BOX = 8 * 2;
	private const int Y_OFFSET_BOUNDING_BOX = 13 * 2;

	public CAndy()
	{
		setFrames (Resources.LoadAll<Sprite> ("Sprites/zorra"));
		setName ("zorra");
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
            if (CKeyboard.firstPress (CKeyboard.LEFT_CTRL))
            {
                setState(STATE_ATTACKING);
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
            if (CKeyboard.firstPress(CKeyboard.LEFT_CTRL))
            {
                setState(STATE_ATTACKING);
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
				setState (STATE_POSITIONING);
				return;
			}

			if (isRoof (getX (), getY () - 1)) 
			{
				setY ((mUpY+1) * CTileMap.TILE_HEIGHT);
				setVelY (0);
				setState (STATE_FALLING);
				return;
			}
            if (CKeyboard.firstPress(CKeyboard.SPACE))
            {
                setState(STATE_SECOND_JUMP);
                return;
            }
            if (CKeyboard.firstPress(CKeyboard.LEFT_CTRL))
            {
                setState(STATE_AIR_ATTACK);
            }
            // NUEVO ESTADO



            /*if (isWallLeft(getX(), getY()))
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
            }*/
        }
		else if (getState () == STATE_FALLING) 
		{
			controlMoveHorizontal ();

			if (isFloor(getX(), getY()+1))
			{
				setY (mDownY * CTileMap.TILE_HEIGHT - getHeight());
				setState (STATE_POSITIONING);
				return;
			}
            // NUEVO ESTADO


            /*
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
            }*/


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
        else if (getState() == STATE_POSITIONING)
        {
            controlMoveHorizontal();
            if (this.isEnded())
            {
                setState(STATE_STAND);
            }
            Debug.Log("state positioning");
        }
        else if (getState() == STATE_ATTACKING)
        {
            controlMoveHorizontal();
            if (CKeyboard.pressed(CKeyboard.LEFT))
            {
                // Chequear pared a la izquierda.
                // Si hay pared a la izquierda vamos a stand.
                if (isWallLeft(getX(), getY()))
                {
                    // Reposicionar el personaje contra la pared.
                    //setX((((int) getX ()/CTileMap.TILE_WIDTH)+1)*CTileMap.TILE_WIDTH);
                    setX(((mLeftX + 1) * CTileMap.TILE_WIDTH) - X_OFFSET_BOUNDING_BOX);

                    // Carlos version.
                    //setX (getX()+CTileMap.TILE_WIDTH/(getWidth()-1));

                    setState(STATE_STAND);
                    return;
                }
                else
                {
                    // No hay pared, se puede mover.
                    setVelX(-800);
                    setFlip(true);
                }
            }
            else
            {
                // Chequear pared a la derecha.
                // Si hay pared a la derecha vamos a stand.
                if (isWallRight(getX(), getY()))
                {
                    // Reposicionar el personaje contra la pared.
                    setX((((mRightX) * CTileMap.TILE_WIDTH) - getWidth()) + X_OFFSET_BOUNDING_BOX);

                    setState(STATE_STAND);
                    return;
                }
                else
                {
                    // No hay pared, se puede mover.
                    setVelX(800);
                    setFlip(false);
                }
            }
            if (this.isEnded())
            {
                setState(STATE_STAND);
            }

        }
        else if (getState() == STATE_SECOND_JUMP)
        {
            controlMoveHorizontal();
            if (this.isEnded())
            {
                setState(STATE_FALLING);
            }
            /*if (CKeyboard.firstPress(CKeyboard.LEFT_CTRL))
            {
                setState(STATE_AIR_ATTACK);
            }*/

        }
        else if (getState() == STATE_AIR_ATTACK)
        {
            controlMoveHorizontal();
            if (isFloor(getX(), getY() + 1))
            {
                this.stopMove();
                setState(STATE_WALKING);
            }
            if (this.isEnded())
            {
                setState(STATE_FALLING);
            }
            else
            {
                if (CKeyboard.pressed(CKeyboard.LEFT))
                {
                    setVelX(-800);
                    setFlip(true);
                }
                else if (CKeyboard.pressed(CKeyboard.RIGHT))
                {
                    setVelX(800);
                    setFlip(false);

                }
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
			//setVelX (0);
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
		/*mRect.setXY (getX(), getY());
		mRect.setScaleX(WIDTH);
		mRect.setScaleY(HEIGHT);
		mRect.update ();

		mRect.render ();

		// Bounding box.
		mRect2.setXY (getX() + X_OFFSET_BOUNDING_BOX, getY() + Y_OFFSET_BOUNDING_BOX);
		mRect2.setScaleX(WIDTH - (X_OFFSET_BOUNDING_BOX * 2));
		mRect2.setScaleY(HEIGHT - Y_OFFSET_BOUNDING_BOX);
		mRect2.update ();

		mRect2.render ();*/
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
            initAnimation(1, 8, 12, true);
		} 
		else if (getState () == STATE_WALKING) 
		{
            initAnimation(1, 8, 12, true);
        }
		else if (getState () == STATE_JUMPING) 
		{
            initAnimation(9, 15, 12, false);
            setVelY (CGameConstants.JUMP_SPEED);
			setAccelY (CGameConstants.GRAVITY);
		}
		else if (getState () == STATE_FALLING) 
		{
			initAnimation (16, 22, 12, false);
			setAccelY (CGameConstants.GRAVITY);
		}
        else if (getState() == STATE_STAND_WALL)
        {

            gotoAndStop(26);
        }
        else if (getState() == STATE_POSITIONING)
        {
            stopMove();
            
            initAnimation(23, 25, 8, false);
        }
        else if (getState() == STATE_ATTACKING)
        {
<<<<<<< HEAD
            setVelX(2000);
=======
            //setVelX(800);
            initAnimation(26, 31, 12, false);
        }
        else if (getState() == STATE_SECOND_JUMP)
        {
            setVelY(CGameConstants.JUMP_SPEED);
            setAccelY(CGameConstants.GRAVITY);
            //initAnimation(26, 31, 12, false);
        }
        else if (getState() == STATE_AIR_ATTACK)
        {
            //setVelY(CGameConstants.JUMP_SPEED);
            //setAccelY(CGameConstants.GRAVITY);
            setVelX(800);
>>>>>>> 3bcacea26a419efe47040cadded0356d2ddf746e
            initAnimation(26, 31, 12, false);
        }
    }
}
