using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;         
    public float m_Speed = 6f;            
    public float m_TurnSpeed = 90f;       
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.2f;
	public GameObject grassTrimmings;

	private float grassSpeedModifier = 0.25f;
	private float speedModifier;
	private float defaultSpeedModifier = 1f;

    private string m_MovementAxisName;     
    private string m_TurnAxisName;         
    private Rigidbody m_Rigidbody;         
    private float m_MovementInputValue;    
    private float m_TurnInputValue;        
    private float m_OriginalPitch;         

	private float grassTrimFXTimer = 0f;
	private float grassTrimFXDisableDelay = 0.5f;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
		speedModifier = defaultSpeedModifier;
		grassTrimmings.SetActive (false);
    }


    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }

	// Kinematic means no forces will be applied
    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }
    

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
		m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
		m_TurnInputValue = Input.GetAxis (m_TurnAxisName);

		EngineAudio ();
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
		if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f) 
		{
			if (m_MovementAudio.clip == m_EngineDriving) 
			{
				m_MovementAudio.clip = m_EngineIdling;
				m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
				m_MovementAudio.Play ();
			}
		} 
		else 
		{
			if (m_MovementAudio.clip == m_EngineIdling) 
			{
				m_MovementAudio.clip = m_EngineDriving;
				m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
				m_MovementAudio.Play ();
			}
		}
    }


    private void FixedUpdate()
    {
        // Move and turn the tank.
		Move ();
		Turn ();
		HandleGrassTrimFX ();
    }

    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
		Vector3 movement = transform.forward * m_MovementInputValue * (m_Speed * speedModifier) * Time.deltaTime;

		m_Rigidbody.MovePosition (m_Rigidbody.position + movement);
    }

    private void Turn()
    {
		float turn = m_TurnInputValue * (m_TurnSpeed * speedModifier) * Time.deltaTime;

		Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

		m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
    }

	private void HandleGrassTrimFX ()
	{
		if (grassTrimmings.activeSelf) 
		{
			if (speedModifier == defaultSpeedModifier) 
			{
				if (grassTrimFXTimer >= grassTrimFXDisableDelay) 
				{
					grassTrimmings.SetActive (false);
					ResetFXTimer ();
				}
				else
					grassTrimFXTimer += Time.fixedDeltaTime;
			} 
			else 
			{
				ResetFXTimer ();
			}
		}
	}

	private void ResetFXTimer()
	{
		grassTrimFXTimer = 0f;
	}

	void OnTriggerEnter(Collider other)
	{
		HandleMowSpeed (other.GetComponent<Grass> ());
	}

	void OnTriggerStay(Collider other)
	{
		Grass grass = other.GetComponent<Grass> ();

		grass.Mow ();

		HandleMowSpeed (grass);
	}

	void OnTriggerExit(Collider other)
	{
		speedModifier = defaultSpeedModifier;
	}

	private void HandleMowSpeed(Grass grass)
	{
		if (!grass.Mowed)
			grassTrimmings.SetActive (true);

		if (grass.Mowed)
			speedModifier = defaultSpeedModifier;
		else
			speedModifier = grassSpeedModifier;
	}
}