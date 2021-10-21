using UnityEngine;

public class CharactersAnimation : MonoBehaviour
{
    private Animator characterAnim;

    // Start is called before the first frame update
    void Start()
    {
        characterAnim = GetComponent<Animator>();

        // Starts animation
        StartAnimation();
    }

    // Animations
    private void StartAnimation()
    {
        int animationInt = 0;
        int headMoveInt = 0;

        switch (gameObject.tag)
        {
            case "SmokerCharacter":
                animationInt = 5;
                characterAnim.SetFloat("Character_Animation_Speed", 0.5f);
                break;

            case "SitCharacter":
                animationInt = 9;
                break;

            case "Walker":
                float walkerSpeed = Random.Range(0.26f, 0.5f);
                if (walkerSpeed > 0.4f)
                {
                    headMoveInt = 3;
                }
                characterAnim.SetFloat("Speed_f", walkerSpeed);
                characterAnim.SetBool("Static_b", false);
                characterAnim.SetFloat("Character_Animation_Speed", walkerSpeed * 2.5f);
                break;
            case "SitDog":
                characterAnim.SetFloat("Character_Animation_Speed", 1f);
                characterAnim.SetBool("Sit_b", true);
                break;
            default:
                break;
        }

        characterAnim.SetInteger("Animation_int", animationInt);
        characterAnim.SetInteger("Head_Move_int", headMoveInt);
    }
}
