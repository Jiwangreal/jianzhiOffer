/*******************************************************************
Copyright(c) 2016, Harry He
All rights reserved.

Distributed under the BSD license.
(See accompanying file LICENSE.txt at
https://github.com/zhedahht/CodingInterviewChinese2/blob/master/LICENSE.txt)
*******************************************************************/

//==================================================================
// 《剑指Offer——名企面试官精讲典型编程题》代码
// 作者：何海涛
//==================================================================

// 面试题2：实现Singleton模式
// 题目：设计一个类，我们只能生成该类的一个实例。

using System;

// 将构造函数设为私有函数以禁止他人创建实例
namespace _02_Singleton
{
    // （1）只是适用于单线程环境
    public sealed class Singleton1
    {
        private Singleton1()
        {
        }

        private static Singleton1 instance = null;
        // 在静态熟悉Instance中，只有在instance为null时才创建一个实例以避免重复创建
        public static Singleton1 Instance
        {
            get
            {
                if (instance == null)
                    instance = new Singleton1();

                return instance;
            }
        }
    }

// （2）虽然多线程环境中工作但是效率不高的方法
    public sealed class Singleton2
    {
        private Singleton2()
        {
        }

        private static readonly object syncObj = new object();

        private static Singleton2 instance = null;
        public static Singleton2 Instance
        {
            get
            {
                // 加一把同步锁
                lock (syncObj)
                {
                    if (instance == null)
                        instance = new Singleton2();
                }

                return instance;
            }
        }
    }

// （3）加锁前后两次判断实例是否已存在
    public sealed class Singleton3
    {
        private Singleton3()
        {
        }

        private static object syncObj = new object();

        private static Singleton3 instance = null;
        public static Singleton3 Instance
        {
            get
            {
                // Singleton3只有当instance=null，即没有创建时，需要加锁操作，当instance已经创建出来之后，则无需加锁
                if (instance == null)
                {
                    lock (syncObj)
                    {
                        if (instance == null)
                            instance = new Singleton3();
                    }
                }

                return instance;
            }
        }
    }

// （4）利用静态构造函数
    public sealed class Singleton4
    {
        private Singleton4()
        {
            Console.WriteLine("An instance of Singleton4 is created.");
        }

        public static void Print()
        {
            Console.WriteLine("Singleton4 Print");
        }

        // 由于C#是在调用静态构造函数时初始化静态变量，.NET运行时能够确保只调用一次静态构造函数，这样能能保证只初始化一次instance
        // 该类型的静态构造函数是在第一次用到Singleton4时就会被创建
        private static Singleton4 instance = new Singleton4();
        public static Singleton4 Instance
        {
            get
            {
                return instance;
            }
        }
    }
// (5)按需创建实例
    public sealed class Singleton5
    {
        Singleton5()
        {
            Console.WriteLine("An instance of Singleton5 is created.");
        }

        public static void Print()
        {
            Console.WriteLine("Singleton5 Print");
        }
        
        // 当通过Singleton5.Instance得到Singleton5实例时,会自动调用Nested的静态构造函数创建实例Instance
        // 如果我们不调用属性Singleton5.Instance,那么就不会触发.NET运行时调用Nested,也就不会创建实例,这样就做到了按需创建
        public static Singleton5 Instance
        {
            get
            {
                return Nested.instance;
            }
        }
        //类型Nested只在属性Singleton5.Instance中被用到,由于其私有属性,他人是无法使用Nested类型
        class Nested
        {
            static Nested()
            {
            }

            internal static readonly Singleton5 instance = new Singleton5();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 也会打印An instance of Singleton4 is created.
            Singleton4.Print();

            // 不会打印An instance of Singleton5 is created.
            Singleton5.Print();
        }
    }
}
