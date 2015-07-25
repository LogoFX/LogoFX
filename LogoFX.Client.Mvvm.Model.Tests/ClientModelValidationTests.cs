﻿using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class ClientModelValidationTests
    {
        [Test]
        public void ValueObjectIsValid_ErrorIsNull()
        {
            var valueObject = new SimpleTestValueObject("valid name", 5);

            var error = valueObject.Error;
            Assert.IsNullOrEmpty(error);
        }

        [Test]
        public void ValueObjectIsInValid_ErrorIsNotNull()
        {
            var valueObject = new SimpleTestValueObject("in$valid name", 5);

            var error = valueObject.Error;
            Assert.IsNotNullOrEmpty(error);
        }
    }

    class SimpleTestValueObject : ValueObject
    {        
        public SimpleTestValueObject(string name, int age)
        {
            Name = name;
            Age = age;
        }

        [NameValidation]
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class NameValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var str = value as string;
            return str != null && str.Contains("$") == false;
        }
    }
}
