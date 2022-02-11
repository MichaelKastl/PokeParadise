using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    /// <summary>
    /// This makes an object move randomly in a set of directions, with some random time delay in between each decision
    /// </summary>
    public class Wanderer : MonoBehaviour
    {
        internal Transform thisTransform;
        internal Animator animator;

        // The movement speed of the object
        public float moveSpeed = 0.4f;

        // A minimum and maximum time delay for taking a decision, choosing a direction to move in
        public Vector2 decisionTime = new Vector2(1, 20);
        internal float decisionTimeCount = 0;

        // The possible directions that the object can move int, right, left, up, down, and zero for staying in place. I added zero twice to give a bigger chance if it happening than other directions
        internal Vector3[] moveDirections = new Vector3[] { Vector3.right, Vector3.left, Vector3.down, Vector3.up, Vector3.zero, Vector3.zero };
        internal int currentMoveDirection;

        // Use this for initialization
        public void Start()
        {
            // Cache the transform for quicker access
            thisTransform = this.transform;

            //Same for the animator
            animator = GetComponent<Animator>();

            // Set a random time delay for taking a decision ( changing direction, or standing in place for a while )
            decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);

            // Choose a movement direction, or stay in place
            ChooseMoveDirection();
        }

        // Update is called once per frame
        public void Update()
        {
            Wander();
            if (decisionTimeCount > 0)
            {
                decisionTimeCount -= Time.deltaTime;
            }
            else
            {
                // Choose a random time delay for taking a decision ( changing direction, or standing in place for a while )
                decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);

                // Choose a movement direction, or stay in place
                ChooseMoveDirection();
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Environment") || collision.gameObject.CompareTag("PlayerPokemon"))
            {
                Wander();
                ChooseMoveDirection(moveDirections[currentMoveDirection]);
                decisionTimeCount = 0;
            }
        }

        private void ChooseMoveDirection(Vector3 currentDir)
        {
            // Choose whether to move sideways or up/down
            currentMoveDirection = Mathf.FloorToInt(Random.Range(0, moveDirections.Length));
            while (moveDirections[currentMoveDirection] == currentDir && moveDirections[currentMoveDirection] != new Vector3(0, 0, 0))
            {
                currentMoveDirection = Mathf.FloorToInt(Random.Range(0, moveDirections.Length));
            }
        }

        private void ChooseMoveDirection()
        {
            // Choose whether to move sideways or up/down
            currentMoveDirection = Mathf.FloorToInt(Random.Range(0, moveDirections.Length));
        }

        private void Wander()
        {
            switch (currentMoveDirection)
            {
                case 0:
                    animator.SetBool("isRight", true);
                    animator.SetBool("isLeft", false);
                    animator.SetBool("isForward", false);
                    animator.SetBool("isBackward", false);
                    animator.speed = 1;
                    break;
                case 1:
                    animator.SetBool("isLeft", true);
                    animator.SetBool("isRight", false);
                    animator.SetBool("isForward", false);
                    animator.SetBool("isBackward", false);
                    animator.speed = 1;
                    break;
                case 2:
                    animator.SetBool("isForward", true);
                    animator.SetBool("isBackward", false);
                    animator.SetBool("isRight", false);
                    animator.SetBool("isLeft", false);
                    animator.speed = 1;
                    break;
                case 3:
                    animator.SetBool("isBackward", true);
                    animator.SetBool("isForward", false);
                    animator.SetBool("isRight", false);
                    animator.SetBool("isLeft", false);
                    animator.speed = 1;
                    break;
                case 4:
                    animator.SetBool("isBackward", false);
                    animator.SetBool("isForward", false);
                    animator.SetBool("isRight", false);
                    animator.SetBool("isLeft", false);
                    animator.speed = 0;
                    break;
                case 5:
                    animator.SetBool("isBackward", false);
                    animator.SetBool("isForward", false);
                    animator.SetBool("isRight", false);
                    animator.SetBool("isLeft", false);
                    animator.speed = 0;
                    break;
                default:
                    break;
            }
            // Move the object in the chosen direction at the set speed
            thisTransform.position += moveSpeed * Time.deltaTime * moveDirections[currentMoveDirection];
        }
    }
}