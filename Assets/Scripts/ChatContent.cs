using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatContent : MonoBehaviour
{
    Player Player
    {
        get
        {
            return FindObjectOfType<Player>();
        }
    }

    const int countMax = 8;
    // Update is called once per frame
    void Update()
    {
        int childCount = transform.childCount;
        if (childCount > countMax)
        {
            for(int i = 0; i < childCount - countMax; ++i)
            {
                var comment = transform.GetChild(i).GetComponent<CommentUI>();
                if (!comment.Nice && !comment.Muted)
                {
                    Player.Quality -= 125;
                }
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
