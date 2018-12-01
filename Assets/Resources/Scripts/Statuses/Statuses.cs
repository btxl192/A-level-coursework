using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Statuses : MonoBehaviour {

    private GameObject StatusesCanvas;
	private GameObject target;
	public enum statuses {Poison, Slow, Rage, GetHyped};
    public static Dictionary<statuses, string> StatusString = new Dictionary<statuses, string>()
    {
        {statuses.Poison, "Poison" },
        {statuses.Slow, "Slow" },
        {statuses.Rage, "Rage" },
        {statuses.GetHyped, "Hyped Up" }
    };
    public static Dictionary<System.Type, statuses> TypeToStatus = new Dictionary<System.Type, statuses>()
    {
        {typeof(Poison), statuses.Poison },
        {typeof(Slow), statuses.Slow },
        {typeof(RageStatus), statuses.Rage },
        {typeof(GetHypedStatus), statuses.GetHyped }
    };
    [System.Serializable]
    public struct StatusType
    {
        public statuses Type;
        public int Level;
    }

    [SerializeField]
	private List<StatusBase> CurrentStatuses = new List<StatusBase> ();

    public List<StatusBase> CurrentStatusEffects
    {
        get
        {
            return CurrentStatuses;
        }
    }

    void Awake()
    {
        StatusBase.StatusApplied += AddStatus;
        StatusBase.EndStatus += RemoveStatus;
        target = this.gameObject;
        StatusesCanvas = GameObject.FindGameObjectWithTag("MainUI").transform.Find("Statuses").gameObject;
    }

    void AddStatus(StatusBase s, GameObject g)
    {
        if (gameObject == g)
        {
            CurrentStatuses.Add(s);
        }       
    }

    void RemoveStatus(StatusBase s, GameObject g)
    {
        if (gameObject == g)
        {
            CurrentStatuses.Remove(s);
        }
    }

    //Enables the statuses UI
    private void Update()
    {
        if (gameObject.GetComponent<PlayerBehaviour>() != null)
        {
            if (CurrentStatuses.Count == 0 || GenericMenu2.OpenMenu != null)
            {
                StatusesCanvas.SetActive(false);
            }
            else
            {

                StatusesCanvas.SetActive(true);
            }
        }
    }

    //Applies the status specified. Extra parameters may be required e.g. Poison requires a damage value
    public void ApplyStatus(StatusType StatusType, params object[] miscparam)
    {
        if (StatusType.Type == statuses.Poison)
        {
            if (target.GetComponent<Poison>() == null)
            {
                target.AddComponent<Poison>().SetLevel(StatusType.Level);
                try
                {
                    target.GetComponent<Poison>().damage = (int)miscparam[0];
                }
                catch
                {
                    if (((GameObject)miscparam[0]).GetComponent<GenericWeapon>() != null)
                    {
                        target.GetComponent<Poison>().damage = ((GameObject)miscparam[0]).GetComponent<GenericWeapon>().damage;
                    }
                    if (((GameObject)miscparam[0]).GetComponent<GenericEnemy>() != null)
                    {
                        target.GetComponent<Poison>().damage = ((GameObject)miscparam[0]).GetComponent<GenericEnemy>().damage;
                    }
                }
            }
            else if (target.GetComponent<Poison>().Level <= StatusType.Level)
            {
                target.GetComponent<Poison>().SetLevel(StatusType.Level);
                target.GetComponent<Poison>().SetTimeLived(0);
            }
        }
        if (StatusType.Type == statuses.Slow)
        {
            if (target.GetComponent<Slow>() == null)
            {
                target.AddComponent<Slow>().SetLevel(StatusType.Level);
            }
            else if (target.GetComponent<Slow>().Level <= StatusType.Level)
            {
                target.GetComponent<Slow>().SetLevel(StatusType.Level);
                target.GetComponent<Slow>().SetTimeLived(0);
            }
        }
        if (StatusType.Type == statuses.Rage)
        {
            if (target.GetComponent<RageStatus>() == null)
            {
                target.AddComponent<RageStatus>().SetLevel(StatusType.Level);
            }
            else if (target.GetComponent<RageStatus>().Level <= StatusType.Level)
            {
                target.GetComponent<RageStatus>().SetLevel(StatusType.Level);
                target.GetComponent<RageStatus>().SetTimeLived(0);
            }
        }
        if (StatusType.Type == statuses.GetHyped)
        {
            if (target.GetComponent<GetHypedStatus>() == null)
            {
                target.AddComponent<GetHypedStatus>().SetLevel(StatusType.Level);
            }
            else
            {
                target.GetComponent<GetHypedStatus>().ResetTimer();
                target.GetComponent<GetHypedStatus>().SetLevel(StatusType.Level);
            }
        }
    }

    //Creates a new StatusType
    public static StatusType MakeStatus(statuses Type, int Level)
    {
        StatusType temp;
        temp.Type = Type;
        temp.Level = Level;
        return temp;
    }

    private void OnDestroy()
    {
        StatusBase.StatusApplied -= AddStatus;
        StatusBase.EndStatus -= RemoveStatus;
    }
}
