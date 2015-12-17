namespace IssueDemos
{
    using System;
    using System.Reflection;
    using Xamarin.Forms;

    public class DemoInfo
    {
        private readonly DemoPageAttribute dpa;
        private readonly Type type;

        public DemoInfo(Type type)
        {
            this.type = type;
            this.dpa = type.GetTypeInfo().GetCustomAttribute<DemoPageAttribute>();
        }

        public Page CreatePage()
        {
            return (Page)Activator.CreateInstance(this.type);
        }

		public string Title { get { return this.dpa.Title; } }

		public string Details { get { return this.dpa.Details; } }
    }
}