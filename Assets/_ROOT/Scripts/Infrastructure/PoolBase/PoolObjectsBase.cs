namespace Scripts.Infrastructure.PoolBase
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class PoolObjectsBase<T> where T : Component
    {
        protected T prefab;
        protected Transform container;

        protected List<T> pool;

        public PoolObjectsBase(T prefab, Transform container, int count)
        {
            this.prefab = prefab;
            this.container = container;

            CreatePool(count);
        }

        public bool HasFreeElements(out T element)
        {
            foreach (var poolObject in pool)
            {
                if (ObjectIsFree(poolObject))
                {
                    element = poolObject;
                    return true;
                }
            }

            element = null;
            return false;
        }

        public virtual bool ObjectIsFree(T element) => element.gameObject.activeInHierarchy == false;

        public T GetFreeElement()
        {
            if (HasFreeElements(out var element))
                return element;
            else
                return CreateObject();
        }

        protected void CreatePool(int count)
        {
            pool = new List<T>(count);
            for (int i = 0; i < count; i++)
                CreateObject();
        }

        protected virtual T CreateObject()
        {
            var createdObject = Object.Instantiate(prefab, container);
            pool.Add(createdObject);
            createdObject.gameObject.SetActive(false);
            return createdObject;
        }
    }
}