using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using helpers;

namespace nn
{
    public partial class ANN
    {        
        internal bool SetVar(string key, object value)
        {
            if (!vars.ContainsKey(key))
            {
                vars.Add(key,value);   
                return true;
            }
            else
            {
                vars[key] = value;
                return false;             
            }
        }
        internal T GetVarAs<T>(string key)
        {
            if (!vars.ContainsKey(key))
            {
                throw new KeyNotFoundException("Variable \"" + key + "\" not found");
            }
            else
            {
                return (T)vars[key];
            }
        }

        internal object GetVar(string key)
        {
            string var = (key.Contains('$'))? key.Replace("$","") : key;
            
            if (!vars.ContainsKey(var))
            {
                throw new KeyNotFoundException("Variable \"" + var + "\" not found");
            }
            else
            {
                return vars[var];
            }
        }
    }
}