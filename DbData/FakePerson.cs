namespace CampSignUpProject.Media
{

    public class FakePerson
    {
        public string status { get; set; }
        public int code { get; set; }
        public int total { get; set; }
        public Person[] data { get; set; }
    }

    public class Person
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string birthday { get; set; }
        public string gender { get; set; }
        public Address address { get; set; }
        public string website { get; set; }
        public string image { get; set; }
    }

    public class Address
    {
        public int id { get; set; }
        public string street { get; set; }
        public string streetName { get; set; }
        public string buildingNumber { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public string country { get; set; }
        public string county_code { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
    }

}
