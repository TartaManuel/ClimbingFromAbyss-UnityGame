using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:Component
{
    //o instanta deja existenta, ca sa se poate crea un nou obiect trebuie sa fie null
    //e statica pentru ca e aceeasi pentru toate instantele
    private static T existingInstance;
    public static T Instance
    {
        get
        {
            if (existingInstance == null)
            {
                //in cazul in care existingInstance este inca null, verific daca in scena actuala exista deja un obiect de tip T
                existingInstance = FindObjectOfType<T>();
                if (existingInstance == null)
                {
                    //daca nu exista un obiect de acest tip, conceptul de singleton este indeplinit, deci creez eu un nou obiect, si il salvez
                    //asa urmatoarele dati cand se va dori crearea unei instante, existingInstance nu va mai fi null
                    GameObject newInstance = new GameObject();
                    newInstance.name = typeof(T).Name;
                    existingInstance = newInstance.AddComponent<T>();
                }
            }
            return existingInstance;
        }
    }

    public virtual void Awake()
    {
        if (existingInstance == null)
        {
            //daca nu exista o instanta inainte, o stochez si nu o distrug
            existingInstance = this as T;
        }
        else
        {
            //daca exista deja o distrug
            Debug.Log(gameObject.name);
            Destroy(gameObject);
        }
    }
}
