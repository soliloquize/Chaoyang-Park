
using UnityEngine;
using System.Collections;

abstract public class GameSingleton<T> : MonoBehaviour where T : GameSingleton<T>
{
	private static T m_instance = null;
	private static readonly object m_locker = new object();

	public static T Singleton
	{
		get
		{
			if (m_instance == null)
			{
				lock (m_locker)
				{
					if (m_instance == null)
					{
						GameObject obj = new GameObject (typeof(T).FullName);
						obj.hideFlags = HideFlags.HideAndDontSave;
						Object.DontDestroyOnLoad(obj);
						m_instance = obj.AddComponent(typeof(T)) as T;
					}
				}
			}

			return m_instance;
		}
	}

	protected void Awake ()
	{
		if (m_instance != this)
		{
#if UNITY_EDITOR
			DestroyImmediate(m_instance);
#else
			Destroy(m_instance);
#endif
			m_instance = null;
		}
		
		m_instance = this as T;
	}

	public virtual void Initialize() { }

	private void OnApplicationQuit()
	{
		if (m_instance != null)
		{
			Destroy(m_instance);
			Destroy(gameObject);
		}

		m_instance = null;
	}
	
}
