using UnityEngine;

namespace Completed
{
    public class EnemyAnimations : ICharacterAttackAnimations
    {
        private Animator _animator;

        public EnemyAnimations(Animator animator)
        {
            _animator = animator;
        }

        public void SetAttack()
        {
            _animator.SetTrigger("enemyAttack");
        }
    }
}