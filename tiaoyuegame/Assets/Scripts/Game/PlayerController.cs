using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Transform rayDown, rayLeft, rayRight;
    public LayerMask platformLayer, obstacleLayer;


    private bool isMoveLeft = false;


    private bool isJumping = false;
    private Vector3 nextPlatformLeft, nextPlatformRight;
    private ManagerVars vars;
    private Rigidbody2D my_Body;
    private SpriteRenderer spriteRenderer;
    private bool isMove = false;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        spriteRenderer = GetComponent<SpriteRenderer>();
        my_Body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Debug.DrawRay(rayDown.position, Vector2.down * 1,Color.red);
        //Debug.DrawRay(rayLeft.position, Vector2.left * 0.15f, Color.red);
        //Debug.DrawRay(rayRight.position, Vector2.right * 0.15f, Color.red);

        if (EventSystem.current.IsPointerOverGameObject())  
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && isJumping == false)
        {

            if (isMove==false)
            {
                EventCenter.Broadcast(EventDefine.PlayerMove);
            }
            if (GameManager.Instance.IsGameStarted == false || GameManager.Instance.IsGameOver
                ||GameManager.Instance.IsPause)
            {
                return;
            }
            EventCenter.Broadcast(EventDefine.DecidePath);
            isJumping = true;

            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x <= Screen.width / 2)
            {
                isMoveLeft = true;
            }
            else if (mousePos.x > Screen.width / 2)
            {
                isMoveLeft = false;

            }
            Jump();
        }
        //isGameover
        if (my_Body.velocity.y < 0 && IsRayPlatform() == false && GameManager.Instance.IsGameOver == false)
        {
           
            spriteRenderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.IsGameOver = true;
            StartCoroutine(DealyShowGameOverPanel());
        }
        if (isJumping&&IsRayObstacle()&&GameManager.Instance.IsGameOver==false)
        {
            GameObject go = ObjectPool.Instance.GetDeathEffect();
            go.SetActive(true);
            go.transform.position = transform.position;
            GameManager.Instance.IsGameOver = true;
            spriteRenderer.enabled = false;

            StartCoroutine(DealyShowGameOverPanel());
        }
        if (transform.position.y-Camera.main.transform.position.y<-6&&GameManager.Instance.IsGameOver==false)
        {
            GameManager.Instance.IsGameOver = true;

            StartCoroutine(DealyShowGameOverPanel());
        }
    }
    IEnumerator DealyShowGameOverPanel()
    {
        yield return new WaitForSeconds(1f);

        //show Gameover
        EventCenter.Broadcast(EventDefine.ShowGameOverPanel);

    }



    private GameObject lastHitGo = null;


    private bool IsRayPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 10f, platformLayer);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Platform")
            {   
                if (lastHitGo!=hit.collider.gameObject)
                {
                    if (lastHitGo==null)
                    {
                        lastHitGo = hit.collider.gameObject;
                        return true;
                    }
                      EventCenter.Broadcast(EventDefine.AddScore);
                    lastHitGo = hit.collider.gameObject;
                }
                return true;

            }


        } else if (hit.collider == null)
        {
            Debug.Log("hit.collider null");
            Debug.DrawRay(transform.position, Vector2.down, Color.red);
        }
        return false;
    }

    private bool IsRayObstacle()
    {
        RaycastHit2D leftHit= Physics2D.Raycast(rayLeft.position, Vector2.left, 0.15f, obstacleLayer);
        RaycastHit2D rightHit= Physics2D.Raycast(rayLeft.position, Vector2.right, 0.15f, obstacleLayer);

        if (leftHit.collider !=null)
        {
     
            if (leftHit.collider.tag== "Obstacle")
            {
                return true;
            }
              
        }

        if (rightHit.collider != null)
        {
            if (rightHit.collider.tag == "Obstacle")
            {
                return true;
            }
        }
            return false;
    }

    private void Jump()
    {
        if (isJumping)
        {
            if (isMoveLeft)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.DOMoveX(nextPlatformLeft.x, 0.2f);
                transform.DOMoveY(nextPlatformLeft.y + 0.8f, 0.15f);
            }
            else
            {
                transform.DOMoveX(nextPlatformRight.x, 0.2f);
                transform.DOMoveY(nextPlatformRight.y + 0.8f, 0.15f);
                transform.localScale = Vector3.one;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag =="Platform")
        {

            isJumping=false;
            Vector3 currentPlatformPos = collision.gameObject.transform.position;

            nextPlatformLeft = new Vector3(currentPlatformPos.x -
                vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);

            nextPlatformRight = new Vector3(currentPlatformPos.x +
                vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Pickup")
        {

            EventCenter.Broadcast(EventDefine.AddDiamond);
            collision.gameObject.SetActive(false);

        }
    }
}
