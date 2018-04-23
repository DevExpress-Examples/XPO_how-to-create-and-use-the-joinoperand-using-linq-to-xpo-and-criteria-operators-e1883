using DevExpress.Xpo;
namespace DXSample {
    public class Person :XPObject {
        public Person(Session session) : base(session) { }

        private string name;
        public string Name {
            get { return name; }
            set { SetPropertyValue<string>("Name", ref name, value); }
        }

        private int age;
        public int Age {
            get { return age; }
            set { SetPropertyValue<int>("Age", ref age, value); }
        }
    }
}