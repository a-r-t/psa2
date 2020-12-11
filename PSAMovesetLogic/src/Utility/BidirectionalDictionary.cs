using System;
using System.Collections.Generic;
using System.Text;

namespace PSA2MovesetLogic.src.Utility
{
    public class BidirectionalDictionary<TOne, TTwo>
    {
        private Dictionary<TOne, List<TTwo>> forward;
        private Dictionary<TTwo, List<TOne>> backward;

        public BidirectionalDictionary()
        {
            forward = new Dictionary<TOne, List<TTwo>>();
            backward = new Dictionary<TTwo, List<TOne>>();
        }

        /*
        public TTwo GetFirstForward(TOne key)
        {
            if (forward.ContainsKey(key))
            {
                List<TTwo> value = forward[key];
                return value[0];
            }
            else
            {
                return default;
            }
        }
        */

        public List<TTwo> GetAllForward(TOne key)
        {
            if (forward.ContainsKey(key))
            {
                List<TTwo> value = forward[key];
                return value;
            }
            else
            {
                return default;
            }
        }

        /*
        public TOne GetFirstBackward(TTwo key)
        {
            if (backward.ContainsKey(key))
            {
                List<TOne> value = backward[key];
                return value[0];
            }
            else
            {
                return default;
            }
        }
        */

        public List<TOne> GetAllBackward(TTwo key)
        {
            if (backward.ContainsKey(key))
            {
                List<TOne> value = backward[key];
                return value;
            }
            else
            {
                return default;
            }
        }

        public void AddEntry(TOne key, TTwo value)
        {
            if (forward.ContainsKey(key))
            {
                forward[key].Add(value);
            }
            else
            {
                List<TTwo> values = new List<TTwo>();
                values.Add(value);
                forward.Add(key, values);
            }

            if (backward.ContainsKey(value))
            {
                backward[value].Add(key);
            }
            else
            {
                List<TOne> keys = new List<TOne>();
                keys.Add(key);
                backward.Add(value, keys);
            }
        }

        public void AddEntry(TTwo key, TOne value)
        {
            if (backward.ContainsKey(key))
            {
                backward[key].Add(value);
            }
            else
            {
                List<TOne> values = new List<TOne>();
                values.Add(value);
                backward.Add(key, values);
            }

            if (forward.ContainsKey(value))
            {
                forward[value].Add(key);
            }
            else
            {
                List<TTwo> keys = new List<TTwo>();
                keys.Add(key);
                forward.Add(value, keys);
            }
        }

        public void DeleteEntry(TOne key)
        {
            if (forward.ContainsKey(key))
            {
                List<TTwo> tTwoValues = forward[key];
                forward.Remove(key);
                foreach (TTwo tTwoValue in tTwoValues) 
                {
                    if (backward.ContainsKey(tTwoValue))
                    {
                        List<TOne> tOneValues = backward[tTwoValue];
                        tOneValues.Remove(key);
                        if (tOneValues.Count == 0)
                        {
                            backward.Remove(tTwoValue);
                        }
                    }
                }
            }
        }

        public void DeleteEntry(TTwo key)
        {
            if (backward.ContainsKey(key))
            {
                List<TOne> tOneValues = backward[key];
                backward.Remove(key);
                foreach (TOne tOneValue in tOneValues)
                {
                    if (forward.ContainsKey(tOneValue))
                    {
                        List<TTwo> tTwoValues = forward[tOneValue];
                        tTwoValues.Remove(key);
                        if (tTwoValues.Count == 0)
                        {
                            forward.Remove(tOneValue);
                        }
                    }
                }
            }
        }

        public bool ContainsKey(TOne key)
        {
            return forward.ContainsKey(key);
        }

        public bool ContainsKey(TTwo key)
        {
            return backward.ContainsKey(key);
        }
    }
}
