using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;
    public Text countText;
    public Text winText;
    public Text deathText;

    private Rigidbody rb;

    private int count;
    private int freezeTime = 5;

    private bool debugMode = true;

    private static int amountDeaths = 0;

    void Start()
    {
        deathText.text = "Deaths: " + amountDeaths.ToString();
        count = 0;
        rb = GetComponent<Rigidbody>();
        setCountText();
        winText.text = "";
    }

	void FixedUpdate () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);

        if(debugMode)
            if(Input.GetKeyDown(KeyCode.R))
            {
                reset();
            }
	}

    void OnCollisionEnter(Collision collision)
    {
        rb.AddForce(collision.contacts[0].normal , ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            setCountText();
        }

        else if(other.gameObject.CompareTag("Hole"))
          {
            death();
          }
        
        else if (other.gameObject.CompareTag("Rebound"))
        {
            rb.AddForce(rb.velocity.normalized * -15 , ForceMode.Impulse);
        }
    }

    void setCountText()
    {
        countText.text = "Score = " + count.ToString();
        if(count >= 12){
            freezeGame(freezeTime);
        }
    }
    IEnumerator ResetAfterSeconds(int seconds)
    {
        float pauseEndTime = Time.realtimeSinceStartup + seconds;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return null;  // Attend un frame      
        }

        reset();
    }

    void reset()
    {
        Application.LoadLevel(0);
        Time.timeScale = 1f;
    }

    void freezeGame(int amount)
    {
        StartCoroutine(ResetAfterSeconds(amount));
        Time.timeScale = 0f;
    }

    void death()
    {
        amountDeaths++;
        winText.text = "YOU LOSE";
        freezeGame(freezeTime);    
    }
}
