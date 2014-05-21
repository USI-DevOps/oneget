﻿// 
//  Copyright (c) Microsoft Corporation. All rights reserved. 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//  

namespace Microsoft.OneGet.Test {
    using System;
    using Core.DuckTyping;
    using Xunit;

    public class RequiredAttribute : Attribute {
    }

    public interface IDynTest {
        [Required]
        void One();

        [Required]
        bool Two();

        string Three();

        string Four(int a);

        string Five(int a, string b);
    }

    public class DynInst {
        public void One() {
            Console.WriteLine("In One");
        }

        public bool Two() {
            Console.WriteLine("In Two");
            return true;
        }

        public string Three() {
            Console.WriteLine("In three");
            return "Three";
        }

        public string Four(int a) {
            Console.WriteLine("Four {0}", a);
            return "Four" + a;
        }

        public string Five(int a, string b) {
            Console.WriteLine("Five {0} {1}", a, b);
            return "Four" + a + b;
        }
    }

    public class DynamicInterfaceTest {
        [Fact]
        public void TestDynamicInterfaceAgainstClass() {
            var di = new DynamicInterface();

            var idyn = di.Create<IDynTest>(typeof (DynInst));

            idyn.One();
            Assert.True(idyn.Two());
            Assert.Equal("Three", idyn.Three());
            Assert.Equal("Four4", idyn.Four(4));
            Assert.Equal("Four5hi", idyn.Five(5, "hi"));
        }

        [Fact]
        public void TestDynamicInterfaceAgainstAnonymousObject() {
            var di = new DynamicInterface();

            var idyn2 = di.Create<IDynTest>(new {
                One = new Action(() => {}),
                Two = new Func<bool>(() => {return true;})
            });

            idyn2.One();

            Assert.True(idyn2.Two());
            Assert.Equal(null, idyn2.Four(4));
        }

        [Fact]
        public void TestDynamicInterfaceRequired() {
            var di = new DynamicInterface();

            Assert.Throws<Exception>(() => {
                var idyn3 = di.Create<IDynTest>(new {
                    One = new Action(() => {})
                });
            });
        }
    }
}