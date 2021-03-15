using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;

    public Text winText;

    public Image imgLives;

    public Sprite[] heart;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioSource musicSource;

    private int scoreValue = 0;

    private int lives = 0;

    private int level = 0;

    private bool playable;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playable = true;
        score.text = scoreValue.ToString();
        winText.text = "";
        lives = 3;
        imgLives.sprite = heart[lives];
        level = 1;
        musicSource.clip = musicClipOne;
        musicSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playable) {
            float hozMovement = Input.GetAxis("Horizontal");
            float vertMovement = Input.GetAxis("Vertical");
            rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
            anim.SetBool("xMove",Mathf.Abs(rd2d.velocity.x)>0);
            anim.SetFloat("ySpeed",rd2d.velocity.y);
            if(rd2d.velocity.x>0) {
                GetComponent<SpriteRenderer>().flipX = false;
            } else if(rd2d.velocity.x<0) {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            anim.SetBool("Grounded",Mathf.Abs(rd2d.velocity.y)<=0);
        }
    }

    void Update()
    {
        if (Input.GetKey("escape")) {
            Application.Quit();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin") {
            scoreValue += 1;
            Destroy(collision.collider.gameObject);
            if(scoreValue >=4) {
                if(level==1) {
                    transform.position = new Vector2(42f, 3f);
                    rd2d.velocity = Vector2.zero;
                    scoreValue = 0;
                    lives = 3;
                    imgLives.sprite = heart[lives];
                    level = 2;
                } else if(level==2) {
                    winText.text = "You win! Game created by Moses";
                    musicSource.clip = musicClipTwo;
                    musicSource.loop = false;
                    musicSource.Play();
                }
            }
            score.text = scoreValue.ToString();
        } else if (collision.collider.tag == "Enemy") {
            lives -= 1;
            imgLives.sprite = heart[lives];
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            if(lives <=0) {
                winText.text = "You lose!";
                playable = false;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && Mathf.Abs(rd2d.velocity.y)<=0) {
            if (Input.GetKey(KeyCode.W) && playable) {
                rd2d.AddForce(new Vector2(0, 7), ForceMode2D.Impulse);
            }
        }
    }
}