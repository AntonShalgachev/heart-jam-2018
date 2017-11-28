using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Targets : PropertyChanger
    {
        List<GameEvents> targets = new List<GameEvents>();

        public bool TryAddTarget(GameEvents target)
        {
            if (!target)
                return false;

            if (!findInTargets(target))
            {
                targets.Add(target);
                Debug.LogFormat("Target '{0}' added to targets", target.name);
            }
            OnPropertyChanged("targets");
            return true;
        }

        public List<GameEvents> GetTargets()
        {
            return new List<GameEvents>(targets);
        }

        public void RemoveTarget(GameEvents target)
        {
            targets.Remove(target);
            OnPropertyChanged("targets");
        }

        bool findInTargets(GameEvents target)
        {
            bool _res = false;
            GameEvents remove_it = target;

            if (targets.Count == 0) return false;

            foreach (GameEvents _target in targets)
            {
                if (_target.id == target.id)
                {
                    if (_target.index < target.index)
                    {
                        targets.Add(target);
                        remove_it = _target;
                    }
                    _res = true;
                    break;
                }
            }
            if(remove_it != target) targets.Remove(remove_it);
            return _res;
        }
    }
}
