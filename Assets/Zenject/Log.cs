using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ModestTree.Zenject
{
    // Simple wrapper around unity's logging system
    public static class Log
    {
        public static void Debug(string message, params object[] args)
        {
            //UnityEngine.Debug.Log(string.Format(message, args));
        }

        /////////////

        public static void Info(string message, params object[] args)
        {
            UnityEngine.Debug.Log(string.Format(message, args));
        }

        /////////////

        public static void Warn(string message, params object[] args)
        {
            UnityEngine.Debug.LogWarning(string.Format(message, args));
        }

        /////////////

        public static void ErrorException(Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }

        public static void ErrorException(string message, Exception e)
        {
            UnityEngine.Debug.LogError(message);
            UnityEngine.Debug.LogException(e);
        }

        public static void Error(string message, params object[] args)
        {
            UnityEngine.Debug.LogError(string.Format(message, args));
        }
    }
}

