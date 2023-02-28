using UnityEngine;

public class PoolObject : MonoBehaviour
{
    private enum Pools {ChickenPool = 0, DuckPool = 1 }
    [SerializeField] private Pools pools;
    [SerializeField] private AnimalManager animalManager;

    //[SerializeField] private float returnPoolTime;
    //private void OnEnable()
    //{
    //    Invoke(nameof(ReturnHome), returnPoolTime);
    //}

    private void ReturnHome()
    {
        switch ((int)pools)
        {
            case 0:
                GameManager.Instance.ReturnChickenPool.Invoke(animalManager);
                break;
            case 1:
                GameManager.Instance.ReturnDuckPool.Invoke(animalManager);
                break;
        }

    }
}
