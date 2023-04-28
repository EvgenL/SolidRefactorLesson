using UnityEngine;

namespace Completed
{
    public class PlayerAnimations : ICharacterAnimations
    {
        private Animator _animator;

        public PlayerAnimations(Animator animator)
        {
            _animator = animator;
        }
        
        public void SetPlayerHit()
        {
            _animator.SetTrigger("playerHit");
        }

        public void SetAttack()
        {            
            _animator.SetTrigger("playerChop");
        }
    }
}