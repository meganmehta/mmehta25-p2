﻿using Zinnia.Data.Collection.List;
using Zinnia.Data.Type.Transformation.Aggregation;

namespace Test.Zinnia.Data.Type.Transformation.Aggregation
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class FloatAdderTest
    {
        private GameObject containingObject;
        private FloatAdder subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<FloatAdder>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Transform()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            FloatObservableList collection = containingObject.AddComponent<FloatObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(0f);
            subject.Collection.Add(0f);
            subject.Collection.Add(0f);

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            subject.Collection.SetAt(1f, 0);
            subject.Collection.SetAt(2f, 1);
            subject.Collection.CurrentIndex = 2;

            float result = subject.Transform(3f);

            Assert.AreEqual(6f, result);
            Assert.AreEqual(6f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }

        [Test]
        public void TransformEmptyCollection()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            FloatObservableList collection = containingObject.AddComponent<FloatObservableList>();
            subject.Collection = collection;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            float result = subject.Transform();

            Assert.AreEqual(0f, result);
            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsTrue(failedListenerMock.Received);
        }

        [Test]
        public void TransformWithIndex()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            FloatObservableList collection = containingObject.AddComponent<FloatObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(0f);
            subject.Collection.Add(0f);
            subject.Collection.Add(0f);

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            subject.Collection.SetAt(1f, 0);
            subject.Collection.SetAt(2f, 1);
            float result = subject.Transform(3f, 2);

            Assert.AreEqual(6f, result);
            Assert.AreEqual(6f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }

        [Test]
        public void TransformExceedingIndex()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            FloatObservableList collection = containingObject.AddComponent<FloatObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(0f);
            subject.Collection.Add(0f);

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            // adds 1 to index 0 -> adds 2 to index 1 -> attempts to add 3 to index 2 but is out of range so sets it at index 1
            // collection result is [1f, 3f]

            subject.Collection.SetAt(1f, 0);
            subject.Collection.SetAt(2f, 1);
            float result = subject.Transform(3f, 2);

            Assert.AreEqual(4f, result);
            Assert.AreEqual(4f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            FloatObservableList collection = containingObject.AddComponent<FloatObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(0f);
            subject.Collection.Add(0f);

            subject.gameObject.SetActive(false);

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            subject.Collection.SetAt(1f, 0);
            float result = subject.Transform(3f, 1);

            Assert.AreEqual(0f, result);
            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            FloatObservableList collection = containingObject.AddComponent<FloatObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(0f);
            subject.Collection.Add(0f);

            subject.enabled = false;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            subject.Collection.SetAt(1f, 0);
            float result = subject.Transform(3f, 1);

            Assert.AreEqual(0f, result);
            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }
    }
}