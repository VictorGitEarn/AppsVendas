using AspNetCore.Identity.Mongo.Model;

namespace Apps.Domain.Business
{
    public class User : MongoUser
    {
        public string Nome { get; set; }

        public string SocialSecurity { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public Address Address { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }

        public int Number { get; set; }

        public string Neighborhood { get; set; }

        public string Postcode { get; set; }

        public string City { get; set; }

        public string UF { get; set; }
    }
}
