using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField] Disjunction[] and;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (Disjunction disjunction in and)
            {
                if (!disjunction.Check(evaluators)) return false;
            }
            return true;
        }

        [System.Serializable]
        class Disjunction
        {
            [SerializeField] Predicate[] or;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (Predicate predicate in or)
                {
                    if (predicate.Check(evaluators)) return true;
                }
                return false;
            }
        }

        [System.Serializable]
        class Predicate
        {
            [SerializeField] string predicate;
            [SerializeField] string[] parameters;
            [SerializeField] bool not;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (var evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate(predicate, parameters);
                    if (result == null) continue;

                    if (result == false) return not;
                }
                return !not;
            }
        }
    }
}
