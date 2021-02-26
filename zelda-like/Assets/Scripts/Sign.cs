using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    public string dialogueText;
    public GameObject dialogueBox;
    private bool _isPlayerInRange;

    void Update()
    {
        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            //��ȭ ���ڰ� �̹� �����ִ� ��� �ݱ�
            if (dialogueBox.activeSelf)
                dialogueBox.SetActive(false);
            else
            {
                dialogueBox.transform.GetChild(0).GetComponent<Text>().text = dialogueText;
                dialogueBox.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _isPlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerInRange = false;
            dialogueBox.SetActive(false);
        }
    }
}
