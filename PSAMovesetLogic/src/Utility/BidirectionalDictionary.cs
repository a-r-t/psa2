﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PSA2MovesetLogic.src.Utility
{
    public class BidirectionalDictionary<TOne, TTwo>
    {
        private Dictionary<TOne, TTwo> forward;
        private Dictionary<TTwo, TOne> backward;

        public BidirectionalDictionary()
        {
            forward = new Dictionary<TOne, TTwo>();
            backward = new Dictionary<TTwo, TOne>();
        }

        public TTwo GetForward(TOne key)
        {
            if (forward.ContainsKey(key))
            {
                return forward[key];
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public TOne GetAllBackward(TTwo key)
        {
            if (backward.ContainsKey(key))
            {
                return backward[key];
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void AddEntry(TOne key, TTwo value)
        {
            if (forward.ContainsKey(key))
            {
                forward[key] = value;
            }
            else
            {
                forward.Add(key, value);
            }

            if (backward.ContainsKey(value))
            {
                backward[value] = key;
            }
            else
            {
                backward.Add(value, key);
            }
        }

        public void AddEntry(TTwo key, TOne value)
        {
            if (backward.ContainsKey(key))
            {
                backward[key] = value;
            }
            else
            {
                backward.Add(key, value);
            }

            if (forward.ContainsKey(value))
            {
                forward[value] = key;
            }
            else
            {
                forward.Add(value, key);
            }
        }

        public void DeleteEntry(TOne key)
        {
            if (forward.ContainsKey(key))
            {
                TTwo value = forward[key];
                forward.Remove(key);
                if (backward.ContainsKey(value))
                {
                    backward.Remove(value);
                }
            }
        }

        public void DeleteEntry(TTwo key)
        {
            if (backward.ContainsKey(key))
            {
                TOne value = backward[key];
                backward.Remove(key);
                if (forward.ContainsKey(value))
                {
                    forward.Remove(value);
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
