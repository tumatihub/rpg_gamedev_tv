﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using RPG.Core;

namespace RPG.Abilities
{
    public class AbilityData : IAction
    {
        GameObject user;
        Vector3 targetedPoint;
        IEnumerable<GameObject> targets;
        bool cancelled = false;

        public AbilityData(GameObject user)
        {
            this.user = user;
        }

        public IEnumerable<GameObject> GetTargets()
        {
            return targets;
        }

        public void SetTargets(IEnumerable<GameObject> targets)
        {
            this.targets = targets;
        }

        public void SetTargetedPoint(Vector3 targetedPoint)
        {
            this.targetedPoint = targetedPoint;
        }

        public Vector3 GetTargetedPoint()
        {
            return targetedPoint;
        }

        public GameObject GetUser()
        {
            return user;
        }

        public void StartCoroutine(IEnumerator coroutine)
        {
            user.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
        }

        public void Cancel()
        {
            cancelled = true;
        }

        public bool IsCancelled()
        {
            return cancelled;
        }
    }
}
