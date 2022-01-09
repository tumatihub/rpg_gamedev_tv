using UnityEngine;
using System.Collections;

namespace RPG.Core
{
    public interface IPredicateEvaluator
    {
        bool? Evaluate(string predicate, string[] parameters);
    }

}
