using UnityEngine;
using System.Collections.Generic;

public abstract class GOAPAction : MonoBehaviour
{

	private HashSet<KeyValuePair<string, object>> preconditions;
	private HashSet<KeyValuePair<string, object>> effects;

	private bool inRange = false;

	public float cost = 1f;

	public GameObject target;

	public GOAPAction()
	{
		preconditions = new HashSet<KeyValuePair<string, object>>();
		effects = new HashSet<KeyValuePair<string, object>>();
	}

	public void ResetAction()
	{
		inRange = false;
		target = null;
		ResetGA();
	}

	public abstract void ResetGA();

	public abstract bool IsActionFinished();

	public abstract bool CheckPrecondition(GameObject agent);

	public abstract bool PerformAction(GameObject agent);

	public abstract bool NeedsToBeInRange();

	public bool IsAgentInRange()
	{
		return inRange;
	}

	public void SetInRange(bool val)
	{
		inRange = val;
	}

	public void AddPrecondition(string key, object value)
	{
		preconditions.Add(new KeyValuePair<string, object>(key, value));
	}

	public void RemovePrecondition(string key)
	{
		KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
		foreach (KeyValuePair<string, object> kvp in preconditions)
		{
			if (kvp.Key.Equals(key))
			{
				remove = kvp;
			}
			if (!default(KeyValuePair<string, object>).Equals(remove))
			{
				preconditions.Remove(remove);
			}
		}
	}

	public void AddEffect(string key, object value)
	{
		effects.Add(new KeyValuePair<string, object>(key, value));
	}

	public void RemoveEffect(string key)
	{
		KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
		foreach (KeyValuePair<string, object> kvp in effects)
		{
			if (kvp.Key.Equals(key))
			{
				remove = kvp;
			}
			if (!default(KeyValuePair<string, object>).Equals(remove))
			{
				effects.Remove(remove);
			}
		}
	}

	public HashSet<KeyValuePair<string, object>> Preconditions
	{
		get
		{
			return preconditions;
		}
	}

	public HashSet<KeyValuePair<string, object>> Effects
	{
		get
		{
			return effects;
		}
	}
}
