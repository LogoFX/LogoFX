using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LogoFX.Core.Tests
{
    [TestFixture]    
    public class ConcurrentObservableCollectionTests
    {
        [Test]
        public void CopyTo_CopyCollectionWhileRemoving()
        {
            var array = new int[100];
            for (int i = 0; i < 100; i++)
            {
                array[i] = i;
            }
            var col = new ConcurrentObservableCollection<int>(array);

            var task1 = new Task(() =>
            {
                for (int i = 100; i >= 0; i--)
                {
                    col.Remove(i);
                    Thread.Sleep(1);
                }
            });
            var copy = new int[100];

            var task2 = new Task(() => col.CopyTo(copy, 0));
            task1.Start();
            Thread.Sleep(10);
            task2.Start();
            Task.WaitAll(new[] { task1, task2 });

            Assert.That(copy[2], Is.EqualTo(2));
        }

        [Test]
        public void Ctor_DoesntThrow()
        {
            Assert.DoesNotThrow(() => new ConcurrentObservableCollection<int>());
        }       

        [Test]
        public void Performance_MonitorNumberOfAllocatedThreads()
        {
            int maxNumberOfThreads = 0;

            int currentNumberOfThreads = Process.GetCurrentProcess().Threads.Count;

            Console.WriteLine("Number of threads before run {0}", currentNumberOfThreads);
            for (int j = 0; j < 100; j++)
            {
                var threadSafe = new ConcurrentObservableCollection<int>();
                for (int i = 0; i < 100; i++)
                {
                    threadSafe.Add(i);

                    if (i % 10 == 0)
                    {
                        int tmp = Process.GetCurrentProcess().Threads.Count;
                        if (tmp > maxNumberOfThreads)
                        {
                            maxNumberOfThreads = tmp;
                        }
                    }
                }
            }
            Console.WriteLine("Max number of threads  {0}", maxNumberOfThreads);
            Assert.That(maxNumberOfThreads - currentNumberOfThreads, Is.LessThan(10), "too many threads created");
        }

        [Test]
        public void ReadWrite_EnumeratingThreadTriesToWriteToCollection_NoDeadlock()
        {
            var array = new int[100];
            for (int i = 0; i < 100; i++)
            {
                array[i] = i;
            }
            var col = new ConcurrentObservableCollection<int>(array);

            var task1 = new Task(() => col.ForEach(col.Add));
            task1.Start();

            Task.WaitAll(new[] { task1 });

            Assert.That(col.Count, Is.EqualTo(200));
        }

        [Test]
        public void Read_2ConcurrentEnumerations_BothEnumerationsAreExecutedAtSameTime()
        {
            var array = new int[100];

            for (int i = 0; i < 100; i++)
            {
                array[i] = i;
            }
            var col = new ConcurrentObservableCollection<int>(array);

            int res1 = 1;
            var task1 = new Task(() => col.ForEach(c =>
            {
                Thread.Sleep(1);
                res1 = 1;
            }));
            var task2 = new Task(() => col.ForEach(c =>
            {
                Thread.Sleep(1);
                res1++;
            }));
            task1.Start();
            task2.Start();
            Task.WaitAll(new[] { task1, task2 });
            Assert.That(res1, Is.LessThan(99));
            Debug.Print("result: {0}", res1);
        }

        [Test]
        public void Read_CheckContainsWhileAdding()
        {
            var col = new ConcurrentObservableCollection<int>(new[] { 1, 2, 3, 4, 5 });
            var task1 = new Task(() =>
            {
                for (int i = 10; i < 1000; i++)
                {
                    col.Add(i);
                    Thread.Sleep(1);
                }
            });
            bool contains = false;
            var task2 = new Task(() => { contains = col.Contains(1); });
            task1.Start();
            Thread.Sleep(5);
            task2.Start();
            Task.WaitAll(new[] { task1, task2 });

            Assert.True(contains);
        }

        [Test]
        public void Read_CheckLengthAfterAdd_LengthIsUpdated()
        {
            var col = new ConcurrentObservableCollection<int>();
            col.Add(1);

            Assert.That(col.Count, Is.EqualTo(1));
        }

        [Test]
        public void Read_ExceptionDuringEnumeration_LockReleased()
        {
            var array = new int[100];
            for (int i = 0; i < 100; i++)
            {
                array[i] = i;
            }
            var col = new ConcurrentObservableCollection<int>(array);

            try
            {
                int x = 0;
                col.ForEach(c =>
                {
                    if (x++ > 50)
                    {
                        throw new Exception();
                    }
                });
            }
            catch (Exception)
            {
                Console.WriteLine("Exception was fired");
            }

            col.Add(3);

            Assert.That(col.Count, Is.EqualTo(101));
        }

        [Test]
        public void ToString_ProperlyPrinted()
        {
            var col = new ConcurrentObservableCollection<int>(new[] { 1, 2, 3 });
            Assert.AreEqual("1, 2, 3", col.ToString());
        }

        [Test]
        public void WriteAndRead_ConcurrentReadAndWrite_SuccessfullyWritesElementsToCollection()
        {
            var array = new int[50];

            for (int i = 0; i < 50; i++)
            {
                array[i] = i;
            }
            var col = new ConcurrentObservableCollection<int>(array);

            var task1 = new Task(() =>
            {
                for (int i = 50; i < 100; i++)
                {
                    col.Add(i);
                }
            });
            int current = 0;
            var task2 = new Task(() => col.ForEach(c => { current = c; }));
            task1.Start();
            task2.Start();
            Task.WaitAll(new[] { task1, task2 });

            Assert.That(col.Count, Is.EqualTo(100), "collection was not filled");
            Assert.That(current, Is.InRange(49, 100), "enumeration was not running simultaneously with update");
            Debug.Print("Collection size: {0}", col.Count);
            Debug.Print("current: {0}", current);
        }

        [Test]
        public void Write_AddElement_ElementAdded()
        {
            var col = new ConcurrentObservableCollection<string>();
            col.Add("a");
            Assert.That(col.First(), Is.EqualTo("a"));
        }



        [Test]
        public void Write_AddNull_ElementAdded()
        {
            var col = new ConcurrentObservableCollection<string>();
            var expected = new[] { "a", null };
            col.AddRange(expected);
            CollectionAssert.AreEquivalent(expected, col);
        }

        [Test]
        public void Write_AddRange_ElementsAdded()
        {
            var col = new ConcurrentObservableCollection<string>();
            var expected = new[] { "a", "b" };
            col.AddRange(expected);
            CollectionAssert.AreEquivalent(expected, col);
        }

        [Test]
        public void Write_ComplexOperation_CollectionUpdatedProperly()
        {
            var col = new ConcurrentObservableCollection<string>(new[] { "a", "b", "c" });

            col.Add("d");
            col.Remove("b");
            col.Insert(0, "x");
            col.AddRange(new[] { "z", "f", "y" });
            col.RemoveAt(4);
            col.RemoveRange(new[] { "y", "c" });
            col[2] = "p";
            CollectionAssert.AreEquivalent(new[] { "x", "a", "p", "f" }, col);
        }


        [Test]
        public void AddRange_5SequentialAdds_CollectionChangeEventsAreReported()
        {
            var col = new ConcurrentObservableCollection<string>(new[] { "a" });

            int eventsNumber = 0;
            col.CollectionChanged += (sender, args) => eventsNumber++;
            col.AddRange(new[] { "z1", "f1", "y1" });
            col.AddRange(new[] { "z2", "f2", "y2" });
            col.AddRange(new[] { "z3", "f3", "y3" });
            col.AddRange(new[] { "z4", "f4", "y4" });
            col.AddRange(new[] { "z5", "f5", "y5" });

            Assert.That(eventsNumber, Is.EqualTo(5));
            CollectionAssert.AreEquivalent(new[] { "a", "z1", "f1", "y1", "z2", "f2", "y2", "z3", "f3", "y3", "z4", "f4", "y4", "z5", "f5", "y5" }, col);
        }

        [Test]
        public void Write_ComplexUpdateOperationFrom2ThreadsAndEnumerationInTheMiddle_CollectionUpdatedProperly()
        {
            var array = new int[100];
            for (int i = 0; i < 100; i++)
            {
                array[i] = i;
            }
            var col = new ConcurrentObservableCollection<int>(array);

            var task1 = new Task(() =>
            {
                for (int i = 100; i < 200; i++)
                {
                    Console.WriteLine("Add {0}", i);
                    col.Add(i);
                }
            });
            var task2 = new Task(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine("Remove {0}", i);
                    col.Remove(i);
                }
            });

            var list = new List<int>();
            var task3 = new Task(() => col.ForEach(c =>
            {
                Console.WriteLine("Enumerating {0}", c);
                list.Add(c);
            }));
            task1.Start();
            task2.Start();
            task3.Start();

            Task.WaitAll(new[] { task1, task2, task3 });

            var expected = new int[100];
            for (int i = 100; i < 200; i++)
            {
                expected[i - 100] = i;
            }
            CollectionAssert.AreEquivalent(expected, col, "collection was not properly updated");
            CollectionAssert.IsNotEmpty(list, "Enumeration didnt find any element");
        }

        [Test]
        public void Write_ComplexUpdateOperationFrom2Threads_CollectionUpdatedProperly()
        {
            var array = new int[100];
            for (int i = 0; i < 100; i++)
            {
                array[i] = i;
            }
            var col = new ConcurrentObservableCollection<int>(array);

            var task1 = new Task(() =>
            {
                for (int i = 100; i < 200; i++)
                {
                    col.Add(i);
                }
            });
            var task2 = new Task(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    col.Remove(i);
                }
            });
            task1.Start();
            task2.Start();
            Task.WaitAll(new[] { task1, task2 });

            var expected = new int[100];
            for (int i = 100; i < 200; i++)
            {
                expected[i - 100] = i;
            }
            CollectionAssert.AreEquivalent(expected, col);
        }

        [Test]
        public void Write_ConcurrentWrite_SuccessfullyWritesElementsToCollection()
        {
            var col = new ConcurrentObservableCollection<int>();

            var task1 = new Task(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    col.Add(i);
                    Thread.Sleep(1);
                }
            });
            var task2 = new Task(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    col.Clear();
                    Thread.Sleep(1);
                }
            });
            task1.Start();
            task2.Start();
            Task.WaitAll(new[] { task1, task2 });
            Assert.That(col.Count, Is.LessThan(1000));
            Debug.Print("Collection size: {0}", col.Count);
        }

        [Test]
        public void Write_FiresAddEvent()
        {
            var col = new ConcurrentObservableCollection<string>();
            string received = string.Empty;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    received = args.NewItems.OfType<string>().First();
                }
            };
            col.Add("a");
            Assert.That(received, Is.EqualTo("a"));
        }


        [Test]
        public void AddRange_FiresAddEvent()
        {
            var col = new ConcurrentObservableCollection<string>();
            string received = string.Empty;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    received = args.NewItems.OfType<string>().First();
                }
            };
            col.AddRange(new[] { "a", "b", "c" });
            Assert.That(received, Is.EqualTo("a"));
        }


        [Test]
        public void RemoveRange_FiresRemoveEvent()
        {
            var col = new ConcurrentObservableCollection<string>(new[] { "a", "b", "c" });
            string received = string.Empty;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    received = args.OldItems.OfType<string>().First();
                }
            };
            col.RemoveRange(new[] { "a", "b", "c" });
            Assert.That(received, Is.EqualTo("a"));
        }

        [Test]
        public void Write_FiresRemoveEvent()
        {
            var col = new ConcurrentObservableCollection<string>(new[] { "b", "c" });
            string received = string.Empty;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    received = args.OldItems.OfType<string>().First();
                }
            };
            col.Remove("c");

            Assert.That(received, Is.EqualTo("c"));
        }

        [Test]
        public void Write_FiresResetEvent()
        {
            var col = new ConcurrentObservableCollection<string>(new[] { "b", "c" });
            bool fired = false;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Reset)
                {
                    fired = true;
                }
            };
            col.Clear();

            Assert.True(fired);
        }

        [Test]
        public void Write_InsertElement_ElementInserted()
        {
            var col = new ConcurrentObservableCollection<string>(new[] { "a", "b", "c" });
            col.Insert(1, "x");
            CollectionAssert.AreEqual(col, new[] { "a", "x", "b", "c" });
        }

        [Test]
        public void Write_InsertElement_FiresAddEvent()
        {
            var col = new ConcurrentObservableCollection<string>(new[] { "a", "b", "c" });
            NotifyCollectionChangedEventArgs receivedArgs = null;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    receivedArgs = args;
                }
            };
            col.Insert(1, "x");
            Assert.That(receivedArgs.NewStartingIndex, Is.EqualTo(1), "Index is wrong");
            CollectionAssert.AreEquivalent(receivedArgs.NewItems, new[] { "x" }, "New items collection wrong");
        }

        [Test]
        public void Write_RemoveElementAtIndex_ElementRemoved()
        {
            var col = new ConcurrentObservableCollection<string>(new[] { "a", "b", "c" });
            col.RemoveAt(1);
            CollectionAssert.AreEqual(new[] { "a", "c" }, col);
        }

        [Test]
        public void Write_RemoveElement_ElementRemoved()
        {
            var col = new ConcurrentObservableCollection<string>(new[] { "b", "c", "d" });
            col.Remove("b");
            CollectionAssert.AreEquivalent(col, new[] { "c", "d" });
        }


        [Test]
        public void Write_RemoveNotExisting_DoesntFail()
        {
            var col = new ConcurrentObservableCollection<string>(new[] { "b", "c", "d" });

            col.RemoveRange(new[] { "b", "X" });
            CollectionAssert.AreEquivalent(new[] { "c", "d" }, col);
        }

        [Test]
        public void Write_RemoveRange_ElementsRemoved()
        {
            var col = new ConcurrentObservableCollection<string>(new[] { "b", "c", "d" });

            col.RemoveRange(new[] { "b", "c" });
            CollectionAssert.AreEquivalent(new[] { "d" }, col);
        }

        [Test]
        public void Write_ReplaceElement_ElementReplaced()
        {
            var col = new ConcurrentObservableCollection<string>(new[] { "a", "b", "c" });
            col[2] = "z";
            CollectionAssert.AreEqual(new[] { "a", "b", "z" }, col);
        }

        [Test]
        public void Write_ReplaceElement_FiresReplaceEvent()
        {
            var col = new ConcurrentObservableCollection<string>(new[] { "a", "b", "c" });
            NotifyCollectionChangedEventArgs receivedArgs = null;
            col.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Replace)
                {
                    receivedArgs = args;
                }
            };
            col[2] = "z";
            Assert.That(receivedArgs.NewStartingIndex, Is.EqualTo(2), "Index is wrong");
            CollectionAssert.AreEquivalent(receivedArgs.NewItems, new[] { "z" }, "New items collection wrong");
            CollectionAssert.AreEquivalent(receivedArgs.OldItems, new[] { "c" }, "Old items collection wrong");
        }

        [Test]
        public void RemoveRange_AcquireRangeToRemoveUsingLinq_RangeRemovedWithoutExceptions()
        {

            var col = new ConcurrentObservableCollection<string>(new[] { "a", "b", "c" });

            var select = col.Where(c => c.Equals("c"));

            col.RemoveRange(select);

            CollectionAssert.AreEquivalent(col, new[] { "a", "b" });

        }
    }
}
