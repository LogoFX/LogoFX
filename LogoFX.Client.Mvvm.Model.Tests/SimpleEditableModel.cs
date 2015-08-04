namespace LogoFX.Client.Mvvm.Model.Tests
{
    class SimpleEditableModel : EditableModel
    {
        public SimpleEditableModel(string name, int age)
        {
            Name = name;
            Age = age;
        }

        [NameValidation]
        public new string Name { get; set; }
        public int Age { get; set; }
    }
}