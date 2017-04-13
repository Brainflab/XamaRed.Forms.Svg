using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XamaRed.Forms.Svg.Tests
{
    public static class TestHelpers
    {
        public static T CallPrivateMethod<T>(this object obj, string methodName, params object[] args)
        {
            PrivateObject privateObject = new PrivateObject(obj);
            return (T) privateObject.Invoke(methodName, BindingFlags.NonPublic | BindingFlags.Instance, args);
        }

        public static void CallPrivateMethod(this object obj, string methodName, params object[] args)
        {
            PrivateObject privateObject = new PrivateObject(obj);
            privateObject.Invoke(methodName, BindingFlags.NonPublic | BindingFlags.Instance, args);
        }

        public static void SetMemberValue(this object obj, string memberName, object value)
        {
            PrivateObject privateObject = new PrivateObject(obj);
            privateObject.SetField(memberName, BindingFlags.NonPublic | BindingFlags.Instance, value);
        }

        public static void SetStaticMemberValue(this Type type, string memberName, object value)
        {
            PrivateType privateType = new PrivateType(type);
            privateType.SetStaticField(memberName, BindingFlags.NonPublic | BindingFlags.Static, value);
        }

        public static T GetMemberValue<T>(this object obj, string memberName)
        {
            PrivateObject privateObject = new PrivateObject(obj);
            return (T) privateObject.GetField(memberName, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static T GetStaticMemberValue<T>(this Type type, string memberName)
        {
            PrivateType privateType = new PrivateType(type);
            return (T) privateType.GetStaticField(memberName, BindingFlags.NonPublic | BindingFlags.Static);
        }

        public static void HandleInvocationException(Action test, Type expectedInnerExceptionType)
        {
            try
            {
                test();
                Assert.Fail("An exception should have occured");
            }
            catch (Exception e)
            {
                Assert.IsNotNull(e.InnerException);
                Assert.IsInstanceOfType(e.InnerException, expectedInnerExceptionType);
            }
        }
    }
}
