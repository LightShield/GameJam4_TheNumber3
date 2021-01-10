using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManagerScript : Singleton<EventManagerScript>
{
    protected EventManagerScript()
    {
        Init();
    } // guarantee this will be always a singleton only - can't use the constructor!

    public class FloatEvent : UnityEvent<object> {} //empty class; just needs to exist

    public const string EVENT__BULLET_INACTIVE = "event_bulletInactive";
	public const string EVENT__ENEMY_DEATH = "event_enemyInactive";
	public const string EVENT__REG_BULLET_INACTIVE = "event_regBulletInactive";
	public const string EVENT_PLAYER_HIT_BY_BULLET = "event_playerHitByBullet";
	public const string EVENT_ENEMY_HIT_BY_BULLET = "event_enemyHitByBullet";
	public const string EVENT_PLAYER_CRASH_ENEMY = "event_playerCrashEnemy";

	private Dictionary <string, FloatEvent> eventDictionary;
	
	private void Init ()
	{
		if (eventDictionary == null)
		{
			eventDictionary = new Dictionary<string, FloatEvent>();
		}
	}
	
	public void StartListening (string eventName, UnityAction<object> listener)
	{
		FloatEvent thisEvent = null;
		if (eventDictionary.TryGetValue (eventName, out thisEvent))
		{
			thisEvent.AddListener (listener);
		} 
		else
		{
			thisEvent = new FloatEvent ();
			thisEvent.AddListener (listener);
			eventDictionary.Add (eventName, thisEvent);
		}
	}
	
	public void StopListening (string eventName, UnityAction<object> listener)
	{
		FloatEvent thisEvent = null;
		if (eventDictionary.TryGetValue (eventName, out thisEvent))
		{
			thisEvent.RemoveListener (listener);
		}
	}
	
	public void TriggerEvent (string eventName, object obj)
	{
		FloatEvent thisEvent = null;
		if (eventDictionary.TryGetValue (eventName, out thisEvent))
		{
			thisEvent.Invoke(obj);
		}
	}
}
