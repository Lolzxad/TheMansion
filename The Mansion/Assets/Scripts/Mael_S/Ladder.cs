using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private enum LadderPart { complete, bottom, top};
    [SerializeField] LadderPart part = LadderPart.complete;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Test_PlayerMovement>())
        {
            Debug.Log("Collision avec joueur");

            Test_PlayerMovement player = collision.GetComponent<Test_PlayerMovement>();

            switch (part)
            {
                case LadderPart.complete:
                    Debug.Log("ladderpart is complete");
                    player.canClimb = true;
                    player.ladder = this;
                    break;

                case LadderPart.bottom:
                    Debug.Log("ladderpart is bottom");
                    player.canClimb = false;
                    player.botLadder = true;
                    break;

                case LadderPart.top:
                    Debug.Log("ladderpart is top");
                    player.canClimb = false;
                    //Physics2D.IgnoreLayerCollision(9, 10, false);
                    player.topLadder = true;
                    break;

                default:
                    break;
            }
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Test_PlayerMovement>())
        {
            Test_PlayerMovement player = collision.GetComponent<Test_PlayerMovement>();

            switch (part)
            {
                case LadderPart.complete:
                    player.canClimb = false;
                    break;

                case LadderPart.bottom:
                    player.botLadder = false;
                    break;

                case LadderPart.top:
                    player.topLadder = false;
                    break;

                default:
                    break;
            }
        }
    }
}
