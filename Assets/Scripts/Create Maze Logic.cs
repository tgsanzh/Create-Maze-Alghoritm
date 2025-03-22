using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMazeLogic : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform currentPosition;
    private List<Vector2> was_list = new List<Vector2>();
    private List<Vector2> path = new List<Vector2>();

    private Vector2 up_left = new Vector2(-2.25f, 2.5f);
    private Vector2 down_right = new Vector2(2.75f, -2.5f);
    void Start()
    {
        was_list.Add(currentPosition.position);
        path.Add(currentPosition.position);
        StartCoroutine(dfs());
    }

    void Update()
    {

    }

    private IEnumerator dfs()
    {
        List<Vector2> poses = new List<Vector2>();

        if (isValid(new Vector2(currentPosition.position.x + 0.5f, currentPosition.position.y)))
            poses.Add(new Vector2(currentPosition.position.x + 0.5f, currentPosition.position.y));

        if (isValid(new Vector2(currentPosition.position.x - 0.5f, currentPosition.position.y)))
            poses.Add(new Vector2(currentPosition.position.x - 0.5f, currentPosition.position.y));

        if (isValid(new Vector2(currentPosition.position.x, currentPosition.position.y + 0.5f)))
            poses.Add(new Vector2(currentPosition.position.x, currentPosition.position.y + 0.5f));

        if (isValid(new Vector2(currentPosition.position.x, currentPosition.position.y - 0.5f)))
            poses.Add(new Vector2(currentPosition.position.x, currentPosition.position.y - 0.5f));
        if (poses.Count == 0)
        {
            if (path.Count == 0) 
            {
                yield break;
            }

            path.RemoveAt(path.Count - 1);

            if (path.Count > 0)
            {
                currentPosition.position = path[path.Count - 1];
            }
            else
            {
                yield break; 
            }
        }
        else
        {
            int range = Random.Range(0, poses.Count);
            RaycastHit2D hit = Physics2D.Raycast(
                    new Vector2((currentPosition.position.x + poses[range].x) / 2,
                        (currentPosition.position.y + poses[range].y) / 2)
                    , Vector2.zero
                );
            if (hit)
            {
                hit.collider.gameObject.SetActive(false);
            }
            currentPosition.position = poses[range];
            path.Add(currentPosition.position);
            was_list.Add(currentPosition.position);
            
        }


        yield return new WaitForSeconds(0.1f); 

        StartCoroutine(dfs());
    }

    bool isValid(Vector2 pos)
    {
        return pos.x > up_left.x
            && pos.y < up_left.y
            && pos.x < down_right.x
            && pos.y > down_right.y
            && !was_list.Contains(pos);
    }

}
