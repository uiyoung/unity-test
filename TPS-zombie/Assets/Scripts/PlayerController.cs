using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public AudioClip itemPickupClip;
    public int lifeRemains = 3;

    private AudioSource _audioSource;
    private PlayerMovement _playerMovement;
    private PlayerHealth _playerHealth;
    private PlayerShooter _playerShooter;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerHealth = GetComponent<PlayerHealth>();
        _playerShooter = GetComponent<PlayerShooter>();
        _audioSource = GetComponent<AudioSource>();

        _playerHealth.OnDeath += HandleDeath;

        UIManager.Instance.UpdateLifeText(lifeRemains);
        Cursor.visible = false;
    }

    private void HandleDeath()
    {
        _playerMovement.enabled = false;
        _playerShooter.enabled = false;

        if (lifeRemains > 0)
        {
            lifeRemains--;
            UIManager.Instance.UpdateLifeText(lifeRemains);
            Invoke("Respawn", 3f);
        }
        else
            GameManager.Instance.EndGame();

        Cursor.visible = true;
    }

    public void Respawn()
    {
        gameObject.SetActive(false);    // onEnable과 onDisable을 통해 구현한 초기화를 실행시키기위해
        transform.position = Utility.GetRandomPointOnNavMesh(transform.position, 30f, NavMesh.AllAreas);


        gameObject.SetActive(true);
        _playerMovement.enabled = true;
        _playerShooter.enabled = true;

        _playerShooter.gun.ammoRemain = 120;

        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playerHealth.IsDead)
            return;

        IItem item = other.gameObject.GetComponent<IItem>();
        if (item != null)
        {
            item.Use(gameObject);
            _audioSource.PlayOneShot(itemPickupClip);
        }
    }
}