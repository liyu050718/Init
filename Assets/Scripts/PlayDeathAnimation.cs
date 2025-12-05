using UnityEngine;

public class PlayDeathAnimation : MonoSingleton<PlayDeathAnimation>
{
    [Header("死亡动画设置")]
    public GameObject deathAnimationPrefab;  // 引用您的动画预制体
    public Transform deathSpawnPoint;         // 动画出现的位置（可选）

    // 玩家生命值或其他死亡条件
    private bool isAlive = true;

    void Update()
    {
        // 示例：按D键模拟死亡
        if (Input.GetKeyDown(KeyCode.D) && isAlive)
        {
            Die();
        }
    }

    // 死亡方法
    public void Die()
    {
        if (!isAlive) return;

        isAlive = false;

        // 1. 首先隐藏玩家（可选）
        GetComponent<SpriteRenderer>().enabled = false;
        // 或者直接设置玩家不可用
        // GetComponent<Collider2D>().enabled = false;
        // GetComponent<PlayerMovement>().enabled = false;

        // 2. 在玩家位置创建死亡动画
        Vector3 spawnPosition = deathSpawnPoint != null ?
            deathSpawnPoint.position : transform.position;

        GameObject deathAnim = Instantiate(deathAnimationPrefab, spawnPosition, Quaternion.identity);

        // 3. 播放动画
        Animation anim = deathAnim.GetComponent<Animation>();
        if (anim != null)
        {
            anim.Play();

            // 可选：动画播放完毕后自动销毁
            StartCoroutine(DestroyAfterAnimation(deathAnim, anim.clip.length));
        }

        // 4. 可以在这里添加游戏结束逻辑
        Debug.Log("玩家死亡，播放死亡动画");
    }

    // 协程：动画播放完毕后销毁物体
    System.Collections.IEnumerator DestroyAfterAnimation(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }
}