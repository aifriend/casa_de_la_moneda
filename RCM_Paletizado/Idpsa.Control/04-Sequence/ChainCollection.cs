using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Engine;

namespace Idpsa.Control.Sequence
{
    [Serializable]
    public class ChainCollection : IEnumerable<KeyValuePair<string, Chain>>
    {
        private readonly Dictionary<string, Chain> _chains;
        

        public ChainCollection()
        {
            _chains = new Dictionary<string, Chain>();
        }        

        public ChainCollection(IEnumerable<Chain> chains) : this()
        {
            AddRange(chains);
        }       

        public Chain this[int index]
        {
            get
            {
                try
                {
                    return _chains.ElementAt(index).Value;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public Chain this[string key]
        {
            get
            {
                try
                {
                    return _chains[key];
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public Dictionary<string, Chain>.KeyCollection AllKeys
        {
            get { return _chains.Keys; }
        }

        public Dictionary<string, Chain>.ValueCollection AllValues
        {
            get { return _chains.Values; }
        }

        public int Count
        {
            get { return _chains.Count; }
        }

        public void QuitError()
        {
            foreach (Chain cad in _chains.Values)
            {
                cad.QuitError = true;
            }
        }

        public void PutError()
        {
            foreach (Chain cad in _chains.Values)
            {
                cad.QuitError = false;
            }
        }


        public void Add(Chain value)
        {          
            _chains.Add(value.Name, value);
        }

        public void AddRange(IEnumerable<Chain> chains)
        {
            foreach (Chain chain in chains)
                Add(chain);
        }

        public void Remove(string key)
        {
            _chains.Remove(key);
        }

        public void Clear()
        {
            _chains.Clear();
        }

        public bool StopManager(bool activeMode, ControlStopRequest deactiveModeRequested, ControlModeStatus controlMode)
        {
            bool activated = true;
            if (activeMode)
            {
                if (deactiveModeRequested != ControlStopRequest.None)
                {
                    if (controlMode == ControlModeStatus.Activated)
                    {
                        foreach (Chain cad in _chains.Values)
                        {
                            if (cad.CurrentStep.StopChain)
                            {
                                cad.Stoped = true;
                                cad.CurrentStep.Activated = false;
                            }
                            else
                            {
                                activated = false;
                            }
                        }
                    }
                }
                else
                {
                    QuitStop();
                    activated = false;
                }
            }

            return activated;
        }

        private void QuitStop()
        {
            foreach (Chain cad in _chains.Values)
            {
                cad.Stoped = false;
            }
        }

        public void ForEach(Action<Chain> action)
        {
            foreach (var v in this)
                action(v.Value);
        }

        #region Miembros de IEnumerable<KeyValuePair<string,Chain>>

        public IEnumerator<KeyValuePair<string, Chain>> GetEnumerator()
        {
            return _chains.GetEnumerator();
        }

        #endregion

        #region Miembros de IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _chains.GetEnumerator();
        }

        #endregion
    }
}