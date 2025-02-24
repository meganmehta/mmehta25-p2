﻿using Zinnia.Data.Collection.List;
using Zinnia.Extension;
using Zinnia.Rule;
using BaseRule = Zinnia.Rule.Rule;

namespace Test.Zinnia.Rule
{
    using NUnit.Framework;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.TestTools;
    using Assert = UnityEngine.Assertions.Assert;

    public class ListContainsRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private ListContainsRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            container = new RuleContainer();
            subject = containingObject.AddComponent<ListContainsRule>();
            container.Interface = subject;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [UnityTest]
        public IEnumerator AcceptsMatch()
        {
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            yield return null;

            subject.Objects = objects;
            objects.Add(containingObject);

            Assert.IsTrue(container.Accepts(containingObject));
        }

        [UnityTest]
        public IEnumerator RefusesEmpty()
        {
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            yield return null;

            subject.Objects = objects;

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesNullObjects()
        {
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [UnityTest]
        public IEnumerator RefusesDifferent()
        {
            GameObject wrongGameObject = new GameObject();
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            yield return null;

            subject.Objects = objects;
            objects.Add(wrongGameObject);

            Assert.IsFalse(container.Accepts(containingObject));

            Object.DestroyImmediate(wrongGameObject);
        }

        [UnityTest]
        public IEnumerator RefusesInactiveGameObject()
        {
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            yield return null;

            subject.Objects = objects;
            objects.Add(containingObject);

            subject.gameObject.SetActive(false);
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [UnityTest]
        public IEnumerator RefusesInactiveComponent()
        {
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            yield return null;

            subject.Objects = objects;
            objects.Add(containingObject);

            subject.enabled = false;
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [UnityTest]
        public IEnumerator AcceptsInactiveGameObject()
        {
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            yield return null;

            subject.Objects = objects;
            objects.Add(containingObject);
            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleComponentIsDisabled;

            subject.gameObject.SetActive(false);
            Assert.IsTrue(container.Accepts(containingObject));

            subject.enabled = false;
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [UnityTest]
        public IEnumerator AcceptsInactiveComponent()
        {
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            yield return null;

            subject.Objects = objects;
            objects.Add(containingObject);

            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleGameObjectIsNotActiveInHierarchy;

            subject.enabled = false;
            Assert.IsTrue(container.Accepts(containingObject));

            subject.gameObject.SetActive(false);
            Assert.IsFalse(container.Accepts(containingObject));
        }
    }
}